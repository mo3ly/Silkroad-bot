   M S B A M L   `   `   `  ���� E  ASRO_INGAME, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:SRO_INGAME.ViewSRO_INGAME.View       SRO_INGAME.View.Statistics   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     U QWpfAnimatedGif, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9e7cd3b544a090dc,gif"http://wpfanimatedgif.codeplex.com  )localclr-namespace:SRO_INGAME.View  $	��725��$	��380��  N� Title$  
Statistics��.��� .+��� $��Total kills:��$
��Left=�$��	39,30,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFB4B4B4�� $	��150��$
��Left=�$��	39,48,0,0q�$	��Top=�$��Times New Roman)�$��25(����	#FF7D6A42�� $��Picked items��$
��Left=�$��
208,30,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFB4B4B4�� $	��150��$
��Left=�$��
208,48,0,0q�$	��Top=�$��Times New Roman)�$��25(����	#FF7D6A42�� $��Remaining hours��$
��Left=�$��
39,154,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFB4B4B4�� $��30 mins��$
��Left=�$��
39,172,0,0q�$	��Top=�$��Times New Roman)�$��25(����	#FF7D6A42�� $��Gold gained��$
��Left=�$��
38,295,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFB4B4B4�� $��100,300��$
��Left=�$��208,172,0,0q�$	��Top=�$��Times New Roman)�$��25(����	#FF7D6A42�� $��	Gained SP��$
��Left=�$��208,295,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFB4B4B4�� $��230,302��$
��Left=�$��208,313,0,0q�$	��Top=�$��Times New Roman)�$��25(����	#FF7D6A42�� $��
Gained EXP��$
��Left=�$��208,154,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFB4B4B4�� $	��300��$
��Left=�$��
38,313,0,0q�$	��Top=�$��Times New Roman)�$��25(����	#FF7D6A42&   System.Windows.Controls.DataGrid  -     Name$ NearbyItemsGrid��   CanUserSortColumns .     GridLinesVisibility$
 None=�$   HorizontalScrollBarVisibility$ Hidden=�"   VerticalScrollBarVisibility$ Hidden=�   CanUserResizeRows .     CanUserDeleteRows .     CanUserResizeColumns .  	   CanUserAddRows	 .  
   SelectionMode$
 Single=�   SelectionUnit$ FullRow=�   CanUserReorderColumns .     AutoGenerateColumns .  $
��Left=�$	��322��$��
386,30,0,0q�$	��Top=�$	��312��   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly .    Header$
 Item��   MaxWidth$ 70Q�   Width5  /System.Windows.Controls.DataGridLengthConverter$ 70    CellTemplate �� .R��� $��20��$��20��$
��Left=�.+��� $
��Left=����� *Item+ � ToolTip �� *ItemTooltip+�� $
��Left=�"  WpfAnimatedGif.ImageBehavior   AnimatedSource$V PD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\soxglow.gif��$��	0,-20,0,0q�y��� *Sox+ �� *ItemTooltip+0  *System.Windows.Controls.DataGridTextColumn   . $
 Name��$	 180Q�$	 180 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Name+   . $  ��$ 62Q�$ 62  �� .R��� -   $��Center=�$��Collect�����	#FFDADADA$��10(�$��52��$��20��
������ 
 � Tag �� *UID+����  �� ImageSource �� *	ButtonSrc+�� -    ��Name$ RefreshButton��$
��Left=�$��
361,36,0,0q�$	��Top=�$��20��$��20��$��0q�   Pointer � Cursor# [  $ Refresh nearby items data������ -   RefreshNearbyBG���� -    ��Name$ NearbyCountLabel��$��Nearby Items: 0 item(s)��$
��Left=�$��
555,31,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FF7D6A42