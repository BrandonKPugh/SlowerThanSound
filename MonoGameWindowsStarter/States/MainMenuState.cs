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
using MonoGameWindowsStarter.Rendering;
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

            var menuTextBox = new TextBox(buttonFont)
            {
                TextBoxInfo = ControlConstants.MAINMENU_TITLE,
            };

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.MAINMENU_NEWGAME,
            };

            newGameButton.Click += NewGameButton_Click;

            var tutorialButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.MAINMENU_TUTORIAL,
            };

            tutorialButton.Click += TutorialButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.MAINMENU_QUITGAME,
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<UI_Component>()
            {
                menuTextBox,
                newGameButton,
                tutorialButton,
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

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            ShipConstants.Initialize();

            Ship Ship = new Ship();
            Ship.Initialize(ShipConstants.ROOMPOINTS);

            Texture2D sheet = _content.Load<Texture2D>("STSsheet");
            SpriteSheet spriteSheet = new SpriteSheet(sheet, 32, 32);

            Texture2D tileTexture = _content.Load<Texture2D>("Tile");

            Dictionary<Component.Component_Type, Sprite> textures = new Dictionary<Component.Component_Type, Sprite>();
            textures.Add(Component.Component_Type.Power_Generation, spriteSheet[0]);
            textures.Add(Component.Component_Type.Power_Storage, spriteSheet[1]);
            textures.Add(Component.Component_Type.Weapon, spriteSheet[2]);
            textures.Add(Component.Component_Type.Material_Storage, spriteSheet[3]);
            textures.Add(Component.Component_Type.Structure, spriteSheet[4]);

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

        private void TutorialButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new TutorialState(_game, _graphicsDevice, _content));
        }
    }
}
