using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TrainInfoOutputPlugin
{
    public class IniReader
    {
        //iniファイルの行数を取得
        public static int IniLength(string IniPath)
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
        public static string ReadIni(string IniPath, string target, int len)
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
    }
}
