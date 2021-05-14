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
        private double _lengthSongInSec;

        public MainWindow()
        {
            InitializeComponent();
            BPMTextBox.Text = "BPM";
            Canvas.SetLeft(LeftThumbLine, LeftThumb.Width / 2);
            Canvas.SetLeft(RightThumbLine, Canvas.GetLeft(RightThumb) + (RightThumb.Width / 2));
            CreateCodeBehindAndViewModelBindings();
        }

        private void CreateCodeBehindAndViewModelBindings()
        {
            INotifyPropertyChanged viewModel = (INotifyPropertyChanged)this.DataContext;
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals("Recording") && (sender as IWaveformViewModel).Recording)
                {
                    ResetThumbs();
                }
                if (args.PropertyName.Equals("Playing") && (sender as IWaveformViewModel).Playing)
                {
                    RunProgressBar();

                }
                if (args.PropertyName.Equals("Playing") && !(sender as IWaveformViewModel).Playing)
                {
                    _playing = false;
                }
                if (args.PropertyName.Equals("StreamEnded") && (sender as IWaveformViewModel).StreamEnded)
                {
                    _streamEnded = true;
                }
                if (args.PropertyName.Equals("LengthSongInSec"))
                {
                    _lengthSongInSec = (sender as IWaveformViewModel).LengthSongInSec;

                    ShowBeatsOnWavCanvas(BPMTextBox.Text);
                }

                (sender as IWaveformViewModel).WavLoaded += WavLoaded;
            };
        }

        private void WavLoaded()
        {
            ResetThumbs();
        }

        private void RunProgressBar()
        {
            _playing = true;
            Canvas.SetLeft(ProgressBar, Canvas.GetLeft(LeftThumb));
            double dist = Canvas.GetLeft(RightThumb) - Canvas.GetLeft(LeftThumb);
            double distpersec = dist / _lengthSongInSec;
            UIElementCollection coll = WavCanvas.Children;
            FrameworkElement bar = null;
            foreach (FrameworkElement el in coll)
            {
                if ((el as FrameworkElement).Name == "ProgressBar")
                {
                    bar = el;
                }
            }
            Task.Factory.StartNew(() =>
            {
                ShowProgressBar(distpersec, bar);
            }); ;
        }

        private void ShowProgressBar(double distpersec, FrameworkElement bar)
        {
            int delay = 10;
            double dist = (distpersec * delay) / 1000;
            while (_playing)
            {
                if (_streamEnded)
                {
                    Dispatcher.Invoke(() =>
                    {
                        bar.SetValue(Canvas.LeftProperty, Canvas.GetLeft(LeftThumb));
                    });
                    _streamEnded = false;
                }

                else
                {
                    Thread.Sleep(delay);
                    Dispatcher.Invoke(() =>
                    {
                        double current = (double)bar.GetValue(Canvas.LeftProperty);
                        bar.SetValue(Canvas.LeftProperty, current + dist);
                    });
                }

            }
        }

        private void ResetThumbs()
        {
            Canvas.SetLeft(LeftThumb, 0);
            Canvas.SetLeft(LeftThumbLine, (LeftThumb.Width / 2));
            Canvas.SetLeft(RightThumb, 800);//not hardcoding
            Canvas.SetLeft(RightThumbLine, 800 + (LeftThumb.Width / 2));
            ProcessedWaveformStart.Width = 0;
            UnprocessedWaveform.Width = 800;
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
            ProcessedWaveformStart.Width = Canvas.GetLeft(LeftThumb);
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
            UnprocessedWaveform.Width = Canvas.GetLeft(RightThumb);
        }

        private void DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            IWaveformViewModel viewModel = DataContext as IWaveformViewModel;
            double waveformStart = Canvas.GetLeft(LeftThumb) / WavCanvas.Width;
            double waveformEnd = Canvas.GetLeft(RightThumb) / WavCanvas.Width;
            viewModel.UpdateStartAndEndOfWavCommand.Execute(new Tuple<double, double>(waveformStart, waveformEnd));
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
            e.Handled = !new Regex("^[0-9]+$").IsMatch(e.Text);
        }

        private void ShowBeatsOnWavCanvas(string beatsPerMinute)
        {
            if (!new Regex("^[0-9]+$").IsMatch(beatsPerMinute))
            {
                return;
            }

            RemoveBeats();
            AddBeats(int.Parse(beatsPerMinute));
        }

        private void AddBeats(int bpm)
        {
            double start = Canvas.GetLeft(LeftThumb);
            double end = Canvas.GetLeft(RightThumb);

            int numberOfBeatsToAdd = (int)(((double)bpm * _lengthSongInSec) / 60);
            double intervalInSec = 60.0 / (double)bpm;
            double numberOfPixels = Canvas.GetLeft(RightThumb) - Canvas.GetLeft(LeftThumb);
            double intervalInPixels = (intervalInSec * numberOfPixels) / _lengthSongInSec;
            double currentPixel = start;
            for (int i = 0; i < numberOfBeatsToAdd; i++)
            {
                System.Windows.Shapes.Rectangle ellipse = new System.Windows.Shapes.Rectangle();
                ellipse.Height = 10;
                ellipse.Width = 3;
                ellipse.Fill = new SolidColorBrush() { Color = System.Windows.Media.Color.FromArgb(255, 255, 255, 0) };
                ellipse.Name = "Beat" + i;
                WavCanvas.Children.Add(ellipse);
                Canvas.SetLeft(ellipse, currentPixel);
                currentPixel += intervalInPixels;
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

