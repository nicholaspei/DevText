using System;
using System.Diagnostics;

namespace DevText.Framework.Events
{
    public class SubscriptionToken:IEquatable<SubscriptionToken>
    {
        private readonly Guid token = Guid.NewGuid();

        public bool Equals(SubscriptionToken other)
        {
            return (other != null) && Equals(token, other.token);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || Equals(obj as SubscriptionToken);
        }

        public override int GetHashCode()
        {
            return token.GetHashCode();
        }

        public override string ToString()
        {
            return token.ToString();
        }

    }
}
