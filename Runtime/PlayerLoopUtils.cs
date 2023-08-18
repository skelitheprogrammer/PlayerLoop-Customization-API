using UnityEngine.LowLevel;

namespace PlayerLoopCustomizationAPI.Utils
{
    public static class PlayerLoopUtils
    {
        public static PlayerLoopSystem CreateSystem<T>(PlayerLoopSystem.UpdateFunction updateDelegate = null) where T : struct
        {
            return new()
            {
                type = typeof(T),
                updateDelegate = updateDelegate
            };
        }
    }
}