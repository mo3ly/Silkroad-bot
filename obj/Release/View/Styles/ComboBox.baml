   M S B A M L   `   `   `  ���� E  ASRO_INGAME, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:SRO_INGAME.View.StylesSRO_INGAME.View.Styles  W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   ComboBoxNormalBorderBrush  �� Color ! ComboBoxNormalBackgroundBrush # ComboBoxDisabledForegroundBrush # ComboBoxDisabledBackgroundBrush  ComboBoxDisabledBorderBrush �� 
TargetType   ComboBoxToggleButtonTemplate � ColumnDefinitions �� CornerRadius �� Triggers P� Property P� Value �� 
TargetName �� Property	 �� Value
 �� 
TargetType  Pointer  ComboBoxFlatStyle �� 	ClickMode d� 	IsChecked �� Path �� RelativeSource �� Mode J� IsHitTestVisible �� SelectionBoxItem � Cursor �� SelectionBoxItemTemplate �� 
IsReadOnly �� 
IsReadOnly � AllowsTransparency �� IsDropDownOpen J� SnapsToDevicePixels � ActualWidth �� MaxDropDownHeight-   'System.Windows.Input.KeyboardNavigation    DirectionalNavigation�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     .local$clr-namespace:SRO_INGAME.View.Styles  %J	  &	        &	      &	 *     &	 :     &	 O     (�� d     * SystemColors.HighlightBrushKey+)&	 s     7   7  7  7  7  7  7  '�� ?    7  &	 �    7  7  7  7   �� $  	#FF272728���� $  	#FF1D1D1D���� $
  #888���� $  	#FF1D1D1D���� $  	#FF272728���� $	  Red���� " d�.R��  �� �� $��20 �.+��� $��Border��$��2��8��  $ 0��$��2q�8�� �� $��ButtonBorder��$��1��$��2q����#444$ 
0, 0, 0, 0��$��
0, 0, 0, 0q�8�� F� $��Arrow��$��1��=����        �    �� � 	= ��[ � z �� � z     � 	=  	= 	$��Center=����	#FFA4874B$��Center=� P�  �@J�IsMouseOver . . ��� $ ButtonBorder�� � ��	 �	#FF323232P�  �@d�	IsChecked$
 TrueY�. ��� $ ButtonBorder�� � ��	 �	#FF323232�� $ Arrow�� � ��	 �	#FFA4874BP�  � {� .  . ��� $ Border��8	   � ���� $ ButtonBorder��8	   � ���� $ ButtonBorder��8	   � ���� 8	   � ���� $ Arrow�� � ��	 �#999�� "
 ��.���  � ��	 �	#FFD6D6D6��  � ��	 �	#FF303030��  � ��$	 0q���  �@�Cursor8	   �� "
 ��.���  �@J�SnapsToDevicePixels	 . ��  �@�OverridesDefaultStyle	 . ��  � ��$
	 Auto=���  � ��$
	 Auto=���  � ��	 . ��  � ��
	 �   ���  �@�FocusVisualStyle	 Z� ��  � ��	 �� " ��.R�� .+�d� $��ToggleButton��$��2��$ Press=�|�.  8��   �� $ IsDropDownOpen� �� *TemplatedParent+$ TwoWay=��� $��ContentSite��$��5, 3, 23, 3q� .  $
��Left=�$��Center=�#��z 8  #��z #��z���� $��PART_EditableTextBox��$��3, 3, 23, 3q�# z 8  $y�Hidden=�
������ $
��Left=�$��Center=�|�. .������ " ��.R��� $��PART_ContentHost��|�.  .��� $��Popup��$��Bottom=�|�.   . #��z $��Slide=�.��� $��DropDown�� . #��z #��z .+��� $��DropDownBorder�����	#FF303030$��
0, 1, 0, 0q�$ 0��$��1,1,1,1q�8�� �� $��4q� . .���� $ 	Contained=� P�  �@��HasItems .  . ��� $ DropDownBorder�� � ��$	 95��P�  � {� .  . ���  �@J�Opacity$	 .7Q�P�  �@��
IsGrouping . . ���  � ��	 .  P�  �@��
IsEditable . . ���  �@  	IsTabStop	 .  �� $ PART_EditableTextBox�� � y�$	 Visible=��� $ ContentSite�� � y�$	 Hidden=