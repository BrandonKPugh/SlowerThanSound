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
        public Room(Ship ship, Point p1, Point p2, Room_Type roomType)
        {
            var minX = Math.Min(p1.X, p2.X);
            var minY = Math.Min(p1.Y, p2.Y);
            var maxX = Math.Max(p1.X, p2.X);
            var maxY = Math.Max(p1.Y, p2.Y);
            p1 = new Point(minX, minY);
            p2 = new Point(maxX, maxY);

            IEnumerable<int> xRange = Enumerable.Range(p1.X, p2.X);
            IEnumerable<int> yRange = Enumerable.Range(p1.Y+1, p2.Y - p1.Y-1);

            this.GridLocation = new Rectangle(p1.X, p1.Y,p2.X-p1.X,p2.Y-p1.Y);
            this.RoomType = roomType;
            this.RoomID = GetNextRoomID();
            Components = new List<Component>();
            Ship = ship;

            foreach(int x in xRange)
            {
                Components.Add(new StructureComponent(x, p1.Y, ComponentConstants.COMPONENT_STRUCTURE_COLOR));
                Components.Add(new StructureComponent(x, p2.Y, ComponentConstants.COMPONENT_STRUCTURE_COLOR));
            }
            foreach (int y in yRange)
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
        public Rectangle GetInteriorArea(float tileWidth, float tileHeight)
        {
            var x = GridLocation.X + tileWidth;
            var y = GridLocation.Y + tileHeight;
            var width = GridLocation.Width - tileWidth;
            var height = GridLocation.Height - tileHeight;
            return new Rectangle((int)x,(int)y,(int)width,(int)height);
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
