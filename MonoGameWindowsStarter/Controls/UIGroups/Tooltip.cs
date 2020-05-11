using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls.UIGroups
{
    public class Tooltip : UI_Component
    {
        private BorderBox _box;
        private TextBox _text;
        public bool Show;

        public Tooltip(Texture2D pixelTexture, SpriteFont font)
        {
            _box = new BorderBox(pixelTexture)
            {
                BorderBoxInfo = ControlConstants.TOOLTIP_BOX_SIZE
            };

            _text = new TextBox(font)
            {
                TextBoxInfo = ControlConstants.TOOLTIP_TEXT
            };

            Show = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Show)
            {
                Update(gameTime);
                _box.Draw(gameTime, spriteBatch);
                _text.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            SetPosition(Mouse.GetState().Position.ToVector2());
        }

        public void SetPosition(Vector2 p)
        {
            _box.Position = p;
            _text.Position = p;
        }

        public void SetText(string text)
        {
            _text.SetText(text);
        }
    }
}
