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
        Song audienceBackgroundSong;

        List<SoundEffect> soundEffects;
        List<SoundEffect> crowdSounds;
        List<SoundEffect> AIIntroSounds;

        const int numAnimations = 30;

        List<Player> AIPlayerList;// = new List<Player>(3);
        List<Animation> animations = new List<Animation>(numAnimations);
        List<Animation> outcomeAnimations = new List<Animation>(4);
        List<Animation> crowdcomeAnimations = new List<Animation>(2);
        List<Sprite> displayCircles = new List<Sprite>(6);
        List<Sprite> crowdSprites = new List<Sprite>(2);
        List<Sprite> signSprites = new List<Sprite>(4);
        List<Sprite> confettiSprites = new List<Sprite>(2);
        List<Pose> poses = new List<Pose>(numAnimations);
        List<Pose> outComePoses = new List<Pose>(4);
        List<Texture2D> RoundOutcomeTexturesForJumboTron;
        List<Texture2D> MatchOutComeTextures;
        private List<Texture2D> ringGirlImages;
        private List<Texture2D> playerOneSelectedPoseSpritesToChooseFrom = new List<Texture2D>(5);
        private List<Texture2D> playerTwoSelectedPoseSpritesToChooseFrom = new List<Texture2D>(5);
        private List<Component> menuComponents;

        Sprite controllerAllPosesSprite;
        Sprite _sparklerSprites;


        //List<Pose> defaultPoses = new List<Pose>(8);
        //List<Pose> altPoses = new List<Pose>(8);
        //List<Pose> bearPoses = new List<Pose>(8);
        //List<Pose> raPoses = new List<Pose>(8);
        //List<Pose> luchadorPoses = new List<Pose>(8);

        bool dontDisplayOutcome = true;
        bool playerOneFinishedFirst = true;
        bool gamestart = false;

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
        int counterAI = 5;
        float countDurationAI = .6f;
        float currentTimeAI = 0f;

        float roundTimer = 0;
        int roundNumber = 1;
        int matchNumber = 1;

        float songVolume = .3f;
        int numPosesDisplayedAI = 0;
        private float blackScreenOpacity;

        bool playerTurn = false;
        bool aiTurn = false;
        bool introTurn = true;
        bool player1CanInput = true;
        bool player2CanInput = true;
        bool updatedScoreBoard = false;
        bool displayMatchScore = false;
        bool showingFinalMatchScore = false;
        bool introPlayer1AudioHasPlayed = false;
        bool introPlayer2AudioHasPlayed = false;
        bool introAIAudioHasPlayed = false;
        bool Positionupdated = false;
        bool IsExit = false;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _stageBackground;
        private Texture2D _scoreBoard;
        private Texture2D _stageBackgroundIntro;
        private Texture2D _blackScreenBackground;
        private Texture2D _playerLightsBackground;
        private Texture2D _playerOneLightsBackground;
        private Texture2D _playerTwoLightsBackground;
        private Texture2D _aiLightsBackground;
        private Texture2D _aiLightsColored;
        private Texture2D _aiLightsColored2;
        private Texture2D _ringGirlLightsBackground;
        private Texture2D _playerNumbersBackground;
        private Texture2D _ringWhite;
        private Texture2D _player1Wins;
        private Texture2D _player2Wins;
        private Texture2D _checkOnScoreBoard;
        private Texture2D _XOnScoreBoard;
        private Texture2D _blankOnScoreBoard;
        private Texture2D zero;
        private Texture2D one;
        private Texture2D two;
        private Texture2D three;
        private Texture2D dash;

        private Texture2D twoForPoseStillDoubleScore;

        private void PlayButton_Click(object sender, System.EventArgs e)
        {
            gamestart = true;
            ResetGame();
        }
        private void QuitButton_Click(object sender, System.EventArgs e)
        {
            Exit();
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            soundEffects = new List<SoundEffect>();
            crowdSounds = new List<SoundEffect>();
            AIIntroSounds = new List<SoundEffect>();
            ringGirlImages = new List<Texture2D>();
            RoundOutcomeTexturesForJumboTron = new List<Texture2D>();
            MatchOutComeTextures = new List<Texture2D>();
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            WrestlerPosition1 = new Vector2(4270, 620);
            WrestlerPosition2 = new Vector2(5320, 620);
            AIPosition = new Vector2(3310, 400);

            blackScreenOpacity = 0f;
            base.Initialize();
        }

        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            MediaPlayer.Play(audienceBackgroundSong);//if finish now autoplay background crowd because will be during gameplay
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var menuBackground = new Background(Content.Load<Texture2D>("bighero72"), Color.White)
            {
                Rectangle = new Rectangle(0, 0, 1920, 1080),
            };

            var playButton = new Button(Content.Load<Texture2D>("START"), Content.Load<Texture2D>("START2"))
            {
                Position = new Vector2(800, 500),
            };
            playButton.Click += PlayButton_Click;

            var quitButton = new Button(Content.Load<Texture2D>("EXIT"), Content.Load<Texture2D>("EXIT2"))
            {
                Position = new Vector2(800, 700),
            };
            quitButton.Click += QuitButton_Click;

            menuComponents = new List<Component>()
            {
                menuBackground,
                playButton,
                quitButton,
            };

            _stageBackground = Content.Load<Texture2D>("main_stage_plane_audience");
            _stageBackgroundIntro = Content.Load<Texture2D>("main_stage_plane");
            _playerNumbersBackground = Content.Load<Texture2D>("p1_and_p2");
            
            _player1Wins = Content.Load<Texture2D>("Playerwins1");
            _player2Wins = Content.Load<Texture2D>("Playerwins2");

            _scoreBoard = Content.Load<Texture2D>("scoreboardSingle");
            _blackScreenBackground = Content.Load<Texture2D>("blackscreen");
            _aiLightsBackground = Content.Load<Texture2D>("ailights");
            _aiLightsColored = Content.Load<Texture2D>("colour-spotlightuse");//Colours - lights&glow/colour-spotlightOG
            //_aiLightsColored2 = Content.Load<Texture2D>("Colours - lights&glow/colour-glow2");

            _ringGirlLightsBackground = Content.Load<Texture2D>("ringgirllight");
            _ringWhite = Content.Load<Texture2D>("ringWhiteChunky");
            _blankOnScoreBoard = Content.Load<Texture2D>("0");
            _checkOnScoreBoard = Content.Load<Texture2D>("check");
            _XOnScoreBoard = Content.Load<Texture2D>("X");

            zero = Content.Load<Texture2D>("0");
            one = Content.Load<Texture2D>("1");
            two = Content.Load<Texture2D>("2");
            three = Content.Load<Texture2D>("3");
            dash = Content.Load<Texture2D>("-");

            twoForPoseStillDoubleScore = Content.Load<Texture2D>("x2");

            RoundOutcomeTexturesForJumboTron.Add(_blankOnScoreBoard);
            RoundOutcomeTexturesForJumboTron.Add(_checkOnScoreBoard);
            RoundOutcomeTexturesForJumboTron.Add(_XOnScoreBoard);

            MatchOutComeTextures.Add(zero);
            MatchOutComeTextures.Add(one);
            MatchOutComeTextures.Add(two);
            MatchOutComeTextures.Add(three);
            MatchOutComeTextures.Add(dash);

            _playerLightsBackground = Content.Load<Texture2D>("playerlights");
            _playerOneLightsBackground = Content.Load<Texture2D>("playeroneonlylights");
            _playerTwoLightsBackground = Content.Load<Texture2D>("playertwoonlylights");

            ringGirlImages.Add(Content.Load<Texture2D>("ringgirl1"));
            ringGirlImages.Add(Content.Load<Texture2D>("ringgirl2"));
            ringGirlImages.Add(Content.Load<Texture2D>("ringgirl3"));

            song = Content.Load<Song>("Sound/theme_background");
            audienceBackgroundSong = Content.Load<Song>("crowdSong1MP");

            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = songVolume;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
            soundEffects.Add(Content.Load<SoundEffect>("Sound/shout6"));
            soundEffects.Add(Content.Load<SoundEffect>("Sound/shout2"));
            soundEffects.Add(Content.Load<SoundEffect>("Sound/shout3"));
            soundEffects.Add(Content.Load<SoundEffect>("Sound/shout4"));
            soundEffects.Add(Content.Load<SoundEffect>("Sound/shout5"));
            soundEffects.Add(Content.Load<SoundEffect>("Sound/boxing_bell"));

            crowdSounds.Add(Content.Load<SoundEffect>("boo"));
            crowdSounds.Add(Content.Load<SoundEffect>("cheer"));
            crowdSounds.Add(Content.Load<SoundEffect>("murmurWav"));
            crowdSounds.Add(Content.Load<SoundEffect>("boo_1"));
            crowdSounds.Add(Content.Load<SoundEffect>("cheer_1"));
            crowdSounds.Add(Content.Load<SoundEffect>("boo_2"));
            crowdSounds.Add(Content.Load<SoundEffect>("cheer_2"));
            crowdSounds.Add(Content.Load<SoundEffect>("murmur2"));

            AIIntroSounds.Add(Content.Load<SoundEffect>("warriorcryaiintro"));
            AIIntroSounds.Add(Content.Load<SoundEffect>("bearaiintro"));
            AIIntroSounds.Add(Content.Load<SoundEffect>("egyptianaiintroCut"));

            SoundEffectInstance booInstancePlayerOne = crowdSounds[3].CreateInstance();
            SoundEffectInstance cheerInstancePlayerOne = crowdSounds[4].CreateInstance();
            SoundEffectInstance murmurInstancePlayerOne = crowdSounds[2].CreateInstance();

            SoundEffectInstance booInstancePlayerTwo = crowdSounds[0].CreateInstance();
            SoundEffectInstance cheerInstancePlayerTwo = crowdSounds[1].CreateInstance();
            SoundEffectInstance murmurInstancePlayerTwo = crowdSounds[7].CreateInstance();

            SoundEffect.MasterVolume = 1f;// 0.5f;

            List<string> stillAnimationImageNameStrings = new List<string>(5) { "twodownclear", "pointingclear", "oneupclear", "twoupclear", "herculesclear" };
            for (int i = 0; i < 5; i++)
            {
                playerOneSelectedPoseSpritesToChooseFrom.Add(Content.Load<Texture2D>(stillAnimationImageNameStrings[i]));
                playerTwoSelectedPoseSpritesToChooseFrom.Add(Content.Load<Texture2D>(stillAnimationImageNameStrings[i]));
            }

            float displayCircleLayer = 0.8f;
            float displayCircleScale = 3f;
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("bluefeet"), 3), displayCircleScale, displayCircleLayer));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("greenfeetthree"), 3), displayCircleScale, displayCircleLayer));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("redfeet"), 3), displayCircleScale, displayCircleLayer));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("bluefeet"), 3), displayCircleScale, displayCircleLayer));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("greenfeetthree"), 3), displayCircleScale, displayCircleLayer));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("redfeet"), 3), displayCircleScale, displayCircleLayer));

            controllerAllPosesSprite = new Sprite(new Animation(Content.Load<Texture2D>("Animations/JoystickOnly-Posechart"), 24), 0.7f, 1);
            _sparklerSprites = new Sprite(new Animation(Content.Load<Texture2D>("Colours - lights&glow/sparkler sprite sheet"), 12), 1.5f, 0.89f);//player on .9 so want behind ai


            float crowdScale = 2.4f;
            float crowdLayer = .5f;
            crowdSprites.Add(new Sprite(new Animation(Content.Load<Texture2D>("stageleft"), 3), crowdScale, crowdLayer));
            crowdSprites.Add(new Sprite(new Animation(Content.Load<Texture2D>("stageright"), 3), crowdScale, crowdLayer));

            float signScale = 2.4f;
            float signLayer = .51f;
            signSprites.Add(new Sprite(new Animation(Content.Load<Texture2D>("greenSignsAnim"), 3), signScale, signLayer));
            signSprites.Add(new Sprite(new Animation(Content.Load<Texture2D>("redSignsAnim"), 3), signScale, signLayer));
            signSprites.Add(new Sprite(new Animation(Content.Load<Texture2D>("greenSignsAnim"), 3), signScale, signLayer));
            signSprites.Add(new Sprite(new Animation(Content.Load<Texture2D>("redSignsAnim"), 3), signScale, signLayer));




            confettiSprites.Add(new Sprite(new Animation(Content.Load<Texture2D>("confettispritesheet"), 3), 1.6f, 0.98f));
            confettiSprites.Add(new Sprite(new Animation(Content.Load<Texture2D>("confettispritesheet2"), 3), 1.6f, 0.98f));

            //player one animations:
            animations.Add(new Animation(Content.Load<Texture2D>("idle"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("yellow&crouch"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("purple&lean"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("blue&point"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("red&botharms"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("green&back"), 12));
            //player 2 animations
            animations.Add(new Animation(Content.Load<Texture2D>("idlealt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("yellow&couch-alt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("purple&lean-alt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("blue&point-alt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("red&botharms-alt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("green&back-alt"), 12));
            //AI 2 animations
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/luchador-idle"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/luchador-yellow"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/luchador-purple"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/luchador-blue"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/luchador-red"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/luchador-green"), 12));
            //AI 3 animations
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/bear-idle"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/bear-yeller"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/bear-purple"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/bear-blue"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/bear-red"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/bear-green"), 12));
            //AI 4 animations
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/ra-idle"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/ra-yellow"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/ra-purple"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/ra-blue"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/ra-red"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/ra-green"), 12));

            outcomeAnimations.Add(new Animation(Content.Load<Texture2D>("win"), 12));
            outcomeAnimations.Add(new Animation(Content.Load<Texture2D>("lose"), 12));
            outcomeAnimations.Add(new Animation(Content.Load<Texture2D>("winalt"), 12));
            outcomeAnimations.Add(new Animation(Content.Load<Texture2D>("losealt"), 12));

            outComePoses.Add(new Pose(outcomeAnimations[0], PoseName.Idle, 2.5f, 0.9f));
            outComePoses.Add(new Pose(outcomeAnimations[1], PoseName.Idle, 2.5f, 0.9f));
            outComePoses.Add(new Pose(outcomeAnimations[2], PoseName.Idle, 2.5f, 0.9f));
            outComePoses.Add(new Pose(outcomeAnimations[3], PoseName.Idle, 2.5f, 0.9f));

            //List<List<Pose>> allPoseList = new List<List<Pose>>();
            //allPoseList.Add(defaultPoses);
            //allPoseList.Add(altPoses);
            //allPoseList.Add(bearPoses);
            //allPoseList.Add(raPoses);
            //allPoseList.Add(luchadorPoses);

            //for (int i = 0; i < allPoseList.Count; i++)
            //{
            //    float scale = 2.5f;
            //    //change scale to 0.9f if AI
            //    //won't know that until player makes selection though
            //    //so need to have selection screen, as making selection swap out idle character 
            //    //need like some idles animations list that has different instances of just the idles so player can see them
            //    //and then when that is selected you go in here
            //    for (int j = 0; j < 6; j++)
            //    {
            //        //8 poses, idle, 5 competition poses, win and loss
            //        //issue is that those last two are not accounted for by the compare poses algorithm
            //        //or by a lot of other stuff
            //        //they are just on their own in outcome poses
            //        //so perhaps each character option just has its own outcome poses, just to mkae this simpler
            //        //in which case we are only going through 6 here
            //        allPoseList[i].Add(new Pose(animations[j], (PoseName)j, scale, 0.9f));
            //    }
            //}


            for (int i = 0; i < numAnimations; i++)
            {
                int poseInt = i;
                float scale = 2.5f;
                
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
                if (i > 23)
                {
                    poseInt = poseInt - 6;
                    scale = 1.5f;
                }

                poses.Add(new Pose(animations[i], (PoseName)poseInt, scale, 0.9f));
            }

            player1 = new Player("Player One", WrestlerPosition1, poses[0], new List<Pose>(), booInstancePlayerOne, cheerInstancePlayerOne, murmurInstancePlayerOne);
            player2 = new Player("Player Two", WrestlerPosition2, poses[6], new List<Pose>(), booInstancePlayerTwo, cheerInstancePlayerTwo, murmurInstancePlayerTwo);

            AIPlayerList = new List<Player>
            {
                //the 3rd parameter is the idle parameter for that ai, we now have 2, and idle for alt and bear are 12 and 18 respectively
                new Player("luchadorAI", AIPosition, poses[12], new List<Pose>(3) { poses[13], poses[13], poses[13] }, new List<int>{ 13, 14, 15, 16}, AIIntroSounds[0]),
                new Player("bearAI", AIPosition, poses[18], new List<Pose>(3) { poses[19], poses[19], poses[19], poses[19] }, new List<int>{ 19, 20, 21, 22, 23}, AIIntroSounds[1]),
                new Player("raAI", AIPosition, poses[24], new List<Pose>(4) { poses[25], poses[25], poses[25], poses[25], poses[25] }, new List<int>{ 25, 26, 27, 28, 29}, AIIntroSounds[2]),
            };

            currentAI = AIPlayerList[0];
        }

        protected override void Update(GameTime gameTime)
        {

            if (!gamestart)
                foreach (var button in menuComponents)
                    button.Update(gameTime);
            else
            {
                StickDirection playerOneLeftStick = StickDirection.None;
                StickDirection playerOneRightStick = StickDirection.None;
                StickDirection playerTwoLeftStick = StickDirection.None;
                StickDirection playerTwoRightStick = StickDirection.None;

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
                    player1CanInput = false;
                    player2CanInput = false;
                    dontDisplayOutcome = false;

                    roundTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (roundTimer > 2000)
                    {
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


                                    blackScreenOpacity = (roundTimer - 7000) / 2000;
                                    blackScreenOpacity = MathF.Min(blackScreenOpacity, 1f);

                                    float newVolume = 1 - ((roundTimer - 7000) / 2000);
                                    newVolume *= songVolume;

                                    MediaPlayer.Volume = MathF.Max(newVolume, 0f);

                                    if (roundTimer > 10000)
                                    {
                                        MediaPlayer.Play(audienceBackgroundSong);
                                        currentAI.SetPose(poses[12]);
                                        player1.SetPose(poses[0]);
                                        player2.SetPose(poses[6]);
                                        player1.displayCircle = DisplayCircle.Tied;
                                        player2.displayCircle = DisplayCircle.Tied;
                                        aiTurn = true;
                                        playerTurn = false;
                                        roundTimer = 0;
                                        introTurn = false;


                                        if (!Positionupdated)
                                        {
                                            for (int i = 0; i < AIPlayerList.Count; i++)
                                            {
                                                AIPlayerList[i].IncreaseXPosition(-40);
                                            }
                                            Positionupdated = true;
                                        }

                                        soundEffects[5].CreateInstance().Play();
                                    }
                                }
                            }
                        }
                    }
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed))
                {
                    gamestart = false;
                    playerTurn = false;
                    aiTurn = false;
                    introTurn = true;
                    player1CanInput = true;
                    player2CanInput = true;
                    updatedScoreBoard = false;
                    displayMatchScore = false;
                    showingFinalMatchScore = false;
                    introPlayer1AudioHasPlayed = false;
                    introPlayer2AudioHasPlayed = false;
                    introAIAudioHasPlayed = false;

                    for (int i = 0; i < 5; i++)
                    {
                        player1.PoseOutcomes[i] = 0;
                        player2.PoseOutcomes[i] = 0;
                        player1.WasDoublePoseScore[i] = false;
                        player2.WasDoublePoseScore[i] = false;
                    }

                    counter = 3;
                    counterStart = 3;
                    countDuration = .4f;
                    currentTime = 0f;

                    counter2 = 3;
                    counterStart2 = 3;
                    countDuration2 = .4f;
                    currentTime2 = 0f;

                    counterAI = 5;
                    countDurationAI = .6f;
                    currentTimeAI = 0f;

                    roundTimer = 0;
                    roundNumber = 1;
                    matchNumber = 1;

                    songVolume = .3f;
                    numPosesDisplayedAI = 0;

                    currentAI.SetPose(poses[12]);
                    player1.SetPose(poses[0]);
                    player2.SetPose(poses[6]);
                    player1.CrowdMoving = false;
                    player2.CrowdMoving = false;
                    player1.PlayerWinningForCrowd = false;
                    player2.PlayerWinningForCrowd = false;

                    player1.displayCircle = DisplayCircle.Tied;
                    player2.displayCircle = DisplayCircle.Tied;

                    if (Positionupdated)
                    {
                        for (int i = 0; i < AIPlayerList.Count; i++)
                        {
                            AIPlayerList[i].IncreaseXPosition(40);
                        }
                        Positionupdated = false;
                    }

                    IsExit = true;

                    ResetMatch();
                    ResetGame();
                    //Exit();
                }

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
                        counter = 0;
                        player1CanInput = true;
                    }

                    if (player1.GetPosePattern().Count < currentAI.GetPosePattern().Count && player1CanInput)
                    {
                        if (inputState.IsKeyDown(Keys.A) || ((playerOneLeftStick == StickDirection.Down) && (playerOneRightStick == StickDirection.Down)))
                        {
                            player1CanInput = PlayerPoseSelection(1, 1, 0, player1);
                        }
                        else if (inputState.IsKeyDown(Keys.F) || ((playerOneLeftStick == StickDirection.Up) && (playerOneRightStick == StickDirection.Up)))
                        {
                            player1CanInput = PlayerPoseSelection(4, 4, 3, player1);
                        }
                        else if (inputState.IsKeyDown(Keys.D) || ((playerOneLeftStick == StickDirection.Up) && (playerOneRightStick == StickDirection.Down)))
                        {
                            player1CanInput = PlayerPoseSelection(3, 3, 2, player1);
                        }
                        else if (inputState.IsKeyDown(Keys.S) || ((playerOneLeftStick == StickDirection.Up) && (playerOneRightStick == StickDirection.Right)))
                        {
                            player1CanInput = PlayerPoseSelection(2, 2, 1, player1);
                        }
                        else if (inputState.IsKeyDown(Keys.G) || ((playerOneLeftStick == StickDirection.Left) && (playerOneRightStick == StickDirection.Right)))
                        {
                            player1CanInput = PlayerPoseSelection(5, 5, 4, player1);
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
                            player2CanInput = PlayerPoseSelection(7, 7, 0, player2);
                        }
                        else if (inputState.IsKeyDown(Keys.NumPad4) || ((playerTwoLeftStick == StickDirection.Up) && (playerTwoRightStick == StickDirection.Up)))
                        {
                            player2CanInput = PlayerPoseSelection(10, 10, 3, player2);
                        }
                        else if (inputState.IsKeyDown(Keys.NumPad3) || ((playerTwoLeftStick == StickDirection.Up) && (playerTwoRightStick == StickDirection.Down)))
                        {
                            player2CanInput = PlayerPoseSelection(9, 9, 2, player2);
                        }
                        else if (inputState.IsKeyDown(Keys.NumPad2) || ((playerTwoLeftStick == StickDirection.Up) && (playerTwoRightStick == StickDirection.Right)))
                        {
                            player2CanInput = PlayerPoseSelection(8, 8, 1, player2);
                        }
                        else if (inputState.IsKeyDown(Keys.NumPad5) || ((playerTwoLeftStick == StickDirection.Left) && (playerTwoRightStick == StickDirection.Right)))
                        {
                            player2CanInput = PlayerPoseSelection(11, 11, 4, player2);
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

                        if (!updatedScoreBoard)
                        {
                            Player winner = WinningPlayer();
                            winner.RoundOutcomes[roundNumber - 1] = 1;
                            Player loser = winner == player1 ? player2 : player1;
                            loser.RoundOutcomes[roundNumber - 1] = 2;
                        }

                        updatedScoreBoard = true;

                        if (roundTimer > 3000)
                        {
                            dontDisplayOutcome = true;

                            if (player1.GetScore() > player2.GetScore())
                            {
                                player1.SetPose(outComePoses[0]);
                                player2.SetPose(outComePoses[3]);
                                player1.CrowdMoving = true;
                                player2.CrowdMoving = false;
                                player1.PlayerWinningForCrowd = true;
                                player2.PlayerWinningForCrowd = false;
                            }
                            else if (player2.GetScore() > player1.GetScore())
                            {
                                player1.SetPose(outComePoses[1]);
                                player2.SetPose(outComePoses[2]);
                                player1.CrowdMoving = false;
                                player2.CrowdMoving = true;
                                player1.PlayerWinningForCrowd = false;
                                player2.PlayerWinningForCrowd = true;
                            }
                            else
                            {
                                if (playerOneFinishedFirst)
                                {
                                    player1.SetPose(outComePoses[0]);
                                    player2.SetPose(outComePoses[3]);
                                    player1.CrowdMoving = true;
                                    player2.CrowdMoving = false;
                                    player1.PlayerWinningForCrowd = true;
                                    player2.PlayerWinningForCrowd = false;
                                }
                                else
                                {
                                    player1.SetPose(outComePoses[1]);
                                    player2.SetPose(outComePoses[2]);
                                    player1.CrowdMoving = false;
                                    player2.CrowdMoving = true;
                                    player1.PlayerWinningForCrowd = false;
                                    player2.PlayerWinningForCrowd = true;
                                }
                            }

                            int lastRoundTimer = 5000;
                            int addToLastMatchLastRound = 0;
                            if (roundNumber > 2)
                            {
                                lastRoundTimer = 8000;

                                if (matchNumber == 3)
                                {
                                    addToLastMatchLastRound = 5000;
                                    showingFinalMatchScore = true;
                                }
                            }

                            if (roundTimer > 5000)
                            {
                                if (lastRoundTimer == 8000)
                                {
                                    dontDisplayOutcome = false;
                                    player1.displayCircle = DisplayCircle.Tied;
                                    player2.displayCircle = DisplayCircle.Tied;
                                    player1.SetPose(poses[0]);
                                    player2.SetPose(poses[6]);
                                    displayMatchScore = true;
                                    player1.CrowdMoving = false;
                                    player2.CrowdMoving = false;
                                }

                                if (roundTimer > (lastRoundTimer + addToLastMatchLastRound))
                                {
                                    dontDisplayOutcome = false;
                                    displayMatchScore = false;
                                    playerTurn = false;
                                    roundTimer = 0;
                                    NewRound();
                                }
                            }
                        }
                    }
                }

                player1.GetPose().GetSprite().Update(gameTime, player1.GetPosition());
                player2.GetPose().GetSprite().Update(gameTime, player2.GetPosition());

                for (int i = 0; i < 3; i++)
                {
                    displayCircles[i].Update(gameTime, new Vector2(880, 1000));
                }
                for (int i = 3; i < 6; i++)
                {
                    displayCircles[i].Update(gameTime, new Vector2(1930, 1000));
                }

                controllerAllPosesSprite.Update(gameTime, new Vector2(5792, 640));
                _sparklerSprites.Update(gameTime, new Vector2(10875, 450));
                crowdSprites[0].Update(gameTime, new Vector2(1060, 525));
                crowdSprites[1].Update(gameTime, new Vector2(2420, 525));
                signSprites[0].Update(gameTime, new Vector2(1060, 525));
                signSprites[1].Update(gameTime, new Vector2(1060, 525));
                signSprites[2].Update(gameTime, new Vector2(2420, 525));
                signSprites[3].Update(gameTime, new Vector2(2420, 525));
                confettiSprites[0].Update(gameTime, new Vector2(1000, 440));
                confettiSprites[1].Update(gameTime, new Vector2(2050, 440));

                if (aiTurn)
                {
                    dontDisplayOutcome = true;

                    if (numPosesDisplayedAI < currentAI.GetPosePattern().Count)
                    {
                        currentTimeAI += (float)gameTime.ElapsedGameTime.TotalSeconds;

                        if (blackScreenOpacity > 0)
                        {
                            roundTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                            blackScreenOpacity = 1 - (roundTimer / 3000);
                            blackScreenOpacity = MathF.Max(blackScreenOpacity, 0f);
                            float newVolume = roundTimer / 3000;
                            MediaPlayer.Volume = MathF.Min(newVolume, 0.4f);
                        }
                        else
                        {
                            roundTimer = 0;
                        }

                        if (currentTimeAI >= countDurationAI)
                        {
                            counterAI--;
                            currentTimeAI -= countDurationAI;
                        }
                        if (counterAI < 0)
                        {
                            bool IsNewPose = false;
                            int randomPoseIndexFromThisAisPoseNumberslist = 0;
                            int poseValueFromThisAisRandomSelection = 0;

                            while (!IsNewPose)
                            {
                                Random rnd = new Random();
                                randomPoseIndexFromThisAisPoseNumberslist = rnd.Next(0, currentAI._poseValuesForThisAI.Count);
                                poseValueFromThisAisRandomSelection = currentAI._poseValuesForThisAI[randomPoseIndexFromThisAisPoseNumberslist];

                                if(numPosesDisplayedAI == 0)
                                {
                                    IsNewPose = true;
                                }
                                else
                                {
                                    Pose thisSelectedPose = poses[poseValueFromThisAisRandomSelection];
                                    Pose previousAIPose = currentAI.GetPose();
                                    if(thisSelectedPose != previousAIPose)
                                    {
                                        IsNewPose = true;
                                    }
                                }
                            }
                            currentAI.SetPose(poses[poseValueFromThisAisRandomSelection]);
                            currentAI.SetPosePattern(numPosesDisplayedAI, poses[poseValueFromThisAisRandomSelection]);
                            soundEffects[randomPoseIndexFromThisAisPoseNumberslist].CreateInstance().Play();
                            float animationTime = currentAI.GetPosePattern()[numPosesDisplayedAI].GetSprite().GetAnimationTime();
                            //int roundedUp = (int)Math.Ceiling(animationTime);//didn't end up needing this, but is to determine pose display time by animation time instead of 
                            //using a fixed value
                            counterAI = 3;
                            numPosesDisplayedAI++;
                        }

                    }

                    if (numPosesDisplayedAI >= currentAI.GetPosePattern().Count)
                    {
                        roundTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (roundTimer > 2000)
                        {
                            int upIndexCurrentAIIdlePoseIndex = (matchNumber - 1) * 6 + 12;// 1: 12 //first ai idle // 2: 18 //second ai idle  // 3: 24 //third ai idle
                            currentAI.SetPose(poses[upIndexCurrentAIIdlePoseIndex]);
                            aiTurn = false;
                            playerTurn = true;
                            dontDisplayOutcome = false;
                            roundTimer = 0;
                        }
                    }
                }

                currentAI.GetPose().GetSprite().Update(gameTime, currentAI.GetPosition());
            }
            base.Update(gameTime);
        }

        private bool PlayerPoseSelection(int poseToSet, int poseForPosePattern, int soundEffect, Player player)
        {
            player.SetPose(poses[poseToSet]);
            player.AddToPosePattern(poses[poseForPosePattern]);
            ComparePosesAndSetScores(player.GetPosePattern().Count - 1, player, currentAI);
            soundEffects[soundEffect].CreateInstance().Play();

            if (player == player1)
            {
                counter = counterStart;
            }
            else
            {
                counter2 = counterStart2;
            }

            return false;
        }

        void ComparePosesAndSetScores(int posePatternIndex, Player player, Player currentAI)
        {
            //bool isTwoPointPlayerVictory = false;
            Player winnerBetweenPlayerTwoAndAI = ComparePoses(player, currentAI, posePatternIndex, out bool isTwoPointPlayerVictory);
            player.CrowdMoving = false;
            player.PlayerWinningForCrowd = false;

            if (winnerBetweenPlayerTwoAndAI != null)
            {
                if (winnerBetweenPlayerTwoAndAI == player)
                {
                    int ScoreToAdd = 1;
                    if (isTwoPointPlayerVictory)
                    {
                        ScoreToAdd = 2;
                        player.WasDoublePoseScore[posePatternIndex] = true;
                    }
                    player.SetScore(player.GetScore() + ScoreToAdd);
                    player.displayCircle = DisplayCircle.Won;
                    player.CheerInstance.Stop();
                    player.CheerInstance.Play();
                    player.CrowdMoving = true;
                    player.PlayerWinningForCrowd = true;
                }
                else
                {
                    player.SetScore(player.GetScore() - 1);
                    player.displayCircle = DisplayCircle.Lost;
                    player.BooInstance.Stop();
                    player.BooInstance.Play();
                    player.CrowdMoving = true;
                    player.PlayerWinningForCrowd = false;
                }
            }
            else
            {
                player.displayCircle = DisplayCircle.Tied;
                player.MurmurInstance.Stop();
                player.MurmurInstance.Play();
                player.CrowdMoving = false;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, null);

            if (!gamestart)
                foreach (var button in menuComponents)
                    button.Draw(gameTime, _spriteBatch);
            else
            {
                if (!introTurn)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        _spriteBatch.Draw(
                                  _scoreBoard,
                                  new Vector2(920 + i * 542, 440),
                                  null,
                                  Color.White,
                                  0f,
                                  new Vector2(_scoreBoard.Width / 2, _scoreBoard.Height / 2),
                                  new Vector2(1.6f, 1.6f),
                                  SpriteEffects.None,
                                  0.4f
                                  );

                    }
                }

                if (displayMatchScore)
                {
                    int localPlayer1MatchScore = 0;
                    int localPlayer2MatchScore = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        //this section basically copies a lot of the functionality from NewRound() and ResetMatch(), but it needs to be done here
                        //again to display the right match score values before the match actually ends. I could refactor to avoid this but it would be 
                        //too much of a buggy pain this late 
                        int localPlayer1RoundScore = player1.roundScore;
                        int localPlayer2RoundScore = player2.roundScore;
                        Player winner = WinningPlayer();
                        if (winner == player1)
                        {
                            localPlayer1RoundScore++;
                        }
                        else
                        {
                            localPlayer2RoundScore++;
                        }

                        localPlayer1MatchScore = player1.matchScore;
                        localPlayer2MatchScore = player2.matchScore;

                        if (localPlayer1RoundScore > localPlayer2RoundScore)
                        {
                            localPlayer1MatchScore++;
                        }
                        else if (localPlayer1RoundScore < localPlayer2RoundScore)
                        {
                            localPlayer2MatchScore++;
                        }

                        Texture2D MatchOutcomeTexture;
                        if (i == 1)
                        {
                            MatchOutcomeTexture = MatchOutComeTextures[4];
                        }
                        else if (i == 0)
                        {
                            MatchOutcomeTexture = MatchOutComeTextures[localPlayer1MatchScore];
                        }
                        else
                        {
                            MatchOutcomeTexture = MatchOutComeTextures[localPlayer2MatchScore];
                        }
                        DrawMatchScores(MatchOutcomeTexture, 850 + i * 100, 650);
                    }

                    if (localPlayer2MatchScore < localPlayer1MatchScore)
                    {

                        confettiSprites[0].Draw(_spriteBatch, false, currentAI);

                        //END GAME AND AFTER END OF LAST ROUND OF LAST MATCH
                        if (matchNumber == 3 && (roundTimer > 8000))
                        {
                            _spriteBatch.Draw(
                             _player1Wins,
                             new Vector2(980, 540),
                             null,
                             Color.Yellow,
                             0f,
                             new Vector2(_player1Wins.Width / 2, _player1Wins.Height / 2),
                             new Vector2(3f, 3f),
                             SpriteEffects.None,
                             0.98f
                             );

                            crowdSprites[0].Draw(_spriteBatch, false, currentAI);
                            signSprites[0].Draw(_spriteBatch, false, currentAI);

                            if (player1.GetPose() != outComePoses[0])
                            {
                                player1.SetPose(outComePoses[0]);
                            }

                            if (player2.GetPose() != outComePoses[3])
                            {
                                player2.SetPose(outComePoses[3]);
                            }
                        }
                    }
                    else
                    {
                        confettiSprites[1].Draw(_spriteBatch, false, currentAI);

                        if (matchNumber == 3 && (roundTimer > 8000))//MATCHCHANGE
                        {
                            _spriteBatch.Draw(
                                _player2Wins,
                                new Vector2(980, 540),
                                null,
                                Color.Yellow,
                                0f,
                                new Vector2(_player2Wins.Width / 2, _player2Wins.Height / 2),
                                new Vector2(3f, 3f),
                                SpriteEffects.None,
                                0.98f
                                );

                            crowdSprites[1].Draw(_spriteBatch, false, currentAI);
                            signSprites[2].Draw(_spriteBatch, false, currentAI);


                            if (player1.GetPose() != outComePoses[1])
                            {
                                player1.SetPose(outComePoses[1]);

                            }

                            if (player2.GetPose() != outComePoses[2])
                            {
                                player2.SetPose(outComePoses[2]);
                            }

                        }
                    }
                }

                if (!introTurn)
                {
                    _spriteBatch.Draw(
                            _stageBackground,
                            new Vector2(960, 540),
                            null,
                            Color.White,
                            0f,
                            new Vector2(_stageBackground.Width / 2, _stageBackground.Height / 2),
                            new Vector2(2.4f, 2.4f),
                            SpriteEffects.None,
                            0f
                            );

                    _spriteBatch.Draw(
                      _playerNumbersBackground,
                      new Vector2(960, 540),
                      null,
                      Color.White,
                      0f,
                      new Vector2(_playerNumbersBackground.Width / 2, _playerNumbersBackground.Height / 2),
                      new Vector2(2.4f, 2.4f),
                      SpriteEffects.None,
                      0.98f
                      );
                }
                else
                {
                    _spriteBatch.Draw(
                       _stageBackgroundIntro,
                       new Vector2(960, 540),
                       null,
                       Color.White,
                       0f,
                       new Vector2(_stageBackgroundIntro.Width / 2, _stageBackgroundIntro.Height / 2),
                       new Vector2(2.4f, 2.4f),
                       SpriteEffects.None,
                       0f
                       );
                }

                player1.GetPose().GetSprite().Draw(_spriteBatch, false, currentAI);
                player2.GetPose().GetSprite().Draw(_spriteBatch, false, currentAI);
                currentAI.GetPose().GetSprite().Draw(_spriteBatch, false, currentAI);

                if (!dontDisplayOutcome)
                {
                    switch (player1.displayCircle)
                    {
                        case DisplayCircle.Tied:
                            displayCircles[0].Draw(_spriteBatch, false, currentAI);
                            break;
                        case DisplayCircle.Won:
                            displayCircles[1].Draw(_spriteBatch, false, currentAI);
                            break;
                        case DisplayCircle.Lost:
                            displayCircles[2].Draw(_spriteBatch, false, currentAI);
                            break;
                        default:
                            break;
                    }

                    switch (player2.displayCircle)
                    {
                        case DisplayCircle.Tied:
                            displayCircles[3].Draw(_spriteBatch, false, currentAI);
                            break;
                        case DisplayCircle.Won:
                            displayCircles[4].Draw(_spriteBatch, false, currentAI);
                            break;
                        case DisplayCircle.Lost:
                            displayCircles[5].Draw(_spriteBatch, false, currentAI);
                            break;
                        default:
                            break;
                    }
                }

                if (!introTurn)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Texture2D outcomeDisplay = RoundOutcomeTexturesForJumboTron[player1.RoundOutcomes[i]];
                        if (player1.RoundOutcomes[i] != 0)
                        {
                            DrawRoundScores(outcomeDisplay, 740, 65 + i * 130);
                        }
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        Texture2D outcomeDisplay = RoundOutcomeTexturesForJumboTron[player2.RoundOutcomes[i]];
                        if (player2.RoundOutcomes[i] != 0)
                        {
                            DrawRoundScores(outcomeDisplay, 1286, 65 + i * 130);
                        }
                    }
                }

                if (!showingFinalMatchScore)
                {
                    if (player1.CrowdMoving)
                    {
                        crowdSprites[0].Draw(_spriteBatch, false, currentAI);

                        if (player1.PlayerWinningForCrowd)
                        {
                            signSprites[0].Draw(_spriteBatch, false, currentAI);
                        }
                        else
                        {
                            signSprites[1].Draw(_spriteBatch, false, currentAI);
                        }
                    }
                    if (player2.CrowdMoving)
                    {
                        crowdSprites[1].Draw(_spriteBatch, false, currentAI);

                        if (player2.PlayerWinningForCrowd)
                        {
                            signSprites[2].Draw(_spriteBatch, false, currentAI);
                        }
                        else
                        {
                            signSprites[3].Draw(_spriteBatch, false, currentAI);
                        }
                    }
                }

                int upIndexCurrentAIIdlePoseIndexForRingGirl = (matchNumber - 1) * 6 + 12;

                if (aiTurn && (roundNumber == 1) && (currentAI.GetPose() == poses[upIndexCurrentAIIdlePoseIndexForRingGirl]))//and round number 0 and current ai pose is idle?
                {
                    _spriteBatch.Draw(
                          ringGirlImages[matchNumber - 1],
                          new Vector2(950, 680),
                          null,
                          Color.White,
                          0f,
                          new Vector2(ringGirlImages[matchNumber - 1].Width / 2, ringGirlImages[matchNumber - 1].Height / 2),
                          2.2f,
                          SpriteEffects.None,
                          0.91f
                          );
                }
                else if (!introTurn && !displayMatchScore && !aiTurn)
                {
                    controllerAllPosesSprite.Draw(_spriteBatch, false, currentAI);
                }
                else
                {
                    int test = 5;
                }

                int picSpacing = 120;
                int deltaLeftForPosePatternDisplayByMatch = (matchNumber - 1) * 55;
                float poseStillAndRingOpacity = 1f;

                if (!introTurn && !aiTurn)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (i >= currentAI.GetPosePattern().Count)
                        {
                            continue;
                        }

                        Color color = Color.White;
                        int playerPoseOutcome = player1.PoseOutcomes[i];//will be player 2 in other loop

                        if (playerPoseOutcome == 1)
                        {
                            color = Color.Green;
                        }
                        else if (playerPoseOutcome == 2)
                        {
                            color = Color.Red;
                        }
                        else if (playerPoseOutcome == 3)
                        {
                            color = Color.FromNonPremultiplied(17, 135, 255, 256);
                        }

                        _spriteBatch.Draw(
                            _ringWhite,
                            new Vector2(120, 440 + i * picSpacing - deltaLeftForPosePatternDisplayByMatch),
                            null,
                            color * poseStillAndRingOpacity,
                            0f,
                            new Vector2(_ringWhite.Width / 2, _ringWhite.Height / 2),
                            0.7f,
                            SpriteEffects.None,
                            0.98f
                            );
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        if (i >= currentAI.GetPosePattern().Count)
                        {
                            continue;
                        }

                        Color color = Color.White;

                        int playerPoseOutcome = player2.PoseOutcomes[i];//will be player 2 in other loop

                        if (playerPoseOutcome == 1)
                        {
                            color = Color.Green;
                        }
                        else if (playerPoseOutcome == 2)
                        {
                            color = Color.Red;
                        }
                        else if (playerPoseOutcome == 3)
                        {
                            color = Color.FromNonPremultiplied(17, 135, 255, 256);
                        }

                        _spriteBatch.Draw(
                            _ringWhite,
                            new Vector2(1770, 440 + i * picSpacing - deltaLeftForPosePatternDisplayByMatch),
                            null,
                            color * poseStillAndRingOpacity,
                            0f,
                            new Vector2(_ringWhite.Width / 2, _ringWhite.Height / 2),
                            0.7f,
                            SpriteEffects.None,
                            0.98f
                            );
                    }
                }

                for (int i = 0; i < 5; i++)
                {

                    if (i >= player1.GetPosePattern().Count)
                    {
                        continue;
                    }

                    int poseNumberPlayerOne = (int)player1.GetPosePattern()[i].GetPoseName() - 1;

                    _spriteBatch.Draw(
                        playerOneSelectedPoseSpritesToChooseFrom[poseNumberPlayerOne],
                        new Vector2(120, 440 + i * picSpacing - deltaLeftForPosePatternDisplayByMatch),
                        null,
                        Color.White * poseStillAndRingOpacity,
                        0f,
                        new Vector2(playerOneSelectedPoseSpritesToChooseFrom[poseNumberPlayerOne].Width / 2, playerOneSelectedPoseSpritesToChooseFrom[poseNumberPlayerOne].Height / 2),
                        0.7f,
                        SpriteEffects.None,
                        0.99f
                        );

                    if (player1.WasDoublePoseScore[i])
                    {
                        _spriteBatch.Draw(
                        twoForPoseStillDoubleScore,
                        new Vector2(120, 440 + i * picSpacing - deltaLeftForPosePatternDisplayByMatch),
                        null,
                        Color.Red * 0.7f,
                        0f,
                        new Vector2(twoForPoseStillDoubleScore.Width / 2, twoForPoseStillDoubleScore.Height / 2),
                        0.7f,
                        SpriteEffects.None,
                        1f
                        );
                    }
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
                        new Vector2(1770, 440 + i * picSpacing - deltaLeftForPosePatternDisplayByMatch),
                        null,
                        Color.White * poseStillAndRingOpacity,
                        0f,
                        new Vector2(playerTwoSelectedPoseSpritesToChooseFrom[poseNumberPlayerTwo].Width / 2, playerTwoSelectedPoseSpritesToChooseFrom[poseNumberPlayerTwo].Height / 2),
                        0.7f,
                        SpriteEffects.None,
                        0.99f
                        );

                    if (player2.WasDoublePoseScore[i])
                    {
                        _spriteBatch.Draw(
                        twoForPoseStillDoubleScore,
                        new Vector2(1770, 440 + i * picSpacing - deltaLeftForPosePatternDisplayByMatch),
                        null,
                        Color.Red * 0.7f,
                        0f,
                        new Vector2(twoForPoseStillDoubleScore.Width / 2, twoForPoseStillDoubleScore.Height / 2),
                        0.7f,
                        SpriteEffects.None,
                        1f
                        );
                    }
                }

                _spriteBatch.Draw(
                      _blackScreenBackground,
                      new Vector2(960, 540),
                      null,
                      Color.White * blackScreenOpacity,
                      0f,
                      new Vector2(_blackScreenBackground.Width / 2, _blackScreenBackground.Height / 2),
                      new Vector2(1f, 1f),
                      SpriteEffects.None,
                      1f
                      );

                //test
                //_sparklerSprites.Draw(_spriteBatch, false, currentAI);

                float lightsScale = 1.6f;
                float lightsOpacity = .7f;
                if (aiTurn)
                {
                    if (aiTurn && (roundNumber == 1) && (currentAI.GetPose() == poses[upIndexCurrentAIIdlePoseIndexForRingGirl]))
                    {
                        DisplayLights(_ringGirlLightsBackground, lightsOpacity, lightsScale, false, 0.95f);
                    }
                    else
                    {
                        DisplayLights(_aiLightsBackground, lightsOpacity, lightsScale, false, 0.95f);
                        DisplayLights(_aiLightsColored, 0.3f, lightsScale, true, 0.89f);
                        DisplayLights(_aiLightsColored, 0.06f, lightsScale, true, 0.95f);
                        _sparklerSprites.Draw(_spriteBatch, true, currentAI);
                        //DisplayLights(_aiLightsColored2, 0.3f, lightsScale, true, 0.89f);

                    }

                }
                else if (playerTurn && !dontDisplayOutcome)
                {
                    DisplayLights(_playerLightsBackground, lightsOpacity, lightsScale, false, 0.95f);
                }
                else if (dontDisplayOutcome)
                {
                    if (introTurn)
                    {
                        DisplayLights(_playerTwoLightsBackground, lightsOpacity, lightsScale, false, 0.95f);
                    }
                    else
                    {
                        Player winner = WinningPlayer();
                        if (winner == player1)
                        {
                            DisplayLights(_playerOneLightsBackground, lightsOpacity, lightsScale, false, 0.95f);
                        }
                        else
                        {
                            DisplayLights(_playerTwoLightsBackground, lightsOpacity, lightsScale, false, 0.95f);
                        }
                    }

                }
                else
                {

                }


            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawMatchScores(Texture2D matchOutcomeTexture, int x, int y)
        {
            _spriteBatch.Draw(
               matchOutcomeTexture,
               new Vector2(x, y),
               null,
               Color.Yellow,
               0f,
               new Vector2(matchOutcomeTexture.Width / 2, matchOutcomeTexture.Height / 2),
               1.7f,//needs adjusting, SCALE
               SpriteEffects.None,
               1.0f//not sure about this value
               );
        }

        private void DrawRoundScores(Texture2D texture, int x, int y)
        {
            _spriteBatch.Draw(
               texture,
               new Vector2(x, y),
               null,
               Color.White,
               0f,
               new Vector2(texture.Width / 2, texture.Height / 2),
               1f,
               SpriteEffects.None,
               0.94f
               );
        }

        private void DisplayLights(Texture2D texture, float opacity, float scale, bool isColoredLight, float layer)
        {
            Color color = Color.White;
            PoseName aIPose = currentAI.GetPose().GetPoseName();

            if (isColoredLight)
            {
                switch (aIPose)
                {
                    case PoseName.Idle:
                        color = Color.White;//don't actually need this one but just in case will put here
                        break;
                    case PoseName.LowHands:
                        color = Color.FromNonPremultiplied(254, 242, 141, 256);
                        break;
                    case PoseName.Pointing:
                        color = Color.FromNonPremultiplied(131, 129, 190, 256);
                        break;
                    case PoseName.OneHandUp:
                        color = Color.FromNonPremultiplied(155, 205, 236, 256);
                        break;
                    case PoseName.HighHands:
                        color = Color.FromNonPremultiplied(205, 153, 155, 256);
                        break;
                    case PoseName.Hercules:
                        color = Color.FromNonPremultiplied(164, 202, 156, 256);
                        break;
                    default:
                        break;
                }
            }

            _spriteBatch.Draw(
                texture,
                new Vector2(960, 540),
                null,
                color * opacity,
                0f,
                new Vector2(texture.Width / 2, texture.Height / 2),
                scale,
                SpriteEffects.None,
                layer
                );
        }

        public Player ComparePoses(Player player, Player AI, int poseIndex, out bool isTwoPointPlayerVictory)
        {
            PoseName poseName1 = player.GetPosePattern()[poseIndex].GetPoseName();//got an out of range exception here?***
            PoseName poseName2 = AI.GetPosePattern()[poseIndex].GetPoseName();
            isTwoPointPlayerVictory = false;

            if (poseName1 == poseName2)
            {
                player.PoseOutcomes[poseIndex] = 3;
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
                player.PoseOutcomes[poseIndex] = 1;
                if(poseName2 == poseName1SecondPoseItBeats)
                {
                    isTwoPointPlayerVictory = true;
                }
                return player;
            }
            else
            {
                player.PoseOutcomes[poseIndex] = 2;
                return AI;
            }
        }

        private Player WinningPlayer()
        {
            Player winner = player2;

            if (player1.GetScore() > player2.GetScore())
            {
                winner = player1;
            }
            else if (player1.GetScore() == player2.GetScore())
            {
                if (playerOneFinishedFirst)
                {
                    winner = player1;
                }
            }

            return winner;
        }

        private void NewRound()
        {
            Player winner = WinningPlayer();
            winner.roundScore++;

            for (int i = 0; i < 5; i++)
            {
                player1.PoseOutcomes[i] = 0;
                player2.PoseOutcomes[i] = 0;
                player1.WasDoublePoseScore[i] = false;
                player2.WasDoublePoseScore[i] = false;
            }

            player1.SetScore(0);
            player2.SetScore(0);
            player1.CrowdMoving = false;
            player2.CrowdMoving = false;

            player1.SetPose(poses[0]);
            player1.SetPosePattern(new List<Pose>());
            player2.SetPose(poses[6]);
            player2.SetPosePattern(new List<Pose>());
            int upIndexCurrentAIIdlePoseIndex = (matchNumber - 1) * 6 + 12;
            currentAI.SetPose(poses[upIndexCurrentAIIdlePoseIndex]);


            player1.displayCircle = DisplayCircle.Tied;
            player2.displayCircle = DisplayCircle.Tied;

            soundEffects[5].CreateInstance().Play();
            roundNumber++;
            counter = counterStart = counter2 = counterStart2 = counterAI = 3;

            numPosesDisplayedAI = 0;
            player1CanInput = true;
            player2CanInput = true;
            playerTurn = false;
            aiTurn = true;
            updatedScoreBoard = false;

            if (roundNumber > 3)
            {
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

            for (int i = 0; i < 3; i++)
            {
                player1.RoundOutcomes[i] = 0;
                player2.RoundOutcomes[i] = 0;
            }


            matchNumber++;
            counterAI = 5;

            if (matchNumber > 3)
            {
                ResetGame();
            }
            else
            {
                currentAI = AIPlayerList[matchNumber - 1];

                if (!IsExit)
                {
                    currentAI._AIIntroSound.CreateInstance().Play();
                }

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

            //currentAI._AIIntroSound.CreateInstance().Play();

            player1.SetScore(0);
            player2.SetScore(0);
            player1.roundScore = 0;
            player2.roundScore = 0;
            player1.matchScore = 0;
            player2.matchScore = 0;
            showingFinalMatchScore = false;
            IsExit = false;
        }
    }


}
