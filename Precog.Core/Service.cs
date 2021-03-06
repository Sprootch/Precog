﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Precog.Core
{
    [DebuggerDisplay("{Address} - {Identity}")]
    public class Service : IEquatable<Service>
    {
        public string Address { get; }
        public string Identity { get; }
        public string Binding { get; }

        public Service(string address, string identity, string binding = null)
        {
            Address = address;
            Identity = identity;
            Binding = binding;
        }

        public bool IsLocalHost => Address.Contains("localhost");

        public override string ToString()
        {
            return $"- {Address}{Environment.NewLine}- {Identity ?? "Undefined"}";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Service);
        }

        public bool Equals(Service other)
        {
            return other != null &&
                   Address == other.Address &&
                   Identity == other.Identity;
        }

        public override int GetHashCode()
        {
            var hashCode = 1769866918;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Address);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Identity);
            return hashCode;
        }
    }
}
