   M S B A M L   `   `   `  ���� E  ASRO_INGAME, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SRO_INGAME
SRO_INGAME      SRO_INGAME.Launcher   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:SRO_INGAME     -     � Title$  Launcher�� � WindowStartupLocation$ CenterScreen=�$	��651��$	��500�� � WindowStyle$
 None=� � AllowsTransparency .  � 
ResizeMode$ NoResize=����	#FFE60B0B 
  Cursor � Cursor# [  ����  �� ImageSource$[ UD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\/Resources\launcher-bg.png��.��� .+�]� -   $
��Left=�$	��651����.  $	��Top=�$	��500������  �� Color$ Black�� �� Opacity$ 0Q��� $��Notice title��$��17��$	��Top=����	#FFCBCBCB���	#FF635439$��84,202,92,0q�$��Times New Roman)�$��10(�$��3,3,1,1q� � -   	  �Name$	 SliderFrame��$	��Top=�$��Center=�$	��325��$��50��$��84,221,92,0q��� -   
 ��Name$
 AutoLoginFrame��$��Disabled=�$��Center=�$	��182��$��85,326,89,143q�.���� .+��� $��	60,10,0,0q�$��Username��$
��Left=�$	��Top=�$��Times New Roman)�$��14(����	#FF7E7C7A�� -    ��Name$ Username��$
��bot1��  	srTextbox#��[  �� TextWrapping$
 Wrap=�$��Center=��� $��60,0,0,0q�$��Password��$
��Left=�$	��Top=�$��Times New Roman)�$��14(����	#FF7E7C7AG� -    G�Name$ Password�� G� Password$	 123��$��Center=��� $��60,0,0,0q�$��	Character��$
��Left=�$	��Top=�$��Times New Roman)�$��14(����	#FF7E7C7A�� -    ��Name$ 	LoginChar��$��0,0,0,10q�  ComboBoxFlatStyle#���    Pointer# [ ���	#FFD4D4D4$	��181��$��Center=�$��25��.@��� $��
ExoriaBot1�� �� 
IsSelected . �� $��mo���� $��Exoria���� -    ��Name$ QuickLoginCheckBox��$��10q� � ToolTip$M GIf auto login -> hide the sro_client untill the char spawn then show it��$ ��Activate Auto Login System��$��Center=����	#FF7E7C7A$��Times New Roman)�$��14(�# [ {�. �� -	   $
 SettingsFrame��$��Disabled=�$��Center=�$	��182��$��85,326,89,143q�$y�Hidden=�.���� .+��� -
   $ QuickLoginCheckBox1��$��10q�$M GIf auto login -> hide the sro_client untill the char spawn then show it��$��	AutoLogin��$��Center=����	#FFBBBBBB$��Times New Roman)�$��14(�# [ {�. �� $��10q�$, &Back to selection screen after restart��$#��Selection screen when restart��$��Center=����	#FFBBBBBB$��Times New Roman)�$��14(�# [ {�. �� $��60,0,0,0q�$��Graphics��$
��Left=�$	��Top=�$��Times New Roman)�$��14(����	#FF7E7C7A�� $��0,0,0,10q�#���  # [ ���	#FFD4D4D4$	��181��$��Center=�.@��� $��Hight�� . �� $��Medium�� . �� -     srButton#��[ $
��Left=�$��334,549,0,0q�$	��Top=�$	��160��$��30��
������ $��Times New Roman)�$��14(����	#FFBBBBBB���� -   StartButtonImg��$* $/Resources/laucnher-start-button.png�� �� .+��� $��Witacheon Headgear (+12)�����#ffd953$
��Bold"�$��-4q� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     
BlurRadius$ 1Q�   	Direction$	 -20Q�   ShadowDepth$ 1Q��� $�� ��$��-4q��� $��Seal of Sun�����#ffd953$
��Bold"�$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $��Power�����#6ce675$
��Bold"�$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $�� ��$��-4q��� $$��Sort of item: Two handed staff�����#efdaa4$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $��
Degree: 11�����#efdaa4$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $�� ��$��-4q��� $)��#Phy.atk.pwr. 2437.0 ~ 2780.0 (+80%)�����	#ffffffff$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $*��$Mag.def.pwr. 3648.0 ~ 4091.0 (+100%)�����	#ffffffff$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $��Durability 138/91 (+0%)�����	#ffffffff$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $��Attack distance 1 m�����	#ffffffff$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $��Attack rate 247 (+100%)�����	#ffffffff$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $�� ��$��-4q��� $��Required level 101�����	#ffffffff$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $��European�����	#ffffffff$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $'��!Max. no. of magic options: 9 Unit�����#efdaa4$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $�� ��$��-4q��� $��Str 7 Increase�����#00eaff$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $��Int 7 Increase�����#00eaff$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $��Immortalt (1Time/times)�����#00eaff$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $�� ��$��-4q��� $'��!Advanced elixir is in effect [+2]�����#c8c8c8$
��Bold"�$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� $6��0Repair Invalid (Maximum Durability 400 Increase)�����#ff2f51$
��Bold"�$��-4q�   $ 1Q�$	 -20Q�$ 1Q��� -    ��Name$ 	AutoLogin��$��	AutoLogin��$
��Left=�$	��Top=�$	��125��$��200,549,0,0q�
�������$
��Bold"�$��Calibri)�$��30��$��0q�$��16(���Z� ��Z� �� -   $ Settigns��$��Settigns��$
��Left=�$	��Top=�$	��125��$��100,549,0,0q�
�������$
��Bold"�$��Calibri)�$��30��$��0q�$��16(���Z� ��Z� �� -   $ exitBrn��$
��Exit��$
��Left=�$	��Top=�$	��125��$��
10,549,0,0q�
�������$
��Bold"�$��Calibri)�$��30��$��0q�$��16(���Z� ��Z� 