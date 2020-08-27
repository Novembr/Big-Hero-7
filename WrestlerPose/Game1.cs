using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace WrestlerPose
{
    public class Game1 : Game
    {
        //chosen textures
        //Texture2D textureAtPosition1;
        //Texture2D textureAtPosition2;
        //positions
        Vector2 WrestlerPosition1;
        Vector2 WrestlerPosition2;

        Player player1;
        Player player2;

        //added textures
        Texture2D WrestlerTexture1;
        Texture2D WrestlerTexture2;
        Texture2D WrestlerTexture3;
        Texture2D WrestlerTexture4;
        Texture2D WrestlerTexture5;
        Texture2D WrestlerTextureIdle;

        List<Texture2D> wrestlerTextures = new List<Texture2D>(6);
        List<Pose> poses = new List<Pose>(6);


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



            //load textures
            WrestlerTextureIdle = Content.Load<Texture2D>("WrestlerIdle");
            WrestlerTexture1 = Content.Load<Texture2D>("Wrestler1");
            WrestlerTexture2 = Content.Load<Texture2D>("Wrestler2");
            WrestlerTexture3 = Content.Load<Texture2D>("Wrestler3");
            WrestlerTexture4 = Content.Load<Texture2D>("Wrestler4");
            WrestlerTexture5 = Content.Load<Texture2D>("Wrestler6");//preferred the image at 6 over the one at 5, 5 is too large, not sure how to resize in here yet

            //place textures in texture list
            //definitely smarter ways to fill out this list
            wrestlerTextures.Add(WrestlerTextureIdle);
            wrestlerTextures.Add(WrestlerTexture1);
            wrestlerTextures.Add(WrestlerTexture2);
            wrestlerTextures.Add(WrestlerTexture3);
            wrestlerTextures.Add(WrestlerTexture4);
            wrestlerTextures.Add(WrestlerTexture5);

            //replace this with player texture at position
            //textureAtPosition1 = WrestlerTextureIdle;
            //textureAtPosition2 = WrestlerTextureIdle;

            //setup each pose with it's pose name enum and associated texture, Poses may want more members 
            //using arbitrary number here
            for (int i = 0; i < 6; i++)
            {
                poses.Add(new Pose(wrestlerTextures[i], (PoseName)i));
            }

            player1 = new Player("Player One", WrestlerPosition1, poses[0]);
            player2 = new Player("Player Two", WrestlerPosition2, poses[0]);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var inputState = Keyboard.GetState();

            //below is kind of awkward and not very safe, just a bunch of conditionals with hard coded indexes that incidentally correspond to poses
            //along with hard coded input, is it worth it to do some input refactoring, so the keyboard input state is not directly exposed
            //here but instead there's some layer of abstraction that handles input more elegantly? may not be worth doing for prototype
            if (inputState.IsKeyDown(Keys.A))
            {
                player1.SetPose(poses[1]);
            }
            else if (inputState.IsKeyDown(Keys.S))
            {
                player1.SetPose(poses[2]);
            }
            else if (inputState.IsKeyDown(Keys.D))
            {
                player1.SetPose(poses[3]);
            }
            else if (inputState.IsKeyDown(Keys.F))
            {
                player1.SetPose(poses[4]);
            }
            else if (inputState.IsKeyDown(Keys.G))
            {
                player1.SetPose(poses[5]);
            }
            else if (inputState.IsKeyDown(Keys.NumPad1))
            {
                player2.SetPose(poses[1]);
            }
            else if (inputState.IsKeyDown(Keys.NumPad2))
            {
                player2.SetPose(poses[2]);
            }
            else if (inputState.IsKeyDown(Keys.NumPad3))
            {
                player2.SetPose(poses[3]);
            }
            else if (inputState.IsKeyDown(Keys.NumPad4))
            {
                player2.SetPose(poses[4]);
            }
            else if (inputState.IsKeyDown(Keys.NumPad5))
            {
                player2.SetPose(poses[5]);
            }

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
                    counter = counterStart;//Reset the counter;
                                           //any actions to perform
                    roundNumber++;
                    //here will call the compare poses***
                    //BUT either you can change your pose, and it visibly changes, as many times as you like during countdown, and then completes
                    //them when the count is 0, like player vs player, OR, the players wrestle pose changes as many times as they like during countdown
                    //but the AI will retain an idle pose until the last final moment
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

                    if(overallWinnerString != null)
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
            _spriteBatch.Draw(
                player1.GetPose().GetTexture(),
                player1.GetPosition(),
                //new Rectangle(50, 50, 300,300), //this can resize, seems to mess with images, might be bad for fixed size photos and not sprites, dunno
                null,
                Color.White,
                0f,
                new Vector2(player1.GetPose().GetTexture().Width / 2, player1.GetPose().GetTexture().Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
                );
            _spriteBatch.Draw(
                player2.GetPose().GetTexture(),
                player2.GetPosition(),
                null,
                Color.White,
                0f,
                new Vector2(player2.GetPose().GetTexture().Width / 2, player2.GetPose().GetTexture().Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
                );

            //_spriteBatch.DrawString(_countDown, counter.ToString(), new Vector2(10, 10), Color.Yellow);
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
            _spriteBatch.DrawString(_title, "Wrestler Mania! ", new Vector2(850, 50), Color.Firebrick, 0, Vector2.Zero, 3, new SpriteEffects(),1);


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

            //pictures not currently matching enums? i think they are but with new animations and sprites they will have to be manually
            //made to match, and of course nothing here is animated so rather than a list of textures it will be some kind of list of 
            //animations I guess
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
            if(player1.GetScore() >= 3)
            {
                return player1.GetName();//so player 1 would win if they both got to score 3 simultaneously but that should never happen
            }
            else if(player2.GetScore() >= 3)
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

            if(matchNumber > 3)
            {
                ResetGame();
            }
            //call some kind of if matchnumber > 3 then restart game totally
            //score is just for winning a match, if you don't win the match then the match resets? that's only against the ai
            //against the player it continues either way
        }

        private void ResetGame()
        {
            matchNumber = 0;
            counterStart = 15;
            counter = 15;

        }
    }


}
