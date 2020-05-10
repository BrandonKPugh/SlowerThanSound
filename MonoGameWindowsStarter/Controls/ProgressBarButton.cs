using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls
{
    public class ProgressBarButton : Button
    {
        public Color FrontColour { get; set; }
        private float _value;
        // Value from 0.0 to 1.0 that represents the progress bar
        public float Value { get { return _value; } set { if (value < 0) _value = 0f; else if (value > 1) _value = 1f; else _value = value; } }
        public ProgressBarButton(Texture2D texture, SpriteFont font) : base(texture, font)
        {
            _texture = texture;
            _font = font;
            PenColour = ControlConstants.BUTTON_PENCOLOR;
            BackColour = ControlConstants.PROGRESSBUTTON_BACKCOLOR;
            FrontColour = ControlConstants.PROGRESSBUTTON_FRONTCOLOR;
            Value = 1.0f;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color backColour = BackColour;
            Color frontColour = FrontColour;
            if (_isHovering)
                backColour = new Color((int)(backColour.R * .50f), (int)(backColour.G * .50f), (int)(backColour.B * .50f), (int)backColour.A);
            if (_isHovering)
                frontColour = new Color((int)(frontColour.R * .50f), (int)(frontColour.G * .50f), (int)(frontColour.B * .50f), (int)frontColour.A);
            spriteBatch.Draw(_texture, Location, backColour);

            Rectangle source = new Rectangle(0, 0, (int)(_texture.Width * Value), _texture.Height);
            Rectangle dest = new Rectangle((int)Position.X, (int)Position.Y, (int)(Size.X * Value), (int)Size.Y);
            spriteBatch.Draw(_texture, dest, source, frontColour);

            if (!string.IsNullOrEmpty(Text))
            {
                CenterString(Text, _font, Position, Size, spriteBatch, PenColour);
            }
        }

        public override void Selected()
        {
            BackColour = ControlConstants.PROGRESSBUTTON_SELECTEDBACKCOLOR;
            FrontColour = ControlConstants.PROGRESSBUTTON_SELECTEDFRONTCOLOR;
        }
    }
}
