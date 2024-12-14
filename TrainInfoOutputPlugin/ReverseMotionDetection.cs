using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrainInfoOutputPlugin
{
    //後退検知機能
    public class ReverseMotionDetection
    {
        public int RunRMD(float Speed, int ReverserPos, int BrakeNotch, int BrakeNotches)
        {
            int BrakeOutput = BrakeNotch;
            
            if(IsHighSpeed(Speed) && IsReverserInReverse(ReverserPos))
            {
                BrakeOutput = BrakeNotches;
            }

            return BrakeOutput;
        }

        private bool IsHighSpeed(float speed)
        {
            return speed >= 5.0;
        }

        private bool IsReverserInReverse(int RevPos)
        {
            return RevPos < 0;
        }
    }
}
