using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Controls.UIGroups;
using MonoGameWindowsStarter.Spaceship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            private int weight, padding;
            public BORDERBOX_INFO(int penWeight, Color color, float x_percent, float y_percent, float width_percent, float height_percent, int padding)
            {
                this.x = x_percent;
                this.y = y_percent;
                this.width = width_percent;
                this.height = height_percent;
                this.weight = penWeight;
                this.color = color;
                this.padding = padding;
            }

            public BORDERBOX_INFO(int penWeight, Color color, int padding)
            {
                this.x = 0;
                this.y = 0;
                this.width = 0;
                this.height = 0;
                this.weight = penWeight;
                this.color = color;
                this.padding = padding;
            }
            public int X { get { return (int)(this.x * Config.GAME_WIDTH); } }
            public int Y { get { return (int)(this.y * Config.GAME_HEIGHT); } }
            public int Width { get { return (int)(this.width * Config.GAME_WIDTH); } }
            public int Height { get { return (int)(this.height * Config.GAME_HEIGHT); } }
            public int Weight { get { return weight; } }
            public Color Color { get { return color; } }
            public int Padding { get { return padding; } }
        }

        public const string BUTTON_TEXTURE = "Button";
        public const string BUTTON_FONT = "DebugFont";
        public static Color BUTTON_PENCOLOR = Color.Black;
        public static Color BUTTON_BACKCOLOR = Color.White;
        public static Color BUTTON_HOVERING = Color.Gray;
        public const float BUTTON_PADDING_RATIO = 0.90f;
        public static Color BUTTON_SELECTED = Color.SkyBlue;

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
        // BORDERBOX_INFO(penWeight, color, padding)
        // Position/size is later set using BorderBox.SetPosition() so that it is aligned with the grid.
        public static BORDERBOX_INFO COMBATMODE_GRIDBOX = new BORDERBOX_INFO(3, Color.Black, 5);

        public static BUTTON_INFO COMBATMODE_TARGETSTORAGES = new BUTTON_INFO("Material Storage", 0.625f, 0.275f, 0.25f, 0.1f);
        public static BUTTON_INFO COMBATMODE_TARGETWEAPONS = new BUTTON_INFO("Weapon Systems", 0.625f, 0.425f, 0.25f, 0.1f);
        public static BUTTON_INFO COMBATMODE_TARGETPOWER = new BUTTON_INFO("Power Generation", 0.625f, 0.575f, 0.25f, 0.1f);

        #endregion

        #region BUILDMODE

        public static BUTTON_INFO BUILDMODE_COMBATMODE = new BUTTON_INFO("Combat Mode", 0.625f, 0.825f, 0.25f, 0.125f);

        private const float _CANVAS_WIDTH = .38125f;
        private const float _CANVAS_HEIGHT = 0.6f;
        private const float _CANVAS_X = 0.5625f;
        private const float _CANVAS_Y = 0.2f;
        private const float _SHIP_RATIO = _CANVAS_WIDTH * 14f / 64f;
        private const float _COMPONENT_RATIO = _CANVAS_WIDTH * 27f / 64f;
        private const float _RESEARCH_RATIO = _CANVAS_WIDTH * 22f / 64f;
        private const float _GAP_RATIO = (_CANVAS_WIDTH - (_SHIP_RATIO + _COMPONENT_RATIO + _RESEARCH_RATIO)) / 2;

        public static BORDERBOX_INFO BUILDMODE_CANVAS = new BORDERBOX_INFO(3, Color.Black, _CANVAS_X, _CANVAS_Y, _CANVAS_WIDTH, _CANVAS_HEIGHT, 5);
        #region SHIPBUILD
        public static BUTTON_INFO BUILDMODE_SHIPBUILD = new BUTTON_INFO("Ship", _CANVAS_X, 0.125f, _SHIP_RATIO, .06875f);
        public static BUTTON_INFO SHIPBUILD_TEST = new BUTTON_INFO("View Ship", _CANVAS_X + 0.05f, 0.25f, _CANVAS_WIDTH - 0.1f, (_CANVAS_WIDTH - 0.1f)/2);
        #endregion
        #region COMPONENTBUILD
        public static BUTTON_INFO BUILDMODE_COMPONENTBUILD = new BUTTON_INFO("Components", _CANVAS_X + _SHIP_RATIO + _GAP_RATIO, 0.125f, _COMPONENT_RATIO, .06875f);

        private const float _SPACINGX = 1 / 64f;
        private const float _SPACINGY = 1 / 64f;
        private const int _BUTTON_COUNT = 6;
        private const float BUTTON_X = _CANVAS_WIDTH - _SPACINGX * 2;
        private const float BUTTON_Y = (_CANVAS_HEIGHT - (_SPACINGY * (_BUTTON_COUNT + 1))) / _BUTTON_COUNT;
        public static BUTTON_INFO PLACE_COMPONENT_WEAPON = new BUTTON_INFO("Place Weapon", _CANVAS_X + _SPACINGX, _CANVAS_Y + _SPACINGY, BUTTON_X, BUTTON_Y);
        public static BUTTON_INFO PLACE_COMPONENT_STORAGE = new BUTTON_INFO("Place Storage", _CANVAS_X + _SPACINGX, _CANVAS_Y + _SPACINGY + (_SPACINGY + BUTTON_Y) * 1, BUTTON_X, BUTTON_Y);
        public static BUTTON_INFO PLACE_COMPONENT_GENERATOR = new BUTTON_INFO("Place Generator", _CANVAS_X + _SPACINGX, _CANVAS_Y + _SPACINGY + (_SPACINGY + BUTTON_Y) * 2, BUTTON_X, BUTTON_Y);
        public static BUTTON_INFO CREATE_ROOM = new BUTTON_INFO("Create Room", _CANVAS_X + _SPACINGX, _CANVAS_Y + _SPACINGY + (_SPACINGY + BUTTON_Y) * 3, BUTTON_X, BUTTON_Y);
        public static BUTTON_INFO DELETE_COMPONENT = new BUTTON_INFO("Delete Components", _CANVAS_X + _SPACINGX, _CANVAS_Y + _SPACINGY + (_SPACINGY + BUTTON_Y) * 4, BUTTON_X, BUTTON_Y);
        public static BUTTON_INFO DELETE_ROOM = new BUTTON_INFO("Delete Room", _CANVAS_X + _SPACINGX, _CANVAS_Y + _SPACINGY + (_SPACINGY + BUTTON_Y) * 5, BUTTON_X, BUTTON_Y);
        #endregion
        #region RESEARCH
        public static BUTTON_INFO BUILDMODE_RESEARCH = new BUTTON_INFO("Research", _CANVAS_X + _SHIP_RATIO + _COMPONENT_RATIO + _GAP_RATIO * 2, 0.125f, _RESEARCH_RATIO, .06875f);
        public static BUTTON_INFO RESEARCH_TEST = new BUTTON_INFO("Research Item", _CANVAS_X + 0.05f, 0.25f, _CANVAS_WIDTH - 0.1f, (_CANVAS_WIDTH - 0.1f) / 2);
        #endregion


        public static TEXTBOX_INFO BUILDMODE_TITLE = new TEXTBOX_INFO("Build Mode", Color.LightSkyBlue, 0.625f, 0.05f, 0.25f, 0.05f);
        public static BORDERBOX_INFO BUILDMODE_TITLEBOX = new BORDERBOX_INFO(3, Color.Black, 5);

        // BORDERBOX_INFO(penWeight, color, padding)
        // Position/size is later set using BorderBox.SetPosition() so that it is aligned with the grid.
        public static BORDERBOX_INFO BUILDMODE_GRIDBOX = new BORDERBOX_INFO(3, Color.Black, 5);

        #endregion

        #endregion
    }
}
