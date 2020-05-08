using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameWindowsStarter.Components;
using MonoGameWindowsStarter.Spaceship;
using Microsoft.Xna.Framework;

namespace MonoGameWindowsStarter.Spaceship
{
    public class Ship
    {
        // List of all components
        //      this will likely be a different data structure later
        // private List<Component> Components;
        // Dictionary for component textures
        private Dictionary<Component.Component_Type, Texture2D> Textures;

        public Grid Grid;

        public List<Room> Rooms;

        #region RESOURCES
        public int Money;
        public int Power;
        public int Material;
        #endregion

        public Ship()
        {

        }

        public void Initialize(List<Tuple<Point,Point, Room.Room_Type>> rooms)
        {
            Textures = new Dictionary<Component.Component_Type, Texture2D>();

            Grid = new Grid(ShipConstants.SHIP_GRID);

            Rooms = new List<Room>();

            foreach (Tuple<Point,Point, Room.Room_Type> a in rooms)
            {
                Rooms.Add(new Room(this, Grid, a.Item1, a.Item2, a.Item3));
            }
        }

        // Alternative initialization, allows passing in some preset components
        public void Initialize(List<Component> components)
        {
            Textures = new Dictionary<Component.Component_Type, Texture2D>();

            Grid = new Grid(ShipConstants.SHIP_GRID);

            foreach (Component a in components)
            {
                //Components.Add(a);
            }
        }

        // Load the textures for all components on the ship
        public void LoadContent(Dictionary<Component.Component_Type, Texture2D> textures, Texture2D TileTexture)
        {
            Grid.LoadContent(TileTexture);

            // Loop through each texture that was just passed in and add it to the ship's dictionary
            foreach (KeyValuePair<Component.Component_Type, Texture2D> pair in textures)
            {
                if(!Textures.ContainsKey(pair.Key))
                    Textures.Add(pair.Key, pair.Value);
            }
            // Loop through all components on the ship and set their textures
            foreach(Room room in Rooms)
            {
                foreach (Component component in room.GetComponents())
                {
                    LoadComponentTexture(component);
                }
            }
        }

        // Loops through all components on the ship and calls Draw() on each one
        public void Draw(SpriteBatch spriteBatch, ModeState.State currentState)
        {
            switch(currentState)
            {
                case ModeState.State.Build:
                    {
                        Grid.Draw(spriteBatch);
                        break;
                    }
                case ModeState.State.Combat:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            foreach (Room room in Rooms)
            {
                // Passes gridInfo so that if/when the grid is altered, the components will be rendered correctly
                room.Draw(spriteBatch, Grid.Info);
            }
            spriteBatch.End();
        }

        //public void AddComponent(Component component)
        //{
        //    // Load the texture for it since the texture is null by default
        //    LoadComponentTexture(component);
        //    Components.Add(component);
        //}

        public List<Component> GetComponents()
        {
            throw new NotImplementedException();
        }

        public bool AddComponent(Component c)
        {
            foreach(Room room in Rooms)
            {
                if(room.Contains(c.TilePosition))
                {
                    room.AddComponent(c);
                    return true;
                }
            }
            return false;
        }



        // Just looks up the component's texture in the Dictionary. If it's not there, it (currently) throws an error
        public void LoadComponentTexture(Component component)
        {
            if (Textures.ContainsKey(component.ComponentType))
            {
                component.LoadContent(Textures[component.ComponentType]);
            }
            else
            {
                throw new Exception("Component's texture was not found. ComponentType: " + component.ComponentType.ToString());
            }
        }

        public List<Tuple<int, Rectangle>> GetRoomPriorities()
        {
            List<Tuple<int, Rectangle>> priorityList = new List<Tuple<int, Rectangle>>();
            foreach (Room room in Rooms){
                var priority = room.GetPriority();
                var rectangle = room.GetInteriorArea();
                priorityList.Add(new Tuple<int,Rectangle>(priority, rectangle));
            }
            return priorityList;
        }

        public bool AddRoom(Room room)
        {
            if(room.GetInteriorArea().Width < 1 || room.GetInteriorArea().Height < 1)
            {
                return false;
            }
            bool intersection = false;
            foreach(Room a in Rooms)
            {
                if(a.GridLocation.Intersects(room.GridLocation))
                {
                    intersection = true;
                }
            }
            if (!intersection)
            {
                Rooms.Add(room);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void RemoveRoom(Room room)
        {
            var removeComponentList = new List<Component>();
            foreach(Component component in room.GetComponents())
            {
                if (component.ComponentType != Component.Component_Type.Structure)
                    removeComponentList.Add(component);
            }
            foreach (Component component1 in removeComponentList)
                room.RemoveComponent(component1);
            Rooms.Remove(room);
        }
    }
}