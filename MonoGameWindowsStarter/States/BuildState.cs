using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameWindowsStarter.Components;
using MonoGameWindowsStarter.Spaceship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameWindowsStarter.Controls;

namespace MonoGameWindowsStarter.States
{
    class BuildState : State
    {
        public Ship Ship;
        private List<UI_Component> _uicomponents;
        public BuildState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Ship ship) : base(game, graphicsDevice, content)
        {
            this.Ship = ship;
            Texture2D buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            SpriteFont buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);
            Texture2D pixelTexture = _content.Load<Texture2D>(Config.PIXEL_TEXTURE);

            Button CombatModeButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.BUILDMODE_COMBATMODE,
            };

            CombatModeButton.Click += CombatModeButton_Click;

            TextBox BuildModeTitle = new TextBox(buttonFont)
            {
                TextBoxInfo = ControlConstants.BUILDMODE_TITLE,
            };

            BorderBox GridBox = new BorderBox(pixelTexture)
            {
                BorderBoxInfo = ControlConstants.BUILDMODE_GRIDBOX
            };

            GridBox.SetPosition(Ship.Grid.Info.GridRectangle, ControlConstants.BUILDMODE_GRIDBOX.Padding);

            _uicomponents = new List<UI_Component>()
            {
                CombatModeButton,
                BuildModeTitle,
                GridBox
            };

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _uicomponents)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            Ship.Draw(spriteBatch, ModeState.State.Build);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new PauseState(_game, _graphicsDevice, _content, this));

            
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                int x = Mouse.GetState().X;
                int y = Mouse.GetState().Y;
                if (Ship.Grid.PixelToTile(x, y, out int tileX, out int tileY))
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
            foreach (var component in _uicomponents)
                component.Update(gameTime);
        }

        private void CombatModeButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new CombatState(_game, _graphicsDevice, _content, Ship));
        }
    }
}
