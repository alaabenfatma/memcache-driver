﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Criteo.MemcacheClient.Headers
{
    public struct MemacheRequestHeader : IEquatable<MemacheRequestHeader>
    {
        public MemacheRequestHeader(Opcode instruction)
        {
            Opcode = instruction;
            KeyLength = 0;
            ExtraLength = 0;
            DataType = 0;
            Reserved = 0;
            TotalBodyLength = 0;
            Opaque = 0;
            Cas = 0;
        }

        private const byte Magic = 0x80;
        public Opcode Opcode;
        public ushort KeyLength;
        public byte ExtraLength;
        public byte DataType;
        private ushort Reserved;
        public uint TotalBodyLength;
        public uint Opaque;
        public ulong Cas;

        public void ToData(byte[] data, int offset = 0)
        {
            data[offset] = Magic;
            data[1 + offset] = (byte)Opcode;
            data.CopyFrom(2 + offset, KeyLength);
            data[4 + offset] = ExtraLength;
            data[5 + offset] = DataType;
            data.CopyFrom(6 + offset, Reserved);
            data.CopyFrom(8 + offset, TotalBodyLength);
            data.CopyFrom(12 + offset, Opaque);
            data.CopyFrom(16 + offset, Cas);
        }

        public void FromData(byte[] data, int offset = 0)
        {
            if (data[offset] != Magic)
                throw new ArgumentException("The buffer does not begin with the MagicNumber");
            Opcode = (Opcode)data[1 + offset];
            KeyLength = data.CopyToUShort(2 + offset);
            ExtraLength = data[4 + offset];
            DataType = data[5 + offset];
            Reserved = data.CopyToUShort(6 + offset);
            TotalBodyLength = data.CopyToUInt(8 + offset);
            Opaque = data.CopyToUInt(12 + offset);
            Cas = data.CopyToULong(16 + offset);
        }

        public override bool Equals(object obj)
        {
            return obj is MemacheRequestHeader
                && Equals((MemacheRequestHeader)obj);
        }

        public bool Equals(MemacheRequestHeader other)
        {
            return other.Opcode == Opcode
                && other.KeyLength == KeyLength
                && other.ExtraLength == ExtraLength
                && other.DataType == DataType
                && other.Reserved == Reserved
                && other.TotalBodyLength == TotalBodyLength
                && other.Opaque == Opaque
                && other.Cas == Cas;
        }

        public override int GetHashCode()
        {
            return Opcode.GetHashCode()
                ^ KeyLength.GetHashCode()
                ^ ExtraLength.GetHashCode()
                ^ DataType.GetHashCode()
                ^ Reserved.GetHashCode()
                ^ TotalBodyLength.GetHashCode()
                ^ Opaque.GetHashCode()
                ^ Cas.GetHashCode();
        }
    }
}
