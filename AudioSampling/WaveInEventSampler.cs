using System;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using AudioSamplingComparison.Models;

namespace AudioSamplingComparison.AudioSampling
{
    public class WaveInEventSampler : BaseAudioSampler
    {
        public override string MethodName => "WaveInEvent";
        public override string MethodDescription => "Uses NAudio's WaveInEvent API to capture audio samples at regular intervals. This method uses Windows waveIn APIs through NAudio and triggers events when new audio data is available.";
        
        private bool isRecording = false;
        
        public override async Task<AudioData> CaptureAudioAsync(int durationSeconds)
        {
            if (isCapturing)
            {
                StopCapture();
            }
            
            isCapturing = true;
            cancellationTokenSource = new CancellationTokenSource();
            
            // Calculate buffer size based on the sampling interval
            int bufferSize = (SampleRate * BitsPerSample * Channels / 8) * SamplingIntervalMs / 1000;
            
            // Calculate number of samples/buffers we need based on duration
            int totalBuffers = (1000 * durationSeconds) / SamplingIntervalMs;
            
            // Initialize audio data object
            AudioData audioData = new AudioData(SampleRate, BitsPerSample, Channels, totalBuffers, SamplingIntervalMs, MethodName);
            capturedAudio = audioData.AudioBuffer;
            currentBufferIndex = 0;
            
            try
            {
                // Create WaveInEvent for capturing
                using (var waveInEvent = new WaveInEvent())
                {
                    var tcs = new TaskCompletionSource<bool>();
                    
                    waveInEvent.DeviceNumber = 0; // Default recording device
                    waveInEvent.WaveFormat = new WaveFormat(SampleRate, BitsPerSample, Channels);
                    waveInEvent.BufferMilliseconds = SamplingIntervalMs;
                    
                    waveInEvent.DataAvailable += (sender, e) =>
                    {
                        if (cancellationTokenSource.Token.IsCancellationRequested || currentBufferIndex >= totalBuffers)
                        {
                            waveInEvent.StopRecording();
                            tcs.TrySetResult(true);
                            return;
                        }
                        
                        // Copy the buffer to our storage array
                        byte[] buffer = new byte[e.BytesRecorded];
                        Array.Copy(e.Buffer, 0, buffer, 0, e.BytesRecorded);
                        capturedAudio[currentBufferIndex] = buffer;
                        
                        // Increment buffer index
                        currentBufferIndex++;
                        
                        // If we've reached the duration, stop recording
                        if (currentBufferIndex >= totalBuffers)
                        {
                            waveInEvent.StopRecording();
                            tcs.TrySetResult(true);
                        }
                    };
                    
                    waveInEvent.RecordingStopped += (sender, e) =>
                    {
                        isRecording = false;
                        tcs.TrySetResult(true);
                    };
                    
                    // Start recording
                    isRecording = true;
                    waveInEvent.StartRecording();
                    
                    // Set up a timeout in case recording doesn't stop on its own
                    var timeoutTask = Task.Delay(durationSeconds * 1000 + 1000, cancellationTokenSource.Token);
                    
                    // Wait for recording to complete or timeout
                    await Task.WhenAny(tcs.Task, timeoutTask);
                    
                    // Stop recording if it hasn't already
                    if (isRecording)
                    {
                        waveInEvent.StopRecording();
                    }
                }
                
                // Finalize the audio data
                audioData.TotalSamples = currentBufferIndex;
                audioData.SignalToNoiseRatio = CalculateSignalToNoiseRatio(audioData);
                audioData.AverageBitRate = (double)(SampleRate * BitsPerSample * Channels);
                
                return audioData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in {MethodName}: {ex.Message}");
                throw;
            }
            finally
            {
                isCapturing = false;
                isRecording = false;
            }
        }
    }
}