internal static class HudGuard
{
    public static bool IsGameOverlayActive
    {
        get
        {
            if (HUD.instance != null && HUD.instance.hidden)
                return true;

            if (Map.Instance != null && Map.Instance.Active)
                return true;

            MoonUI moon = MoonUI.instance;

            return moon != null && moon.objectActive != null && moon.objectActive.activeSelf;
        }
    }
}
