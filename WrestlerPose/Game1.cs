using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using WrestlerPose.Models;
using WrestlerPose.Sprites;

namespace WrestlerPose
{
    public class Game1 : Game
    {
        Vector2 WrestlerPosition1;
        Vector2 WrestlerPosition2;
        Vector2 AIPosition;

        Player player1;
        Player player2;
        Player currentAI;

        Song song;

        List<SoundEffect> soundEffects;
        List<Player> AIPlayerList;// = new List<Player>(3);
        const int numAnimations = 24;

        List<Animation> animations = new List<Animation>(numAnimations);
        List<Animation> outcomeAnimations = new List<Animation>(4);
        List<Sprite> displayCircles = new List<Sprite>(6);
        List<Pose> poses = new List<Pose>(numAnimations);
        List<Pose> outComePoses = new List<Pose>(4);

        bool dontDisplayOutcome = true;
        bool playerOneFinishedFirst = true;


        //countdown timer
        //i got this counter timer basic setup from online somewhere but changed it to count down and not up
        //here https://stackoverflow.com/questions/13394892/how-to-create-a-timer-counter-in-c-sharp-xna
        //player 1 counter
        int counter = 3;
        int counterStart = 3;
        float countDuration = .4f;
        float currentTime = 0f;

        //player 2 counter:
        int counter2 = 3;
        int counterStart2 = 3;
        float countDuration2 = .4f;
        float currentTime2 = 0f;

        //ai round timer counter:
        int counterAI = 3;//this should be set to the time for the first animation of the first ai to run, or to run multiple times i guess
        int counterStartAI = 10;
        float countDurationAI = .6f;
        float currentTimeAI = 0f;

        float roundTimer = 0;
        int roundNumber = 1;
        string overallWinnerString = "";
        int matchNumber = 1;

        //private int numAiPosesThisRound = 3;
        bool playerTurn = false;
        bool aiTurn = false;//game not starting because of this? **need to set to true after intro turn
        bool introTurn = true;
        int numPosesDisplayedAI = 0;
        bool thisRoundTallied = false;
        bool player1CanInput = true;
        bool player2CanInput = true;

        bool introPlayer1AudioHasPlayed = false;
        bool introPlayer2AudioHasPlayed = false;
        bool introAIAudioHasPlayed = false;


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _countDownPlayerOneMove;
        private SpriteFont _countDownPlayerTwoMove;
        private SpriteFont _countDownAI;
        private SpriteFont _playerOneOutcome;
        private SpriteFont _playerTwoOutcome;
        private SpriteFont _playerOneScore;
        private SpriteFont _playerTwoScore;
        private SpriteFont _playerOneRoundScore;
        private SpriteFont _playerTwoRoundScore;
        private SpriteFont _playerOneMatchScore;
        private SpriteFont _playerTwoMatchScore;
        private SpriteFont _round;
        private SpriteFont _match;
        private SpriteFont _overAllWinner;
        private SpriteFont _title;
        private Texture2D _allPosesImage;
        private Texture2D _stageBackground;

        private List<Texture2D> playerOneSelectedPoseSpritesToChooseFrom = new List<Texture2D>(5);//assumes the most will be 5 
        private List<Texture2D> playerTwoSelectedPoseSpritesToChooseFrom = new List<Texture2D>(5);//assumes the most will be 5

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            soundEffects = new List<SoundEffect>();
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            int test = _graphics.PreferredBackBufferWidth;
            int test2 = _graphics.PreferredBackBufferHeight;

            WrestlerPosition1 = new Vector2(3450, 700);
            WrestlerPosition2 = new Vector2(4650, 700);
            //AIPosition = new Vector2(3250, 300);//this was about in the middle, walkway not centered though
            AIPosition = new Vector2(3310, 400);


            base.Initialize();
        }

        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            MediaPlayer.Play(song);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _countDownPlayerOneMove = Content.Load<SpriteFont>("Countdown");
            _countDownPlayerTwoMove = Content.Load<SpriteFont>("Countdown");
            _countDownAI = Content.Load<SpriteFont>("Countdown");

