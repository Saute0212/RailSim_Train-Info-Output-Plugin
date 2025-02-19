using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TrainInfoOutputPlugin
{
    public class Settings
    {
        //ini設定項目
        private static readonly string[] SettingsList = { "[Debug]", "[SerialOutput]", "[ComPort]", "[ComSpeed]",
                                  "[Cars]",  "[Location]", "[Speed]", "[Time]",
                                  "[BcPressure]", "[MrPressure]", "[ErPressure]", "[BpPressure]", "[SapPressure]", "[Current]",
                                  "[AtsP]", "[AtsPs]", "[AtsPf]", "[AtsSx]", "[Atc]", "[Eb]", "[ConstantSpeed]"}; //iniの設定項目
        
        //iniのファイルパス
        private static string IniPath = "./PluginSettings.ini";
        
        //プラグインのフォーマット
        public static int PluginFormat = 0x00020000;
        
        //シリアル通信設定
        public static string ComPort = "COM0";
        public static int ComSpeed = 9600;

        /* 出力設定 */
        //デバッグ設定
        private static bool SettingDebug = false;

        //シリアル通信
        private static bool SettingSerialOutput = false;

        //車両情報
        private static bool SettingCars = false;

        //車両状態
        private static bool SettingLocation = false;
        private static bool SettingSpeed = false;
        private static bool SettingTime = false;
        private static bool SettingBcPressure = false;
        private static bool SettingMrPressure = false;
        private static bool SettingErPressure = false;
        private static bool SettingBpPressure = false;
        private static bool SettingSapPressure = false;
        private static bool SettingCurrent = false;

        //保安装置(ATS-P, ATS-Ps, ATS-PF, ATS-Sx, ATC, EB, 定速制御)
        private static bool SettingAtsP = false;
        private static bool SettingAtsPs = false;
        private static bool SettingAtsPf = false;
        private static bool SettingAtsSx = false;
        private static bool SettingAtc = false;
        private static bool SettingEb = false;
        private static bool SettingConstantSpeed = false;

        //初期設定関数
        public static void SetUp()
        {
            //iniファイルから設定項目を読み込む
            int len = IniReader.IniLength(IniPath);
            foreach (string element in SettingsList)
            {
                if(element == "[ComPort]")
                {
                    ComPort = IniReader.ReadIni(IniPath, element, len);
                }
                else if(element == "[ComSpeed]")
                {
                    ComSpeed = ConvertToInt(IniReader.ReadIni(IniPath, element, len));
                }
                else
                {
                    SetConfig(element, IniReader.ReadIni(IniPath, element, len));
                }
            }
        }

        //設定項目を構造体に格納
        private static void SetConfig(string target, string setting)
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
        private static bool SettingBool(string setting)
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
        private static int ConvertToInt(string str)
        {
            bool flag = int.TryParse(str, out int result);

            if(!flag)
            {
                result = 0;
            }

            return result;
        }

        public static bool SettingDebug_Bool
        {
            get { return SettingDebug; }
            set { SettingDebug = value; }
        }

        public static bool SettingSerialOutput_Bool
        {
            get { return SettingSerialOutput; }
            set { SettingSerialOutput = value; }
        }

        public static bool SettingCars_Bool
        {
            get { return SettingCars; }
            set { SettingCars = value; }
        }

        public static bool SettingLocation_Bool
        {
            get { return SettingLocation; }
            set { SettingLocation = value; }
        }

        public static bool SettingSpeed_Bool
        {
            get { return SettingSpeed; }
            set { SettingSpeed = value; }
        }

        public static bool SettingTime_Bool
        {
            get { return SettingTime; }
            set { SettingTime = value; }
        }

        public static bool SettingBcPressure_Bool
        {
            get { return SettingBcPressure; }
            set { SettingBcPressure = value; }
        }

        public static bool SettingMrPressure_Bool
        {
            get { return SettingMrPressure; }
            set { SettingMrPressure = value; }
        }

        public static bool SettingErPressure_Bool
        {
            get { return SettingErPressure; }
            set { SettingErPressure = value; }
        }

        public static bool SettingBpPressure_Bool
        {
            get { return SettingBpPressure; }
            set { SettingBpPressure = value; }
        }

        public static bool SettingSapPressure_Bool
        {
            get { return SettingSapPressure; }
            set { SettingSapPressure = value; }
        }

        public static bool SettingCurrent_Bool
        {
            get { return SettingCurrent; }
            set { SettingCurrent = value; }
        }

        public static bool SettingAtsP_Bool
        {
            get { return SettingAtsP; }
            set { SettingAtsP = value; }
        }

        public static bool SettingAtsPs_Bool
        {
            get { return SettingAtsPs; }
            set { SettingAtsPs = value; }
        }

        public static bool SettingAtsPf_Bool
        {
            get { return SettingAtsPf; }
            set { SettingAtsPf = value; }
        }

        public static bool SettingAtsSx_Bool
        {
            get { return SettingAtsSx; }
            set { SettingAtsSx = value;}
        }

        public static bool SettingAtc_Bool
        {
            get { return SettingAtc; }
            set { SettingAtc = value; }
        }

        public static bool SettingEb_Bool
        {
            get { return SettingEb; }
            set { SettingEb = value; }
        }

        public static bool SettingConstantSpeed_Bool
        {
            get { return SettingConstantSpeed; }
            set { SettingConstantSpeed = value; }
        }
    }
}
