   M S B A M L   `   `   `  ���� E  ASRO_INGAME, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:SRO_INGAME.View.GameBot.PotionSRO_INGAME.View.GameBot.Potion  4    .SRO_INGAME.View.GameBot.Potion.CharacterPotion   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     >local,clr-namespace:SRO_INGAME.View.GameBot.Potion     $	��290��$	��670��  N� Title$  CharacterPotion��.��� .+��� $
��Left=�$��70��$��
176,10,0,0q�$	��Top=�$	��480�����	#FF424141.+��� $��68��$��0q� � ToolTip$J DMake sure you select a HP potion before to activate the auto-potion.������  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ Black�� � Offset$ 0Q�� $ 	#FF1F1F1F��$ 1Q��� $
��Left=�$��70��$��176,109,0,0q�$	��Top=�$	��480�����	#FF424141.+��� $��68��$��0q�$J DMake sure you select a MP potion before to activate the auto-potion.������ $ 0.5,1&�$ 0.5,0&�.��� $ Black��$ 0Q�� $ 	#FF1F1F1F��$ 1Q��� $
��Left=�$��70��$��176,208,0,0q�$	��Top=�$	��480�����	#FF424141.+��� $��68��$��0q�$P JMake sure you select a abnormal potion before to activate the auto-potion.������ $ 0.5,1&�$ 0.5,0&�.��� $ Black��$ 0Q�� $ 	#FF1F1F1F��$ 1Q��� $
��Left=�$	��Top=�$��42��$��42��$��
197,26,0,0q����	#FF7D6A42.+��� $��38��$��38�����	#FF111111$��2,2,2,1q�.+��� -    ��Name$ 
slotHpIcon��$��32��$��32��$��2,3,2,1q��� $��HP��$
��Left=�$��
288,18,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFBBBBBB�� -    ��Name$ 
hpValueBox��   	srTextbox#��[  $��
323,21,0,0q�$��0�� �� 
IsReadOnly . $��30��$��1q�$��20���� $��%��$
��Left=�$��355,121,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFBBBBBB�� -   	 ��Name$	 
hpCheckBox��$
��Left=�$	��Top=�$��
262,26,0,0q�{�.  �� -   
 ��Name$
 hpSlider��$
��Left=�$��
258,41,0,0q�$	��Top=�$	��380��  Pointer � Cursor# [  � SmallChange$ 1Q� �� TickFrequency$ 1Q� �� IsSnapToTickEnabled .  � Maximum$	 100Q�{�.  �� $
��Left=�$	��Top=�$��42��$��42��$��197,127,0,0q����	#FF7D6A42.+��� $��38��$��38�����	#FF111111$��2,2,2,1q�.+��� -   $ 
slotMpIcon��$��32��$��32��$��2,3,2,1q��� $��MP��$
��Left=�$��288,121,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFBBBBBB�� -   $ 
mpValueBox��#��[  $��323,124,0,0q�$��0�� . $��30��$��1q�$��20���� $��%��$
��Left=�$��
355,18,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFBBBBBB�� -   $	 
mpCheckBox��$
��Left=�$	��Top=�$��262,127,0,0q�{�.  �� -   $
 mpSlider��$
��Left=�$��258,141,0,0q�$	��Top=�$	��380��$��40��# [  � Value$ 0Q�$	 100Q�$ 1Q�$ 1Q� . {�.  �� $
��Left=�$	��Top=�$��42��$��42��$��197,225,0,0q����	#FF7D6A42.+��� $��38��$��38�����	#FF111111$��2,2,2,1q�.+��� -	   $ SlotAbnormal��$��32��$��32��$��2,3,2,1q��� $��Delay��$
��Left=�$��319,244,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFBBBBBB�� #��[  $��258,247,0,0q�$��0�� . $��58��$��1q�$��20���� -
   $	 abnormalCheckBox��$��Abormal status��$
��Left=�$��258,225,0,0q�$	��Top=����	#FFBBBBBB$��Times New Roman)�$��14(�# [ {�.   J� RenderTransformOrigin$ 0.522,-0.188&��� $��Auto switch helth type.��$
��Left=�$��
492,20,0,0q�$	��Top=�$��Times New Roman)�$��12(����	#FF6C6C6C�� -   $	 sHPCheckBox��$
��Left=�$	��Top=�$��
619,26,0,0q�$P JAuto switch to another avaliable HP type when the selected potion run out.���� $
��Left=�$��70��$��8,10,0,0q�$	��Top=�$	��150�����	#FF424141.+��� $
��Auto=�$��68��$��Hidden=����� $ 0.5,1&�$ 0.5,0&�.��� $ Black��$ 0Q�� $ 	#FF1F1F1F��$ 1Q�.��� -    �Name$ HpRootFrame��$
��Auto��$
��Auto�� � ColumnDefinitions �� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* � � RowDefinitions �� $��1* �.+��� $
��Left=�$	��Top=��� $��Auto switch mana type.��$
��Left=�$��490,122,0,0q�$	��Top=�$��Times New Roman)�$��12(����	#FF6C6C6C�� -   $	 sMPCheckBox��$
��Left=�$	��Top=�$��619,127,0,0q�$P JAuto switch to another avaliable MP type when the selected potion run out.���� $
��Left=�$��70��$��	8,109,0,0q�$	��Top=�$	��150�����	#FF424141.+��� $
��Auto=�$��68��$	��150��$��Hidden=����� $ 0.5,1&�$ 0.5,0&�.��� $ Black��$ 0Q�� $ 	#FF1F1F1F��$ 1Q�.��� -   $ MpRootFrame��$
��Auto��$
��Auto�� �� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* � �� $��1* �.+��� $
��Left=�$	��Top=��� $ ��Auto switch abnormal type.��$
��Left=�$��468,234,0,0q�$	��Top=�$��Times New Roman)�$��12(����	#FF6C6C6C�� -   $	 sAbnormalCheckBox��$
��Left=�$	��Top=�$��619,237,0,0q�$S MAuto switch to another avaliable Pills type when the selected potion run out.���� $
��Left=�$��70��$��	8,208,0,0q�$	��Top=�$	��150�����	#FF424141.+��� $
��Auto=�$��68��$	��150��$��Hidden=����� $ 0.5,1&�$ 0.5,0&�.��� $ Black��$ 0Q�� $ 	#FF1F1F1F��$ 1Q�.��� -   $ AbnormalRootFrame��$
��Auto��$
��Auto�� �� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* � �� $��1* �.+��� $
��Left=�$	��Top=�� $
��Left=�$	��298��$��	6,-10,0,0q�$	��Top=�$	��650��