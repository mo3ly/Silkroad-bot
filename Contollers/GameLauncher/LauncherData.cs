using SilkroadSecurityApi;
using SRO_INGAME.Common;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;

namespace SRO_INGAME.GameLauncher
{
    class LauncherData
    {
        /// <summary>
        /// Load all the luncher data and assign it to DataStorage memebers
        /// </summary>
        public static void LoadData()
        {
            // check if the game has a domain name instead of ip
            SRCommon.clientIP = GetClientIP();
            SRCommon.clientPort = GetClientPort();
            SRCommon.clientVersion = GetClientVersion();
        }

        /// <summary>
        /// Get Client IP
        /// </summary>
        /// <returns> string client ip </returns>
        public static string GetClientIP()
        {
            try
            {
                using (Stream stream = SRCommon.PK2.GetFileStream("DIVISIONINFO.TXT"))
                {
                    using (BinaryReader read = new BinaryReader(stream))
                    {
                        byte ContentID = read.ReadByte();
                        byte dCount = read.ReadByte();
                        for (int i = 0; i < dCount; i++)
                        {
                            string DivisionName = Encoding.GetEncoding(1252).GetString(read.ReadBytes(Convert.ToInt32(read.ReadUInt32())));
                            read.ReadByte(); //nullTerminator
                            for (int x = 0; x < read.ReadByte(); x++)
                                return Dns.GetHostAddresses(Encoding.GetEncoding(1252).GetString(read.ReadBytes(Convert.ToInt32(read.ReadUInt32()))))[0].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());//"Error while reading client IP.";
            }
            return null;
        }

        /// <summary>
        /// get client port
        /// </summary>
        /// <returns> int client port </returns>
        public static int GetClientPort()
        {
            try
            {
                using (Stream stream = SRCommon.PK2.GetFileStream("GATEPORT.TXT"))
                {
                    using (BinaryReader read = new BinaryReader(stream))
                    {
                        int x = 0, counter = 0;
                        byte[] newByteArray = new byte[(int)read.BaseStream.Length - 1];
                        while (counter < 5)
                        {
                            newByteArray[x++] = read.ReadByte();
                            counter++;
                        }
                        return Convert.ToInt32(Encoding.UTF8.GetString(newByteArray));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());//"Error while reading client port.";
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SVTContent"></param>
        /// <returns>int</returns>
        static int VersionFromSVT(byte[] SVTContent)
        {
            byte[] bfKey = ASCIIEncoding.ASCII.GetBytes("SILKROAD");
            Blowfish blowfish = new Blowfish();
            blowfish.Initialize(bfKey);

            byte[] toDecode = new byte[8];
            Buffer.BlockCopy(SVTContent, 4, toDecode, 0, 8);

            byte[] decoded = blowfish.Decode(toDecode);
            string verStr = Encoding.ASCII.GetString(decoded);

            int firstZeroTermAt = verStr.IndexOf('\0');
            verStr = verStr.Remove(firstZeroTermAt, verStr.Length - firstZeroTermAt);
            byte[] verStrBytes = Encoding.ASCII.GetBytes(verStr);
            return int.Parse(verStr);
        }

        /// <summary>
        /// get client version
        /// </summary>
        /// <returns> int client version </returns>
        public static int GetClientVersion()
        {
            try
            {
                using (Stream stream = SRCommon.PK2.GetFileStream("SV.T"))
                {
                    using (BinaryReader read = new BinaryReader(stream))
                    {
                        int length = (int)read.BaseStream.Length;
                        byte[] buffer = read.ReadBytes(length);
                        return VersionFromSVT(buffer);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());//"Error while reading version port.";
            }
            return 0;
        }
    }
}
