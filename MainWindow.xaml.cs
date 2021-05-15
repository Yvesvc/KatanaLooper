using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KatanaLooper
{
    public partial class MainWindow : Window
    {
        public volatile bool _playing;
        public volatile bool _streamEnded;
        private double _lengthRecordingInSec;

        public MainWindow()
        {
            InitializeComponent();
            BPMTextBox.Text = "BPM";
            Canvas.SetLeft(LeftThumbLine, LeftThumb.Width / 2);
            Canvas.SetLeft(RightThumbLine, Canvas.GetLeft(RightThumb) + (RightThumb.Width / 2));
            CreateBindingsBetweenCodeBehindAndViewModel();
        }

        private void CreateBindingsBetweenCodeBehindAndViewModel()
        {
            INotifyPropertyChanged viewModel = (INotifyPropertyChanged)this.DataContext;
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals("Recording") && (sender as IWavformViewModel).Recording)
                {
                    ResetThumbs();
                }
                if (args.PropertyName.Equals("Playing"))
                {
                    if((sender as IWavformViewModel).Playing)
                    {
                        _playing = true;
                        RunProgressBar();
                    }

                    else
                    {
                        _playing = false;
                    }
                    
                }
                if (args.PropertyName.Equals("StreamEnded") && (sender as IWavformViewModel).StreamEnded)
                {
                    _streamEnded = true;
                }
                if (args.PropertyName.Equals("LengthRecordingInSec"))
                {
                    _lengthRecordingInSec = (sender as IWavformViewModel).LengthRecordingInSec;

                    ShowBeatsOnWavCanvas(BPMTextBox.Text);
                }

                (sender as IWavformViewModel).WavLoaded += WavLoaded;
            };
        }

        private void WavLoaded()
        {
            ResetThumbs();
        }

        private void RunProgressBar()
        {
            Canvas.SetLeft(ProgressBar, Canvas.GetLeft(LeftThumb));
            double pixelsPerSec = Canvas.GetLeft(RightThumb) - Canvas.GetLeft(LeftThumb) / _lengthRecordingInSec;

            FrameworkElement progressBar = null;
            foreach (FrameworkElement el in WavCanvas.Children)
            {
                if ((el as FrameworkElement).Name == nameof(ProgressBar))
                {
                    progressBar = el;
                }
            }
            Task.Factory.StartNew(() => //Start on new thread otherwise Thread.Sleep(delay) in ShowProgressBar() would continuously block the current (UI) thread
            {
                ShowProgressBar(progressBar, pixelsPerSec);
            }); ;
        }

        private void ShowProgressBar(FrameworkElement progressBar, double pixelsPerSec)
        {
            int delay = 10;
            double pixels = (pixelsPerSec * delay) / 1000;
            while (_playing)
            {
                if (_streamEnded)
                {
                    Dispatcher.Invoke(() =>
                    {
                        progressBar.SetValue(Canvas.LeftProperty, Canvas.GetLeft(LeftThumb));
                    });
                    _streamEnded = false;
                }

                else
                {
                    Thread.Sleep(delay);
                    Dispatcher.Invoke(() =>
                    {
                        progressBar.SetValue(Canvas.LeftProperty, (double)progressBar.GetValue(Canvas.LeftProperty) + pixels);
                    });
                }

            }
        }

        private void ResetThumbs()
        {
            Canvas.SetLeft(LeftThumb, 0);
            Canvas.SetLeft(LeftThumbLine, (LeftThumb.Width / 2));
            Canvas.SetLeft(RightThumb, WavCanvas.Width);
            Canvas.SetLeft(RightThumbLine, WavCanvas.Width + (LeftThumb.Width / 2));
            TrimmedWavformStart.Width = 0;
            UntrimmedWavform.Width = WavCanvas.Width;
        }

        private void LeftThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if ((Canvas.GetLeft(LeftThumb) + e.HorizontalChange) < 0.0)
            {
                return;
            }

            if (Canvas.GetLeft(RightThumb) - (Canvas.GetLeft(LeftThumb) + e.HorizontalChange) < 5.00)
            {
                return;
            }

            Canvas.SetLeft(LeftThumb, Canvas.GetLeft(LeftThumb) + e.HorizontalChange);
            Canvas.SetLeft(LeftThumbLine, Canvas.GetLeft(LeftThumb) + (LeftThumb.Width / 2));
            TrimmedWavformStart.Width = Canvas.GetLeft(LeftThumb);
        }

        private void RightThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if ((Canvas.GetLeft(RightThumb) + e.HorizontalChange) > WavCanvas.Width)
            {
                return;
            }

            if (Canvas.GetLeft(RightThumb) - (Canvas.GetLeft(LeftThumb) + e.HorizontalChange) < 5.00)
            {
                return;
            }

            Canvas.SetLeft(RightThumb, Canvas.GetLeft(RightThumb) + e.HorizontalChange);
            Canvas.SetLeft(RightThumbLine, Canvas.GetLeft(RightThumb) + (RightThumb.Width / 2));
            UntrimmedWavform.Width = Canvas.GetLeft(RightThumb);
        }

        private void DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            IWavformViewModel viewModel = DataContext as IWavformViewModel;
            double wavformStart = Canvas.GetLeft(LeftThumb) / WavCanvas.Width;
            double wavformEnd = Canvas.GetLeft(RightThumb) / WavCanvas.Width;
            viewModel.UpdateStartAndEndOfWavCommand.Execute(new Tuple<double, double>(wavformStart, wavformEnd));
        }

        private void BPMTextBox_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBox bpmTextBox = sender as TextBox;
            if (new Regex("^[a-zA-Z ]+$").IsMatch(bpmTextBox.Text))
            {
                bpmTextBox.Text = string.Empty;
            }
        }

        private void BPMTextBox_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsNumeric(e.Text); new Regex("^[0-9]+$").IsMatch(e.Text);
        }

        private bool IsNumeric(string text)
        {
            return new Regex("^[0-9]+$").IsMatch(text);
        }

        private void ShowBeatsOnWavCanvas(string beatsPerMinute)//Based on the beat, it's easier to know where to trim the end of the wave form (only works if you play in rhythm ;) )
        {
            if (!IsNumeric(beatsPerMinute))
            {
                return;
            }

            RemoveBeats();
            AddBeats(int.Parse(beatsPerMinute));
        }

        private void AddBeats(int bpm)
        {
            int numberOfBeatsToAdd = (int)(((double)bpm * _lengthRecordingInSec) / 60);
            double secPerBeat = 60.0 / (double)bpm;
            double numberOfPixels = Canvas.GetLeft(RightThumb) - Canvas.GetLeft(LeftThumb);
            double pixelsPerBeat = (secPerBeat * numberOfPixels) / _lengthRecordingInSec;
            double currentPixel = Canvas.GetLeft(LeftThumb);
            for (int i = 0; i < numberOfBeatsToAdd; i++)
            {
                System.Windows.Shapes.Rectangle beat = new System.Windows.Shapes.Rectangle
                {
                    Height = 10,
                    Width = 3,
                    Fill = new SolidColorBrush() { Color = System.Windows.Media.Color.FromArgb(255, 255, 255, 0) },
                    Name = "Beat" + i
                };
                WavCanvas.Children.Add(beat);
                Canvas.SetLeft(beat, currentPixel);
                currentPixel += pixelsPerBeat;
            }
        }

        private void RemoveBeats()
        {
            List<FrameworkElement> beats = WavCanvas.Children.Cast<FrameworkElement>().Where(element => element.Name.Contains("Beat")).ToList();

            int numberOfBeatsInCanvas = beats.Count();
            for (int i = 0; i < numberOfBeatsInCanvas; i++)
            {
                WavCanvas.Children.Remove(beats.ElementAt(i));
            }
        }

        private void BPMTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowBeatsOnWavCanvas((sender as TextBox).Text);
        }

    }
}