            _playerOneOutcome = Content.Load<SpriteFont>("PlayerOneOutcome");
            _playerTwoOutcome = Content.Load<SpriteFont>("PlayerTwoOutcome");
            _playerOneScore = Content.Load<SpriteFont>("PlayerOneScore");
            _playerTwoScore = Content.Load<SpriteFont>("PlayerTwoScore");
            _playerOneRoundScore = Content.Load<SpriteFont>("PlayerOneRoundScore");
            _playerTwoRoundScore = Content.Load<SpriteFont>("PlayerTwoRoundScore");
            _playerOneMatchScore = Content.Load<SpriteFont>("PlayerOneMatchScore");
            _playerTwoMatchScore = Content.Load<SpriteFont>("PlayerTwoMatchScore");
            _round = Content.Load<SpriteFont>("Round");
            _match = Content.Load<SpriteFont>("Match");
            _overAllWinner = Content.Load<SpriteFont>("OverallWinner");
            _title = Content.Load<SpriteFont>("Title");
            _allPosesImage = Content.Load<Texture2D>("posechart1");
            _stageBackground = Content.Load<Texture2D>("main_stage_plane_audience");

            song = Content.Load<Song>("Sound/theme_background");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
            soundEffects.Add(Content.Load<SoundEffect>("Sound/shout6"));
            soundEffects.Add(Content.Load<SoundEffect>("Sound/shout2"));
            soundEffects.Add(Content.Load<SoundEffect>("Sound/shout3"));
            soundEffects.Add(Content.Load<SoundEffect>("Sound/shout4"));
            soundEffects.Add(Content.Load<SoundEffect>("Sound/shout5"));
            soundEffects.Add(Content.Load<SoundEffect>("Sound/boxing_bell"));
            SoundEffect.MasterVolume = 0.5f;

            soundEffects[5].CreateInstance().Play();//do I need to create instance when doing this? if I don't I think taht the same sound will stop itself if called before finishing

            // Play that can be manipulated after the fact
            //var instance = soundEffects[0].CreateInstance();
            //instance.IsLooped = true;
            //instance.Play();
            //end sound

            List<string> stillAnimationImageNameStrings = new List<string>(5) { "twodownclear", "pointingclear", "oneupclear", "twoupclear", "herculesclear" };
            for (int i = 0; i < 5; i++)
            {
                playerOneSelectedPoseSpritesToChooseFrom.Add(Content.Load<Texture2D>(stillAnimationImageNameStrings[i]));
                playerTwoSelectedPoseSpritesToChooseFrom.Add(Content.Load<Texture2D>(stillAnimationImageNameStrings[i]));
            }

