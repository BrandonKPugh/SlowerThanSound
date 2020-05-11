using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls.UIGroups
{
    public class BuildStateComponentUI : UIGroup
    {
        public BuildStateComponentUI(ContentManager _content)
        {
            Texture2D buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            SpriteFont buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);
            Texture2D pixelTexture = _content.Load<Texture2D>(Config.PIXEL_TEXTURE);

            Button DeleteComponents = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.DELETE_COMPONENT,
            };

            Button PlaceStorage = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.PLACE_COMPONENT_STORAGE,
            };

            Button PlaceWeapon = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.PLACE_COMPONENT_WEAPON,
            };

            Button PlaceGenerator = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.PLACE_COMPONENT_GENERATOR,
            };

            Button PlaceBattery = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.PLACE_COMPONENT_BATTERY,
            };

            UI_Components = new List<UI_Component>()
            {
                DeleteComponents,
                PlaceStorage,
                PlaceWeapon,
                PlaceGenerator,
                PlaceBattery,
            };
        }
    }
}
