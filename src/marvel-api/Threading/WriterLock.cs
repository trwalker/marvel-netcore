using System;
using System.Threading;

namespace marvel_api.Threading
{
    public class WriterLock : IDisposable
    {
        private ReaderWriterLockSlim _slimLock;

        public WriterLock(ReaderWriterLockSlim slimLock)
        {
            _slimLock = slimLock;
        }

        public void EnterWriteLock()
        {
            _slimLock.EnterWriteLock();
        }

        public void Dispose()
        {
            _slimLock.ExitWriteLock();
        }
    }
}