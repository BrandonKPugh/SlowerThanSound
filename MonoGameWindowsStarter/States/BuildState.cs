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
            // Don't change the order, always A then placingA
            None,
            Room,
            DeleteComponent,
            DeleteRoom,
            PlacingRoom,
            Weapon,
            PlacingWeapon,
            Storage,
            PlacingStorage,
            Generator,
            PlacingGenerator
        }
        public Ship Ship;
        private List<UI_Component> _uicomponents;
        private UIGroup _activeCanvas;
        private Placement_Type _placementType = Placement_Type.None;
        private Room _temporaryRoom = null;
        private Point _temporaryRoomStart;
        private Component _temporaryComponent = null;
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
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            foreach (var component in _uicomponents)
                component.Draw(gameTime, spriteBatch);

            if(_activeCanvas != null)
                _activeCanvas.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            Ship.Draw(spriteBatch, ModeState.State.Build);

            if(_temporaryRoom != null)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
                _temporaryRoom.Draw(spriteBatch, Ship.Grid.Info);
                spriteBatch.End();
            }
            if(_temporaryComponent != null)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
                _temporaryComponent.Draw(spriteBatch, Ship.Grid.Info);
                spriteBatch.End();
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new PauseState(_game, _graphicsDevice, _content, this));

            int x = Mouse.GetState().X;
            int y = Mouse.GetState().Y;
            bool mousePressed = (Mouse.GetState().LeftButton == ButtonState.Pressed);
            bool mouseOnTile = Ship.Grid.PixelToTile(x, y, out int tileX, out int tileY);
            Point tileUnderMouse = new Point(tileX, tileY);

            switch (_placementType)
            {
                case Placement_Type.None:
                    {
                        break;
                    }
                case Placement_Type.Room:
                    {
                        if (mousePressed)
                        {
                            bool beginPlacement = false;
                            bool foundRoom = false;
                            if (mouseOnTile)
                            {
                                foreach (Room room in Ship.Rooms)
                                {
                                    if (room.Contains(tileUnderMouse))
                                    {
                                        foundRoom = true;
                                        Component found = null;
                                        foreach (Component c in room.GetComponents())
                                        {
                                            if (c.TilePosition == tileUnderMouse)
                                            {
                                                found = c;
                                                break;
                                            }
                                        }
                                        if(found != null && found.ComponentType == Component.Component_Type.Structure)
                                        {
                                            beginPlacement = true;
                                        }
                                    }
                                }
                                if (foundRoom == false)
                                {
                                    beginPlacement = true;
                                }
                                if(beginPlacement)
                                {
                                    _temporaryRoomStart = tileUnderMouse;
                                    _temporaryRoom = new Room(Ship, Ship.Grid, tileUnderMouse, tileUnderMouse, Room.Room_Type.None);
                                    foreach (Component component in _temporaryRoom.GetComponents())
                                    {
                                        Ship.LoadComponentTexture(component);
                                    }
                                    _placementType = Placement_Type.PlacingRoom;
                                }
                            }
                        }
                        break;
                    }
                case Placement_Type.PlacingRoom:
                    {
                        if (mousePressed && mouseOnTile)
                        {
                            var minX = Math.Min(tileX, _temporaryRoomStart.X);
                            var minY = Math.Min(tileY, _temporaryRoomStart.Y);
                            var maxX = Math.Max(tileX, _temporaryRoomStart.X);
                            var maxY = Math.Max(tileY, _temporaryRoomStart.Y);
                            Point p1 = new Point(minX, minY);
                            Point p2 = new Point(maxX, maxY);

                            _temporaryRoom = new Room(Ship, Ship.Grid, p1, p2, Room.Room_Type.None);
                            foreach (Component component in _temporaryRoom.GetComponents())
                            {
                                Ship.LoadComponentTexture(component);
                            }
                        }
                        else if(!mousePressed && mouseOnTile)
                        {
                            // Released mouse, finalize room
                            _placementType = Placement_Type.Room;
                            Ship.AddRoom(_temporaryRoom);
                            _temporaryRoom = null;
                        }
                        break;
                    }
                case Placement_Type.Storage:
                case Placement_Type.Generator:
                // Drops through to Weapon
                case Placement_Type.Weapon:
                    {
                        if (mousePressed && mouseOnTile)
                        {
                            foreach (Room room in Ship.Rooms)
                            {
                                if (room.Contains(tileUnderMouse))
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
                                        if(Component.RoomTypeMatches(_placementType, room.RoomType) || room.RoomType == Room.Room_Type.None)
                                        {
                                            switch (_placementType)
                                            {
                                                case Placement_Type.Storage:
                                                    {
                                                        _temporaryComponent = new MaterialStorageComponent(tileUnderMouse.X, tileUnderMouse.Y, ComponentConstants.COMPONENT_MATERIALSTORAGE_COLOR);
                                                        Ship.LoadComponentTexture(_temporaryComponent);
                                                        _placementType++;
                                                        break;
                                                    }
                                                case Placement_Type.Generator:
                                                    {
                                                        _temporaryComponent = new PowerGenerationComponent(tileUnderMouse.X, tileUnderMouse.Y, ComponentConstants.COMPONENT_POWERGENERATOR_COLOR);
                                                        Ship.LoadComponentTexture(_temporaryComponent);
                                                        _placementType++;
                                                        break;
                                                    }
                                                case Placement_Type.Weapon:
                                                    {
                                                        _temporaryComponent = new WeaponComponent(tileUnderMouse.X, tileUnderMouse.Y, ComponentConstants.COMPONENT_WEAPON_COLOR);
                                                        Ship.LoadComponentTexture(_temporaryComponent);
                                                        _placementType++;
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        throw new NotImplementedException("That component type doesn't exist!");
                                                    }
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case Placement_Type.DeleteComponent:
                    {
                        if (mousePressed && mouseOnTile)
                        {
                            foreach (Room room in Ship.Rooms)
                            {
                                if (room.Contains(tileUnderMouse))
                                {
                                    foreach (Component c in room.GetComponents())
                                    {
                                        if (c.X == tileX && c.Y == tileY && c.ComponentType != Component.Component_Type.Structure)
                                        {
                                            room.RemoveComponent(c);
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case Placement_Type.DeleteRoom:
                    {
                        if (mousePressed && mouseOnTile)
                        {
                            foreach (Room room in Ship.Rooms)
                            {
                                if (room.InteriorContains(tileUnderMouse))
                                {
                                    Ship.RemoveRoom(room);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case Placement_Type.PlacingStorage:
                case Placement_Type.PlacingGenerator:
                // Drops through to PlacingWeapon
                case Placement_Type.PlacingWeapon:
                    {
                        if (mousePressed && mouseOnTile)
                        {
                            foreach (Room room in Ship.Rooms)
                            {
                                if (room.Contains(tileUnderMouse) && room.Contains(_temporaryComponent.TilePosition))
                                {
                                    Component found = null;
                                    foreach (Component c in room.GetComponents())
                                    {
                                        if (c.TilePosition == tileUnderMouse)
                                        {
                                            found = c;
                                            break;
                                        }
                                    }
                                    // Another component isn't under the mouse already
                                    if (found == null)
                                    {
                                        _temporaryComponent.TilePosition = tileUnderMouse;
                                    }
                                    break;
                                }
                            }
                        }
                        else if(!mousePressed)
                        {
                            if (mouseOnTile && (tileUnderMouse == _temporaryComponent.TilePosition))
                            {
                                Ship.AddComponent(_temporaryComponent);
                            }
                            else 
                            {
                                foreach (Room room in Ship.Rooms)
                                {
                                    if (room.Contains(tileUnderMouse) && room.Contains(_temporaryComponent.TilePosition))
                                    {
                                        Ship.AddComponent(_temporaryComponent);
                                    }
                                }
                            }
                            _temporaryComponent = null;

                            _placementType--;
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
            _placementType = Placement_Type.None;
            _temporaryRoom = null;
            _temporaryComponent = null;
        }
        private void ComponentBuildButton_Click(object sender, EventArgs e)
        {
            BuildStateComponentUI componentCanvas = new BuildStateComponentUI(_content);
            componentCanvas.InitializeButton(PlaceWeaponButton_Click, ControlConstants.PLACE_COMPONENT_WEAPON.Text);
            componentCanvas.InitializeButton(PlaceStorageButton_Click, ControlConstants.PLACE_COMPONENT_STORAGE.Text);
            componentCanvas.InitializeButton(PlaceGeneratorButton_Click, ControlConstants.PLACE_COMPONENT_GENERATOR.Text);
            componentCanvas.InitializeButton(CreateRoomButton_Click, ControlConstants.CREATE_ROOM.Text);
            componentCanvas.InitializeButton(DeleteComponentButton_Click, ControlConstants.DELETE_COMPONENT.Text);
            componentCanvas.InitializeButton(DeleteRoomButton_Click, ControlConstants.DELETE_ROOM.Text);
            _activeCanvas = componentCanvas;
        }
        private void ResearchButton_Click(object sender, EventArgs e)
        {
            BuildStateResearchUI componentCanvas = new BuildStateResearchUI(_content);
            _activeCanvas = componentCanvas;
            _placementType = Placement_Type.None;
            _temporaryRoom = null;
            _temporaryComponent = null;
        }

        private void DeleteComponentButton_Click(object sender, EventArgs e)
        {
            _placementType = Placement_Type.DeleteComponent;
        }
        private void DeleteRoomButton_Click(object sender, EventArgs e)
        {
            _placementType = Placement_Type.DeleteRoom;
        }

        private void PlaceWeaponButton_Click(object sender, EventArgs e)
        {
            _placementType = Placement_Type.Weapon;
        }

        private void PlaceStorageButton_Click(object sender, EventArgs e)
        {
            _placementType = Placement_Type.Storage;

        }
        private void PlaceGeneratorButton_Click(object sender, EventArgs e)
        {
            _placementType = Placement_Type.Generator;

        }
        private void CreateRoomButton_Click(object sender, EventArgs e)
        {
            _placementType = Placement_Type.Room;

        }
    }
}
