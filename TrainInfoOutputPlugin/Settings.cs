using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TrainInfoOutputPlugin
{
    public class Settings
    {
        private static OutputSetting outputConfig;

        string[] SettingsList = { "[Debug]", "[ComPort]", "[ComSpeed]",
                                  "[Cars]",  "[Location]", "[Speed]", "[Time]",
                                  "[BcPressure]", "[MrPressure]", "[ErPressure]", "[BpPressure]", "[SapPressure]", "[Current]",
                                  "[AtsP]", "[AtsPs]", "[AtsPf]", "[AtsSx]", "[Atc]", "[Eb]", "[ConstantSpeed]"}; //iniの設定項目
        string IniPath = "./PluginSettings.ini";
        public int PluginFormat = 0x00020000;
        public string ComPort = "COM0";
        public int ComSpeed = 9600;

        //出力設定
        public struct OutputSetting
        {
            //デバッグ設定
            public bool SettingDebug;

            //車両情報
            public bool SettingCars;

            //車両状態
            public bool SettingLocation;
            public bool SettingSpeed;
            public bool SettingTime;
            public bool SettingBcPressure;
            public bool SettingMrPressure;
            public bool SettingErPressure;
            public bool SettingBpPressure;
            public bool SettingSapPressure;
            public bool SettingCurrent;

            //保安装置(ATS-P, ATS-Ps, ATS-PF, ATS-Sx, ATC, EB, 定速制御)
            public bool SettingAtsP;
            public bool SettingAtsPs;
            public bool SettingAtsPf;
            public bool SettingAtsSx;
            public bool SettingAtc;
            public bool SettingEb;
            public bool SettingConstantSpeed;

            //初期化
            public OutputSetting(bool ResetSetting)
            {
                SettingDebug = ResetSetting;
                SettingCars = ResetSetting;
                SettingLocation = ResetSetting;
                SettingSpeed = ResetSetting;
                SettingTime = ResetSetting;
                SettingBcPressure = ResetSetting;
                SettingMrPressure = ResetSetting;
                SettingErPressure = ResetSetting;
                SettingBpPressure = ResetSetting;
                SettingSapPressure = ResetSetting;
                SettingCurrent = ResetSetting;
                SettingAtsP = ResetSetting;
                SettingAtsPs = ResetSetting;
                SettingAtsPf = ResetSetting;
                SettingAtsSx = ResetSetting;
                SettingAtc = ResetSetting;
                SettingEb = ResetSetting;
                SettingConstantSpeed = ResetSetting;
            }
        }

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
                    outputConfig.SettingCars = SettingBool(setting);
                    break;
                case "[Location]":
                    outputConfig.SettingLocation = SettingBool(setting);
                    break;
                case "[Speed]":
                    outputConfig.SettingSpeed = SettingBool(setting);
                    break;
                case "[Time]":
                    outputConfig.SettingTime = SettingBool(setting);
                    break;
                case "[BcPressure]":
                    outputConfig.SettingBcPressure = SettingBool(setting);
                    break;
                case "[MrPressure]":
                    outputConfig.SettingMrPressure = SettingBool(setting);
                    break;
                case "[ErPressure]":
                    outputConfig.SettingErPressure = SettingBool(setting);
                    break;
                case "[BpPressure]":
                    outputConfig.SettingBpPressure = SettingBool(setting);
                    break;
                case "[SapPressure]":
                    outputConfig.SettingSapPressure = SettingBool(setting);
                    break;
                case "[Current]":
                    outputConfig.SettingCurrent = SettingBool(setting);
                    break;
                case "[AtsP]":
                    outputConfig.SettingAtsP = SettingBool(setting);
                    break;
                case "[AtsPs]":
                    outputConfig.SettingAtsPs = SettingBool(setting);
                    break;
                case "[AtsPf]":
                    outputConfig.SettingAtsPf = SettingBool(setting);
                    break;
                case "[AtsSx]":
                    outputConfig.SettingAtsSx = SettingBool(setting);
                    break;
                case "[Atc]":
                    outputConfig.SettingAtc = SettingBool(setting);
                    break;
                case "[Eb]":
                    outputConfig.SettingEb = SettingBool(setting);
                    break;
                case "[ConstantSpeed]":
                    outputConfig.SettingConstantSpeed = SettingBool(setting);
                    break;
                case "[Debug]":
                    outputConfig.SettingDebug = SettingBool(setting);
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
    }
}
