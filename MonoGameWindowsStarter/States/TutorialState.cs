using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Controls;
using MonoGameWindowsStarter.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.States
{
    public class TutorialState : State
    {
        private UIBox _primaryBox;
        private List<UI_Component> _uicomponents;
        public TutorialState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Texture2D tutorialTexture = _content.Load<Texture2D>(ControlConstants.TUTORIAL_TEXTURE);
            Texture2D buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            SpriteFont buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);

            _primaryBox = new UIBox(tutorialTexture)
            {
                UIBoxInfo = ControlConstants.TUTORIAL_TEXTURE_BOX
            };

            Button ExitButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.EXIT_TUTORIAL
            };

            ExitButton.Click += ExitButton_Click;

            _uicomponents = new List<UI_Component>()
            {
                _primaryBox,
                ExitButton
            };



        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
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
            foreach (UI_Component component in _uicomponents)
            {
                component.Update(gameTime);
            }
        }

        public void ExitButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MainMenuState(_game, _graphicsDevice, _content));
        }
    }
}
