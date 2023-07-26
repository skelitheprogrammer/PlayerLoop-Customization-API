using NUnit.Framework;
using PlayerLoopCustomizationAPI.Runtime;
using PlayerLoopCustomizationAPI.Utils;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace PlayerLoopCustomizationAPI.Tests.PlayMode
{
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

        private PlayerLoopSystem _customSystemAtBeginning;
        private PlayerLoopSystem _customSystemAtEnd;
        private PlayerLoopSystem _customSystemBefore;
        private PlayerLoopSystem _customSystemAfter;

        private PlayerLoopSystem _currentLoopSystem;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            InitSystems();
            InsertSystems();

            _currentLoopSystem = PlayerLoop.GetCurrentPlayerLoop();

            void InitSystems()
            {
                _customSystemAtBeginning = new()
                {
                    type = typeof(CustomSystemAtBeginning)
                };

                _customSystemAtEnd = new()
                {
                    type = typeof(CustomSystemAtEnd)
                };

                _customSystemBefore = new()
                {
                    type = typeof(CustomSystemBefore)
                };

                _customSystemAfter = new()
                {
                    type = typeof(CustomSystemAfter)
                };
            }

            void InsertSystems()
            {
                ref PlayerLoopSystem copyLoop = ref PlayerLoopAPI.GetCustomPlayerLoop();

                copyLoop.GetLoopSystem<Update>()
                    .InsertAtBeginning(_customSystemAtBeginning)
                    .InsertAtEnd(_customSystemAtEnd)
                    .InsertSystemBefore<Update.ScriptRunBehaviourUpdate>(_customSystemBefore)
                    .InsertSystemAfter<Update.ScriptRunBehaviourUpdate>(_customSystemAfter)
                    ;

                PlayerLoop.SetPlayerLoop(copyLoop);
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Debug.Log(PlayerLoopUtils.ShowLoopSystems(PlayerLoop.GetCurrentPlayerLoop()));
            PlayerLoop.SetPlayerLoop(PlayerLoop.GetDefaultPlayerLoop());
            Debug.Log(PlayerLoopUtils.ShowLoopSystems(PlayerLoop.GetCurrentPlayerLoop()));

            _customSystemAtBeginning = default;
            _customSystemAtEnd = default;
            _customSystemBefore = default;
            _customSystemAfter = default;
        }

        [Test]
        public void InsertSystemAtBeginningOfUpdateLoop()
        {
            Assert.IsNotNull(_currentLoopSystem.GetLoopSystem<Update>().GetLoopSystem<CustomSystemAtBeginning>());
        }

        [Test]
        public void InsertSystemAtEndOfUpdateLoop()
        {
            Assert.IsNotNull(_currentLoopSystem.GetLoopSystem<Update>().GetLoopSystem<CustomSystemAtEnd>());
        }

        [Test]
        public void InsertSystemBeforeScriptRunBehaviour()
        {
            Assert.IsNotNull(_currentLoopSystem.GetLoopSystem<Update>().GetLoopSystem<CustomSystemBefore>());
        }

        [Test]
        public void InsertSystemAfterScriptRunBehaviour()
        {
            Assert.IsNotNull(_currentLoopSystem.GetLoopSystem<Update>().GetLoopSystem<CustomSystemAfter>());
        }
    }
}