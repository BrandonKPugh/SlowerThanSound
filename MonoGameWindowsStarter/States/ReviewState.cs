using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameWindowsStarter.Controls;
using MonoGameWindowsStarter.Controls.UIGroups;
using MonoGameWindowsStarter.Spaceship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.States
{
    public class ReviewState : State
    {
        public Ship Ship;
        private List<UI_Component> _uicomponents;
        public struct CombatInfo
        {
            public enum Battle_State
            {
                Win,
                Lose,
                Flee
            }

            public Battle_State BattleState;
            public int MetalCollected;
        }
        public ReviewState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Ship ship, CombatInfo info) : base(game, graphicsDevice, content)
        {
            Texture2D buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            SpriteFont buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);
            Texture2D pixelTexture = _content.Load<Texture2D>(Config.PIXEL_TEXTURE);
            Ship = ship;

            Button BuildPhaseButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.REVIEW_BUILD_BUTTON
            };
            BuildPhaseButton.Click += BuildPhaseButton_Click;

            ReviewStateStats stats = new ReviewStateStats(_content);
            stats.SetValues(info);

            _uicomponents = new List<UI_Component>()
            {
                BuildPhaseButton,
                stats
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            foreach (UI_Component component in _uicomponents)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new PauseState(_game, _graphicsDevice, _content, this));

            foreach (UI_Component component in _uicomponents)
            {
                component.Update(gameTime);
            }
        }

        public void BuildPhaseButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new BuildState(_game, _graphicsDevice, _content, Ship));
        }
    }
}
