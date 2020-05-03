using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Spaceship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls
{
    public static class ControlConstants
    {
        #region BUTTONS

        // Button info is created with Text, and four floats (representing percentages of the screen's width/height. 0.1f = 10% from the edge and 0.5f = the center of the screen)
        // BUTTON_INFO(Text, X, Y, Width, Height)
        public struct BUTTON_INFO
        {
            private float x, y, width, height;
            private string text;
            public BUTTON_INFO(string text, float x_percent, float y_percent, float width_percent, float height_percent)
            {
                this.x = x_percent;
                this.y = y_percent;
                this.width = width_percent;
                this.height = height_percent;
                this.text = text;
            }
            public int X { get { return (int)(this.x * Config.GAME_WIDTH); }}
            public int Y { get { return (int)(this.y * Config.GAME_HEIGHT); }}
            public int Width { get { return (int)(this.width * Config.GAME_WIDTH); }}
            public int Height{ get { return (int)(this.height * Config.GAME_HEIGHT); }}
            public string Text { get { return text; }}
        }

        public struct TEXTBOX_INFO
        {
            private float x, y, width, height;
            private string text;
            private Color color;
            public TEXTBOX_INFO(string text, Color color, float x_percent, float y_percent, float width_percent, float height_percent)
            {
                this.x = x_percent;
                this.y = y_percent;
                this.width = width_percent;
                this.height = height_percent;
                this.text = text;
                this.color = color;
            }
            public int X { get { return (int)(this.x * Config.GAME_WIDTH); } }
            public int Y { get { return (int)(this.y * Config.GAME_HEIGHT); } }
            public int Width { get { return (int)(this.width * Config.GAME_WIDTH); } }
            public int Height { get { return (int)(this.height * Config.GAME_HEIGHT); } }
            public string Text { get { return text; } }
            public Color Color { get { return color; } }
        }

        public struct BORDERBOX_INFO
        {
            private float x, y, width, height;
            private Color color;
            private int weight;
            public BORDERBOX_INFO(int weight, Color color, float x_percent, float y_percent, float width_percent, float height_percent)
            {
                this.x = x_percent;
                this.y = y_percent;
                this.width = width_percent;
                this.height = height_percent;
                this.weight = weight;
                this.color = color;
            }
            public int X { get { return (int)(this.x * Config.GAME_WIDTH); } }
            public int Y { get { return (int)(this.y * Config.GAME_HEIGHT); } }
            public int Width { get { return (int)(this.width * Config.GAME_WIDTH); } }
            public int Height { get { return (int)(this.height * Config.GAME_HEIGHT); } }
            public int Weight { get { return weight; } }
            public Color Color { get { return color; } }
        }

        public const string BUTTON_TEXTURE = "Button";
        public const string BUTTON_FONT = "DebugFont";
        public static Color BUTTON_PENCOLOR = Color.Black;
        public static Color BUTTON_HOVERING = Color.Gray;
        public const float BUTTON_PADDING_RATIO = 0.90f;

        public static Color BAR_PENCOLOR = Color.Black;

        #region MAINMENU

        public static TEXTBOX_INFO MAINMENU_TITLE = new TEXTBOX_INFO("Slower Than Sound", Color.LightSkyBlue, 0.3f, 0.05f, 0.4f, 0.2f);
        public static BUTTON_INFO MAINMENU_NEWGAME = new BUTTON_INFO("New Game", 0.35f, 0.3f, 0.3f, 0.15f);
        public static BUTTON_INFO MAINMENU_LOADGAME = new BUTTON_INFO("Load Game", 0.35f, 0.5f, 0.3f, 0.15f);
        public static BUTTON_INFO MAINMENU_QUITGAME = new BUTTON_INFO("Quit Game", 0.35f, 0.7f, 0.3f, 0.15f);

        #endregion

        #region PAUSEMENU

        public static BUTTON_INFO PAUSEMENU_RESUMEGAME = new BUTTON_INFO("Resume Game", 0.35f, 0.1f, 0.3f, 0.15f);
        public static BUTTON_INFO PAUSEMENU_SAVEGAME = new BUTTON_INFO("Save Game", 0.35f, 0.3f, 0.3f, 0.15f);
        public static BUTTON_INFO PAUSEMENU_LOADGAME = new BUTTON_INFO("Load Game", 0.35f, 0.5f, 0.3f, 0.15f);
        public static BUTTON_INFO PAUSEMENU_QUITGAME = new BUTTON_INFO("Exit to Menu", 0.35f, 0.7f, 0.3f, 0.15f);

        #endregion

        #region COMBATMODE

        public static BUTTON_INFO COMBATMODE_BUILDMODE = new BUTTON_INFO("Build Mode", 0.625f, 0.775f, 0.25f, 0.125f);
        public static TEXTBOX_INFO COMBATMODE_TITLE = new TEXTBOX_INFO("Combat Mode", Color.LightSkyBlue, 0.625f, 0.05f, 0.25f, 0.125f);


        #endregion

        #region BUILDMODE

        public static BUTTON_INFO BUILDMODE_COMBATMODE = new BUTTON_INFO("Combat Mode", 0.625f, 0.775f, 0.25f, 0.125f);
        public static TEXTBOX_INFO BUILDMODE_TITLE = new TEXTBOX_INFO("Build Mode", Color.LightSkyBlue, 0.625f, 0.05f, 0.25f, 0.125f);
        public static BORDERBOX_INFO BUILDMODE_GRIDBOX = new BORDERBOX_INFO(3, Color.Black, .05f, .05f, .50625f, .9f);

        #endregion

        #endregion
    }
}
