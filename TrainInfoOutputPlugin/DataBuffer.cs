using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrainInfoOutputPlugin
{
    public class DataBuffer
    {
        //ATS_VEHICLESTATE, ATS_HANDLESの統合バッファ
        public struct ATS_IntegratedData
        {
            //ATS_VEHICLESTATE
            public double Location; //車両位置(m)
            public float Speed; //車両速度(km/h)
            public int Time; //時刻(ms)
            public float BcPressure; //ブレーキシリンダ圧力(Pa)
            public float MrPressure; //元空気ダメ圧力(Pa)
            public float ErPressure; //釣り合い空気ダメ圧力(Pa)
            public float BpPressure; //ブレーキ管圧力(Pa)
            public float SapPressure; //直通管圧力(Pa)
            public float Current; //電流値(A)

            //ATS_HANDLES
            public int Brake; //ブレーキノッチ
            public int Power; //力行ノッチ
            public int Reverser; //レバーサー位置
            public int ConstantSpeed; //定速制御の状態
        }

        //ATS_OutputBuffer
        public struct ATS_OutputData
        {
            public int Id;
            public int Interruption;
            public int DataPriority;
            public int DataFormat;
            public int ParityBitSetting;
            public int DataType;
            public int DataLength;
            public object Data;
            public int ParityBit;
        }

        //シリアル出力用統合バッファ
        public struct IntegratedInfomationData
        {
            public int Id;
            public int Interruption;
            public int DataPriority;
            public int DataFormat;
            public int ParityBitSetting;
            public int DataType;
            public int DataLength;
            public object Data;
            public int ParityBit;
        }

        //車両状態バッファ
        public class VehicleStateBuffer
        {
            //バッファの設定
            private const int BufSize = 8; //2の累乗
            private static int ReadIndexNum;
            private static int WriteIndexNum;

            //ATS_IntegratedDataのバッファ
            private static ATS_IntegratedData[] buffers = new ATS_IntegratedData[BufSize];

            //データの有無
            private static bool[] ValidData = new bool[BufSize];

            //ロック機構
            private static object[] BufferLocks = new object[BufSize];
            private static object ReadIndexLock = new object();
            private static object WriteIndexLock = new object();

            //バッファを初期化
            public static void SetupBuffer()
            {
                lock (ReadIndexLock)
                {
                    ReadIndexNum = 0;
                }

                lock (WriteIndexLock)
                {
                    WriteIndexNum = 0;
                }

                buffers = new ATS_IntegratedData[BufSize];
                for (int i = 0; i < BufSize; i++)
                {
                    buffers[i] = new ATS_IntegratedData();
                    BufferLocks[i] = new object();
                    ValidData[i] = false;
                }
            }

            //バッファにデータがあるかreturn
            public static bool ValidDataExist()
            {
                lock (BufferLocks)
                {
                    return ValidData.Any(v => v);
                }
            }

            //読み出す行をreturn
            public static int ReadIndex()
            {
                lock (ReadIndexLock)
                {
                    int tmp = ReadIndexNum;
                    if (tmp == BufSize)
                    {
                        ReadIndexNum = 0;
                        tmp = 0;
                    }

                    ReadIndexNum++;

                    return tmp;
                }
            }

            //書き込む行をreturn
            public static int WriteIndex()
            {
                lock (WriteIndexLock)
                {
                    int tmp = WriteIndexNum;
                    if (tmp == BufSize)
                    {
                        WriteIndexNum = 0;
                        tmp = 0;
                    }

                    WriteIndexNum++;

                    return tmp;
                }
            }

            //指定した行からデータを読み出す
            public static ATS_IntegratedData? ReadData(int line)
            {
                if (line < 0 || line >= BufSize)
                {
                    return null;
                }

                lock (BufferLocks[line])
                {
                    if (!ValidData[line])
                    {
                        return null;
                    }

                    ValidData[line] = false;
                    return buffers[line];
                }
            }

            //指定した行にデータを書き込む
            public static void WriteData(int line, ATS_VEHICLESTATE vehicleState, ATS_HANDLES handles)
            {
                if (line < 0 || line >= BufSize)
                {
                    return;
                }

                lock (BufferLocks[line])
                {
                    ATS_IntegratedData tmp = new ATS_IntegratedData();

                    tmp.Location = vehicleState.Location;
                    tmp.Speed = vehicleState.Speed;
                    tmp.Time = vehicleState.Time;
                    tmp.BcPressure = vehicleState.BcPressure;
                    tmp.MrPressure = vehicleState.MrPressure;
                    tmp.ErPressure = vehicleState.ErPressure;
                    tmp.BpPressure = vehicleState.BpPressure;
                    tmp.SapPressure = vehicleState.SapPressure;
                    tmp.Current = vehicleState.Current;

                    tmp.Brake = handles.Brake;
                    tmp.Power = handles.Power;
                    tmp.Reverser = handles.Reverser;
                    tmp.ConstantSpeed = handles.ConstantSpeed;

                    buffers[line] = tmp;
                    ValidData[line] = true;
                }
            }
        }

        //ATS Controller用バッファ
        public class AtsControllerBuffer
        {
            //バッファの設定
            private const int BufSize = 8; //2の累乗
            private static int ReadIndexNum_IntegratedData;
            private static int WriteIndexNum_IntegratedData;
            private static int ReadIndexNum_BeaconData;
            private static int WriteIndexNum_BeaconData;

            //ATS_IntegratedDataのバッファ
            private static ATS_IntegratedData[] buffers_IntegratedData = new ATS_IntegratedData[BufSize];
            private static ATS_BEACONDATA[] buffers_BeaconData = new ATS_BEACONDATA[BufSize];

            //データの有無
            private static bool[] ValidData_IntegratedData = new bool[BufSize];
            private static bool[] ValidData_BeaconData = new bool[BufSize];

            //ロック機構
            private static object[] BufferLocks_IntegratedData = new object[BufSize];
            private static object[] BufferLocks_BeaconData = new object[BufSize];
            private static object ReadIndexLock_IntegratedData = new object();
            private static object ReadIndexLock_BeaconData = new object();
            private static object WriteIndexLock_IntegratedData = new object();
            private static object WriteIndexLock_BeaconData = new object();

            //バッファを初期化
            public static void SetupBuffer()
            {
                lock (ReadIndexLock_IntegratedData)
                {
                    ReadIndexNum_IntegratedData = 0;
                }

                lock (ReadIndexLock_BeaconData)
                {
                    ReadIndexNum_BeaconData = 0;
                }

                lock (WriteIndexLock_IntegratedData)
                {
                    WriteIndexNum_IntegratedData = 0;
                }

                lock (WriteIndexLock_BeaconData)
                {
                    WriteIndexNum_BeaconData = 0;
                }

                buffers_IntegratedData = new ATS_IntegratedData[BufSize];
                buffers_BeaconData = new ATS_BEACONDATA[BufSize];

                for (int i = 0; i < BufSize; i++)
                {
                    buffers_IntegratedData[i] = new ATS_IntegratedData();
                    buffers_BeaconData[i] = new ATS_BEACONDATA();
                    BufferLocks_IntegratedData[i] = new object();
                    BufferLocks_BeaconData[i] = new object();
                    ValidData_IntegratedData[i] = false;
                    ValidData_BeaconData[i] = false;
                }
            }

            //バッファにデータがあるかreturn
            public static bool ValidDataExist_IntegratedData()
            {
                lock (BufferLocks_IntegratedData)
                {
                    return ValidData_IntegratedData.Any(v => v);
                }
            }

            public static bool ValidDataExist_BeaconData()
            {
                lock (BufferLocks_BeaconData)
                {
                    return ValidData_BeaconData.Any(v => v);
                }
            }

            //読み出す行をreturn
            public static int ReadIndex_IntegratedData()
            {
                lock (ReadIndexLock_IntegratedData)
                {
                    int tmp = ReadIndexNum_IntegratedData;
                    if (tmp == BufSize)
                    {
                        ReadIndexNum_IntegratedData = 0;
                        tmp = 0;
                    }

                    ReadIndexNum_IntegratedData++;

                    return tmp;
                }
            }

            public static int ReadIndex_BeaconData()
            {
                lock (ReadIndexLock_BeaconData)
                {
                    int tmp = ReadIndexNum_BeaconData;
                    if (tmp == BufSize)
                    {
                        ReadIndexNum_BeaconData = 0;
                        tmp = 0;
                    }

                    ReadIndexNum_BeaconData++;

                    return tmp;
                }
            }

            //書き込む行をreturn
            public static int WriteIndex_IntegratedData()
            {
                lock (WriteIndexLock_IntegratedData)
                {
                    int tmp = WriteIndexNum_IntegratedData;
                    if (tmp == BufSize)
                    {
                        WriteIndexNum_IntegratedData = 0;
                        tmp = 0;
                    }

                    WriteIndexNum_IntegratedData++;

                    return tmp;
                }
            }

            public static int WriteIndex_BeaconData()
            {
                lock (WriteIndexLock_BeaconData)
                {
                    int tmp = WriteIndexNum_BeaconData;
                    if (tmp == BufSize)
                    {
                        WriteIndexNum_BeaconData = 0;
                        tmp = 0;
                    }

                    WriteIndexNum_BeaconData++;

                    return tmp;
                }
            }

            //指定した行からデータを読み出す
            public static ATS_IntegratedData? ReadData_IntegratedData(int line)
            {
                if (line < 0 || line >= BufSize)
                {
                    return null;
                }

                lock (BufferLocks_IntegratedData[line])
                {
                    if (!ValidData_IntegratedData[line])
                    {
                        return null;
                    }

                    ValidData_IntegratedData[line] = false;
                    return buffers_IntegratedData[line];
                }
            }

            public static ATS_BEACONDATA? ReadData_BeaconData(int line)
            {
                if (line < 0 || line >= BufSize)
                {
                    return null;
                }

                lock (BufferLocks_BeaconData[line])
                {
                    if (!ValidData_BeaconData[line])
                    {
                        return null;
                    }

                    ValidData_BeaconData[line] = false;
                    return buffers_BeaconData[line];
                }
            }

            //指定した行にデータを書き込む
            public static void WriteData_IntegratedData(int line, ATS_VEHICLESTATE vehicleState, ATS_HANDLES handles)
            {
                if (line < 0 || line >= BufSize)
                {
                    return;
                }

                lock (BufferLocks_IntegratedData[line])
                {
                    ATS_IntegratedData tmp = new ATS_IntegratedData();

                    tmp.Location = vehicleState.Location;
                    tmp.Speed = vehicleState.Speed;
                    tmp.Time = vehicleState.Time;
                    tmp.BcPressure = vehicleState.BcPressure;
                    tmp.MrPressure = vehicleState.MrPressure;
                    tmp.ErPressure = vehicleState.ErPressure;
                    tmp.BpPressure = vehicleState.BpPressure;
                    tmp.SapPressure = vehicleState.SapPressure;
                    tmp.Current = vehicleState.Current;

                    tmp.Brake = handles.Brake;
                    tmp.Power = handles.Power;
                    tmp.Reverser = handles.Reverser;
                    tmp.ConstantSpeed = handles.ConstantSpeed;

                    buffers_IntegratedData[line] = tmp;
                    ValidData_IntegratedData[line] = true;
                }
            }

            public static void WriteData_BeaconData(int line, ATS_BEACONDATA BeaconData)
            {
                if (line < 0 || line >= BufSize)
                {
                    return;
                }

                lock (BufferLocks_BeaconData[line])
                {
                    ATS_BEACONDATA tmp = new ATS_BEACONDATA();

                    tmp.Type = BeaconData.Type;
                    tmp.Signal = BeaconData.Signal;
                    tmp.Distance = BeaconData.Distance;
                    tmp.Optional = BeaconData.Optional;

                    buffers_BeaconData[line] = tmp;
                    ValidData_BeaconData[line] = true;
                }
            }
        }

        //ATS Controller出力バッファ
        public class AtsOutputBuffer
        {
            //バッファの設定
            private const int BufSize = 8; //2の累乗
            private static int ReadIndexNum;
            private static int WriteIndexNum;

            //ATS_OutputDataのバッファ
            private static ATS_OutputData[] buffers = new ATS_OutputData[BufSize];

            //データの有無
            private static bool[] ValidData = new bool[BufSize];

            //ロック機構
            private static object[] BufferLocks = new object[BufSize];
            private static object ReadIndexLock = new object();
            private static object WriteIndexLock = new object();

            //バッファを初期化
            public static void SetupBuffer()
            {
                lock (ReadIndexLock)
                {
                    ReadIndexNum = 0;
                }

                lock (WriteIndexLock)
                {
                    WriteIndexNum = 0;
                }

                buffers = new ATS_OutputData[BufSize];
                for (int i = 0; i < BufSize; i++)
                {
                    buffers[i] = new ATS_OutputData();
                    BufferLocks[i] = new object();
                    ValidData[i] = false;
                }
            }

            //バッファにデータがあるかreturn
            public static bool ValidDataExist()
            {
                lock (BufferLocks)
                {
                    return ValidData.Any(v => v);
                }
            }

            //読み出す行をreturn
            public static int ReadIndex()
            {
                lock (ReadIndexLock)
                {
                    int tmp = ReadIndexNum;
                    if (tmp == BufSize)
                    {
                        ReadIndexNum = 0;
                        tmp = 0;
                    }

                    ReadIndexNum++;

                    return tmp;
                }
            }

            //書き込む行をreturn
            public static int WriteIndex()
            {
                lock (WriteIndexLock)
                {
                    int tmp = WriteIndexNum;
                    if (tmp == BufSize)
                    {
                        WriteIndexNum = 0;
                        tmp = 0;
                    }

                    WriteIndexNum++;

                    return tmp;
                }
            }

            //指定した行からデータを読み出す
            public static ATS_OutputData? ReadData(int line)
            {
                if (line < 0 || line >= BufSize)
                {
                    return null;
                }

                lock (BufferLocks[line])
                {
                    if (!ValidData[line])
                    {
                        return null;
                    }

                    ValidData[line] = false;
                    return buffers[line];
                }
            }

            //指定した行にデータを書き込む
            public static void WriteData(int line, ATS_OutputData data)
            {
                if (line < 0 || line >= BufSize)
                {
                    return;
                }

                lock (BufferLocks[line])
                {
                    ATS_OutputData tmp = new ATS_OutputData();

                    tmp.Id = data.Id;
                    tmp.Interruption = data.Interruption;
                    tmp.DataPriority = data.DataPriority;
                    tmp.DataFormat = data.DataFormat;
                    tmp.ParityBitSetting = data.ParityBitSetting;
                    tmp.DataType = data.DataType;
                    tmp.DataLength = data.DataLength;
                    tmp.Data = data.Data;
                    tmp.ParityBit = data.ParityBit;

                    buffers[line] = tmp;
                    ValidData[line] = true;
                }
            }
        }

        //統合バッファ
        public class IntegratedInfomationBuffer
        {
            //バッファの設定
            private const int BufSize = 8; //2の累乗
            private static int ReadIndexNum;
            private static int WriteIndexNum;

            //ATS_IntegratedInfomationDataのバッファ
            private static IntegratedInfomationData[] buffers = new IntegratedInfomationData[BufSize];

            //データの有無
            private static bool[] ValidData = new bool[BufSize];

            //ロック機構
            private static object[] BufferLocks = new object[BufSize];
            private static object ReadIndexLock = new object();
            private static object WriteIndexLock = new object();

            //バッファを初期化
            public static void SetupBuffer()
            {
                lock (ReadIndexLock)
                {
                    ReadIndexNum = 0;
                }

                lock (WriteIndexLock)
                {
                    WriteIndexNum = 0;
                }

                buffers = new IntegratedInfomationData[BufSize];
                for (int i = 0; i < BufSize; i++)
                {
                    buffers[i] = new IntegratedInfomationData();
                    BufferLocks[i] = new object();
                    ValidData[i] = false;
                }
            }

            //バッファにデータがあるかreturn
            public static bool ValidDataExist()
            {
                lock (BufferLocks)
                {
                    return ValidData.Any(v => v);
                }
            }

            //読み出す行をreturn
            public static int ReadIndex()
            {
                lock (ReadIndexLock)
                {
                    int tmp = ReadIndexNum;
                    if (tmp == BufSize)
                    {
                        ReadIndexNum = 0;
                        tmp = 0;
                    }

                    ReadIndexNum++;

                    return tmp;
                }
            }

            //書き込む行をreturn
            public static int WriteIndex()
            {
                lock (WriteIndexLock)
                {
                    int tmp = WriteIndexNum;
                    if (tmp == BufSize)
                    {
                        WriteIndexNum = 0;
                        tmp = 0;
                    }

                    WriteIndexNum++;

                    return tmp;
                }
            }

            //指定した行からデータを読み出す
            public static IntegratedInfomationData? ReadData(int line)
            {
                if (line < 0 || line >= BufSize)
                {
                    return null;
                }

                lock (BufferLocks[line])
                {
                    if (!ValidData[line])
                    {
                        return null;
                    }

                    ValidData[line] = false;
                    return buffers[line];
                }
            }

            //指定した行にデータを書き込む
            public static void WriteData(int line, IntegratedInfomationData data)
            {
                if (line < 0 || line >= BufSize)
                {
                    return;
                }

                lock (BufferLocks[line])
                {
                    IntegratedInfomationData tmp = new IntegratedInfomationData();

                    tmp.Id = data.Id;
                    tmp.Interruption = data.Interruption;
                    tmp.DataPriority = data.DataPriority;
                    tmp.DataFormat = data.DataFormat;
                    tmp.ParityBitSetting = data.ParityBitSetting;
                    tmp.DataType = data.DataType;
                    tmp.DataLength = data.DataLength;
                    tmp.Data = data.Data;
                    tmp.ParityBit = data.ParityBit;

                    buffers[line] = tmp;
                    ValidData[line] = true;
                }
            }
        }

        //音声再生用バッファ
        public class PlayMusic
        {
            //バッファの設定
            private const int BufSize = 8; //2の累乗
            private const int SubBufSize = 2; //2で固定
            private static int ReadIndexNum;
            private static int WriteIndexNum;

            //string型のバッファ
            private static string[][] buffers = new string[BufSize][]; //buffers[line][0] : option, buffers[line][1] : data

            //データの有無
            private static bool[] ValidData = new bool[BufSize];

            //ロック機構
            private static object[] BufferLocks = new object[BufSize];
            private static object ReadIndexLock = new object();
            private static object WriteIndexLock = new object();

            //バッファを初期化
            public static void SetupBuffer()
            {
                lock (ReadIndexLock)
                {
                    ReadIndexNum = 0;
                }

                lock (WriteIndexLock)
                {
                    WriteIndexNum = 0;
                }

                buffers = new string[BufSize][];
                for (int i = 0; i < BufSize; i++)
                {
                    buffers[i] = new string[SubBufSize];
                    for (int j = 0; j < SubBufSize; j++)
                    {
                        buffers[i][j] = "";
                    }

                    BufferLocks[i] = new object();
                    ValidData[i] = false;
                }
            }

            //バッファにデータがあるかreturn
            public static bool ValidDataExist()
            {
                lock (BufferLocks)
                {
                    return ValidData.Any(v => v);
                }
            }

            //読み出す行をreturn
            public static int ReadIndex()
            {
                lock (ReadIndexLock)
                {
                    int tmp = ReadIndexNum;
                    if (tmp == BufSize)
                    {
                        ReadIndexNum = 0;
                        tmp = 0;
                    }

                    ReadIndexNum++;

                    return tmp;
                }
            }

            //書き込む行をreturn
            public static int WriteIndex()
            {
                lock (WriteIndexLock)
                {
                    int tmp = WriteIndexNum;
                    if (tmp == BufSize)
                    {
                        WriteIndexNum = 0;
                        tmp = 0;
                    }

                    WriteIndexNum++;

                    return tmp;
                }
            }

            //指定した行からデータを読み出す
            public static string[] ReadData(int line)
            {
                if (line < 0 || line >= BufSize)
                {
                    return new string[] { "" };
                }

                lock (BufferLocks[line])
                {
                    if (!ValidData[line])
                    {
                        return new string[] { "" };
                    }

                    ValidData[line] = false;
                    return new string[] { buffers[line][0].ToString() + "", buffers[line][1].ToString() + "" };
                }
            }

            //指定した行にデータを書き込む
            public static void WriteData(int line, string option, string data)
            {
                if (line < 0 || line >= BufSize)
                {
                    return;
                }

                lock (BufferLocks[line])
                {
                    buffers[line][0] = option;
                    buffers[line][1] = data;
                    ValidData[line] = true;
                }
            }
        }

        //デバッグコンソール用バッファ
        public class DebugFunction
        {
            //バッファの設定
            private const int BufSize = 8; //2の累乗
            private static int ReadIndexNum;
            private static int WriteIndexNum;

            //string型のバッファ
            private static string[] buffers = new string[BufSize];

            //データの有無
            private static bool[] ValidData = new bool[BufSize];

            //ロック機構
            private static object[] BufferLocks = new object[BufSize];
            private static object ReadIndexLock = new object();
            private static object WriteIndexLock = new object();

            //バッファを初期化
            public static void SetupBuffer()
            {
                lock (ReadIndexLock)
                {
                    ReadIndexNum = 0;
                }

                lock (WriteIndexLock)
                {
                    WriteIndexNum = 0;
                }

                buffers = new string[BufSize];
                for (int i = 0; i < BufSize; i++)
                {
                    buffers[i] = "";
                    BufferLocks[i] = new object();
                    ValidData[i] = false;
                }
            }

            //バッファにデータがあるかreturn
            public static bool ValidDataExist()
            {
                lock (BufferLocks)
                {
                    return ValidData.Any(v => v);
                }
            }

            //読み出す行をreturn
            public static int ReadIndex()
            {
                lock (ReadIndexLock)
                {
                    int tmp = ReadIndexNum;
                    if (tmp == BufSize)
                    {
                        ReadIndexNum = 0;
                        tmp = 0;
                    }

                    ReadIndexNum++;

                    return tmp;
                }
            }

            //書き込む行をreturn
            public static int WriteIndex()
            {
                lock (WriteIndexLock)
                {
                    int tmp = WriteIndexNum;
                    if (tmp == BufSize)
                    {
                        WriteIndexNum = 0;
                        tmp = 0;
                    }

                    WriteIndexNum++;

                    return tmp;
                }
            }

            //指定した行からデータを読み出す
            public static string ReadData(int line)
            {
                if (line < 0 || line >= BufSize)
                {
                    return "";
                }

                lock (BufferLocks[line])
                {
                    if (!ValidData[line])
                    {
                        return "";
                    }

                    ValidData[line] = false;
                    return buffers[line].ToString() + "";
                }
            }

            //指定した行にデータを書き込む
            public static void WriteData(int line, string data)
            {
                if (line < 0 || line >= BufSize)
                {
                    return;
                }

                lock (BufferLocks[line])
                {
                    buffers[line] = data;
                    ValidData[line] = true;
                }
            }
        }

        //シリアル通信用バッファ
        public class SerialCommunications
        {
            //バッファの設定
            private const int BufSize = 8; //2の累乗
            private static int ReadIndexNum;
            private static int WriteIndexNum;

            //byte列のバッファ
            private static List<byte>[] buffers = new List<byte>[BufSize];

            //データの有無
            private static bool[] ValidData = new bool[BufSize];

            //ロック機構
            private static object[] BufferLocks = new object[BufSize];
            private static object ReadIndexLock = new object();
            private static object WriteIndexLock = new object();

            //バッファを初期化
            public static void SetupBuffer()
            {
                lock (ReadIndexLock)
                {
                    ReadIndexNum = 0;
                }

                lock (WriteIndexLock)
                {
                    WriteIndexNum = 0;
                }

                buffers = new List<byte>[BufSize];
                for (int i = 0; i < BufSize; i++)
                {
                    buffers[i] = new List<byte>();
                    BufferLocks[i] = new object();
                    ValidData[i] = false;
                }
            }

            //バッファにデータがあるかreturn
            public static bool ValidDataExist()
            {
                lock (BufferLocks)
                {
                    return ValidData.Any(v => v);
                }
            }

            //読み出す行をreturn
            public static int ReadIndex()
            {
                lock (ReadIndexLock)
                {
                    int tmp = ReadIndexNum;
                    if (tmp == BufSize)
                    {
                        ReadIndexNum = 0;
                        tmp = 0;
                    }

                    ReadIndexNum++;

                    return tmp;
                }
            }

            //書き込む行をreturn
            public static int WriteIndex()
            {
                lock (WriteIndexLock)
                {
                    int tmp = WriteIndexNum;
                    if (tmp == BufSize)
                    {
                        WriteIndexNum = 0;
                        tmp = 0;
                    }

                    WriteIndexNum++;

                    return tmp;
                }
            }

            //指定した行からデータを読み出す
            public static byte[] ReadData(int line)
            {
                if (line < 0 || line >= BufSize)
                {
                    byte[] tmp = { 0x00 };
                    return tmp;
                }

                lock (BufferLocks[line])
                {
                    if (!ValidData[line])
                    {
                        byte[] tmp = { 0x00 };
                        return tmp;
                    }

                    ValidData[line] = false;
                    return buffers[line].ToArray();
                }
            }

            //指定した行にデータを書き込む
            public static void WriteData(int line, byte[] data)
            {
                if (line < 0 || line >= BufSize)
                {
                    return;
                }

                lock (BufferLocks[line])
                {
                    //バッファのサイズを変更
                    var tmp = buffers[line];
                    if (tmp.Count < data.Length)
                    {
                        tmp.AddRange(new byte[data.Length - tmp.Count]);
                    }
                    else if (tmp.Count > data.Length)
                    {
                        tmp.RemoveRange(data.Length, tmp.Count - data.Length);
                    }

                    //データを書き込み
                    for (int i = 0; i < data.Length; i++)
                    {
                        buffers[line][i] = data[i];
                    }

                    ValidData[line] = true;
                }
            }
        }
    }
}
