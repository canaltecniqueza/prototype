﻿using System;
using System.ComponentModel;
using System.IO;
using Gibbed.Helpers;
using Gibbed.Prototype.Helpers;

namespace Gibbed.Prototype.FileFormats
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public void Serialize(Stream output)
        {
            output.WriteF32(this.X);
            output.WriteF32(this.Y);
            output.WriteF32(this.Z);
        }

        public void Deserialize(Stream input)
        {
            this.X = input.ReadF32();
            this.Y = input.ReadF32();
            this.Z = input.ReadF32();
        }
    }
}
