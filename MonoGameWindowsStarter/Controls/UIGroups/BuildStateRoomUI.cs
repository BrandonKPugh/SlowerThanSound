using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Spaceship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls.UIGroups
{
    public class BuildStateRoomUI : UIGroup
    {
        public Room SelectedRoom;
        private Room _oldSelectedRoom;
        private TextBox _roomInfo;
        public BuildStateRoomUI(ContentManager _content)
        {
            Texture2D buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            SpriteFont buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);
            Texture2D pixelTexture = _content.Load<Texture2D>(Config.PIXEL_TEXTURE);

            _roomInfo = new TextBox(buttonFont)
            {
                TextBoxInfo = ControlConstants.ROOM_INFO_TEXTBOX
            };

            UI_Components = new List<UI_Component>()
            {
                _roomInfo
            };
        }

        public override void Update(GameTime gameTime)
        {
            if(SelectedRoom != null && (_oldSelectedRoom == null || SelectedRoom.RoomID != _oldSelectedRoom.RoomID))
            {
                _oldSelectedRoom = SelectedRoom;
                _roomInfo.Text = SelectedRoom.GetInfo();
            }

            foreach (UI_Component ui_component in UI_Components)
            {
                ui_component.Update(gameTime);
            }
        }
    }
}
