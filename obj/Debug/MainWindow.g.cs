﻿#pragma checksum "..\..\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "231C924088FDBF12BD683541AEF5535D30291270FBA9071DE69FDD8E4FB7F380"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using KatanaLooper;
using KatanaLooper.Converters;
using KatanaLooper.Settings;
using KatanaLooper.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace KatanaLooper {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 39 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BPMTextBox;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas WavCanvas;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ProcessedWaveformEnd;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image UnprocessedWaveform;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ProcessedWaveformStart;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle ThumbSlider;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle ProgressBar;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.Thumb LeftThumb;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle LeftThumbLine;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.Thumb RightThumb;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle RightThumbLine;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/KatanaLooper;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.BPMTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 39 "..\..\MainWindow.xaml"
            this.BPMTextBox.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.BPMTextBox_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 39 "..\..\MainWindow.xaml"
            this.BPMTextBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.BPMTextBox_TextInput);
            
            #line default
            #line hidden
            
            #line 39 "..\..\MainWindow.xaml"
            this.BPMTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.BPMTextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.WavCanvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 3:
            this.ProcessedWaveformEnd = ((System.Windows.Controls.Image)(target));
            return;
            case 4:
            this.UnprocessedWaveform = ((System.Windows.Controls.Image)(target));
            return;
            case 5:
            this.ProcessedWaveformStart = ((System.Windows.Controls.Image)(target));
            return;
            case 6:
            this.ThumbSlider = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 7:
            this.ProgressBar = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 8:
            this.LeftThumb = ((System.Windows.Controls.Primitives.Thumb)(target));
            
            #line 90 "..\..\MainWindow.xaml"
            this.LeftThumb.DragDelta += new System.Windows.Controls.Primitives.DragDeltaEventHandler(this.LeftThumb_DragDelta);
            
            #line default
            #line hidden
            
            #line 90 "..\..\MainWindow.xaml"
            this.LeftThumb.DragCompleted += new System.Windows.Controls.Primitives.DragCompletedEventHandler(this.DragCompleted);
            
            #line default
            #line hidden
            return;
            case 9:
            this.LeftThumbLine = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 10:
            this.RightThumb = ((System.Windows.Controls.Primitives.Thumb)(target));
            
            #line 100 "..\..\MainWindow.xaml"
            this.RightThumb.DragDelta += new System.Windows.Controls.Primitives.DragDeltaEventHandler(this.RightThumb_DragDelta);
            
            #line default
            #line hidden
            
            #line 100 "..\..\MainWindow.xaml"
            this.RightThumb.DragCompleted += new System.Windows.Controls.Primitives.DragCompletedEventHandler(this.DragCompleted);
            
            #line default
            #line hidden
            return;
            case 11:
            this.RightThumbLine = ((System.Windows.Shapes.Rectangle)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

