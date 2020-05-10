using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Components;
using MonoGameWindowsStarter.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
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
        private int roomFlashingFrames = 0;
        private enum Room_Flashing_State
        {
            Bright,
            Dim
        }
        private Room_Flashing_State roomFlashingState = Room_Flashing_State.Bright;

        private List<Component> Components;
        private Ship Ship;
        private Grid Grid;

        public Rectangle GridLocation;
        public Room_Type RoomType;
        public uint RoomID;

        private int roomHealth;
        private int maxRoomHealth;
        public int RoomHealth { get { return roomHealth; } }
        public bool isBroken;


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
            if (roomFlashingFrames <= 0)
            {
                roomFlashingFrames = ControlConstants.COMBATMODE_ROOMFLASHINGFRAMES;
                if(roomFlashingState == Room_Flashing_State.Bright)
                {
                    roomFlashingState = Room_Flashing_State.Dim;
                }
                else if(roomFlashingState == Room_Flashing_State.Dim)
                {
                    roomFlashingState = Room_Flashing_State.Bright;
                }
            }
            roomFlashingFrames--;
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

        // Determines if a grid coordinate is inside this room, this includes structures
        public bool Contains(int x, int y)
        {
            return (x >= GridLocation.X && x <= GridLocation.X + GridLocation.Width && y >= GridLocation.Y && y <= GridLocation.Y + GridLocation.Height);
        }
        public bool Contains(Point p)
        {
            return Contains(p.X, p.Y);
        }

        public bool InteriorContains(int x, int y)
        {
            return (x > GridLocation.X && x < GridLocation.X + GridLocation.Width && y > GridLocation.Y && y < GridLocation.Y + GridLocation.Height);
        }

        public bool InteriorContains(Point p)
        {
            return InteriorContains(p.X, p.Y);
        }

        public void AddComponent(Component component)
        {
            if(RoomType == Room_Type.None)
            {
                RoomType = GetRoomType(component);
            }
            if(component.Texture == null)
                Ship.LoadComponentTexture(component);
            Components.Add(component);
        }
        public int RemoveComponent(Component component)
        {
            var value = component.value;
            Components.Remove(component);
            foreach (Component comp in Components)
            {
                if (comp.ComponentType != Component.Component_Type.Structure)
                    return value;
            }
            SetRoomType(Room_Type.None);
            return value;
        }

        private Room_Type GetRoomType(Component component)
        {
            switch(component.ComponentType)
            {
                case Component.Component_Type.Material_Storage:
                    {
                        return Room_Type.Material_Storage;
                    }
                case Component.Component_Type.Weapon:
                    {
                        return Room_Type.Weapon;
                    }
                case Component.Component_Type.Power_Generation:
                    {
                        return Room_Type.Power_Generation;
                    }
                case Component.Component_Type.Power_Storage:
                    {
                        return Room_Type.Power_Storage;
                    }
                default:
                    {
                        throw new NotImplementedException("RoomType/ComponentType not associated.");
                    }

            }
        }

        public int GetHealthAlpha()
        {
            if(roomHealth > 0)
                return (int)((1f - ((float)roomHealth / (float)maxRoomHealth)) * ControlConstants.COMBATMODE_ROOMHEALTHALPHA);
            else
            {
                if(roomFlashingState == Room_Flashing_State.Bright)
                {
                    return ControlConstants.COMBATMODE_ROOMHEALTHALPHA * 2;
                }
                else
                {
                    return ControlConstants.COMBATMODE_ROOMHEALTHALPHA;
                }
            }    
        }

        // Gets all components including strucures
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
            var bottomRight = info.TileBounds(GridLocation.X+GridLocation.Width, GridLocation.Y+GridLocation.Height);
            var x = topLeft.X + topLeft.Width;
            var y = topLeft.Y + topLeft.Height;
            var width = bottomRight.X - x;
            var height = bottomRight.Y - y;

            return new Rectangle(x, y, width, height);
        }

        public Vector2 GetCenter()
        {
            var info = Grid.Info;
            var topLeft = info.TileBounds(GridLocation.X, GridLocation.Y);
            var bottomRight = info.TileBounds(GridLocation.X + GridLocation.Width, GridLocation.Y + GridLocation.Height);
            var x = topLeft.X + topLeft.Width;
            var y = topLeft.Y + topLeft.Height;
            var width = bottomRight.X - x;
            var height = bottomRight.Y - y;

            return new Vector2(x+(width/2), y+(height/2));
        }

        public int GetPriority()
        {
            var priority = 1;
            //Debug value, will get value of each component and return the total value at some point
            foreach(Component comp in Components)
            {
                priority += comp.getValue();
            }
            return priority;
        }

        public void SetRoomType(Room_Type type)
        {
            RoomType = type;
        }

        public int SetRoomHealth()
        {
            roomHealth = 0;
            foreach (Component component in Components)
            {
                roomHealth += 1;
            }
            maxRoomHealth = roomHealth;
            return roomHealth;
        }

        public void AlterHealth(int damage)
        {
            roomHealth -= damage;
            if (roomHealth < 0)
                roomHealth = 0;
            if (roomHealth == 0)
                isBroken = true;
        }

        public string GetInfo()
        {
            switch (RoomType)
            {
                case Room_Type.None:
                    {
                        return "Room Type: None";
                    }
                case Room_Type.Weapon:
                    {
                        string s = "Room Type: Weapon";
                        s += "\nComponents: " + ComponentCount();
                        s += "\nDamage per shot: " + string.Format("{0:0.00}", DamagePerShot());
                        s += "\nShots per second: " + string.Format("{0:0.00}", ShotsPerSecond());
                        s += "\nDamage per second: " + string.Format("{0:0.00}", DamagePerSecond());
                        s += "\nPower per shot: " + string.Format("{0:0.00}", PowerPerShot());
                        s += "\nPower per second: " + string.Format("{0:0.00}", PowerUsePerSecond());
                        return s;
                    }
                case Room_Type.Material_Storage:
                    {
                        string s = "Room Type: Material Storage";
                        s += "\nComponents: " + ComponentCount();
                        s += "\nStorage Capacity: " + string.Format("{0:0}", MaterialStorageCapacity());
                        return s;
                    }
                case Room_Type.Power_Storage:
                    {
                        string s = "Room Type: Battery";
                        s += "\nComponents: " + ComponentCount();
                        s += "\nPower Capacity: " + string.Format("{0:0}", PowerStorageCapacity());
                        return s;
                    }
                case Room_Type.Power_Generation:
                    {
                        string s = "Room Type: Generator";
                        s += "\nComponents: " + ComponentCount();
                        s += "\nPower per second: " + string.Format("{0:0.00}", PowerGenerationPerSecond());
                        return s;
                    }
                case Room_Type.Shield:
                    {
                        string s = "Room Type: Shield";
                        s += "\nComponents: " + ComponentCount();
                        return s;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        // Excludes structures
        public int ComponentCount()
        {
            int count = 0;
            foreach(Component c in Components)
            {
                if(c.ComponentType != Component.Component_Type.Structure)
                {
                    count++;
                }
            }
            return count;
        }

        private int MaterialStorageCapacity()
        {
            int capacity = 0;
            foreach (Component c in Components)
            {
                if (c.ComponentType == Component.Component_Type.Material_Storage)
                {
                    capacity += ((MaterialStorageComponent)c).StorageCapacity;
                }
            }
            return (int)(Math.Pow(capacity, 1.5f));
        }

        private float PowerGenerationPerSecond()
        {
            float power = 0f;
            foreach (Component c in Components)
            {
                if (c.ComponentType == Component.Component_Type.Power_Generation)
                {
                    power += ((PowerGenerationComponent)c).PowerPerSecond;
                }
            }
            return (float)(Math.Pow(power, 1.5f));
        }

        private float PowerStorageCapacity()
        {
            float power = 0f;
            foreach (Component c in Components)
            {
                if (c.ComponentType == Component.Component_Type.Power_Storage)
                {
                    power += ((PowerStorageComponent)c).PowerCapacity;
                }
            }
            return (float)(Math.Pow(power, 1.5f));
        }

        public float DamagePerShot()
        {
            float damage = 0f;
            foreach (Component c in Components)
            {
                if(c.ComponentType == Component.Component_Type.Weapon)
                {
                    damage += ((WeaponComponent)c).WeaponDamage;
                }
            }
            return (float)Math.Sqrt(damage);
        }

        private float ShotsPerSecond()
        {
            float sps = 0f;
            foreach(Component c in Components)
            {
                if(c.ComponentType == Component.Component_Type.Weapon)
                {
                    sps += ((WeaponComponent)c).ShotsPerSecond;
                }
            }
            return (float)Math.Sqrt(sps);
        }

        private float PowerPerShot()
        {
            float pps = 0f;
            foreach (Component c in Components)
            {
                if (c.ComponentType == Component.Component_Type.Weapon)
                {
                    pps += ((WeaponComponent)c).PowerPerShot;
                }
            }
            return pps;
        }

        private float DamagePerSecond()
        {
            return ShotsPerSecond() * DamagePerShot();
        }

        private float PowerUsePerSecond()
        {
            return PowerPerShot() * ShotsPerSecond();
        }

        public Color GetColor()
        {
            switch (RoomType)
            {
                case Room_Type.None:
                    {
                        return ComponentConstants.COMPONENT_DEFAULT_COLOR;
                    }
                case Room_Type.Weapon:
                    {
                        return ComponentConstants.COMPONENT_WEAPON_COLOR;
                    }
                case Room_Type.Material_Storage:
                    {
                        return ComponentConstants.COMPONENT_MATERIALSTORAGE_COLOR;
                    }
                case Room_Type.Power_Generation:
                    {
                        return ComponentConstants.COMPONENT_POWERGENERATOR_COLOR;
                    }
                case Room_Type.Power_Storage:
                    {
                        return ComponentConstants.COMPONENT_POWERSTORAGE_COLOR;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

    }
}
