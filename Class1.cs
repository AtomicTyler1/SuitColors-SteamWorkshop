using HarmonyLib;
using System;
using UnityEngine;
using Zorro.Settings;

namespace SuitColors
{
    [ContentWarningPlugin("com.atomic.suitcolors", "1.0.0", true)]
    internal class ContentPrioritization
    {
        static ContentPrioritization() { new GameObject().AddComponent<Plugin>(); }
    }

    [ContentWarningSetting]
    public class BrighterColors : BoolSetting, IExposedSetting
    {
        public override void ApplyValue() { }
        public string GetDisplayName() => "Enable brigher suit colors";
        protected override bool GetDefaultValue() => false;
        public SettingCategory GetSettingCategory() => SettingCategory.Mods;
    }

    public class Plugin : MonoBehaviour
    {
        private void Start ()
        {
            new Harmony("com.atomic.suitcolors").PatchAll();
        }

        [HarmonyPatch(typeof(PlayerVisor), "Update")]
        internal class PlayerVisorPatch
        {
            private static void Postfix(PlayerVisor __instance)
            {
                float num;
                float num2;
                float num3;
                Color.RGBToHSV(__instance.visorColor.Value, out num, out num2, out num3);
                Color color = Color.HSVToRGB(__instance.hue.Value, num2, GameHandler.Instance.SettingsHandler.GetSetting<BrighterColors>().Value ? 1f : 0.4f);
                GameObject gameObject = __instance.gameObject.transform.GetChild(1).gameObject;
                gameObject.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material.color = color;
                gameObject.transform.GetChild(3).gameObject.GetComponent<SkinnedMeshRenderer>().material.color = color;
            }
        }
    }
}
