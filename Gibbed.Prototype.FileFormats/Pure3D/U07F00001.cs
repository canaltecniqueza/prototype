﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gibbed.Helpers;
using Gibbed.Prototype.Helpers;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x07F00001)]
    public class U07F00001 : Node
    {
        public byte[] Data { get; set; }

        public override void Serialize(Stream output)
        {
            output.WriteS32(this.Data.Length);
            output.Write(this.Data, 0, this.Data.Length);
        }

        public override void Deserialize(Stream input)
        {
            int length = input.ReadS32();
            this.Data = new byte[length];
            input.Read(this.Data, 0, this.Data.Length);
        }
    }
}
