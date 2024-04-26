using System;
using NUnit.Framework;
using PlayerLoopExtender.Tests.Editor.Utils;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using static PlayerLoopExtender.Tests.Editor.Utils.TestUtils;
using static UnityEngine.LowLevel.PlayerLoop;

namespace PlayerLoopExtender.Tests.Editor
{
    internal sealed class TryRemoveTests
    {
        [Test]
        public void Remove_Update()
        {
            PlayerLoopSystem copyLoop = GetDefaultPlayerLoop();

            Type removeTarget = typeof(Update);
            copyLoop.TryRemoveSystem(removeTarget);

            bool result = copyLoop.TryGetSystem(removeTarget, out _);

            Assert.IsFalse(result);
        }

        [Test]
        public void Remove_Update_ScriptRunBehaviourUpdate()
        {
            PlayerLoopSystem copyLoop = GetDefaultPlayerLoop();

            Type removeTarget = typeof(Update.ScriptRunBehaviourUpdate);
            copyLoop.TryRemoveSystem(removeTarget);
            
            bool result = copyLoop.TryGetSystem(removeTarget, out _);

            Assert.IsFalse(result);
        }

        [Test]
        public void Remove_TestSystemName()
        {
            PlayerLoopSystem copyLoop = GetDefaultPlayerLoop();

            Type updateType = typeof(Update);

            copyLoop.InsertSystem(TestSystemName.Create(), updateType, PlayerLoopSystemExtensions.InsertType.AFTER);
            bool result = copyLoop.TryRemoveSystem(typeof(TestSystemName));

            Assert.IsTrue(result);
            PassWithLog(copyLoop);
        }
    }
}