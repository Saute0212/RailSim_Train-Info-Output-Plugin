﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TrainInfoOutputPlugin
{
    public class DebugFunction
    {
        //コンソール用Dllをインポート
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        //コンソールの状態
        public static bool isConsoleOpen = false;

        //コンソールの初期化
        public static void InitConsole()
        {
            ShowConsole();
            DisplayString("The debug console has been launched.");
        }

        //コンソールを表示
        public static void ShowConsole()
        {
            if(!isConsoleOpen)
            {
                try
                {
                    AllocConsole();
                    isConsoleOpen = true;
                }
                catch
                {
                }
            }
        }

        //文字列を表示
        public static void DisplayString(string str)
        {
            if(isConsoleOpen)
            {
                try
                {
                    Console.WriteLine(str);
                }
                catch
                {
                }
            }
        }

        //Byte列を表示
        public static void DisplayBytes(string str, byte[] data)
        {
            if(isConsoleOpen)
            {
                try
                {
                    Console.WriteLine(str + ":" + BitConverter.ToString(data));
                }
                catch
                {
                }
            }
        }

        //コンソールを閉じる
        public static void CloseConsole()
        {
            if (isConsoleOpen)
            {
                try
                {
                    FreeConsole();
                    isConsoleOpen= false;
                }
                catch
                {
                }
            }
        }
    }
}
