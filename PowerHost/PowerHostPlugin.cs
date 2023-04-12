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
		private Harmony _harmony { get; } = new Harmony(Id);

		public override void Load()
		{
			_harmony.PatchAll(typeof(PlayerVoteAreaPatch));
			_harmony.PatchAll(typeof(GameStartManagerPatch));
			
			Assets.LoadAssets();
		}
	}
}