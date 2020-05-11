using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls.UIGroups
{
    public class ReviewStateStats : UIGroup
    {
        private delegate void SetText(string text);
        private ContentManager _content;

        private SetText MetalCollected;
        public ReviewStateStats(ContentManager content)
        {
            this._content = content;

            Texture2D buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            SpriteFont buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);
            Texture2D pixelTexture = _content.Load<Texture2D>(Config.PIXEL_TEXTURE);

            TextBox MetalCollectedTextbox = new TextBox(buttonFont)
            {
                TextBoxInfo = ControlConstants.REVIEW_METAL_GATHERED_TEXT
            };

            TextBox MetalCollectedValue = new TextBox(buttonFont)
            {
                TextBoxInfo = ControlConstants.REVIEW_METAL_GATHERED_VALUE
            };
            MetalCollected = MetalCollectedValue.SetText;

            UI_Components = new List<UI_Component>()
            {
                MetalCollectedTextbox,
                MetalCollectedValue
            };
        }

        public void SetValues(ReviewState.CombatInfo info)
        {
            MetalCollected(info.MetalCollected.ToString());
        }
    }
}
