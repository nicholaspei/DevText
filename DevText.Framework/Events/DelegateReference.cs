using System;
using System.Diagnostics;
using System.Reflection;

namespace DevText.Framework.Events
{
    public class DelegateReference:IDelegateReference
    {
        private readonly Delegate @delegate;
        private readonly WeakReference weakReference;
        private readonly MethodInfo method;
        private readonly Type delegateType;

        public DelegateReference(Delegate @delegate,bool keepReferenceAlive)
        {
            if (keepReferenceAlive)
            {
                this.@delegate = @delegate;
            }
            else
            {
                weakReference = new WeakReference(@delegate.Target);
                method = @delegate.Method;
                delegateType = @delegate.GetType();
            }
        }

        public Delegate Target
        {
            get 
            {
                return @delegate ?? TryGetDelegate();
            }
        }

        private Delegate TryGetDelegate()
        {
            if (method.IsStatic)
            {
                return Delegate.CreateDelegate(delegateType,null, method);
            }

            object target = weakReference.Target;
            return (target != null) ? Delegate.CreateDelegate(delegateType, target, method) : null;
        }
    }
}
