using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioSamplingComparison.AudioSampling;
using AudioSamplingComparison.Models;

namespace AudioSamplingComparison
{
    public partial class Form1 : Form
    {
        private List<IAudioSampler> samplers;
        private IAudioSampler currentSampler;
        private Dictionary<string, AudioData> capturedAudioData;
        private bool isCapturing = false;
        private bool isPlaying = false;

        public Form1()
        {
            InitializeComponent();
            
            // Initialize sampler instances
            samplers = new List<IAudioSampler>
            {
                new WaveInEventSampler(),
                new WasapiSampler(),
                // ASIO may not be available on all systems, so we'll add it conditionally
            };
            
            // Initialize with the first sampler to avoid nullability warning
            currentSampler = samplers[0];
            
            try
            {
                // Check if ASIO is available
                string[] driverNames = NAudio.Wave.AsioOut.GetDriverNames();
                if (driverNames.Length > 0)
                {
                    samplers.Add(new AsioSampler());
                }
            }
            catch (Exception)
            {
                // ASIO is not available, so we won't add it
            }
            
            capturedAudioData = new Dictionary<string, AudioData>();
            
            // Set up the UI controls after form load
            this.Load += Form1_Load;
        }
        
        private void Form1_Load(object? sender, EventArgs e)
        {
            SetupUI();
        }
        
        private void SetupUI()
        {
            // Set up the sampler combo box
            cmbSamplingMethod.Items.Clear();
            foreach (var sampler in samplers)
            {
                cmbSamplingMethod.Items.Add(sampler.MethodName);
            }
            
            if (cmbSamplingMethod.Items.Count > 0)
            {
                cmbSamplingMethod.SelectedIndex = 0;
                currentSampler = samplers[0];
            }
            
            // Set up numeric controls with default values
            numSampleRate.Value = currentSampler.SampleRate;
            numBitsPerSample.Value = currentSampler.BitsPerSample;
            numChannels.Value = currentSampler.Channels;
            numSamplingInterval.Value = currentSampler.SamplingIntervalMs;
            
            // Disable play button initially
            btnPlay.Enabled = false;
            
            // Register events
            cmbSamplingMethod.SelectedIndexChanged += CmbSamplingMethod_SelectedIndexChanged;
            btnCapture.Click += BtnCapture_Click;
            btnStop.Click += BtnStop_Click;
            btnPlay.Click += BtnPlay_Click;
            
            // Results listview setup
            lvResults.View = View.Details;
            lvResults.FullRowSelect = true;
            lvResults.Columns.Add("Method", 150);
            lvResults.Columns.Add("Sample Rate", 100);
            lvResults.Columns.Add("Bits", 50);
            lvResults.Columns.Add("Channels", 70);
            lvResults.Columns.Add("Interval (ms)", 90);
            lvResults.Columns.Add("Samples", 80);
            lvResults.Columns.Add("Size (KB)", 80);
            lvResults.Columns.Add("SNR (dB)", 80);
            lvResults.Columns.Add("Bit Rate", 80);
            
            // Results list click event for playback selection
            lvResults.ItemSelectionChanged += LvResults_ItemSelectionChanged;
        }
        
        private void LvResults_ItemSelectionChanged(object? sender, ListViewItemSelectionChangedEventArgs e)
        {
            btnPlay.Enabled = e.IsSelected && e.Item != null && capturedAudioData.ContainsKey(e.Item.Text);
        }
        
