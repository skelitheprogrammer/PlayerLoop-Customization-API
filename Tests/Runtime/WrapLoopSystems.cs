using System.Security.Cryptography;
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

        private PlayerLoopSystem _customBeforeSystem;
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
                _customBeforeSystem = new()
                {
                    type = typeof(CustomBeforeSystemName)
                };

                _customSystemAfter = new()
                {
                    type = typeof(CustomAfterSystemName)
                };
            }

            void InsertSystems()
            {
                ref PlayerLoopSystem copyLoop = ref PlayerLoopAPI.GetCustomPlayerLoop();

                copyLoop.GetLoopSystem<Update>().WrapSystemsAt<Update.ScriptRunBehaviourUpdate>(_customBeforeSystem, _customSystemAfter);

                PlayerLoop.SetPlayerLoop(copyLoop);
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Debug.Log(PlayerLoopUtils.ShowLoopSystems(PlayerLoop.GetCurrentPlayerLoop()));
            PlayerLoop.SetPlayerLoop(PlayerLoop.GetDefaultPlayerLoop());
            Debug.Log(PlayerLoopUtils.ShowLoopSystems(PlayerLoop.GetCurrentPlayerLoop()));

            _customBeforeSystem = default;
            _customSystemAfter = default;
            _customSystemAfter = default;
            _currentLoopSystem = default;
        }

        [Test]
        public void WrapSystemAroundScripRunBehaviour()
        {
            var updateLoop = _currentLoopSystem.GetLoopSystem<Update>();
            
            Assert.IsNotNull(updateLoop.GetLoopSystem<CustomBeforeSystemName>());
            Assert.IsNotNull(updateLoop.GetLoopSystem<CustomAfterSystemName>());
        }
    }
}