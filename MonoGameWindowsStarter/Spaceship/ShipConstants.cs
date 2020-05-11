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
        //public static List<Component> COMPONENTS = new List<Component>();
        public static List<Tuple<Point, Point, Room.Room_Type>> ROOMPOINTS;
        public const float TICK_INTERVAL = 1000/60f;

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
        public static GRID_INFO SHIP_GRID = new GRID_INFO(20, 20, 0.05625f, .1f, .45f, .8f);

        //      Color of the lines on the grid (the tile centers are *currently* transparent)
        public static Color GRID_COLOR = new Color(30, 30, 30);
        //      Gets a Rectangle for the Grid's location

        public static void Initialize()
        {
            ROOMPOINTS = new List<Tuple<Point, Point, Room.Room_Type>>();
            ROOMPOINTS.Add(new Tuple<Point, Point, Room.Room_Type>(new Point(4, 4), new Point(7, 7), Room.Room_Type.None));
            ROOMPOINTS.Add(new Tuple<Point, Point, Room.Room_Type>(new Point(7, 4), new Point(10, 7), Room.Room_Type.None));
            ROOMPOINTS.Add(new Tuple<Point, Point, Room.Room_Type>(new Point(4, 7), new Point(7, 10), Room.Room_Type.None));
            ROOMPOINTS.Add(new Tuple<Point, Point, Room.Room_Type>(new Point(7, 7), new Point(10, 10), Room.Room_Type.None));
        }
    }
}
