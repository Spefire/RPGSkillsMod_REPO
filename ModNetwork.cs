using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

internal static class ModNetwork
{
    private const string PlayerPropertyKey = "RPGSkillsMod";
    private const float NotifyIntervalSeconds = 5f;

    private static float lastNotifyTime = float.NegativeInfinity;

    public static void Announce()
    {
        if (!PhotonNetwork.InRoom)
            return;

        Player local = PhotonNetwork.LocalPlayer;

        if (local == null || local.CustomProperties.ContainsKey(PlayerPropertyKey))
            return;

        local.SetCustomProperties(new Hashtable { { PlayerPropertyKey, Plugin.ModVersion } });
    }

    public static bool HasMod(Player player)
    {
        return player != null && (player.IsLocal || player.CustomProperties.ContainsKey(PlayerPropertyKey));
    }

    public static bool EnsureHostHasMod()
    {
        if (HasMod(PhotonNetwork.MasterClient))
            return true;

        NotifyMissingMod("The host doesn't have RPGSkillsMod");
        return false;
    }

    public static bool TryRpcToMaster(PhotonView view, string method, params object[] args)
    {
        if (view == null || !EnsureHostHasMod())
            return false;

        view.RPC(method, RpcTarget.MasterClient, args);

        return true;
    }

    public static int RpcToModdedPlayers(PhotonView view, string method, params object[] args)
    {
        if (view == null)
            return 0;

        int sent = 0;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!HasMod(player))
                continue;

            view.RPC(method, player, args);
            sent++;
        }

        return sent;
    }

    private static void NotifyMissingMod(string reason)
    {
        Plugin.Log.LogWarning($"{reason}: skill unavailable.");

        if (Time.unscaledTime - lastNotifyTime < NotifyIntervalSeconds)
            return;

        lastNotifyTime = Time.unscaledTime;

        if (ChatManager.instance != null)
            ChatManager.instance.ForceSendMessage($"[RPGSkillsMod] {reason}, skill unavailable.");
    }
}
