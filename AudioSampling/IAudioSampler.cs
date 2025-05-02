using System;
using System.Threading.Tasks;
using AudioSamplingComparison.Models;

namespace AudioSamplingComparison.AudioSampling
{
    public interface IAudioSampler
    {
        // Method information
        string MethodName { get; }
        string MethodDescription { get; }
        
        // Sampling parameters
        int SampleRate { get; set; }
        int BitsPerSample { get; set; }
        int Channels { get; set; }
        int SamplingIntervalMs { get; set; }
        
        // Sampling operations
        Task<AudioData> CaptureAudioAsync(int durationSeconds);
        void PlayAudio(AudioData audioData);
        void StopPlayback();
        void StopCapture();
        
        // Lấy dữ liệu âm thanh hiện tại khi dừng thu âm
        AudioData? GetCurrentAudioData();
        
        // Quality measurement
        double CalculateSignalToNoiseRatio(AudioData audioData);
    }
}