﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Runtime.InteropServices;
using Gibbed.Helpers;
using Gibbed.Prototype.Helpers;
using System.Xml.Serialization;

namespace Gibbed.Prototype.FileFormats.Fight.Condition
{
    [KnownCondition(typeof(Context.Scenario), "hasTag")]
    public class HasTag : ConditionBase
    {
        public ScenarioGameObjectSlot Affiliate;
        public string Tag;

        public override void Serialize(Stream output, FightFile fight)
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(Stream input, FightFile fight)
        {
            this.Affiliate = fight.ReadPropertyEnum<ScenarioGameObjectSlot>(input);
            this.Tag = fight.ReadPropertyString(input);
        }
    }
}