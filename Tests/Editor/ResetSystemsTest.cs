using NUnit.Framework;
using PlayerLoopExtender.Tests.Editor.Utils;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using static PlayerLoopExtender.Tests.Editor.Utils.TestUtils;

namespace PlayerLoopExtender.Tests.Editor
{
    internal sealed class ResetSystemsTest
    {
        [Test]
        public void ResetInsertedSystem()
        {
            PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();

            PlayerLoopSystem playerLoopSystem = TestSystemName.Create(UpdateDelegate);

            copyLoop.InsertSystem(playerLoopSystem, typeof(Update.ScriptRunBehaviourUpdate), PlayerLoopSystemExtensions.InsertType.BEFORE);

            ref PlayerLoopSystem system = ref copyLoop.FindSystem(typeof(TestSystemName));

            Assert.IsNotNull(system.updateDelegate);
            system.updateDelegate = null;

            Assert.IsNull(copyLoop.FindSystem(typeof(TestSystemName)).updateDelegate);

            PassWithLog(copyLoop);

            static void UpdateDelegate()
            {
            }
        }
    }
}