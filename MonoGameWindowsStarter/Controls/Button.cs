using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls
{
    public class Button : UI_Component
    {
        #region Fields

        protected MouseState _currentMouse;

        protected SpriteFont _font;

        protected bool _isHovering;

        protected MouseState _previousMouse;

        protected Texture2D _texture;

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; }
        public Color BackColour { get; set; }
        public bool IsActive = true;

        public ControlConstants.BUTTON_INFO ButtonInfo { set { Position = new Vector2(value.X, value.Y); Size = new Vector2(value.Width, value.Height); Text = value.Text; } }

        public Rectangle Location
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        public string Text { get; set; }

        #endregion

        #region Methods

        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;

            _font = font;

            PenColour = ControlConstants.BUTTON_PENCOLOR;

            BackColour = ControlConstants.BUTTON_BACKCOLOR;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color colour = BackColour;
            if (_isHovering || !IsActive)
                colour = new Color((int) (colour.R * .50f), (int)(colour.G * .50f), (int)(colour.B * .50f), (int)colour.A);

            spriteBatch.Draw(_texture, Location, colour);

            if (!string.IsNullOrEmpty(Text))
            {
                CenterString(Text, _font, Position, Size, spriteBatch, PenColour);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                _previousMouse = _currentMouse;
                _currentMouse = Mouse.GetState();

                var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

                _isHovering = false;

                if (mouseRectangle.Intersects(Location))
                {
                    _isHovering = true;

                    if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                    {
                        Click?.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        public virtual void Selected()
        {
            BackColour = ControlConstants.BUTTON_SELECTED;
        }

        #endregion
    }
}
