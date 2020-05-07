using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls.UIGroups
{
    public class BuildStateShipUI : UIGroup
    {
        public BuildStateShipUI(ContentManager _content)
        {
            Texture2D buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            SpriteFont buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);
            Texture2D pixelTexture = _content.Load<Texture2D>(Config.PIXEL_TEXTURE);

            Button TestButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.SHIPBUILD_TEST,
            };

            UI_Components = new List<UI_Component>()
            {
                TestButton
            };
        }
    }
}
