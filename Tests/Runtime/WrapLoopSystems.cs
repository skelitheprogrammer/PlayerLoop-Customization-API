using NUnit.Framework;
using PlayerLoopCustomizationAPI.Runtime;
using PlayerLoopCustomizationAPI.Utils;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace PlayerLoopCustomizationAPI.Tests.Runtime
{
    [TestFixture]
    internal class WrapLoopSystems
    {
        private struct CustomBeforeSystemName
        {
        }

        private struct CustomAfterSystemName
        {
        }

        [Test]
        public void WrapSystemsAroundScripRunBehaviour()
        {
            PlayerLoopSystem[] customSystems = CreateSystems();

            PlayerLoopSystem copyLoop = PlayerLoop.GetCurrentPlayerLoop();
            ref PlayerLoopSystem updateLoop = ref copyLoop.GetLoopSystem<Update>();

            PlayerLoopTestUtils.Log("Before", updateLoop);

            updateLoop.WrapSystemsAt<Update.ScriptRunBehaviourUpdate>(customSystems[0], customSystems[1]);

            PlayerLoopTestUtils.Log("After", updateLoop);

            for (int i = 0; i < updateLoop.subSystemList.Length; i++)
            {
                ref PlayerLoopSystem loopSystem = ref updateLoop.subSystemList[i];

                if (loopSystem.type != typeof(Update.ScriptRunBehaviourUpdate))
                {
                    continue;
                }

                ref PlayerLoopSystem beforeSystem = ref updateLoop.subSystemList[i - 1];
                ref PlayerLoopSystem afterSystem = ref updateLoop.subSystemList[i + 1];

                Assert.AreEqual(typeof(CustomBeforeSystemName), beforeSystem.type);
                Assert.AreEqual(typeof(CustomAfterSystemName), afterSystem.type);
            }
        }

        [Test]
        public void WrapSystemsAroundWholeUpdateSystem()
        {
            PlayerLoopSystem[] customSystems = CreateSystems();

            PlayerLoopSystem copyLoop = PlayerLoop.GetCurrentPlayerLoop();

            ref PlayerLoopSystem updateLoop = ref copyLoop.GetLoopSystem<Update>();

            PlayerLoopTestUtils.Log("Before", updateLoop);

            updateLoop.WrapSystems(customSystems[0], customSystems[1]);

            PlayerLoopTestUtils.Log("After", updateLoop);

            PlayerLoopSystem firstSystem = updateLoop.subSystemList[0];
            PlayerLoopSystem lastSystem = updateLoop.subSystemList[^1];

            Assert.AreEqual(typeof(CustomBeforeSystemName), firstSystem.type);
            Assert.AreEqual(typeof(CustomAfterSystemName), lastSystem.type);
        }

        private static PlayerLoopSystem[] CreateSystems()
        {
            return new PlayerLoopSystem[]
            {
                new()
                {
                    type = typeof(CustomBeforeSystemName)
                },

                new()
                {
                    type = typeof(CustomAfterSystemName)
                }
            };
        }
    }

    internal static class PlayerLoopTestUtils
    {
        internal static void Log(string prefix, in PlayerLoopSystem playerLoopSystem)
        {
            Debug.Log($"{prefix} \n {PlayerLoopUtils.ShowLoopSystems(playerLoopSystem)}");
        }
    }
}