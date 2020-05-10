using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameWindowsStarter.Controls;
using MonoGameWindowsStarter.Components;
using MonoGameWindowsStarter.Spaceship;
using MonoGameWindowsStarter.AI;
using MonoGameWindowsStarter.Controls.UIGroups;

namespace MonoGameWindowsStarter.States
{
    public class CombatState : State
    {
        public Spaceship.Ship Ship;
        private List<UI_Component> _uicomponents;
        private EnemyAI enemyAI;
        private List<Projectile> projectiles;
        private List<Projectile> deadProjectiles;
        private CombatStateTargetUI _canvas;

        private Texture2D projectileTexture;
        private Texture2D _pixelTexture;

        private enum Clicked_State
        {
            None,
            SetAttack,
            Attacking,
            Repair
        }
        private Clicked_State clicked_State;

        Room attackingRoom;

        private Dictionary<uint, UIBox> _roomHealthBoxes = new Dictionary<uint, UIBox>();

        public CombatState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Ship ship)
          : base(game, graphicsDevice, content)
        {
            ShipConstants.Initialize();
            
            Ship = ship;
            //Ship.Initialize(ShipConstants.COMPONENTS);
            projectileTexture = _content.Load<Texture2D>("Pixel");

            Texture2D buttonTexture = _content.Load<Texture2D>(ControlConstants.BUTTON_TEXTURE);
            SpriteFont buttonFont = _content.Load<SpriteFont>(ControlConstants.BUTTON_FONT);
            Texture2D tileTexture = content.Load<Texture2D>("Tile");
            _pixelTexture = _content.Load<Texture2D>(Config.PIXEL_TEXTURE);

            Dictionary<Component.Component_Type, Texture2D> textures = new Dictionary<Component.Component_Type, Texture2D>();
            Texture2D weaponTexture = content.Load<Texture2D>("Component_Weapon");
            textures.Add(Component.Component_Type.Weapon, weaponTexture);
            Texture2D structureTexture = content.Load<Texture2D>("Structure");
            textures.Add(Component.Component_Type.Structure, structureTexture);

            _canvas = new CombatStateTargetUI(content);
            _canvas.SetShipGridLocation(Ship.Grid.Info.GridRectangle);
            _canvas.InitializeButton(TargetEnemyShipButton_Click, ControlConstants.COMBATMODE_TARGETENEMYSHIP.Text);
            _canvas.InitializeButton(TargetStoragesButton_Click, ControlConstants.COMBATMODE_TARGETSTORAGES.Text);
            _canvas.InitializeButton(TargetWeaponsButton_Click, ControlConstants.COMBATMODE_TARGETWEAPONS.Text);
            _canvas.InitializeButton(TargetPowerGenButton_Click, ControlConstants.COMBATMODE_TARGETPOWERGEN.Text);
            _canvas.InitializeButton(TargetPowerStorageButton_Click, ControlConstants.COMBATMODE_TARGETPOWERSTORAGE.Text);

            Button BuildModeButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.COMBATMODE_BUILDMODE
            };

            BuildModeButton.Click += BuildModeButton_Click;

            _uicomponents = new List<UI_Component>
            {
                BuildModeButton,
                _canvas
            };

            Ship.LoadContent(textures, tileTexture);
            this.Ship = ship;
            Ship.SetShipHealth();

            enemyAI = new EnemyAI(this.Ship, this);
            projectiles = new List<Projectile>();
            deadProjectiles = new List<Projectile>();
            clicked_State = Clicked_State.None;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Ship.Draw(spriteBatch, ModeState.State.Combat);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

            foreach (var component in _uicomponents)
                component.Draw(gameTime, spriteBatch);
            foreach (var projectile in projectiles)
                projectile.Draw(spriteBatch);
            foreach(UI_Component ui in _roomHealthBoxes.Values)
            {
                ui.Draw(gameTime, spriteBatch);
            }

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

            switch (clicked_State)
            {
                case (Clicked_State.None):
                    {

                        break;
                    }
                case (Clicked_State.SetAttack):
                    {
                        if (mousePressed & mouseOnTile)
                        {
                            foreach (Room room in Ship.Rooms)
                            {
                                if (room.Contains(tileUnderMouse) & room.RoomType == Room.Room_Type.Weapon)
                                {
                                    attackingRoom = room;
                                    clicked_State = Clicked_State.None;
                                }
                            }
                        }
                        break;
                    }
                case (Clicked_State.Repair):
                    {
                        if (mousePressed & mouseOnTile)
                        {
                            foreach (Room room in Ship.Rooms)
                            {
                                if (room.Contains(tileUnderMouse))
                                {
                                    room.Repair();
                                }
                            }
                        }
                        break;
                    }
            }

            _canvas.SetProgressButtonValue(ControlConstants.COMBATMODE_TARGETENEMYSHIP.Text, 0.55f);
            _canvas.SetProgressButtonValue(ControlConstants.COMBATMODE_TARGETWEAPONS.Text, 0.775f);
            _canvas.SetProgressButtonValue(ControlConstants.COMBATMODE_TARGETSTORAGES.Text, 0.65f);
            _canvas.SetProgressButtonValue(ControlConstants.COMBATMODE_TARGETPOWERGEN.Text, 0.45f);
            _canvas.SetProgressButtonValue(ControlConstants.COMBATMODE_TARGETPOWERSTORAGE.Text, 0.35f);

            foreach (var component in _uicomponents)
                component.Update(gameTime);

            enemyAI.Update(gameTime);

            foreach(var projectile in projectiles)
            {
                projectile.Update(gameTime);
                if (projectile.AtTarget == true)
                    deadProjectiles.Add(projectile);
            }
            foreach (var projectile in deadProjectiles)
            {
                projectiles.Remove(projectile);
            }
            foreach(Room room in Ship.Rooms)
            {
                if(_roomHealthBoxes.ContainsKey(room.RoomID))
                {
                    _roomHealthBoxes[room.RoomID].Color = new Color(ControlConstants.COMBATMODE_ROOMHEALTHCOLOR, room.GetHealthAlpha());
                }
                else
                {
                    UIBox toAdd = new UIBox(_pixelTexture);
                    toAdd.SetPosition(room.GetInteriorArea());
                    toAdd.Color = new Color(ControlConstants.COMBATMODE_ROOMHEALTHCOLOR, room.GetHealthAlpha());
                    _roomHealthBoxes.Add(room.RoomID, toAdd);
                }
            }
        }

        public void AddProjectile(Projectile projectile)
        {
            projectile._texture = projectileTexture;
            projectiles.Add(projectile);
        }
        private void BuildModeButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new BuildState(_game, _graphicsDevice, _content, Ship));
        }

        private void AttackButton_Click(object sender, EventArgs e)
        {
            clicked_State = Clicked_State.SetAttack;
        }

        private void RepairButton_Click(object sender, EventArgs e)
        {
            clicked_State = Clicked_State.Repair;
        }

        private void TargetStoragesButton_Click(object sender, EventArgs e)
        {
            Ship.AttackEnemy(attackingRoom, this, Projectile.Attack_Against.EnemyStorage);
            clicked_State = Clicked_State.None;
        }

        private void TargetWeaponsButton_Click(object sender, EventArgs e)
        {

        }

        private void TargetPowerGenButton_Click(object sender, EventArgs e)
        {

        }

        private void TargetPowerStorageButton_Click(object sender, EventArgs e)
        {

        }

        private void TargetEnemyShipButton_Click(object sender, EventArgs e)
        {

        }
    }
}