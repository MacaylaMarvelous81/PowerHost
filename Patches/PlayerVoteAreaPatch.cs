using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using PowerHost.Components;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace PowerHost.Patches
{
	public static class PlayerVoteAreaPatch
	{
		[HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.Start))]
		[HarmonyPrefix]
		public static bool AddExtension(PlayerVoteArea __instance)
		{
			PlayerVoteAreaExtension extension = __instance.gameObject.AddComponent<PlayerVoteAreaExtension>();

			if (AmongUsClient.Instance.AmHost)
			{
				extension.CreateExecuteButton();
			}

			return true;
		}

		[HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.Select))]
		[HarmonyPostfix]
		public static void RevealExecuteButton(PlayerVoteArea __instance)
		{
			if (!AmongUsClient.Instance.AmHost)
			{
				return;
			}

			PlayerVoteAreaExtension extension = __instance.gameObject.GetComponent<PlayerVoteAreaExtension>();
			extension.RevealExecuteButton();
		}
	}
}