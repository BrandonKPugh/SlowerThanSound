﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameWindowsStarter.Spaceship;

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

        public Component_Type ComponentType;

        // Health of this specific component
        public int Health;
        public bool Broken { get { return Health <= 0; } }

        public Color Color;
        public Texture2D Texture;

        public Component(int x, int y, Color color)
        {
            this.X = x;
            this.Y = y;
            this.Color = color;
            // For now, grabs the generic health value
            this.Health = ComponentConstants.GENERIC_COMPONENT_HEALTH;
        }

        public abstract void Initialize();

        public virtual void LoadContent(Texture2D texture)
        {
            Texture = texture;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void AlterHealth(int damage)
        {
            Health -= damage;
            if (Health < 0)
                Health = 0;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Grid.GridInfo gridInfo)
        {
            if (Broken)
            {
                // Not sure how to handle this yet
                throw new Exception("Component is broken!");
            }
            else
            {
                spriteBatch.Draw(Texture, gridInfo.TileBounds(X, Y), Color);
            }
        }

    }
}