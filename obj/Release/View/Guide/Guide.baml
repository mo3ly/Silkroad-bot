   M S B A M L   `   `   `  ���� E  ASRO_INGAME, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:SRO_INGAME.View.GuideSRO_INGAME.View.Guide  !    SRO_INGAME.View.Guide.Guide   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     U QWpfAnimatedGif, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9e7cd3b544a090dc,gif"http://wpfanimatedgif.codeplex.com  /local#clr-namespace:SRO_INGAME.View.Guide  $	��725��$	��370��  N� Title$  Guide�� � 	Resources  �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� Triggers P� Property	 P� Value
 �� 
TargetName J� SnapsToDevicePixels-  'System.Windows.Input.KeyboardNavigation   TabNavigation � ColumnDefinitions �Name � RowDefinitions �� CornerRadius K� IsItemsHost K� ZIndex   TabIndex   DirectionalNavigation �� SelectedContent �� SelectedContentTemplate" �� SelectedContentStringFormat �� ContentStringFormat��%�  '��       '�� Y    �� " ��.���  � ��$ 12(���  � �� �	#FFFDFDFD��  � ��
 ���� ��  � �� �� " ��.R��� $ PART_Border��#��z��$��1q����	#FF323232$��2q�._��� $��Header��$��2q� P�  �@��
IsSelected	 . . ��� $
 PART_Border�� � �� �	#FF404040�� $
 PART_Border�� � �� �	#FF232323�� " ��.���  �@��TabStripPlacement$
 Left=���  � ��$ 2q���  � ��$ 2q���  � ��
 �   ���  � �� �� " ��.R�� }�.  . $ Local=� �� $ ColumnDefinition0���� $ ColumnDefinition1��$��0 � �� $ RowDefinition0��$
��Auto ��� $ RowDefinition1��$��* �.+��� $ HeaderBorder�����	#FF232323$��1q�$ 5�����	#FF080808$��0,0,0,5q�._��� $��HeaderPanel�� . $ 1��$ 1��$��0��$��0��� $��ContentPanel��$ 2��$ Local=�$ 	Contained=�$��0��$��1��.+��� #��z��#��z��#��z��$ 4��._��� $��PART_SelectedContentHost��#��z #��z # z $��SelectedContent��$��2q�# z  P�  �@��TabStripPlacement$	 Bottom=�. ��� $
 HeaderPanel�� � ��$ 1���� $
 ContentPanel�� � ��$ 0���� $
 RowDefinition0�� � ��$ * ��� $
 RowDefinition1�� � ��$
 Auto ��� $
 HeaderBorder�� � ��$ 0,5,0,0q�P�  �@��TabStripPlacement$
	 Left=�. ��� $
 HeaderPanel�� � ��$ 0���� $
 ContentPanel�� � ��$ 0���� $
 HeaderPanel�� � ��$ 0���� $
 ContentPanel�� � ��$ 1���� $
 ColumnDefinition0�� � ��$
 Auto ��� $
 ColumnDefinition1�� � ��$ * ��� $
 RowDefinition0�� � ��$ * ��� $
 RowDefinition1�� � ��$ 0 ��� $
 HeaderBorder�� � ��$ 0,0,5,0q�P�  �@��TabStripPlacement$	 Right=�. ��� $
 HeaderPanel�� � ��$ 0���� $
 ContentPanel�� � ��$ 0���� $
 HeaderPanel�� � ��$ 1���� $
 ContentPanel�� � ��$ 0���� $
 ColumnDefinition0�� � ��$ * ��� $
 ColumnDefinition1�� � ��$
 Auto ��� $
 RowDefinition0�� � ��$ * ��� $
 RowDefinition1�� � ��$ 0 ��� $
 HeaderBorder�� � ��$ 5,0,0,0q�P�  � {�	 .  . ��� # � �� � ��.��� .+��� $
��Left=�$	��360��$��10,5,0,0q�$	��Top=�$	��705�����	#FF252525���	#FFA2A2A2.@��� $��Chat Commands��.��� 
���   �.+��� $��Block��$
��Left=�$��	10,10,0,0q�$	��Top=�$��Times New Roman)�$��25(����	#FF7D6A42�� $!��→ \block exc < charname >��$
��Left=�$��	10,49,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFB4B4B4���	#FF1D1D1D	� $��RadioButton��$
��Left=�$��
10,462,0,0q�$	��Top=��� $1��+Block this character from sending exchange.��$
��Left=�$��
181,49,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFB4B4B4���	#00000000�� $��	Auto hunt��.��� 
���   ��� $��Wheel of fortune��.��� 
���   ��� $��Map Test��.��� 
���   �.+��� $
��Left=�$	��281��$��	94,43,0,0q�$	��Top=�$	��408��$R��LD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources/map.jpg���� $��10q�.+��� -    ��Name$ MyRectangle��$	��246�����	#A51960DC$��
84,35,81,0q� � Triggers :�  :� RoutedEvent$ FrameworkElement.Loaded��.[��� .���� .��W�  �� 
TargetName$ MyRectangle�� �� TargetProperty$ Opacity� W� From$	 1.0X�	 W� To$	 0.0X�  g� Duration$  0:0:5D�! g� AutoReverse! . " g� RepeatBehavior$" Forever���� -   # ��Name$# 
zoneCircle��"  WpfAnimatedGif.ImageBehavior$   AnimatedSource$Z$ TD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\zone-circle.gif��$��90,62,82,90q�% J� RenderTransformOrigin$% 0.5,0.5&�z��� & �� ScaleX$	& 1.5Q�' �� ScaleY$	' 1.5Q