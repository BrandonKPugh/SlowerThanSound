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
        public EnemyAI enemyAI;
        private List<Projectile> projectiles;
        private List<Projectile> deadProjectiles;
        private CombatStateTargetUI _canvas;
        //private TextBox _powerAmount;
        private Tooltip _tooltip;
        private UIBox _selectedRoomBox;
        private ProgressBar _playerHealthBar;
        private ProgressBar _playerPowerBar;

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
            Ship.CurrentHealth = Ship.MaxHealth;

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
            _canvas.InitializeButton(FireWeaponButton_Click, ControlConstants.COMBATMODE_FIREWEAPON.Text);
            _canvas.InitializeButton(RepairRoomButton_Click, ControlConstants.COMBATMODE_REPAIRROOM.Text);

            Button BuildModeButton = new Button(buttonTexture, buttonFont)
            {
                ButtonInfo = ControlConstants.COMBATMODE_BUILDMODE
            };

            _selectedRoomBox = new UIBox(_pixelTexture)
            {
                UIBoxInfo = ControlConstants.ROOM_INFO_BOX,
                Color = new Color(Color.White, ControlConstants.ROOM_INFO_BOX_ALPHA)
            };

            /*
            TextBox PowerAmountText = new TextBox(buttonFont)
            {
                TextBoxInfo = ControlConstants.PRIMARY_TEXTBOX,
                Text = "Power: "
            };
            */

            _playerHealthBar = new ProgressBar(_pixelTexture, buttonFont)
            {
                ProgressBarInfo = ControlConstants.PLAYER_HEALTHBAR
            };
            _playerHealthBar.Value = 0.5f;

            /*
            _powerAmount = new TextBox(buttonFont)
            {
                TextBoxInfo = ControlConstants.PRIMARY_TEXTBOX_VALUE
            };
            */

            _playerPowerBar = new ProgressBar(_pixelTexture, buttonFont)
            {
                ProgressBarInfo = ControlConstants.PLAYER_POWERBAR
            };


            BuildModeButton.Click += BuildModeButton_Click;

            _uicomponents = new List<UI_Component>
            {
                BuildModeButton,
                //PowerAmountText,
                //_powerAmount,
                _canvas,
                _selectedRoomBox,
                _playerHealthBar,
                _playerPowerBar
            };

            _tooltip = new Tooltip(_pixelTexture, buttonFont);

            Ship.LoadContent(textures, tileTexture);
            this.Ship = ship;
            Ship.SetShipHealth();
            Ship.SetCombatValues();

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

            //_powerAmount.Text = Ship.Power.ToString();
            _playerPowerBar.Value = ((float)Ship.Power / (float)Ship.maxPower);
            _playerPowerBar.SetText("Power: " + (int)Ship.Power + " / " + (int)Ship.maxPower);

            _tooltip.Show = false;
            switch (clicked_State)
            {
                case (Clicked_State.None):
                    {
                        break;
                    }
                case (Clicked_State.SetAttack):
                    {
                        if(mouseOnTile)
                        {
                            foreach(Room room in Ship.Rooms)
                            {
                                if(room.RoomType == Room.Room_Type.Weapon && room.InteriorContains(tileUnderMouse))
                                {
                                    _tooltip.Show = true;
                                    _tooltip.SetText("DPS: " + room.DamagePerSecond());
                                }
                            }
                        }
                        if (mousePressed & mouseOnTile)
                        {
                            foreach (Room room in Ship.Rooms)
                            {
                                if (room.Contains(tileUnderMouse) & room.RoomType == Room.Room_Type.Weapon && !room.isBroken)
                                {
                                    attackingRoom = room;
                                    clicked_State = Clicked_State.Attacking;
                                    _selectedRoomBox.SetPosition(room.GetInteriorArea());
                                    _selectedRoomBox.Show = true;
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

            _canvas.SetProgressButtonValue(ControlConstants.COMBATMODE_TARGETENEMYSHIP.Text, ((float)enemyAI.shipHealth / (float)enemyAI.maxShipHealth));
            _canvas.SetProgressButtonValue(ControlConstants.COMBATMODE_TARGETWEAPONS.Text, ((float)enemyAI.weaponHealth / (float)enemyAI.weaponMaxHealth));
            _canvas.SetProgressButtonValue(ControlConstants.COMBATMODE_TARGETSTORAGES.Text, ((float)enemyAI.materialStorageHealth / (float)enemyAI.materialMaxHealth));
            _canvas.SetProgressButtonValue(ControlConstants.COMBATMODE_TARGETPOWERGEN.Text, ((float)enemyAI.powerGeneratorHealth / (float)enemyAI.generatorMaxHealth));
            _canvas.SetProgressButtonValue(ControlConstants.COMBATMODE_TARGETPOWERSTORAGE.Text, ((float)enemyAI.powerStorageHealth / (float)enemyAI.powerStorageMaxHealth));


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

            Ship.Update(gameTime);
            if (Ship.CurrentHealth < 0)
                LoseGame();
            _playerHealthBar.Value = ((float)Ship.CurrentHealth / (float)Ship.MaxHealth);

            
        }

        public void AddProjectile(Projectile projectile)
        {
            projectile._texture = projectileTexture;
            projectiles.Add(projectile);
        }

        public void LoseGame()
        {
            _game.ChangeState(new MainMenuState(_game, _graphicsDevice, _content));
        }

        private void BuildModeButton_Click(object sender, EventArgs e)
        {
            ReviewState.CombatInfo info = new ReviewState.CombatInfo();
            info.MetalCollected = 10;
            _game.ChangeState(new ReviewState(_game, _graphicsDevice, _content, Ship, info));
        }

        private void TargetStoragesButton_Click(object sender, EventArgs e)
        {
            if (attackingRoom != null)
            {
                Ship.AttackEnemy(attackingRoom, this, Projectile.Attack_Against.EnemyStorage);
                clicked_State = Clicked_State.None;
                attackingRoom = null;
                _selectedRoomBox.Show = false;
            }
        }

        private void TargetWeaponsButton_Click(object sender, EventArgs e)
        {
            if (attackingRoom != null)
            {
                Ship.AttackEnemy(attackingRoom, this, Projectile.Attack_Against.EnemyWeapon);
                clicked_State = Clicked_State.None;
                attackingRoom = null;
                _selectedRoomBox.Show = false;
            }
        }

        private void TargetPowerGenButton_Click(object sender, EventArgs e)
        {
            if (attackingRoom != null)
            {
                Ship.AttackEnemy(attackingRoom, this, Projectile.Attack_Against.EnemyGenerator);
                clicked_State = Clicked_State.None;
                attackingRoom = null;
                _selectedRoomBox.Show = false;
            }
        }

        private void TargetPowerStorageButton_Click(object sender, EventArgs e)
        {
            if (attackingRoom != null)
            {
                Ship.AttackEnemy(attackingRoom, this, Projectile.Attack_Against.EnemyPowerStorage);
                clicked_State = Clicked_State.None;
                attackingRoom = null;
                _selectedRoomBox.Show = false;
            }
        }

        private void TargetEnemyShipButton_Click(object sender, EventArgs e)
        {
            if (attackingRoom != null)
            {
                Ship.AttackEnemy(attackingRoom, this, Projectile.Attack_Against.EnemyHull);
                clicked_State = Clicked_State.None;
                attackingRoom = null;
                _selectedRoomBox.Show = false;
            }
        }

        private void FireWeaponButton_Click(object sender, EventArgs e)
        {
            clicked_State = Clicked_State.SetAttack;
        }

        private void RepairRoomButton_Click(object sender, EventArgs e)
        {
            clicked_State = Clicked_State.Repair;
        }
    }
}