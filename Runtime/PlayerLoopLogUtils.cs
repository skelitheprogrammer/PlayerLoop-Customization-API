using System.Text;
using UnityEngine;
using UnityEngine.LowLevel;

namespace PlayerLoopExtender
{
    public static class PlayerLoopLogUtils
    {
        private static readonly StringBuilder EDITOR_LOG_BUILDER = new();

        public static void LogLoopSystem(in PlayerLoopSystem system)
        {
            EDITOR_LOG_BUILDER.Clear();
            ListSystemNames(system, EDITOR_LOG_BUILDER);
            Debug.Log(EDITOR_LOG_BUILDER);
        }

        public static void ListSystemNames(PlayerLoopSystem playerLoopSystem, StringBuilder builder, int inline = 0)
        {
            if (playerLoopSystem.type != null)
            {
                builder.AppendLine();
                for (int i = 0; i < inline; i++)
                {
                    builder.Append("   ");
                }

                builder.Append(playerLoopSystem.type.Name);
            }

            if (playerLoopSystem.subSystemList != null)
            {
                inline++;
                for (int index = 0; index < playerLoopSystem.subSystemList.Length; index++)
                {
                    ListSystemNames(playerLoopSystem.subSystemList[index], builder, inline);
                }
            }
        }

        public static string GetSystemName(this ref PlayerLoopSystem system) => system.type == null
            ? "[EmptyName]"
            : system.type.Name;
    }
}