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

        //設定の読み込み
        private void LoadSettings()
        {
            settingsEnv = new Settings();
            settingsStatus = new Settings.OutputSetting(false);
            settingsEnv.SetUp();
        }
    }
}
