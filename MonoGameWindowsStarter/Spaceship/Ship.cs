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
        private List<Component> Components;
        // Dictionary for component textures
        private Dictionary<Component.Component_Type, Texture2D> Textures;

        public Grid Grid;

        public List<Room> Rooms;

        public Ship()
        {

        }

        public void Initialize(List<Tuple<Point,Point>> rooms)
        {
            Textures = new Dictionary<Component.Component_Type, Texture2D>();

            Grid = new Grid(ShipConstants.SHIP_GRID);

            Rooms = new List<Room>();

            foreach (Tuple<Point,Point> a in rooms)
            {
                Rooms.Add(new Room(this, a.Item1, a.Item2, Room.Room_Type.None));
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
            return Components;
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

        public Dictionary<uint,Rectangle> GetRoomPriorities()
        {
            Dictionary<uint, Rectangle> priorityDict = new Dictionary<uint, Rectangle>();
            foreach (Room room in Rooms){
                var priority = room.GetPriority();
                var rectangle = room.GetArea();
                priorityDict.Add(priority, rectangle);
            }
            return priorityDict;
        }
    }
}