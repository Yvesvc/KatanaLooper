using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NAudio.Wave;
using System;
using System.Linq;
using System.Windows.Input;
using System.IO;
using KatanaLooper.Classes;
using System.Drawing;
using KatanaLooper.Settings;
using Microsoft.Win32;

namespace KatanaLooper.ViewModel
{

    public class MainViewModel : ViewModelBase, IWavformViewModel
    {
        private readonly WavformSettings wavSettings = new WavformSettings(800, 200);
        Recorder recorder;
        WaveFileReader wavFileReader;
        WaveOut player;

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

        private string _recordWavCommandText;
        public string RecordWavCommandText
        {
            get => _recordWavCommandText;
            set { _recordWavCommandText = value; RaisePropertyChanged(); }
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

        public string SaveWavCommandImage => GeneralSettings.IconsDirectory + "Save" + ".PNG";
        public string LoadWavCommandImage => GeneralSettings.IconsDirectory + "Open" + ".PNG";

        public double RecordWavCommandOpacity => _canRecord ? 1.0 : 0.25; 
        public double PlayWavCommandOpacity => _canPlayWav ? 1.0 : 0.25; 
        public double SaveWavCommandOpacity => _canSaveWav ? 1.0 : 0.25; 
        public double LoadWavCommandOpacity => _canLoadWav ? 1.0 : 0.25; 

        public ICommand RecordWavCommand { get; set; }
        public ICommand UpdateStartAndEndOfWavCommand { get; set; }
        public ICommand PlayWavCommand { get; set; }
        public ICommand SaveWavCommand { get; set; }
        public ICommand LoadWavCommand { get; set; }
        
        private int _wavformWidth;
        public int WavformWidth
        {
            get => _wavformWidth;
            set
            {
                _wavformWidth = value;
                RaisePropertyChanged();
            }
        }

        private int _wavformHeight;
        public int WavformHeight
        {
            get => _wavformHeight;
            set
            {
                _wavformHeight = value;
                RaisePropertyChanged();
            }
        }

        private Bitmap _wavform;
        public Bitmap Wavform
        {
            get => _wavform;
            set
            {
                _wavform = value;
                RaisePropertyChanged();
            }
        }

        private Bitmap _greyedOutWavform;
        public Bitmap GreyedOutWavform
        {
            get => _greyedOutWavform;
            set
            {
                _greyedOutWavform = value;
                RaisePropertyChanged();
            }
        }

        private double _lengthRecordingInSec;
        public double LengthRecordingInSec
        {
            get => _lengthRecordingInSec;
            set
            {
                _lengthRecordingInSec = value;
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
            UpdateStartAndEndOfWavCommand = new RelayCommand<Tuple<double, double>>(UpdateStartAndEndOfTrimmedWav);
            PlayWavCommand = new RelayCommand(PlayWav, _canPlayWav);
            SaveWavCommand = new RelayCommand(SaveWav, _canSaveWav);
            LoadWavCommand = new RelayCommand(LoadWav, _canLoadWav);
            WavformWidth = wavSettings.WavformWidth;
            WavformHeight = wavSettings.WavformHeight;
        }

        private bool _canPlayWav => !Recording;

        private void PlayWav()
        {
            if (!Playing)
            {
                StartPlaying();
            }
            else
            {
                StopPlaying();
            }
        }


        private void StartPlaying()
        {
            if (_startOrEndOfTrimmedWavHasChanged)
            {
                WavTrimmer.Trim(wavSettings.UntrimmedRecordingFilePath, wavSettings.TrimmedRecordingFilePath, _startOfTrimmedWav, _endOfTrimmedWav);
            }

            wavFileReader = new WaveFileReader(wavSettings.TrimmedRecordingFilePath);
            LoopStream loopstream = new LoopStream(wavFileReader);
            loopstream.StreamEnded += LoopStreamEnded;
            LengthRecordingInSec = (double)wavFileReader.Length / (double)wavFileReader.WaveFormat.AverageBytesPerSecond;
            player = new WaveOut();
            player.DesiredLatency = 50;
            player.Init(loopstream);
            Playing = true;
            player.Play();
        }

        private void LoopStreamEnded()
        {
            StreamEnded = true;
        }

        private void StopPlaying()
        {
            Playing = false;
            player.Stop();
            player.Dispose();
            wavFileReader.Dispose();
        }

        private bool _canSaveWav => !Recording && !Playing && File.Exists(wavSettings.TrimmedRecordingFilePath);

        private void SaveWav()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "wav | *.wav*",
                FileName = "test",
                DefaultExt = "wav",
                AddExtension = true
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                File.Copy(wavSettings.TrimmedRecordingFilePath, saveFileDialog.FileName);
            }
        }

        private bool _canLoadWav => !Recording && !Playing;

        private void LoadWav()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "wav | *.wav*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                File.Copy(openFileDialog.FileName, wavSettings.UntrimmedRecordingFilePath, true);
                File.Copy(openFileDialog.FileName, wavSettings.TrimmedRecordingFilePath, true);
                RenderWavform();
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
            Recording = false;
            recorder.StopRecording();
            recorder.Dispose();
            RenderWavform();
        }


        private void RenderWavform()
        {
            Bitmap wavform = WavformRenderer.Render(wavSettings);
            Bitmap greyedOutWavform = WavformRenderer.IncreaseBrightness(wavform);

            Wavform = wavform;
            GreyedOutWavform = greyedOutWavform;

            WaveFileReader wavFileReader = new WaveFileReader(wavSettings.TrimmedRecordingFilePath);
            LengthRecordingInSec = (double)wavFileReader.Length / (double)wavFileReader.WaveFormat.AverageBytesPerSecond;
            wavFileReader.Dispose();
            RaisePropertyChanged(nameof(SaveWavCommandOpacity));
        }

    }
}
