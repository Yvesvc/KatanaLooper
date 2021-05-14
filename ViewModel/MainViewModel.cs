using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.IO;
using System.Xml.Serialization.Configuration;
using KatanaLooper.Classes;
using System.Windows.Markup;
using System.Windows.Media;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using KatanaLooper.Settings;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace KatanaLooper.ViewModel
{

    public class MainViewModel : ViewModelBase, IWaveformViewModel
    {
        private readonly WaveformSettings wavSettings = new WaveformSettings(800, 200);

        private string _recordWavCommandText;
        public string RecordWavCommandText
        {
            get => _recordWavCommandText;
            set { _recordWavCommandText = value; RaisePropertyChanged(); }
        }

        private bool _recording;
        public bool Recording
        {
            get => _recording;
            set
            {
                _recording = value;
                RaisePropertyChanged();
                RecordWavCommandText = _recording ? "Stop" : "Start";
                RecordWavCommandImage = GeneralSettings.IconsDirectory + (_recording ? "RecordingOn" : "RecordingOff") + ".PNG";
                RaisePropertyChanged(nameof(PlayWavCommandOpacity));
                RaisePropertyChanged(nameof(LoadWavCommandOpacity));
                RaisePropertyChanged(nameof(SaveWavCommandOpacity));
            }
        }

        private bool _playing;
        public bool Playing
        {
            get => _playing;

            set
            {
                _playing = value;
                PlayWavCommandImage = GeneralSettings.IconsDirectory + (_playing ? "Stop" : "Start") + ".PNG";
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(RecordWavCommandOpacity));
                RaisePropertyChanged(nameof(LoadWavCommandOpacity));
                RaisePropertyChanged(nameof(SaveWavCommandOpacity));
            }
        }

        private bool _streamEnded;
        public bool StreamEnded
        {
            get => _streamEnded;

            set
            {
                _streamEnded = value;
                RaisePropertyChanged();
            }
        }

        private string _recordWavCommandImage;
        public string RecordWavCommandImage
        {
            get => _recordWavCommandImage;
            set
            {
                _recordWavCommandImage = value;
                RaisePropertyChanged();
            }
        }

        private string _playWavCommandImage;
        public string PlayWavCommandImage
        {
            get => _playWavCommandImage;
            set
            {
                _playWavCommandImage = value;
                RaisePropertyChanged();
            }
        }

        public string SaveWavCommandImage
        {
            get => GeneralSettings.IconsDirectory + "Save" + ".PNG";
        }

        public string LoadWavCommandImage
        {
            get => GeneralSettings.IconsDirectory + "Open" + ".PNG";
        }

        public double RecordWavCommandOpacity => _canRecord ? 1.0 : 0.25; 
        public double PlayWavCommandOpacity => _canPlayWav ? 1.0 : 0.25; 
        public double SaveWavCommandOpacity => _canSaveWav ? 1.0 : 0.25; 
        public double LoadWavCommandOpacity => _canLoadWav ? 1.0 : 0.25; 

        public ICommand RecordWavCommand { get; set; }
        public ICommand RenderWaveformCommand { get; set; }
        public ICommand UpdateStartAndEndOfWavCommand { get; set; }
        public ICommand PlayWavCommand { get; set; }
        public ICommand SaveWavCommand { get; set; }
        public ICommand LoadWavCommand { get; set; }

        Recorder recorder;

        WaveFileReader wfreader;
        WaveOut player;

        private int _waveformWidth;
        public int WaveformWidth
        {
            get => _waveformWidth;
            set
            {
                _waveformWidth = value;
                RaisePropertyChanged();
            }
        }

        private int _waveformHeight;
        public int WaveformHeight
        {
            get => _waveformHeight;
            set
            {
                _waveformHeight = value;
                RaisePropertyChanged();
            }
        }

        private Bitmap _waveform;
        public Bitmap Waveform
        {
            get => _waveform;
            set
            {
                _waveform = value;
                RaisePropertyChanged();
            }
        }

        private Bitmap _greyedOutWaveform;
        public Bitmap GreyedOutWaveform
        {
            get => _greyedOutWaveform;
            set
            {
                _greyedOutWaveform = value;
                RaisePropertyChanged();
            }
        }

        private double _lengthSongInSec;
        public double LengthSongInSec
        {
            get => _lengthSongInSec;
            set
            {
                _lengthSongInSec = value;
                RaisePropertyChanged();
            }
        }

        private double _startOfTrimmedWav;
        private double _endOfTrimmedWav;
        private bool _startOrEndOfTrimmedWavHasChanged;

        public event Action WavLoaded;

        public MainViewModel()
        {
            Recording = false;
            Playing = false;
            RecordWavCommand = new RelayCommand(Record, _canRecord);
            RenderWaveformCommand = new RelayCommand(RenderWaveform);
            UpdateStartAndEndOfWavCommand = new RelayCommand<Tuple<double, double>>(UpdateStartAndEndOfTrimmedWav);
            PlayWavCommand = new RelayCommand(PlayWav, _canPlayWav);
            SaveWavCommand = new RelayCommand(SaveWav, _canSaveWav);
            LoadWavCommand = new RelayCommand(LoadWav, _canLoadWav);
            //wavSettings = new WaveformSettings(800, 200);
            WaveformWidth = wavSettings.WaveformWidth;
            WaveformHeight = wavSettings.WaveformHeight;
        }

        private bool _canPlayWav => !Recording;

        private void PlayWav()
        {
            if (!Playing)
            {
                //Playing = true;
                StartPlaying();
            }
            else
            {
                Playing = false;
                StopPlaying();
            }
        }


        private void StartPlaying()
        {
            if (_startOrEndOfTrimmedWavHasChanged)
            {
                WavTrimmer.Trim(wavSettings.UntrimmedRecordingFilePath, wavSettings.TrimmedRecordingFilePath, _startOfTrimmedWav, _endOfTrimmedWav);
            }

            wfreader = new WaveFileReader(wavSettings.TrimmedRecordingFilePath);
            LoopStream loop = new LoopStream(wfreader);
            loop.StreamEnded += Loop_StreamEnded;
            LengthSongInSec = (double)wfreader.Length / (double)wfreader.WaveFormat.AverageBytesPerSecond;
            player = new WaveOut();
            player.DesiredLatency = 50;
            player.Init(loop);
            Playing = true;
            player.Play();
        }

        private void Loop_StreamEnded()
        {
            StreamEnded = true;
        }

        private void StopPlaying()
        {
            player.Stop();
            player.Dispose();
            wfreader.Dispose();
        }

        private bool _canSaveWav => !Recording && !Playing && File.Exists(wavSettings.TrimmedRecordingFilePath);


        private void SaveWav()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "wav | *.wav*";
            saveFileDialog.FileName = "test";
            saveFileDialog.DefaultExt = "wav";
            saveFileDialog.AddExtension = true;
            if (saveFileDialog.ShowDialog() == true)
            {
                File.Copy(wavSettings.TrimmedRecordingFilePath, saveFileDialog.FileName);
            }
        }

        private bool _canLoadWav => !Recording && !Playing;

        private void LoadWav()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "wav | *.wav*";
            if (openFileDialog.ShowDialog() == true)
            {
                File.Copy(openFileDialog.FileName, wavSettings.UntrimmedRecordingFilePath, true);
                File.Copy(openFileDialog.FileName, wavSettings.TrimmedRecordingFilePath, true);
                RenderWaveform();
                WavLoaded.Invoke();
            }
        }

        private void UpdateStartAndEndOfTrimmedWav(Tuple<double, double> startAndEnd)
        {
            _startOfTrimmedWav = startAndEnd.Item1;
            _endOfTrimmedWav = startAndEnd.Item2;
            _startOrEndOfTrimmedWavHasChanged = true;
        }

        private bool _canRecord =>!Playing;

        private void Record()
        {
            if (!Recording)
            {
                StartRecording();
            }
            else
            {
                Recording = false;
                StopRecording();
            }
        }

        private void StartRecording()
        {
            Recording = true;

            if (!AsioOut.GetDriverNames().Contains("KATANA"))
            {
                throw new Exception("KATANA Asio Driver is not installed.");
            }

            try
            {
                recorder = new Recorder(wavSettings, 0, 1, 44100);
            }
            catch
            {
                Recording = false;
                throw new Exception("KATANA amp not found, amp must be connected after the program is started.");
            }
            recorder.StartRecording();
        }

        private void StopRecording()
        {
            recorder.StopRecording();
            recorder.Dispose();
            RenderWaveform();
        }


        private void RenderWaveform()
        {
            Bitmap waveform = WaveformRenderer.Render(wavSettings);
            Bitmap greyedOutWaveform = WaveformRenderer.IncreaseBrightness(waveform);

            Waveform = waveform;
            GreyedOutWaveform = greyedOutWaveform;

            WaveFileReader reader = new WaveFileReader(wavSettings.TrimmedRecordingFilePath);
            LengthSongInSec = (double)reader.Length / (double)reader.WaveFormat.AverageBytesPerSecond;
            reader.Dispose();
            RaisePropertyChanged(nameof(SaveWavCommandOpacity));
        }

    }
}
