   M S B A M L   `   `   `  ���� E  ASRO_INGAME, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null_4clr-namespace:SRO_INGAME.View.Main.LauncherCompnents&SRO_INGAME.View.Main.LauncherCompnents  8    2SRO_INGAME.View.Main.LauncherCompnents.ItemsSlider   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Flocal4clr-namespace:SRO_INGAME.View.Main.LauncherCompnents     -     N�Name$  Slider��$��50��$	��325�� N� Title$ ItemsSlider�� 
  Cursor � Cursor# [   � 	Resources  �� 
TargetType �� Property �� Value �� Triggers P� Property	 P� Value  DefaultPosition
 �� TargetProperty	 t� To g� DecelerationRatio g� Duration	 W� To  SlideLeftIn t� From  SlideLeftOut g� AccelerationRatio  SlideRightIn  SlideRightOut  SlideAndFadeIn W� From  SlideAndFadeOut��%�  '��       &	     &	 �    &	 �    &	 (    &	 t    &	 �    &	 2    �� " ��.���  �@�Cursor$
 Hand����  � ��$ 32����  � ��$ 32����  � ��$ 0q���  � ��$ Center=���  � ��$ Center=���  �@J�Opacity$ 1Q� P�  �@J�IsMouseOver	 . . ���  �@J�Opacity$	 0.7Q��� .��t� $
 Margin�$ 0X�$ .9Q�$ 0:0:.01D�W� $
 Opacity�$ 1X�$ 0:0:.01D�W� $L
 F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�$ 1X�$ 0:0:.01D�$ .9Q�W� $L
 F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�$ 1X�$ 0:0:.01D�$ .9Q�W� $K
 E(RenderTransform).(TransformGroup.Children)[1].(SkewTransform.AngleX)�$ 0X�$ 0:0:.01D�$ .9Q�W� $K
 E(RenderTransform).(TransformGroup.Children)[1].(SkewTransform.AngleY)�$ 0X�$ 0:0:.01D�$ .9Q�W� $L
 F(RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)�$ 0X�$ 0:0:.01D�$ .9Q��� .��t� $
 Margin�$ 250,0,-250,0X�$ 0X�$ .9Q�$ 0:0:.75D��� .��t� $
 Margin�$ -250,0,250,0X�$ .9Q�$ 0:0:.5D��� .��t� $
 Margin�$ -250,0,250,0X�$ 0X�$ .9Q�$ 0:0:.75D��� .��t� $
 Margin�$ 250,0,-250,0X�$ .9Q�$ 0:0:.5D��� .��t� $
 Margin�$ 250,0,-250,0X�$ 0X�$ .9Q�$ 0:0:.6D�W� $
 Opacity�$ 0X�$ 1X�$ 0:0:.5D��� .��t� $
 Margin�$ -250,0,250,0X�$ .9Q�$ 0:0:.5D�W� $
 Opacity�$ 0X�$ 0:0:.5D�.���� $��Center=�$��Center=����	#19000000$��50������ *ActualWidth+ �� ElementName$ Slider��.+��  � Orientation$ 
Horizontal=�$��Center=�.+��� -    ��Name$ 	LeftArrow�� K� ZIndex$ 1��$��0,2,0,0q�$��24��$
��Left=�  Pointer# [ �� $��Hidden=�$��Hidden=�$��Center=�$��Center=�$	��230��$��50��.���� -    ��Name$ ItemsCanvas�����	#19000000.+�� -    �Name$ 
CanvasGrid������ *ActualWidth+$ ItemsCanvas������ *ActualHeight+$ ItemsCanvas���� -   $ 
RightArrow��$ 1��$��0,2,0,0q�$��24��$��Right=�# [ 