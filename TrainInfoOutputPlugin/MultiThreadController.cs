using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TrainInfoOutputPlugin
{
    public class MultiThreadController
    {
        //スレッドの状態を管理する変数
        private static bool thread1_IsRunning = false;
        private static bool thread2_IsRunning = false;
        private static bool thread3_IsRunning = false;
        private static bool thread4_IsRunning = false;
        private static bool thread5_IsRunning = false;
        private static bool thread6_IsRunning = false;
        private static bool thread7_IsRunning = false;

        //スレッドのID
        public const int Thread_SerialCommunications = 1;
        public const int Thread_DebugFunction = 2;
        public const int Thread_PlayMusic = 3;
        public const int Thread_AtsController = 4;
        public const int Thread_AtsInfoOutput = 5;
        public const int Thread_VehicleInfoOutput = 6;
        public const int Thread_DataConverter = 7;

        //スレッドの最大個数
        private static int MaxThreads = 7;

        //指定されたスレッドを立ち上げる関数
        public static void TurnOnThread(int threadId)
        {
            if (threadId <= 0 || threadId > MaxThreads)
            {
                DebugFunction.DisplayString("ERROR : threadId is not correct."); //エラー処理
                return;
            }

            try
            {
                switch (threadId)
                {
                    case Thread_SerialCommunications:
                        if(!thread1_IsRunning)
                        {
                            thread1_IsRunning = true;
                            Thread thread1 = new Thread(() => Run_SerialCommunications());
                            thread1.IsBackground = true;
                            thread1.Start();
                        }
                        break;
                    case Thread_DebugFunction:
                        if(!thread2_IsRunning)
                        {
                            thread2_IsRunning = true;
                            Thread thread2 = new Thread(() => Run_DebugFunction());
                            thread2.IsBackground = true;
                            thread2.Start();
                        }
                        break;
                    case Thread_PlayMusic:
                        if(!thread3_IsRunning)
                        {
                            thread3_IsRunning = true;
                            Thread thread3 = new Thread(() => Run_PlayMusic());
                            thread3.IsBackground = true;
                            thread3.Start();
                        }
                        break;
                    case Thread_AtsController:
                        if(!thread4_IsRunning)
                        {
                            thread4_IsRunning = true;
                            Thread thread4 = new Thread(() => Run_AtsController());
                            thread4.IsBackground = true;
                            thread4.Start();
                        }
                        break;
                    case Thread_AtsInfoOutput:
                        if(!thread5_IsRunning)
                        {
                            thread5_IsRunning = true;
                            Thread thread5 = new Thread(() => Run_AtsInfoOutput());
                            thread5.IsBackground = true;
                            thread5.Start();
                        }
                        break;
                    case Thread_VehicleInfoOutput:
                        if(!thread6_IsRunning)
                        {
                            thread6_IsRunning = true;
                            Thread thread6 = new Thread(() => Run_VehicleInfoOutput());
                            thread6.IsBackground = true;
                            thread6.Start();
                        }
                        break;
                    case Thread_DataConverter:
                        if(!thread7_IsRunning)
                        {
                            thread7_IsRunning = true;
                            Thread thread7 = new Thread(() => Run_DataConverter());
                            thread7.IsBackground = true;
                            thread7.Start();
                        }
                        break;
                }
            }
            catch
            {
                DebugFunction.DisplayString("ERROR : Could not turn on thread."); //エラー処理
            }
        }

        //指定されたスレッドを終了する関数
        public static void TurnOffThread(int threadId)
        {
            if (threadId <= 0 || threadId > MaxThreads)
            {
                DebugFunction.DisplayString("ERROR : threadId is not correct."); //エラー処理
                return;
            }

            try
            {
                switch(threadId)
                {
                    case Thread_SerialCommunications:
                        thread1_IsRunning = false;
                        break;
                    case Thread_DebugFunction:
                        thread2_IsRunning = false;
                        break;
                    case Thread_PlayMusic:
                        thread3_IsRunning = false;
                        break;
                    case Thread_AtsController:
                        thread4_IsRunning = false;
                        break;
                    case Thread_AtsInfoOutput:
                        thread5_IsRunning = false;
                        break;
                    case Thread_VehicleInfoOutput:
                        thread6_IsRunning = false;
                        break;
                    case Thread_DataConverter:
                        thread7_IsRunning = false;
                        break;
                }
            }
            catch
            {
                DebugFunction.DisplayString("ERROR : Could not turn off thread."); //エラー処理
            }
        }

        //指定されたスレッドの状態をreturnする関数
        public static bool IsThreadRunning(int threadId)
        {
            bool result = false;

            switch(threadId)
            {
                case Thread_SerialCommunications:
                    result = thread1_IsRunning;
                    break;
                case Thread_DebugFunction:
                    result = thread2_IsRunning;
                    break;
                case Thread_PlayMusic:
                    result = thread3_IsRunning;
                    break;
                case Thread_AtsController:
                    result = thread4_IsRunning;
                    break;
                case Thread_AtsInfoOutput:
                    result = thread5_IsRunning;
                    break;
                case Thread_VehicleInfoOutput:
                    result = thread6_IsRunning;
                    break;
                case Thread_DataConverter:
                    result = thread7_IsRunning;
                    break;
            }

            return result;
        }

        //各スレッドの処理
        private static void Run_SerialCommunications()
        {
            while(IsThreadRunning(Thread_SerialCommunications))
            {
                byte[] reset = new byte[1];
                reset[0] = 0x00;
                byte[] tmp = DataBuffer.SerialCommunications.ReadData(DataBuffer.SerialCommunications.ReadIndex());
                SerialCommunications.SendData_Byte(tmp); //データの送信
                SerialCommunications.SendData_Byte(reset); //リセット信号送信
                Thread.Sleep(500);
            }
        }

        private static void Run_DebugFunction()
        {
            while(IsThreadRunning(Thread_DebugFunction))
            {
                string tmp = DataBuffer.DebugFunction.ReadData(DataBuffer.DebugFunction.ReadIndex());
                DebugFunction.DisplayString(tmp);
                Thread.Sleep(500);
            }
        }

        private static void Run_PlayMusic()
        {
            while(IsThreadRunning(Thread_PlayMusic))
            {
                string[] tmp = DataBuffer.PlayMusic.ReadData(DataBuffer.PlayMusic.ReadIndex());
                Thread.Sleep(500);
            }
        }

        private static void Run_AtsController()
        {
            while(IsThreadRunning(Thread_AtsController))
            {
                Thread.Sleep(500);
            }
        }

        private static void Run_AtsInfoOutput()
        {
            while(IsThreadRunning(Thread_AtsInfoOutput))
            {
                Thread.Sleep(500);
            }
        }

        private static void Run_VehicleInfoOutput()
        {
            while(IsThreadRunning(Thread_VehicleInfoOutput))
            {
                Thread.Sleep(500);
            }
        }

        private static void Run_DataConverter()
        {
            while(IsThreadRunning(Thread_DataConverter))
            {
                Thread.Sleep(500);
            }
        }
    }
}
