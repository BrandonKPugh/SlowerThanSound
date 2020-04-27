using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    // Inherits from abstract Component class
    public class WeaponComponent : Component
    {
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
    }
}
