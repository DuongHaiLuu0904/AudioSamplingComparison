using System;
using System.Collections.Generic;

namespace AudioSamplingComparison.Models
{
    public class AudioData
    {
        // Audio format information
        public int SampleRate { get; set; }
        public int BitsPerSample { get; set; }
        public int Channels { get; set; }
        
        // Raw audio data (buffer)
        public byte[][] AudioBuffer { get; set; }
        
        // Metadata
        public string SamplingMethod { get; set; }
        public DateTime RecordedTime { get; set; }
        public int TotalSamples { get; set; }
        public int SamplingIntervalMs { get; set; }
        
        // Statistics for quality comparison
        public double SignalToNoiseRatio { get; set; }
        public double AverageBitRate { get; set; }
        
        public AudioData(int sampleRate, int bitsPerSample, int channels, int bufferSize, int samplingIntervalMs, string method)
        {
            SampleRate = sampleRate;
            BitsPerSample = bitsPerSample;
            Channels = channels;
            AudioBuffer = new byte[bufferSize][];
            SamplingMethod = method;
            RecordedTime = DateTime.Now;
            SamplingIntervalMs = samplingIntervalMs;
        }
        
        // Calculate size in bytes
        public long GetTotalSizeInBytes()
        {
            long total = 0;
            if (AudioBuffer != null)
            {
                foreach (var buffer in AudioBuffer)
                {
                    if (buffer != null)
                    {
                        total += buffer.Length;
                    }
                }
            }
            return total;
        }
    }
}