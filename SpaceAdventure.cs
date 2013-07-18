using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;


namespace SpaceAdventure
{
    public class SpaceAdventure : Microsoft.Xna.Framework.Game
    {
        #region Instance Variables
        /**Kinect and Gesture recognition**/
        private KinectSensor sensor;
        private Skeleton[] skeletons;
        private GestureRecognitionEngine gestureRecognition;

        /**Basic XNA Variables**/
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        /**Level components**/
        private Texture2D titleScreen;
        private Texture2D gameOver;
        private Texture2D ready;
        private Texture2D levelCompleted;
        private Level[] levels;
        private Level currentLevel;
        private Player player;
        private State gameState;

        /** Enemies **/
        private Enemy[] enemies;
        private Enemy activeEnemy;
        private Vector2 leftEnemyLocation;
        private Vector2 rightEnemyLocation;
        private Vector2 topEnemyLocation;

        /**Utilities**/
        private Random random;
        private SpriteFont font;
        private KeyboardState oldKeyboardState;
        private KeyboardState newKeyboardState;
        private Rectangle backgroundRectangle;

        /**Audio**/
        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;
        private Cue mainCue;
        private Cue soundEffectCue;
        private AudioEmitter emitter;
        private AudioListener listener;
        private SpeechSynthesizer speech;

        /**Health Bar**/
        Texture2D healthBar;
        Texture2D underlyingHealthBar;
        Rectangle healthBarRectangle;
        Rectangle healthBarBoundingRectangle;
        Rectangle healthBarUnderLayingRectangle;

        /** Game Menu options and logic **/
        MenuOption startGame;
        MenuOption options;
        MenuOption tutorial;
        MenuOption[] menuOptions;
        MenuOption currentSelection;

        /**Other Helper variables**/
        double oldTime; //helps compare the current time to a time in the pass to calculate elapsed time since an event
        double introDelay; //the time it takes for the intro to play before enemies should start appearing
        double timeGameStarted;
        double timeGameEnded;
        int soundEffectIndex;
        int menuOptionsIndex;
        int levelIndex;
        int gameDifficulty;
        const int ENEMY_LIMIT = 10; //number of enemies to beat in order to win the level
        const int MAX_HP = 100;
        int enemyIndex;

        #endregion
        public enum State
        {
            menu, starting, started, pause,tutorial, gameOver, levelCompleted, scoreScreen,
        }
        public SpaceAdventure()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsFixedTimeStep = false;
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;

            this.graphics.IsFullScreen = true;
            oldKeyboardState = Keyboard.GetState();
            gameState = State.menu;
        }

