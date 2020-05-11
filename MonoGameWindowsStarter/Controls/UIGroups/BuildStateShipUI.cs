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
    public class BuildStateShipUI : UIGroup
    {
        public Room SelectedRoom;
        private Room _oldSelectedRoom;
        private TextBox _roomInfo;
        private UIBox _roomOutline;
        public BuildStateShipUI(ContentManager _content)
        {
            Texture2D buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            SpriteFont buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);
            Texture2D pixelTexture = _content.Load<Texture2D>(Config.PIXEL_TEXTURE);

            _roomInfo = new TextBox(buttonFont)
            {
                TextBoxInfo = ControlConstants.ROOM_INFO_TEXTBOX
            };

            _roomOutline = new UIBox(pixelTexture)
            {
                UIBoxInfo = ControlConstants.ROOM_INFO_BOX
            };

            UI_Components = new List<UI_Component>()
            {
                _roomInfo,
                _roomOutline
            };
        }

        public override void Update(GameTime gameTime)
        {
            if(SelectedRoom != null && (_oldSelectedRoom == null || SelectedRoom.RoomID != _oldSelectedRoom.RoomID))
            {
                _oldSelectedRoom = SelectedRoom;
                _roomInfo.Text = SelectedRoom.GetInfo();
                _roomOutline.SetPosition(SelectedRoom.GetInteriorArea());
                _roomOutline.Color = new Color(SelectedRoom.GetColor(), ControlConstants.ROOM_INFO_BOX_ALPHA);
            }

            foreach (UI_Component ui_component in UI_Components)
            {
                ui_component.Update(gameTime);
            }
        }
    }
}
