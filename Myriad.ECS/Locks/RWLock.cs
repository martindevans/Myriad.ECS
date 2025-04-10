﻿namespace Myriad.ECS.Locks;

internal class RWLock<T>(T value)
    where T : class
{
    private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.SupportsRecursion);

    public ReadLockHandle EnterReadLock()
    {
        _lock.EnterReadLock();
        return new ReadLockHandle(_lock, value);
    }

    public WriteLockHandle EnterWriteLock()
    {
        _lock.EnterWriteLock();
        return new WriteLockHandle(_lock, value);
    }

    public readonly ref struct ReadLockHandle
    {
        private readonly ReaderWriterLockSlim _lock;
        public readonly T Value;

        internal ReadLockHandle(ReaderWriterLockSlim @lock, T value)
        {
            _lock = @lock;
            Value = value;
        }

        public void Dispose()
        {
            _lock.ExitReadLock();
        }
    }

    public readonly ref struct WriteLockHandle
    {
        private readonly ReaderWriterLockSlim _lock;
        public readonly T Value;

        internal WriteLockHandle(ReaderWriterLockSlim @lock, T value)
        {
            _lock = @lock;
            Value = value;
        }

        public void Dispose()
        {
            _lock.ExitWriteLock();
        }
    }
}