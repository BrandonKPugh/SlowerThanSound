using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    // Use to create/render buttons
    public class Button
    {
        private Rectangle Location;
        private string Text;
        // Color of the button's background.
        //      The text and border are black.
        private Color Color;
        private SpriteFont Font;
        private Texture2D ButtonTexture;

        /// <param name="color">Background color of the button, text and border are black</param>
        public Button(Rectangle location, string text, Color color)
        {
            this.Location = location;
            this.Text = text;
            this.Color = color;
        }

        public void Initialize()
        {

        }

        public void LoadContent(SpriteFont font, Texture2D buttonTexture)
        {
            this.Font = font;
            this.ButtonTexture = buttonTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(ButtonTexture, Location, Color);
            RenderHelper.CenterString(Text, Font, Location, spriteBatch);
            spriteBatch.End();
        }

        // Determines if the pixel coordinates (i.e. the mouse) overlap the button
        public bool IsWithin(int x, int y)
        {
            return(x >= Location.X && x < Location.X + Location.Width && y >= Location.Y && y < Location.Y + Location.Height);
        }
    }
}
