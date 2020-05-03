using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls
{
    public class BorderBox : UI_Component
    {
        private Texture2D _texture;
        public int PenWeight { get; set; }
        public Color Color { get; set; }
        public ControlConstants.BORDERBOX_INFO BorderBoxInfo { set { Position = new Vector2(value.X, value.Y); Size = new Vector2(value.Width, value.Height); Color = value.Color; PenWeight = value.Weight; } }

        public BorderBox()
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
