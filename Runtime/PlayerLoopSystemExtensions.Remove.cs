using System;
using UnityEngine.LowLevel;

namespace PlayerLoopExtender
{
    public static partial class PlayerLoopSystemExtensions
    {
        /// <summary>
        /// From https://github.com/Baste-RainGames/PlayerLoopInterface/blob/a4c15199ecb5f88b8a5009d0c391b967d768068c/Runtime/PlayerLoopInterface.cs#L136
        /// </summary>
        /// <param name="root"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool TryRemoveSystem(this ref PlayerLoopSystem root, Type type)
        {
            if (root.subSystemList == null)
            {
                return false;
            }

            for (int i = 0; i < root.subSystemList.Length; i++)
            {
                if (root.subSystemList[i].type == type)
                {
                    PlayerLoopSystem[] newSubSystems = new PlayerLoopSystem[root.subSystemList.Length - 1];

                    Array.Copy(root.subSystemList, newSubSystems, i);
                    Array.Copy(root.subSystemList, i + 1, newSubSystems, i, root.subSystemList.Length - i - 1);

                    root.subSystemList = newSubSystems;

                    return true;
                }

                if (TryRemoveSystem(ref root.subSystemList[i], type))
                {
                    return true;
                }
            }

            return false;
        }
    }
}