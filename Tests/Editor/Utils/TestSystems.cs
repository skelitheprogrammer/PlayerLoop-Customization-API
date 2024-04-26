using UnityEngine.LowLevel;

namespace PlayerLoopExtender.Tests.Editor.Utils
{
    internal struct TestSystemName
    {
        public static PlayerLoopSystem Create(PlayerLoopSystem.UpdateFunction updateFunction = null) => new()
        {
            type = typeof(TestSystemName),
            updateDelegate = updateFunction
        };
    }
}