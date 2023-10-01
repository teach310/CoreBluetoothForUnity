using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

namespace CoreBluetoothTests
{
    public abstract class CBTests
    {
        protected string validUUID1 = "EA521290-A651-4FA0-A958-0CE73F4DAE55";

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
