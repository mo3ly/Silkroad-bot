   M S B A M L   `   `   `  ���� E  ASRO_INGAME, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:SRO_INGAME.View.GameBot.PotionSRO_INGAME.View.GameBot.Potion  .    (SRO_INGAME.View.GameBot.Potion.PetPotion   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     >local,clr-namespace:SRO_INGAME.View.GameBot.Potion     $	��290��$	��670��  N� Title$  	PetPotion��.��� .+��� $
��Left=�$��70��$��
176,10,0,0q�$	��Top=�$	��480�����	#FF424141.+��� $��68��$��0q� � ToolTip$L FMake sure you select a pet potions before to activate the auto-potion.������  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ Black�� � Offset$ 0Q�� $ 	#FF1F1F1F��$ 1Q��� $
��Left=�$	��Top=�$��
197,26,0,0q�$��42��$��42�����	#FF7D6A42.+��� $��38��$��38�����	#FF111111$��2,2,2,1q�.+��� -    ��Name$ 
SlotHgpPet��$��32��$��32��$��2,3,2,1q��� $	��HGP��$
��Left=�$��
288,18,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFBBBBBB�� -    ��Name$ petHGPValueBox��   	srTextbox#��[  $��
323,21,0,0q�$��0�� �� 
IsReadOnly . $��30��$��1q�$��20���� $��%��$
��Left=�$��
355,18,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFBBBBBB�� -   	 ��Name$	 petHGPCheckBox��$
��Left=�$	��Top=�$��
262,26,0,0q�{�.  �� -   
 ��Name$
 petHGPSlider��$
��Left=�$��
258,41,0,0q�$	��Top=�$	��380��$��40��  Pointer � Cursor# [  � Value$ 0Q� � Maximum$	 100Q� � SmallChange$ 1Q� �� TickFrequency$ 1Q� �� IsSnapToTickEnabled . {�.  �� $��Auto switch pet HGP type.��$
��Left=�$��
475,20,0,0q�$	��Top=�$��Times New Roman)�$��12(����	#FF6C6C6C�� -   $	 sHGPCheckBox��$
��Left=�$	��Top=�$��
619,26,0,0q�$X RAuto switch to another avaliable pet potion type when the selected potion run out.���� $
��Left=�$��70��$��8,10,0,0q�$	��Top=�$	��150�����	#FF424141.+��� $
��Auto=�$��68��$	��150��$��Hidden=����� $ 0.5,1&�$ 0.5,0&�.��� $ Black��$ 0Q�� $ 	#FF1F1F1F��$ 1Q�.��� -    �Name$ PetRootFrame��$
��Auto��$
��Auto�� � ColumnDefinitions �� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* � � RowDefinitions �� $��1* �.+��� $
��Left=�$	��Top=