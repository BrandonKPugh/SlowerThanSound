using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Components
{
    // Inherits from abstract Component class
    public class PowerGenerationComponent : Component
    {
        public int generationAmount = 1;
        public PowerGenerationComponent(int x, int y, Color color) : base(x, y, color)
        {
            this.X = x;
            this.Y = y;
            this.Color = color;
            this.ComponentType = Component_Type.Power_Generation;
        }

        public override void Initialize()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override int getValue()
        {
            return value;
        }
    }
}
