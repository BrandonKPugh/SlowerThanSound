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

        Texture2D tutorialTexture1;
        Texture2D tutorialTexture2;
        Texture2D tutorialTexture3;
        Texture2D[] textures;
        int currentTexture = 0;
        public TutorialState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            tutorialTexture1 = _content.Load<Texture2D>(ControlConstants.TUTORIAL_TEXTURE1);
            tutorialTexture2 = _content.Load<Texture2D>(ControlConstants.TUTORIAL_TEXTURE3);
            tutorialTexture3 = _content.Load<Texture2D>(ControlConstants.TUTORIAL_TEXTURE2);
            textures = new Texture2D[3];
            textures[0] = tutorialTexture1;
            textures[1] = tutorialTexture2;
            textures[2] = tutorialTexture3;
            Texture2D buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            SpriteFont buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);

            _primaryBox = new UIBox(tutorialTexture1)
            {
                UIBoxInfo = ControlConstants.TUTORIAL_TEXTURE_BOX
            };

            Button ExitButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.EXIT_TUTORIAL
            };

            ExitButton.Click += ExitButton_Click;

            Button NextButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.NEXT_TUTORIAL
            };

            NextButton.Click += NextButton_Click;

            Button PreviousButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.PREVIOUS_TUTORIAL
            };

            PreviousButton.Click += PreviousButton_Click;

            _uicomponents = new List<UI_Component>()
            {
                _primaryBox,
                ExitButton,
                NextButton,
                PreviousButton
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
        public void NextButton_Click(object sender, EventArgs e)
        {
            if(currentTexture+1 < textures.Length)
            {
                currentTexture++;
                ((UIBox)_uicomponents[0]).setTexture(textures[currentTexture]);
            }
        }
        public void PreviousButton_Click(object sender, EventArgs e)
        {
            if (currentTexture - 1 >= 0)
            {
                currentTexture--;
                ((UIBox)_uicomponents[0]).setTexture(textures[currentTexture]);
            }
        }
    }
}
