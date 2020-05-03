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
        public ControlConstants.TEXTBOX_INFO TextBoxInfo { set { Position = new Vector2(value.X, value.Y); Size = new Vector2(value.Width, value.Height); Text = value.Text; } }

        private SpriteFont _font;

        public TextBox(string text, SpriteFont font)
        {
            this.Text = text;
            this._font = font;
            this.Color = Color.White;
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
