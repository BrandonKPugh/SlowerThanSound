using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameWindowsStarter.Components;
using MonoGameWindowsStarter.Spaceship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameWindowsStarter.Controls;
using MonoGameWindowsStarter.Controls.UIGroups;
using System.ComponentModel.Design;

namespace MonoGameWindowsStarter.States
{
    class BuildState : State
    {
        public enum Placement_Type
        {
            None,
            Room,
            Weapon,
            Storage
        }
        public Ship Ship;
        private List<UI_Component> _uicomponents;
        private UIGroup _activeCanvas;
        private Placement_Type _placementType = Placement_Type.None;
        public BuildState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Ship ship) : base(game, graphicsDevice, content)
        {
            this.Ship = ship;
            Texture2D buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            SpriteFont buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);
            Texture2D pixelTexture = _content.Load<Texture2D>(Config.PIXEL_TEXTURE);

            Button CombatModeButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.BUILDMODE_COMBATMODE,
            };

            CombatModeButton.Click += CombatModeButton_Click;

            Button ShipBuildButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.BUILDMODE_SHIPBUILD,
            };

            ShipBuildButton.Click += ShipBuildButton_Click;

            Button ComponentBuildButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.BUILDMODE_COMPONENTBUILD,
            };

            ComponentBuildButton.Click += ComponentBuildButton_Click;

            Button ResearchButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.BUILDMODE_RESEARCH,
            };

            ResearchButton.Click += ResearchButton_Click;

            TextBox BuildModeTitle = new TextBox(buttonFont)
            {
                TextBoxInfo = ControlConstants.BUILDMODE_TITLE,
            };

            BorderBox GridBox = new BorderBox(pixelTexture)
            {
                BorderBoxInfo = ControlConstants.BUILDMODE_GRIDBOX
            };

            GridBox.SetPosition(Ship.Grid.Info.GridRectangle, ControlConstants.BUILDMODE_GRIDBOX.Padding);

            BorderBox Canvas = new BorderBox(pixelTexture)
            {
                BorderBoxInfo = ControlConstants.BUILDMODE_CANVAS
            };

            BorderBox BuildModeTitleBox = new BorderBox(pixelTexture)
            {
                BorderBoxInfo = ControlConstants.BUILDMODE_TITLEBOX
            };

            BuildModeTitleBox.SetPosition(BuildModeTitle.Location, 0);

            _uicomponents = new List<UI_Component>()
            {
                CombatModeButton,
                ShipBuildButton,
                ComponentBuildButton,
                ResearchButton,
                BuildModeTitle,
                GridBox,
                Canvas,
                BuildModeTitleBox
            };

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _uicomponents)
                component.Draw(gameTime, spriteBatch);

            if(_activeCanvas != null)
                _activeCanvas.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            Ship.Draw(spriteBatch, ModeState.State.Build);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new PauseState(_game, _graphicsDevice, _content, this));

            switch(_placementType)
            {
                case Placement_Type.None:
                    {
                        break;
                    }
                case Placement_Type.Room:
                    {
                        break;
                    }
                case Placement_Type.Storage:
                case Placement_Type.Weapon:
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            int x = Mouse.GetState().X;
                            int y = Mouse.GetState().Y;

                            if (Ship.Grid.PixelToTile(x, y, out int tileX, out int tileY))
                            {
                                foreach (Room room in Ship.Rooms)
                                {
                                    if (room.Contains(tileX, tileY))
                                    {
                                        Component found = null;
                                        foreach (Component c in room.GetComponents())
                                        {
                                            if (c.X == tileX && c.Y == tileY)
                                            {
                                                found = c;
                                                break;
                                            }
                                        }
                                        if (found == null)
                                        {
                                            if(Component.RoomTypeMatches(_placementType, room.RoomType))
                                            {
                                                Component newComponent;
                                                switch (_placementType)
                                                {
                                                    case Placement_Type.Storage:
                                                        {
                                                            newComponent = new MaterialStorageComponent(tileX, tileY, ComponentConstants.COMPONENT_MATERIALSTORAGE_COLOR);
                                                            break;
                                                        }
                                                    case Placement_Type.Weapon:
                                                        {
                                                            newComponent = new WeaponComponent(tileX, tileY, ComponentConstants.COMPONENT_WEAPON_COLOR);
                                                            break;
                                                        }
                                                    default:
                                                        {
                                                            throw new NotImplementedException("That component type doesn't exist!");
                                                        }
                                                }
                                                room.AddComponent(newComponent);
                                            }
                                        }
                                        break;
                                    }
                                }


                            }
                        }
                        break;
                    }
            }

            foreach (var component in _uicomponents)
                component.Update(gameTime);

            if(_activeCanvas != null)
                _activeCanvas.Update(gameTime);
        }

        private void CombatModeButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new CombatState(_game, _graphicsDevice, _content, Ship));
        }

        private void ShipBuildButton_Click(object sender, EventArgs e)
        {
            BuildStateShipUI componentCanvas = new BuildStateShipUI(_content);
            _activeCanvas = componentCanvas;
        }
        private void ComponentBuildButton_Click(object sender, EventArgs e)
        {
            BuildStateComponentUI componentCanvas = new BuildStateComponentUI(_content);
            componentCanvas.InitializeButton(PlaceWeaponButton_Click, ControlConstants.PLACE_COMPONENT_WEAPON.Text);
            componentCanvas.InitializeButton(PlaceStorageButton_Click, ControlConstants.PLACE_COMPONENT_STORAGE.Text);
            componentCanvas.InitializeButton(CreateRoomButton_Click, ControlConstants.CREATE_ROOM.Text);
            _activeCanvas = componentCanvas;
        }
        private void ResearchButton_Click(object sender, EventArgs e)
        {
            BuildStateResearchUI componentCanvas = new BuildStateResearchUI(_content);
            _activeCanvas = componentCanvas;
        }

        private void PlaceWeaponButton_Click(object sender, EventArgs e)
        {
            _placementType = Placement_Type.Weapon;
        }

        private void PlaceStorageButton_Click(object sender, EventArgs e)
        {
            _placementType = Placement_Type.Storage;

        }

        private void CreateRoomButton_Click(object sender, EventArgs e)
        {
            _placementType = Placement_Type.Room;

        }
    }
}
