using System;
using UnityEngine.PlayerLoop;

namespace PlayerLoopExtender.Filter
{
    public static partial class PlayerLoopSystemFilter
    {
        public static readonly Type[] XR =
        {
            typeof(Initialization.XREarlyUpdate),
            typeof(EarlyUpdate.XRUpdate),
            typeof(FixedUpdate.XRFixedUpdate),
            typeof(PostLateUpdate.XRPostPresent),
            typeof(PostLateUpdate.XRPostLateUpdate),
            typeof(PostLateUpdate.XRPreEndFrame),
        };

        public static readonly Type[] AR =
        {
            typeof(EarlyUpdate.ARCoreUpdate),
        };

        public static readonly Type[] Physics2D =
        {
#if UNITY_2022_2_OR_NEWER
            typeof(EarlyUpdate.Physics2DEarlyUpdate),
#endif
            typeof(FixedUpdate.Physics2DFixedUpdate),
            typeof(PreUpdate.Physics2DUpdate),
            typeof(PreLateUpdate.Physics2DLateUpdate),
        };

        public static readonly Type[] Physics3D =
        {
#if UNITY_2022_2_OR_NEWER
            typeof(EarlyUpdate.PhysicsResetInterpolatedTransformPosition),
#endif
            typeof(FixedUpdate.PhysicsFixedUpdate),
            typeof(PreUpdate.PhysicsUpdate),
            typeof(PreLateUpdate.PhysicsLateUpdate),
        };

        public static readonly Type[] AI =
        {
            typeof(PreUpdate.AIUpdate),
            typeof(PreLateUpdate.AIUpdatePostScript),
        };

        public static readonly Type[] Cloth =
        {
            typeof(PostLateUpdate.PhysicsSkinnedClothBeginUpdate),
            typeof(PostLateUpdate.PhysicsSkinnedClothFinishUpdate),
        };

        public static readonly Type[] OldNetwork =
        {
            typeof(PreLateUpdate.UpdateNetworkManager),
        };
        
    }

}