        protected override void Initialize()
        {
            base.Initialize();

        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            InitializeKinect();

            random = new Random();
            player = new Player();
            soundEffectIndex = 0;

            font = Content.Load<SpriteFont>("Font");
            titleScreen = Content.Load<Texture2D>("Backgrounds/title");
            timeGameStarted = 0;
            introDelay = 4500;
            oldTime = 0;
            backgroundRectangle = new Rectangle(0, 0, 800, 600);
            #region Level initialization

            ready = Content.Load<Texture2D>("Screen Messages/Ready");
            gameOver = Content.Load<Texture2D>("Screen Messages/GameOver");
            levelCompleted = Content.Load<Texture2D>("Screen Messages/LevelCompleted");
            Texture2D background1 = Content.Load<Texture2D>("Backgrounds/background1");
            Texture2D background2 = Content.Load<Texture2D>("Backgrounds/background2");
            Texture2D background3 = Content.Load<Texture2D>("Backgrounds/background3");

            levels = new Level[3];
            levels[0] = new Level(background1, "Level 1 Song");
            levels[1] = new Level(background2, "Level 2 Song");
            levels[2] = new Level(background3, "Level 3 Song");
            levelIndex = 0;

            gameDifficulty = 1; //default difficulty
            currentLevel = levels[levelIndex];
            #endregion
            player.attackSoundEffect = "Player Attack 3";
            player.missSoundEffect = "Player Miss";

            #region Menu initialization
            Texture2D startGameSelected = Content.Load<Texture2D>("Game Menu/startGameHighlighted");
            Texture2D startGameUnselected = Content.Load<Texture2D>("Game Menu/startGameOption");
            startGame = new MenuOption(startGameUnselected, startGameSelected);

            Texture2D optionsSelected = Content.Load<Texture2D>("Game Menu/optionsHighlighted");
            Texture2D optionsUnselected = Content.Load<Texture2D>("Game Menu/optionsOption");
            options = new MenuOption(optionsUnselected, optionsSelected);

            Texture2D tutorialSelected = Content.Load<Texture2D>("Game Menu/tutorialHighlighted");
            Texture2D tutorialUnselected = Content.Load<Texture2D>("Game Menu/tutorialOption");
            tutorial = new MenuOption(tutorialUnselected, tutorialSelected);
            menuOptions = new MenuOption[3];
            menuOptions[0] = startGame;
            menuOptions[1] = tutorial;
            menuOptions[2] = options;

            currentSelection = menuOptions[0];
            currentSelection.isSelected = true;
            menuOptionsIndex = 0;
            #endregion
            #region Audio Initialization
            audioEngine = new AudioEngine("Content/gameAudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content/Sound Bank.xsb");
            emitter = new AudioEmitter();
            listener = new AudioListener();
            speech = new SpeechSynthesizer();
            speech.Rate = 1;
            listener.Position = Vector3.UnitZ;
            startSound();
            #endregion
            #region Enemy Initialization
            Texture2D rightAttackImage = Content.Load<Texture2D>("Enemies/rightAttack");
            Texture2D rightStanceImage = Content.Load<Texture2D>("Enemies/rightStance");
            Texture2D leftAttackImage = Content.Load<Texture2D>("Enemies/leftAttack");
            Texture2D leftStanceImage = Content.Load<Texture2D>("Enemies/leftStance");
            Texture2D topAttackImage = Content.Load<Texture2D>("Enemies/topAttack2");
            Texture2D topStanceImage = Content.Load<Texture2D>("Enemies/topStance2");

            AnimatedSprite rightAttack = new AnimatedSprite(rightAttackImage, 1, 2, 2, 20);
            AnimatedSprite rightStance = new AnimatedSprite(rightStanceImage, 1, 3, 3, 10);
            AnimatedSprite leftAttack = new AnimatedSprite(leftAttackImage, 1, 2, 2, 20);
            AnimatedSprite leftStance = new AnimatedSprite(leftStanceImage, 1, 3, 3, 10);
            AnimatedSprite topAttack = new AnimatedSprite(topAttackImage, 1, 4, 4, 20);
            AnimatedSprite topStance = new AnimatedSprite(topStanceImage, 1, 3, 3, 10);

            leftEnemyLocation = new Vector2(-60, 270); //initial location, will move 50 pts X after
            rightEnemyLocation = new Vector2(560, 250);//initial location, will move 50 pts X after
            topEnemyLocation = new Vector2(250, -20);


            enemies = new Enemy[8];
            Enemy rightEnemy = new Enemy(AttackType.Right, rightStance, rightAttack, 20, "Right Attack", rightEnemyLocation, Color.White);
            Enemy leftEnemy = new Enemy(AttackType.Left, leftStance, leftAttack, 20, "Left Attack", leftEnemyLocation, Color.White);
            Enemy topEnemy = new Enemy(AttackType.Guard, topStance, topAttack, 20, "Top Attack", topEnemyLocation, Color.White);
            //Initialize Enemy sequence. By using a list with 30 Enemys you ensure that the results are more random
            for (int i = 0; i < enemies.Length; i++)
            {
                if (i % 7 == 0) enemies[i] = topEnemy;
                else if (i % 2 == 0) enemies[i] = rightEnemy;
                else enemies[i] = leftEnemy;
            }
            enemyIndex = 0;

            #endregion
            //Health bar
            healthBar = new Texture2D(GraphicsDevice, 1, 1);
            healthBar.SetData(new[] { Color.Red });
            underlyingHealthBar = new Texture2D(GraphicsDevice, 1, 1);
            underlyingHealthBar.SetData(new[] { Color.Yellow });
        }

