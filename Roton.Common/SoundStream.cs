﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roton.Common
{
    public class SoundStream : Stream
    {
        private const int HEADER_LENGTH = 44;

        public SoundStream()
        {
            Initialize();
        }

        public SoundStream(byte[] buffer)
        {
            Initialize();
            this.Buffer = buffer;
        }

        protected byte[] _buffer;
        public byte[] Buffer
        {
            get
            {
                return _buffer;
            }
            set
            {
                _buffer = value;
                int dataLength = _buffer.Length;
                int headerLength = HEADER_LENGTH;
                int totalLength = dataLength + headerLength - 4;
                Data[0x04] = (byte)(totalLength & 0xFF);
                Data[0x05] = (byte)((totalLength >> 8) & 0xFF);
                Data[0x06] = (byte)((totalLength >> 16) & 0xFF);
                Data[0x07] = (byte)((totalLength >> 24) & 0xFF);
                Data[0x28] = (byte)(dataLength & 0xFF);
                Data[0x29] = (byte)((dataLength >> 8) & 0xFF);
                Data[0x2A] = (byte)((dataLength >> 16) & 0xFF);
                Data[0x2B] = (byte)((dataLength >> 24) & 0xFF);
                _length = HEADER_LENGTH + dataLength;
            }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        protected byte[] Data
        {
            get;
            set;
        }

        public override void Flush()
        {
            // do nothing
        }

        protected int _frequency;
        public int Frequency
        {
            get
            {
                return Frequency;
            }
            set
            {
                int rate = value << 1;
                Data[0x18] = (byte)(value & 0xFF);
                Data[0x19] = (byte)((value >> 8) & 0xFF);
                Data[0x1A] = (byte)((value >> 16) & 0xFF);
                Data[0x1B] = (byte)((value >> 24) & 0xFF);
                Data[0x1C] = (byte)(rate & 0xFF);
                Data[0x1D] = (byte)((rate >> 8) & 0xFF);
                Data[0x1E] = (byte)((rate >> 16) & 0xFF);
                Data[0x1F] = (byte)((rate >> 24) & 0xFF);
            }
        }

        void Initialize()
        {
            Data = new byte[] {
                0x52, 0x49, 0x46, 0x46,
                0x00, 0x00, 0x00, 0x00,
                0x57, 0x41, 0x56, 0x45,
                0x66, 0x6D, 0x74, 0x20,
                0x10, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x01, 0x00,
                0x44, 0xAC, 0x00, 0x00,
                0x44, 0xAC, 0x00, 0x00,
                0x04, 0x00, 0x10, 0x00,
                0x64, 0x61, 0x74, 0x61,
                0x00, 0x00, 0x00, 0x00
            };
        }

        private int _length;
        public override long Length
        {
            get { return _length; }
        }

        private int _position;
        public override long Position
        {
            get { return _position; }
            set { _position = (int)value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int total = 0;
            while (count > 0 && _position < _length)
            {
                if (_position < HEADER_LENGTH)
                {
                    buffer[offset] = Data[_position];
                }
                else
                {
                    buffer[offset] = Buffer[_position - HEADER_LENGTH];
                }
                offset++;
                _position++;
                count--;
                total++;
            }
            return total;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    _position = (int)offset;
                    break;
                case SeekOrigin.End:
                    _position = _length - (int)offset;
                    break;
                case SeekOrigin.Current:
                    _position += (int)offset;
                    break;
            }
            return _position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}
