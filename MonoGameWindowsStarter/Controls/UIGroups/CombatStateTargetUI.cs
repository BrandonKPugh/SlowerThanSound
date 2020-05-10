using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls.UIGroups
{
    public class CombatStateTargetUI : UIGroup
    {
        private BorderBox _gridBox;
        public CombatStateTargetUI(ContentManager _content)
        {
            Texture2D buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            SpriteFont buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);
            Texture2D pixelTexture = _content.Load<Texture2D>(Config.PIXEL_TEXTURE);

            BorderBox CanvasBox = new BorderBox(pixelTexture)
            {
                BorderBoxInfo = ControlConstants.COMBATMODE_CANVAS
            };

            Button BuildModeButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.COMBATMODE_BUILDMODE,
            };

            ProgressBarButton TargetEnemyShipButton = new ProgressBarButton(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.COMBATMODE_TARGETENEMYSHIP,
            };

            ProgressBarButton TargetStoragesButton = new ProgressBarButton(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.COMBATMODE_TARGETSTORAGES,
            };

            ProgressBarButton TargetWeaponsButton = new ProgressBarButton(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.COMBATMODE_TARGETWEAPONS,
            };

            ProgressBarButton TargetPowerGenButton = new ProgressBarButton(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.COMBATMODE_TARGETPOWERGEN,
            };

            ProgressBarButton TargetPowerStorageButton = new ProgressBarButton(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.COMBATMODE_TARGETPOWERSTORAGE,
            };

            TextBox CombatModeTitle = new TextBox(buttonFont)
            {
                TextBoxInfo = ControlConstants.COMBATMODE_TITLE,
            };

            _gridBox = new BorderBox(pixelTexture)
            {
                BorderBoxInfo = ControlConstants.BUILDMODE_GRIDBOX
            };

            UI_Components = new List<UI_Component>()
            {
                CanvasBox,
                BuildModeButton,
                CombatModeTitle,
                _gridBox,
                TargetEnemyShipButton,
                TargetStoragesButton,
                TargetWeaponsButton,
                TargetPowerGenButton,
                TargetPowerStorageButton
            };
        }

        public void SetShipGridLocation(Rectangle rect)
        {
            _gridBox.SetPosition(rect, ControlConstants.BUILDMODE_GRIDBOX.Padding);
        }

        public void SetProgressButtonValue(string text, float value)
        {
            foreach(UI_Component component in UI_Components)
            {
                if (component.GetType() == typeof(ProgressBarButton))
                {
                    // Set the Progress Bar's value
                    ProgressBarButton pbb = (ProgressBarButton)component;
                    if (pbb.Text == text)
                        pbb.Value = value;
                }
            }
        }
    }
}
