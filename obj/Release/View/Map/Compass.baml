   M S B A M L   `   `   `  ���� E  ASRO_INGAME, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:SRO_INGAME.View.MapSRO_INGAME.View.Map  !    SRO_INGAME.View.Map.Compass   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     3local!clr-namespace:SRO_INGAME.View.Map       � Title$  Compass�� � WindowStyle$
 None=� � 
ResizeMode$ NoResize=� � AllowsTransparency .  � Topmost . $	��120��$	��633�� � ShowInTaskbar .   
  Cursor � Cursor# [  
������ .��� .+��� $a��[D:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\Compass\Compass-bg.png�� J� Opacity$	 0.2Q�$��Center=�$��Center=��� -    ��Name$ CompassImage��$
��None=�	 � UseLayoutRounding	 . 
 �� BitmapScalingMode$
 HighQuality=� J� RenderTransformOrigin$ 0.5,0.5&�$^��XD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\Compass\compass.png��$
��6140��$��Center=�$��Center=�$��-2755,0,-2755,0q�z�u� -   transformCompass�� u� Matrix$ 0.8 0 0 0.8 0 0x�� 
������� � Points �   @KL ��� ��� $��311,25,310,76q��� -    ��Name$ DegreeLabel��$	��360��$
��Bold"�$��Center=�$��Bottom=�
�������$��14(�$��299,0,297,3q�$y�Hidden=�$��37��