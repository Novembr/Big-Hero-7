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

        Player player1;
        Player player2;

        List<Animation> animations = new List<Animation>(12);
        List<Pose> poses = new List<Pose>(12);


        //countdown timer
        //i got this counter timer basic setup from online somewhere but changed it to count down and not up
        //here https://stackoverflow.com/questions/13394892/how-to-create-a-timer-counter-in-c-sharp-xna
        //dunno if that sort of thing needs to be documented
        int counter = 15;
        int counterStart = 15;
        float countDuration = 1f;
        float currentTime = 0f;
        int roundNumber = 1;
        string overallWinnerString = "";
        int matchNumber = 1;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _countDown;
        private SpriteFont _playerOneOutcome;
        private SpriteFont _playerTwoOutcome;
        private SpriteFont _playerOneScore;
        private SpriteFont _playerTwoScore;
        private SpriteFont _round;
        private SpriteFont _match;
        private SpriteFont _overAllWinner;
        private SpriteFont _title;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1900;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();

            WrestlerPosition1 = new Vector2(_graphics.PreferredBackBufferWidth / 4, _graphics.PreferredBackBufferHeight / 2);
            WrestlerPosition2 = new Vector2(_graphics.PreferredBackBufferWidth * 3 / 4, _graphics.PreferredBackBufferHeight / 2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _countDown = Content.Load<SpriteFont>("Countdown");
            _playerOneOutcome = Content.Load<SpriteFont>("PlayerOneOutcome");
            _playerTwoOutcome = Content.Load<SpriteFont>("PlayerTwoOutcome");
            _playerOneScore = Content.Load<SpriteFont>("PlayerOneScore");
            _playerTwoScore = Content.Load<SpriteFont>("PlayerTwoScore");
            _round = Content.Load<SpriteFont>("Round");
            _match = Content.Load<SpriteFont>("Match");
            _overAllWinner = Content.Load<SpriteFont>("OverallWinner");
            _title = Content.Load<SpriteFont>("Title");

            //DOUBled number of animations because same one can't be in two places it seems and
            //will need different ones anyway once getting new characters etc...
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/wrestleidleonerow"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/download"), 6));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/capguy-walk"), 8));
            animations.Add(new Animation(Content.Load<Texture2D>("onehandup"), 1));
            animations.Add(new Animation(Content.Load<Texture2D>("pointing"), 1));
            animations.Add(new Animation(Content.Load<Texture2D>("hercules"), 1));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/wrestleidleonerow"), 12));
            animations.Add(new Animation(Content.Load<Texture2D>("twohandsdown"), 1));
            animations.Add(new Animation(Content.Load<Texture2D>("twohandsup"), 1));
            animations.Add(new Animation(Content.Load<Texture2D>("onehandup"), 1));
            animations.Add(new Animation(Content.Load<Texture2D>("astronaught"), 9));
            animations.Add(new Animation(Content.Load<Texture2D>("Animations/wrestleidleonerow"), 12));

            for (int i = 0; i < 12; i++)
            {
                int poseInt = i;
                if(i > 5)
                {
                    poseInt = poseInt - 5;
                }
                poses.Add(new Pose(animations[i], (PoseName)poseInt));
            }

            player1 = new Player("Player One", WrestlerPosition1, poses[0]);
            player2 = new Player("Player Two", WrestlerPosition2, poses[6]);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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

            //below is kind of awkward and not very safe, just a bunch of conditionals with hard coded indexes that incidentally correspond to poses
            //along with hard coded input, is it worth it to do some input refactoring, so the keyboard input state is not directly exposed
            //here but instead there's some layer of abstraction that handles input more elegantly? may not be worth doing for prototype
            //poses[0] and poses[6] are idle poses
            if (inputState.IsKeyDown(Keys.A) || ((playerOneLeftStick == StickDirection.Down) && (playerOneRightStick == StickDirection.Down)))
            {
                player1.SetPose(poses[1]);
            }
            else if (inputState.IsKeyDown(Keys.S) || ((playerOneLeftStick == StickDirection.Up) && (playerOneRightStick == StickDirection.Up)))
            {
                player1.SetPose(poses[2]);
            }
            else if (inputState.IsKeyDown(Keys.D) || ((playerOneLeftStick == StickDirection.Up) && (playerOneRightStick == StickDirection.Down)))
            {
                player1.SetPose(poses[3]);
            }
            else if (inputState.IsKeyDown(Keys.F) || ((playerOneLeftStick == StickDirection.Up) && (playerOneRightStick == StickDirection.Right)))
            {
                player1.SetPose(poses[4]);
            }
            else if (inputState.IsKeyDown(Keys.G) || ((playerOneLeftStick == StickDirection.Left) && (playerOneRightStick == StickDirection.Right)))
            {
                player1.SetPose(poses[5]);
            }
            else if (inputState.IsKeyDown(Keys.NumPad1) || ((playerTwoLeftStick == StickDirection.Down) && (playerTwoRightStick == StickDirection.Down)))
            {
                player2.SetPose(poses[7]);
            }
            else if (inputState.IsKeyDown(Keys.NumPad2) || ((playerTwoLeftStick == StickDirection.Up) && (playerTwoRightStick == StickDirection.Up)))
            {
                player2.SetPose(poses[8]);
            }
            else if (inputState.IsKeyDown(Keys.NumPad3) || ((playerTwoLeftStick == StickDirection.Up) && (playerTwoRightStick == StickDirection.Down)))
            {
                player2.SetPose(poses[9]);
            }
            else if (inputState.IsKeyDown(Keys.NumPad4) || ((playerTwoLeftStick == StickDirection.Up) && (playerTwoRightStick == StickDirection.Right)))
            {
                player2.SetPose(poses[10]);
            }
            else if (inputState.IsKeyDown(Keys.NumPad5) || ((playerTwoLeftStick == StickDirection.Left) && (playerTwoRightStick == StickDirection.Right)))
            {
                player2.SetPose(poses[11]);
            }

            //update animations:
            //seems like only one sprite copy for each so can't both display same sprite at same time?
            player1.GetPose().GetSprite().Update(gameTime, player1.GetPosition());
            player2.GetPose().GetSprite().Update(gameTime, player2.GetPosition());

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

            if ((player1.GetPose().GetPoseName() != 0) && (player2.GetPose().GetPoseName() != 0))
            {
                overallWinnerString = null;//putting this here to reset the overall winner once the game ends

                currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentTime >= countDuration)
                {
                    counter--;
                    currentTime -= countDuration;
                }
                if (counter < 0)
                {
                    counter = counterStart;
                    roundNumber++;
                    Player winner = ComparePoses(player1, player2);
                    //should separate all the below out into some ResolveOutcome() type function rather than just putting it in update
                    //that displays proper values and sets scores etc...
                    if (winner != null)
                    {
                        if (winner == player1)
                        {
                            player1.SetCurrentOutcome("Winner");
                            player2.SetCurrentOutcome("Loser");
                            player1.SetScore(player1.GetScore() + 1);
                        }
                        else
                        {
                            player1.SetCurrentOutcome("Loser");
                            player2.SetCurrentOutcome("Winner");
                            player2.SetScore(player2.GetScore() + 1);
                        }
                    }
                    else
                    {
                        //should make some kind of outcomes enum rather than just hard coded strings
                        player1.SetCurrentOutcome("Tie");
                        player2.SetCurrentOutcome("Tie");
                    }

                    //do some kind of check score here
                    overallWinnerString = CheckOverallWinner(player1, player2);

                    if (overallWinnerString != null)
                    {
                        //somebody won
                        ResetMatch();
                    }

                    player1.SetPose(poses[0]);
                    player2.SetPose(poses[0]);

                    //then pause at the end of this to display final outcome for a few seconds and then restart timer?
                    //need to do that later, will have to mess with timer so it pauses while waiting at the round victory screen
                    //could just change both players pose back to idle once round ends
                }
            }
            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            player1.GetPose().GetSprite().Draw(_spriteBatch);
            player2.GetPose().GetSprite().Draw(_spriteBatch);

            _spriteBatch.DrawString(_countDown, counter.ToString(), new Vector2(10, 10), Color.Yellow, 0, Vector2.Zero, 3, new SpriteEffects(), 1);

            //make below color changes based on wether won or lost?
            //so rather than having outcome as a string it could be a class with a string name and a color and
            //maybe some other stuff and above player outcome is set to that class rather than a hardcoded string
            _spriteBatch.DrawString(_playerOneOutcome, "Last Round Outcome: " + player1.GetCurrentOutcome(), new Vector2(200, 200), Color.Red);
            _spriteBatch.DrawString(_playerTwoOutcome, "Last Round Outcome: " + player2.GetCurrentOutcome(), new Vector2(1500, 200), Color.Red);
            _spriteBatch.DrawString(_playerOneScore, "Player 1 Score:  " + player1.GetScore(), new Vector2(200, 100), Color.YellowGreen);
            _spriteBatch.DrawString(_playerTwoScore, "Player 2 Score:  " + player2.GetScore(), new Vector2(1500, 100), Color.YellowGreen);
            _spriteBatch.DrawString(_match, "Match: " + matchNumber, new Vector2(100, 50), Color.Red);
            _spriteBatch.DrawString(_round, "Round: " + roundNumber, new Vector2(100, 100), Color.Orange);

            //so below is a test scaled up text, but it is of course distorted and pixellated, so there 
            _spriteBatch.DrawString(_title, "Wrestler Mania! ", new Vector2(850, 50), Color.Firebrick, 0, Vector2.Zero, 3, new SpriteEffects(), 1);


            //would be whitespace at first
            if (!String.IsNullOrWhiteSpace(overallWinnerString))
            {
                //_spriteBatch.DrawString(_overAllWinner, "Overall Winner: " + overallWinnerString, new Vector2(500, 150), Color.Fuchsia);
                _spriteBatch.DrawString(_overAllWinner, "Overall Winner: " + overallWinnerString, new Vector2(850, 150), Color.Fuchsia, 0, Vector2.Zero, 2, new SpriteEffects(), 1);

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        //don't need to feed in player 1 and 2 really? because those are public variables already accessible in this class
        public Player ComparePoses(Player player1, Player player2)
        {
            PoseName poseName1 = player1.GetPose().GetPoseName();
            PoseName poseName2 = player2.GetPose().GetPoseName();

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
                return player1;
            }
            else
            {
                return player2;
            }
        }

        private string CheckOverallWinner(Player player1, Player player2)
        {
            if (player1.GetScore() >= 3)
            {
                return player1.GetName();//so player 1 would win if they both got to score 3 simultaneously but that should never happen
            }
            else if (player2.GetScore() >= 3)
            {
                return player2.GetName();
            }
            else
            {
                return null;
            }
        }

        private void ResetMatch()
        {
            player1.SetPose(poses[0]);
            player2.SetPose(poses[0]);
            //overallWinnerString = null;//don't set it here because then you never see the overall winner displayed, instead set it 
            //back to null once someone has initiated a new game of 3 rounds I guess by selecting a non idle pose again?
            roundNumber = 1;
            matchNumber++;
            counterStart = (4 - matchNumber) * 5;
            counter = counterStart;
            player1.SetScore(0);
            player2.SetScore(0);
            player1.SetCurrentOutcome("Pending");
            player2.SetCurrentOutcome("Pending");

            if (matchNumber > 3)
            {
                ResetGame();
            }
        }

        private void ResetGame()
        {
            matchNumber = 0;
            counterStart = 15;
            counter = 15;

        }
    }


}
