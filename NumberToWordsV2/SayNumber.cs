using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.Windows;

namespace NumberToWordsV2
{
    public partial class MainWindow
    {
        public SpeechSynthesizer? synthsizer;
        private List<InstalledVoice>? voiceList;


        private void InitializeSynthsizer()
        {
            this.synthsizer = new SpeechSynthesizer();
            this.synthsizer.StateChanged += Synthsizer_StateChanged;
            this.synthsizer.SpeakCompleted += Synthsizer_SpeakCompleted;
            this.synthsizer.SpeakProgress += Synthsizer_SpeakProgress;
            this.synthsizer.SelectVoice(GetVoiceName());
            this.synthsizer.Rate = GetVoiceRate();
        }

        private void Synthsizer_StateChanged(object? sender, StateChangedEventArgs e)
        {
            switch (e.State)
            {
                case SynthesizerState.Paused:
                    if (this.synthsizer != null)
                    {
                        this.synthsizer.Pause();
                    }
                    break;
            }
        }

        private void Synthsizer_SpeakCompleted(object? sender, SpeakCompletedEventArgs e)
        {
            if (this.synthsizer != null)
            {
                this.synthsizer.Dispose();
                this.synthsizer = null;
            }

            this.NumberInWords.IsInactiveSelectionHighlightEnabled = false;
            this.NumberInWords.Focus();
            this.NumberInWords.Select(0, 0);
            this.cboVoices.IsEnabled = true;
            this.cboRate.IsEnabled = true;

        }

        private void Synthsizer_SpeakProgress(object? sender, SpeakProgressEventArgs e)
        {

            this.NumberInWords.Focus();
            this.NumberInWords.Select(e.CharacterPosition, e.Text.Length);
        }

        private void SetVoiceRates()
        {
            cboRate.Items.Add("Slower");
            cboRate.Items.Add("Normal");
            cboRate.Items.Add("Faster");

            cboRate.SelectedIndex = 1;
        }

        private bool LoadInstalledVoices()
        {

            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                voiceList = new List<InstalledVoice>();
                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    voiceList.Add(voice);
                    cboVoices.Items.Add($"{voice.VoiceInfo.Name}-" +
                        $"{voice.VoiceInfo.Gender}-{voice.VoiceInfo.Culture}");
                }
                if (voiceList.Count == 0)
                    return (false);
            }

            cboVoices.SelectedIndex = 0;
            return (true);

        }

        private string? GetVoiceName()
        {
            if (this.cboVoices.Items.Count == 0)
            {
                MessageBox.Show("You must first install some voices");
                return null;
            }

            if (this.cboVoices.SelectedIndex < 0)
            {
                MessageBox.Show("You must first install some voices");
                return null;
            }
            else
            {

                string cboVoiceValue = (string)this.cboVoices.SelectedItem;

                var buckets = cboVoiceValue.ToString().Split("-");

                if (voiceList != null)
                {
                    for (int j = 0; j < voiceList.Count; j++)
                    {
                        var _v = voiceList[j];
                        var v_o = _v.VoiceInfo.Name.ToString().Trim().ToLower();
                        var v_b = buckets[0].ToString().Trim().ToLower();
                        if (v_o == v_b)
                            return (_v.VoiceInfo.Name.ToString());
                    }
                }
                return null;
            }
        }



        private int GetVoiceRate()
        {
            int voiceRate = 0;
            switch (this.cboRate.SelectedValue)
            {
                case "Slower": voiceRate = -2; break;
                case "Normal": voiceRate = 2; break;
                case "Faster": voiceRate = 4; break;
            }
            return (voiceRate);
        }


    }
}
