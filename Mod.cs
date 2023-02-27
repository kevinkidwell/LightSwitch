using System;
using GenericModConfigMenu;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;


namespace LightSwitch
{
    /// <summary>The mod entry point</summary>
    public class Mod : StardewModdingAPI.Mod
    {
        internal static ModConfig Config;
        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();

            if (!Config.EnableLight)
                return;

            helper.Events.Input.ButtonPressed += Input_ButtonPressed;
            helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
            helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
        }

        private void GameLoop_UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (Config.EnableLight && Game1.ambientLight != Color.White * 0f)
            {
                Game1.ambientLight = Color.White * 0f;
            }
        }

        private void Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.CanPlayerMove)
                return;
            if (e.Button == Config.Keybind)
            {
                if (Config.EnableLight)
                { Config.EnableLight = false; }
                else
                { Config.EnableLight = true; }
            }
        }

        // Add GMCM
        private void GameLoop_GameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // get Generic Mod Config Menu's API (if it's installed)
            var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            // register mod
            configMenu.Register(
                mod: ModManifest,
                reset: () => Config = new ModConfig(),
                save: () => Helper.WriteConfig(Config)
            );

            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => "Enable Light",
                getValue: () => Config.EnableLight,
                setValue: value => Config.EnableLight = value
            );

            configMenu.AddKeybind(
                mod: ModManifest,
                name: () => "Keybind",
                getValue: () => Config.Keybind,
                setValue: value => Config.Keybind = value
            );
        }
    }

    public class ModConfig
    {
        public bool EnableLight { get; set; } = true;
        public SButton Keybind { get; set; } = SButton.K;
    }
}
