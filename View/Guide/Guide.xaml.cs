using System.Windows.Controls;
namespace SRO_INGAME.View.Guide
{
    /// <summary>
    /// Interaction logic for Guide.xaml
    /// </summary>
    public partial class Guide : Page
    {
        //add the in game commands whisper and block whisper and all of that to this list
        /*
        1	UIIT_STT_CHAT_COMMAND_FINDUSER	/??	0	0	0	0	0	/Acquire	/Acquire	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_TRADE	/??	0	0	0	0	0	/Exchange	/Exchange	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_PVP_PROPOSAL	/??	0	0	0	0	0	/Fight	/Fight	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_PARTY_INVITE	/????	0	0	0	0	0	/InviteToParty	/InviteToParty	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_PARTY_EXPEL	/????	0	0	0	0	0	/BanishFromParty	/BanishFromParty	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_SECRET_TALK	/??	0	0	0	0	0	/Whisper	/Whisper	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_STREETSTORE	/??	0	0	0	0	0	/Stall	/Stall	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_SITDOWN	/??	0	0	0	0	0	/SitDown	/SitDown	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_PARTY_EXIT	/????	0	0	0	0	0	/LeaveTheParty	/LeaveTheParty	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_REPLY1	/??	0	0	0	0	0	/Reply	/Reply	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_REPLY2	/r	0	0	0	0	0	/r	/r	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_REPLY3	/?	0	0	0	0	0	/re	/re	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_REPLY4	/R	0	0	0	0	0	/R	/R	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_WHISPER1	/???	0	0	0	0	0	/whisper	/whisper	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_WHISPER2	/?	0	0	0	0	0	/W	/W	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_WHISPER3	/w	0	0	0	0	0	/w	/w	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_WHISPER4	/?	0	0	0	0	0	/w	/w	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_WHISPER5	/W	0	0	0	0	0	/w	/w	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_FRIEND_INVITE1	/????	0	0	0	0	0	/invite	/invite	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_FRIEND_INVITE2	/??	0	0	0	0	0	/friend	/friend	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_PARTY_INVITE2	/??	0	0	0	0	0	/party	/party	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_GUILD_INVITE1	/????	0	0	0	0	0	/join	/join	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_GUILD_INVITE2	/??	0	0	0	0	0	/guild	/guild	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_TRADE2	/??	0	0	0	0	0	/trade	/trade	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_BLOCK	/????	0	0	0	0	0	/ban	/ban	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_ADMISSION	/????	0	0	0	0	0	/unban	/unban	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_WHISPER_BLOCK	/?????	0	0	0	0	0	/mute	/mute	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_WHISPER_ADMISSION	/?????	0	0	0	0	0	/unmute	/unmute	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_FRIEND_INVITE	/????	0	0	0	0	0	/f	/f	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_GUILD_INVITE	/????	0	0	0	0	0	/g	/g	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_FRIEND_BLOCK	/??????	0	0	0	0	0	/BlockFriendInvitation	/BlockFriendInvitation	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_FRIEND_ADMISSION	/??????	0	0	0	0	0	/AcceptFriendInvitation	/AcceptFriendInvitation	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_GUILD_BLOCK	/??????	0	0	0	0	0	/BlockGuildInvitation	/BlockGuildInvitation	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_GUILD_ADMISSION	/??????	0	0	0	0	0	/AcceptGuildInvitation	/AcceptGuildInvitation	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_TRADE_BLOCK	/????	0	0	0	0	0	/BlockTradeInvitation	/BlockTradeInvitation	0	0	0	0	0	0
        1	UIIT_STT_CHAT_COMMAND_TRADE_ADMISSION	/????	0	0	0	0	0	/AcceptTradeInvitation	/AcceptTradeInvitation	0	0	0	0	0	0


        * delete the unnessery
        * send (friend) notice when /friend command is called
        * check on the bot if you can set value to chat directly so use it with adding item name to chat and adding commands to chat from bot
        * menthion in the theard the in game commands and the custom ones
        * Multi command /LeaveTheParty <name> or /pt and selected user and /pt <name>
        * Multi command /BanishFromParty <name> kick from party
        * add Apoint Leader
        * Multi command /Trade <name> /Exchange <name> or /exc and selected user and /exc <name>
        */
        public Guide()
        {
            InitializeComponent();
        }
    }
}
