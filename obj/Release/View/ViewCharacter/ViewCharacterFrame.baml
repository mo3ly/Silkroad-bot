   M S B A M L   `   `   `  ���� E  ASRO_INGAME, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:SRO_INGAME.View.ViewCharacterSRO_INGAME.View.ViewCharacter  6    0SRO_INGAME.View.ViewCharacter.ViewCharacterFrame   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     U QWpfAnimatedGif, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9e7cd3b544a090dc,gif"http://wpfanimatedgif.codeplex.com  7local+clr-namespace:SRO_INGAME.View.ViewCharacter  -   $	��435��$	��736��  N� Title$  ViewCharacter��.��� -    �Name$ CharacterGrid��.+��� $
��Left=�$	��116��$��	16,15,0,0q�$	��Top=�$	��128�����	#FF4B4B4B$��1q� �� CornerRadius$ 3,0,0,0��
���   �._��� -    ��Name$ CharacterImage��$
��Left=�$��95��$	��Top=�$	��108��   Pointer � Cursor# [  $��9,18,0,0q��� -    ��Name$ CharacterLabel��$��	CHARACTER��$
��Left=�$��	18,17,0,0q�$	��Top=����	#FFBDBDBD$��Times New Roman)�$	��124��$��17��$��2,2,2,2q����	#FF4B4B4B$��0,0,0,1q�����  �� EndPoint$ 0.5,1&� � MappingMode$ RelativeToBoundingBox=� �� 
StartPoint$ 0.5,0&�.��� 	 � Color$	 Black��
 � Offset$
 1Q�� $	 	#7F494949���� -   $ CharacterRaceIcon��$
��Left=�$��14��$��
126,18,0,0q�$	��Top=�$��14���� -   $ CharacterJobIcon��$
��Left=�$��14��$��
107,18,0,0q�$	��Top=�$��14�� � ToolTip$ Level: 3���� $
��Left=�$	��116��$��
175,15,0,0q�$	��Top=�$	��312�����	#FF4B4B4B$��1q�$ 3,0,0,0��
���   ��� -   $ 
LevelLabel��$��	Level: 20��$
��Left=�$��
179,44,0,0q�$	��Top=����	#FF76736F$��Times New Roman)�$��14(����	#00000000�� $
��Left=�$��12��$��
185,77,0,0q�$	��Top=�$��4�����	#FF850E16�� -   $ HPLabel��$��56504��$
��Left=�$��
189,70,0,0q�$	��Top=����	#FF76736F$��Times New Roman)�$��14(����	#00000000$��24��$ Strength: 312���� $
��Left=�$��12��$��185,103,0,0q�$	��Top=�$��4�����	#FF122F7E�� -	   $ MPLabel��$��86447��$
��Left=�$��
189,96,0,0q�$	��Top=����	#FF76736F$��Times New Roman)�$��14(����	#00000000$��24��$ Intellect: 109���� -
   $ 
MasteryOne��$
��Left=�$��20��$��
185,22,0,0q�$	��Top=�$��20��$	 Bow���� -   $ 
MasteryTwo��$
��Left=�$��20��$��
212,22,0,0q�$	��Top=�$��20��$
 Cold���� -   $ RankOnServer��$��Rank: 1st on server��$
��Left=�$��
325,46,0,0q�$	��Top=����	#FF76736F$��Times New Roman)�$��14(����	#00000000$��24��$7 1Updated every 1 day: based on all game activities���� -   $ HonorPoints��$��Honor Points: 420��$
��Left=�$��
325,71,0,0q�$	��Top=����	#FF76736F$��Times New Roman)�$��14(����	#00000000$��24��$ Graduated Characters: 320���� -   $ 
ItemPoints��$��Item Points: 828��$
��Left=�$��
325,96,0,0q�$	��Top=����	#FF76736F$��Times New Roman)�$��14(����	#00000000$��24��$ Average plus: +9��&   System.Windows.Controls.DataGrid  -     Name$ GameLogGrid��   GridLinesVisibility$ 
Horizontal=�$   HorizontalScrollBarVisibility$ Hidden=�"   VerticalScrollBarVisibility$ Hidden=�   CanUserResizeRows .     CanUserDeleteRows .     CanUserResizeColumns .     CanUserAddRows .     SelectionMode$ Single=�   SelectionUnit$ FullRow=�   CanUserReorderColumns .     AutoGenerateColumns .  $
��Left=�$	��260��$��175,155,0,0q�$	��Top=�$	��312��   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly .    Header$ Image��   MaxWidth$ 60Q�   Width5  /System.Windows.Controls.DataGridLengthConverter$ 60    CellTemplate �� .R��� $
��Left=����� *Image+0  *System.Windows.Controls.DataGridTextColumn   . $ Unique��$	 160Q�$	 160 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Unique+   . $ Total Kills��$ 92Q�$ 92  �� *
TotalKills+�� -   $ GearsFrameBG��$d��^D:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\character\equip_frame.png��$	��400��$	��200��$��	0,0,20,20q�$��Right=�$��Bottom=��� -   $ GearsFrameLabel��$��	EQUIPMENT��$
��left=�$��
520,18,0,0q�$	��Top=����	#FF76736F$��Times New Roman)�$��12(����	#00000000 J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect      Color$  	#FF522500��!   ShadowDepth$! 1Q�"   	Direction$" 20Q�#   
BlurRadius$# 3Q��� -   $ ��Name$$ SwitchGearsButton��$��Right=�$��	0,20,25,0q�$	��Top=�$��20��$��20��$��0q�# [  $ View Avatar Gears.������ -   SwitchGearsButtonBG��� -   $ EquipmentFrame��$��516,47,20,20q�.+��� -   $ weaponFrame��$��32��$��32��$��15,10,153,326q�# [  �� -   $ weaponFrameSox��"  WpfAnimatedGif.ImageBehavior%   AnimatedSource$V% PD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\soxglow.gif��$��20��$��20��$��15,10,165,338q�$y�Hidden=��� -   $ ShieldFrame��$��32��$��32��$��153,10,15,326q�# [  �� -   $ ShieldFrameSox��$V% PD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\soxglow.gif��$��20��$��20��$��153,10,27,338q�$y�Hidden=��� -   $ 	HeadFrame��$��32��$��32��$��7,96,161,240q�# [  �� -   $ HeadFrameSox��$V% PD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\soxglow.gif��$��20��$��20��$��7,96,173,252q�$y�Hidden=��� -   $ ShouldersFrame��$��32��$��32��$��161,96,7,240q�# [  �� -   $ ShouldersFrameSox��$V% PD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\soxglow.gif��$��20��$��20��$��161,96,19,252q�$y�Hidden=��� -   $ 
ChestFrame��$��32��$��32��$��7,145,161,191q�# [  �� -   $ ChestFrameSox��$V% PD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\soxglow.gif��$��20��$��20��$��7,145,173,203q�$y�Hidden=��� -   $ 
HandsFrame��$��32��$��32��$��161,145,7,191q�# [  �� -    $ HandsFrameSox��$V% PD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\soxglow.gif��$��20��$��20��$��161,145,19,203q�$y�Hidden=��� -!   $ 	LegsFrame��$��32��$��32��$��7,194,161,142q�# [  �� -"   $ LegsFrameSox��$V% PD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\soxglow.gif��$��20��$��20��$��7,194,173,154q�$y�Hidden=��� -#   $ 
BootsFrame��$��32��$��32��$��161,194,7,142q�# [  �� -$   $ BootsFrameSox��$V% PD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\soxglow.gif��$��20��$��20��$��161,194,19,154q�$y�Hidden=��� -%   $ NecklaceFrame��$��32��$��32��$��7,269,161,67q�# [  �� -&   $ NecklaceFrameSox��$V% PD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\soxglow.gif��$��20��$��20��$��7,269,173,79q�$y�Hidden=��� -'   $ EarringFrame��$��32��$��32��$��161,269,7,67q�# [  �� -(   $ EarringFrameSox��$V% PD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\soxglow.gif��$��20��$��20��$��161,269,19,79q�$y�Hidden=��� -)   $ LeftRingFrame��$��32��$��32��$��7,318,161,18q�# [  �� -*   $ LeftRingFrameSox��$V% PD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\soxglow.gif��$��20��$��20��$��7,318,173,30q�$y�Hidden=��� -+   $ RightRingFrame��$��32��$��32��$��161,318,7,18q�# [  �� -,   $ RightRingFrameSox��$V% PD:\disk\Games\Visual Studio Projects\SRO_INGAME\SRO_INGAME\Resources\soxglow.gif��$��20��$��20��$��161,318,19,30q�$y�Hidden=��� --   $ AvatarHeadFrame��$��32��$��32��$��523,143,181,260q�# [  $y�Hidden=��� -.   $ AvatarFlagFrame��$��32��$��32��$��677,143,27,260q�# [  $y�Hidden=��� -/   $ AvatarDressFrame��$��32��$��32��$��523,192,181,211q�# [  $y�Hidden=��� -0   $ AvatarAttachFrame��$��32��$��32��$��677,192,27,211q�# [  $y�Hidden=��� -1   $ AvatarDevilFrame��$��32��$��32��$��523,241,181,162q�# [  $y�Hidden=��� -2   $ UniqueKillsButton��$��Unique Kills��$
��Left=�$��
16,155,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FFD4D4D4# [  $	��128�����	#99232323���	#FF4B4B4B$��1,0,0,0q��� -3   $ 	PvpButton��$��PVP Activity��$
��Left=�$��
16,203,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FF535353# [  ���	#99232323���	#FF4B4B4B$��1,0,0,0q�$	��128���� -4   $ 	JobButton��$��Job Activity��$
��Left=�$��
16,251,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FF535353# [  ���	#99232323���	#FF4B4B4B$��1,0,0,0q�$	��128���� -5   $ BattleRoyaleButton��$��Battle Royale��$
��Left=�$��
16,297,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FF535353# [  ���	#99232323���	#FF4B4B4B$��1,0,0,0q�$	��128���� -6   $ GlobalButton��$��Recent Globals��$
��Left=�$��
16,342,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FF535353# [  $	��128�����	#99232323���	#FF4B4B4B$��1,0,0,0q��� -7   $ AchievementsButton��$��Achievements��$
��Left=�$��
16,389,0,0q�$	��Top=�$��Times New Roman)�$��14(����	#FF535353# [  $	��128�����	#99232323���	#FF4B4B4B$��1,0,0,0q