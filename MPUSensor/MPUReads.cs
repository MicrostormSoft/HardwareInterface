using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareInterface.MPUSensor
{
    public struct Vect3Result
    {
        public double X, Y, Z;
    }

    public enum MesureRange
    {
        MP2g = 0x00, MP4g = 0x01, MP8g = 0x02, MP16g = 0x03
    }
}
