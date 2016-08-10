using System;
using System.Threading;

namespace marvel_api.Threading
{
    public class ReaderWriterLock
    {
        private readonly ReaderWriterLockSlim _slimLock;
        public ReaderWriterLock()
        {
            _slimLock = new ReaderWriterLockSlim();
        }

        public IDisposable EnterReadLock()
        {
            var readerLock = new ReaderLock(_slimLock);
            readerLock.EnterReadLock();

            return readerLock;
        }

        public IDisposable EnterWriteLock()
        {
            var writerLock = new WriterLock(_slimLock);
            writerLock.EnterWriteLock();

            return writerLock;
        }
    }
}