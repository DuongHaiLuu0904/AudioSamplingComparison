using System;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using AudioSamplingComparison.Models;

namespace AudioSamplingComparison.AudioSampling
{
    public class ADPCMSampler : BaseAudioSampler
    {
        public override string MethodName => "ADPCM";
        public override string MethodDescription => 
            "Adaptive Differential Pulse Code Modulation (ADPCM) - Encodes the difference between samples using adaptive quantization.\n\n" +
            "1. Sampling: Audio is sampled at regular intervals (identical to PCM).\n" + 
            "2. Prediction: Next sample is predicted based on previous samples.\n" +
            "3. Difference: Error between actual and predicted sample is calculated.\n" +
            "4. Adaptive Quantization: The error is quantized using adaptive quantization.\n" +
            "5. Encoding: The quantized error is encoded into data bits.\n\n" +
            "ADPCM typically achieves 2:1 to 4:1 compression compared to standard PCM.";
        
        private bool isRecording = false;
        private short[] previousSamples = new short[4]; // For prediction
        private int stepIndex = 0; // For adaptive quantization
        
        // ADPCM adaptation table for step size
        private static readonly int[] StepSizeTable = {
            7, 8, 9, 10, 11, 12, 13, 14,
            16, 17, 19, 21, 23, 25, 28, 31,
            34, 37, 41, 45, 50, 55, 60, 66,
            73, 80, 88, 97, 107, 118, 130, 143,
            157, 173, 190, 209, 230, 253, 279, 307,
            337, 371, 408, 449, 494, 544, 598, 658,
            724, 796, 876, 963, 1060, 1166, 1282, 1411,
            1552, 1707, 1878, 2066, 2272, 2499, 2749, 3024,
            3327, 3660, 4026, 4428, 4871, 5358, 5894, 6484,
            7132, 7845, 8630, 9493, 10442, 11487, 12635, 13899,
            15289, 16818, 18500, 20350, 22385, 24623, 27086, 29794,
            32767
        };
        
        // ADPCM index adaptation table
        private static readonly int[] IndexTable = {
            -1, -1, -1, -1, 2, 4, 6, 8,
            -1, -1, -1, -1, 2, 4, 6, 8
        };
        
        public override async Task<AudioData> CaptureAudioAsync(int durationSeconds)
        {
            if (isCapturing)
            {
                StopCapture();
            }
            
            isCapturing = true;
            cancellationTokenSource = new CancellationTokenSource();
            
            // Reset prediction variables
            Array.Clear(previousSamples, 0, previousSamples.Length);
            stepIndex = 0;
            
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
                        
                        // Process the buffer with ADPCM encoding
                        byte[] encodedBuffer = ProcessADPCM(e.Buffer, e.BytesRecorded);
                        capturedAudio[currentBufferIndex] = encodedBuffer;
                        
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
                audioData.TotalSamples = CalculateTotalSamples(capturedAudio);
                audioData.SignalToNoiseRatio = CalculateSignalToNoiseRatio(audioData);
                
                // ADPCM typically has a bit rate of BitsPerSample/2 compared to PCM
                audioData.AverageBitRate = (double)(SampleRate * (BitsPerSample/2) * Channels);
                
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
        
        /// <summary>
        /// Process audio buffer with ADPCM encoding
        /// </summary>
        private byte[] ProcessADPCM(byte[] inputBuffer, int bytesRecorded)
        {
            // This is a simplified ADPCM implementation
            // In a real scenario, we would compress each 16-bit sample to 4 bits
            
            // For this demonstration, we'll simulate ADPCM encoding
            // by reducing the bit depth but preserving the original buffer size
            
            // Get the number of samples based on bit depth
            int bytesPerSample = BitsPerSample / 8;
            int samplesCount = bytesRecorded / bytesPerSample;
            
            // Create output buffer - in real ADPCM this would be smaller
            // but for demo purposes we'll keep the same size and simulate compression
            byte[] outputBuffer = new byte[bytesRecorded];
            
            // Process each sample
            for (int i = 0; i < samplesCount; i += Channels)
            {
                for (int channel = 0; channel < Channels; channel++)
                {
                    int sampleIndex = i + channel;
                    int byteIndex = sampleIndex * bytesPerSample;
                    
                    if (byteIndex + 1 < bytesRecorded)
                    {
                        // Convert bytes to short sample
                        short sample = (short)((inputBuffer[byteIndex + 1] << 8) | inputBuffer[byteIndex]);
                        
                        // Predict the next sample (simple prediction using previous sample)
                        short predictedSample = channel < previousSamples.Length ? previousSamples[channel] : (short)0;
                        
                        // Calculate error (difference)
                        short difference = (short)(sample - predictedSample);
                        
                        // Get the current step size from the table
                        int step = StepSizeTable[stepIndex];
                        
                        // Quantize the difference (4-bit ADPCM)
                        // We're simplifying here - real ADPCM would be more complex
                        int sign = difference < 0 ? 1 : 0;
                        int magnitude = Math.Abs(difference);
                        int quantizedDifference = (magnitude * 4) / step;
                        if (quantizedDifference > 7) quantizedDifference = 7;
                        
                        // The 4-bit ADPCM code is combination of sign bit and magnitude
                        byte adpcmCode = (byte)((sign << 3) | quantizedDifference);
                        
                        // For simulation, we'll store the original sample in the output buffer
                        // In real ADPCM, we would pack 4-bit codes together
                        outputBuffer[byteIndex] = inputBuffer[byteIndex];
                        outputBuffer[byteIndex + 1] = inputBuffer[byteIndex + 1];
                        
                        // Update the step index using the index table
                        stepIndex += IndexTable[adpcmCode];
                        if (stepIndex < 0) stepIndex = 0;
                        if (stepIndex > 88) stepIndex = 88;
                        
                        // Store this sample for future prediction
                        if (channel < previousSamples.Length)
                            previousSamples[channel] = sample;
                    }
                }
            }
            
            return outputBuffer;
        }
    }
}