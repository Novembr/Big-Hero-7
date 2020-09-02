using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        //Player firstAI;
        Player currentAI;

        List<Player> AIPlayerList;// = new List<Player>(3);


        const int numAnimations = 18;

        List<Animation> animations = new List<Animation>(numAnimations);
        List<Animation> outcomeAnimations = new List<Animation>(4);
        List<Sprite> displayCircles = new List<Sprite>(6);
        List<Pose> poses = new List<Pose>(numAnimations);
        List<Pose> outComePoses = new List<Pose>(4);



        //countdown timer
        //i got this counter timer basic setup from online somewhere but changed it to count down and not up
        //here https://stackoverflow.com/questions/13394892/how-to-create-a-timer-counter-in-c-sharp-xna
        //dunno if that sort of thing needs to be documented

        //player 1 counter
        int counter = 3;
        int counterStart = 3;
        float countDuration = .2f;
        float currentTime = 0f;

        //player 2 counter:
        int counter2 = 3;
        int counterStart2 = 3;
        float countDuration2 = .2f;
        float currentTime2 = 0f;

        //ai round timer counter:
        int counterAI = 3;//this should be set to the time for the first animation of the first ai to run, or to run multiple times i guess
        int counterStartAI = 10;
        float countDurationAI = .3f;
        float currentTimeAI = 0f;

        float roundTimer = 0;
        int roundNumber = 1;
        string overallWinnerString = "";
        int matchNumber = 1;

        //private int numAiPosesThisRound = 3;
        bool playerTurn = false;
        bool aiTurn = true;//game not starting because of this?
        int numPosesDisplayedAI = 0;
        bool thisRoundTallied = false;


        bool player1CanInput = true;
        bool player2CanInput = true;




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
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            int test = _graphics.PreferredBackBufferWidth;
            int test2 = _graphics.PreferredBackBufferHeight;

            WrestlerPosition1 = new Vector2(3450, 700); // new Vector2(_graphics.PreferredBackBufferWidth / 4, _graphics.PreferredBackBufferHeight / 2);
            WrestlerPosition2 = new Vector2(4650, 700);//new Vector2(_graphics.PreferredBackBufferWidth * 3 / 4, _graphics.PreferredBackBufferHeight / 2);
            AIPosition = new Vector2(3200, 300); //new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 3);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //can these 2 all load the same thing because it's just an empty sprite?
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
            _allPosesImage = Content.Load<Texture2D>("AllPosesImage");
            _stageBackground = Content.Load<Texture2D>("Stage");




            List<string> stillAnimationImageNameStrings = new List<string>(5) { "twohandsdown", "pointing", "onehandup", "twohandsup", "hercules" };
            for (int i = 0; i < 5; i++)
            {
                playerOneSelectedPoseSpritesToChooseFrom.Add(Content.Load<Texture2D>(stillAnimationImageNameStrings[i]));
                playerTwoSelectedPoseSpritesToChooseFrom.Add(Content.Load<Texture2D>(stillAnimationImageNameStrings[i]));
            }

            //DOUBled number of animations because same one can't be in two places it seems and
            //will need different ones anyway once getting new characters etc...
            /*
             LowHands,
            Pointing,
            OneHandUp,
            HighHands,
            Hercules
             */
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
            //player 3 animations
            animations.Add(new Animation(Content.Load<Texture2D>("idlealt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("yellow&couch-alt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("purple&lean-alt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("blue&point-alt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("red&botharms-alt"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("green&back-alt"), 12));

            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("white"), 1), 1));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("green"), 1), 1));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("red"), 1), 1));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("white"), 1), 1));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("green"), 1), 1));
            displayCircles.Add(new Sprite(new Animation(Content.Load<Texture2D>("red"), 1), 1));

            outcomeAnimations.Add(new Animation(Content.Load<Texture2D>("win"), 12));
            outcomeAnimations.Add(new Animation(Content.Load<Texture2D>("lose"), 12));
            outcomeAnimations.Add(new Animation(Content.Load<Texture2D>("win"), 12));
            outcomeAnimations.Add(new Animation(Content.Load<Texture2D>("lose"), 12));

            outComePoses.Add(new Pose(outcomeAnimations[0], PoseName.Idle, 2));//scalse is 2 for player, does posename matter here though?
            outComePoses.Add(new Pose(outcomeAnimations[1], PoseName.Idle, 2));//scalse is 2 for player, does posename matter here though?
            outComePoses.Add(new Pose(outcomeAnimations[2], PoseName.Idle, 2));//scalse is 2 for player, does posename matter here though?
            outComePoses.Add(new Pose(outcomeAnimations[3], PoseName.Idle, 2));//scalse is 2 for player, does posename matter here though?



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
                poses.Add(new Pose(animations[i], (PoseName)poseInt, scale));
            }

            //make 3 players and set each to it's first animation, idle animation, 
            //initialize to empty pose list to increase and decrease as poses are added?
            player1 = new Player("Player One", WrestlerPosition1, poses[0], new List<Pose>());
            player2 = new Player("Player Two", WrestlerPosition2, poses[6], new List<Pose>());

            //so just initialize with it's first pattern, or should this be list of 2 and then the ai plays
            //it's pose list in index 0, 1, 0 or whatever? probably simpler this way even if it repeats some info

            //eventually th elist of Poses (lengths 3, 4 and 5 below) should be flipped around between rounds within a match, rather than being the same pattern
            //displayed 3 times
            AIPlayerList = new List<Player>
            {
                new Player("firstAI", AIPosition, poses[12], new List<Pose>(3) { poses[13], poses[14], poses[14] }, new List<int>{ 13, 14, 15}),
                new Player("secondAI", AIPosition, poses[12], new List<Pose>(4) { poses[14], poses[16], poses[15], poses[14] }, new List<int>{ 13, 14, 15, 17}),
                new Player("thirdAI", AIPosition, poses[12], new List<Pose>(5) { poses[17], poses[16], poses[17], poses[15], poses[13] }, new List<int>{ 13, 14, 15, 16, 17}),
            };

            currentAI = AIPlayerList[0];//later have a set currentai method rather than just always setting to first ai like here
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

            //below is kind of awkward and not very safe, just a bunch of conditionals with hard coded indexes that incidentally correspond to poses
            //along with hard coded input, is it worth it to do some input refactoring, so the keyboard input state is not directly exposed
            //here but instead there's some layer of abstraction that handles input more elegantly? may not be worth doing for prototype
            //poses[0] and poses[6] are idle poses
            //so here we are setting poses for the player, we also need to be adding the selected pose to the pose list
            //and there needs to be some kind of timer between setting the poses?
            if (playerTurn)
            {
                //there needs to be some kind of counter in here to prevent the player pose from being set 3 times in a row super rapidly from a single
                //input, so after an input has been detected and a pose added to the pattern then a timer needs to countdown before another 
                //can be accepted

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
                        player1CanInput = false;
                        counter = counterStart;

                    }
                    else if (inputState.IsKeyDown(Keys.S) || ((playerOneLeftStick == StickDirection.Up) && (playerOneRightStick == StickDirection.Up)))
                    {
                        player1.SetPose(poses[4]);//*** two hands up
                        player1.AddToPosePattern(poses[4]);
                        ComparePosesAndSetScores(player1.GetPosePattern().Count - 1, player1, currentAI);
                        player1CanInput = false;
                        counter = counterStart;

                    }
                    else if (inputState.IsKeyDown(Keys.D) || ((playerOneLeftStick == StickDirection.Up) && (playerOneRightStick == StickDirection.Down)))
                    {
                        player1.SetPose(poses[3]);
                        player1.AddToPosePattern(poses[3]);
                        ComparePosesAndSetScores(player1.GetPosePattern().Count - 1, player1, currentAI);
                        player1CanInput = false;
                        counter = counterStart;

                    }
                    else if (inputState.IsKeyDown(Keys.F) || ((playerOneLeftStick == StickDirection.Up) && (playerOneRightStick == StickDirection.Right)))
                    {
                        player1.SetPose(poses[2]); //***pointing
                        player1.AddToPosePattern(poses[2]);
                        ComparePosesAndSetScores(player1.GetPosePattern().Count - 1, player1, currentAI);
                        player1CanInput = false;
                        counter = counterStart;

                    }
                    else if (inputState.IsKeyDown(Keys.G) || ((playerOneLeftStick == StickDirection.Left) && (playerOneRightStick == StickDirection.Right)))
                    {
                        player1.SetPose(poses[5]);
                        player1.AddToPosePattern(poses[5]);
                        ComparePosesAndSetScores(player1.GetPosePattern().Count - 1, player1, currentAI);
                        player1CanInput = false;
                        counter = counterStart;

                    }
                }

                //the timer should start counting down after you have made a selection, then when it reaches 0 it stays there and you can take as long as you like to choose
                //then when you choose it starts counting down again
                currentTime2 += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentTime2 >= countDuration2)
                {
                    counter2--;
                    currentTime2 -= countDuration2;
                }
                if (counter2 < 0)
                {
                    //counter2 = counterStart2;
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
                        player2CanInput = false;
                        counter2 = counterStart2;
                    }
                    else if (inputState.IsKeyDown(Keys.NumPad2) || ((playerTwoLeftStick == StickDirection.Up) && (playerTwoRightStick == StickDirection.Up)))
                    {
                        player2.SetPose(poses[10]);//**two hands up
                        player2.AddToPosePattern(poses[10]);
                        ComparePosesAndSetScores(player2.GetPosePattern().Count - 1, player2, currentAI);
                        player2CanInput = false;
                        counter2 = counterStart2;
                    }
                    else if (inputState.IsKeyDown(Keys.NumPad3) || ((playerTwoLeftStick == StickDirection.Up) && (playerTwoRightStick == StickDirection.Down)))
                    {
                        player2.SetPose(poses[9]);
                        player2.AddToPosePattern(poses[9]);
                        ComparePosesAndSetScores(player2.GetPosePattern().Count - 1, player2, currentAI);
                        player2CanInput = false;
                        counter2 = counterStart2;
                    }
                    else if (inputState.IsKeyDown(Keys.NumPad4) || ((playerTwoLeftStick == StickDirection.Up) && (playerTwoRightStick == StickDirection.Right)))
                    {
                        player2.SetPose(poses[8]);//**pointing
                        player2.AddToPosePattern(poses[8]);
                        ComparePosesAndSetScores(player2.GetPosePattern().Count - 1, player2, currentAI);
                        player2CanInput = false;
                        counter2 = counterStart2;
                    }
                    else if (inputState.IsKeyDown(Keys.NumPad5) || ((playerTwoLeftStick == StickDirection.Left) && (playerTwoRightStick == StickDirection.Right)))
                    {
                        player2.SetPose(poses[11]);
                        player2.AddToPosePattern(poses[11]);
                        ComparePosesAndSetScores(player2.GetPosePattern().Count - 1, player2, currentAI);
                        player2CanInput = false;
                        counter2 = counterStart2;
                    }
                }

                //update animations:
                //seems like only one sprite copy for each so can't both display same sprite at same time?
                //should these only be updating during player turn? and also should probably not loop the animations now?


                if ((player1.GetPosePattern().Count >= currentAI.GetPosePattern().Count) && (player2.GetPosePattern().Count >= currentAI.GetPosePattern().Count))
                {
                    //if both players have selected all of their poses:
                    //this is effectively the post round turn
                    roundTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    thisRoundTallied = true;

                    if(player1.GetScore() > player2.GetScore())
                    {
                        player1.SetPose(outComePoses[0]);
                        player2.SetPose(outComePoses[3]);
                        //player1.roundScore++;
                    }
                    else if(player2.GetScore() > player1.GetScore())
                    {
                        player1.SetPose(outComePoses[1]);
                        player2.SetPose(outComePoses[2]);
                        //player2.roundScore++;
                    }
                    else
                    {
                        player1.SetPose(poses[0]);
                        player2.SetPose(poses[6]);
                    }

                    

                    if (roundTimer > 3000)
                    {
                        playerTurn = false;
                        roundTimer = 0;
                        NewRound();
                    }


                    //aiTurn = true;//don't do this here because will start ai turn again before doing comparisons?
                    //isntead do it in like a match restart method
                }

            }//end player turn section
            player1.GetPose().GetSprite().Update(gameTime, player1.GetPosition());
            player2.GetPose().GetSprite().Update(gameTime, player2.GetPosition());
            //displayCircles[1].Update(gameTime,/* player1.GetPosition()*/ /*new Vector2(100, 100)*/ player1.GetPosition());

            for (int i = 0; i < 3; i++)
            {
                displayCircles[i].Update(gameTime, /*player1.GetPosition()*/ new Vector2(370, 1000));
            }
            for (int i = 3; i < 6; i++)
            {
                displayCircles[i].Update(gameTime, /*player2.GetPosition()*/ new Vector2(1600, 1000));
            }

            //switch (player1.displayCircle)
            //{
            //    case DisplayCircle.Tied:
            //        displayCircles[0].Update(gameTime, player1.GetPosition() /*new Vector2(100, 100)*/);
            //        break;
            //    case DisplayCircle.Won:
            //        displayCircles[1].Update(gameTime, player1.GetPosition());
            //        break;
            //    case DisplayCircle.Lost:
            //        displayCircles[2].Update(gameTime, player1.GetPosition());
            //        break;
            //    default:
            //        break;
            //}

            //switch (player2.displayCircle)
            //{
            //    case DisplayCircle.Tied:
            //        displayCircles[3].Update(gameTime, player2.GetPosition());
            //        break;
            //    case DisplayCircle.Won:
            //        displayCircles[4].Update(gameTime, player2.GetPosition());
            //        break;
            //    case DisplayCircle.Lost:
            //        displayCircles[5].Update(gameTime, player2.GetPosition());
            //        break;
            //    default:
            //        break;
            //}


            if (aiTurn)
            {
                //need numAiPosesThisRound to increase with AI
                //perhaps add property to player that is an AI only property of something like numPoses
                //so that each ai can have a different number of poses
                //that is number of poses it displays, not number of diff poses
                //so it's pose list might have 2 poses but it displays 3, like ABA or whatever

                //need to do some kind of while in here rather than for loop?
                //for (int i = 0; i < currentAI.GetPosePattern().Count; i++)
                //need if not while here? because if while then it will never get to base.update gametime at the bottom
                if (numPosesDisplayedAI < currentAI.GetPosePattern().Count)
                {

                    /*
                     int counterAI = 10;
                     int counterStartAI = 10;
                     float countDurationAI = 1f;
                     float currentTimeAI = 0f;
                     */

                    currentTimeAI += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (currentTimeAI >= countDurationAI)
                    {
                        counterAI--;
                        currentTimeAI -= countDurationAI;
                    }
                    if (counterAI < 0)
                    {
                        //poses 12-17 are ai poses
                        //12 is idle
                        //later these will be fed in by the ai pose generator and not just hard coded to be same pose each round
                        Random rnd = new Random();
                        int randomPoseIndexFromThisAisPoseNumberslist = rnd.Next(0, currentAI._poseValuesForThisAI.Count);//***this is displaying a random selection but not setting the pose to the reandom selection?
                        int poseValueFromThisAisRandomSelection = currentAI._poseValuesForThisAI[randomPoseIndexFromThisAisPoseNumberslist];
                        //why when doing compares later does it not work? like now the display does not match the compares
                        currentAI.SetPose(poses[poseValueFromThisAisRandomSelection]);
                        currentAI.SetPosePattern(numPosesDisplayedAI, poses[poseValueFromThisAisRandomSelection]);
                        //then wait for the amount of time it takes to play the animation, which is gonna be framespeed * frame count
                        float animationTime = currentAI.GetPosePattern()[numPosesDisplayedAI].GetSprite().GetAnimationTime();
                        int roundedUp = (int)Math.Ceiling(animationTime);
                        counterAI = roundedUp * 2;//set it to play that animation twice basically? well rounded upa nd doubled would be more than twice, maybe many times more
                        //although with the still pictures we pretty much need a fixed time otherwise it would too rapidly go through them
                        numPosesDisplayedAI++;
                    }

                }

                //added this but why was it displaying at all without it?


                if (numPosesDisplayedAI >= currentAI.GetPosePattern().Count)
                {
                    aiTurn = false;
                    playerTurn = true;
                    //currentAI.SetPose(poses[12]);//doing this means I think that you miss the last one? yes you will because will be overwritten before displaying
                }
            }

            currentAI.GetPose().GetSprite().Update(gameTime, currentAI.GetPosition());
            //timer
            //mess with this later to get it to pause between rounds, etc...
            //distinct rounds not currently setup really, just a counter
            //ai not setup either just hotseat
            //also do something here like:
            //&& (player1.GetPose().GetPoseName() != 0) && (player2.GetPose().GetPoseName() != 0)
            //which is like an informal check if both players have made a move yet, and don't start counting until then
            //because checks if both no longer the initial idle state (which you cannot switch back to on your own)
            //after round ends could switch both player poses back to idle and then countdown won't start again until both
            //players have selected a new pose
            //presumably you pose should be hidden from the opponent, or not displayed i guess, until the countdown ends, because then
            //you will just be both clicking back and forth to counter one another's poses

            //this is like the new end turn condition, when the ai turn condition end is met and so is the player turn condition
            //but really should be able to set this off of whether we already had ai turn true then false, then player turn true then
            //false or something?

            //now do all of the below as poses are selected by the player? or at the end of the 2ish second period in which you have to select a pose, then display
            // the green or red or whatever under their feet, then go to another pose turn



            base.Update(gameTime);
        }

        void ComparePosesAndSetScores(int posePatternIndex, Player player, Player currentAI)
        {
            //if (
            //      (numPosesDisplayedAI >= currentAI.GetPosePattern().Count)
            //     && (player1.GetPosePattern().Count >= currentAI.GetPosePattern().Count)
            //     && (player2.GetPosePattern().Count >= currentAI.GetPosePattern().Count)
            //     )
            // {
            //  for (int i = 0; i < currentAI.GetPosePattern().Count; i++)
            // {
            // if (!thisRoundTallied)
            // {
            // Player winnerBetweenPlayerOneAndAI = ComparePoses(player1, currentAI, i);
            // Player winnerBetweenPlayerTwoAndAI = ComparePoses(player2, currentAI, i);

            Player winnerBetweenPlayerTwoAndAI = ComparePoses(player, currentAI, posePatternIndex);


            if (winnerBetweenPlayerTwoAndAI != null)
            {
                if (winnerBetweenPlayerTwoAndAI == player)
                {
                    player.SetScore(player.GetScore() + 1);
                    player.displayCircle = DisplayCircle.Won;
                    //***SET WINNING Circle draw
                }
                else
                {
                    player.SetScore(player.GetScore() - 1);
                    //***SET LOSING Circle draw
                    player.displayCircle = DisplayCircle.Lost;


                }
            }
            else
            {
                //***SET TIED Circle draw
                player.displayCircle = DisplayCircle.Tied;

                //just change scores for now and don't worry about current outcome because that would change rapidly during one round
                //player1.SetCurrentOutcome("Tie");
                //player2.SetCurrentOutcome("Tie");
                //if a tie then no score change but might put something here later
            }

            // if (winnerBetweenPlayerTwoAndAI != null)
            // {
            //     if (winnerBetweenPlayerTwoAndAI == player2)
            //     {
            //         player2.SetScore(player2.GetScore() + 1);
            //     }
            //     else
            //     {
            //         player2.SetScore(player2.GetScore() - 1);
            //     }
            // }
            // else
            // {
            //     //just change scores for now and don't worry about current outcome because that would change rapidly during one round
            //     //player1.SetCurrentOutcome("Tie");
            //     //player2.SetCurrentOutcome("Tie");
            //     //if a tie then no score change but might put something here later
            // }
            // }

            //  if (i == (currentAI.GetPosePattern().Count - 1))
            //  {
            //      roundTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //      thisRoundTallied = true;
            //      if (roundTimer > 2000)
            //      {
            //          roundTimer = 0;
            //          NewRound();
            //      }
            //  }
            // }
            //}
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

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

            //for (int i = 0; i < displayCircles.Count; i++)
            //{
            //    //displayCircles[i].Draw(_spriteBatch);
            //    displayCircles[2].Draw(_spriteBatch);
            //
            //}

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


            _spriteBatch.DrawString(_countDownPlayerOneMove, counter.ToString(), new Vector2(200, 10), Color.Yellow, 0, Vector2.Zero, 3, new SpriteEffects(), 1);
            _spriteBatch.DrawString(_countDownPlayerTwoMove, counter2.ToString(), new Vector2(1500, 10), Color.Yellow, 0, Vector2.Zero, 3, new SpriteEffects(), 1);
            _spriteBatch.DrawString(_countDownAI, counterAI.ToString(), new Vector2(890, 10), Color.Yellow, 0, Vector2.Zero, 3, new SpriteEffects(), 1);



            //make below color changes based on wether won or lost?
            //so rather than having outcome as a string it could be a class with a string name and a color and
            //maybe some other stuff and above player outcome is set to that class rather than a hardcoded string
            //_spriteBatch.DrawString(_playerOneOutcome, "Last Round Outcome: " + player1.GetCurrentOutcome(), new Vector2(200, 200), Color.Red);
            //_spriteBatch.DrawString(_playerTwoOutcome, "Last Round Outcome: " + player2.GetCurrentOutcome(), new Vector2(1500, 200), Color.Red);
            _spriteBatch.DrawString(_playerOneMatchScore, "Player 1 Match Score:  " + player1.matchScore, new Vector2(200, 100), Color.DarkRed);
            _spriteBatch.DrawString(_playerTwoMatchScore, "Player 2 Match Score:  " + player2.matchScore, new Vector2(1500, 100), Color.DarkRed);
            _spriteBatch.DrawString(_playerOneRoundScore, "Player 1 Round Score:  " + player1.roundScore, new Vector2(200, 150), Color.Red);
            _spriteBatch.DrawString(_playerTwoRoundScore, "Player 2 Round Score:  " + player2.roundScore, new Vector2(1500, 150), Color.Red);
            _spriteBatch.DrawString(_playerOneScore, "Player 1 Score:  " + player1.GetScore(), new Vector2(200, 200), Color.Pink);
            _spriteBatch.DrawString(_playerTwoScore, "Player 2 Score:  " + player2.GetScore(), new Vector2(1500, 200), Color.Pink);
            _spriteBatch.DrawString(_match, "Match: " + matchNumber, new Vector2(100, 50), Color.Red);
            _spriteBatch.DrawString(_round, "Round: " + roundNumber, new Vector2(100, 100), Color.Orange);

            //so below is a test scaled up text, but it is of course distorted and pixellated, so there 
            _spriteBatch.DrawString(_title, "Pose'em! ", new Vector2(800, 100), Color.Firebrick, 0, Vector2.Zero, 3, new SpriteEffects(), 1);

            _spriteBatch.Draw(
                    _allPosesImage,
                    new Vector2(950, 800),
                    null,
                    Color.White,
                    0f,
                    new Vector2(_allPosesImage.Width / 2, _allPosesImage.Height / 2),
                    0.5f,
                    SpriteEffects.None,
                    0f
                    );


            //would be whitespace at first
            if (!String.IsNullOrWhiteSpace(overallWinnerString))
            {
                //_spriteBatch.DrawString(_overAllWinner, "Overall Winner: " + overallWinnerString, new Vector2(500, 150), Color.Fuchsia);
                _spriteBatch.DrawString(_overAllWinner, "Overall Winner: " + overallWinnerString, new Vector2(850, 150), Color.Fuchsia, 0, Vector2.Zero, 2, new SpriteEffects(), 1);

            }

            //these seem to be working but they disappear before the last one gets displayed
            //because the next round starts automatically
            for (int i = 0; i < 5; i++)
            {
                //_spriteBatch.DrawString(_playerOneOutcome, "Last Round Outcome: " + player1.GetCurrentOutcome(), new Vector2(200, 200), Color.Red);
                if (i >= player1.GetPosePattern().Count)
                {
                    continue;
                }

                //subtracting 1 from each of them because the idle pattern is not inside of the playerOneSelectedPoseSpritesToChooseFrom lists
                int poseNumberPlayerOne = (int)player1.GetPosePattern()[i].GetPoseName() - 1;


                _spriteBatch.Draw(
                    playerOneSelectedPoseSpritesToChooseFrom[poseNumberPlayerOne],
                    new Vector2(200 + i * 100, 400),
                    null,
                    Color.White,

                    0f,
                    new Vector2(playerOneSelectedPoseSpritesToChooseFrom[poseNumberPlayerOne].Width / 2, playerOneSelectedPoseSpritesToChooseFrom[poseNumberPlayerOne].Height / 2),
                    0.1f,
                    SpriteEffects.None,
                    0f

                    );
            }

            //because the next round starts automatically
            for (int i = 0; i < 5; i++)
            {


                if (i >= player2.GetPosePattern().Count)
                {
                    continue;
                }
                int poseNumberPlayerTwo = (int)player2.GetPosePattern()[i].GetPoseName() - 1;

                _spriteBatch.Draw(
                    playerTwoSelectedPoseSpritesToChooseFrom[poseNumberPlayerTwo],
                    new Vector2(1400 + i * 100, 400),
                    null,
                    Color.White,

                    0f,
                    new Vector2(playerTwoSelectedPoseSpritesToChooseFrom[poseNumberPlayerTwo].Width / 2, playerTwoSelectedPoseSpritesToChooseFrom[poseNumberPlayerTwo].Height / 2),
                    0.1f,
                    SpriteEffects.None,
                    0f

                    );
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        //don't need to feed in player 1 and 2 really? because those are public variables already accessible in this class
        //below is instead going to be player and AI,
        public Player ComparePoses(Player player, Player AI, int poseIndex)
        {
            //PoseName poseName1 = player.GetPose().GetPoseName();
            //PoseName poseName2 = AI.GetPose().GetPoseName();
            //so issue ****is the second ai as 4 pose patterns and players still have 3 here, or maybe 0? so need to reset 
            //player pose list size to current ai size between matches?
            //it's also now trying to compare poses when the pose count for the players are zero, so in some kind of between matches phase it looks like

            PoseName poseName1 = player.GetPosePattern()[poseIndex].GetPoseName();//got an out of range exception here?***
            PoseName poseName2 = AI.GetPosePattern()[poseIndex].GetPoseName();

            //each pose beats the pose after, and the pose two after it (wrapping around with modulo)
            //so can do this comparison with the enums numbers
            if (poseName1 == poseName2)
            {
                return null;//just return null if it's a tie? might be better way
            }

            //below comparison with modulo doesn't work because of idle, which occupies the 0 slot in the posname enum
            //so I have to not use modulo and instead wrap the pose and 
            //then add 1 more, but below doesn't look very good because of it.
            //here I am just assigning the values a pose can beat to 1 and 3 ahead of it, as per the 5 way rock paper scissors spock lizard
            //diagram
            int poseName1FirstValueItBeats = ((int)poseName1 + 1);//%5;
            if (poseName1FirstValueItBeats > 5)
            {
                int newNum = poseName1FirstValueItBeats - 5;//manual modulo so if done then add 1 to skip idle
                poseName1FirstValueItBeats = newNum;
            }
            PoseName poseName1FirstPoseItBeats = (PoseName)poseName1FirstValueItBeats;
            int poseName1SecondValueItBeats = ((int)poseName1 + 3);// % 5;
            if (poseName1SecondValueItBeats > 5)
            {
                int newNum = poseName1SecondValueItBeats - 5;//manual modulo so if done then add 1 to skip idle
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

        //private string CheckOverallWinner(Player player1, Player player2)
        //{
        //    if (player1.GetScore() >= 3)
        //    {
        //        return player1.GetName();//so player 1 would win if they both got to score 3 simultaneously but that should never happen
        //    }
        //    else if (player2.GetScore() >= 3)
        //    {
        //        return player2.GetName();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        private void NewRound()
        {
            //Before resetting these pose pattersn that the display of the chosen patterns depends on, instead pause in here
            //for a few seconds and display the round winner and keep the chosen things selected, or I guess wait a few seconds utnil newround is called

            if (player1.GetScore() > player2.GetScore())
            {
                player1.roundScore++;
            }
            else if (player2.GetScore() > player1.GetScore())
            {
                player2.roundScore++;
            }

            //so this is like pose score now, and round score is what adds up to determine an overall round winner I guess test comment
            player1.SetScore(0);
            player2.SetScore(0);

            player1.SetPose(poses[0]);
            player1.SetPosePattern(new List<Pose>());//should these be reset to empty pose lists?
            player2.SetPose(poses[6]);
            player2.SetPosePattern(new List<Pose>());
            currentAI.SetPose(poses[12]);

            player1.displayCircle = DisplayCircle.Tied;
            player2.displayCircle = DisplayCircle.Tied;

            

            //potentially lots of other bools need to be reset in here?

            roundNumber++;
            //counterStart = (4 - matchNumber) * 5;
            //counter = counterStart;

            //****need to do someting in here with currentai pose list, set it to a new order of the same 4 poses, so that each round within a match is 
            //not the same

            //all of these counters are now just between moves times
            //they should eventually be set to something other than 3 at round start?
            //i think this will give 3 seconds before you can start?
            counter = counterStart = counter2 = counterStart2 = counterAI = counterStartAI = 3;

            //scores are cumulative across rounds now
            //player1.SetScore(0);
            //player2.SetScore(0);
            // player1.SetCurrentOutcome("Pending");
            //player2.SetCurrentOutcome("Pending");

            //some of these at least need to be reset?
            numPosesDisplayedAI = 0;//I think I should reset this here, maybe above tough
            player1CanInput = true;
            player2CanInput = true;//true because still won't be able to input until player turn is true
            playerTurn = false;//necessary?
            aiTurn = true;
            thisRoundTallied = false;

            if (roundNumber > 3)
            {
                thisRoundTallied = true;
                ResetMatch();//auto new match if round number greater than 3?
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


            //these 4 already done in newround() that calls this
            // player1.SetPose(poses[0]);
            // player2.SetPose(poses[6]);

            //this hits out of index ranged exception after 3rd match, but not sure why, match number should be 1 again
            currentAI = AIPlayerList[matchNumber];//.SetPose(poses[12]);//is this a ref or value?

            player1.SetPosePattern(new List<Pose>(currentAI.GetPosePattern().Count));//this count doesn't do anything, and shouldn't need to because shouldn't compare again until filled back up after beginning
            //of next round but instead for some reason it's going bacvk into compare poses
            player2.SetPosePattern(new List<Pose>(currentAI.GetPosePattern().Count));
            roundNumber = 1;
            matchNumber++;//match number starts at 1 not zero so get ai from match number before incrememnting
            //thisRoundTallied = false;//made this true again briefly to prevent out of index error from comparing poses between matches when player pose pateterns were still size 0
            //but above do they need to be set to new lists that have values and not just counts?
            //overallWinnerString = null;//don't set it here because then you never see the overall winner displayed, instead set it 
            //back to null once someone has initiated a new game of 3 rounds I guess by selecting a non idle pose again?
            //counterStart = (4 - matchNumber) * 5;
            //counter = counterStart;

            //SCORES? 
            // player1.SetScore(0);//se these scores to 0 here between matches? or will scores add up across matches? if they add up across
            //matches then don't set them to zero here, if instead they only determine match winner then set them to zero here and increment
            //up a separate match winner tally score in a new property
            //player2.SetScore(0);

            //not using this for now:
            //player1.SetCurrentOutcome("Pending");
            //player2.SetCurrentOutcome("Pending");

            //aiTurn = true;

        }

        private void ResetGame()
        {
            matchNumber = 1;
            roundNumber = 1;
            currentAI = AIPlayerList[0];
            //there's some other stuff that would likely have to be reset in here
            player1.SetScore(0);
            player2.SetScore(0);
            player1.roundScore = 0;
            player2.roundScore = 0;
            player1.matchScore = 0;
            player2.matchScore = 0;
            //counterStart = 15;
            //counter = 15;

        }
    }


}
