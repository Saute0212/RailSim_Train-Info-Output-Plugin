using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TrainInfoOutputPlugin
{
    //車両情報
    [StructLayout(LayoutKind.Sequential)]
    public struct ATS_VEHICLESPEC
    {
        public int BrakeNotches; //ブレーキノッチ数
        public int PowerNotches; //力行ノッチ数
        public int AtsNotch; //ATS確認ノッチ
        public int B67Notch; //ブレーキ弁67度に相当するノッチ
        public int Cars; //編成量数
    }

    //車両の状態量
    [StructLayout(LayoutKind.Sequential)]
    public struct ATS_VEHICLESTATE
    {
        public double Location; //車両位置(m)
        public float Speed; //車両速度(km/h)
        public int Time; //時刻(ms)
        public float BcPressure; //ブレーキシリンダ圧力(Pa)
        public float MrPressure; //元空気ダメ圧力(Pa)
        public float ErPressure; //釣り合い空気ダメ圧力(Pa)
        public float BpPressure; //ブレーキ管圧力(Pa)
        public float SapPressure; //直通管圧力(Pa)
        public float Current; //電流値(A)
    }

    //車上子で受け取った情報
    [StructLayout(LayoutKind.Sequential)]
    public struct ATS_BEACONDATA
    {
        public int Type; //地上子種別
        public int Signal; //対となるセクションの信号
        public float Distance; //対となるセクションまでの距離(m)
        public int Optional; //地上子に設定された任意の値
    }

    //BVEに渡すハンドル制御値
    [StructLayout(LayoutKind.Sequential)]
    public struct ATS_HANDLES
    {
        public int Brake; //ブレーキノッチ
        public int Power; //力行ノッチ
        public int Reverser; //レバーサー位置
        public int ConstantSpeed; //定速制御の状態
    }

    public class TrainInfoOutputPlugin
    {
        private static Settings settingsEnv = null;
        
        private static ATS_VEHICLESPEC VehicleSpec;
        private static ATS_VEHICLESTATE VehicleState;
        private static ATS_HANDLES HandleInput;
        private static ATS_HANDLES HandleOutput;

        //ATS Keys
        public const int ATS_KEY_S = 0;
        public const int ATS_KEY_A1 = 1;
        public const int ATS_KEY_A2 = 2;
        public const int ATS_KEY_B1 = 3;
        public const int ATS_KEY_B2 = 4;
        public const int ATS_KEY_C1 = 5;
        public const int ATS_KEY_C2 = 6;
        public const int ATS_KEY_D = 7;
        public const int ATS_KEY_E = 8;
        public const int ATS_KEY_F = 9;
        public const int ATS_KEY_G = 10;
        public const int ATS_KEY_H = 11;
        public const int ATS_KEY_I = 12;
        public const int ATS_KEY_J = 13;
        public const int ATS_KEY_K = 14;
        public const int ATS_KEY_L = 15;

        //ハンドルの初期位置
        public const int ATS_INIT_SVC = 0; //常用位置
        public const int ATS_INIT_EMG = 1; //非常位置
        public const int ATS_INIT_REMOVED = 2; //抜き取り

        //音声コントロール
        public const int ATS_SOUND_STOP = -1000; //停止
        public const int ATS_SOUND_PLAY = 1; //1回再生
        public const int ATS_SOUND_PLAYLOOPING = 0; //繰り返し再生
        public const int ATS_SOUND_CONTINUE = 2; //現在の状態を維持する

        //Horn Type
        public const int ATS_HORN_PRIMARY = 0; //電気笛, AW2
        public const int ATS_HORN_SECONDARY = 1; //空気笛, AW5
        public const int ATS_HORN_MUSIC = 2; //ミュージックホーン

        //定速制御コントロール
        public const int ATS_CONSTANTSPEED_CONTINUE = 0; //現在の状態を維持する
        public const int ATS_CONSTANTSPEED_ENABLE = 1; //起動
        public const int ATS_CONSTANTSPEED_DISABLE = 2; //停止

        //プラグイン読み込み時に実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void Load()
        {
            SetupSystem();
        }

        //プラグイン解放時に実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void Dispose()
        {

        }

        //ATSプラグインのバージョンを返す
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static int GetPluginVersion()
        {
            return Settings.PluginFormat;
        }

        //車両読み込み時に実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void SetVehicleSpec(ATS_VEHICLESPEC VehicleSpec)
        {

        }

        //ゲーム開始時に実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void Initialize(int brake)
        {
            
        }

        //毎フレーム実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static ATS_HANDLES Elapse(ATS_VEHICLESTATE VehicleState, int[] panel, int[] sound)
        {
            HandleOutput.Power = HandleInput.Power;
            HandleOutput.Brake = HandleInput.Brake;
            HandleOutput.Reverser = HandleInput.Reverser;

            return HandleOutput;
        }

        //主ハンドル操作時に実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void SetPower(int notch)
        {
            HandleInput.Power = notch;
        }

        //ブレーキ操作時に実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void SetBrake(int notch)
        {
            HandleInput.Brake = notch;
        }

        //レバーサー操作時に実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void SetReverser(int pos)
        {
            HandleInput.Reverser = pos;
        }

        //ATSキーが押されたときに実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void KeyDown(int atsKeyCode)
        {

        }

        //ATSキーが離されたときに実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void KeyUp(int atsKeyCode)
        {

        }

        //警笛操作時に実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void HornBlow(int hornType)
        {

        }

        //ドアが開いたときに実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void DoorOpen()
        {

        }

        //ドアが閉まったときに実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void DoorClose()
        {

        }

        //閉塞の信号が変化したときに実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void SetSignal(int signal)
        {

        }

        //地上子を超えたときに実行
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void SetBeaconData(ATS_BEACONDATA BeaconData)
        {

        }

        //設定の読み込み
        private static void SetupSystem()
        {
            Settings.SetUp();
            SerialCommunications.InitSerialCommunications(Settings.ComPort, Settings.ComSpeed.ToString());
        }
    }
}
