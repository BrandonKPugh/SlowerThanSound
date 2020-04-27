using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    public static class RenderHelper
    {
        // Use to center and draw a string within a box
        //      (used for buttons, progress bars, etc.)
        public static void CenterString(string text, SpriteFont spriteFont, Rectangle location, SpriteBatch spriteBatch, float Depth = 0.95f)
        {
            Vector2 MeasuredSize = spriteFont.MeasureString(text);

            float scale = 1.0f;
            if (MeasuredSize.X > location.Width * Config.BUTTON_PADDING_RATIO)
            {
                scale = (location.Width * Config.BUTTON_PADDING_RATIO) / MeasuredSize.X;
            }
            MeasuredSize *= scale;
            Vector2 position = new Vector2(location.X + (location.Width - MeasuredSize.X) / 2, location.Y + (location.Height - MeasuredSize.Y) / 2);
            spriteBatch.DrawString(spriteFont, text, position, Color.Black, 0.0f, new Vector2(0), scale, SpriteEffects.None, Depth);
        }
    }
}
