using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameWindowsStarter.Spaceship;
using static MonoGameWindowsStarter.States.BuildState;
using MonoGameWindowsStarter.Rendering;
using MonoGameWindowsStarter.States;

namespace MonoGameWindowsStarter.Components
{
    // abstract component class that all other components will inherit from
    public abstract class Component
    {
        // Component_Type to help manage what type each component is
        public enum Component_Type
        {
            //Generic,
            Structure,
            Weapon,
            Material_Storage,
            Power_Storage,
            Power_Generation,
            Shield
        }

        // These are the coordinates in the grid, not the pixel coordinates
        public int X;
        public int Y;

        public int priority = 5;
        public int value = 5;
        public int health = 5;
        public Point TilePosition { get { return new Point(X, Y); } set { X = value.X; Y = value.Y; } }

        public Component_Type ComponentType;

        //// Health of this specific component
        //public int Health;
        //public bool Broken { get { return Health <= 0; } }

        public Color Color;
        public Texture2D Texture;
        public Sprite Sprite;

        public Component(int x, int y, Color color)
        {
            this.X = x;
            this.Y = y;
            this.Color = color;
            // For now, grabs the generic health value
            //this.Health = ComponentConstants.GENERIC_COMPONENT_HEALTH;
        }

        public abstract void Initialize();

        public virtual void LoadContent(Texture2D texture)
        {
            Texture = texture;
        }
        public virtual void LoadContent(Sprite sprite)
        {
            Sprite = sprite;
        }

        public abstract void Update(GameTime gameTime);


        public abstract int getValue();

        public virtual void Draw(SpriteBatch spriteBatch, Grid.GridInfo gridInfo)
        {
                //spriteBatch.Draw(Texture, gridInfo.TileBounds(X, Y), Color);
                Sprite.Draw(spriteBatch, gridInfo.TileBounds(X, Y), Color.White);
        }

        internal static bool RoomTypeMatches(Placement_Type placementType, Room.Room_Type roomType)
        {
            switch (placementType)
            {
                case Placement_Type.None:
                    {
                        return false;
                    }
                case Placement_Type.Weapon:
                    {
                        return (roomType == Room.Room_Type.Weapon);
                    }
                case Placement_Type.Storage:
                    {
                        return (roomType == Room.Room_Type.Material_Storage);
                    }
                case Placement_Type.Generator:
                    {
                        return (roomType == Room.Room_Type.Power_Generation);
                    }
                case Placement_Type.Battery:
                    {
                        return (roomType == Room.Room_Type.Power_Storage);
                    }
                default:
                    {
                        throw new NotImplementedException("Unknown placement type");
                    }
            }
        }

        internal static int GetBaseValue(Placement_Type type)
        {
            switch (type)
            {
                case Placement_Type.Weapon:
                case Placement_Type.PlacingWeapon:
                    return new WeaponComponent(0, 0, Color.White).getValue();
                case Placement_Type.Storage:
                case Placement_Type.PlacingStorage:
                    return new MaterialStorageComponent(0, 0, Color.White).getValue();
                case Placement_Type.Generator:
                case Placement_Type.PlacingGenerator:
                    return new PowerGenerationComponent(0, 0, Color.White).getValue();
                case Placement_Type.Battery:
                case Placement_Type.PlacingBattery:
                    return new PowerStorageComponent(0, 0, Color.White).getValue();
            }
            return 0;
        }
    }
}
