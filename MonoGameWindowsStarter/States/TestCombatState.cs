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
using MonoGameWindowsStarter.AI;

namespace MonoGameWindowsStarter.States
{
    public class TestCombatState : State
    {
        public Spaceship.Ship PlayerShip;
        public Spaceship.Ship EnemyShip;
        private EnemyAI enemy;

        public TestCombatState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Ship ship)
          : base(game, graphicsDevice, content)
        {
            this.PlayerShip = ship;
            enemy = new EnemyAI(EnemyShip, PlayerShip);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            PlayerShip.Draw(spriteBatch);
            EnemyShip.Draw(spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new PauseState(_game, _graphicsDevice, _content, this));

            enemy.Update(gameTime);
        }
    }
}