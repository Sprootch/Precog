using System;
using System.Collections.Generic;

namespace Precog.Core
{
    public class Service : IEquatable<Service>
    {
        public string Address { get; private set; }
        public string Identity { get; private set; }

        public Service(string address, string identity)
        {
            this.Address = address;
            this.Identity = identity;
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
