using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Components;
using MonoGameWindowsStarter.Controls;
using MonoGameWindowsStarter.Spaceship;

namespace MonoGameWindowsStarter.States
{
    public class MainMenuState : State
    {
        private List<UI_Component> _components;

        public MainMenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            var buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);

            var menuTextBox = new TextBox("Slower Than Sound", buttonFont)
            {
                TextBoxInfo = ControlConstants.MAINMENU_TITLE,
            };

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.MAINMENU_NEWGAME,
            };

            newGameButton.Click += NewGameButton_Click;

            var loadGameButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.MAINMENU_LOADGAME,
            };

            loadGameButton.Click += LoadGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.MAINMENU_QUITGAME,
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<UI_Component>()
            {
                menuTextBox,
                newGameButton,
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

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Load Game");
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            ShipConstants.Initialize();

            Ship Ship = new Ship();
            Ship.Initialize(ShipConstants.COMPONENTS);

            Texture2D tileTexture = _content.Load<Texture2D>("Tile");

            Dictionary<Component.Component_Type, Texture2D> textures = new Dictionary<Component.Component_Type, Texture2D>();
            Texture2D weaponTexture = _content.Load<Texture2D>("Component_Weapon");
            textures.Add(Component.Component_Type.Weapon, weaponTexture);
            Texture2D structureTexture = _content.Load<Texture2D>("Structure");
            textures.Add(Component.Component_Type.Structure, structureTexture);

            //SpriteFont font = content.Load<SpriteFont>("DebugFont");
            //Texture2D pixel = content.Load<Texture2D>("pixel");

            Ship.LoadContent(textures, tileTexture);

            _game.ChangeState(new BuildState(_game, _graphicsDevice, _content, Ship));
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

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
    }
}
