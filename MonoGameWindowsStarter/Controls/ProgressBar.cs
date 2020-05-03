using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls
{
    // Use to create/render a progress bar (i.e. health bar, ammo bar)
    public class ProgressBar : UI_Component
    {
        // Between 0 and 1, represents the progress
        //      Moves from left (0) to right (1)
        private float _value;
        public float Value
        { 
            get { return _value; }
            set { _value = value;
                if (_value > 1f)
                    _value = 1f;
                if (_value < 0f)
                    _value = 0f; }
        }

        private Rectangle Location;

        // Optional Text
        //      Overlays the progress bar, centered, black text
        private string Text;
        private SpriteFont Font;

        // Represents the border of the progress bar, black
        private Texture2D BarTexture;

        // Represents the background of the progress bar
        private Color BackgroundColor;
        //      Probably use 1x1 pixel Texture for this
        private Texture2D BackgroundTexture; 

        // Represents the moving progress portion
        private Color ProgressionColor;
        //      Probably use 1x1 pixel Texture for this too
        private Texture2D ProgressionTexture;

        // Creates a progress bar
        //      Value and text are optional
        public ProgressBar(Rectangle location, Color backgroundColor, Color progressionColor, float value = 1.0f, string text = "")
        {
            this.Location = location;
            this.BackgroundColor = backgroundColor;
            this.ProgressionColor = progressionColor;
            this.Text = text;
        }

        public void Initialize()
        {

        }

        // Needs all three textures (progression and background are probably 1x1 pixel Texture) as well as a font
        public void LoadContent(Texture2D barTexture, Texture2D progressionTexture, Texture2D backgroundTexture, SpriteFont font)
        {
            this.BarTexture = barTexture;
            this.ProgressionTexture = progressionTexture;
            this.BackgroundTexture = backgroundTexture;
            this.Font = font;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            Rectangle progressionDest = new Rectangle(Location.X, Location.Y, (int) (Location.Width * Value), Location.Height);
            spriteBatch.Draw(BackgroundTexture, Location, BackgroundColor);
            spriteBatch.Draw(ProgressionTexture, progressionDest, ProgressionColor);
            spriteBatch.Draw(BarTexture, Location, Color.Black);
            Button.CenterString(Text, Font, Position, Size, spriteBatch, ControlConstants.BAR_PENCOLOR);

            spriteBatch.End();

        }
    }
}
