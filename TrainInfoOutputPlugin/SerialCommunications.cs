using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace TrainInfoOutputPlugin
{
    public class SerialCommunications
    {
        //COMポート番号(Default:COM0)
        private static string port = "COM0";

        //シリアル通信速度(Default:9600bps)
        private static int speed = 9600;

        //選択したCOMポート
        private static SerialPort SelectedPort = null;

        //シリアル通信初期化
        public static void InitSerialCommunications(string ini_port, string ini_speed)
        {
            port = ini_port;
            SetComSpeed(ini_speed);
            OpenPort();
        }

        //COMポートを開く
        public static void OpenPort()
        {
            try
            {
                SelectedPort = new SerialPort(port, speed, Parity.None, 8, StopBits.One);
                SelectedPort.Open();
            }
            catch(Exception ex)
            {
                DebugFunction.DisplayString("ERROR : " + ex.Message);
            }
        }

        //データを送信(文字列)
        public static void SendData_String(string str)
        {
            if(SelectedPort.IsOpen)
            {
                try
                {
                    SelectedPort.Write(str + "\n");
                }
                catch(Exception ex)
                {
                    DebugFunction.DisplayString("ERROR : " + ex.Message);
                }
            }            
        }

        //データを送信(バイト列)
        public static void SendData_Byte(byte[] data)
        {
            if (SelectedPort.IsOpen)
            {
                try
                {
                    SelectedPort.Write(data, 0, data.Length);
                }
                catch(Exception ex)
                {
                    DebugFunction.DisplayString("ERROR : " + ex.Message);
                }
            }

        }

        //COMポートを閉じる
        public static void ClosePort()
        {
            if(SelectedPort != null)
            {
                try
                {
                    SelectedPort.Close();
                    SelectedPort = null;
                }
                catch(Exception ex)
                {
                    DebugFunction.DisplayString("ERROR : " + ex.Message);
                }
            }
        }

        //シリアル通信速度設定
        private static void SetComSpeed(string target)
        {
            switch(target)
            {
                case "300":
                    speed = 300;
                    break;
                case "600":
                    speed = 600;
                    break;
                case "1200":
                    speed = 1200;
                    break;
                case "2400":
                    speed = 2400;
                    break;
                case "4800":
                    speed = 4800;
                    break;
                case "9600":
                    speed = 9600;
                    break;
                case "14400":
                    speed = 14400;
                    break;
                case "19200":
                    speed = 19200;
                    break;
                case "38400":
                    speed = 38400;
                    break;
                case "57600":
                    speed = 57600;
                    break;
                case "115200":
                    speed = 115200;
                    break;
                default:
                    break;
            }
        }
    }
}
