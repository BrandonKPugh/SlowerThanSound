using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameWindowsStarter.Controls;
using MonoGameWindowsStarter.Components;
using MonoGameWindowsStarter.Spaceship;

namespace MonoGameWindowsStarter.States
{
    public class CombatState : State
    {
        public Spaceship.Ship Ship;

        public CombatState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            ShipConstants.Initialize();

            Ship = new Ship();
            Ship.Initialize(ShipConstants.COMPONENTS);

            Texture2D tileTexture = content.Load<Texture2D>("Tile");

            Dictionary<Component.Component_Type, Texture2D> textures = new Dictionary<Component.Component_Type, Texture2D>();
            Texture2D weaponTexture = content.Load<Texture2D>("Component_Weapon");
            textures.Add(Component.Component_Type.Weapon, weaponTexture);
            Texture2D structureTexture = content.Load<Texture2D>("Structure");
            textures.Add(Component.Component_Type.Structure, structureTexture);

            //SpriteFont font = content.Load<SpriteFont>("DebugFont");
            //Texture2D pixel = content.Load<Texture2D>("pixel");

            Ship.LoadContent(textures, tileTexture);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Ship.Draw(spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new PauseState(_game, _graphicsDevice, _content, this));

            /*
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
                        Component newComponent = new WeaponComponent(tileX, tileY, ComponentConstants.COMPONENT_WEAPON_COLOR);
                        Ship.AddComponent(newComponent);
                    }
                }
            }
            */
        }
    }
}