            float displayCircleLayer = 0.0f;
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("bluefeet"), 3), 2.5f, displayCircleLayer));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("greenfeetthree"), 3), 2.5f, displayCircleLayer));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("redfeet"), 3), 2.5f, displayCircleLayer));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("bluefeet"), 3), 2.5f, displayCircleLayer));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("greenfeetthree"), 3), 2.5f, displayCircleLayer));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("redfeet"), 3), 2.5f, displayCircleLayer));

            /*
             LowHands,
            Pointing,
            OneHandUp,
            HighHands,
            Hercules
             */

            //DOUBled number of animations because same one can't be in two places it seems and
            //will need different ones anyway once getting new characters etc...
            //player one animations:
            animations.Add(new Animation(Content.Load<Texture2D>("idle"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("yellow&crouch"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("purple&lean"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("blue&point"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("red&botharms"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("green&back"), 12));
            //player 2 animations
            animations.Add(new Animation(Content.Load<Texture2D>("idle"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("yellow&crouch"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("purple&lean"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("blue&point"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("red&botharms"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("green&back"), 12));
            //AI 1 animations
            animations.Add(new Animation(Content.Load<Texture2D>("idlealt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("yellow&couch-alt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("purple&lean-alt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("blue&point-alt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("red&botharms-alt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("green&back-alt"), 12));
            //AI 2 animations
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/bear-idle"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/bear-yeller"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/bear-purple"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/bear-blue"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/bear-red"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/bear-green"), 12));

            outcomeAnimations.Add(new Animation(Content.Load<Texture2D>("win"), 12));
            outcomeAnimations.Add(new Animation(Content.Load<Texture2D>("lose"), 12));
            outcomeAnimations.Add(new Animation(Content.Load<Texture2D>("win"), 12));
            outcomeAnimations.Add(new Animation(Content.Load<Texture2D>("lose"), 12));

            outComePoses.Add(new Pose(outcomeAnimations[0], PoseName.Idle, 2, 0.9f));//scalse is 2 for player, does posename matter here though?
            outComePoses.Add(new Pose(outcomeAnimations[1], PoseName.Idle, 2, 0.9f));//scalse is 2 for player, does posename matter here though?
            outComePoses.Add(new Pose(outcomeAnimations[2], PoseName.Idle, 2, 0.9f));//scalse is 2 for player, does posename matter here though?
            outComePoses.Add(new Pose(outcomeAnimations[3], PoseName.Idle, 2, 0.9f));//scalse is 2 for player, does posename matter here though?

            for (int i = 0; i < numAnimations; i++)
            {
                int poseInt = i;
                float scale = 2f;
                //should make this a non-arbitrary number later
                if (i > 5)
                {
                    poseInt = poseInt - 6;
                }
                if (i > 11)
                {
                    poseInt = poseInt - 6;
                    scale = 1.5f;
                }
                if (i > 17)
                {
                    poseInt = poseInt - 6;
                    scale = 1.5f;
                }
                poses.Add(new Pose(animations[i], (PoseName)poseInt, scale, 0.9f));
            }

            player1 = new Player("Player One", WrestlerPosition1, poses[0], new List<Pose>());
            player2 = new Player("Player Two", WrestlerPosition2, poses[6], new List<Pose>());

            //the new pose list with poses below doesn't really matter because they are later randomized based on the in list after it
            AIPlayerList = new List<Player>
            {
                //the 3rd parameter is the idle parameter for that ai, we now have 2, and idle for alt and bear are 12 and 18 respectively
                new Player("firstAI", AIPosition, poses[12], new List<Pose>(3) { poses[13], poses[14], poses[14] }, new List<int>{ 13, 14, 15}),
                new Player("secondAI", AIPosition, poses[18], new List<Pose>(4) { poses[19], poses[19], poses[19], poses[19] }, new List<int>{ 19, 20, 21, 22}),
                new Player("thirdAI", AIPosition, poses[18], new List<Pose>(5) { poses[19], poses[19], poses[19], poses[19], poses[19] }, new List<int>{ 19, 22, 23, 21, 20}),
            };

            currentAI = AIPlayerList[0];
        }

        protected override void Update(GameTime gameTime)
        {
            //this is a default condition, currently only player 1 controller can exit game?
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            StickDirection playerOneLeftStick = StickDirection.None;
            StickDirection playerOneRightStick = StickDirection.None;
            StickDirection playerTwoLeftStick = StickDirection.None;
            StickDirection playerTwoRightStick = StickDirection.None;

            //setting up player inputs, shouldn't need to change with addition of AI because can just dictate those in code witout gamepad input
            for (int i = 0; i < 2; i++)
            {
                PlayerIndex playerIndex = (PlayerIndex)i;
                GamePadCapabilities capabilities = GamePad.GetCapabilities(playerIndex);

                if (capabilities.IsConnected)
                {
                    GamePadState state = GamePad.GetState(playerIndex);
                    Player player = player1;

                    if (i == 1)
                    {
                        player = player2;
                    }

                    if (capabilities.HasLeftXThumbStick && capabilities.HasLeftYThumbStick)
                    {
                        if (state.ThumbSticks.Left.X < -0.5f)
                        {
                            player.SetLeftStickDirection(StickDirection.Left);
                        }
                        else if (state.ThumbSticks.Left.X > 0.5f)
                        {
                            player.SetLeftStickDirection(StickDirection.Right);
                        }
                        else if (state.ThumbSticks.Left.Y < -0.5f)
                        {
                            player.SetLeftStickDirection(StickDirection.Down);
                        }
                        else if (state.ThumbSticks.Left.Y > 0.5f)
                        {
                            player.SetLeftStickDirection(StickDirection.Up);
                        }
                        else
                        {
                            player.SetLeftStickDirection(StickDirection.None);
                        }
                    }

                    if (capabilities.HasRightXThumbStick && capabilities.HasRightYThumbStick)
                    {
                        if (state.ThumbSticks.Right.X < -0.5f)
                        {
                            player.SetRightStickDirection(StickDirection.Left);
                        }
                        else if (state.ThumbSticks.Right.X > 0.5f)
                        {
                            player.SetRightStickDirection(StickDirection.Right);
                        }
                        else if (state.ThumbSticks.Right.Y < -0.5f)
                        {
                            player.SetRightStickDirection(StickDirection.Down);
                        }
                        else if (state.ThumbSticks.Right.Y > 0.5f)
                        {
                            player.SetRightStickDirection(StickDirection.Up);
                        }
                        else
                        {
                            player.SetLeftStickDirection(StickDirection.None);
                        }
                    }

                    if (i == 0)
                    {
                        playerOneLeftStick = player.GetLeftStickDirection();
                        playerOneRightStick = player.GetRightStickDirection();
                    }
                    else
                    {
                        playerTwoLeftStick = player.GetLeftStickDirection();
                        playerTwoRightStick = player.GetRightStickDirection();
                    }
                }

            }

            var inputState = Keyboard.GetState();

            if (introTurn)
            {
                //input:
                player1CanInput = false;
                player2CanInput = false;
                dontDisplayOutcome = false;//might need to get reset to true after? see what it is initially


               


                roundTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (roundTimer > 2000)
                {
                    //could make it false initially and then not have to do it here
                    //also add in the changing player lights here
                    if (!introAIAudioHasPlayed)
                    {
                        currentAI.SetPose(poses[13]);
                        soundEffects[1].CreateInstance().Play();
                        introAIAudioHasPlayed = true;
                    }

                    if (roundTimer > 4000)
                    {
                        if (!introPlayer1AudioHasPlayed)
                        {
                            player1.SetPose(poses[2]);
                            soundEffects[2].CreateInstance().Play();
                            introPlayer1AudioHasPlayed = true;
                            player1.displayCircle = DisplayCircle.Lost;
                        }

                        if (roundTimer > 4500)
                        {

                            if (!introPlayer2AudioHasPlayed)
                            {
                                player2.SetPose(poses[9]);
                                soundEffects[3].CreateInstance().Play();
                                introPlayer2AudioHasPlayed = true;
                                player2.displayCircle = DisplayCircle.Won;
                            }

                            if (roundTimer > 7000)
                            {
                                player1.SetPose(outComePoses[1]);
                                player2.SetPose(outComePoses[2]);
                                dontDisplayOutcome = true;

                                if (roundTimer > 10000)
                                { 
                                    currentAI.SetPose(poses[12]);
                                    player1.SetPose(poses[0]);
                                    player2.SetPose(poses[6]);
                                    player1.displayCircle = DisplayCircle.Tied;
                                    player2.displayCircle = DisplayCircle.Tied;
                                    //and now start the game
                                    aiTurn = true;
                                    playerTurn = false;
                                    roundTimer = 0;
                                    introTurn = false;
                                }
                            }
                        }
                    }
                }
            }

            //below is kind of awkward and not very safe, just a bunch of conditionals with hard coded indexes that incidentally correspond to poses
            //along with hard coded input, is it worth it to do some input refactoring, so the keyboard input state is not directly exposed
            //here but instead there's some layer of abstraction that handles input more elegantly? may not be worth doing for prototype
            if (playerTurn)
            {
                currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentTime >= countDuration)
                {
                    counter--;
                    currentTime -= countDuration;
                }
                if (counter < 0)
                {
                    //counter = counterStart;
                    counter = 0;
                    player1CanInput = true;
                }

                if (player1.GetPosePattern().Count < currentAI.GetPosePattern().Count && player1CanInput)
                {
                    if (inputState.IsKeyDown(Keys.A) || ((playerOneLeftStick == StickDirection.Down) && (playerOneRightStick == StickDirection.Down)))
                    {
                        player1.SetPose(poses[1]);
                        player1.AddToPosePattern(poses[1]);
                        ComparePosesAndSetScores(player1.GetPosePattern().Count - 1, player1, currentAI);
                        soundEffects[0].CreateInstance().Play();
                        player1CanInput = false;
                        counter = counterStart;

                    }
                    else if (inputState.IsKeyDown(Keys.S) || ((playerOneLeftStick == StickDirection.Up) && (playerOneRightStick == StickDirection.Up)))
                    {
                        player1.SetPose(poses[4]);//*** two hands up
                        player1.AddToPosePattern(poses[4]);
                        ComparePosesAndSetScores(player1.GetPosePattern().Count - 1, player1, currentAI);
                        soundEffects[3].CreateInstance().Play();
                        player1CanInput = false;
                        counter = counterStart;

                    }
                    else if (inputState.IsKeyDown(Keys.D) || ((playerOneLeftStick == StickDirection.Up) && (playerOneRightStick == StickDirection.Down)))
                    {
                        player1.SetPose(poses[3]);
                        player1.AddToPosePattern(poses[3]);
                        ComparePosesAndSetScores(player1.GetPosePattern().Count - 1, player1, currentAI);
                        soundEffects[2].CreateInstance().Play();
                        player1CanInput = false;
                        counter = counterStart;

                    }
                    else if (inputState.IsKeyDown(Keys.F) || ((playerOneLeftStick == StickDirection.Up) && (playerOneRightStick == StickDirection.Right)))
                    {
                        player1.SetPose(poses[2]); //***pointing
                        player1.AddToPosePattern(poses[2]);
                        ComparePosesAndSetScores(player1.GetPosePattern().Count - 1, player1, currentAI);
                        soundEffects[1].CreateInstance().Play();
                        player1CanInput = false;
                        counter = counterStart;

                    }
                    else if (inputState.IsKeyDown(Keys.G) || ((playerOneLeftStick == StickDirection.Left) && (playerOneRightStick == StickDirection.Right)))
                    {
                        player1.SetPose(poses[5]);
                        player1.AddToPosePattern(poses[5]);
                        ComparePosesAndSetScores(player1.GetPosePattern().Count - 1, player1, currentAI);
                        soundEffects[4].CreateInstance().Play();
                        player1CanInput = false;
                        counter = counterStart;

                    }
                }

                currentTime2 += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentTime2 >= countDuration2)
                {
                    counter2--;
                    currentTime2 -= countDuration2;
                }
                if (counter2 < 0)
                {
                    counter2 = 0;
                    player2CanInput = true;
                }

                if (player2.GetPosePattern().Count < currentAI.GetPosePattern().Count && player2CanInput)
                {
                    if (inputState.IsKeyDown(Keys.NumPad1) || ((playerTwoLeftStick == StickDirection.Down) && (playerTwoRightStick == StickDirection.Down)))
                    {
                        player2.SetPose(poses[7]);
                        player2.AddToPosePattern(poses[7]);
                        ComparePosesAndSetScores(player2.GetPosePattern().Count - 1, player2, currentAI);
                        soundEffects[0].CreateInstance().Play();
                        player2CanInput = false;
                        counter2 = counterStart2;
                    }
                    else if (inputState.IsKeyDown(Keys.NumPad2) || ((playerTwoLeftStick == StickDirection.Up) && (playerTwoRightStick == StickDirection.Up)))
                    {
                        player2.SetPose(poses[10]);//**two hands up
                        player2.AddToPosePattern(poses[10]);
                        ComparePosesAndSetScores(player2.GetPosePattern().Count - 1, player2, currentAI);
                        soundEffects[3].CreateInstance().Play();
                        player2CanInput = false;
                        counter2 = counterStart2;
                    }
                    else if (inputState.IsKeyDown(Keys.NumPad3) || ((playerTwoLeftStick == StickDirection.Up) && (playerTwoRightStick == StickDirection.Down)))
                    {
                        player2.SetPose(poses[9]);
                        player2.AddToPosePattern(poses[9]);
                        ComparePosesAndSetScores(player2.GetPosePattern().Count - 1, player2, currentAI);
                        soundEffects[2].CreateInstance().Play();
                        player2CanInput = false;
                        counter2 = counterStart2;
                    }
                    else if (inputState.IsKeyDown(Keys.NumPad4) || ((playerTwoLeftStick == StickDirection.Up) && (playerTwoRightStick == StickDirection.Right)))
                    {
                        player2.SetPose(poses[8]);//**pointing
                        player2.AddToPosePattern(poses[8]);
                        ComparePosesAndSetScores(player2.GetPosePattern().Count - 1, player2, currentAI);
                        soundEffects[1].CreateInstance().Play();//if I want these sounds to consistent with the ai sounds then I should swap the index here and with two hands up maybe
                        player2CanInput = false;
                        counter2 = counterStart2;
                    }
                    else if (inputState.IsKeyDown(Keys.NumPad5) || ((playerTwoLeftStick == StickDirection.Left) && (playerTwoRightStick == StickDirection.Right)))
                    {
                        player2.SetPose(poses[11]);
                        player2.AddToPosePattern(poses[11]);
                        ComparePosesAndSetScores(player2.GetPosePattern().Count - 1, player2, currentAI);
                        soundEffects[4].CreateInstance().Play();
                        player2CanInput = false;
                        counter2 = counterStart2;
                    }
                }

                if ((player1.GetPosePattern().Count >= currentAI.GetPosePattern().Count) && (player2.GetPosePattern().Count < currentAI.GetPosePattern().Count))
                {
                    playerOneFinishedFirst = true;
                }
                else if ((player2.GetPosePattern().Count >= currentAI.GetPosePattern().Count) && (player1.GetPosePattern().Count < currentAI.GetPosePattern().Count))
                {
                    playerOneFinishedFirst = false;
                }


                if ((player1.GetPosePattern().Count >= currentAI.GetPosePattern().Count) && (player2.GetPosePattern().Count >= currentAI.GetPosePattern().Count))
                {
                    roundTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    thisRoundTallied = true;

                    if (roundTimer > 3000)
                    {
                        dontDisplayOutcome = true;

                        if (player1.GetScore() > player2.GetScore())
                        {
                            player1.SetPose(outComePoses[0]);
                            player2.SetPose(outComePoses[3]);
                        }
                        else if (player2.GetScore() > player1.GetScore())
                        {
                            player1.SetPose(outComePoses[1]);
                            player2.SetPose(outComePoses[2]);
                        }
                        else
                        {
                            if (playerOneFinishedFirst)
                            {
                                player1.SetPose(outComePoses[0]);
                                player2.SetPose(outComePoses[3]);
                            }
                            else
                            {
                                player1.SetPose(outComePoses[1]);
                                player2.SetPose(outComePoses[2]);
                            }
                        }



                        if (roundTimer > 5000)
                        {
                            dontDisplayOutcome = false;
                            playerTurn = false;
                            roundTimer = 0;
                            NewRound();
                        }
                    }
                }
            }
            player1.GetPose().GetSprite().Update(gameTime, player1.GetPosition());
            player2.GetPose().GetSprite().Update(gameTime, player2.GetPosition());


            for (int i = 0; i < 3; i++)
            {
                displayCircles[i].Update(gameTime, new Vector2(750, 1000));
            }
            for (int i = 3; i < 6; i++)
            {
                displayCircles[i].Update(gameTime, new Vector2(1950, 1000));
            }

            if (aiTurn)
            {
                dontDisplayOutcome = true;

                if (numPosesDisplayedAI < currentAI.GetPosePattern().Count)
                {

                    currentTimeAI += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (currentTimeAI >= countDurationAI)
                    {
                        counterAI--;
                        currentTimeAI -= countDurationAI;
                    }
                    if (counterAI < 0)
                    {
                        Random rnd = new Random();
                        int randomPoseIndexFromThisAisPoseNumberslist = rnd.Next(0, currentAI._poseValuesForThisAI.Count);
                        int poseValueFromThisAisRandomSelection = currentAI._poseValuesForThisAI[randomPoseIndexFromThisAisPoseNumberslist];
                        currentAI.SetPose(poses[poseValueFromThisAisRandomSelection]);
                        currentAI.SetPosePattern(numPosesDisplayedAI, poses[poseValueFromThisAisRandomSelection]);
                        soundEffects[randomPoseIndexFromThisAisPoseNumberslist].CreateInstance().Play();
                        //then wait for the amount of time it takes to play the animation, which is gonna be framespeed * frame count
                        float animationTime = currentAI.GetPosePattern()[numPosesDisplayedAI].GetSprite().GetAnimationTime();
                        int roundedUp = (int)Math.Ceiling(animationTime);
                        counterAI = roundedUp * 2;//set it to play that animation twice basically? well rounded upa nd doubled would be more than twice, maybe many times more
                        numPosesDisplayedAI++;
                    }

                }

                if (numPosesDisplayedAI >= currentAI.GetPosePattern().Count)
                {
                    roundTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (roundTimer > 3000)
                    {
                        //should probably not do this upIndexCurrentAIIdlePoseIndex bit and instead have some kind of currentai.setidlepose method that can more reliable find this
                        //and is done in one place
                        int upIndexCurrentAIIdlePoseIndex = (matchNumber - 1) * 6 + 12;
                        currentAI.SetPose(poses[upIndexCurrentAIIdlePoseIndex]);
                        aiTurn = false;
                        playerTurn = true;
                        dontDisplayOutcome = false;
                        roundTimer = 0;
                    }
                }
            }

            currentAI.GetPose().GetSprite().Update(gameTime, currentAI.GetPosition());

            base.Update(gameTime);
        }

        void ComparePosesAndSetScores(int posePatternIndex, Player player, Player currentAI)
        {
            Player winnerBetweenPlayerTwoAndAI = ComparePoses(player, currentAI, posePatternIndex);
            if (winnerBetweenPlayerTwoAndAI != null)
            {
                if (winnerBetweenPlayerTwoAndAI == player)
                {
                    player.SetScore(player.GetScore() + 1);
                    player.displayCircle = DisplayCircle.Won;
                }
                else
                {
                    player.SetScore(player.GetScore() - 1);
                    player.displayCircle = DisplayCircle.Lost;
                }
            }
            else
            {
                player.displayCircle = DisplayCircle.Tied;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack, null);

            _spriteBatch.Draw(
                    _stageBackground,
                    new Vector2(960, 540),
                    null,
                    Color.White,
                    0f,
                    new Vector2(_stageBackground.Width / 2, _stageBackground.Height / 2),
                    new Vector2(3.25f, 2.4f),
                    SpriteEffects.None,
                    0f
                    );

            player1.GetPose().GetSprite().Draw(_spriteBatch);
            player2.GetPose().GetSprite().Draw(_spriteBatch);
            currentAI.GetPose().GetSprite().Draw(_spriteBatch);

            if (!dontDisplayOutcome)
            {
                switch (player1.displayCircle)
                {
                    case DisplayCircle.Tied:
                        displayCircles[0].Draw(_spriteBatch);
                        break;
                    case DisplayCircle.Won:
                        displayCircles[1].Draw(_spriteBatch);
                        break;
                    case DisplayCircle.Lost:
                        displayCircles[2].Draw(_spriteBatch);
                        break;
                    default:
                        break;
                }

                switch (player2.displayCircle)
                {
                    case DisplayCircle.Tied:
                        displayCircles[3].Draw(_spriteBatch);
                        break;
                    case DisplayCircle.Won:
                        displayCircles[4].Draw(_spriteBatch);
                        break;
                    case DisplayCircle.Lost:
                        displayCircles[5].Draw(_spriteBatch);
                        break;
                    default:
                        break;
                }
            }

            _spriteBatch.DrawString(_countDownPlayerOneMove, counter.ToString(), new Vector2(200, 10), Color.Yellow, 0, Vector2.Zero, 3, new SpriteEffects(), 1);
            _spriteBatch.DrawString(_countDownPlayerTwoMove, counter2.ToString(), new Vector2(1500, 10), Color.Yellow, 0, Vector2.Zero, 3, new SpriteEffects(), 1);
            _spriteBatch.DrawString(_countDownAI, counterAI.ToString(), new Vector2(930, 1), Color.Yellow, 0, Vector2.Zero, 3, new SpriteEffects(), 1);
            _spriteBatch.DrawString(_playerOneMatchScore, "Player 1 Match Score:  " + player1.matchScore, new Vector2(200, 100), Color.DarkRed);
            _spriteBatch.DrawString(_playerTwoMatchScore, "Player 2 Match Score:  " + player2.matchScore, new Vector2(1500, 100), Color.DarkRed);
            _spriteBatch.DrawString(_playerOneRoundScore, "Player 1 Round Score:  " + player1.roundScore, new Vector2(200, 150), Color.LightSalmon);
            _spriteBatch.DrawString(_playerTwoRoundScore, "Player 2 Round Score:  " + player2.roundScore, new Vector2(1500, 150), Color.LightSalmon);
            _spriteBatch.DrawString(_playerOneScore, "Pose Score:  " + player1.GetScore(), new Vector2(260, 290), Color.Yellow, 0, Vector2.Zero, 2, new SpriteEffects(), 1);
            _spriteBatch.DrawString(_playerTwoScore, "Pose Score:  " + player2.GetScore(), new Vector2(1450, 290), Color.Yellow, 0, Vector2.Zero, 2, new SpriteEffects(), 1);
            _spriteBatch.DrawString(_match, "Match: " + matchNumber, new Vector2(100, 50), Color.Red);
            _spriteBatch.DrawString(_round, "Round: " + roundNumber, new Vector2(100, 100), Color.Orange);
            //_spriteBatch.DrawString(_title, "Pose'em! ", new Vector2(840, 35), Color.Firebrick, 0, Vector2.Zero, 3, new SpriteEffects(), 1);

            _spriteBatch.Draw(
                    _allPosesImage,
                    new Vector2(950, 900),
                    null,
                    Color.White,
                    0f,
                    new Vector2(_allPosesImage.Width / 2, _allPosesImage.Height / 2),
                    0.5f,
                    SpriteEffects.None,
                    0f
                    );


            //if (!String.IsNullOrWhiteSpace(overallWinnerString))
            //{
            //    _spriteBatch.DrawString(_overAllWinner, "Overall Winner: " + overallWinnerString, new Vector2(850, 150), Color.Fuchsia, 0, Vector2.Zero, 2, new SpriteEffects(), 1);
            //}

            for (int i = 0; i < 5; i++)
            {
                if (i >= player1.GetPosePattern().Count)
                {
                    continue;
                }

                int poseNumberPlayerOne = (int)player1.GetPosePattern()[i].GetPoseName() - 1;

                _spriteBatch.Draw(
                    playerOneSelectedPoseSpritesToChooseFrom[poseNumberPlayerOne],
                    new Vector2(220 + i * 130, 400),
                    null,
                    Color.White,
                    0f,
                    new Vector2(playerOneSelectedPoseSpritesToChooseFrom[poseNumberPlayerOne].Width / 2, playerOneSelectedPoseSpritesToChooseFrom[poseNumberPlayerOne].Height / 2),
                    0.9f,
                    SpriteEffects.None,
                    0f
                    );
            }

            for (int i = 0; i < 5; i++)
            {
                if (i >= player2.GetPosePattern().Count)
                {
                    continue;
                }
                int poseNumberPlayerTwo = (int)player2.GetPosePattern()[i].GetPoseName() - 1;

                _spriteBatch.Draw(
                    playerTwoSelectedPoseSpritesToChooseFrom[poseNumberPlayerTwo],
                    new Vector2(1400 + i * 130, 400),
                    null,
                    Color.White,

                    0f,
                    new Vector2(playerTwoSelectedPoseSpritesToChooseFrom[poseNumberPlayerTwo].Width / 2, playerTwoSelectedPoseSpritesToChooseFrom[poseNumberPlayerTwo].Height / 2),
                    0.9f,
                    SpriteEffects.None,
                    0f

                    );
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public Player ComparePoses(Player player, Player AI, int poseIndex)
        {
            PoseName poseName1 = player.GetPosePattern()[poseIndex].GetPoseName();//got an out of range exception here?***
            PoseName poseName2 = AI.GetPosePattern()[poseIndex].GetPoseName();

            if (poseName1 == poseName2)
            {
                return null;
            }

            int poseName1FirstValueItBeats = ((int)poseName1 + 1);
            if (poseName1FirstValueItBeats > 5)
            {
                int newNum = poseName1FirstValueItBeats - 5;
                poseName1FirstValueItBeats = newNum;
            }
            PoseName poseName1FirstPoseItBeats = (PoseName)poseName1FirstValueItBeats;
            int poseName1SecondValueItBeats = ((int)poseName1 + 3);
            if (poseName1SecondValueItBeats > 5)
            {
                int newNum = poseName1SecondValueItBeats - 5;
                poseName1SecondValueItBeats = newNum;
            }
            PoseName poseName1SecondPoseItBeats = (PoseName)poseName1SecondValueItBeats;

            if ((poseName2 == poseName1FirstPoseItBeats) || (poseName2 == poseName1SecondPoseItBeats))
            {
                return player;
            }
            else
            {
                return AI;
            }
        }

        private void NewRound()
        {
            if (player1.GetScore() > player2.GetScore())
            {
                player1.roundScore++;
            }
            else if (player2.GetScore() > player1.GetScore())
            {
                player2.roundScore++;
            }
            else
            {
                if (playerOneFinishedFirst)
                {
                    player1.roundScore++;
                }
                else
                {
                    player2.roundScore++;
                }
            }

            player1.SetScore(0);
            player2.SetScore(0);

            player1.SetPose(poses[0]);
            player1.SetPosePattern(new List<Pose>());
            player2.SetPose(poses[6]);
            player2.SetPosePattern(new List<Pose>());
            int upIndexCurrentAIIdlePoseIndex = (matchNumber - 1) * 6 + 12;
            currentAI.SetPose(poses[upIndexCurrentAIIdlePoseIndex]);//was 12

            player1.displayCircle = DisplayCircle.Tied;
            player2.displayCircle = DisplayCircle.Tied;

            soundEffects[5].CreateInstance().Play();
            roundNumber++;
            counter = counterStart = counter2 = counterStart2 = counterAI = counterStartAI = 3;

            numPosesDisplayedAI = 0;
            player1CanInput = true;
            player2CanInput = true;
            playerTurn = false;
            aiTurn = true;
            thisRoundTallied = false;

            if (roundNumber > 3)
            {
                thisRoundTallied = true;
                ResetMatch();
            }
        }


        private void ResetMatch()
        {
            if (player1.roundScore > player2.roundScore)
            {
                player1.matchScore++;
            }
            else if (player2.roundScore > player1.roundScore)
            {
                player2.matchScore++;
            }
            else
            {

            }

            if (matchNumber > 3)
            {
                ResetGame();
            }
            else
            {
                matchNumber++;
                currentAI = AIPlayerList[matchNumber - 1];//.SetPose(poses[12]);//is this a ref or value?
                player1.SetPosePattern(new List<Pose>(currentAI.GetPosePattern().Count));
                player2.SetPosePattern(new List<Pose>(currentAI.GetPosePattern().Count));
                roundNumber = 1;
                player1.roundScore = 0;
                player2.roundScore = 0;
            }
        }

        private void ResetGame()
        {
            matchNumber = 1;
            roundNumber = 1;
            currentAI = AIPlayerList[0];
            player1.SetScore(0);
            player2.SetScore(0);
            player1.roundScore = 0;
            player2.roundScore = 0;
            player1.matchScore = 0;
            player2.matchScore = 0;

        }
    }


}
