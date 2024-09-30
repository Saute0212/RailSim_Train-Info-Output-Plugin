using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TrainInfoOutputPlugin
{
    public class TrainInfoOutputPlugin
    {
        private static Settings settingsEnv = null;
        private static Settings.OutputSetting settingsStatus;

        //プラグイン読み込み時に実行
        public void Load()
        {

        }

        //プラグイン解放時に実行
        public void Dispose()
        {

        }

        //ATSプラグインのバージョンを返す
        public int GetPluginVersion()
        {
            return settingsEnv.PluginFormat;
        }

        //車両読み込み時に実行
        public void SetVehicleSpec()
        {

        }

        //ゲーム開始時に実行
        public void Initialize(int brake)
        {

        }

        //毎フレーム実行
        public void Elapse()
        {

        }

        //主ハンドル操作時に実行
        public void SetPower(int notch)
        {

        }

        //ブレーキ操作時に実行
        public void SetBrake(int notch)
        {

        }

        //レバーサー操作時に実行
        public void SetReverser(int pos)
        {

        }

        //ATSキーが押されたときに実行
        public void KeyDown(int atsKeyCode)
        {

        }

        //ATSキーが離されたときに実行
        public void KeyUp(int atsKeyCode)
        {

        }

        //警笛操作時に実行
        public void HornBlow(int hornType)
        {

        }

        //ドアが開いたときに実行
        public void DoorOpen()
        {

        }

        //ドアが閉まったときに実行
        public void DoorClose()
        {

        }

        //閉塞の信号が変化したときに実行
        public void SetSignal(int signal)
        {

        }

        //地上子を超えたときに実行
        public void SetBeaconData()
        {

        }

        //設定の読み込み
        private void LoadSettings()
        {
            settingsEnv = new Settings();
            settingsStatus = new Settings.OutputSetting(false);
            settingsEnv.SetUp();
        }
    }
}
