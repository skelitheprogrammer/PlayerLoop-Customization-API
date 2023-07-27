using System;
using NUnit.Framework;
using PlayerLoopCustomizationAPI.Runtime;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace PlayerLoopCustomizationAPI.Tests.Runtime
{
    [TestFixture]
    internal class GetLoopSystems
    {
        private struct CustomSystemName
        {
        }

        [Test]
        public void GetUpdateLoop()
        {
            PlayerLoopSystem updateLoop = PlayerLoopAPI.GetLoopSystem<Update>();
            Assert.AreSame(typeof(Update), updateLoop.type);
        }

        [Test]
        public void GetUpdateLoopUsingExtensions()
        {
            PlayerLoopSystem copyLoop = PlayerLoopAPI.GetCustomPlayerLoop();
            PlayerLoopSystem updateLoop = copyLoop.GetLoopSystem<Update>();
            Assert.AreSame(typeof(Update), updateLoop.type);
        }

        [Test]
        public void GetSubSystemInUpdate()
        {
            PlayerLoopSystem subSystem = PlayerLoopAPI.GetLoopSystem<Update>().GetLoopSystem<Update.ScriptRunBehaviourUpdate>();
            Assert.AreSame(typeof(Update.ScriptRunBehaviourUpdate), subSystem.type);
        }

        [Test]
        public void GetSubSystemInUpdateUsingExtensions()
        {
            PlayerLoopSystem subSystem = PlayerLoopAPI.GetLoopSystem<Update>().GetLoopSystem<Update.ScriptRunBehaviourUpdate>();
            Assert.AreSame(typeof(Update.ScriptRunBehaviourUpdate), subSystem.type);
        }

        [Test]
        public void FailGetWrongSystem()
        {
            Assert.Catch<NullReferenceException>(() => PlayerLoopAPI.GetLoopSystem<CustomSystemName>());
        }

        [Test]
        public void FailGetWrongSystemUsingExtensions()
        {
            PlayerLoopSystem copyLoop = PlayerLoopAPI.GetCustomPlayerLoop();
            Assert.Catch<NullReferenceException>(() => copyLoop.GetLoopSystem<CustomSystemName>());
        }
    }
}