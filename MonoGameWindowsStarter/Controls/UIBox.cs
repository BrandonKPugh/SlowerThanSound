using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls
{
    public class UIBox : UI_Component
    {
        private Texture2D _texture;
        public Color Color;

        public ControlConstants.UIBOX_INFO UIBoxInfo { set { Position = new Vector2(value.X, value.Y); Size = new Vector2(value.Width, value.Height); Color = value.Color; } }

        public UIBox(Texture2D texture)
        {
            _texture = texture;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), Color);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public void SetPosition(Rectangle dest)
        {
            this.Position = new Vector2(dest.X, dest.Y);
            this.Size = new Vector2(dest.Width, dest.Height);
        }
    }
}
