using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Spaceship
{
    public class Room
    {
        // Represents a room on the ship
        // The Type determines which components can go in this room
        public enum Room_Type
        {
            None,
            //Generic,
            Weapon,
            Material_Storage,
            Power_Storage,
            Power_Generation,
            Shield
        }
        // Used for getting the next room id. Use GetNextRoomID() to avoid errors.
        private static uint NextRoomID = 0;

        private List<Component> Components;
        private Ship Ship;
        private Grid Grid;

        public Rectangle GridLocation;
        public Room_Type RoomType;
        public uint RoomID;

        /// <param name="gridLocation">Location is in grid coordinates, not pixel coordinates</param>
        public Room(Ship ship, Rectangle gridLocation, Room_Type roomType)
        {
            this.GridLocation = gridLocation;
            this.RoomType = roomType;
            this.RoomID = GetNextRoomID();
            Components = new List<Component>();
            Ship = ship;
        }
        public Room(Ship ship, Grid grid, Point p1, Point p2, Room_Type roomType)
        {
            var minX = Math.Min(p1.X, p2.X);
            var minY = Math.Min(p1.Y, p2.Y);
            var maxX = Math.Max(p1.X, p2.X);
            var maxY = Math.Max(p1.Y, p2.Y);
            p1 = new Point(minX, minY);
            p2 = new Point(maxX, maxY);

            this.GridLocation = new Rectangle(p1.X, p1.Y,p2.X-p1.X,p2.Y-p1.Y);
            this.RoomType = roomType;
            this.RoomID = GetNextRoomID();
            Components = new List<Component>();
            Ship = ship;
            Grid = grid;

            for(int x = p1.X; x <= p2.X; x++)
            {
                Components.Add(new StructureComponent(x, p1.Y, ComponentConstants.COMPONENT_STRUCTURE_COLOR));
                Components.Add(new StructureComponent(x, p2.Y, ComponentConstants.COMPONENT_STRUCTURE_COLOR));
            }
            for(int y = p1.Y + 1; y < p2.Y; y++)
            {
                Components.Add(new StructureComponent(p1.X, y, ComponentConstants.COMPONENT_STRUCTURE_COLOR));
                Components.Add(new StructureComponent(p2.X, y, ComponentConstants.COMPONENT_STRUCTURE_COLOR));
            }
            
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch, Grid.GridInfo gridInfo)
        {
            foreach (Component component in Components)
            {
                component.Draw(spriteBatch, gridInfo);
            }
        }

        // Gets the next id to avoid duplicates
        private static uint GetNextRoomID()
        {
            uint toReturn = NextRoomID;
            NextRoomID++;
            return toReturn;
        }

        // Determines if a grid coordinate is inside this room
        public bool Contains(int x, int y)
        {
            return (x >= GridLocation.X && x < GridLocation.X + GridLocation.Width && y >= GridLocation.Y && y < GridLocation.Y + GridLocation.Height);
        }

        public void AddComponent(Component component)
        {
            // Load the texture for it since the texture is null by default
            Ship.LoadComponentTexture(component);
            Components.Add(component);
        }

        public List<Component> GetComponents()
        {
            return Components;
        }

        public Rectangle GetArea()
        {
            return GridLocation;
        }
        public Rectangle GetInteriorArea()
        {
            var info = Grid.Info;
            var topLeft = info.TileBounds(GridLocation.X, GridLocation.Y);
            var bottomRight = info.TileBounds(GridLocation.Width, GridLocation.Height);
            var x = topLeft.X + topLeft.Width;
            var y = topLeft.Y + topLeft.Height;
            var width = bottomRight.X - x;
            var height = bottomRight.Y - y;

            return new Rectangle(x, y, width, height);
        }

        public uint GetPriority()
        {
            //Debug value, will get value of each component and return the total value at some point
            return 1;
        }

        public void SetRoomType(Room_Type type)
        {
            RoomType = type;
        }
    }
}
