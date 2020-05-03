using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameWindowsStarter.Components;
using Microsoft.Win32;

namespace MonoGameWindowsStarter.Spaceship
{
    public static class ShipConstants
    {
        public static List<Component> COMPONENTS = new List<Component>();

        public struct GRID_INFO
        {
            private float x, y, width, height;
            private int tilesX, tilesY;

            public GRID_INFO(int tilesWide, int tilesHigh, float x_percent, float y_percent, float width_percent, float height_percent)
            {
                this.tilesX = tilesWide;
                this.tilesY = tilesHigh;
                this.x = x_percent;
                this.y = y_percent;
                this.width = width_percent;
                this.height = height_percent;
            }
            public int TilesWide { get { return tilesX; } }
            public int TilesHigh { get { return tilesY; } }

            public int X { get { return (int)(this.x * Config.GAME_WIDTH); } }
            public int Y { get { return (int)(this.y * Config.GAME_HEIGHT); } }
            public int Width { get { return (int)(this.width * Config.GAME_WIDTH); } }
            public int Height { get { return (int)(this.height * Config.GAME_HEIGHT); } }
            public Rectangle Rect { get { return new Rectangle(X, Y, Width, Height); } }
        }

        // GRID_INFO(tilesWide, tilesHigh, x, y, width, height);
        public static GRID_INFO SHIP_GRID = new GRID_INFO(12, 12, .05f, .05f, .50625f, .9f);

        //      Color of the lines on the grid (the tile centers are *currently* transparent)
        public static Color GRID_COLOR = new Color(30, 30, 30);
        //      Gets a Rectangle for the Grid's location

        public static void Initialize()
        {
            List<Point> s = new List<Point>();
            List<Point> w = new List<Point>();

            // Temporarily hard-coded
            s.Add(new Point(1, 1));
            s.Add(new Point(1, 2));
            s.Add(new Point(1, 3));
            s.Add(new Point(1, 4));
            s.Add(new Point(1, 5));
            s.Add(new Point(1, 6));
            s.Add(new Point(1, 7));
            s.Add(new Point(1, 8));
            s.Add(new Point(1, 9));
            s.Add(new Point(2, 1));
            s.Add(new Point(2, 4));
            s.Add(new Point(2, 9));
            s.Add(new Point(3, 1));
            s.Add(new Point(3, 4));
            s.Add(new Point(3, 9));
            s.Add(new Point(4, 1));
            s.Add(new Point(4, 4));
            s.Add(new Point(4, 5));
            s.Add(new Point(4, 6));
            s.Add(new Point(4, 7));
            s.Add(new Point(4, 8));
            s.Add(new Point(4, 9));
            s.Add(new Point(5, 1));
            s.Add(new Point(5, 2));
            s.Add(new Point(5, 3));
            s.Add(new Point(5, 4));
            s.Add(new Point(5, 9));
            s.Add(new Point(6, 1));
            s.Add(new Point(6, 4));
            s.Add(new Point(6, 9));
            s.Add(new Point(7, 1));
            s.Add(new Point(7, 4));
            s.Add(new Point(7, 9));
            s.Add(new Point(8, 1));
            s.Add(new Point(8, 4));
            s.Add(new Point(8, 9));
            s.Add(new Point(9, 1));
            s.Add(new Point(9, 2));
            s.Add(new Point(9, 3));
            s.Add(new Point(9, 4));
            s.Add(new Point(9, 5));
            s.Add(new Point(9, 6));
            s.Add(new Point(9, 7));
            s.Add(new Point(9, 8));
            s.Add(new Point(9, 9));

            foreach (Point p in s)
            {
                COMPONENTS.Add(new StructureComponent(p.X, p.Y, ComponentConstants.COMPONENT_STRUCTURE_COLOR));
            }

            // Temporarily hard-coded
            w.Add(new Point(2, 5));
            w.Add(new Point(2, 6));
            w.Add(new Point(2, 8));
            w.Add(new Point(3, 8));

            foreach (Point p in w)
            {
                COMPONENTS.Add(new WeaponComponent(p.X, p.Y, ComponentConstants.COMPONENT_WEAPON_COLOR));
            }
        }
    }
}
