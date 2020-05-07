using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Components;
using MonoGameWindowsStarter.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Controls
{
    public abstract class UIGroup : UI_Component
    {
        public List<UI_Component> UI_Components;

        public override Vector2 Position { get { return new Vector2(0); } }
        public override Vector2 Size { get { return new Vector2(0); } }

        public UIGroup()
        {
            UI_Components = new List<UI_Component>();
        }

        public void Add(UI_Component ui_component)
        {
            UI_Components.Add(ui_component);
        }

        public bool Remove(UI_Component ui_component)
        {
            if(UI_Components.Contains(ui_component))
            {
                UI_Components.Remove(ui_component);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach(UI_Component ui_component in UI_Components)
            {
                ui_component.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch  spriteBatch)
        {
            foreach(UI_Component ui_component in UI_Components)
            {
                ui_component.Draw(gameTime, spriteBatch);
            }
        }

        public bool InitializeButton(EventHandler clickEvent, string buttonText)
        {
            foreach(UI_Component component in UI_Components)
            {
                if(component.GetType() == typeof(Button))
                {
                    Button button = (Button)component;
                    if(button.Text == buttonText)
                    {
                        button.Click += clickEvent;
                        button.Click += SetSelectedButton;
                        return true;
                    }
                }
            }
            return false;
        }

        private void SetSelectedButton(object sender, EventArgs e)
        {
            foreach (UI_Component component in UI_Components)
            {
                if (component.GetType() == typeof(Button))
                {
                    ((Button)component).BackColour = ControlConstants.BUTTON_BACKCOLOR;
                }
            }
            ((Button)sender).BackColour = ControlConstants.BUTTON_SELECTED;
        }
    }
}
