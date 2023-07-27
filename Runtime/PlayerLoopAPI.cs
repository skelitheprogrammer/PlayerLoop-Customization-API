#define PLAYERLOOPBUILDERAPI_EXPERIMENTAL
using System;
using UnityEngine;
using UnityEngine.LowLevel;

namespace PlayerLoopCustomizationAPI.Runtime
{
    public static class PlayerLoopAPI
    {
        private static PlayerLoopSystem _customPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

        public static ref PlayerLoopSystem GetCustomPlayerLoop()
        {
            return ref _customPlayerLoop;
        }

        public static ref PlayerLoopSystem GetLoopSystem<T>() where T : struct
        {
            return ref GetLoopSystem<T>(ref _customPlayerLoop);
        }

        public static ref PlayerLoopSystem GetLoopSystem<T>(ref PlayerLoopSystem loopSystem) where T : struct
        {
            if (loopSystem.subSystemList == null)
            {
                throw new NullReferenceException($"Empty subSystemList in {loopSystem.type.Name} system");
            }

            for (int i = 0; i < loopSystem.subSystemList.Length; i++)
            {
                if (loopSystem.subSystemList[i].type == typeof(T))
                {
                    return ref loopSystem.subSystemList[i];
                }
            }

            throw new NullReferenceException($"System {typeof(T).Name} is not presented in {(loopSystem.type != null ? loopSystem.type : "MainPlayerLoop")} system");
        }

        public static ref PlayerLoopSystem InsertSystemAt(ref PlayerLoopSystem loopSystem, in PlayerLoopSystem newSystem, int index)
        {
            if (loopSystem.subSystemList == null)
            {
                loopSystem.subSystemList = new PlayerLoopSystem[1];
                loopSystem.subSystemList[0] = newSystem;
                return ref loopSystem;
            }
            
            PlayerLoopSystem[] updatedLoop = new PlayerLoopSystem[loopSystem.subSystemList.Length + 1];

            for (int i = 0; i < updatedLoop.Length; i++)
            {
                if (i == index)
                {
                    updatedLoop[i] = newSystem;
                }
                else if (i < index)
                {
                    updatedLoop[i] = loopSystem.subSystemList[i];
                }
                else
                {
                    updatedLoop[i] = loopSystem.subSystemList[i - 1];
                }
            }

            loopSystem.subSystemList = updatedLoop;
            return ref loopSystem;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BuildInternal()
        {
            HandleSubscription();

            PlayerLoop.SetPlayerLoop(_customPlayerLoop);
            Debug.Log("Build");

            static void HandleSubscription()
            {
                Application.quitting -= Reset;
                Application.quitting += Reset;
            }
        }

        private static void Reset()
        {
            PlayerLoop.SetPlayerLoop(PlayerLoop.GetDefaultPlayerLoop());
        }

#if PLAYERLOOPBUILDERAPI_EXPERIMENTAL
        
        public static ref PlayerLoopSystem Query<T>(ref PlayerLoopSystem loopSystem) where T : struct
        {
            if (loopSystem.subSystemList == null)
            {
                return ref loopSystem;
            }

            for (int i = 0; i < loopSystem.subSystemList.Length; i++)
            {
                if (loopSystem.subSystemList[i].type == typeof(T))
                {
                    return ref loopSystem.subSystemList[i];
                }

                GetLoopSystem<T>(ref loopSystem.subSystemList[i]);
            }

            return ref loopSystem;
        }
        
#endif
    }
}