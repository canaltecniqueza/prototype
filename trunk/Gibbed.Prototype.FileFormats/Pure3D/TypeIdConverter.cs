﻿using System;
using System.ComponentModel;
using System.Globalization;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    internal class TypeIdConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(UInt32))
            {
                return string.Format("0x{0:X8}", value);
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string input = (string)value;

                if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    input = input.Substring(2);
                }

                return UInt32.Parse(input, NumberStyles.HexNumber, culture);
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }
}
