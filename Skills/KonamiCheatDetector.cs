using HarmonyLib;
using Photon.Pun;
using UnityEngine;

[HarmonyPatch(typeof(RunManager), "Update")]
internal static class KonamiCheatDetector
{
    private static readonly KeyCode[] Sequence =
    {
        KeyCode.UpArrow, KeyCode.UpArrow,
        KeyCode.DownArrow, KeyCode.DownArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.B, KeyCode.A
    };

    private static readonly KeyCode[] RelevantKeys =
    {
        KeyCode.UpArrow, KeyCode.DownArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.B, KeyCode.A
    };

    private static int progress = 0;

    private static void Postfix()
    {
        if (!Plugin.EnableMod.Value)
            return;

        KeyCode expected = Sequence[progress];

        if (Input.GetKeyDown(expected))
        {
            progress++;

            if (progress >= Sequence.Length)
            {
                progress = 0;
                ToggleFreeClassChange();
            }

            return;
        }

        foreach (KeyCode key in RelevantKeys)
        {
            if (Input.GetKeyDown(key))
            {
                progress = (key == Sequence[0]) ? 1 : 0;
                return;
            }
        }
    }

    private static void ToggleFreeClassChange()
    {
        bool newState = !KonamiCheatRelay.FreeClassChangeUnlocked;
        PlayerAvatar local = PlayerAvatar.instance;

        if (SemiFunc.IsMultiplayer() && local != null && local.photonView != null)
        {
            KonamiCheatRelay relay = local.GetComponent<KonamiCheatRelay>();

            if (relay == null)
                relay = local.gameObject.AddComponent<KonamiCheatRelay>();

            local.photonView.RPC(nameof(KonamiCheatRelay.SetFreeClassChangeRPC), RpcTarget.All, newState);
        }
        else
        {
            KonamiCheatRelay.FreeClassChangeUnlocked = newState;
        }

        if (ChatManager.instance != null)
        {
            ChatManager.instance.ForceSendMessage(newState
                ? "*Konami Code* : changement de classe libre débloqué !"
                : "*Konami Code* : changement de classe libre désactivé.");
        }
    }
}
