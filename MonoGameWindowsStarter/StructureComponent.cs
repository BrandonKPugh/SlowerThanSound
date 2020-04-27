using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    public class StructureComponent : Component
    {
        /// Structure Component
        public StructureComponent(int x, int y, Color color) : base(x, y, color)
        {
            this.ComponentType = Component_Type.Structure;
            this.X = x;
            this.Y = y;
            this.Color = color;
        }

        public override void AlterHealth(int damage)
        {
            // Doesn't (currently) have health, so don't allow alterations
        }

        public override void Initialize()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
