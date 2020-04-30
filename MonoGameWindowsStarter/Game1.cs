using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameWindowsStarter.States;
using System.Collections.Generic;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private State _currentState;

        private State _nextState;

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        public Grid Grid;
        public Ship Ship;

        // Temporary:
        Button BuildModeButton;
        ProgressBar HealthBar;
        int frame = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Config.Initialize();

            graphics.PreferredBackBufferWidth = Config.GAME_WIDTH;
            graphics.PreferredBackBufferHeight = Config.GAME_HEIGHT;
            graphics.ApplyChanges();

            Grid = new Grid(Config.GRID_COUNT_X, Config.GRID_COUNT_Y);
            Grid.Initialize(Config.GRID_DESTINATION);

            Ship = new Ship();
            Ship.Initialize(Config.COMPONENTS);

            //*
            Rectangle loc = new Rectangle(Config.PRIMARY_BUTTON_X, Config.PRIMARY_BUTTON_Y, Config.PRIMARY_BUTTON_WIDTH, Config.PRIMARY_BUTTON_HEIGHT);
            BuildModeButton = new Button(loc, "Build Mode", Color.ForestGreen);

            loc.Y -= 200;
            HealthBar = new ProgressBar(loc, Color.DarkGray, Color.LightYellow, 1.0f, "Test Text");
            HealthBar.Initialize();
            //*/

            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D tileTexture = Content.Load<Texture2D>("Tile");
            Grid.LoadContent(tileTexture);

            Dictionary<Component.Component_Type, Texture2D> textures = new Dictionary<Component.Component_Type, Texture2D>();
            //Texture2D componentTexture = Content.Load<Texture2D>("Component");
            //textures.Add(Component.Component_Type.Generic, componentTexture);
            Texture2D weaponTexture = Content.Load<Texture2D>("Component_Weapon");
            textures.Add(Component.Component_Type.Weapon, weaponTexture);
            Texture2D structureTexture = Content.Load<Texture2D>("Structure");
            textures.Add(Component.Component_Type.Structure, structureTexture);

            //*
            Texture2D buttonTexture = Content.Load<Texture2D>("Button");
            SpriteFont font = Content.Load<SpriteFont>("DebugFont");
            BuildModeButton.LoadContent(font, buttonTexture);

            Texture2D barTexture = Content.Load<Texture2D>("Progress_Bar");
            Texture2D pixel = Content.Load<Texture2D>("pixel");
            HealthBar.LoadContent(barTexture, pixel, pixel, font);
            //*/

            Ship.LoadContent(textures);

            _currentState = new MenuState(this, graphics.GraphicsDevice, Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                int x = Mouse.GetState().X;
                int y = Mouse.GetState().Y;
                if(Grid.PixelToTile(x, y, out int tileX, out int tileY))
                {
                    Component found = null;
                    foreach(Component c in Ship.GetComponents())
                    {
                        if(c.X == tileX && c.Y == tileY)
                        {
                            found = c;
                            break;
                        }
                    }
                    if(found == null)
                    {
                        Component newComponent = new WeaponComponent(tileX, tileY, Config.COMPONENT_WEAPON_COLOR);
                        Ship.AddComponent(newComponent);
                    }
                }
            }

            //*
            HealthBar.Update((1000 - frame) / 1000f);
            frame++;
            if (frame > 1000)
                frame = 0;
            //*/

            if (_nextState != null)
            {
                _currentState = _nextState;

                _nextState = null;
            }

            _currentState.Update(gameTime);

            _currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Config.BACKGROUND_COLOR);

            Grid.Draw(spriteBatch);

            Ship.Draw(spriteBatch, Grid.Info);

            _currentState.Draw(gameTime, spriteBatch);

            //*
            BuildModeButton.Draw(spriteBatch);

            HealthBar.Draw(spriteBatch);
            //*/

            base.Draw(gameTime);
        }
    }
}
