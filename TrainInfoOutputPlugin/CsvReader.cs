using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TrainInfoOutputPlugin
{
    public class CsvReader
    {
        //csvファイルの読み込み
        public string[, ] ReadCsv(string CsvPath, bool FillWithSharp)
        {
            var lines = File.ReadAllLines(CsvPath);

            int rowCount = lines.Length;
            int colCount = lines[0].Split(',').Length;

            string[, ] result = new string[rowCount, colCount];

            for(int i = 0; i < rowCount; i++)
            {
                string[] tmp1 = lines[i].Split(',');

                for(int j = 0; j< tmp1.Length; j++)
                {
                    string tmp2 = FixCsvFormat(tmp1[j]);

                    if(StartWithSharp(tmp2))
                    {
                        if (FillWithSharp)
                        {
                            for(int k = j; k < tmp1.Length; k++)
                            {
                                result[i, k] = "#";
                            }
                            break;
                        }
                        else
                        {
                            result[i, j] = "#";
                        }
                    }
                    else
                    {
                        result[i, j] = tmp2;
                    }
                }
            }

            return result;
        }

        //ダブルクォーテーション・空白の除去
        public string FixCsvFormat(string str)
        {
            if (str.StartsWith("\"") && str.EndsWith("\""))
            {
                str = str.Trim('"');
            }

            str = str.Replace(" ", "");

            return str;
        }

        //コメントアウト(#)の探索
        public bool StartWithSharp(string str)
        {
            bool result = false;

            if (str.StartsWith("#"))
            {
                result = true;
            }

            return result;
        }
    }
}
