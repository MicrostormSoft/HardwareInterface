﻿using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareInterface.MPUSensor
{
    class MPU6050 : ISensor
    {
        public const byte ADDRESS = 0x68;
        private const byte PWR_MGMT_1 = 0x6B;
        private const byte SMPLRT_DIV = 0x19;
        private const byte CONFIG = 0x1A;
        private const byte GYRO_CONFIG = 0x1B;
        private const byte ACCEL_CONFIG = 0x1C;
        private const byte FIFO_EN = 0x23;
        private const byte INT_ENABLE = 0x38;
        private const byte INT_STATUS = 0x3A;
        private const byte USER_CTRL = 0x6A;
        private const byte FIFO_COUNT = 0x72;
        private const byte FIFO_R_W = 0x74;

        private double accrate = 0;

        I2cDevice device;

        public MPU6050(int busid = 1, byte addr = ADDRESS)
        {
            I2cConnectionSettings cs = new I2cConnectionSettings(busid, addr);
            device = I2cDevice.Create(cs);
        }

        public void Abort()
        {
            device.Dispose();
        }

        public void Init()
        {
            Init(MesureRange.MP2g);
        }

        public void Init(MesureRange acc_range)
        {
            WriteByte(PWR_MGMT_1, 0x01);
            Task.Delay(100).Wait();
            WriteByte(ACCEL_CONFIG, (byte)acc_range);
            switch (acc_range)
            {
                case MesureRange.MP2g:
                    accrate = 2d / 65535d;
                    break;
                case MesureRange.MP4g:
                    accrate = 4d / 65535d;
                    break;
                case MesureRange.MP8g:
                    accrate = 8d / 65535d;
                    break;
                case MesureRange.MP16g:
                    accrate = 16d / 65535d;
                    break;
            }
        }

        public Vect3Result ReadAccelerometer()
        {
            return new Vect3Result
            {
                X = ReadWord(0x3B) * accrate,//3B 3C
                Y = ReadWord(0x3D) * accrate,//3D 3E
                Z = ReadWord(0x3F) * accrate //3F 40
            };
        }

        public Vect3Result ReadGyroscope()
        {
            return new Vect3Result
            {
                X = ReadWord(0x43),//43 44
                Y = ReadWord(0x45),//45 46
                Z = ReadWord(0x47) //47 48
            };
        }

        private byte[] ReadBytes(byte regAddr, int length)
        {
            byte[] values = new byte[length];
            byte[] buffer = new byte[1];
            buffer[0] = regAddr;
            device.WriteRead(buffer, values);
            return values;
        }

        private ushort ReadWord(byte address)
        {
            byte[] buffer = ReadBytes(address, 2);
            return (ushort)(((int)buffer[0] << 8) | (int)buffer[1]);
        }

        private void WriteByte(byte regAddr, byte data)
        {
            byte[] buffer = new byte[2];
            buffer[0] = regAddr;
            buffer[1] = data;
            device.Write(buffer);
        }
    }
}