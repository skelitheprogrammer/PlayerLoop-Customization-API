using UnityEditor;
using UnityEngine.LowLevel;
using static PlayerLoopExtender.PlayerLoopLogUtils;

namespace PlayerLoopExtender
{
    internal static class PlayerLoopMenuCommands
    {
        [MenuItem("PlayerLoop/Get Default Loop List")]
        private static void LogDefaultLoop() => LogLoopSystem(PlayerLoop.GetDefaultPlayerLoop());

        [MenuItem("PlayerLoop/Get Current Loop List")]
        private static void LogCurrentLoop() => LogLoopSystem(PlayerLoop.GetCurrentPlayerLoop());

        [MenuItem("PlayerLoop/Reset To Default Player Loop")]
        private static void ResetToDefault() => PlayerLoop.SetPlayerLoop(PlayerLoop.GetDefaultPlayerLoop());
    }
}