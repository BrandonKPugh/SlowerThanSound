using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls.UIGroups
{
    public class BuildStateRoomsUI : UIGroup
    {
        public BuildStateRoomsUI(ContentManager _content)
        {
            Texture2D buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            SpriteFont buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);
            Texture2D pixelTexture = _content.Load<Texture2D>(Config.PIXEL_TEXTURE);


            Button DeleteRoom = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.DELETE_ROOM,
            };

            Button CreateRoom = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.CREATE_ROOM,
            };

            UI_Components = new List<UI_Component>()
            {
                DeleteRoom,
                CreateRoom
            };
        }
    }
}
