using System;

namespace DevText.Framework.Events
{
    public interface IDelegateReference
    {
        Delegate Target
        {
            get;
        }
    }
}
