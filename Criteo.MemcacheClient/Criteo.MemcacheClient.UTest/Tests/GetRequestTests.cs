﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using Criteo.MemcacheClient.Requests;
using Criteo.MemcacheClient.Headers;
using Criteo.MemcacheClient.Exceptions;

namespace Criteo.MemcacheClient.UTest.Tests
{
    /// <summary>
    /// Test around the GetRequest object
    /// </summary>
    [TestFixture]
    public class GetRequestTests
    {
        static readonly byte[] GET_QUERY = 
        {
            0x80, 0x00, 0x00, 0x05,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x05,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x48, 0x65, 0x6c, 0x6c,
            0x6f,
        };

        static readonly byte[] GET_MAGIC = 
        {
            0xde, 0xad, 0xbe, 0xef,
        };

        static readonly byte[] GET_BAD_MAGIC = 
        {
            0xb, 0xad, 0xfe, 0xed,
        };

        static readonly byte[] GET_MESSAGE = 
        {
            0x57, 0x6f, 0x72, 0x6c,
            0x64,
        };

        [Test]
        public void GetRequestTest()
        {
            byte[] message = null;
            var request = new GetRequest { Key = @"Hello", RequestId = 0, Callback = (s, m) => message = m };

            var queryBuffer = request.GetQueryBuffer();

            CollectionAssert.AreEqual(GET_QUERY, queryBuffer, "The get query buffer is different of the expected one");

            var header = new MemcacheResponseHeader { Opcode = Opcode.Get, Status = Status.NoError };
            Assert.Throws(typeof(MemcacheException), () => request.HandleResponse(header, GET_BAD_MAGIC, GET_MESSAGE), "The get query doesn't detect bad magic");
            Assert.DoesNotThrow(() => request.HandleResponse(header, GET_MAGIC, GET_MESSAGE), "Handle request should not throw an exception");

            Assert.AreSame(GET_MESSAGE, message, "Sent message and the one return by the request are different");
        }
    }
}
