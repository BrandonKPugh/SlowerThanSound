using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    // Using this class to hold all of the constants
    //      Eventually would be nice to have a settings menu in-game to alter certain settings
    public static class Config
    {
        // Screen
        public const int GAME_WIDTH = 1600;
        public const int GAME_HEIGHT = 800;
        public static Color BACKGROUND_COLOR = new Color(75, 75, 75);

        // Buttons
        //      Percentage of the button's width that the text can utilize
        public const float BUTTON_PADDING_RATIO = 0.90f;

        //      Location and size of the primary button
        public const int PRIMARY_BUTTON_X = 1000;
        public const int PRIMARY_BUTTON_Y = 620;
        public const int PRIMARY_BUTTON_WIDTH = 400;
        public const int PRIMARY_BUTTON_HEIGHT = 100;


        // Grid
        //      Number of tiles
        public const int GRID_COUNT_X = 20;
        public const int GRID_COUNT_Y = 20;
        //      Grid's location (top left coordinates)
        public const int GRID_DESTINATION_X = 80;
        public const int GRID_DESTINATION_Y = 80;
        //      Size of each tile
        public const int GRID_TILE_WIDTH = 32;
        public const int GRID_TILE_HEIGHT = 32;
        //      Color of the lines on the grid (the tile centers are *currently* transparent)
        public static Color GRID_COLOR = new Color(30, 30, 30);
        //      Gets a Rectangle for the Grid's location
        public static Rectangle GRID_DESTINATION
        {
            get
            {
                return new Rectangle(GRID_DESTINATION_X, GRID_DESTINATION_Y, GRID_TILE_WIDTH * GRID_COUNT_X, GRID_TILE_HEIGHT * GRID_COUNT_Y);
            }
        }

        // Components
        //      Default health for now
        public static int GENERIC_COMPONENT_HEALTH = 1000;
        //      List of components, including structure components
        public static List<Component> COMPONENTS = new List<Component>();
        public static Color COMPONENT_STRUCTURE_COLOR = Color.Black;
        public static Color COMPONENT_WEAPON_COLOR = Color.DarkRed;


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
                COMPONENTS.Add(new StructureComponent(p.X, p.Y, COMPONENT_STRUCTURE_COLOR));
            }

            // Temporarily hard-coded
            w.Add(new Point(2, 5));
            w.Add(new Point(2, 6));
            w.Add(new Point(2, 8));
            w.Add(new Point(3, 8));

            foreach (Point p in w)
            {
                COMPONENTS.Add(new WeaponComponent(p.X, p.Y, COMPONENT_WEAPON_COLOR));
            }
        }
    }
}
