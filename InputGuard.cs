using System;
using HarmonyLib;

internal static class InputGuard
{
    private static readonly AccessTools.FieldRef<ChatManager, bool> ChatActive = CreateChatActiveAccessor();

    public static bool IsTyping
    {
        get
        {
            ChatManager chat = ChatManager.instance;

            return chat != null && ChatActive != null && ChatActive(chat);
        }
    }

    private static AccessTools.FieldRef<ChatManager, bool> CreateChatActiveAccessor()
    {
        try
        {
            return AccessTools.FieldRefAccess<ChatManager, bool>("chatActive");
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"ChatManager.chatActive not found, hotkeys won't be blocked while typing: {ex.Message}");
            return null;
        }
    }
}
