   M S B A M L   `   `   `  ���� E  ASRO_INGAME, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:SRO_INGAME.View.StallsSRO_INGAME.View.Stalls  )    #SRO_INGAME.View.Stalls.NearbyStalls   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6local$clr-namespace:SRO_INGAME.View.Stalls     -   $	��435��$	��736��  N� Title$  NearbyStalls��.��� .+��� -    ��Name$ labelStallsCount��$��Nearby Stalls: 0��$
��Left=�$��	42,29,0,0q�$	��Top=�$��Times New Roman)�$��18(����	#FFAEAEAE�� -    ��Name$ PrevPageButton��$��Right=�$��
0,40,101,0q�$	��Top=�$��16��$��16��$��0q�   Pointer � Cursor# [   � ToolTip$ Previous Page.������ -   PrevPageButtonBG���� -    ��Name$ PaginationNumberLabel��  	srTextbox#��[ $��
641,38,0,0q�$��0�� �� 
IsReadOnly . $��30��$��1q�$��19�� �� TextAlignment$ Center=����	#FF191919$��1q����	#FFCDCBCB�� -   $ NextPageButton��$��Right=�$��	0,40,43,0q�$	��Top=�$��16��$��16��$��0q�# [  $ 
Next page.������ -   NextPageButtonBG��&   System.Windows.Controls.DataGrid  -     Name$ NearbyStallsGrid��  xStlalDataGrid#��[ 	   CanUserSortColumns	 .  
   GridLinesVisibility$
 
Horizontal=�$   HorizontalScrollBarVisibility$ Hidden=�   CanUserResizeRows .     CanUserDeleteRows .     CanUserResizeColumns .     CanUserAddRows .     SelectionMode$ Single=�   SelectionUnit$ FullRow=�   CanUserReorderColumns .     AutoGenerateColumns .  $
��Left=�$	��365��$��	42,38,0,0q�$	��Top=�$	��651��   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly .    Header$  ��   MaxWidth$ 70Q�   Width5  /System.Windows.Controls.DataGridLengthConverter$ 70    CellTemplate �� .R��� $
��Left=�$��45��$��45��.+��� $
��Left=�$ Location x: -50 y: 904��$
��Fill=����� *Icon+0  *System.Windows.Controls.DataGridTextColumn   . $  ��$	 100Q�$	 100 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Name+   . $  ��$ 90Q�$ 90  �� *
ItemsCount+   . $  ��$	 160Q�$	 160  �� *
Location+   . $  ��$	 160Q�$	 160  �� *Date+   . $  ��$ 52Q�$ 52  �� .R��� -	   $��Center=�$��Details�����	#FFDADADA$��10(�$��52��$��20��
������ 
 � Tag �� *	StallID+����  �� ImageSource �� *	ButtonSrc+�� -
    ��Name$ Overlay�����	#90000000$��42,65,43,32q�$y�Hidden=��� -    ��Name$ EmptyStallLabel��$��Center=�$��Center=����	#FF494949$��1q� �� Padding$ 2q�  �� CornerRadius$  4��$y�	Collapsed=�._��� $-��'There is no avaliable stalls right now.��$��Times New Roman)�$��18(����	#FFBDBDBD � -   !  �Name$! 
stallFrame��$	��416��$	��239��