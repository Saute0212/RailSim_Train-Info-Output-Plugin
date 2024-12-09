using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TrainInfoOutputPlugin
{
    public class Settings
    {
        string[] SettingsList = { "[Debug]", "[SerialOutput]", "[ComPort]", "[ComSpeed]",
                                  "[Cars]",  "[Location]", "[Speed]", "[Time]",
                                  "[BcPressure]", "[MrPressure]", "[ErPressure]", "[BpPressure]", "[SapPressure]", "[Current]",
                                  "[AtsP]", "[AtsPs]", "[AtsPf]", "[AtsSx]", "[Atc]", "[Eb]", "[ConstantSpeed]"}; //iniの設定項目
        
        //iniのファイルパス
        string IniPath = "./PluginSettings.ini";
        
        //プラグインのフォーマット
        public int PluginFormat = 0x00020000;
        
        //シリアル通信設定
        public string ComPort = "COM0";
        public int ComSpeed = 9600;

        /* 出力設定 */
        //デバッグ設定
        private bool SettingDebug = false;

        //シリアル通信
        private bool SettingSerialOutput = false;

        //車両情報
        private bool SettingCars = false;

        //車両状態
        private bool SettingLocation = false;
        private bool SettingSpeed = false;
        private bool SettingTime = false;
        private bool SettingBcPressure = false;
        private bool SettingMrPressure = false;
        private bool SettingErPressure = false;
        private bool SettingBpPressure = false;
        private bool SettingSapPressure = false;
        private bool SettingCurrent = false;

        //保安装置(ATS-P, ATS-Ps, ATS-PF, ATS-Sx, ATC, EB, 定速制御)
        private bool SettingAtsP = false;
        private bool SettingAtsPs = false;
        private bool SettingAtsPf = false;
        private bool SettingAtsSx = false;
        private bool SettingAtc = false;
        private bool SettingEb = false;
        private bool SettingConstantSpeed = false;

        //初期設定関数
        public void SetUp()
        {
            //iniファイルから設定項目を読み込む
            int len = IniLength(IniPath);
            foreach (string element in SettingsList)
            {
                if(element == "[ComPort]")
                {
                    ComPort = ReadIni(IniPath, element, len);
                }
                else if(element == "[ComSpeed]")
                {
                    ComSpeed = ConvertToInt(ReadIni(IniPath, element, len));
                }
                else
                {
                    SetConfig(element, ReadIni(IniPath, element, len));
                }
            }
        }

        //iniファイルの行数を取得
        private int IniLength(string IniPath)
        {
            int len;

            if (File.Exists(IniPath))
            {
                string[] lines = File.ReadAllLines(IniPath);
                len = lines.Length;
            }
            else
            {
                len = 0;
            }

            return len;
        }

        //iniファイルからtargetの設定を読み込む
        private string ReadIni(string IniPath, string target, int len)
        {
            string[] list = new string[len];
            int index = 0;
            string answer = "false";

            if (File.Exists(IniPath))
            {
                using (StreamReader reader = new StreamReader(IniPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        list[index] = line;
                        index++;
                    }
                }
            }

            for (int i = 0; i < len; i++)
            {
                if (list[i] == target)
                {
                    answer = list[i + 1];
                    break;
                }
            }

            return answer;
        }

        //設定項目を構造体に格納
        private void SetConfig(string target, string setting)
        {
            switch(target)
            {
                case "[Cars]":
                    SettingCars = SettingBool(setting);
                    break;
                case "[Location]":
                    SettingLocation = SettingBool(setting);
                    break;
                case "[Speed]":
                    SettingSpeed = SettingBool(setting);
                    break;
                case "[Time]":
                    SettingTime = SettingBool(setting);
                    break;
                case "[BcPressure]":
                    SettingBcPressure = SettingBool(setting);
                    break;
                case "[MrPressure]":
                    SettingMrPressure = SettingBool(setting);
                    break;
                case "[ErPressure]":
                    SettingErPressure = SettingBool(setting);
                    break;
                case "[BpPressure]":
                    SettingBpPressure = SettingBool(setting);
                    break;
                case "[SapPressure]":
                    SettingSapPressure = SettingBool(setting);
                    break;
                case "[Current]":
                    SettingCurrent = SettingBool(setting);
                    break;
                case "[AtsP]":
                    SettingAtsP = SettingBool(setting);
                    break;
                case "[AtsPs]":
                    SettingAtsPs = SettingBool(setting);
                    break;
                case "[AtsPf]":
                    SettingAtsPf = SettingBool(setting);
                    break;
                case "[AtsSx]":
                    SettingAtsSx = SettingBool(setting);
                    break;
                case "[Atc]":
                    SettingAtc = SettingBool(setting);
                    break;
                case "[Eb]":
                    SettingEb = SettingBool(setting);
                    break;
                case "[ConstantSpeed]":
                    SettingConstantSpeed = SettingBool(setting);
                    break;
                case "[Debug]":
                    SettingDebug = SettingBool(setting);
                    break;
                case "[SerialOutput]":
                    SettingSerialOutput = SettingBool(setting);
                    break;
                default:
                    break;
            }
        }

        //設定項目のtrueとflaseを判定
        private bool SettingBool(string setting)
        {
            if (setting == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //文字列をint型に変換
        private int ConvertToInt(string str)
        {
            bool flag = int.TryParse(str, out int result);

            if(!flag)
            {
                result = 0;
            }

            return result;
        }

        public bool SettingDebug_Bool
        {
            get { return SettingDebug; }
            set { SettingDebug = value; }
        }

        public bool SettingSerialOutput_Bool
        {
            get { return SettingSerialOutput; }
            set { SettingSerialOutput = value; }
        }

        public bool SettingCars_Bool
        {
            get { return SettingCars; }
            set { SettingCars = value; }
        }

        public bool SettingLocation_Bool
        {
            get { return SettingLocation; }
            set { SettingLocation = value; }
        }

        public bool SettingSpeed_Bool
        {
            get { return SettingSpeed; }
            set { SettingSpeed = value; }
        }

        public bool SettingTime_Bool
        {
            get { return SettingTime; }
            set { SettingTime = value; }
        }

        public bool SettingBcPressure_Bool
        {
            get { return SettingBcPressure; }
            set { SettingBcPressure = value; }
        }

        public bool SettingMrPressure_Bool
        {
            get { return SettingMrPressure; }
            set { SettingMrPressure = value; }
        }

        public bool SettingErPressure_Bool
        {
            get { return SettingErPressure; }
            set { SettingErPressure = value; }
        }

        public bool SettingBpPressure_Bool
        {
            get { return SettingBpPressure; }
            set { SettingBpPressure = value; }
        }

        public bool SettingSapPressure_Bool
        {
            get { return SettingSapPressure; }
            set { SettingSapPressure = value; }
        }

        public bool SettingCurrent_Bool
        {
            get { return SettingCurrent; }
            set { SettingCurrent = value; }
        }

        public bool SettingAtsP_Bool
        {
            get { return SettingAtsP; }
            set { SettingAtsP = value; }
        }

        public bool SettingAtsPs_Bool
        {
            get { return SettingAtsPs; }
            set { SettingAtsPs = value; }
        }

        public bool SettingAtsPf_Bool
        {
            get { return SettingAtsPf; }
            set { SettingAtsPf = value; }
        }

        public bool SettingAtsSx_Bool
        {
            get { return SettingAtsSx; }
            set { SettingAtsSx = value;}
        }

        public bool SettingAtc_Bool
        {
            get { return SettingAtc; }
            set { SettingAtc = value; }
        }

        public bool SettingEb_Bool
        {
            get { return SettingEb; }
            set { SettingEb = value; }
        }

        public bool SettingConstantSpeed_Bool
        {
            get { return SettingConstantSpeed; }
            set { SettingConstantSpeed = value; }
        }
    }
}
