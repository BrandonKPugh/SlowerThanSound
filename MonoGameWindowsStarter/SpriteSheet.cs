using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// A class representing a SpriteSheet
    /// </summary>
    public class SpriteSheet
    {
        // The SpriteSheet's texture
        Texture2D sheet;

        // An array of sprites found in the spritesheet
        Sprite[] sprites;

        /// <summary>
        /// Constructs a SpriteSheet from a texture
        /// </summary>
        /// <param name="texture">The spritesheet texture</param>
        /// <param name="width">The width of the sprites</param>
        /// <param name="height">The height of the sprites</param>
        /// <param name="offset">The offset to the first sprite in the spritesheet</param>
        /// <param name="gutter">The gutter between sprites</param>
        public SpriteSheet(Texture2D texture, int width, int height, int offset = 0, int gutter = 0)
        {
            sheet = texture;
            var columns = (texture.Width - offset) / (width + gutter);
            var rows = (texture.Height - offset) / (height + gutter);
            sprites = new Sprite[rows * columns];
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    sprites[y * columns + x] = new Sprite(new Rectangle(
                        x * (width + gutter) + offset,
                        y * (height + gutter) + offset,
                        width,
                        height
                        ), texture);
                }
            }
        }

        /// <summary>
        /// An indexer for accessing individual sprites in the spritesheet
        /// </summary>
        /// <param name="index">The index of the sprite</param>
        /// <returns>The sprite</returns>
        public Sprite this[int index]
        {
            get => sprites[index];
        }

        /// <summary>
        /// The number of sprites in the spritesheet
        /// </summary>
        public int Count => sprites.Length;
    }
}