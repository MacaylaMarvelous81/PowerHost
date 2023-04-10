using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using PowerHost.Patches;
using Reactor;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace PowerHost
{
	[BepInAutoPlugin("io.github.macaylamarvelous81.powerhost")]
	[BepInProcess("Among Us.exe")]
	[BepInDependency(ReactorPlugin.Id)]
	public partial class PowerHostPlugin : BasePlugin
	{
		public Harmony Harmony { get; } = new Harmony(Id);

		public override void Load()
		{
			Harmony.PatchAll(typeof(PlayerVoteAreaPatch));
			
			Assets.LoadAssets();
		}
	}
}