using System;
using System.Diagnostics;
using System.Threading;

namespace DevText.Framework.Extensions
{

    public static class ReaderWriterLockSlimExtensions
    {
        public static IDisposable ReadAndMaybeWrite(this ReaderWriterLockSlim instance)
        {
            
            instance.EnterUpgradeableReadLock();

            return new SynchronizedCodeBlock(instance.ExitUpgradeableReadLock);
        }

        public static IDisposable Read(this ReaderWriterLockSlim instance)
        {
            instance.EnterReadLock();

            return new SynchronizedCodeBlock(instance.ExitReadLock);
        }

        public static IDisposable Write(this ReaderWriterLockSlim instance)
        {
            
            instance.EnterWriteLock();

            return new SynchronizedCodeBlock(instance.ExitWriteLock);
        }

        private sealed class SynchronizedCodeBlock : IDisposable
        {
            private readonly Action action;

            public SynchronizedCodeBlock(Action action)
            {
                this.action = action;
            }

            public void Dispose()
            {
                action();
            }
        }
    }
}
