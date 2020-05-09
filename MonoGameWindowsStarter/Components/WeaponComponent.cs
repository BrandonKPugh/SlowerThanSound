using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Components
{
    // Inherits from abstract Component class
    public class WeaponComponent : Component
    {
        public float WeaponDamage = 2f;
        public float ShotsPerSecond = 0.5f;
        public float PowerPerShot = 2f;
        public WeaponComponent(int x, int y, Color color) : base(x, y, color)
        {
            this.X = x;
            this.Y = y;
            this.Color = color;
            this.ComponentType = Component_Type.Weapon;
        }

        public override void Initialize()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override int getValue()
        {
            return 1 + (int)WeaponDamage;
        }
    }
}
