using Dalamud.Game.Command;
using Dalamud.Plugin;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace QuietGlamours
{
    public class Plugin : IDalamudPlugin
    {
        public string Name => "Quiet Glamours";
        private const string commandName = "/qg";

        private DalamudPluginInterface pi;
        private Configuration configuration;
        private QuietGlamoursUI ui;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pi = pluginInterface;

            this.configuration = this.pi.GetPluginConfig() as Configuration ?? new Configuration();
            this.configuration.Initialize(this.pi);

            this.ui = new QuietGlamoursUI(this.configuration);

            this.pi.CommandManager.AddHandler(commandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Open config window for Quiet Glamours"
            });

            this.pi.UiBuilder.OnBuildUi += DrawUI;
            this.pi.UiBuilder.OnOpenConfigUi += (sender, args) => DrawConfigUI();
            this.pi.Framework.Gui.Chat.OnChatMessage += OnChatMessage;
        }

        public void Dispose()
        {
            this.ui.Dispose();
            this.pi.CommandManager.RemoveHandler(commandName);
            this.pi.Framework.Gui.Chat.OnChatMessage -= OnChatMessage;
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            this.ui.SettingsVisible = true;
        }

        //EN: You cast a glamour.
        //FR: Vous projetez un mirage.
        //DE: Du projizierst ein
        //JP: の外見を武具投影した。
        private void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            if (!this.configuration.enabled)
            {
                return;
            }
            if ((message.TextValue.Contains("You cast a glamour.") | message.TextValue.Contains("の外見を武具投影した") |
                message.TextValue.Contains("Vous projetez un mirage.") | message.TextValue.Contains("Du projizierst ein")) &
                (type & XivChatType.SystemMessage) == XivChatType.SystemMessage)
            {
                isHandled = true;
                return;
            }
        }

        private void DrawUI()
        {
            this.ui.Draw();
        }

        private void DrawConfigUI()
        {
            this.ui.SettingsVisible = true;
        }
    }
}