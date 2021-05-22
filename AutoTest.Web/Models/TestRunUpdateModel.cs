// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoTest.Web.Models
{
    public class TestRunUpdateModel
    {
        public int TimeInMS { get; set; }

        public DateTime Created { get; set; }

        public ulong EntrantId { get; set; }

        public ulong MarshalId { get; set; }

        public IEnumerable<PenaltySaveModel> Penalties { get; set; } = Enumerable.Empty<PenaltySaveModel>();
    }
}
