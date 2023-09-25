/* Author: Ali Sher
 * File Name: The Crate Escape
 * Creation Date: Nov. 1st 2016
 * Modification Date: Nov. 11th 2016
 * Description: Endless runner game
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Assignment2_EndlessRunner
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class EndlessRunner : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //background variables (for looping too)
        Texture2D bgImg;
        Rectangle bgBox;
        Rectangle bgBox2;

        //Song for game
        Song bgSong;

        //crate variables
        Texture2D crateImg;
        Rectangle crateBox;
        //broken crate
        Texture2D crateImgBroke;
        Rectangle crateBoxBroke;

        //Time coutner variables
        int counter = 0;
        int score = 0;
        const int COUNTER_RATE = 1;
        const int SCORE_RATE = 1;
        float speedUpCounter = 0;

        //scroll speed variable
        int screenScrollSpeed = 5;
        const int RUNNER_SCROLL_SPEED = 20;
        int birdSpeed;
        const int BULLET_SPEED = 20;

        //variables for chests
        Texture2D chestImgGold;
        Texture2D chestImgSilver;
        Texture2D chestImgBronze;
        Rectangle chestBoxG;
        Rectangle chestBoxS;
        Rectangle chestBoxB;

        //variables for runner animation
        Texture2D runnerImg;
        Rectangle runnerBox;
        float runnerW;
        float runnerH;
        Rectangle runnerSrcBox;
        float runnerFramesWide = 5;
        float runnerFramesHigh = 2;
        float runnerNumFrames = 9;
        float runnerFrameNum = 0;
        float runnerSmoothness = 4;
        float runnerFrame;

        //variables for ghost animation
        Texture2D ghostImg;
        Rectangle ghostBox;
        float ghostW;
        float ghostH;
        Rectangle ghostSrcBox;
        float ghostFramesWide = 12;
        float ghostFramesHigh = 2;
        float ghostNumFrames = 23;
        float ghostFrameNum = 0;
        float ghostSmoothness = 2;
        float ghostFrame;

        //variables for bird enemy animation
        Texture2D birdImg;
        Rectangle birdBox;
        float birdW;
        float birdH;
        Rectangle birdSrcBox;
        float birdFramesWide = 5;
        float birdFramesHigh = 4;
        float birdNumFrames = 19;
        float birdFrameNum = 0;
        float birdSmoothness = 5;
        float birdFrame;

        //variables for powerup animation
        Texture2D PUImg;
        Rectangle PUBox;
        float PUW;
        float PUH;
        Rectangle PUSrcBox;
        float PUFramesWide = 4;
        float PUFramesHigh = 1;
        float PUNumFrames = 4;
        float PUFrameNum = 0;
        float PUSmoothness = 2;
        float PUFrame;

        //variables for bullet image
        Texture2D bulletImg;
        Rectangle[] bulletBox;

        //turret shot timer
        int turretShot;

        //chest spawning variables
        Random chestSpawner;
        Random chestType;
        int chestTypeStorage;
        int chestSpawnerStorage;
        int chestYLoc;
        int chestSpawnRate;

        //turret spawning variables
        Random turretSpawner;
        int turretSpawnerStorage;
        bool turretWillSpawn;
        int turretSpawnRate;

        //turret spawning variables
        Random crateSpawner;
        int crateSpawnerStorage;
        bool crateWillSpawn;
        int crateSpawnRate;

        //bird spawning variables
        Random birdSpawner;
        int birdSpawnerStorage;
        int birdYLoc;
        int birdSpawnRate;

        //turret variables
        Texture2D turretImg;
        Rectangle turretBox;
        //turret broke variables
        Texture2D turretImgBroke;
        Rectangle turretBoxBroke;

        //track who killed you
        int[] kills;
        string killedby;

        //physics variables
        float angle = 80;     
        float speed = 15;        
        Vector2 velocity;           
        Vector2 initialVelocity;   
        Vector2 gravity;
        const float RUNNER_SPEED = 10f;

        //jump variable
        bool jumped = false;
        int floorLoc;

        //screen size variables
        int screenWidth;
        int screenLength;

        //Assign mouseState variable
        MouseState mouse;

        //Assign keyboardState variable
        KeyboardState keyB;

        //!!!!!!!!!! TEST !!!!!!!!!!!
        Vector2 mouseLoc;
        SpriteFont mousePX;

        //Score Display Sprite
        Vector2 scoreLoc;
        SpriteFont spriteText;

        //menu screen vectors
        Vector2 titleLoc;
        Vector2 startLoc;
        Vector2 leaderboardLoc;
        //menu variables
        SpriteFont buttonFont;
        Texture2D whiteimg;
        Rectangle[] whiteBox;

        //leaderboard variables
        Vector2 lbBackLoc;
        Vector2 lbLoc;
        int topScore;

        //deathscreen variables
        Vector2 tryAgainLoc;
        Vector2 mainMenuLoc;
        Vector2 quitLoc;
        Vector2 finalScoreLoc;
        Vector2 killedByLoc;

        //chest collection
        int[] chestsCollected;

        //CollisionDetection variables
        const bool COLLISION = true;
        const bool NO_COLLISION = false;

        //mouse rectangle
        Rectangle mouseRec;

        //GameStates
        int gamestate = 0;
        const int MENU = 0;
        const int TOP_SCORE = 1;
        const int DEATHSCREEN = 2;
        const int GAME = 3;


        //||||||||||||||SUBPROGRAMS||||||||||||||\\

        //Collision Detection Subprogram
        private bool CollisionDetection(Rectangle box1, Rectangle box2)
        {
            if (!(
                box1.Bottom < box2.Top ||
                box1.Right < box2.Left ||
                box1.Top > box2.Bottom ||
                box1.Left > box2.Right))
            {
                return COLLISION;
            }
            else
            {
                return NO_COLLISION;
            }
        }

        //Calculate the velocity of the runner (X is set to 0 so there is only vertical jump)
        private Vector2 CalcVelocity(float speed, float angle)
        {
            float x = 0;
            float y = speed * (float)(MathHelper.ToRadians(angle)) * -1;

            return new Vector2(x, y);
        }

        public EndlessRunner()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //!!!!! REMOVE TEST (show mouse) !!!!!!!!
            this.IsMouseVisible = true;

            //Change screen size and resolution
            graphics.PreferredBackBufferWidth = 1070;
            graphics.PreferredBackBufferHeight = 720;

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

            //assign calculations to velocity and make starting velocity equal to velocity & assign value to gravity (GRAVITY IS STRONGER TO REDUCE AIR TIME)
            velocity = CalcVelocity(speed, angle);
            initialVelocity = velocity;
            gravity = new Vector2(0, 30f / 60);

            // set screenwidth and screenlength to the screen
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenLength = graphics.GraphicsDevice.Viewport.Height;

            //Initialize chest object
            chestSpawner = new Random();
            chestType = new Random();

            //initialize bird spawner
            birdSpawner = new Random();

            //initialize turret spawner
            turretSpawner = new Random();

            //initilize cratespawner
            crateSpawner = new Random();

            //bullet boxes initlization
            bulletBox = new Rectangle[3];

            //set the value for the vector2 to hold the score
            scoreLoc = new Vector2(0, 0);

            //initilize white box for many uses
            whiteBox = new Rectangle[7];

            //initilize chests collected
            chestsCollected = new int[3];

            //array to track who killed you
            kills = new int[3];

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


            //Load all content here
            bgImg = Content.Load<Texture2D>("Images/Background/Background");
            bgSong = Content.Load<Song>("Songs/Abadox (NES) - Introduction");
            runnerImg = Content.Load<Texture2D>("Images/Sprites/Runner/CharSprite");
            chestImgGold = Content.Load<Texture2D>("Images/Sprites/GoldChest");
            chestImgSilver = Content.Load<Texture2D>("Images/Sprites/SilverTreasureChest");
            chestImgBronze = Content.Load<Texture2D>("Images/Sprites/BronzeChest");
            crateImg = Content.Load<Texture2D>("Images/Sprites/crate_obstacle");
            crateImgBroke = Content.Load<Texture2D>("Images/Sprites/crate_obstacle_broke");
            birdImg = Content.Load<Texture2D>("Images/Sprites/eagleGOOD");
            ghostImg = Content.Load<Texture2D>("Images/Sprites/ghost");
            PUImg = Content.Load<Texture2D>("Images/Sprites/PowerUpGood");
            turretImg = Content.Load<Texture2D>("Images/Sprites/TBturret");
            turretImgBroke = Content.Load<Texture2D>("Images/Sprites/TBturretBroke");
            bulletImg = Content.Load<Texture2D>("Images/Sprites/bullet");

            //menu content
            whiteimg = Content.Load<Texture2D>("Images/Background/whiteBox");
            whiteBox[0] = new Rectangle(190, 380, 200, 100);
            whiteBox[1] = new Rectangle(680, 380, 200, 100);

            //set vector2 for menu screen
            titleLoc = new Vector2(280, screenLength / 4);
            startLoc = new Vector2(260, 420);
            leaderboardLoc = new Vector2(710, 420);

            //deathscreen content
            whiteBox[2] = new Rectangle (190, 481, 200, 100);
            whiteBox[1] = new Rectangle (680, 481, 200, 100);

            //vector2 for deathscreen
            tryAgainLoc = new Vector2(240, 520);
            mainMenuLoc = new Vector2(710, 520);
            quitLoc = new Vector2(0, 0);
            finalScoreLoc = new Vector2(0, 600);
            killedByLoc = new Vector2(280, 250);

            //Vector2 for leaderboard
            lbBackLoc = new Vector2(100, 660);
            lbLoc = new Vector2(460, 50);


            //Load all static Rectangles here
            bgBox = new Rectangle(0, 0, screenWidth, screenLength);
            bgBox2 = new Rectangle(screenWidth, 0, screenWidth, screenLength);


            //Load runner rectangles here
            runnerW = runnerImg.Width / (int)runnerFramesWide;
            runnerH = runnerImg.Height / (int)runnerFramesHigh;
            runnerBox = new Rectangle(380, 690 - (int)runnerH, (int)runnerW, (int)runnerH);
            runnerSrcBox = new Rectangle(runnerSrcBox.X, runnerSrcBox.Y, (int)runnerW, (int)runnerH);

            //Load bird enemy rectangles here
            birdW = birdImg.Width / (int)birdFramesWide;
            birdH = birdImg.Height / (int)birdFramesHigh;
            birdSrcBox = new Rectangle(birdSrcBox.X, runnerSrcBox.Y, (int)birdW, (int)birdH);

            //Load ghost rectangles here
            ghostW = ghostImg.Width / (int)ghostFramesWide;
            ghostH = ghostImg.Height / (int)ghostFramesHigh;
            ghostBox = new Rectangle(0, 690 - (int)ghostH, (int)ghostW, (int)ghostH);
            ghostSrcBox = new Rectangle(ghostSrcBox.X, ghostSrcBox.Y, (int)ghostW, (int)ghostH);

            //Load ghost rectangles here
            PUW = PUImg.Width / (int)PUFramesWide;
            PUH = PUImg.Height / (int)PUFramesHigh;
            PUSrcBox = new Rectangle(PUSrcBox.X, PUSrcBox.Y, (int)PUW, (int)PUH);

            //Load floorLoc to hold value for floor collision
            floorLoc = 690 - (int)runnerH;

            //Play song
            MediaPlayer.Play(bgSong);
            MediaPlayer.Volume = 1f;
            MediaPlayer.IsRepeating = true;

            //Load SpriteFonts here

            //General SpriteFont
            spriteText = Content.Load<SpriteFont>("Fonts/Score Font");
            buttonFont = Content.Load<SpriteFont>("Fonts/titleFont");

            //!!!!!!! TEST !!!!!!!!!!!!
            mousePX = Content.Load<SpriteFont>("Fonts/Test Fonts/MouseLocation");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            //Assign value to keyB and mouse
            mouse = Mouse.GetState();
            keyB = Keyboard.GetState();


            //Update bird speed to match screen speed
            birdSpeed = screenScrollSpeed + 3;


            // TODO: Add your update logic here


            // !!!!!! FOR TESTING, REMOVE AFTER !!!!!!
            mouseLoc = new Vector2(mouse.X, mouse.Y + 20);


            //Code to loop background
            bgBox.X = bgBox.X - screenScrollSpeed;
            bgBox2.X = bgBox2.X - screenScrollSpeed;

            if (bgBox.X <= -screenWidth)
            {
                bgBox.X = screenWidth;
            }

            if (bgBox2.X <= -screenWidth)
            {
                bgBox2.X = screenWidth;
            }


            //|||||||||||||||||||||IN MENU||||||||||||||||||||||||||||\\

            if (gamestate == MENU)
            {
                screenScrollSpeed = 5;
                bgBox.X = bgBox.X - screenScrollSpeed;
                bgBox2.X = bgBox2.X - screenScrollSpeed;

                if (bgBox.X <= -screenWidth)
                {
                    bgBox.X = screenWidth;
                }

                if (bgBox2.X <= -screenWidth)
                {
                    bgBox2.X = screenWidth;
                }


                //assign mouseRec for sick collisions
                mouseRec = new Rectangle(mouse.X, mouse.Y, 1, 1);

                //sick effect with start btn
                if (CollisionDetection(mouseRec, whiteBox[0]))
                {
                    whiteBox[0] = new Rectangle(165, 355, 250, 150);

                    //start game if mouse is clicked
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        gamestate = GAME;
                    }
                }
                else
                {
                    whiteBox[0] = new Rectangle(190, 380, 200, 100);
                }

                //sick effect with leaderboard btn
                if (CollisionDetection(mouseRec, whiteBox[1]))
                {
                    whiteBox[1] = new Rectangle(655, 355, 250, 150);

                    //go to leaderboard
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        gamestate = TOP_SCORE;
                    }
                }
                else
                {
                    whiteBox[1] = new Rectangle(680, 380, 200, 100);
                }
            }



            //|||||||||||||||||||||||TOP_SCORE||||||||||||||||||||||||||||||||\\

            if (gamestate == TOP_SCORE)
            {
                whiteBox[4] = new Rectangle(10, screenLength - 110, 200, 100);
                

                if (keyB.IsKeyDown(Keys.Escape))
                {
                    gamestate = MENU;
                }

            }



            //|||||||||||||||||||||||DEATH SCREEN||||||||||||||||||||||||||||||||\\
            
            if (gamestate == DEATHSCREEN)
            {
                mouseRec = new Rectangle(mouse.X, mouse.Y, 1, 1);

                //top score tracker
                if (score > topScore)
                {
                    topScore = score;
                }

                if (keyB.IsKeyDown(Keys.Escape))
                {
                  this.Exit();
                }

                //Resart game
                if (CollisionDetection(mouseRec, whiteBox[2]))
                {
                    whiteBox[2] = new Rectangle(165, 455, 250, 150);

                    //start game if mouse is clicked
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        gamestate = GAME;
                        
                        //reset stats
                        score = 0;
                        screenScrollSpeed = 7;
                        chestsCollected[0] = 0;
                        chestsCollected[1] = 0;
                        chestsCollected[2] = 0;
                        kills[0] = 0;
                        kills[1] = 0;
                        kills[2] = 0;
                        speedUpCounter = 0;
                        crateSpawnRate = 0;
                        counter = 0;
                        ghostBox.X = 0;
                        turretBox.Y = -500;
                        turretBoxBroke.Y = -500;
                        birdBox.Y = -500;
                        chestBoxB.Y = -500;
                        chestBoxS.Y = -500;
                        chestBoxG.Y = -500;
                        runnerBox.Y = floorLoc;

                    }
                }
                else
                {
                    whiteBox[2] = new Rectangle(190, 481, 200, 100);
                }

                //sick effect with main menu
                if (CollisionDetection(mouseRec, whiteBox[3]))
                {
                    whiteBox[3] = new Rectangle(655, 455, 250, 150);

                    //go to menu
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {

                        gamestate = MENU;

                    }
                }
                else
                {
                    whiteBox[3] = new Rectangle(680, 481, 200, 100);
                }
            }





            //|||||||||||||||||||||IN GAME||||||||||||||||||||||||||||\\

            if (gamestate == GAME)
            {

                screenScrollSpeed = 5;

                //Jump Logic including gravity calculations
                if (!jumped)
                {
                    if (keyB.IsKeyDown(Keys.Space))
                    {
                        runnerBox.Y -= 10;
                        jumped = true;
                        initialVelocity = velocity;
                    }
                }

                if (jumped == true)
                {
                    velocity = velocity + gravity;

                    runnerBox.Offset((int)velocity.X, (int)velocity.Y);

                    if (runnerBox.Y > floorLoc)
                    {
                        runnerBox.Y = floorLoc;
                        jumped = false;
                        velocity = initialVelocity;
                    }
                }

                //|||||||||||||||||||||||ANIMATION LOGIC||||||||||||||||||||||||||||||||\\

                //logic to animate runner
                if (runnerFrame == 0)
                {
                    runnerFrameNum++;
                    int runnerCol = (int)runnerFrameNum % (int)runnerFramesWide;
                    int runnerRow = (int)runnerFrameNum / (int)runnerFramesWide;
                    runnerSrcBox.X = runnerCol * (int)runnerW;
                    runnerSrcBox.Y = runnerRow * (int)runnerH;
                }

                if (runnerFrameNum == runnerNumFrames)
                {
                    runnerFrameNum = 0;
                }

                runnerFrame = (runnerFrame + 1) % runnerSmoothness;

                //logic to animate ghost
                if (ghostFrame == 0)
                {
                    ghostFrameNum++;
                    int ghostCol = (int)ghostFrameNum % (int)ghostFramesWide;
                    int ghostRow = (int)ghostFrameNum / (int)ghostFramesWide;
                    ghostSrcBox.X = ghostCol * (int)ghostW;
                    ghostSrcBox.Y = ghostRow * (int)ghostH;
                }

                if (ghostFrameNum == ghostNumFrames)
                {
                    ghostFrameNum = 0;
                }

                ghostFrame = (ghostFrame + 1) % ghostSmoothness;

                //logic to animate bird
                if (birdFrame == 0)
                {
                    birdFrameNum++;
                    int birdCol = (int)birdFrameNum % (int)birdFramesWide;
                    int birdRow = (int)birdFrameNum / (int)birdFramesWide;
                    birdSrcBox.X = birdCol * (int)birdW;
                    birdSrcBox.Y = birdRow * (int)birdH;
                }

                if (birdFrameNum == birdNumFrames)
                {
                    birdFrameNum = 0;
                }

                birdFrame = (birdFrame + 1) % birdSmoothness;

                //logic to animate power up
                if (PUFrame == 0)
                {
                    PUFrameNum++;
                    int PUCol = (int)PUFrameNum % (int)PUFramesWide;
                    int PURow = (int)PUFrameNum / (int)PUFramesWide;
                    PUSrcBox.X = PUCol * (int)PUW;
                    PUSrcBox.Y = PURow * (int)PUH;
                }

                if (PUFrameNum == PUNumFrames)
                {
                    PUFrameNum = 0;
                }

                PUFrame = (PUFrame + 1) % PUSmoothness;



                //|||||||||||||||||||||||TRANSLATION LOGIC||||||||||||||||||||||||||||||||\\

                //have objects scrolling at same speed as background
                chestBoxG.X -= screenScrollSpeed;
                chestBoxB.X -= screenScrollSpeed;
                chestBoxS.X -= screenScrollSpeed;
                crateBox.X -= screenScrollSpeed;
                crateBoxBroke.X -= screenScrollSpeed;
                turretBox.X -= screenScrollSpeed;
                turretBoxBroke.X -= screenScrollSpeed;
                PUBox.X -= screenScrollSpeed;
                birdBox.X -= birdSpeed;
                bulletBox[0].X -= BULLET_SPEED;
                bulletBox[1].X -= BULLET_SPEED;
                bulletBox[2].X -= BULLET_SPEED;


                //Set borders
                runnerBox.X = Math.Max(runnerBox.X, 0);
                runnerBox.X = Math.Min(runnerBox.X, 1080 - runnerBox.Width);


                //Time counter
                counter = counter + COUNTER_RATE;

                if (counter == 60)
                {
                    score = score + SCORE_RATE;
                    counter = 0;
                }


                //Counter to speed up SCROLL SPEED
                speedUpCounter += 1;

                if (speedUpCounter == 1200)
                {
                    screenScrollSpeed += 5;
                    speedUpCounter = 0;

                }

                //Stops the screen counter to reacha maximum
                if (screenScrollSpeed >= 15)
                {
                    speedUpCounter = 121;
                }


                //|||||||||||||||||||||||SPAWNS||||||||||||||||||||||||||||||||\\

                //spawns different chests at a certain rate
                chestSpawnRate += 1;
                if (chestSpawnRate == 500)
                {
                    //Choose the chest type
                    chestTypeStorage = chestType.Next(1, 100);

                    if (chestTypeStorage < 11)
                    {
                        //if chest is gold:
                        switch (chestSpawnerStorage = chestSpawner.Next(1, 4))
                        {
                            case 1:
                                chestYLoc = 100;
                                break;

                            case 2:
                                chestYLoc = 300;
                                break;

                            case 3:
                                chestYLoc = 500;
                                break;
                        }

                        chestBoxG = new Rectangle((int)(screenWidth + chestImgGold.Width), chestYLoc, chestImgGold.Width, chestImgGold.Height);
                        chestSpawnRate = 0;
                    }
                    // if chest is silver
                    else if (chestTypeStorage > 10 && chestTypeStorage < 51)
                    {

                        switch (chestSpawnerStorage = chestSpawner.Next(1, 4))
                        {
                            case 1:
                                chestYLoc = 100;
                                break;

                            case 2:
                                chestYLoc = 300;
                                break;

                            case 3:
                                chestYLoc = 500;
                                break;
                        }

                        chestBoxS = new Rectangle((int)(screenWidth + chestImgGold.Width), chestYLoc, chestImgGold.Width, chestImgGold.Height);
                        chestSpawnRate = 0;

                    }
                    // if chest is bronze
                    else if (chestTypeStorage > 50 && chestTypeStorage < 101)
                    {
                        switch (chestSpawnerStorage = chestSpawner.Next(1, 4))
                        {
                            case 1:
                                chestYLoc = 100;
                                break;

                            case 2:
                                chestYLoc = 300;
                                break;

                            case 3:
                                chestYLoc = 500;
                                break;
                        }

                        chestBoxB = new Rectangle((int)(screenWidth + chestImgGold.Width), chestYLoc, chestImgGold.Width, chestImgGold.Height);
                        chestSpawnRate = 0;
                    }
                }

                //Generate bird spawns
                birdSpawnRate += 1;

                if (birdSpawnRate == 400)
                {
                    switch (birdSpawnerStorage = birdSpawner.Next(1, 3))
                    {
                        case 1:
                            birdYLoc = 200;
                            break;

                        case 2:
                            birdYLoc = 550;
                            break;
                    }

                    birdBox = new Rectangle((int)(screenWidth + birdImg.Width), birdYLoc, (int)birdW, (int)birdH);
                    birdSpawnRate = 0;
                }

                //Generate turret spawns
                turretSpawnRate += 1;
                if (turretSpawnRate == 200)
                {
                    switch (turretSpawnerStorage = turretSpawner.Next(1, 4))
                    {
                        case 1:
                            turretWillSpawn = true;
                            break;

                        case 2:
                            turretWillSpawn = false;
                            break;

                        case 3:
                            turretWillSpawn = false;
                            break;
                    }

                    turretSpawnRate = 0;
                }

                if (turretWillSpawn == true)
                {
                    turretBox = new Rectangle(screenWidth + turretImg.Width, 690 - turretImg.Height + 50, turretImg.Width - 50, turretImg.Height - 50);
                    turretWillSpawn = false;
                }


                //when to shoot turret
                if (turretBox.X <= screenWidth && turretBox.Y < 690 && turretBox.Y > 1)
                {
                    turretShot++;
                    //shoot first bullet
                    if (turretShot == 90)
                    {   
                        bulletBox[0] = new Rectangle(turretBox.X, turretBox.Y + 55, bulletImg.Width, bulletImg.Height);
                    }
                        //shoot second bullet
                    if (turretShot == 150)
                    {       
                        bulletBox[1] = new Rectangle(turretBox.X, turretBox.Y + 55, bulletImg.Width, bulletImg.Height);

                    }
                            //shoot third bullet
                    if (turretShot == 210)
                    {
                        bulletBox[2] = new Rectangle(turretBox.X, turretBox.Y + 55, bulletImg.Width, bulletImg.Height);
                        turretShot = 0;
                    }
                }





                // Generate crate spawns
                crateSpawnRate += 1;
                if (crateSpawnRate == 300)
                {
                    switch (crateSpawnerStorage = crateSpawner.Next(1, 3))
                    {
                        case 1:
                            crateWillSpawn = true;
                            break;

                        case 2:
                            crateWillSpawn = false;
                            break;

                    }

                    crateSpawnRate = 0;
                }

                if (crateWillSpawn == true)
                {
                    crateBox = new Rectangle(screenWidth + crateImg.Width, 690 - crateImg.Height, crateImg.Width, crateImg.Height);
                    crateWillSpawn = false;
                }


                //|||||||||||||||||||||||COLLISIONS||||||||||||||||||||||||||||||||\\

                //Chesck for collisions between chest to add points

                //Gold Collision
                if (CollisionDetection(runnerBox, chestBoxG))
                {
                    PUBox = new Rectangle(chestBoxG.X, chestBoxG.Y, (int)PUW, (int)PUH);
                    chestBoxG.Y = -110;
                    score += 30;
                    chestsCollected[0]++;
                }

                //Silver Collision
                if (CollisionDetection(runnerBox, chestBoxS))
                {
                    PUBox = new Rectangle(chestBoxS.X, chestBoxS.Y, (int)PUW, (int)PUH);
                    chestBoxS.Y = -110;
                    score += 15;
                    chestsCollected[1]++;
                }

                //Bronze Collision
                if (CollisionDetection(runnerBox, chestBoxB))
                {
                    PUBox = new Rectangle(chestBoxB.X, chestBoxB.Y, (int)PUW, (int)PUH);
                    chestBoxB.Y = -110;
                    score += 5;
                    chestsCollected[2]++;
                }

                //Check for collision between runner and crate and impact score accordingly
                if (CollisionDetection(runnerBox, crateBox))
                {
                    //Load rectangle of broken crate
                    crateBoxBroke = new Rectangle(crateBox.X, crateBox.Y, crateImgBroke.Width, crateImgBroke.Height);

                    crateBox.Y = -110;
                    score -= 30;

                    //speed up ghost to chase faster after runner
                    ghostBox.X += 100;
                }


                //Check for collision between runner and bird
                if (CollisionDetection(runnerBox, birdBox))
                {
                    gamestate = DEATHSCREEN;
                    
                    //track if you died to bird
                    kills[0] ++;
                }

                //check for collision between runner and ghost
                if (CollisionDetection(runnerBox, ghostBox))
                {
                    gamestate = DEATHSCREEN;

                    //track if you died to ghost
                    kills[1]++;
                }


                //check for collision between all three bullets
                if (CollisionDetection(runnerBox, bulletBox[0]))
                {
                    gamestate = DEATHSCREEN;

                    //track if you died to bullet
                    kills[2]++;
                }

                if (CollisionDetection(runnerBox, bulletBox[1]))
                {
                    gamestate = DEATHSCREEN;

                    //track if you died to bullet
                    kills[2]++;
                }

                if (CollisionDetection(runnerBox, bulletBox[2]))
                {
                    gamestate = DEATHSCREEN;

                    //track if you died to bullet
                    kills[2]++;
                }


                //break turret if runner and turret collide
                if (CollisionDetection(runnerBox, turretBox))
                {
                    turretBoxBroke = new Rectangle(turretBox.Y, turretBox.X, turretImgBroke.Width, turretImgBroke.Height);
                    turretBox.Y = -500;
                    score += 15;
                }

                //mssg if you died to the bird
                if (kills[0] > kills[1] && kills[0] > kills[2])
                {
                    killedby = "You died to the great crate bird!";
                }

                //mssg if you died to the gohst
                if (kills[1] > kills[0] && kills[1] > kills[2])
                {
                    killedby = "You died to the wandering ghost!";
                }

                //msg if you died to the turret
                if (kills[2] > kills[1] && kills[2] > kills[0])
                {
                    killedby = "You died to dwarf's contraption!";
                }
            }


            base.Update(gameTime);
        }
        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //Draw game if in menu
            if (gamestate == MENU)
            {
                //Draw BG
                spriteBatch.Draw(bgImg, bgBox, Color.White);
                spriteBatch.Draw(bgImg, bgBox2, Color.White);
                //Draw title
                spriteBatch.DrawString(spriteText, "The CRATE Escape!", titleLoc, Color.Firebrick);
                //Draw buttons
                spriteBatch.Draw(whiteimg, whiteBox[0], Color.Firebrick);
                spriteBatch.DrawString(buttonFont, "Start!", startLoc, Color.Black);
                spriteBatch.Draw(whiteimg, whiteBox[1], Color.Firebrick);
                spriteBatch.DrawString(buttonFont, "TOP SCORE", leaderboardLoc, Color.Black);
            }

            //leaderboard
            if (gamestate == TOP_SCORE)
            {
                //background
                spriteBatch.Draw(bgImg, bgBox, Color.Gray);
                spriteBatch.Draw(bgImg, bgBox2, Color.Gray);
                //buttons
                spriteBatch.Draw(whiteimg, whiteBox[4], Color.Firebrick);
                //text
                spriteBatch.DrawString(buttonFont, "Back", lbBackLoc, Color.Black);
                spriteBatch.DrawString(spriteText,"TOP SCORE \n \n" + topScore , lbLoc, Color.Firebrick);
            }


            //Draw game if in game
            if (gamestate == GAME)
            {
                //Draw Background
                spriteBatch.Draw(bgImg, bgBox, Color.White);
                spriteBatch.Draw(bgImg, bgBox2, Color.White);
                //Draw bird idle
                spriteBatch.Draw(birdImg, birdBox, birdSrcBox, Color.White);
                //Draw different chests
                spriteBatch.Draw(chestImgGold, chestBoxG, Color.White);
                spriteBatch.Draw(chestImgSilver, chestBoxS, Color.White);
                spriteBatch.Draw(chestImgBronze, chestBoxB, Color.White);
                //Draw power up
                spriteBatch.Draw(PUImg, PUBox, PUSrcBox, Color.White);
                //Draw turret obstacle
                spriteBatch.Draw(turretImg, turretBox, Color.White);
                //Draw turret broke
                spriteBatch.Draw(turretImgBroke, turretBoxBroke, Color.White);
                //Draw runner
                spriteBatch.Draw(runnerImg, runnerBox, runnerSrcBox, Color.White);
                //Draw power up
                spriteBatch.Draw(PUImg, PUBox, PUSrcBox, Color.White);
                //Draw crate obstacle
                spriteBatch.Draw(crateImg, crateBox, Color.White);
                spriteBatch.Draw(crateImgBroke, crateBoxBroke, Color.White);
                //Draw bullet
                spriteBatch.Draw(bulletImg, bulletBox[0], Color.White);
                spriteBatch.Draw(bulletImg, bulletBox[1], Color.White);
                spriteBatch.Draw(bulletImg, bulletBox[2], Color.White);
                //Draw ghost
                spriteBatch.Draw(ghostImg, ghostBox, ghostSrcBox, Color.White);
                //Draw SpriteFonts
                spriteBatch.DrawString(spriteText, "Score:" + score, scoreLoc, Color.Firebrick);
            }

            //if game is in deathscreen

            if (gamestate == DEATHSCREEN)
            {
                //Background in deathscreen
                spriteBatch.Draw(bgImg, bgBox, Color.Gray);
                spriteBatch.Draw(bgImg, bgBox2, Color.Gray);
                //you died
                spriteBatch.DrawString(spriteText, "You died!", titleLoc, Color.Firebrick);
                //Killed by message
                spriteBatch.DrawString(buttonFont, "" + killedby, killedByLoc, Color.Firebrick);
                //Text in deathscreen
                spriteBatch.Draw(whiteimg, whiteBox[2], Color.Firebrick);
                spriteBatch.DrawString(buttonFont, "Try Again", tryAgainLoc, Color.Black);
                spriteBatch.Draw(whiteimg, whiteBox[3], Color.Firebrick);
                spriteBatch.DrawString(buttonFont, "Main Menu", mainMenuLoc, Color.Black);
                spriteBatch.DrawString(buttonFont, "Press ESCAPE to quit game", quitLoc, Color.Firebrick);
                //Draw final score
                spriteBatch.DrawString(buttonFont, "Final score: " + score + "\nGold crates collected: " + chestsCollected[0] + "\nSilver crates collected : " + chestsCollected[1] + "\nBronze crates collected: " + chestsCollected[2], finalScoreLoc, Color.Firebrick);

            }

                
            //TEST!!!!!!!!!!
                //!!!!! TEST FONT !!!!!
            spriteBatch.DrawString(mousePX, "X:" + bulletBox[1].X + ", Y:" + bulletBox[1].Y + "" + "  " + turretShot + gamestate, mouseLoc, Color.Firebrick);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
