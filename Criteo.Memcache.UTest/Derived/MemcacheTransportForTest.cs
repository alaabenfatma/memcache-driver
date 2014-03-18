﻿/* Licensed to the Apache Software Foundation (ASF) under one
   or more contributor license agreements.  See the NOTICE file
   distributed with this work for additional information
   regarding copyright ownership.  The ASF licenses this file
   to you under the Apache License, Version 2.0 (the
   "License"); you may not use this file except in compliance
   with the License.  You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing,
   software distributed under the License is distributed on an
   "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
   KIND, either express or implied.  See the License for the
   specific language governing permissions and limitations
   under the License.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Criteo.Memcache.Requests;
using Criteo.Memcache.Authenticators;
using Criteo.Memcache.Configuration;
using Criteo.Memcache.Headers;
using Criteo.Memcache.Exceptions;
using Criteo.Memcache.Transport;

namespace Criteo.Memcache.UTest
{
    internal class MemcacheTransportForTest : MemcacheTransport
    {
        public bool _disposed = false;
        public Action OnDispose { private get; set; }

        public MemcacheTransportForTest(EndPoint endpoint, MemcacheClientConfiguration clientConfig, Action<IMemcacheTransport> registerEvents, Action<IMemcacheTransport> transportAvailable, bool planToConnect, Func<bool> nodeClosing, Action onCreate, Action onDispose)
            : base(endpoint, clientConfig, registerEvents, transportAvailable, planToConnect, nodeClosing)
        {
            if (onCreate != null)
                onCreate();

            OnDispose = onDispose;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                if (OnDispose != null)
                    OnDispose();
                base.Dispose(disposing);
            }
        }

        public override string ToString()
        {
            return "MemcacheTransportForTest " + GetHashCode();
        }
    }
}
