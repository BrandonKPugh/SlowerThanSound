using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls
{
    public abstract class UI_Component
    {
        public virtual Vector2 Position { get; set; }
        public virtual Vector2 Size { get; set; }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        protected static void CenterString(string text, SpriteFont spriteFont, Vector2 position, Vector2 size, SpriteBatch spriteBatch, Color color, float Depth = 0.95f)
        {
            Vector2 MeasuredSize = spriteFont.MeasureString(text);

            float scale = 1.0f;
            if (MeasuredSize.X > size.X)
            {
                scale = (size.X) / MeasuredSize.X;
            }
            MeasuredSize *= scale;
            Vector2 pos = new Vector2(position.X + (size.X - MeasuredSize.X) / 2, position.Y + (size.Y - MeasuredSize.Y) / 2);
            spriteBatch.DrawString(spriteFont, text, pos, color, 0.0f, new Vector2(0), scale, SpriteEffects.None, Depth);
        }
    }
}
