using System;
using Hazel;
using Il2CppInterop.Runtime.Attributes;
using Il2CppSystem.Collections.Generic;
using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace PowerHost.Components
{
	[RegisterInIl2Cpp]
	public class PlayerVoteAreaExtension : MonoBehaviour
	{
		private GameObject _executeButton;

		public PlayerVoteAreaExtension(IntPtr ptr) : base(ptr) {}

		[HideFromIl2Cpp] public PlayerVoteArea Base => gameObject.GetComponent<PlayerVoteArea>();

		private void ExecuteButtonClick()
		{
			foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
				if (player.PlayerId != Base.TargetPlayerId)
				{
					continue;
				}

				// Kill the player without leaving a body via Exile RPC. (Exile locally first)
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(player.KillSfx, false, 0.8f, null);
				}

				player.Exiled();
				MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(player.NetId, 4, SendOption.Reliable);
				AmongUsClient.Instance.FinishRpcImmediately(messageWriter);

				// Close the menu for the controller maanager (outlines, controller selection things) and hide the
				// actual buttons.
				ControllerManager.Instance.CloseOverlayMenu(Base.name);
				Base.ClearButtons();
				
				break;
			}
		}

		public void CreateExecuteButton()
		{
			if (_executeButton != null)
			{
				Logger<PowerHostPlugin>.Error("Tried to create an execute button for a player which already has one?!");
			}
			
			// Cloning the Confirm Button (green checkmark), instead of creating one programatically from scratch.
			GameObject executeButton = Instantiate(Base.ConfirmButton.gameObject, Base.Buttons.transform);

			SpriteRenderer spriteRenderer = executeButton.GetComponent<SpriteRenderer>();
			spriteRenderer.sprite = Assets.UIButtonExecute;
			
			PassiveButton button = executeButton.GetComponent<PassiveButton>();
			
			// The existing behavior for this button, which comes from the cloned button, is to cast a vote. This was
			// probably defined in the Unity Editor, which made it 'persistent', and unaffected by RemoveAllListeners.
			// We can instead disable this behavior by setting its state to Off. I just guessed the index of the cast
			// vote behavior.
			button.OnClick.SetPersistentListenerState(0, UnityEventCallState.Off);
			button.OnClick.AddListener((UnityAction)ExecuteButtonClick);

			_executeButton = executeButton;
		}

		public void DestroyExecuteButton()
		{
			if (_executeButton == null) {
				Logger<PowerHostPlugin>.Error("Tried to destroy an execute button for a player which doesn't have one?!");
			}
			
			_executeButton.Destroy();
		}

		public void RevealExecuteButton()
		{
			if (_executeButton == null)
			{
				Logger<PowerHostPlugin>.Error("The execute button is missing while trying to reveal it.");
			}

			if (ControllerManager.Instance.IsMenuActiveAtAll(Base.name))
			{
				float startPos = Base.AnimateButtonsFromLeft ? 0.2f : 1.95f;
				StartCoroutine(Effects.Lerp(0.45f, (Action<float>)((t) =>
				{
					_executeButton.transform.localPosition = Vector2.Lerp(Vector2.right * startPos, Vector2.right * 0.0f,
						Effects.ExpOut(t));
				})));
				ControllerManager.Instance.GetMenu(Base.name).SelectableUiElements
					.Add(_executeButton.GetComponent<UiElement>());
			}
		}

		public void OnDestroy()
		{
			if (_executeButton != null)
			{
				DestroyExecuteButton();
			}
		}
	}
}