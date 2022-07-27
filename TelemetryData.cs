using System;
using System.Runtime.InteropServices;
using Math = System.Math;

namespace SimFeedback.telemetry
{

    public struct TelemetryData
    {
        #region For SimFeedback Available Values

        public double Pitch { get; set; }

        public double Yaw { get; set; }

        public double Roll { get; set; }

        public double Heave { get; set; }

        public double Sway { get; set; }

        public double Surge { get; set; }

        public double Rpm { get; set; }

        public double Speed { get; set; }
        #endregion

        #region Conversion calculations
        private static double ConvertRadiansToDegrees(double radians)
        {
            var degrees = (double)(180 / Math.PI) * radians;
            return degrees;
        }

        private static double ConvertAccel(double accel)
        {
            return (double) (accel / 9.80665);
        }

        private static double LoopAngle(double angle, double minMag)
        {

            double absAngle = Math.Abs(angle);

            if (absAngle <= minMag)
            {
                return angle;
            }

            double direction = angle / absAngle;

            //(180.0f * 1) - 135 = 45
            //(180.0f *-1) - -135 = -45
            double loopedAngle = (180.0f * direction) - angle;

            return loopedAngle;
        }
        #endregion
    }
}