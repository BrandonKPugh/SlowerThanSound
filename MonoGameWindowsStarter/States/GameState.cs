using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.States
{
    public class GameState : State
    {
        public Grid Grid;
        public Ship Ship;

        // Temporary:
        Button BuildModeButton;
        ProgressBar HealthBar;
        int frame = 0;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            Grid = new Grid(Config.GRID_COUNT_X, Config.GRID_COUNT_Y);
            Grid.Initialize(Config.GRID_DESTINATION);

            Ship = new Ship();
            Ship.Initialize(Config.COMPONENTS);

            //*
            Rectangle loc = new Rectangle(Config.PRIMARY_BUTTON_X, Config.PRIMARY_BUTTON_Y, Config.PRIMARY_BUTTON_WIDTH, Config.PRIMARY_BUTTON_HEIGHT);
            BuildModeButton = new Button(loc, "Build Mode", Color.ForestGreen);

            loc.Y -= 200;
            HealthBar = new ProgressBar(loc, Color.DarkGray, Color.LightYellow, 1.0f, "Test Text");
            HealthBar.Initialize();

            Texture2D tileTexture = content.Load<Texture2D>("Tile");
            Grid.LoadContent(tileTexture);

            Dictionary<Component.Component_Type, Texture2D> textures = new Dictionary<Component.Component_Type, Texture2D>();
            //Texture2D componentTexture = Content.Load<Texture2D>("Component");
            //textures.Add(Component.Component_Type.Generic, componentTexture);
            Texture2D weaponTexture = content.Load<Texture2D>("Component_Weapon");
            textures.Add(Component.Component_Type.Weapon, weaponTexture);
            Texture2D structureTexture = content.Load<Texture2D>("Structure");
            textures.Add(Component.Component_Type.Structure, structureTexture);

            //*
            Texture2D buttonTexture = content.Load<Texture2D>("Button");
            SpriteFont font = content.Load<SpriteFont>("DebugFont");
            BuildModeButton.LoadContent(font, buttonTexture);

            Texture2D barTexture = content.Load<Texture2D>("Progress_Bar");
            Texture2D pixel = content.Load<Texture2D>("pixel");
            HealthBar.LoadContent(barTexture, pixel, pixel, font);
            //*/

            Ship.LoadContent(textures);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Grid.Draw(spriteBatch);

            Ship.Draw(spriteBatch, Grid.Info);

            BuildModeButton.Draw(spriteBatch);

            HealthBar.Draw(spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                int x = Mouse.GetState().X;
                int y = Mouse.GetState().Y;
                if (Grid.PixelToTile(x, y, out int tileX, out int tileY))
                {
                    Component found = null;
                    foreach (Component c in Ship.GetComponents())
                    {
                        if (c.X == tileX && c.Y == tileY)
                        {
                            found = c;
                            break;
                        }
                    }
                    if (found == null)
                    {
                        Component newComponent = new WeaponComponent(tileX, tileY, Config.COMPONENT_WEAPON_COLOR);
                        Ship.AddComponent(newComponent);
                    }
                }
            }
            HealthBar.Update((1000 - frame) / 1000f);
            frame++;
            if (frame > 1000)
                frame = 0;
        }
    }
}