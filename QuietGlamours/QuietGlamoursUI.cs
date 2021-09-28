using ImGuiNET;
using System;
using System.Numerics;

namespace QuietGlamours
{
    // It is good to have this be disposable in general, in case you ever need it
    // to do any cleanup
    class QuietGlamoursUI : IDisposable
    {
        private Configuration configuration;

        // this extra bool exists for ImGui, since you can't ref a property

        private bool settingsVisible = false;
        public bool SettingsVisible
        {
            get { return this.settingsVisible; }
            set { this.settingsVisible = value; }
        }

        public QuietGlamoursUI(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public void Dispose()
        {
        }

        public void Draw()
        {
            DrawSettingsWindow();
        }

        public void DrawSettingsWindow()
        {
            if (!SettingsVisible)
            {
                return;
            }

            ImGui.SetNextWindowSize(new Vector2(275, 85), ImGuiCond.Always);
            if (ImGui.Begin("Quiet Glamours", ref this.settingsVisible,
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                // can't ref a property, so use a local copy
                var configValue = this.configuration.enabled;
                if (ImGui.Checkbox("Suppress system glamour messages", ref configValue))
                {
                    this.configuration.enabled = configValue;
                    // can save immediately on change, if you don't want to provide a "Save and Close" button
                    this.configuration.Save();
                }
                var plateValue = this.configuration.plateEnabled;
                if (ImGui.Checkbox("Suppress system glamour plate messages", ref plateValue))
                {
                    this.configuration.plateEnabled = plateValue;
                    this.configuration.Save();
                }
            }
            ImGui.End();
        }
    }
}