        private void CmbSamplingMethod_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbSamplingMethod.SelectedIndex >= 0)
            {
                currentSampler = samplers[cmbSamplingMethod.SelectedIndex];
                
                // Update numeric controls to match the selected sampler
                numSampleRate.Value = currentSampler.SampleRate;
                numBitsPerSample.Value = currentSampler.BitsPerSample;
                numChannels.Value = currentSampler.Channels;
                numSamplingInterval.Value = currentSampler.SamplingIntervalMs;
                
                // Show method description
                txtMethodDescription.Text = currentSampler.MethodDescription;
            }
        }
        
        private async void BtnCapture_Click(object? sender, EventArgs e)
        {
            if (isCapturing)
                return;
            
            isCapturing = true;
            
            // Update UI state
            btnCapture.Enabled = false;
            btnStop.Enabled = true;
            cmbSamplingMethod.Enabled = false;
            
            // Apply current parameters to the sampler
            currentSampler.SampleRate = (int)numSampleRate.Value;
            currentSampler.BitsPerSample = (int)numBitsPerSample.Value;
            currentSampler.Channels = (int)numChannels.Value;
            currentSampler.SamplingIntervalMs = (int)numSamplingInterval.Value;
            
            // Start progress indicator
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.Value = 0;
            
            try
            {
                // Capture audio for 60 seconds (1 minute)
                var result = await currentSampler.CaptureAudioAsync(60);
                
                // Store captured data
                if (capturedAudioData.ContainsKey(result.SamplingMethod))
                {
                    capturedAudioData[result.SamplingMethod] = result;
                }
                else
                {
                    capturedAudioData.Add(result.SamplingMethod, result);
                }
                
                // Update results list
                UpdateResultsList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error capturing audio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Reset UI state
                isCapturing = false;
                btnCapture.Enabled = true;
                btnStop.Enabled = false;
                cmbSamplingMethod.Enabled = true;
                progressBar.Style = ProgressBarStyle.Blocks;
            }
        }
        
        private void BtnStop_Click(object? sender, EventArgs e)
        {
            if (isCapturing)
            {
                currentSampler.StopCapture();
                isCapturing = false;
                
                // Reset UI state
                btnCapture.Enabled = true;
                btnStop.Enabled = false;
                cmbSamplingMethod.Enabled = true;
                progressBar.Style = ProgressBarStyle.Blocks;
            }
            else if (isPlaying)
            {
                currentSampler.StopPlayback();
                isPlaying = false;
                
                // Reset UI state
                btnPlay.Text = "Play";
            }
        }
        
        private void BtnPlay_Click(object? sender, EventArgs e)
        {
            if (isPlaying)
            {
                currentSampler.StopPlayback();
                isPlaying = false;
                btnPlay.Text = "Play";
                return;
            }
            
            if (lvResults.SelectedItems.Count > 0)
            {
                string method = lvResults.SelectedItems[0].Text;
                
                if (capturedAudioData.ContainsKey(method))
                {
                    isPlaying = true;
                    btnPlay.Text = "Stop";
                    
                    // Play the selected audio data
                    Task.Run(() =>
                    {
                        try
                        {
                            currentSampler.PlayAudio(capturedAudioData[method]);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error playing audio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                isPlaying = false;
                                btnPlay.Text = "Play";
                            });
                        }
                    });
                }
            }
        }
        
        private void UpdateResultsList()
        {
            lvResults.Items.Clear();
            
            foreach (var entry in capturedAudioData)
            {
                var audioData = entry.Value;
                
                ListViewItem item = new ListViewItem(audioData.SamplingMethod);
                item.SubItems.Add(audioData.SampleRate.ToString());
                item.SubItems.Add(audioData.BitsPerSample.ToString());
                item.SubItems.Add(audioData.Channels.ToString());
                item.SubItems.Add(audioData.SamplingIntervalMs.ToString());
                item.SubItems.Add(audioData.TotalSamples.ToString());
                item.SubItems.Add((audioData.GetTotalSizeInBytes() / 1024).ToString("F2"));
                item.SubItems.Add(audioData.SignalToNoiseRatio.ToString("F2"));
                item.SubItems.Add((audioData.AverageBitRate / 1000).ToString("F2") + " kbps");
                
                lvResults.Items.Add(item);
            }
            
            // Auto-size columns
            for (int i = 0; i < lvResults.Columns.Count; i++)
            {
                lvResults.Columns[i].Width = -2;
            }
        }
    }
}
