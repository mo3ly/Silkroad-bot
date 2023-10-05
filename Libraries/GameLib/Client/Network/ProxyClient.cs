using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SilkroadSecurityApi;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Windows;
using SRO_INGAME.Common;

namespace SilkroadInformationAPI.Client.Network
{
    public class ProxyClient
    {
        public static event Action OnProxyClientlessStart;
        public static event Action<Packet> OnProxyServerPacketReceive;
        public static event Action<Packet> OnProxyClientPacketSent;
        public static event Action OnProxyDisconnect;

        class Context
        {
            public Socket Socket { get; set; }
            public Security Security { get; set; }
            public TransferBuffer Buffer { get; set; }
            public Security RelaySecurity { get; set; }

            public Context()
            {
                Socket = null;
                Security = new Security();
                RelaySecurity = null;
                Buffer = new TransferBuffer(8192);
            }
        }

        static bool ConnectedToWorldwideServer = false;
        public static bool AutoSwitchToClientless = false;

        static uint SessionID;
        static string Username;
        static string Password;
        static byte Locale;


        public static void StartProxy(string remote_ip, ushort remote_port, string local_ip, ushort local_port) //Pushedx proxy.
        {
            Client.RefreshClient();
            ConnectedToWorldwideServer = false;

            Context local_context = new Context();
            local_context.Security.GenerateSecurity(true, true, true);

            Context remote_context = new Context();

            SroClient.RemoteSecurity = remote_context.Security;
            SroClient.LocalSecurity = local_context.Security;

            remote_context.RelaySecurity = local_context.Security;
            local_context.RelaySecurity = remote_context.Security;

            List<Context> contexts = new List<Context>();
            contexts.Add(local_context);
            contexts.Add(remote_context);

            using (Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                server.Bind(new IPEndPoint(IPAddress.Parse(local_ip), local_port));
                server.Listen(1);

                local_context.Socket = server.Accept();
            }

            using (local_context.Socket)
            {
                using (remote_context.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    remote_context.Socket.Connect(remote_ip, remote_port);
                    while (true)
                    {

                        #region Transfer Incomming
                        foreach (Context context in contexts) // Network input event processing
                        {
                            try
                            {
                                if (context.Socket.Poll(0, SelectMode.SelectRead))
                                {
                                    int count = context.Socket.Receive(context.Buffer.Buffer);
                                    if (count == 0)
                                    {
                                        Console.WriteLine("Null recevied, disconnected");
                                        OnProxyDisconnect?.Invoke();
                                        throw new Exception("The remote connection has been lost.");
                                    }
                                    context.Security.Recv(context.Buffer.Buffer, 0, count);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("[TransferIncoming] Error: " + ex.Message);
                                //If local context disconnects && the account is connected to agent server > switch clientless
                                /*if (context == local_context && ConnectedToWorldwideServer == true && AutoSwitchToClientless)
                                {
                                    OnProxyClientlessStart?.Invoke();
                                    Clientless_Start(remote_context);
                                }*/
                                // reconnect
                                SRCommon.game.StopConnection();
                                SRCommon.game.StartProxyConnection(SRCommon.clientIP, (ushort)SRCommon.botPort, false);
                                return;
                            }
                        }

                        foreach (Context context in contexts) // Logic event processing
                        {
                            List<Packet> packets = context.Security.TransferIncoming();
                            if (packets != null)
                            {
                                foreach (Packet packet in packets)
                                {
                                    Dispatcher.Process(new Packet(packet));
                                    OnProxyServerPacketReceive?.Invoke(new Packet(packet));

                                    //custom global
                                    //if (packet.Opcode == 0x3026)
                                    //continue;

                                    //
                                    bool inBattleRoyal = true;
                                    if (inBattleRoyal)
                                    {
                                        // skip the guild data and academy data in Battle royal
                                        //if (new UInt32[] { 0x3102, 0x34B3 , 0x3101 , 0x34B4 }.Contains(packet.Opcode))
                                        if (packet.Opcode == 0x3101)
                                            SroClient.SystemMessage($"(Guild) packet has been received");

                                        if (new uint[] { 0x30CD, 0x30CE, 0x30D3 }.Contains(packet.Opcode))  // skip pk packets
                                        {
                                            // cancel purchaing any items when playing battle royale
                                            SroClient.SystemMessage($"(PK) packet has been skipped"); // 30D2, 30CE
                                            continue;
                                        }

                                        if(packet.Opcode == 0xB516)
                                        {
                                           // SroClient.SystemMessage($"(PVP) Packet has been skipped");
                                           // continue;
                                        }

                                    }

                                    // custom commands
                                    if (packet.Opcode == 0x7608)
                                        continue;

                                    if (packet.Opcode == 0x3080) // block party and exc
                                    {
                                        uint flag = packet.ReadUInt8();
                                        if (flag == 0x1) // exchange
                                        {
                                            if (packet.GetBytes().Length == 5)
                                            {
                                                uint sUniqueID = packet.ReadUInt32();
                                                if (SRCommon.SessionData.blockedExchange.ContainsKey(sUniqueID))
                                                {
                                                    SroClient.RejectExchange(sUniqueID);
                                                    SroClient.SystemMessage($"(Exchange) has rejected from blocked user {SRCommon.SessionData.blockedExchange[sUniqueID]}.");
                                                }
                                            }
                                            else
                                                packet.ReadUInt8();

                                        }
                                        else if (flag == 0x2)
                                        { //party
                                            if (packet.GetBytes().Length == 6)
                                            {
                                                uint sUniqueID = packet.ReadUInt32();
                                                if (SRCommon.SessionData.blockedParty.ContainsKey(sUniqueID))
                                                {
                                                    SroClient.RejectParty();
                                                    SroClient.SystemMessage($"(Party) has rejected from blocked user {SRCommon.SessionData.blockedParty[sUniqueID]}.");
                                                    continue;
                                                }
                                                packet.ReadUInt8();
                                            }
                                            else
                                                packet.ReadUInt16();
                                        }
                                    }

                                    if (packet.Opcode == 0xB001)
                                        ConnectedToWorldwideServer = true;

                                    // filter chat for commands
                                    /*if (packet.Opcode == 0x7025)
                                    {
                                        packet.ReadUInt8();
                                        packet.ReadUInt8();
                                        if (packet.ReadAscii().StartsWith("\\"))
                                            continue;
                                    }*/

                                    if (packet.Opcode == 0x5000 || packet.Opcode == 0x9000) // ignore always
                                    {
                                    }
                                    else if (packet.Opcode == 0x2001)
                                    {
                                        if (context == remote_context) // ignore local to proxy only
                                        {
                                            context.RelaySecurity.Send(packet); // proxy to remote is handled by API
                                        }
                                    }
                                    else if (packet.Opcode == 0xA102)
                                    {
                                        // add this to dispatcher
                                        //Show second pass // maybe it won't work in login screen because of the estimated time the so you can make it after login
                                        //Application.Current.Dispatcher.Invoke((Action)delegate {
                                        //new vBOT_WPF.View.SecondPass.SecondPassWindow().Show();
                                        //});
                                        byte result = packet.ReadUInt8();
                                        if (result == 1)
                                        {
                                            SessionID = packet.ReadUInt32();
                                            string ip = packet.ReadAscii();
                                            ushort port = packet.ReadUInt16();

                                            var agentThread = new Thread(() => StartProxy(ip, ushort.Parse((port).ToString()), local_ip, local_port));
                                            agentThread.Start();

                                            Thread.Sleep(500);

                                            //Fake response to redirect the client.
                                            Packet new_packet = new Packet(0xA102, true);
                                            new_packet.WriteUInt8(result);
                                            new_packet.WriteUInt32(SessionID);
                                            new_packet.WriteAscii(local_ip);
                                            new_packet.WriteUInt16(local_port);

                                            context.RelaySecurity.Send(new_packet);
                                        }
                                        else
                                        {
                                            context.RelaySecurity.Send(packet);
                                        }
                                    }
                                    else if (packet.Opcode == 0x6103)
                                    {
                                        //If the client is sending 0x6103 agent auth packet, don't relay it
                                        //because we send our own.
                                    }
                                    else
                                    {
                                        context.RelaySecurity.Send(packet);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Transfer Outgoing
                        foreach (Context context in contexts) // Network output event processing
                        {
                            if (context.Socket.Poll(0, SelectMode.SelectWrite))
                            {
                                List<KeyValuePair<TransferBuffer, Packet>> buffers = context.Security.TransferOutgoing();
                                if (buffers != null)
                                {
                                    foreach (KeyValuePair<TransferBuffer, Packet> kvp in buffers)
                                    {
                                        TransferBuffer buffer = kvp.Key;
                                        OnProxyClientPacketSent?.Invoke(kvp.Value);
                                        Console.WriteLine(string.Format("[{0}][{1:X4}][{2} bytes]{3}{4}{6}{5}{6}", context == local_context ? "S->C" : "C->S", kvp.Value.Opcode, kvp.Value.GetBytes().Length, kvp.Value.Encrypted ? "[Encrypted]" : "", kvp.Value.Massive ? "[Massive]" : "", SilkroadSecurityApi.Utility.HexDump(kvp.Value.GetBytes()), Environment.NewLine));

                                        // filter chat for commands
                                        /*if (kvp.Value.Opcode == 0x7025)
                                        {
                                            var p = new Packet(kvp.Value);
                                            p.ReadUInt8();
                                            p.ReadUInt8();
                                            if (p.ReadAscii().StartsWith("\\"))
                                                continue;
                                        }*/

                                        //Save locale, username, password
                                        if (kvp.Value.Opcode == 0x6102)
                                        {
                                            var p = new Packet(kvp.Value);
                                            Locale = p.ReadUInt8();
                                            Username = p.ReadAscii();
                                            Password = p.ReadAscii();
                                        }

                                        //if the remote server signals switching to agent, send the auth packet.
                                        else if (kvp.Value.Opcode == 0x2001)
                                            if (kvp.Value.ReadAscii() == "AgentServer")
                                                AgentAuthRequest.Send(SessionID, Username, Password, Locale);

                                        while (true)
                                        {
                                            int count = context.Socket.Send(buffer.Buffer, buffer.Offset, buffer.Size, SocketFlags.None);
                                            buffer.Offset += count;
                                            if (buffer.Offset == buffer.Size)
                                            {
                                                break;
                                            }
                                            Thread.Sleep(1);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        Thread.Sleep(1); //prevent 100% CPU usage
                    }
                }
            }
        }

        #region Clienless
        static void Clientless_Start(Context context)
        {
            //Clientless started, start the ping thread.
            var keepalive = new Thread(() => Clientless_Ping(context));
            keepalive.Start();

            try
            {
                while (true)
                {
                    if (context.Socket.Poll(0, SelectMode.SelectRead))
                    {
                        int count = context.Socket.Receive(context.Buffer.Buffer);
                        if (count == 0)
                        {
                            Console.WriteLine("Null received, Disconnected!");
                            OnProxyDisconnect?.Invoke();
                        }
                        context.Security.Recv(context.Buffer.Buffer, 0, count);
                    }

                    List<Packet> packets = context.Security.TransferIncoming();
                    if (packets != null)
                    {
                        foreach (Packet packet in packets)
                        {
                            Dispatcher.Process(new Packet(packet));
                            OnProxyServerPacketReceive?.Invoke(packet);

                            if (packet.Opcode == 0x34B5) //Character teleport successfully
                            {
                                Packet answer = new Packet(0x34B6); //Teleport confirmation packet
                                context.Security.Send(answer);
                            }
                        }
                    }

                    if (context.Socket.Poll(0, SelectMode.SelectWrite))
                    {
                        List<KeyValuePair<TransferBuffer, Packet>> buffers = context.Security.TransferOutgoing();
                        if (buffers != null)
                        {
                            foreach (KeyValuePair<TransferBuffer, Packet> kvp in buffers)
                            {
                                TransferBuffer buffer = kvp.Key;
                                OnProxyClientPacketSent?.Invoke(kvp.Value);
                                while (true)
                                {
                                    int count = context.Socket.Send(buffer.Buffer, buffer.Offset, buffer.Size, SocketFlags.None);
                                    buffer.Offset += count;
                                    if (buffer.Offset == buffer.Size)
                                    {
                                        break;
                                    }
                                    Thread.Sleep(1);
                                }
                            }
                        }
                    }

                }

            }
            catch
            {
                Console.WriteLine("Disconnected!");
                OnProxyDisconnect?.Invoke();
            }
        }
        #endregion

        static void Clientless_Ping(Context context)
        {
            while (true)
            {
                if (context.Socket.Connected == false)
                    break;
                Packet p = new Packet(0x2002);
                context.Security.Send(p);
                Thread.Sleep(5000);
            }
        }

    }
}
