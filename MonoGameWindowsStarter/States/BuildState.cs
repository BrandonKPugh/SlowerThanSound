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
using MonoGameWindowsStarter.Rendering;

namespace MonoGameWindowsStarter.States
{
    class BuildState : State
    {
        public enum Placement_Type
        {
            // Don't change the order, always A then placingA
            None,
            //Room,
            DeleteComponent,
            //DeleteRoom,
            //PlacingRoom,
            Weapon,
            PlacingWeapon,
            Storage,
            PlacingStorage,
            Generator,
            PlacingGenerator,
            Battery,
            PlacingBattery
        }
        private enum Tab_State
        {
            None,
            Ship,
            Component,
            Room
        }
        private enum Room_State
        {
            None,
            Room,
            PlacingRoom,
            DeleteRoom
        }
        public Ship Ship;
        private List<UI_Component> _uicomponents;
        private UIGroup _activeCanvas;
        private Tab_State _tabState = Tab_State.None;
        private Placement_Type _placementType = Placement_Type.None;
        private Room _temporaryRoom = null;
        private Point _temporaryRoomStart;
        private Component _temporaryComponent = null;
        private bool _drawTemporaryComponent = false;
        private Room_State roomState = Room_State.None;
        private TextBox _metalAmount;
        private Tooltip _tooltip;
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

            TextBox MetalAmountText = new TextBox(buttonFont)
            {
                TextBoxInfo = ControlConstants.PRIMARY_TEXTBOX
            };

            _metalAmount = new TextBox(buttonFont)
            {
                TextBoxInfo = ControlConstants.PRIMARY_TEXTBOX_VALUE
            };

            Button RoomButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.BUILDMODE_ROOM,
            };

            RoomButton.Click += RoomButton_Click;

