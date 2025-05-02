using System;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using AudioSamplingComparison.Models;

namespace AudioSamplingComparison.AudioSampling
{
    public abstract class BaseAudioSampler : IAudioSampler, IDisposable
    {
        // Common properties
        public abstract string MethodName { get; }
        public abstract string MethodDescription { get; }
        
        public int SampleRate { get; set; } = 44100;
        public int BitsPerSample { get; set; } = 16;
        public int Channels { get; set; } = 2;
        public int SamplingIntervalMs { get; set; } = 10; // Default 10ms
        
        // NAudio components - making these nullable to fix warnings
        protected WaveIn? waveIn;
        protected WaveOut? waveOut;
        protected BufferedWaveProvider? bufferedWaveProvider;
        
        // Capture state
        protected bool isCapturing = false;
        protected bool isPlaying = false;
        protected CancellationTokenSource? cancellationTokenSource;
        
        // Captured audio data
        protected byte[][]? capturedAudio;
        protected int currentBufferIndex = 0;
        
        // Base methods
        public abstract Task<AudioData> CaptureAudioAsync(int durationSeconds);
        
        public virtual AudioData? GetCurrentAudioData()
        {
            // Nếu không có dữ liệu thu âm, trả về null
            if (capturedAudio == null || currentBufferIndex == 0)
                return null;
                
            // Tạo đối tượng AudioData mới với dữ liệu hiện tại
            var result = new AudioData(
                SampleRate,
                BitsPerSample,
                Channels,
                currentBufferIndex, // Số lượng buffer đã thu được
                SamplingIntervalMs,
                MethodName
            );
            
            // Đặt thời gian ghi âm là thời điểm hiện tại để đảm bảo tính duy nhất
            result.RecordedTime = DateTime.Now;
            
            // Sao chép dữ liệu từ capturedAudio sang audioBuffer
            for (int i = 0; i < currentBufferIndex; i++)
            {
                if (capturedAudio[i] != null)
                {
                    result.AudioBuffer[i] = new byte[capturedAudio[i].Length];
                    Array.Copy(capturedAudio[i], result.AudioBuffer[i], capturedAudio[i].Length);
                }
            }
            
            // Tính toán thông số
            result.TotalSamples = CalculateTotalSamples(result.AudioBuffer);
            result.AverageBitRate = CalculateAverageBitRate(result.AudioBuffer);
            
            // Tính toán SNR
            result.SignalToNoiseRatio = CalculateSignalToNoiseRatio(result);
            
            return result;
        }
        
        // Helper method to calculate total samples
        protected int CalculateTotalSamples(byte[][] buffer)
        {
            int totalSamples = 0;
            foreach (var b in buffer)
            {
                if (b != null)
                {
                    totalSamples += b.Length / (BitsPerSample / 8);
                }
            }
            return totalSamples;
        }
        
        // Helper method to calculate average bit rate
        protected double CalculateAverageBitRate(byte[][] buffer)
        {
            long totalBytes = 0;
            foreach (var b in buffer)
            {
                if (b != null)
                {
                    totalBytes += b.Length;
                }
            }
            
            // Calculate bit rate in bits per second
            double durationSeconds = buffer.Length * SamplingIntervalMs / 1000.0;
            return totalBytes * 8 / durationSeconds;
        }
        
        public virtual void PlayAudio(AudioData audioData)
        {
            if (isPlaying)
            {
                StopPlayback();
            }
            
            isPlaying = true;
            
            try
            {
                bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(audioData.SampleRate, audioData.BitsPerSample, audioData.Channels));
                waveOut = new WaveOut();
                waveOut.Init(bufferedWaveProvider);
                waveOut.Play();
                
                // Play the audio data from memory buffer
                for (int i = 0; i < audioData.AudioBuffer.Length; i++)
                {
                    if (audioData.AudioBuffer[i] != null)
                    {
                        bufferedWaveProvider.AddSamples(audioData.AudioBuffer[i], 0, audioData.AudioBuffer[i].Length);
                        // Wait for the sampling interval to simulate the original capture timing
                        Thread.Sleep(audioData.SamplingIntervalMs);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing audio: {ex.Message}");
                StopPlayback();
            }
        }
        
        public virtual void StopPlayback()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            
            isPlaying = false;
        }
        
        public virtual void StopCapture()
        {
            if (isCapturing)
            {
                cancellationTokenSource?.Cancel();
                
                if (waveIn != null)
                {
                    waveIn.StopRecording();
                    waveIn.Dispose();
                    waveIn = null;
                }
                
                isCapturing = false;
            }
        }
        
        public virtual double CalculateSignalToNoiseRatio(AudioData audioData)
        {
            // Simple SNR calculation based on signal power vs. noise power estimation
            double signalPower = 0;
            double noisePower = 0;
            int totalSamples = 0;
            
            foreach (var buffer in audioData.AudioBuffer)
            {
                if (buffer != null)
                {
                    for (int i = 0; i < buffer.Length; i += 2)
                    {
                        if (i + 1 < buffer.Length)
                        {
                            short sample = (short)((buffer[i + 1] << 8) | buffer[i]);
                            double normalizedSample = sample / 32768.0; // Normalize to -1.0 to 1.0
                            
                            signalPower += normalizedSample * normalizedSample;
                            totalSamples++;
                        }
                    }
                }
            }
            
            signalPower /= totalSamples;
            noisePower = 0.0001; // Estimate of noise power (can be refined for better estimation)
            
            // Calculate SNR in dB
            double snrDb = 10 * Math.Log10(signalPower / noisePower);
            return snrDb;
        }
        
        public void Dispose()
        {
            StopCapture();
            StopPlayback();
        }
    }
}