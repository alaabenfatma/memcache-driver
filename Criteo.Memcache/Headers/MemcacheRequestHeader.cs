﻿/* Licensed to the Apache Software Foundation (ASF) under one
   or more contributor license agreements.  See the NOTICE file
   distributed with this work for additional information
   regarding copyright ownership.  The ASF licenses this file
   to you under the Apache License, Version 2.0 (the
   "License"); you may not use this file except in compliance
   with the License.  You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing,
   software distributed under the License is distributed on an
   "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
   KIND, either express or implied.  See the License for the
   specific language governing permissions and limitations
   under the License.
*/
using System;
using System.Text;

using Criteo.Memcache.Exceptions;

namespace Criteo.Memcache.Headers
{
    public struct MemcacheRequestHeader : IEquatable<MemcacheRequestHeader>
    {
        public static readonly int Size = 24;

        public MemcacheRequestHeader(Opcode instruction)
        {
            Opcode = instruction;
            KeyLength = 0;
            ExtraLength = 0;
            DataType = 0;
            _reserved = 0;
            TotalBodyLength = 0;
            Opaque = 0;
            Cas = 0;
        }

        private const byte Magic = 0x80;
        public Opcode Opcode;
        public ushort KeyLength;
        public byte ExtraLength;
        public byte DataType;
        private ushort _reserved;
        public uint TotalBodyLength;
        public uint Opaque;
        public ulong Cas;

        public override string ToString()
        {
            var bld = new StringBuilder();
            bld.Append("Opcode:").Append(Opcode.ToString()).Append('|')
                .Append("KeyLength:").Append(KeyLength).Append('|')
                .Append("ExtraLength:").Append(ExtraLength).Append('|')
                .Append("DataType:").Append(DataType).Append('|')
                .Append("Reserved:").Append(_reserved).Append('|')
                .Append("TotalBodyLength:").Append(TotalBodyLength).Append('|')
                .Append("Opaque:").Append(Opaque).Append('|')
                .Append("Cas:").Append(Cas);
            return bld.ToString();
        }

        public void ToData(byte[] data, int offset = 0)
        {
            data[offset] = Magic;
            data[1 + offset] = (byte)Opcode;
            data.CopyFrom(2 + offset, KeyLength);
            data[4 + offset] = ExtraLength;
            data[5 + offset] = DataType;
            data.CopyFrom(6 + offset, _reserved);
            data.CopyFrom(8 + offset, TotalBodyLength);
            data.CopyFrom(12 + offset, Opaque);
            data.CopyFrom(16 + offset, Cas);
        }

        public void FromData(byte[] data, int offset = 0)
        {
            if (data[offset] != Magic)
                throw new MemcacheException("The buffer does not begin with the MagicNumber");
            Opcode = (Opcode)data[1 + offset];
            KeyLength = data.CopyToUShort(2 + offset);
            ExtraLength = data[4 + offset];
            DataType = data[5 + offset];
            _reserved = data.CopyToUShort(6 + offset);
            TotalBodyLength = data.CopyToUInt(8 + offset);
            Opaque = data.CopyToUInt(12 + offset);
            Cas = data.CopyToULong(16 + offset);
        }

        public override bool Equals(object obj)
        {
            return obj is MemcacheRequestHeader
                && Equals((MemcacheRequestHeader)obj);
        }

        public bool Equals(MemcacheRequestHeader other)
        {
            return other.Opcode == Opcode
                && other.KeyLength == KeyLength
                && other.ExtraLength == ExtraLength
                && other.DataType == DataType
                && other._reserved == _reserved
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
                ^ _reserved.GetHashCode()
                ^ TotalBodyLength.GetHashCode()
                ^ Opaque.GetHashCode()
                ^ Cas.GetHashCode();
        }
    }
}
