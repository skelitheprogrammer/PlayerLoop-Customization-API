using System;
using UnityEngine.LowLevel;

namespace PlayerLoopExtender
{
    public static partial class PlayerLoopSystemExtensions
    {
        public static ref PlayerLoopSystem FindSystem(this ref PlayerLoopSystem parent, Type searchType)
        {
            if (parent.subSystemList == null)
            {
                return ref parent;
            }

            for (int i = 0; i < parent.subSystemList.Length; i++)
            {
                ref PlayerLoopSystem system = ref FindSystem(ref parent.subSystemList[i], searchType);

                if (system.type == searchType)
                {
                    return ref system;
                }
            }

            return ref parent;
        }

        public static bool TryGetSystem(this ref PlayerLoopSystem parent, Type searchType, out PlayerLoopSystem seekSystem)
        {
            seekSystem = default;
            if (parent.subSystemList == null)
            {
                return false;
            }

            for (int i = 0; i < parent.subSystemList.Length; i++)
            {
                ref PlayerLoopSystem system = ref FindSystem(ref parent.subSystemList[i], searchType);

                if (system.type == searchType)
                {
                    seekSystem = system;
                    return true;
                }
            }

            return false;
        }
    }
}