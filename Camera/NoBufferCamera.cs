using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HardwareInterface
{

    public partial class NoBufferCamera
    {
        public VideoCapture cap;
        public NoBufferCamera(int cam_id = 0, VideoCaptureAPIs vpis = VideoCaptureAPIs.V4L)
        {
            cap = new VideoCapture(cam_id, vpis)
            {
                ConvertRgb = true
            };
            cap.BufferSize = 1;
        }

        public new void Init()
        {
            while (!cap.IsOpened()) Thread.Sleep(0);
        }

        public new Mat GetLatestFrame()
        {
            Mat src = new Mat();
            while (!cap.Read(src)) Thread.Sleep(0);
            return src;
        }

        public void Abort()
        {
            cap.Dispose();
        }
    }
}
