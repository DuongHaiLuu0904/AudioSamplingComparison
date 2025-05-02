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
        // Thay đổi kiểu Dictionary để sử dụng string duy nhất làm khóa
        private Dictionary<string, AudioData> capturedAudioData;
        private bool isCapturing = false;
        private bool isPlaying = false;

        public Form1()
        {
            InitializeComponent();
            
            // Initialize sampler instances with our new PCM and ADPCM samplers
            samplers = new List<IAudioSampler>
            {
                new PCMSampler(),
                new ADPCMSampler()
            };
            
            // Initialize with the first sampler to avoid nullability warning
            currentSampler = samplers[0];
            
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
            // Set PCM as the default selection
            radioPCM.Checked = true;
            
            // Set up numeric controls with default values
            numSampleRate.Value = currentSampler.SampleRate;
            numBitsPerSample.Value = currentSampler.BitsPerSample;
            numChannels.Value = currentSampler.Channels;
            numSamplingInterval.Value = currentSampler.SamplingIntervalMs;
            
            // Populate method description
            txtMethodDescription.Text = currentSampler.MethodDescription;
            
            // Set the method icon indicator (optional, can be improved with actual icons)
            pictureMethod.BackColor = System.Drawing.Color.LightBlue;
            
            // Disable play button initially
            btnPlay.Enabled = false;
            
            // Register events
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
        
        private void RadioMethod_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Checked)
            {
                // Determine which sampler to use based on the selected radio button
                if (radioButton == radioPCM)
                {
                    currentSampler = samplers[0]; // PCM sampler
                    lblSelectedMethod.Text = "Selected Method: PCM";
                    pictureMethod.BackColor = System.Drawing.Color.LightBlue;
                }
                else if (radioButton == radioADPCM)
                {
                    currentSampler = samplers[1]; // ADPCM sampler
                    lblSelectedMethod.Text = "Selected Method: ADPCM";
                    pictureMethod.BackColor = System.Drawing.Color.LightGreen;
                }
                
                // Update method description
                txtMethodDescription.Text = currentSampler.MethodDescription;
                
                // Update numeric controls to match the selected sampler
                numSampleRate.Value = currentSampler.SampleRate;
                numBitsPerSample.Value = currentSampler.BitsPerSample;
                numChannels.Value = currentSampler.Channels;
                numSamplingInterval.Value = currentSampler.SamplingIntervalMs;
            }
        }
        
        private void LvResults_ItemSelectionChanged(object? sender, ListViewItemSelectionChangedEventArgs e)
        {
            string? selectedKey = e.Item?.Tag as string;
            btnPlay.Enabled = e.IsSelected && selectedKey != null && capturedAudioData.ContainsKey(selectedKey);
        }
        
        private async void BtnCapture_Click(object? sender, EventArgs e)
        {
            if (isCapturing)
                return;
            
            isCapturing = true;
            
            // Update UI state
            btnCapture.Enabled = false;
            btnStop.Enabled = true;
            radioPCM.Enabled = false;
            radioADPCM.Enabled = false;
            
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
                
                // Tạo khóa duy nhất cho kết quả thu âm
                string uniqueKey = GenerateUniqueKey(result);
                
                // Lưu dữ liệu đã thu được
                capturedAudioData[uniqueKey] = result;
                
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
                radioPCM.Enabled = true;
                radioADPCM.Enabled = true;
                progressBar.Style = ProgressBarStyle.Blocks;
            }
        }
        
        private void BtnStop_Click(object? sender, EventArgs e)
        {
            if (isCapturing)
            {
                // Lấy dữ liệu âm thanh hiện tại trước khi dừng thu âm
                var result = currentSampler.GetCurrentAudioData();
                
                // Dừng thu âm
                currentSampler.StopCapture();
                isCapturing = false;
                
                // Nếu có dữ liệu thu được, thêm vào danh sách kết quả
                if (result != null)
                {
                    // Tạo khóa duy nhất cho kết quả thu âm
                    string uniqueKey = GenerateUniqueKey(result);
                    
                    // Lưu dữ liệu đã thu được
                    capturedAudioData[uniqueKey] = result;
                    
                    // Cập nhật danh sách kết quả
                    UpdateResultsList();
                }
                
                // Reset UI state
                btnCapture.Enabled = true;
                btnStop.Enabled = false;
                radioPCM.Enabled = true;
                radioADPCM.Enabled = true;
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
        
        // Tạo khóa duy nhất kết hợp giữa phương thức thu âm và thời gian ghi
        private string GenerateUniqueKey(AudioData data)
        {
            return $"{data.SamplingMethod} - {data.RecordedTime.ToString("yyyy-MM-dd HH:mm:ss")}";
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
                string? selectedKey = lvResults.SelectedItems[0].Tag as string;
                
                if (selectedKey != null && capturedAudioData.ContainsKey(selectedKey))
                {
                    isPlaying = true;
                    btnPlay.Text = "Stop";
                    
                    // Play the selected audio data
                    Task.Run(() =>
                    {
                        try
                        {
                            currentSampler.PlayAudio(capturedAudioData[selectedKey]);
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
                string key = entry.Key;
                
                // Hiển thị tên phương thức và thời gian thu âm
                string displayName = $"{audioData.SamplingMethod} - {audioData.RecordedTime.ToString("HH:mm:ss")}";
                
                ListViewItem item = new ListViewItem(displayName);
                item.Tag = key; // Lưu khóa duy nhất vào Tag để sử dụng sau này
                
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
