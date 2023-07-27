using NUnit.Framework;
using PlayerLoopCustomizationAPI.Runtime;
using PlayerLoopCustomizationAPI.Utils;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

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
        
        private PlayerLoopSystem _customSystemAtBeginning;
        private PlayerLoopSystem _customSystemAtEnd;
        private PlayerLoopSystem _customSystemBefore;
        private PlayerLoopSystem _customSystemAfter;
        private PlayerLoopSystem _customNestedSystem;

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

                _customNestedSystem = new()
                {
                    type = typeof(CustomNestedSystem)
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

                copyLoop.GetLoopSystem<Update>().GetLoopSystem<Update.ScriptRunBehaviourUpdate>().InsertAtBeginning(_customNestedSystem);
                
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
            _currentLoopSystem = default;
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

        [Test]
        public void InsertSystemInsideNewSubSystems()
        {
            Assert.IsNotNull(_currentLoopSystem.GetLoopSystem<Update>().GetLoopSystem<Update.ScriptRunBehaviourUpdate>().GetLoopSystem<CustomNestedSystem>());
        }
    }
}