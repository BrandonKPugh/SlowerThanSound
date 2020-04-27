using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGameWindowsStarter
{
    // Holds all of the Tiles that make up the grid structure
    public class Grid
    {
        // This struct is passed around so that other classes know where the grid is (for when we have camera movement)
        public struct GridInfo
        {
            public Rectangle GridRectangle;
            public int TilesWide;
            public int TilesHigh;
            public int TileWidth;
            public int TileHeight;
        }

        // 2D List, Tiles[x][y]
        public List<List<Tile>> Tiles;

        public GridInfo Info;

        // Texture for each individual tile, currently 32x32 transparent square
        public Texture2D TileTexture;

        // Gets the center of the grid
        public int Center_X { get { return Info.TilesWide / 2; } }
        public int Center_Y { get { return Info.TilesHigh / 2; } }

        // countX and countY to determine how many tiles to create
        public Grid(int countX, int countY)
        {
            Info.TilesWide = countX;
            Info.TilesHigh = countY;
        }

        // Set the location/size of the grid
        public void Initialize(Rectangle dest)
        {
            SetLocation(dest);
            Tiles = new List<List<Tile>>();
            // Create all of the tiles 
            //      (currently each tile holds no data and isn't even used for rendering)
            for (int x = 0; x < Info.TilesWide; x++)
            {
                Tiles.Add(new List<Tile>());
                for (int y = 0; y < Info.TilesHigh; y++)
                {
                    Tiles[x].Add(new Tile(x, y));
                }
            }
        }

        public void LoadContent(Texture2D tileTexture)
        {
            TileTexture = tileTexture;
        }

        // Alter the location/size of the grid
        public void SetLocation(Rectangle dest)
        {
            Info.GridRectangle = dest;

            Info.TileWidth = dest.Width / Info.TilesWide;
            Info.TileHeight = dest.Height / Info.TilesHigh;
        }

        // Uses the grid's location and size to render each tile.
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

            for(int x = 0; x < Info.TilesWide; x++)
            {
                for(int y = 0; y < Info.TilesHigh; y++)
                {
                    Rectangle newDest = new Rectangle(Info.GridRectangle.X + Info.TileWidth * x, Info.GridRectangle.Y + Info.TileHeight * y, Info.TileWidth, Info.TileHeight);
                    spriteBatch.Draw(TileTexture, newDest, Config.GRID_COLOR);
                }
            }

            spriteBatch.End();
        }

        // Takes pixel coordinates and determines which (if any) tile is at that location.
        //      i.e. takes the mouse location and gets the grid coordinates for that tile.
        // boolean determines if the pixel coordinates are even on the grid
        public bool PixelToTile(int pixelX, int pixelY, out int tileX, out int tileY)
        {
            if(pixelX >= Info.GridRectangle.X && pixelX < Info.GridRectangle.X + Info.GridRectangle.Width && pixelY >= Info.GridRectangle.Y && pixelY < Info.GridRectangle.Y + Info.GridRectangle.Height)
            {
                tileX = (int)(pixelX - Info.GridRectangle.X) / Info.TileWidth;
                tileY = (int)(pixelY - Info.GridRectangle.Y) / Info.TileHeight;
                return true;
            }
            else
            {
                // pixel coordinates aren't on the grid at all
                tileX = -1;
                tileY = -1;
                return false;
            }
        }
    }
}
