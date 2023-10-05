using System;
using SilkroadSecurityApi;
using SRO_INGAME.Common;

namespace SilkroadInformationAPI.Client.Packets.Gateway
{
    public class LoginResponse
    {

        public delegate void LoginResponseSuccessHandler(LoginResponseSuccessEventArgs AgentInfo);
        public static event LoginResponseSuccessHandler OnGatewayLoginResponseSuccess;

        public delegate void LoginResponseFailedHandler(LoginResponseFailedEventArgs FailureCode);
        public static event LoginResponseFailedHandler OnGatewayLoginResponseFailed;

        public static void Parse(Packet p)
        {
            byte result = p.ReadUInt8();
            if (result == 0x01)
            {
                uint session = p.ReadUInt32();
                string agentIP = p.ReadAscii();
                ushort agentPort = p.ReadUInt16();
                OnGatewayLoginResponseSuccess?.Invoke(new LoginResponseSuccessEventArgs(session, agentIP, agentPort));
            }
            else if (result == 0x02)
            {
                byte error = p.ReadUInt8();
                LoginErrorType errorCode = (LoginErrorType)error;
                var args = new LoginResponseFailedEventArgs(errorCode);
                if (error == 0x01)
                {
                    args.MaxAttempts = p.ReadUInt32();
                    args.CurrentAttempts = p.ReadUInt32();
                }
                else if (error == 0x02)
                {
                    byte blockType = p.ReadUInt8();
                    errorCode = (LoginErrorType)(100 + blockType);

                    if (errorCode == LoginErrorType.Banned)
                        args.BlockReason = p.ReadAscii();

                    args.ErrorCode = errorCode;
                }
                else if (error == 0x03) // by mo -> already connected
                {
                    args.ErrorCode = errorCode;
                }
                else if (error == 0x05) // full server -> TODO: make server make connection 5 and try
                {
                    args.ErrorCode = errorCode;
                } else // maybe wrong !!
                {
                    Console.WriteLine("Class: LoginResponse -> MaybeWrong");
                    SRCommon.game.StopConnection();
                    SRCommon.game.StartProxyConnection(SRCommon.clientIP, (ushort)SRCommon.botPort, false);
                }

                OnGatewayLoginResponseFailed?.Invoke(args);
            }
        }
    }

    public class LoginResponseSuccessEventArgs : EventArgs
    {
        public uint SessionID;
        public string AgentIP;
        public ushort AgentPort;

        public LoginResponseSuccessEventArgs(uint _session, string _ip, ushort _port)
        {
            SessionID = _session;
            AgentIP = _ip;
            AgentPort = _port;
        }
    }
    public class LoginResponseFailedEventArgs : EventArgs
    {
        public LoginErrorType ErrorCode;

        /// <summary>
        /// This is only available in case of error being wrong password.
        /// </summary>
        public uint MaxAttempts;

        /// <summary>
        /// This is only available in case of error being wrong password.
        /// </summary>
        public uint CurrentAttempts;

        /// <summary>
        /// This is only available in case of error being punishment.
        /// </summary>
        public string BlockReason;

        public LoginResponseFailedEventArgs(LoginErrorType error)
        {
            ErrorCode = error;
        }
    }
}
