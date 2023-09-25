using GorillaCosmetics.HarmonyPatches.Patches;
using HarmonyLib;
using System;
using System.Reflection;

namespace GorillaCosmetics.HarmonyPatches
{
    /// <summary>
    /// Apply and remove all of our Harmony patches through this class
    /// </summary>
    public class GorillaCosmeticsPatches
    {
        private static Harmony instance;

        public static bool IsPatched { get; private set; }
        public const string InstanceId = "org.legoandmars.gorillatag.gorillacosmetics";

        internal static void ApplyHarmonyPatches()
        {
            if (!IsPatched)
            {
                instance ??= new Harmony(InstanceId);

                instance.PatchAll(Assembly.GetExecutingAssembly());
                Type RigSerializeType = typeof(GorillaTagger).Assembly.GetType("VRRigSerializer");
                instance.Patch(RigSerializeType.GetMethod("OnInstantiateSetup", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreReturn), postfix: new HarmonyMethod(typeof(CustomCosmeticsControllerPatches), nameof(CustomCosmeticsControllerPatches.InstantiateSetupPatch)));
                instance.Patch(RigSerializeType.GetMethod("CleanUp", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreReturn), prefix: new HarmonyMethod(typeof(CustomCosmeticsControllerPatches), nameof(CustomCosmeticsControllerPatches.CleanUpPatch)));

                IsPatched = true;
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            if (instance != null && IsPatched)
            {
                instance.UnpatchSelf();
                IsPatched = false;
            }
        }
    }
}