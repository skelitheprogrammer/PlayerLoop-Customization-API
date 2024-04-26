using System;
using NUnit.Framework;
using PlayerLoopExtender.Tests.Editor.Utils;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using static NUnit.Framework.Assert;
using static PlayerLoopExtender.Tests.Editor.Utils.TestUtils;

namespace PlayerLoopExtender.Tests.Editor
{
    internal sealed class InsertSystemTests
    {
        [Test]
        public void Insert_Before_Update()
        {
            Type anchorType = typeof(Update);
            Type insertType = typeof(TestSystemName);

            PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();

            copyLoop.InsertSystem(TestSystemName.Create(), anchorType, PlayerLoopSystemExtensions.InsertType.BEFORE);

            PlayerLoopSystem system = copyLoop.FindSystem(insertType);

            AreEqual(system.type, insertType);

            PassWithLog(copyLoop);
        }

        [Test]
        public void Insert_After_Update()
        {
            Type anchorType = typeof(Update);
            Type insertType = typeof(TestSystemName);

            PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();

            copyLoop.InsertSystem(TestSystemName.Create(), anchorType, PlayerLoopSystemExtensions.InsertType.AFTER);

            PlayerLoopSystem system = copyLoop.FindSystem(insertType);

            AreEqual(system.type, insertType);

            PassWithLog(copyLoop);
        }


        [Test]
        public void Insert_Before_Update_ScriptRunBehaviourUpdate()
        {
            Type anchorType = typeof(Update.ScriptRunBehaviourUpdate);
            Type insertType = typeof(TestSystemName);

            PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();

            copyLoop.InsertSystem(TestSystemName.Create(), anchorType, PlayerLoopSystemExtensions.InsertType.BEFORE);

            PlayerLoopSystem system = copyLoop.FindSystem(insertType);

            AreEqual(system.type, insertType);

            PassWithLog(copyLoop);
        }

        [Test]
        public void Insert_After_Update_ScriptRunBehaviourUpdate()
        {
            Type anchorType = typeof(Update.ScriptRunBehaviourUpdate);
            Type insertType = typeof(TestSystemName);

            PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();

            copyLoop.InsertSystem(TestSystemName.Create(), anchorType, PlayerLoopSystemExtensions.InsertType.AFTER);

            PlayerLoopSystem system = copyLoop.FindSystem(insertType);

            AreEqual(system.type, insertType);

            PassWithLog(copyLoop);
        }
        
    }
}