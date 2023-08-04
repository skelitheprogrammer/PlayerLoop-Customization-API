using NUnit.Framework;
using PlayerLoopCustomizationAPI.Runtime;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;

namespace PlayerLoopCustomizationAPI.Tests.Runtime
{
    [TestFixture]
    internal class InsertLoopSystems
    {
        private struct CustomSystemAtBeginning
        {
        }

        private struct CustomSystemAtEnd
        {
        }

        private struct CustomSystemBefore
        {
        }

        private struct CustomSystemAfter
        {
        }

        private struct CustomNestedSystem
        {
        }

        [Test]
        public void InsertSystemAtBeginningOfUpdateLoop()
        {
            PlayerLoopSystem customSystem = new()
            {
                type = typeof(CustomSystemAtBeginning)
            };

            PlayerLoopSystem copyLoop = PlayerLoop.GetCurrentPlayerLoop();
            ref PlayerLoopSystem updateLoop = ref copyLoop.GetLoopSystem<Update>();

            PlayerLoopTestUtils.Log("Before ", updateLoop);

            updateLoop.InsertAtBeginning(customSystem);

            PlayerLoopTestUtils.Log("After", updateLoop);

            Assert.AreEqual(updateLoop.subSystemList[0].type, typeof(CustomSystemAtBeginning));
        }

        [Test]
        public void InsertSystemAtEndOfUpdateLoop()
        {
            PlayerLoopSystem customSystem = new()
            {
                type = typeof(CustomSystemAtEnd)
            };

            PlayerLoopSystem copyLoop = PlayerLoop.GetCurrentPlayerLoop();
            ref PlayerLoopSystem updateLoop = ref copyLoop.GetLoopSystem<Update>();

            PlayerLoopTestUtils.Log("Before ", updateLoop);

            updateLoop.InsertAtEnd(customSystem);

            PlayerLoopTestUtils.Log("After", updateLoop);

            Assert.AreEqual(updateLoop.subSystemList[^1].type, typeof(CustomSystemAtEnd));
        }

        [Test]
        public void InsertSystemBeforeScriptRunBehaviour()
        {
            PlayerLoopSystem customSystem = new()
            {
                type = typeof(CustomSystemBefore)
            };

            PlayerLoopSystem copyLoop = PlayerLoop.GetCurrentPlayerLoop();
            ref PlayerLoopSystem updateLoop = ref copyLoop.GetLoopSystem<Update>();

            PlayerLoopTestUtils.Log("Before ", updateLoop);

            updateLoop.InsertSystemBefore<Update.ScriptRunBehaviourUpdate>(customSystem);

            PlayerLoopTestUtils.Log("After", updateLoop);

            for (int i = 0; i < updateLoop.subSystemList.Length; i++)
            {
                if (updateLoop.subSystemList[i].type == typeof(Update.ScriptRunBehaviourUpdate))
                {
                    Assert.AreEqual(updateLoop.subSystemList[i - 1].type, typeof(CustomSystemBefore));
                    break;
                }
            }
        }

        [Test]
        public void InsertSystemAfterScriptRunBehaviour()
        {
            PlayerLoopSystem customSystem = new()
            {
                type = typeof(CustomSystemAfter)
            };

            PlayerLoopSystem copyLoop = PlayerLoop.GetCurrentPlayerLoop();
            ref PlayerLoopSystem updateLoop = ref copyLoop.GetLoopSystem<Update>();

            PlayerLoopTestUtils.Log("Before ", updateLoop);

            updateLoop.InsertSystemAfter<Update.ScriptRunBehaviourUpdate>(customSystem);

            PlayerLoopTestUtils.Log("After", updateLoop);

            for (int i = 0; i < updateLoop.subSystemList.Length; i++)
            {
                if (updateLoop.subSystemList[i].type == typeof(Update.ScriptRunBehaviourUpdate))
                {
                    Assert.AreEqual(updateLoop.subSystemList[i + 1].type, typeof(CustomSystemAfter));
                    break;
                }
            }
        }

        [Test]
        public void InsertSystemInsideNewSubSystems()
        {
            PlayerLoopSystem customSystem = new()
            {
                type = typeof(CustomNestedSystem)
            };

            PlayerLoopSystem copyLoop = PlayerLoop.GetCurrentPlayerLoop();
            ref PlayerLoopSystem scriptRunBehaviour = ref copyLoop.GetLoopSystem<Update>().GetLoopSystem<Update.ScriptRunBehaviourUpdate>();

            PlayerLoopTestUtils.Log("Before ", scriptRunBehaviour);

            PlayerLoopAPI.InsertSystemAt(ref scriptRunBehaviour, customSystem, 0);

            PlayerLoopTestUtils.Log("After", scriptRunBehaviour);

            Assert.AreEqual(scriptRunBehaviour.GetLoopSystem<CustomNestedSystem>().type, typeof(CustomNestedSystem));
        }
    }
}