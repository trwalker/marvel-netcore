using System;
using System.Threading;

namespace marvel_api.Threading
{
    public class ReaderLock : IDisposable
    {
        private ReaderWriterLockSlim _slimLock;

        public ReaderLock(ReaderWriterLockSlim slimLock)
        {
            _slimLock = slimLock;
        }

        public void EnterReadLock()
        {
            _slimLock.EnterReadLock();
        }

        public void Dispose()
        {
            _slimLock.ExitReadLock();
        }
    }
}