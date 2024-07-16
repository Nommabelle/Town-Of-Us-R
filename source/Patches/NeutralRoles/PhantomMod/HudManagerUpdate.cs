using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.NeutralRoles.PhantomMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudManagerUpdate
    {
        public static void Postfix(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Phantom)) return;
            var role = Role.GetRole<Phantom>(PlayerControl.LocalPlayer);
            if (role.Caught) return;

            // set kill button which can be used by phantom to turn to a ghost with PerformKill
            var killButton = __instance.KillButton;
            killButton.gameObject.SetActive((__instance.UseButton.isActiveAndEnabled || __instance.PetButton.isActiveAndEnabled)
                    && !MeetingHud.Instance
                    && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started);

            killButton.graphic.enabled = true;

            // enable the kill button immediately
            var renderer = killButton.graphic;
            renderer.color = Palette.EnabledColor;
            renderer.material.SetFloat("_Desat", 0f);

            // probably dont need this let's hope
            //KillButtonTarget.SetTarget(killButton, closestBody, role);
        }
    }
}