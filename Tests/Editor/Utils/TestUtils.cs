using System.Text;
using NUnit.Framework;
using UnityEngine.LowLevel;
using static PlayerLoopExtender.PlayerLoopLogUtils;

namespace PlayerLoopExtender.Tests.Editor.Utils
{
    internal static class TestUtils
    {
        private static readonly StringBuilder BUILDER = new();

        public static void PassWithLog(in PlayerLoopSystem copyLoop)
        {
            BUILDER.Clear();
            ListSystemNames(copyLoop, BUILDER);
            Assert.Pass(BUILDER.ToString());
        }
    }
}