        protected override void UnloadContent()
        {
            if (sensor != null)
                this.sensor.Stop();
        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            newKeyboardState = Keyboard.GetState();
            audioEngine.Update();
            #region Game State Switch
            switch (gameState)
            {
                #region game: Started
                case (State.started):
                    activeEnemy.UpdateStanceImage();
                    healthBarRectangle = new Rectangle(50, 20, player.HP, 20);

                    #region Enemy State Switch
                    switch (activeEnemy.state)
                    {
                        #region enemy: Actice
                        case (Enemy.State.active):
                            #region Trigger Attack
                            if ((gameTime.TotalGameTime.TotalMilliseconds - oldTime) >= currentLevel.enemyAttackDelay)
                            {
                                activeEnemy.state = Enemy.State.attacking;
                                soundBank.PlayCue(activeEnemy.attackSoundEffect);
                            }
                            #endregion
                            #region Attack: Left
                            if (oldKeyboardState.IsKeyUp(Keys.Left) && newKeyboardState.IsKeyDown(Keys.Left))
                            {
                                if (player.attack(activeEnemy, AttackType.Left)) // if attack is succesful
                                {
                                    soundBank.PlayCue(player.attackSoundEffect);
                                }
                                else
                                    soundBank.PlayCue(player.missSoundEffect);
                            }
                            #endregion
                            #region Attack: Right
                            else if (oldKeyboardState.IsKeyUp(Keys.Right) && newKeyboardState.IsKeyDown(Keys.Right))
                            {
                                if (player.attack(activeEnemy, AttackType.Right)) // if attack is succesful
                                {
                                    soundBank.PlayCue(player.attackSoundEffect);
                                }
                                else
                                    soundBank.PlayCue(player.missSoundEffect);
                            }
                            #endregion  Attack: Top
                            #region Attack: Up
                            else if (oldKeyboardState.IsKeyUp(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up))
                            {
                                if (player.attack(activeEnemy, AttackType.Guard)) // if attack is succesful
                                {
                                    soundBank.PlayCue(player.attackSoundEffect);
                                }
                                else
                                    soundBank.PlayCue(player.missSoundEffect);
                            }
                            break;

                            #endregion
                        #endregion
                        #region enemy: Inactive
                        case (Enemy.State.inactive):

                            if ((gameTime.TotalGameTime.TotalMilliseconds - oldTime) >= currentLevel.enemyEntranceDelay)
                            {
                                #region Check if Level is Over
                                if (player.enemiesEliminated == ENEMY_LIMIT)
                                {
                                    
                                    timeGameEnded = gameTime.TotalGameTime.TotalMilliseconds;
                                    gameState = State.levelCompleted;
                                    startSound();

                                    levelIndex = (levelIndex == 2) ? 0 : levelIndex + 1;

                                    //reset values
                                    player.HP = 0;
                                    enemyIndex = 0;
                                    player.enemiesEliminated = 0;

                                    startSound();
                                    break;
                                }
                                #endregion
                                activeEnemy = enemies[random.Next(8)];
                                enemyIndex++;
                                activeEnemy.state = Enemy.State.fadingIn;
                                #region Apply 3D sound effect
                                soundEffectIndex = (soundEffectIndex == 10) ? 1 : soundEffectIndex + 1;

                                soundEffectCue = (activeEnemy.attackType == AttackType.Guard) ? soundBank.GetCue("Top Entrance") :
                                    soundBank.GetCue("Robot Entrance " + soundEffectIndex.ToString());

                                float angle = 90;
                                if (activeEnemy.attackType == AttackType.Left) angle = radians(180); //PI * 1.5
                                else if (activeEnemy.attackType == AttackType.Right) angle = radians(0);
                                emitter.Position = calculateLocation(angle, 10f);
                                soundEffectCue.Apply3D(listener, emitter);
                                soundEffectCue.Play();
                                #endregion
                            }

                            break;
                        #endregion
                        #region enemy: Attacking
                        case (Enemy.State.attacking):
                            activeEnemy.attackImage.Update();
                            if (activeEnemy.attackImage.numberOfLoops > 0)
                            {
                                activeEnemy.attack(player);
                                activeEnemy.state = Enemy.State.fadingOut;
                                oldTime = gameTime.TotalGameTime.TotalMilliseconds;
                                activeEnemy.attackImage.resetNumberOfLoops();

                                /** Check if the game is over **/
                                if (player.HP <= 0)
                                {
                                    gameState = State.gameOver;
                                    startSound();
                                    timeGameEnded = gameTime.TotalGameTime.TotalMilliseconds;
                                }
                            }
                            break;
                        #endregion
                        #region enemy: Fading In
                        case (Enemy.State.fadingIn):

                            int movementOffset = 50; //How much the enemy moves when it comes into the screen
                            int topMovementOffset = 200; //How much the top enemy moves
                            if (activeEnemy.attackType == AttackType.Left)
                            {
                                activeEnemy.location.X += 2;
                                if ((activeEnemy.location.X - activeEnemy.originalLocationX) > movementOffset)
                                {
                                    activeEnemy.state = Enemy.State.active;
                                    oldTime = gameTime.TotalGameTime.TotalMilliseconds;
                                }
                            }
                            else if (activeEnemy.attackType == AttackType.Right)
                            {
                                activeEnemy.location.X -= 2;
                                if ((-activeEnemy.location.X + activeEnemy.originalLocationX) > movementOffset)
                                {
                                    oldTime = gameTime.TotalGameTime.TotalMilliseconds;
                                    activeEnemy.state = Enemy.State.active;
                                }
                            }
                            else if (activeEnemy.attackType == AttackType.Guard)
                            {
                                activeEnemy.location.Y += 3;
                                if ((activeEnemy.location.Y + activeEnemy.originalLocationY) > topMovementOffset)
                                {
                                    oldTime = gameTime.TotalGameTime.TotalMilliseconds;
                                    activeEnemy.state = Enemy.State.active;
                                }
                            }
                            break;
                        #endregion
                        #region enemy: Fading Out
                        case (Enemy.State.fadingOut):
                            movementOffset = 50; //How much the enemy moves when it comes into the screen
                            topMovementOffset = 200; //How much the top enemy moves

                            if (activeEnemy.attackType == AttackType.Left)
                            {
                                activeEnemy.location.X -= 7;
                                if (activeEnemy.location.X <= activeEnemy.originalLocationX)
                                    activeEnemy.state = Enemy.State.inactive;
                            }
                            else if (activeEnemy.attackType == AttackType.Right)
                            {
                                activeEnemy.location.X += 7;
                                if (activeEnemy.location.X >= activeEnemy.originalLocationX)
                                    activeEnemy.state = Enemy.State.inactive;
                            }
                            else if (activeEnemy.attackType <= AttackType.Guard)
                            {
                                activeEnemy.location.Y -= 11;
                                if (activeEnemy.location.Y <= activeEnemy.originalLocationY)
                                    activeEnemy.state = Enemy.State.inactive;
                            }
                            break;
                        #endregion
                        #region enemy: Destroyed
                        case (Enemy.State.destroyed):
                            Console.WriteLine("destroyed");
                            activeEnemy.color.A = (byte)(activeEnemy.color.A - 20);
                            if (activeEnemy.color.A <= 10)
                            {
                                oldTime = gameTime.TotalGameTime.TotalMilliseconds;
                                activeEnemy.state = Enemy.State.inactive;
                                activeEnemy.color.A = 255;
                                activeEnemy.moveBackToOrigin();
                            }
                            break;
                        #endregion
                    }
                    #endregion
                    break;
                #endregion
                #region game: Starting
                case (State.starting):
                    //animate healthbar
                    if (player.HP < MAX_HP) player.HP++;
                    healthBarRectangle = new Rectangle(50, 20, player.HP, 20);
                    healthBarBoundingRectangle = new Rectangle(48, 18, healthBarRectangle.Width + 3, 22);
                    //end animate

                    if ((gameTime.TotalGameTime.TotalMilliseconds - timeGameStarted) > introDelay)
                    {
                        gameState = State.started;
                        healthBarUnderLayingRectangle = new Rectangle(50, 20, player.HP, 20);
                    }
                    break;
                #endregion
                #region game: Tutorial
                case (State.tutorial):
                    if (soundEffectCue.IsStopped)
                        gameState = State.menu;
                    break;
                #endregion
                #region game: Menu
                case (State.menu):
                    #region Read Input
                    if (mainCue.IsStopped)
                        startSound();
                    if (oldKeyboardState.IsKeyUp(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up))
                    {
                        currentSelection.isSelected = false;
                        menuOptionsIndex = (menuOptionsIndex == 0) ? 2 : menuOptionsIndex - 1;
                        currentSelection = menuOptions[menuOptionsIndex];
                        currentSelection.isSelected = true;
                    }

                    else if (oldKeyboardState.IsKeyUp(Keys.Down) && newKeyboardState.IsKeyDown(Keys.Down))
                    {
                        currentSelection.isSelected = false;
                        menuOptionsIndex = (menuOptionsIndex == 2) ? 0 : menuOptionsIndex + 1;
                        currentSelection = menuOptions[menuOptionsIndex];
                        currentSelection.isSelected = true;
                    }
                    #endregion

                    if (newKeyboardState.IsKeyDown(Keys.Enter) && oldKeyboardState.IsKeyUp(Keys.Enter))
                    {
                        switch (menuOptionsIndex)
                        {
                            case 0: //start game

                                /** Set difficulty for all levels**/
                                for (int i = 0; i < 3; i++)
                                {
                                    levels[i].Difficulty = gameDifficulty + i;
                                }
                                currentLevel = levels[levelIndex];
                                activeEnemy = enemies[enemyIndex];
                                player.score = 0;
                                gameState = State.starting;
                                timeGameStarted = gameTime.TotalGameTime.TotalMilliseconds;
                                startSound();
                                break;
                            case 1: //tutorial
                                gameState = State.tutorial;
                                startSound();
                                soundEffectCue = soundBank.GetCue("Tutorial");
                                soundEffectCue.Play();
                                break;
                        }
                    }
                    break;
                #endregion
                #region game: Game Over
                case (State.gameOver):
                    if ((gameTime.TotalGameTime.TotalMilliseconds - timeGameEnded) > introDelay)
                    {
                        levelIndex = 0;
                        gameState = State.scoreScreen;
                        player.score *= 85; //85 is an arbitrary number
                        speech.Speak("Your score is: " + player.score.ToString());
                    }
                    break;
                #endregion
                #region game: Level Completed
                case (State.levelCompleted):
                    if (newKeyboardState.IsKeyDown(Keys.Enter))
                    {
                        if (levelIndex == 0)
                        {
                            gameState = State.scoreScreen;
                            int choice = random.Next(3);
                            if (choice == 0)
                                player.score *= 88;
                            if (choice == 1)
                                player.score *= 78;
                            if (choice == 2)
                                player.score *= 95;

                            speech.Speak("Your score is: " + player.score.ToString());
                        }
                        else
                        {
                            currentLevel = levels[levelIndex];
                            timeGameStarted = gameTime.TotalGameTime.TotalMilliseconds;
                            gameState = State.starting;
                            player.score += player.HP * 105;
                            startSound();
                        }
                    }
                    break;
                #endregion
                #region game: Score Screen
                case (State.scoreScreen):
                    //Calculate score
                    //Bonus for any remaining hp: 55 and 85 are just arbitrary numbers to bloat the score
                    if (newKeyboardState.IsKeyDown(Keys.Enter) && oldKeyboardState.IsKeyUp(Keys.Enter))
                    {

                        gameState = State.menu;
                        timeGameStarted = gameTime.TotalGameTime.TotalMilliseconds;
                        startSound();
                    }
                    break;
                #endregion
            }
            #endregion

            oldKeyboardState = newKeyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            base.Draw(gameTime);
            spriteBatch.Begin();
            #region Game State
            switch (gameState)
            {
                case (State.started):
                    spriteBatch.Draw(currentLevel.bkgdImage, backgroundRectangle, Color.White);
                    spriteBatch.Draw(underlyingHealthBar, healthBarUnderLayingRectangle, Color.Yellow);
                    spriteBatch.Draw(healthBar, healthBarRectangle, Color.Red);
                    DrawHollowRectangle(healthBar, healthBarBoundingRectangle, 3, Color.Black);
                    #region Enemy State Switch
                    switch (activeEnemy.state)
                    {
                        case (Enemy.State.attacking):
                            activeEnemy.drawAttackImage(spriteBatch);
                            break;
                        case (Enemy.State.active):
                            activeEnemy.drawStanceImage(spriteBatch);
                            break;
                        case (Enemy.State.inactive):
                            //Do nothing
                            break;
                        default:
                            activeEnemy.drawStanceImage(spriteBatch);
                            break;
                    }
                    #endregion
                    break;
                case (State.menu):
                    spriteBatch.Draw(titleScreen, backgroundRectangle, Color.White);
                    menuOptions[0].Draw(spriteBatch, new Vector2(100, 300));
                    menuOptions[1].Draw(spriteBatch, new Vector2(100, 350));
                    break;
                case (State.tutorial):
                    spriteBatch.Draw(titleScreen, backgroundRectangle, Color.White);
                    spriteBatch.DrawString(font, "Tutorial Mode", new Vector2(300, 300), Color.White);
                    break;

                case (State.starting):
                    spriteBatch.Draw(currentLevel.bkgdImage, backgroundRectangle, Color.White);
                    spriteBatch.Draw(healthBar, healthBarRectangle, Color.Red);
                    if ((gameTime.TotalGameTime.TotalMilliseconds - timeGameStarted) > 3300)
                        spriteBatch.Draw(ready, new Vector2(200, 50), Color.White);
                    DrawHollowRectangle(healthBar, healthBarBoundingRectangle, 3, Color.Black);
                    break;
                case (State.gameOver):
                    spriteBatch.Draw(currentLevel.bkgdImage, backgroundRectangle, Color.Gray);
                    spriteBatch.Draw(gameOver, new Vector2(70, 50), Color.White);
                    break;
                case (State.levelCompleted):
                    spriteBatch.Draw(currentLevel.bkgdImage, backgroundRectangle, Color.Silver);
                    spriteBatch.Draw(levelCompleted, new Vector2(3, 100), Color.White);
                    spriteBatch.DrawString(font, "Press <Enter> to Continue", new Vector2(300, 300), Color.White);
                    break;
                case (State.scoreScreen):
                    spriteBatch.Draw(currentLevel.bkgdImage, backgroundRectangle, Color.Gray);
                    spriteBatch.DrawString(font, "Score: " + player.score, new Vector2(300, 200), Color.White);
                    break;
            }
            #endregion
            spriteBatch.End();
        }

