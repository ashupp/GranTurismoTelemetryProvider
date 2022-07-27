using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using ILogger = SimFeedback.log.ILogger;

namespace SimFeedback.telemetry
{
    public sealed class TelemetryProvider : AbstractTelemetryProvider
    {

        private const string IpAddr = "127.0.0.1";
        private const int PortNum = 26998;
        private bool _isStopped = true;
        private Thread _t;

        public TelemetryProvider()
        {
            Author = "ashupp / ashnet GmbH";
            Version = Assembly.LoadFrom(Assembly.GetExecutingAssembly().Location).GetName().Version.ToString();
            BannerImage = @"img\banner_granturismo.png";
            IconImage = @"img\icon_granturismo.png";
            TelemetryUpdateFrequency = 60;
        }

        public override string Name => "granturismo";

        public override void Init(ILogger logger)
        {
            base.Init(logger);
            Log("Initializing Gran Turismo Telemetry Provider");
            Log("Using Sample Period: " + SamplePeriod);
        }

        public override string[] GetValueList()
        {
            return GetValueListByReflection(typeof(TelemetryData));
        }

        public override void Stop()
        {
            if (_isStopped) return;
            LogDebug("Stopping Gran Turismo Telemetry Provider");
            _isStopped = true;
            if (_t != null) _t.Join();
        }

        public override void Start()
        {
            if (_isStopped)
            {
                LogDebug("Starting Gran Turismo Telemetry Provider");
                _isStopped = false;
                _t = new Thread(Run);
                _t.Start();
            }
        }

        private void Run()
        {
            UdpClient socket = new UdpClient { ExclusiveAddressUse = false };
            socket.Client.ReceiveBufferSize = 20;
            socket.Client.Bind(new IPEndPoint(IPAddress.Parse(IpAddr), PortNum));
            var endpoint = new IPEndPoint(IPAddress.Parse(IpAddr), PortNum);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (!_isStopped)
            {
                try
                {

                    // get data from game, 
                    if (socket.Available == 0)
                    {
                        if (sw.ElapsedMilliseconds > 500)
                        {
                            IsRunning = false;
                            IsConnected = false;
                            Thread.Sleep(1000);
                        }
                        continue;
                    }
                    IsConnected = true;
                    IsRunning = true;
                    Byte[] received = socket.Receive(ref endpoint);
                    string resp = Encoding.UTF8.GetString(received);
                    TelemetryData telemetryData = ParseReponse(resp);

                    TelemetryEventArgs args = new TelemetryEventArgs(new SpecificTelemetryInfo(telemetryData));
                    RaiseEvent(OnTelemetryUpdate, args);
                    sw.Restart();


                }
                catch (Exception e)
                {
                    LogError("Gran Turismo Telemetry Provider Exception while processing data", e);
                    IsConnected = false;
                    IsRunning = false;
                    Thread.Sleep(1000);
                }
            }
            sw.Stop();
            IsConnected = false;
            IsRunning = false;
        }

        private TelemetryData ParseReponse(string resp)
        {
            TelemetryData telemetryData = new TelemetryData();

            var lines = resp.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            if (lines.Length <= 0) return telemetryData;
            var dict = new Dictionary<string, double>();
            foreach (var line in lines)
            {
                var tmpLineItems = line.Split('=');
                LogDebug(line);
                if (tmpLineItems.Length == 2)
                {
                    var newDoubleA = Convert.ToDouble(tmpLineItems[1].Replace(",","."), CultureInfo.InvariantCulture);
                    LogDebug(line);
                    LogDebug(newDoubleA.ToString());
                    dict.Add(tmpLineItems[0], newDoubleA);
                }
            }

            dict.TryGetValue("pitch", out var tmpPitch);
            telemetryData.Pitch = tmpPitch;

            dict.TryGetValue("roll", out var tmpRoll);
            telemetryData.Roll = tmpRoll;

            dict.TryGetValue("surge", out var tmpSurge);
            telemetryData.Surge = tmpSurge;

            dict.TryGetValue("yaw", out var tmpYaw);
            telemetryData.Yaw = tmpYaw;

            dict.TryGetValue("heave", out var tmpHeave);
            telemetryData.Heave = tmpHeave;

            dict.TryGetValue("rpm", out var tmpRpm);
            telemetryData.Rpm = tmpRpm;

            dict.TryGetValue("speed", out var tmpSpeed);
            telemetryData.Speed = tmpSpeed;

            return telemetryData;
        }

    }
}