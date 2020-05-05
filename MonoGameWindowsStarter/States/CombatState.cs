﻿using Microsoft.Xna.Framework;
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
        private List<UI_Component> _uicomponents;

        public CombatState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Ship ship)
          : base(game, graphicsDevice, content)
        {
            ShipConstants.Initialize();

            Ship = new Ship();
            Ship.Initialize(ShipConstants.COMPONENTS);

            Texture2D buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            SpriteFont buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);
            Texture2D tileTexture = content.Load<Texture2D>("Tile");
            Texture2D pixelTexture = _content.Load<Texture2D>(Config.PIXEL_TEXTURE);

            Dictionary<Component.Component_Type, Texture2D> textures = new Dictionary<Component.Component_Type, Texture2D>();
            Texture2D weaponTexture = content.Load<Texture2D>("Component_Weapon");
            textures.Add(Component.Component_Type.Weapon, weaponTexture);
            Texture2D structureTexture = content.Load<Texture2D>("Structure");
            textures.Add(Component.Component_Type.Structure, structureTexture);

            Button BuildModeButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.COMBATMODE_BUILDMODE,
            };

            BuildModeButton.Click += BuildModeButton_Click;

            Button TargetStoragesButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.COMBATMODE_TARGETSTORAGES,
            };

            TargetStoragesButton.Click += TargetStoragesButton_Click;

            Button TargetWeaponsButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.COMBATMODE_TARGETWEAPONS,
            };

            TargetWeaponsButton.Click += TargetWeaponsButton_Click;

            Button TargetPowerButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.COMBATMODE_TARGETPOWER,
            };

            TargetPowerButton.Click += TargetPowerButton_Click;

            TextBox CombatModeTitle = new TextBox(buttonFont)
            {
                TextBoxInfo = ControlConstants.COMBATMODE_TITLE,
            };

            BorderBox GridBox = new BorderBox(pixelTexture)
            {
                BorderBoxInfo = ControlConstants.BUILDMODE_GRIDBOX
            };
            GridBox.SetPosition(Ship.Grid.Info.GridRectangle, ControlConstants.BUILDMODE_GRIDBOX.Padding);

            _uicomponents = new List<UI_Component>()
            {
                BuildModeButton,
                CombatModeTitle,
                GridBox,
                TargetStoragesButton,
                TargetWeaponsButton,
                TargetPowerButton
            };


            Ship.LoadContent(textures, tileTexture);
            this.Ship = ship;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Ship.Draw(spriteBatch, ModeState.State.Combat);

            spriteBatch.Begin();

            foreach (var component in _uicomponents)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new PauseState(_game, _graphicsDevice, _content, this));

            foreach (var component in _uicomponents)
                component.Update(gameTime);

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
        private void BuildModeButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new BuildState(_game, _graphicsDevice, _content, Ship));
        }

        private void TargetStoragesButton_Click(object sender, EventArgs e)
        {

        }

        private void TargetWeaponsButton_Click(object sender, EventArgs e)
        {

        }

        private void TargetPowerButton_Click(object sender, EventArgs e)
        {

        }
    }
}