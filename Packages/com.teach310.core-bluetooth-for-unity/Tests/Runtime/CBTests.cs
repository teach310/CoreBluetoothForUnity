using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

namespace CoreBluetoothTests
{
    public abstract class CBTests
    {
        protected string validUUID1 = "EA521290-A651-4FA0-A958-0CE73F4DAE55";
        protected string validUUID2 = "C5D90910-62A3-4316-A827-E7925BA9B0B4";
        protected string validUUID3 = "B11F52DE-9D40-4832-B85E-438136538785";

        protected IEnumerator WaitUntilWithTimeout(Func<bool> predicate, float timeout)
        {
            var startTime = Time.realtimeSinceStartup;
            while (!predicate())
            {
                if (Time.realtimeSinceStartup - startTime > timeout)
                {
                    Assert.Fail("Timeout");
                    yield break;
                }
                yield return null;
            }
        }
    }
}
