   M S B A M L   `   `   `  ����G  CKatanaLooper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:KatanaLooperKatanaLooper  5   	   A%clr-namespace:KatanaLooper.ConvertersKatanaLooper.Converters  5   	   ?$clr-namespace:KatanaLooper.ViewModelKatanaLooper.ViewModel  5   	   =#clr-namespace:KatanaLooper.SettingsKatanaLooper.Settings  5	   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5
   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	       KatanaLooper.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:KatanaLooper     5   	   6conv%clr-namespace:KatanaLooper.Converters     5   	   3VM$clr-namespace:KatanaLooper.ViewModel     5   	   8Settings#clr-namespace:KatanaLooper.Settings     5	   	   ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     5
   	   9System&clr-namespace:System;assembly=mscorlib     5   	     � Title$  Katana Looper��5   	   $	��600��6   $
��1000��6,    � DataContext 5      *   $KatanaLooper.ViewModel.MainViewModel  5   
    � 	Resources 5      0   *KatanaLooper.Converters.CanvasTopConverter   CanvasTopConverter:   4KatanaLooper.Converters.BitmapToBitmapImageConverter   BitmapToBitmapImageConverter��%:   &	        &	        5   
   6"     5   
   6,   5       J� InputBindings 5      �� 5   
   
 �� Key$ R��6    �� Command 6   �� *RecordWavCommand+�� 5   
   $ Space��6    6!   �� *PlayWavCommand+�� 5   
   $ O��6    �� 	Modifiers$
 Ctrlf�6    6.   �� *LoadWavCommand+�� 5   
   $ S��6   $
 Ctrlf�6    6.   �� *SaveWavCommand+.��5      �  � ColumnDefinitions 5   
   �� 5      $��50 �6   6,   �� 5      $��50 �6   6,   �� 5      $��50 �6   6,   �� 5       $��50 �6   6,   �� 5!      $��50 �6   6,    � RowDefinitions 5#   
   �� 5$      $��25 �6   6*   �� 5%      $
��Auto �6   6,   .+�5)   
   �� -   5        $��
BPMTextBox��5)      $��0��6$   $��0��64   $��Center=�6A   $��Center=�6^   $��0q�6y   6�   �� 5*   
   $��1��6   $��0��6!   
�������$��0q�6A   	 J� Opacity	 6U   �� *RecordWavCommandOpacity+.��5+      �� ��6   �� *RecordWavCommandImage+.  (System.Windows.Interactivity.Interaction
   Triggers
 5,      /  )System.Windows.Interactivity.EventTrigger  5-         	EventName$ Click��6!   .  (System.Windows.Interactivity.TriggerBase   Actions. 5.      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6,   �� *RecordWavCommand+5/      51      �� 52   
   $��2��6   $��0��6!   
�������	 6A   �� *PlayWavCommandOpacity+.��53      �� ��6   �� *PlayWavCommandImage+
 54        55      $ Click��6!   . 56         6,   �� *PlayWavCommand+57      59      �� 5:   
   $��3��6   $��0��6!   
�������	 6A   �� *SaveWavCommandOpacity+.��5;      �� ��6   �� *SaveWavCommandImage+
 5<        5=      $ Click��6!   . 5>         6,   �� *SaveWavCommand+5?      5A      �� 5B   
   $��4��6   $��0��6!   
�������	 6A   �� *LoadWavCommandOpacity+.��5C      �� ��6   �� *LoadWavCommandImage+
 5D        5E      $ Click��6!   . 5F         6,   �� *LoadWavCommand+5G      5I      �� 5L   
   -   5         ��Name$ 	WavCanvas��5L      $��0��6$   $��4��64   $��1��6H   $��0,5,0,0q�6�   ��6V   �� *WavformHeight+��6w   �� *WavformWidth+.+�5O      �� -   5        $��ProcessedWavformEnd��5O       �� Left$ 0��60   $��UniformToFill=�6�   ��6@   �� *WavformHeight+��6a   �� *WavformWidth+��5P      �� *GreyedOutWavform+ �� 	Converter# [ �� 5Q      -   5        $��UntrimmedWavform��5Q      $ 0��6-   $��UniformToFill=�6}   ��6=   �� *WavformHeight+��6^   �� *WavformWidth+��5R      �� *	Wavform+# [ �� 5S      -   5        $��TrimmedWavformStart��5S      $ 0��60   $��0��6a   $��UniformToFill=�6k   ��6@   �� *WavformHeight+��5T      �� *GreyedOutWavform+# [ �� 5U      -   5         ��Name$ ThumbSlider��5U      $��5��6-   ��68   �� *WavformWidth+
 �� Top 5V      `� 5W       `� 	Converter# [  6#   .1�5X      ��  �� Path$ Height�6"    �� ElementName$ 	WavCanvas��60   �� 5Y      $ Height�6"   $ ThumbSlider��60   5Z      5\      �� 5]      -   5        $ ProgressBar��5]      $��5��6N   
��� � ���6-   �� *WavformHeight+6X   n� 5^      -   5         n�Name$ 	LeftThumb��5^      $��10��6'   $��5��63   $ 0��6=    5_      `� 5`      # [  6#   .1�5a      �� $ Height�6"   $ 	WavCanvas��60   �� 5b      $ Height�6"   $ 	LeftThumb��60   5c      5e      �� 5f      -	   5        $ LeftThumbLine��5f      $��1��6P   
���   ���6/   �� *WavformHeight+6Z   n� 5h      -
   5        $ 
RightThumb��5h      $��10��6(   $��5��64    6>   �� *WavformWidth+ 5i      `� 5j      # [  6#   .1�5k      �� $ Height�6"   $ 	WavCanvas��60   �� 5l      $ Height�6"   $ 
RightThumb��60   5m      5o      �� 5p      -   5        $ RightThumbLine��5p      $��1��6Q   
���   ���60   �� *WavformHeight+5q      5r      5t      5u      