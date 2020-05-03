using Microsoft.Xna.Framework;
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

        public Rectangle GridLocation;
        public Room_Type RoomType;
        public uint RoomID;

        /// <param name="gridLocation">Location is in grid coordinates, not pixel coordinates</param>
        public Room(Rectangle gridLocation, Room_Type roomType)
        {
            this.GridLocation = gridLocation;
            this.RoomType = roomType;
            this.RoomID = GetNextRoomID();
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
    }
}
