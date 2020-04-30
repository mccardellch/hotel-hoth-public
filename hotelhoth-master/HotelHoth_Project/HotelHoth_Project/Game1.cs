using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace HotelHoth_Project
{
    /// <summary>
    /// Cortland Greindl Reporting For Duty
    /// This is Hotel Hoth 
    /// A GDAPS 2 group project
    /// 
    /// cool
    /// Raiden Mao is writing sth here
    /// I like Boxhead 
    /// It's a rouge like shooting game(hopefully):[
    /// 
    ///     

    //The game state
    enum GameState
    {
        Menu,
        Game,
        GameOver,
        Victory
    }

    //States for players and enemies probably
    enum PlayerState
    {
        FaceN, FaceS, FaceW, FaceE,
        WalkN, WalkS, WalkW, WalkE,
    }

    //Direction of sprite using enum (North, South, East, West)
    enum SpriteDirection
    {
        S,
        W,
        N,
        E
    }

    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SoundEffect gunshot;
        
        // Fields for all texture types
        Texture2D coinTexture;
        Texture2D weaponTexture;
        Texture2D heartTexture;
        Texture2D bulletTexture;
        Texture2D enemyTexture;
        Texture2D victoryTexture;

        int WIDTH;
        int HEIGHT;

        //two keyboard states to update input for keyboard
        KeyboardState kbState;
        KeyboardState previousKbState;

        //two Mouse state to update input for mouse
        MouseState currentMouse;
        MouseState previousMouse;

        private GameState currentGameState; //keeps track of the current state of the game
        private GameState prevGameState;    //previous state of the game

        //player stats
        private PlayerState playerDirection;  //The direction enum

        // Animation
        int frame;              // The current animation frame
        double timeCounter;     // The amount of time that has passed
        double fps;             // The speed of the animation
        double timePerFrame;    // The amount of time (in fractional seconds) per frame
        private int rotation;   // Rotation of spritesheet for vertical 

        // Constants for "source" rectangle (inside the image)
        const int WalkFrameCount = 19;         // The number of frames in the animation
        const int PlayerRectOffsetY = 142;    // How far down in the image are the frames?
        const int PlayerRectHeight = 35;     // The height of a single frame
        const int PlayerRectWidth = 45;      // The width of a single frame
        Vector2 spriteOrigin;

        double playerTimeCounter;

        private SpriteFont arial12text; //basic spritefont with arial font and size 12

        /// <summary>
        /// Creating Objects for each class
        /// Interface, Player
        /// Coin, Heart, Weapon
        /// </summary>
        //Interface Objects
        Interface startGame;
        Interface exitGame;
        Interface playAgain;
        Interface title;

        //Player Object
        Player player;

        // Enemy Object
        Enemy enemy;

        // Collectible Objects
        Coin coin;
        Weapon knife;
        Heart heart;

        //Envrionment Object
        Environment wall;

        //Gate Object
        Gate gate;

        //Set up ExternalTool object and array to be used for the map
        ExternalTool levelEditor;
        string[,] LevelMap;

        //Lists for Collectible Items
        List<Coin> listOfCoins;
        List<Weapon> listOfWeapons;
        List<Heart> listOfHearts;

        // List of enemies
        List<Enemy> listOfEnemies;

        Texture2D block; //filled block used for drawing collectible/heart background

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            //Sets the size of the game window
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1200;
            graphics.ApplyChanges();

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
            // TODO: Add your initialization logic here

            //Initializing ExternalTool, it's method and array
            levelEditor = new ExternalTool();
            levelEditor.ListWalls = new List<Environment>();
            levelEditor.Reader();
            LevelMap = levelEditor.GetArray;

            WIDTH = GraphicsDevice.Viewport.Width;
            HEIGHT = GraphicsDevice.Viewport.Height;

            currentGameState = GameState.Menu;

            //We have to have the player class later, but currently I will set the direction to South for now
            playerDirection = PlayerState.FaceS;

            //timer
            playerTimeCounter = 0;

            //Initializing Interface objects
            startGame = new Interface(new Rectangle(WIDTH/2 - 64, HEIGHT/2 + 20, 128, 64), null, null);
            playAgain = new Interface(new Rectangle(WIDTH/2 - 138, HEIGHT/2 + 20, 128, 64), null, null);
            exitGame = new Interface(new Rectangle(WIDTH/2 + 10, HEIGHT/2 + 20, 128, 64), null, null);
            title = new Interface(new Rectangle(WIDTH/2 - 200, HEIGHT/3, 400, 150), null, null);

            //Initializing Player Object
            player = new Player(null, new Rectangle(50, 50, 30, 30));

            //Collectible Objects
            coin = new Coin(null, new Rectangle(0, 0, 100, 100));
            knife = new Weapon(null, new Rectangle(0, 0, 50, 50));
            heart = new Heart(null, new Rectangle(0, 0, 50, 50));

            // Enemy Object
            enemy = new Enemy(null, new Rectangle(0, 0, 40, 40), BulletDirection.Right);

            //Initializing Environment wall object
            wall = new Environment(null, new Rectangle(0, 0, 10, 10));

            //Initializing Gate object
            gate = new Gate(null, new Rectangle(0, 0, 20, 20));

            // Initializing List of Enemies
            listOfEnemies = new List<Enemy>();

            //Initializing Lists for Collectibles
            listOfCoins = new List<Coin>();
            listOfWeapons = new List<Weapon>();
            listOfHearts = new List<Heart>();

            //for animation drawing
            fps = 25.0;
            timePerFrame = 1.0 / fps;
            rotation = 0;
            spriteOrigin = new Vector2(22.5f, 17.5f);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Loading textures
            arial12text = Content.Load<SpriteFont>("arial12text");

            //Loading Interface Textures
            startGame.LoadContent(this, "menus/start_up", "menus/start_ovr");
            playAgain.LoadContent(this, "menus/playAgain_up", "menus/playAgain_ovr");
            exitGame.LoadContent(this, "menus/exit_up", "menus/exit_ovr");
            title.Texture = Content.Load<Texture2D>("menus/hotelhoth_title");
            victoryTexture = Content.Load<Texture2D>("menus/victory");

            //Loading Player and Collectible Objects
            player.ObjectTexture = Content.Load<Texture2D>("gameAssets/testSpriteSheet");
            coin.ObjectTexture = Content.Load<Texture2D>("Collectibles/coin");
            knife.ObjectTexture = Content.Load<Texture2D>("Collectibles/knife");
            heart.ObjectTexture = Content.Load<Texture2D>("Collectibles/heart");
            enemy.ObjectTexture = Content.Load<Texture2D>("randomCharacter");
            gate.GateTexture = Content.Load<Texture2D>("gameAssets/door");

            // Loading Gunshot
            gunshot = Content.Load<SoundEffect>("gameAssets/gunshot");

            //Loading Texture2D for wall 
            wall.EnvironmentTexture = Content.Load<Texture2D>("wall");
            levelEditor.PopulateWallList(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, wall.EnvironmentTexture);

            //Create map for the game
            CreateMap();

            // Loads bullet texture
            bulletTexture = Content.Load<Texture2D>("gameAssets/Simple_Bullet");

            block = Content.Load<Texture2D>("gameAssets/block");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            // TODO: Add your update logic here

            this.IsMouseVisible = true; //showing mouse on the screen

            //Save the old keyboard state in the prevKBState field and get the new one.
            kbState = Keyboard.GetState();
            currentMouse = Mouse.GetState();

            /// <summary>
            /// Menu switch case
            /// through Menu, Game, GameOver
            /// </summary>
            switch (currentGameState)   
            {
                //  MENU STATE
                case GameState.Menu:
                    startGame.ButtonHover(currentMouse);

                    if (startGame.Hover == true && currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton != ButtonState.Pressed)
                    {
                        currentGameState = GameState.Game;
                    }
                    if (kbState.IsKeyDown(Keys.Enter) && !previousKbState.IsKeyDown(Keys.Enter))
                    {
                        currentGameState = GameState.Game;
                    }
                    if (kbState.IsKeyDown(Keys.Insert))
                    {
                        player.Health = 5;
                        player.Coins = 3;
                    }
                    break;
                //  IN-GAME STATE
                case GameState.Game:
                    //player movement
                    //Update the time in game for player
                    playerTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;

                    UpdateAnimation(gameTime);

                    player.PlayerMove(levelEditor); //temporary movement for single sprite character
                    player.PlayerToEnemyCollisions(listOfEnemies);                    

                    //loop through the List for coins
                    for (int i = listOfCoins.Count - 1; i >= 0; i--) 
                    {
                        //Call checkcollision on the specific coin to the player
                        if (listOfCoins[i].CheckCollision(player))
                        {
                            //if yes, remove that coin
                            listOfCoins.Remove(listOfCoins[i]);
                        }
                    }

                    //loop through the List for hearts
                    for (int i = listOfHearts.Count - 1; i >= 0; i--) 
                    {
                        //Call checkcollision on the specific heart to the player
                        if (listOfHearts[i].CheckCollision(player))
                        {
                            listOfHearts.Remove(listOfHearts[i]);
                        }
                    }

                    //loop through the List for weapons
                    for (int i = listOfWeapons.Count - 1; i >= 0; i--)
                    {
                        //Call checkcollision on the specific weapon to the player
                        if (listOfWeapons[i].CheckCollision(player))
                        {
                            //if yes, remove that weapon
                            listOfWeapons.Remove(listOfWeapons[i]);
                        }
                    }

                    // Press space to attack
                    if (kbState.IsKeyDown(Keys.Space) && !previousKbState.IsKeyDown(Keys.Space))
                    {
                        if(player.Attack(bulletTexture))
                        {
                            gunshot.Play();
                        }
                    }

                    // Update player bullet positions
                    player.UpdatePlayerBulletPositions(levelEditor);

                    // Enemies attack if facing player
                    for (int i = listOfEnemies.Count - 1; i >= 0; i--)
                    {
                        switch (listOfEnemies[i].Direction)
                        {
                            case BulletDirection.Up:
                                if (player.ObjectPosition.Y < listOfEnemies[i].ObjectPosition.Y && (int)(playerTimeCounter * 1000) % 2000 == 0)
                                {
                                    if (listOfEnemies[i].Attack(bulletTexture))
                                    {
                                        gunshot.Play();
                                    }
                                }
                                break;
                            case BulletDirection.Down:
                                if (player.ObjectPosition.Y > listOfEnemies[i].ObjectPosition.Y && (int)(playerTimeCounter * 1000) % 2000 == 0)
                                {
                                    if (listOfEnemies[i].Attack(bulletTexture))
                                    {
                                        gunshot.Play();
                                    }
                                }
                                break;
                            case BulletDirection.Left:
                                if (player.ObjectPosition.X < listOfEnemies[i].ObjectPosition.X && (int)(playerTimeCounter * 1000) % 2000 == 0)
                                {
                                    if (listOfEnemies[i].Attack(bulletTexture))
                                    {
                                        gunshot.Play();
                                    }
                                }
                                break;
                            case BulletDirection.Right:
                                if (player.ObjectPosition.X > listOfEnemies[i].ObjectPosition.X && (int)(playerTimeCounter * 1000) % 2000 == 0)
                                {
                                    if (listOfEnemies[i].Attack(bulletTexture))
                                    {
                                        gunshot.Play();
                                    }
                                }
                                break;
                        }
                    }

                    // Update enemy bullet positions
                    foreach (Enemy e in listOfEnemies)
                    {
                        e.UpdateEnemyBulletPositions(levelEditor);
                    }

                    // Update player bullet list
                    player.UpdatePlayerBullets(this.GraphicsDevice);

                    // Update enemy bullet lists
                    for (int i = 0; i < listOfEnemies.Count; i++)
                    {
                        listOfEnemies[i].UpdateEnemyBullets(listOfEnemies, this.GraphicsDevice);
                    }
                    

                    // Checks if player's bullets hit any enemies
                    for (int i = listOfEnemies.Count - 1; i >= 0; i--)
                    {
                        for (int j = player.Bullets.Count - 1; j >= 0; j--)
                        {
                            if (player.Bullets[j].CheckCollision(listOfEnemies[i]))
                            {
                                player.Bullets.Remove(player.Bullets[j]);

                                if (listOfEnemies[i].Health <= 0)
                                {
                                    listOfEnemies.Remove(listOfEnemies[i]);
                                }
                            }
                        }
                    }

                    // Checks if enemy's bullets hit the player
                    foreach (Enemy e in listOfEnemies)
                    {
                        for (int i = e.Bullets.Count - 1; i >= 0; i--)
                        {
                            if (e.Bullets[i].CheckCollision(player))
                            {
                                e.Bullets.Remove(e.Bullets[i]);
                            }
                        }
                    }

                    // Checks conditions to end the game
                    if (kbState.IsKeyDown(Keys.Enter) && !previousKbState.IsKeyDown(Keys.Enter) || player.Health <= 0)
                    {
                        currentGameState = GameState.GameOver;
                    }

                    if (gate.PlayerCollides(player))
                    {
                        currentGameState = GameState.Victory;
                    }

                    break;

               //   GAME OVER STATE
                case GameState.GameOver:
                    playAgain.ButtonHover(currentMouse);
                    if (playAgain.Hover == true && currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton != ButtonState.Pressed)
                    {
                        ResetGame();
                        currentGameState = GameState.Game;
                    }

                    exitGame.ButtonHover(currentMouse);
                    if (exitGame.Hover == true && currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton != ButtonState.Pressed)
                    {
                        this.Exit();
                    }
                    
                    if (kbState.IsKeyDown(Keys.Tab) && !previousKbState.IsKeyDown(Keys.Tab))
                    {
                        ResetGame();
                        currentGameState = GameState.Menu;
                    }
                    break;

                case GameState.Victory:
                    playAgain.ButtonHover(currentMouse);
                    if (playAgain.Hover == true && currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton != ButtonState.Pressed)
                    {
                        ResetGame();
                        currentGameState = GameState.Game;
                    }

                    exitGame.ButtonHover(currentMouse);
                    if (exitGame.Hover == true && currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton != ButtonState.Pressed)
                    {
                        this.Exit();
                    }
                    break;
            }
            previousKbState = kbState;

            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSlateGray);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            switch (currentGameState)
            {
                case GameState.Menu:
                    startGame.Draw(spriteBatch);
                    title.Draw(spriteBatch);
                    if (kbState.IsKeyDown(Keys.Insert))
                    {
                        spriteBatch.DrawString(arial12text, "enhanced collectibles enabled", new Vector2(500, 600), Color.Black);
                    }
                    break;

                case GameState.Game:
                    //Draws player object
                    //player.Draw(spriteBatch);

                    gate.Draw(spriteBatch);

                    // Draws animation
                    switch (player.PlayerCurrentState)
                    {
                        // LEFT
                        case PlayerState.FaceW:
                            player.PlayerPreviousState = PlayerState.FaceW;
                            rotation = 0;
                            DrawPlayerStanding(SpriteEffects.FlipHorizontally);
                            break;
                        case PlayerState.WalkW:
                            player.PlayerPreviousState = PlayerState.WalkW;
                            rotation = 0;
                            DrawPlayerWalking(SpriteEffects.FlipHorizontally);
                            break;

                        // RIGHT
                        case PlayerState.FaceE:
                            player.PlayerPreviousState = PlayerState.FaceE;
                            rotation = 0;
                            DrawPlayerStanding(SpriteEffects.None);
                            break;
                        case PlayerState.WalkE:
                            player.PlayerPreviousState = PlayerState.WalkE;
                            rotation = 0;
                            DrawPlayerWalking(SpriteEffects.None);
                            break;

                        //UP
                        case PlayerState.FaceN:
                            player.PlayerPreviousState = PlayerState.FaceN;
                            rotation = -90;
                            DrawPlayerStanding(SpriteEffects.None);
                            break;
                        case PlayerState.WalkN:
                            player.PlayerPreviousState = PlayerState.WalkN;
                            rotation = -90;
                            DrawPlayerWalking(SpriteEffects.None);
                            break;

                        // DOWN
                        case PlayerState.FaceS:
                            player.PlayerPreviousState = PlayerState.FaceS;
                            rotation = 90;
                            DrawPlayerStanding(SpriteEffects.None);
                            break;
                        case PlayerState.WalkS:
                            player.PlayerPreviousState = PlayerState.WalkS;
                            rotation = 90;
                            DrawPlayerWalking(SpriteEffects.None);
                            break;
                    }

                    //DRAW FROM THE LISTS HERE
                    //drawing coins
                    for (int i = 0; i < listOfCoins.Count; i++) 
                    {
                        listOfCoins[i].Draw(spriteBatch);
                    }
                        //drawing hearts
                    for (int i = 0; i < listOfHearts.Count; i++)
                    {
                        listOfHearts[i].Draw(spriteBatch);
                    }
                        //drawing weapons
                    for (int i = 0; i < listOfWeapons.Count; i++)
                    {
                        listOfWeapons[i].Draw(spriteBatch);
                    }
                        //drawing enemies
                    for (int i = 0; i < listOfEnemies.Count; i++)
                    {
                        listOfEnemies[i].Draw(spriteBatch);
                    }

                    //loop through each element [Environment wall] in the list (referenced from ExternalTool levelEditor) and draw
                    for (int i = 0; i < levelEditor.ListWalls.Count; i++)
                    {
                        spriteBatch.Draw(wall.EnvironmentTexture, levelEditor.ListWalls[i].EnvironmentPosition, Color.White);
                    }

                    //Draws the amount of health/hearts the player has 
                    spriteBatch.Draw(heart.ObjectTexture, new Rectangle(20, 1, 20, 20), Color.DimGray);
                    spriteBatch.Draw(heart.ObjectTexture, new Rectangle(42, 1, 20, 20), Color.DimGray);
                    spriteBatch.Draw(heart.ObjectTexture, new Rectangle(64, 1, 20, 20), Color.DimGray);
                    for (int i = 0; i < player.Health; i++)
                    {
                        if (player.Health > 3)
                        {
                            spriteBatch.Draw(heart.ObjectTexture, new Rectangle(20 + (i * 22), 1, 20, 20), Color.LimeGreen);
                        }
                        else
                        {
                            spriteBatch.Draw(heart.ObjectTexture, new Rectangle(20 + (i * 22), 1, 20, 20), Color.White);
                        }
                    }

                    //Draws the amount of coins the player has collected
                    spriteBatch.Draw(coin.ObjectTexture, new Rectangle(155, 1, 18, 18), Color.DimGray);
                    spriteBatch.Draw(coin.ObjectTexture, new Rectangle(177, 1, 18, 18), Color.DimGray);
                    spriteBatch.Draw(coin.ObjectTexture, new Rectangle(199, 1, 18, 18), Color.DimGray);
                    for (int i = 0; i < player.Coins; i++)
                    {
                        spriteBatch.Draw(coin.ObjectTexture, new Rectangle(155 + (i * 22), 1, 18, 18), Color.White);
                    }
                    
                    // Draws player bullets
                    player.DrawPlayerBullets(spriteBatch);

                    // Draws enemy bullets
                    for (int i = listOfEnemies.Count - 1; i >= 0; i--)
                    {
                        listOfEnemies[i].DrawEnemyBullets(spriteBatch);
                    }
                    break;
                    

                case GameState.GameOver:
                    //spriteBatch.DrawString(arial12text, "Game Over", new Vector2(50, 50), Color.Black);
                    //spriteBatch.DrawString(arial12text, "Press TAB to return to Menu", new Vector2(50, 75), Color.Black);
                    playAgain.Draw(spriteBatch);
                    exitGame.Draw(spriteBatch);
                    title.Draw(spriteBatch);
                    break;

                case GameState.Victory:
                    spriteBatch.Draw(victoryTexture, new Rectangle(WIDTH / 2 - 200, HEIGHT / 3 - 100, 400, 150), Color.White);
                    //spriteBatch.DrawString(arial12text, "Press TAB to return to Menu", new Vector2(50, 75), Color.Black);
                    playAgain.Draw(spriteBatch);
                    exitGame.Draw(spriteBatch);
                    title.Draw(spriteBatch);

                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
        

        /// <summary>
        /// Level Editor Loop
        /// This loop runs the level editor and puts everything into lists
        /// </summary>
        public void CreateMap()
        {
            for (int i = 0; i < levelEditor.GetArrayWidth; i++)
            {
                for (int j = 0; j < levelEditor.GetArrayHeight; j++)
                {
                    if (LevelMap[i, j] == "-")   //draws nothing
                    {
                        //draw nothing
                    }
                    else if (LevelMap[i, j] == "c")     //Draws coin
                    {
                        coin.ObjectPosition = new Rectangle(i * (GraphicsDevice.Viewport.Width / levelEditor.GetArrayWidth), j * (GraphicsDevice.Viewport.Height / levelEditor.GetArrayHeight), coin.ObjectPosition.Width, coin.ObjectPosition.Height);
                        listOfCoins.Add(new Coin(coin.ObjectTexture, coin.ObjectPosition));
                    }
                    else if (LevelMap[i, j] == "h")     //Draws heart
                    {
                        heart.ObjectPosition = new Rectangle(i * (GraphicsDevice.Viewport.Width / levelEditor.GetArrayWidth), j * (GraphicsDevice.Viewport.Height / levelEditor.GetArrayHeight), heart.ObjectPosition.Width, heart.ObjectPosition.Height);
                        listOfHearts.Add(new Heart(heart.ObjectTexture, heart.ObjectPosition));
                    }
                    else if (LevelMap[i, j] == "k")     //Draws knife
                    {
                        knife.ObjectPosition = new Rectangle(i * (GraphicsDevice.Viewport.Width / levelEditor.GetArrayWidth), j * (GraphicsDevice.Viewport.Height / levelEditor.GetArrayHeight), knife.ObjectPosition.Width, knife.ObjectPosition.Height);
                        listOfWeapons.Add(new Weapon(knife.ObjectTexture, knife.ObjectPosition));
                    }
                    else if (LevelMap[i, j] == "U")     //Draws Up Facing Enemy
                    {
                        enemy.ObjectPosition = new Rectangle(i * (GraphicsDevice.Viewport.Width / levelEditor.GetArrayWidth), j * (GraphicsDevice.Viewport.Height / levelEditor.GetArrayHeight), enemy.ObjectPosition.Width, enemy.ObjectPosition.Height);
                        listOfEnemies.Add(new Enemy(enemy.ObjectTexture, enemy.ObjectPosition, BulletDirection.Up));
                    }
                    else if (LevelMap[i, j] == "D")     //Draws Down Facing Enemy
                    {
                        enemy.ObjectPosition = new Rectangle(i * (GraphicsDevice.Viewport.Width / levelEditor.GetArrayWidth), j * (GraphicsDevice.Viewport.Height / levelEditor.GetArrayHeight), enemy.ObjectPosition.Width, enemy.ObjectPosition.Height);
                        listOfEnemies.Add(new Enemy(enemy.ObjectTexture, enemy.ObjectPosition, BulletDirection.Down));
                    }
                    else if (LevelMap[i, j] == "L")     //Draws Left Facing Enemy
                    {
                        enemy.ObjectPosition = new Rectangle(i * (GraphicsDevice.Viewport.Width / levelEditor.GetArrayWidth), j * (GraphicsDevice.Viewport.Height / levelEditor.GetArrayHeight), enemy.ObjectPosition.Width, enemy.ObjectPosition.Height);
                        listOfEnemies.Add(new Enemy(enemy.ObjectTexture, enemy.ObjectPosition, BulletDirection.Left));
                    }
                    else if (LevelMap[i, j] == "R")     //Draws Right Facing Enemy
                    {
                        enemy.ObjectPosition = new Rectangle(i * (GraphicsDevice.Viewport.Width / levelEditor.GetArrayWidth), j * (GraphicsDevice.Viewport.Height / levelEditor.GetArrayHeight), enemy.ObjectPosition.Width, enemy.ObjectPosition.Height);
                        listOfEnemies.Add(new Enemy(enemy.ObjectTexture, enemy.ObjectPosition, BulletDirection.Right));
                    }
                    else if (LevelMap[i, j] == "P")     //Draws Player
                    {
                        player.ObjectPosition = new Rectangle(i * (GraphicsDevice.Viewport.Width / levelEditor.GetArrayWidth), j * (GraphicsDevice.Viewport.Height / levelEditor.GetArrayHeight), player.ObjectPosition.Width, player.ObjectPosition.Height);
                    }
                    else if (LevelMap[i,j] == "g")
                    {
                        gate = new Gate(gate.GateTexture, new Rectangle(i * (GraphicsDevice.Viewport.Width / levelEditor.GetArrayWidth), j * (GraphicsDevice.Viewport.Height / levelEditor.GetArrayHeight), 40, 40));
                    }
                    else    //Draws Walls (edges of map)
                    {
                        wall.EnvironmentPosition = new Rectangle(i * (GraphicsDevice.Viewport.Width / levelEditor.GetArrayWidth), +
                            j * (GraphicsDevice.Viewport.Height / levelEditor.GetArrayHeight), wall.EnvironmentPosition.Width, wall.EnvironmentPosition.Height);
                    }
                }
            }
        }
       
        /// <summary>
        /// Resets the game completely
        /// </summary>
        public void ResetGame()
        {
            player.ResetPlayer();
            listOfEnemies.Clear();
            listOfCoins.Clear();
            listOfHearts.Clear();
            listOfWeapons.Clear();
            CreateMap();
            playerTimeCounter = 0;
        }

        //Update's players animation as needed
        private void UpdateAnimation(GameTime gameTime)
        {
            // Handle animation timing
            // - Add to the time counter
            // - Check if we have enough "time" to advance the frame
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeCounter >= timePerFrame)
            {
                frame += 1;                     // Adjust the frame

                if (frame > WalkFrameCount)     // Check the bounds
                    frame = 1;                  // Back to 1 (since 0 is the "standing" frame)

                timeCounter -= timePerFrame;    // Remove the time we "used"
            }
        }

        private void DrawPlayerStanding(SpriteEffects flipSprite)
        {
            Vector2 loc = new Vector2(player.ObjectPosition.X + 15, player.ObjectPosition.Y + 16);
            spriteBatch.Draw(
                player.ObjectTexture,      // - The texture to draw
                loc,                       // - The location to draw on the screen
                new Rectangle(0, PlayerRectOffsetY, PlayerRectWidth, PlayerRectHeight), // - The "source" rectangle --> This rectangle specifies where "inside" the texture to get pixels (We don't want to draw the whole thing)     
                Color.White,                    // - The color
                rotation,                              // - Rotation
                spriteOrigin,                   // - Origin inside the image (middle)
                0.75f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        private void DrawPlayerWalking(SpriteEffects flipSprite)
        {
            Vector2 loc = new Vector2(player.ObjectPosition.X + 15, player.ObjectPosition.Y + 16);
            spriteBatch.Draw(
                player.ObjectTexture,      // - The texture to draw
                loc,                       // - The location to draw on the screen
                new Rectangle(frame * PlayerRectWidth, PlayerRectOffsetY, PlayerRectWidth, PlayerRectHeight), // - The "source" rectangle --> This rectangle specifies where "inside" the texture to get pixels (We don't want to draw the whole thing)     
                Color.White,                    // - The color
                rotation,                              // - Rotation
                spriteOrigin,                   // - Origin inside the image (middle)
                0.75f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
    }
}
