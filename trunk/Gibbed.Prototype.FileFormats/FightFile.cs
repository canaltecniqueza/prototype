﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Runtime.InteropServices;
using Gibbed.Helpers;
using Gibbed.Prototype.Helpers;
using System.Xml;

namespace Gibbed.Prototype.FileFormats
{
    public class FightFile
    {
        public UInt64 ReadHash(Stream stream)
        {
            if (this.HashesArePrecalculated)
            {
                return stream.ReadValueU64();
            }
            else
            {
                throw new InvalidOperationException("unsure how to handle this");
                // suspected that this is a uint32 + string rather than the hash
                return stream.ReadStringASCII(stream.ReadValueU32()).Hash1003F();
            }
        }

        public T ReadPropertyEnum<T>(Stream stream)
        {
            UInt64 value = stream.ReadValueU64();
            if (Enum.IsDefined(typeof(T), value) == false)
            {
                throw new Exception(FightHashes.Lookup(value));
            }
            return (T)Enum.ToObject(typeof(T), value);
        }

        public UInt64 ReadPropertyName(Stream stream)
        {
            return this.ReadHash(stream);
        }

        public string ReadPropertyString(Stream stream)
        {
            return stream.ReadAlignedASCII();
        }

        public int ReadPropertyInt(Stream stream)
        {
            return stream.ReadValueS32();
        }

        public float ReadPropertyFloat(Stream stream)
        {
            return stream.ReadValueF32();
        }

        public bool ReadPropertyBool(Stream stream)
        {
            return stream.ReadValueU32() == 0 ? false : true;
        }

        public Fight.BranchReference ReadPropertyBranch(Stream stream)
        {
            Fight.BranchReference rez = new Fight.BranchReference();

            rez.Name = stream.ReadAlignedASCII();

            if (rez.Name == null || rez.Name.Length == 0)
            {
                rez.Index = stream.ReadValueU32();
            }

            return rez;
        }

        public UInt32 Flags;
        #region public bool HashesArePrecalculated
        public bool HashesArePrecalculated
        {
            get
            {
                if ((this.Flags & 4) == 4)
                {
                    return true;
                }

                return false;
            }
        }
        #endregion

        public UInt32 Unknown1;
        public UInt32 Unknown2;
        public UInt32 Unknown3;
        public UInt64 NameHash; // should be the hash of FightDefinition.Name (from P3D)
        public string Path;
        public UInt32 Unknown4;

        public Fight.ContextBase Context;

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            if (input.ReadStringASCII(4) != "fig0")
            {
                throw new FormatException("not a fight file");
            }

            this.Flags = input.ReadValueU32();
            this.Unknown1 = input.ReadValueU32();

            if (input.ReadValueU64() != (UInt64)FightHash.Chunk)
            {
                throw new FormatException("containing fight type is not chunk");
            }

            this.Unknown2 = input.ReadValueU32();
            this.Unknown3 = input.ReadValueU32();

            this.NameHash = this.ReadHash(input);
            
            UInt64 contextHash = this.ReadHash(input);

            this.Path = input.ReadAlignedASCII();
            this.Unknown4 = input.ReadValueU32();

            if ((this.Flags & 1) == 0)
            {
                throw new Exception();
            }

            Type contextType = Fight.ContextCache.GetContext(contextHash);
            if (contextType == null)
            {
                throw new InvalidOperationException("unknown context type (" + FightHashes.Lookup(contextHash) + ")");
            }

            try
            {
                this.Context = (Fight.ContextBase)System.Activator.CreateInstance(contextType);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }

            this.Context.Deserialize(input, this);
        }
    }
}
