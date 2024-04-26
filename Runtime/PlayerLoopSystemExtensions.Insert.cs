using System;
using UnityEngine.LowLevel;

namespace PlayerLoopExtender
{
    public static partial class PlayerLoopSystemExtensions
    {
        public enum InsertType
        {
            BEFORE,
            AFTER,
        }

        public static void InsertSystem(this ref PlayerLoopSystem root, in PlayerLoopSystem toInsert, Type insertTarget, InsertType insertType)
        {
            if (root.subSystemList == null)
            {
                return;
            }

            int indexOfTarget = -1;
            for (int i = 0; i < root.subSystemList.Length; i++)
            {
                if (root.subSystemList[i].type == insertTarget)
                {
                    indexOfTarget = i;
                    break;
                }
            }

            if (indexOfTarget != -1)
            {
                InsertSystemImpl(ref root, toInsert, indexOfTarget, insertType);
            }
            else
            {
                for (int i = 0; i < root.subSystemList.Length; i++)
                {
                    InsertSystem(ref root.subSystemList[i], toInsert, insertTarget, insertType);
                }
            }
        }

        private static void InsertSystemImpl(ref PlayerLoopSystem loopSystem, in PlayerLoopSystem toInsert, int indexOfTarget, InsertType insertType)
        {
            PlayerLoopSystem[] newSubSystems = new PlayerLoopSystem[loopSystem.subSystemList.Length + 1];

            int insertIndex = insertType == InsertType.BEFORE
                ? indexOfTarget
                : indexOfTarget + 1;

            if (insertIndex > 0)
            {
                Array.Copy(loopSystem.subSystemList, newSubSystems, insertIndex);
            }

            newSubSystems[insertIndex] = toInsert;

            if (insertIndex < loopSystem.subSystemList.Length)
            {
                Array.Copy(loopSystem.subSystemList, insertIndex, newSubSystems, insertIndex + 1, loopSystem.subSystemList.Length - insertIndex);
            }

            loopSystem.subSystemList = newSubSystems;
        }
    }
}