using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameWindowsStarter.Components;
using MonoGameWindowsStarter.Spaceship;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Rendering;
using MonoGameWindowsStarter.States;

namespace MonoGameWindowsStarter.Spaceship
{
    public class Ship
    {
        // List of all components
        //      this will likely be a different data structure later
        // private List<Component> Components;
        // Dictionary for component textures
        private Dictionary<Component.Component_Type, Texture2D> Textures;
        private Dictionary<Component.Component_Type, Sprite> Sprites;

        public Grid Grid;

        public List<Room> Rooms;

        #region RESOURCES
        private float _power;
        public float Power { get { return _power; } set { _power = Math.Min(value, maxPower); } }
        public int maxPower = 0;
        public int Material = 25;
        public int maxMaterial = 0;
        public int MaxHealth = 1;
        public int CurrentHealth;
        private int PreviousHealth;
        public List<Tuple<Room, Projectile.Attack_Against>> Attacks = new List<Tuple<Room, Projectile.Attack_Against>>();
        private CombatState _combatState;
        #endregion

        //TimeSpan timer;
        public Ship()
        {

        }

        public void Update(GameTime gameTime)
        {
            foreach(Room room in Rooms)
            {
                room.Update(gameTime);
            }
            FireWeapons(gameTime);
        }

        public void Initialize(List<Tuple<Point,Point, Room.Room_Type>> rooms)
        {
            Textures = new Dictionary<Component.Component_Type, Texture2D>();
            Sprites = new Dictionary<Component.Component_Type, Sprite>();

            Grid = new Grid(ShipConstants.SHIP_GRID);

            Rooms = new List<Room>();

            foreach (Tuple<Point,Point, Room.Room_Type> a in rooms)
            {
                Rooms.Add(new Room(this, Grid, a.Item1, a.Item2, a.Item3));
            }
            CurrentHealth = MaxHealth;
        }

        // Alternative initialization, allows passing in some preset components
        public void Initialize(List<Component> components)
        {
            Textures = new Dictionary<Component.Component_Type, Texture2D>();
            Sprites = new Dictionary<Component.Component_Type, Sprite>();

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

        public void LoadContent(Dictionary<Component.Component_Type, Sprite> textures, Texture2D TileTexture)
        {
            Grid.LoadContent(TileTexture);

            // Loop through each texture that was just passed in and add it to the ship's dictionary
            foreach (KeyValuePair<Component.Component_Type, Sprite> pair in textures)
            {
                if (!Sprites.ContainsKey(pair.Key))
                    Sprites.Add(pair.Key, pair.Value);
            }
            // Loop through all components on the ship and set their textures
            foreach (Room room in Rooms)
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
            if (Sprites.ContainsKey(component.ComponentType))
            {
                //component.LoadContent(Textures[component.ComponentType]);
                component.LoadContent(Sprites[component.ComponentType]);
            }
            else
            {
                throw new Exception("Component's texture was not found. ComponentType: " + component.ComponentType.ToString());
            }
        }

        public void LoadComponentSprite(Component component)
        {
            if (Sprites.ContainsKey(component.ComponentType))
            {
                component.LoadContent(Sprites[component.ComponentType]);
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
                if (!room.isBroken)
                {
                    var priority = room.GetPriority();
                    var rectangle = room.GetInteriorArea();
                    priorityList.Add(new Tuple<int, Rectangle>(priority, rectangle));
                }
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

        public void AttackEnemy(Room weapon, CombatState combatState, Projectile.Attack_Against against)
        {
            List<Tuple<Room, Projectile.Attack_Against>> toRemove = new List<Tuple<Room, Projectile.Attack_Against>>();
            foreach (Tuple<Room, Projectile.Attack_Against> attack in Attacks)
            {
                if(attack.Item1.RoomID == weapon.RoomID)
                {
                    toRemove.Add(attack);
                }
            }
            foreach(Tuple<Room, Projectile.Attack_Against> attack in toRemove)
            {
                Attacks.Remove(attack);
            }
            Attacks.Add(new Tuple<Room, Projectile.Attack_Against>(weapon, against));
            _combatState = combatState;
        }

        public void SetShipHealth()
        {
            PreviousHealth = MaxHealth;
            MaxHealth = 0;
            foreach (Room room in Rooms)
            {
                MaxHealth += room.SetRoomHealth();
            }
            float healthDifference;
            if (PreviousHealth != 0)
                healthDifference = MaxHealth / PreviousHealth;
            else
            {
                healthDifference = 1;
            }
            CurrentHealth = (int)(CurrentHealth * healthDifference);
        }

        public void SetCombatValues()
        {
            foreach (Room room in Rooms)
            {
                maxPower += (int)room.PowerStorageCapacity();
                maxMaterial += room.MaterialStorageCapacity();
            }
            //timer = new TimeSpan(0);
        }

        public void AlterHealth(float damage)
        {
            CurrentHealth -= (int)damage;
                
        }



        public Room GetRoom(Point tilePosition, bool interiorOnly)
        {
            return GetRoom(tilePosition.X, tilePosition.Y, interiorOnly);
        }

        public Room GetRoom(int x, int y, bool interiorOnly)
        {
            foreach(Room room in Rooms)
            {
                if (interiorOnly && room.InteriorContains(x, y))
                {
                    return room;
                }
                else if(!interiorOnly && room.Contains(x, y))
                {
                    return room;
                }
            }
            return null;
        }

        public bool IsValidShip()
        {
            bool hasWeapon = false;
            bool hasPowerGen = false;
            bool hasPowerStorage = false;
            bool hasMaterialStorage = false;

            foreach(Room room in Rooms)
            {
                switch (room.RoomType)
                {
                    case Room.Room_Type.Weapon:
                        {
                            hasWeapon = true;
                            break;
                        }
                    case Room.Room_Type.Material_Storage:
                        {
                            hasMaterialStorage = true;
                            break;
                        }
                    case Room.Room_Type.Power_Storage:
                        {
                            hasPowerStorage = true;
                            break;
                        }
                    case Room.Room_Type.Power_Generation:
                        {
                            hasPowerGen = true;
                            break;
                        }
                    case Room.Room_Type.Shield:
                        break;
                }
            }
            return (hasWeapon && hasPowerGen && hasPowerStorage && hasMaterialStorage);
        }

        private float TotalPowerPerSecond()
        {
            float pow = 0f;
            foreach(Room room in Rooms)
            {
                if (room.RoomType == Room.Room_Type.Power_Generation)
                {
                    if (!room.isBroken)
                    {
                        pow += room.PowerGenerationPerSecond();
                    }
                }
            }
            return pow;
        }

        private float TotalPowerCapacity()
        {
            float pow = 0f;
            foreach (Room room in Rooms)
            {
                if (room.RoomType == Room.Room_Type.Power_Storage)
                {
                    if (!room.isBroken)
                    {
                        pow += room.PowerStorageCapacity();
                    }
                }
            }
            return pow;
        }

        private void FireWeapons(GameTime gameTime)
        {
            foreach (Tuple<Room, Projectile.Attack_Against> attack in Attacks)
            {
                Room room = attack.Item1;
                Projectile.Attack_Against against = attack.Item2;
                if (room.CanShoot && Power >= room.PowerPerShot())
                {
                    room.Shoot();
                    Power -= (int)room.PowerPerShot();
                    _combatState.AddProjectile(new Projectile(new Point(1000, 1000), room.GetCenter(), (int)room.DamagePerShot(), against, _combatState));
                }
            }
        }
    }
}