        protected void startSound()
        {
            String song = " ";
            if (mainCue != null)
                mainCue.Stop(AudioStopOptions.AsAuthored);
            switch (gameState)
            {
                case (State.menu):
                    song = "Title Screen Song";
                    break;
                case (State.starting):
                    song = currentLevel.bkgdSong;
                    break;
                case (State.gameOver):
                    song = "GameOver";
                    break;
                case (State.levelCompleted):
                    song = "Level Completed";
                    break;
                case (State.tutorial):
                    song = "Tutorial Song";
                    break;
            }
            mainCue = soundBank.GetCue(song);
            mainCue.Play();
        }

        protected void DrawHollowRectangle(Texture2D pixel, Rectangle rectangle, int thicknessOfBorder, Color borderColor)
        {
            // Draw top line 
            spriteBatch.Draw(pixel, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thicknessOfBorder), borderColor);

            // Draw left line 
            spriteBatch.Draw(pixel, new Rectangle(rectangle.X, rectangle.Y, thicknessOfBorder, rectangle.Height), borderColor);

            // Draw right line 
            spriteBatch.Draw(pixel, new Rectangle((rectangle.X + rectangle.Width - thicknessOfBorder),
            rectangle.Y,
            thicknessOfBorder,
            rectangle.Height), borderColor);
            // Draw bottom line 
            spriteBatch.Draw(pixel, new Rectangle(rectangle.X,
            rectangle.Y + rectangle.Height - thicknessOfBorder,
            rectangle.Width,
            thicknessOfBorder), borderColor);
        }

        protected void InitializeKinect()
        {

            this.sensor = KinectSensor.KinectSensors[0];
            try
            {
                this.sensor.Start();
            }
            catch (IOException)
            {
                this.sensor = null;
            }

            if (null != this.sensor)
            {
                this.sensor.SkeletonStream.Enable();
                this.sensor.SkeletonFrameReady += sensor_SkeletonFrameReady;
            }

            gestureRecognition = new GestureRecognitionEngine();
            gestureRecognition.GestureRecognized += new EventHandler<GestureEventArgs>(recognitionEngine_GestureRecognized);


        }

        protected void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton firstSkeleton = null;
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                    firstSkeleton = (from trackskeleton in skeletons
                                     where trackskeleton.TrackingState == SkeletonTrackingState.Tracked
                                     select trackskeleton).FirstOrDefault();
                }
            }

            if (firstSkeleton != null)
            {
                gestureRecognition.Skeleton = firstSkeleton;

                gestureRecognition.StartRecognize();
            }
        }

        protected void recognitionEngine_GestureRecognized(object sender, GestureEventArgs e)
        {
            AttackType attack = AttackType.Null;
            if (activeEnemy.state == Enemy.State.active)
            {
                switch (e.GestureType)
                {
                    case (GestureType.LeftSwordSlash):
                        attack = AttackType.Left;
                        break;
                    case (GestureType.RightSwordSlash):
                        attack = AttackType.Right;
                        break;
                    case (GestureType.LeftGuard):
                        attack = AttackType.Guard;
                        break;
                    case (GestureType.RightGuard):
                        attack = AttackType.Guard;
                        break;
                }
            }
            if (player.attack(activeEnemy, attack)) // if attack is succesful
                soundBank.PlayCue(player.attackSoundEffect);
        }

        private Vector3 calculateLocation(float angle, float distance)
        {
            return new Vector3((float)Math.Cos(angle) * distance, 0, (float)Math.Sin(angle) * distance);
        }

        private float radians(float degrees)
        {
            return (float)(degrees * (Math.PI / 180));
        }


    }
}
