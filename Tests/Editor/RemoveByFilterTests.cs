using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine.LowLevel;
using static PlayerLoopExtender.Filter.PlayerLoopSystemFilter;
using static NUnit.Framework.Assert;
using static PlayerLoopExtender.Tests.Editor.Utils.TestUtils;

namespace PlayerLoopExtender.Tests.Editor
{
    internal sealed class RemoveByFilterTests
    {
        [Test]
        public void Remove_XR_Systems()
        {
            PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();

            foreach (Type type in XR)
            {
                copyLoop.TryRemoveSystem(type);
            }

            foreach (Type type in XR)
            {
                bool result = copyLoop.TryGetSystem(type, out _);
                IsFalse(result);
            }

            PassWithLog(copyLoop);
        }

        [Test]
        public void Remove_XR_And_AR_Systems()
        {
            PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();

            Type[] filterCombined = XR.Concat(AR).ToArray();

            foreach (Type type in filterCombined)
            {
                copyLoop.TryRemoveSystem(type);
            }

            foreach (Type type in filterCombined)
            {
                bool result = copyLoop.TryGetSystem(type, out _);
                IsFalse(result);
            }

            PassWithLog(copyLoop);
        }
    }
}