            Button ComponentBuildButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.BUILDMODE_COMPONENTBUILD,
            };

            ComponentBuildButton.Click += ComponentBuildButton_Click;

            Button ShipButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.BUILDMODE_SHIP,
            };

            ShipButton.Click += ShipBuildButton_Click;

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

            _tooltip = new Tooltip(pixelTexture, buttonFont);

            _uicomponents = new List<UI_Component>()
            {
                CombatModeButton,
                RoomButton,
                ComponentBuildButton,
                ShipButton,
                BuildModeTitle,
                GridBox,
                Canvas,
                MetalAmountText,
                _metalAmount,
                //BuildModeTitleBox
            };

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

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
            if(_temporaryComponent != null && _drawTemporaryComponent)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
                _temporaryComponent.Draw(spriteBatch, Ship.Grid.Info);
                spriteBatch.End();
            }
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            _tooltip.Draw(gameTime, spriteBatch);
            spriteBatch.End();
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
            Ship.Update(gameTime);
            _metalAmount.Text = Ship.Material.ToString();

            switch (_tabState)
            {
                case Tab_State.None:
                    {
                        break;
                    }
                case Tab_State.Component:
                    {
                        switch (_placementType)
                        {
                            case Placement_Type.None:
                                {
                                    break;
                                }
                            case Placement_Type.Battery:
                            case Placement_Type.Storage:
                            case Placement_Type.Generator:
                            // Drops through to Weapon
                            case Placement_Type.Weapon:
                                {
                                    _tooltip.Show = false;
                                    if (mouseOnTile)
                                    {
                                        foreach (Room room in Ship.Rooms)
                                        {
                                            if (room.InteriorContains(tileUnderMouse))
                                            {
                                                if (Component.RoomTypeMatches(_placementType, room.RoomType) || room.RoomType == Room.Room_Type.None)
                                                {
                                                    _tooltip.Show = true;
                                                    _tooltip.SetText("Cost: " + Component.GetBaseValue(_placementType));
                                                }
                                            }
                                        }
                                    }
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
                                                    if (Component.RoomTypeMatches(_placementType, room.RoomType) || room.RoomType == Room.Room_Type.None)
                                                    {
                                                        switch (_placementType)
                                                        {
                                                            case Placement_Type.Storage:
                                                                {
                                                                    _temporaryComponent = new MaterialStorageComponent(tileUnderMouse.X, tileUnderMouse.Y, ComponentConstants.COMPONENT_MATERIALSTORAGE_COLOR);
                                                                    Ship.LoadComponentTexture(_temporaryComponent);
                                                                    _drawTemporaryComponent = true;
                                                                    _placementType++;
                                                                    break;
                                                                }
                                                            case Placement_Type.Generator:
                                                                {
                                                                    _temporaryComponent = new PowerGenerationComponent(tileUnderMouse.X, tileUnderMouse.Y, ComponentConstants.COMPONENT_POWERGENERATOR_COLOR);
                                                                    Ship.LoadComponentTexture(_temporaryComponent);
                                                                    _drawTemporaryComponent = true;
                                                                    _placementType++;
                                                                    break;
                                                                }
                                                            case Placement_Type.Weapon:
                                                                {
                                                                    _temporaryComponent = new WeaponComponent(tileUnderMouse.X, tileUnderMouse.Y, ComponentConstants.COMPONENT_WEAPON_COLOR);
                                                                    Ship.LoadComponentTexture(_temporaryComponent);
                                                                    _drawTemporaryComponent = true;
                                                                    _placementType++;
                                                                    break;
                                                                }
                                                            case Placement_Type.Battery:
                                                                {
                                                                    _temporaryComponent = new PowerStorageComponent(tileUnderMouse.X, tileUnderMouse.Y, ComponentConstants.COMPONENT_POWERSTORAGE_COLOR);
                                                                    Ship.LoadComponentTexture(_temporaryComponent);
                                                                    _drawTemporaryComponent = true;
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
                                                        Ship.Material += c.value / 2;
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
                            case Placement_Type.PlacingBattery:
                            case Placement_Type.PlacingStorage:
                            case Placement_Type.PlacingGenerator:
                            // Drops through to PlacingWeapon
                            case Placement_Type.PlacingWeapon:
                                {
                                    if (mousePressed && mouseOnTile)
                                    {
                                        foreach (Room room in Ship.Rooms)
                                        {
                                            if (room.Contains(_temporaryComponent.TilePosition) && !room.InteriorContains(tileUnderMouse))
                                            {
                                                _drawTemporaryComponent = false;
                                                _tooltip.Show = false;
                                            }
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
                                                    _drawTemporaryComponent = true;
                                                    _tooltip.Show = true;
                                                    _tooltip.SetText("Cost: " + _temporaryComponent.getValue().ToString());
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    else if (mousePressed && !mouseOnTile)
                                    {
                                        _drawTemporaryComponent = false;
                                        _tooltip.Show = false;
                                    }
                                    else if (!mousePressed)
                                    {
                                        if (Ship.Material - _temporaryComponent.value >= 0) 
                                        {
                                            if (mouseOnTile && (tileUnderMouse == _temporaryComponent.TilePosition))
                                            {
                                                Ship.Material -= _temporaryComponent.value;
                                                Ship.AddComponent(_temporaryComponent);
                                            }
                                            else
                                            {
                                                foreach (Room room in Ship.Rooms)
                                                {
                                                    if (room.InteriorContains(tileUnderMouse) && room.InteriorContains(_temporaryComponent.TilePosition))
                                                    {
                                                        Ship.Material -= _temporaryComponent.value;
                                                        Ship.AddComponent(_temporaryComponent);
                                                    }
                                                }
                                            }
                                        }
                                        _temporaryComponent = null;
                                        _drawTemporaryComponent = false;
                                        _tooltip.Show = false;

                                        _placementType--;
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case Tab_State.Room:
                    {
                        switch (roomState)
                        {
                            case Room_State.None:
                                break;
                            case Room_State.Room:
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
                                                    if (found != null && found.ComponentType == Component.Component_Type.Structure)
                                                    {
                                                        beginPlacement = true;
                                                    }
                                                }
                                            }
                                            if (foundRoom == false)
                                            {
                                                beginPlacement = true;
                                            }
                                            if (beginPlacement)
                                            {
                                                _temporaryRoomStart = tileUnderMouse;
                                                _temporaryRoom = new Room(Ship, Ship.Grid, tileUnderMouse, tileUnderMouse, Room.Room_Type.None);
                                                foreach (Component component in _temporaryRoom.GetComponents())
                                                {
                                                    Ship.LoadComponentTexture(component);
                                                }
                                                roomState = Room_State.PlacingRoom;
                                            }
                                        }
                                    }
                                    break;
                                }
                            case Room_State.PlacingRoom:
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
                                        _tooltip.Show = true;
                                        _tooltip.SetText("Cost: " + _temporaryRoom.GetValue().ToString());
                                    }
                                    else if (!mousePressed && mouseOnTile)
                                    {
                                        if (Ship.Material - _temporaryRoom.GetValue() >= 0)
                                        {
                                            // Released mouse, finalize room
                                            Ship.Material -= _temporaryRoom.GetValue();
                                            Ship.AddRoom(_temporaryRoom);
                                        }
                                        roomState = Room_State.Room;
                                        _temporaryRoom = null;
                                        _tooltip.Show = false;
                                    }
                                    break;
                                }
                            case Room_State.DeleteRoom:
                                {
                                    _tooltip.Show = false;
                                    if(mouseOnTile)
                                    {
                                        foreach (Room room in Ship.Rooms)
                                        {
                                            if (room.InteriorContains(tileUnderMouse))
                                            {
                                                _tooltip.Show = true;
                                                _tooltip.SetText("Refund: " + room.GetValue() / 2);
                                                break;
                                            }
                                        }
                                    }
                                    if (mousePressed && mouseOnTile)
                                    {
                                        foreach (Room room in Ship.Rooms)
                                        {
                                            if (room.InteriorContains(tileUnderMouse))
                                            {
                                                Ship.Material += room.GetValue() / 2;
                                                Ship.RemoveRoom(room);
                                                _tooltip.Show = false;
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                }
                            default:
                                break;
                        }
                        break;
                    }
                case Tab_State.Ship:
                    {
                        if (mousePressed && mouseOnTile)
                        {
                            Room room = Ship.GetRoom(tileUnderMouse, true);
                            ((BuildStateShipUI)_activeCanvas).SelectedRoom = room;
                        }
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }


            foreach (var component in _uicomponents)
                component.Update(gameTime);
            _tooltip.Update(gameTime);

            if (_activeCanvas != null)
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
            _drawTemporaryComponent = false;
            _tabState = Tab_State.Ship;
            roomState = Room_State.None;
            SetActiveButton(ControlConstants.BUILDMODE_SHIP.Text);
        }
        private void ComponentBuildButton_Click(object sender, EventArgs e)
        {
            BuildStateComponentUI componentCanvas = new BuildStateComponentUI(_content);
            componentCanvas.InitializeButton(PlaceWeaponButton_Click, ControlConstants.PLACE_COMPONENT_WEAPON.Text);
            componentCanvas.InitializeButton(PlaceStorageButton_Click, ControlConstants.PLACE_COMPONENT_STORAGE.Text);
            componentCanvas.InitializeButton(PlaceGeneratorButton_Click, ControlConstants.PLACE_COMPONENT_GENERATOR.Text);
            componentCanvas.InitializeButton(PlaceBatteryButton_Click, ControlConstants.PLACE_COMPONENT_BATTERY.Text);
            componentCanvas.InitializeButton(DeleteComponentButton_Click, ControlConstants.DELETE_COMPONENT.Text);
            _activeCanvas = componentCanvas;
            _tabState = Tab_State.Component;
            SetActiveButton(ControlConstants.BUILDMODE_COMPONENTBUILD.Text);
        }
        private void RoomButton_Click(object sender, EventArgs e)
        {
            BuildStateRoomsUI componentCanvas = new BuildStateRoomsUI(_content);
            componentCanvas.InitializeButton(DeleteRoomButton_Click, ControlConstants.DELETE_ROOM.Text);
            componentCanvas.InitializeButton(CreateRoomButton_Click, ControlConstants.CREATE_ROOM.Text);
            _activeCanvas = componentCanvas;
            _placementType = Placement_Type.None;
            _temporaryRoom = null;
            _temporaryComponent = null;
            _drawTemporaryComponent = false;
            _tabState = Tab_State.Room;
            roomState = Room_State.None;
            SetActiveButton(ControlConstants.BUILDMODE_ROOM.Text);
        }

        private void DeleteComponentButton_Click(object sender, EventArgs e)
        {
            _placementType = Placement_Type.DeleteComponent;
        }
        private void DeleteRoomButton_Click(object sender, EventArgs e)
        {
            roomState = Room_State.DeleteRoom;
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
        private void PlaceBatteryButton_Click(object sender, EventArgs e)
        {
            _placementType = Placement_Type.Battery;
        }
        private void CreateRoomButton_Click(object sender, EventArgs e)
        {
            roomState = Room_State.Room;

        }

        private void SetActiveButton(string text)
        {
            foreach (UI_Component component in _uicomponents)
            {
                if (component.GetType() == typeof(Button))
                {
                    if (((Button)component).Text == text)
                    {
                        ((Button)component).BackColour = ControlConstants.BUTTON_SELECTED;
                    }
                    else
                    {
                        ((Button)component).BackColour = ControlConstants.BUTTON_BACKCOLOR;
                    }
                }
            }
        }
    }
}
