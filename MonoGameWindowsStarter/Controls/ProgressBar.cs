﻿using Microsoft.Xna.Framework;
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
        private BorderBox _borderBox;
        public ControlConstants.PROGRESSBAR_INFO ProgressBarInfo { set { this.BackgroundColor = value.BackColor; this.ProgressionColor = value.FrontColor; this.Location = value.Location; this.Text = value.Text; _borderBox.Location = value.Location; _borderBox.Color = Color.Black; _borderBox.PenWeight = value.PenWeight; } }
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

        public ProgressBar(Texture2D pixelTexture, SpriteFont font)
        {
            //this.BarTexture = pixelTexture;
            this.ProgressionTexture = pixelTexture;
            this.BackgroundTexture = pixelTexture;
            this.Font = font;
            _borderBox = new BorderBox(pixelTexture);
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
            Rectangle progressionDest = new Rectangle(Location.X, Location.Y, (int) (Location.Width * Value), Location.Height);
            spriteBatch.Draw(BackgroundTexture, Location, BackgroundColor);
            spriteBatch.Draw(ProgressionTexture, progressionDest, ProgressionColor);
            Button.CenterString(Text, Font, new Vector2(Location.X, Location.Y), new Vector2(Location.Width, Location.Height), spriteBatch, ControlConstants.BAR_PENCOLOR);

        }

        public void SetText(string text)
        {
            this.Text = text;
        }
    }
}
