using System;
using NUnit.Framework;
using PlayerLoopExtender.Tests.Editor.Utils;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using static NUnit.Framework.Assert;
using static PlayerLoopExtender.Tests.Editor.Utils.TestUtils;

namespace PlayerLoopExtender.Tests.Editor
{
    internal sealed class FindSystemTests
    {
        [Test]
        public void Find_Update()
        {
            PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();

            Type searchType = typeof(Update);

            PlayerLoopSystem searchSystem = copyLoop.FindSystem(searchType);

            AreEqual(searchType, searchSystem.type);

            PassWithLog(copyLoop);
        }

        [Test]
        public void Find_Update_ScriptRunBehaviourUpdate()
        {
            PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();

            Type searchType = typeof(Update.ScriptRunBehaviourUpdate);

            ref PlayerLoopSystem searchSystem = ref copyLoop.FindSystem(searchType);

            AreEqual(searchType, searchSystem.type);

            PassWithLog(copyLoop);
        }

        [Test]
        public void Try_Find_Update_ScriptRunBehaviourUpdate()
        {
            PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();

            Type searchType = typeof(Update.ScriptRunBehaviourUpdate);

            bool result = copyLoop.TryGetSystem(searchType, out PlayerLoopSystem searchSystem);

            IsTrue(result);
            AreEqual(searchType, searchSystem.type);

            PassWithLog(copyLoop);
        }

        [Test]
        public void Try_Find_TestSystemName_Fail()
        {
            PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();

            Type searchType = typeof(TestSystemName);

            bool result = copyLoop.TryGetSystem(searchType, out PlayerLoopSystem _);

            IsFalse(result);

            PassWithLog(copyLoop);
        }
    }
}