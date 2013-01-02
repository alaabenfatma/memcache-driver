﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

using Criteo.Memcache.Node;
using Criteo.Memcache.Requests;

namespace Criteo.Memcache.UTest.Mocks
{
    class NodeQueueMock : IMemcacheRequestsQueue
    {
        BlockingCollection<IMemcacheRequest> _queue = new BlockingCollection<IMemcacheRequest>();

        public IMemcacheRequest Take()
        {
            return _queue.Take();
        }

        public bool TryTake(out IMemcacheRequest request, int timeout)
        {
            return _queue.TryTake(out request, timeout);
        }

        public void Add(IMemcacheRequest request)
        {
            _queue.Add(request);
        }
    }
}