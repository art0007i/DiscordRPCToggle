using HarmonyLib;
using NeosModLoader;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using FrooxEngine;
using FrooxEngine.UIX;
using BaseX;

namespace DiscordRPCToggle
{
    public class DiscordRPCToggle : NeosMod
    {
        public override string Name => "DiscordRPCToggle";
        public override string Author => "art0007i";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/art0007i/DiscordRPCToggle/";

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> KEY_RPC = new ModConfigurationKey<bool>("rpc_enable", "If false discord rpc will be disabled", () => true);

        private static ModConfiguration config;

        public override void OnEngineInit()
        {
            config = GetConfiguration();
            Harmony harmony = new Harmony("me.art0007i.DiscordRPCToggle");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(DiscordPlatformConnector))]
        class DiscordPlatformConnector_Patch
        {
            [HarmonyPrefix]
            [HarmonyPatch("SetCurrentStatus")]
            public static bool Prefix()
            {
                bool yes = config.GetValue(KEY_RPC);
                if (!yes)
                {
                    var discord = Engine.Current.PlatformInterface.GetConnectors<DiscordPlatformConnector>().First();
                    discord.ClearCurrentStatus();
                }
                return yes;
            }
        }
    }
}