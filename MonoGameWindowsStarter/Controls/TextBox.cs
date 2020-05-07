using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls
{
    public class TextBox : UI_Component
    {
        public string Text { get; set; }
        public Color Color { get; set; }
        public ControlConstants.TEXTBOX_INFO TextBoxInfo { set { Position = new Vector2(value.X, value.Y); Size = new Vector2(value.Width, value.Height); Text = value.Text; Color = value.Color; } }

        public Rectangle Location { get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); } }

        private SpriteFont _font;

        public TextBox(SpriteFont font)
        {
            this._font = font;
        }

        public void Initialize()
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            CenterString(Text, _font, Position, Size, spriteBatch, Color);
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
