﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameWindowsStarter.States
{
    public class PauseState : State
    {
        private List<Controls.Button> _components;
        State state;

        public PauseState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, State state)
          : base(game, graphicsDevice, content)
        {
            this.state = state;
            var buttonTexture = _content.Load<Texture2D>("Button");
            var buttonFont = _content.Load<SpriteFont>("DebugFont");

            var resumeGameButton = new Controls.Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "Resume Game",
            };

            resumeGameButton.Click += ResumeGameButton_Click;

            var saveGameButton = new Controls.Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 250),
                Text = "Save Game",
            };

            saveGameButton.Click += SaveGameButton_Click;

            var loadGameButton = new Controls.Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 300),
                Text = "Load Game",
            };

            loadGameButton.Click += LoadGameButton_Click;

            var quitGameButton = new Controls.Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 350),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Controls.Button>()
      {
        resumeGameButton,
        saveGameButton,
        loadGameButton,
        quitGameButton,
      };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void SaveGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Load Game");
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Load Game");
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }
        private void ResumeGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(state);
        }
        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MainMenuState(_game, _graphicsDevice, _content));
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }

        
    }
}
