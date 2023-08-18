using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;

namespace PlayerLoopCustomizationAPI.Editor.Utils
{
    public static class PlayerLoopEditorLog
    {
        [MenuItem("PlayerLoopUtils/Log PlayerLoop")]
        private static void Call()
        {
            Debug.Log(GetLoopSystemsText());
        }        
        
        public static string GetLoopSystemsText(int inline = 0)
        {
            StringBuilder sb = new();
            ShowLoopSystemsInternal(PlayerLoop.GetCurrentPlayerLoop(), sb, inline);
            return sb.ToString();
        }

        public static string GetLoopSystemsText(PlayerLoopSystem playerLoopSystem, int inline = 0)
        {
            StringBuilder sb = new();
            ShowLoopSystemsInternal(playerLoopSystem, sb, inline);
            return sb.ToString();
        }

        private static void ShowLoopSystemsInternal(in PlayerLoopSystem playerLoopSystem, StringBuilder stringBuilder, int inline = 0)
        {
            if (playerLoopSystem.type != null)
            {
                for (int i = 0; i < inline; i++)
                {
                    stringBuilder.Append("\t");
                }

                stringBuilder.AppendLine(playerLoopSystem.type.Name);
            }

            if (playerLoopSystem.subSystemList == null)
            {
                return;
            }

            inline++;

            foreach (PlayerLoopSystem subSystem in playerLoopSystem.subSystemList)
            {
                ShowLoopSystemsInternal(subSystem, stringBuilder, inline);
            }
        }
    }
}