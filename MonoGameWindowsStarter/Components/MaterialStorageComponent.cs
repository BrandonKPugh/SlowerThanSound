using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Components
{
    // Inherits from abstract Component class
    public class MaterialStorageComponent : Component
    {
        public int maxStorage = 1;
        public int storageAmount = 1;
        public MaterialStorageComponent(int x, int y, Color color) : base(x, y, color)
        {
            this.X = x;
            this.Y = y;
            this.Color = color;
            this.ComponentType = Component_Type.Material_Storage;
        }

        public override void Initialize()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override int getValue()
        {
            return value + storageAmount;
        }
    }
}
