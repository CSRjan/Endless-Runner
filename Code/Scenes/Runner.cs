using Endless_Runner.UI;
using Game_Library;
using Game_Library.Graphics;
using Game_Library.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Endless_Runner.Code.Scenes
{
    public class Runner : Scene
    {
        //Title
        string TITLE_STRING = "Runner Demo";
        SpriteFont titleFont;
        SpriteFontText titleText;
        Vector2 titleTextPosition;
        string INPUT_STRING = "Press Enter to Start";
        SpriteFontText inputText;
        Vector2 inputTextPosition;
        bool transition;
        int transparency = 255;
        //Game itself
        string SCORE_STRING = "";
        string HSSCOREKM_FORMAT = "High Score: {0}";
        string SCORE_FORMAT = "Score: {0}";
        string HEALTH_FORMAT = "Health: {0}";
        int score = 1;
        int scoreIncrement = 1;
        int exScore;
        int highScore;
        int finalScore;
        SpriteFont scoreFont;
        SpriteFontText hsScoreText;
        SpriteFontText scoreText;
        SpriteFontText healthText;
        SpriteFontText gameOverText;
        Vector2 scoreTextPosition;
        private TextureAtlas _obatlas;
        Sprite bg;
        Player player;
        ObstacleManager obstacleManager;
        bool gamePaused = true;
        bool titleScreen = true;
        float bufferOffering = 1.5f;
        Rectangle ground = new Rectangle(0, 1080, 1920, 10);
        List<string> animations = new List<string>() { "Run", "CrouchJumpingUp", "JumpingUp", "JumpingDown", "CrouchJumpingDown", "Hurt", "HRHurt"};
        public override void Initialize()
        {
            initializeGame();
            initializeTitle();
            base.Initialize();
        }

        public override void LoadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CheckKeyboardInput();
            if (titleScreen)
            {
                player.characterSprite.Update(gameTime);
                if (transition)
                {
                    gameTransiton();
                    if(transparency <= 0)
                    {
                        hsScoreText.Position = Vector2.Zero;
                        hsScoreText.Color = Color.Black;
                        transition = false;
                        gamePaused = false;
                        player.gamePaused = false;
                        obstacleManager.gamePaused = false;
                        titleScreen = false;
                    }
                }
            }
            else
            {
                if (!gamePaused)
                {
                    if (player.dead)
                    {
                        SaveManager.instance.sd.highScore = highScore;
                        SaveManager.instance.Save();
                        gamePaused = true;
                        player.characterSprite.stop = true;
                        gameOverText.Position = new Vector2(1920 / 2, 1080 / 2);
                    }
                    else
                    {
                        player.hit = obstacleManager.CollisionCheck(player.collider);
                        player.onGround = player.collider.Intersects(ground);
                        if (player.onGround)
                        {
                            player.position.Y = ground.Top - 512;
                        }
                        
                        if (!gamePaused)
                        {
                            score += scoreIncrement;
                            finalScore = score + exScore;
                            scoreText.format(SCORE_FORMAT, finalScore);
                            healthText.format(HEALTH_FORMAT, player.health);
                            if (score % 1000 == 0)
                            {
                                obstacleManager.speed++;
                                obstacleManager.updateState();
                                if (obstacleManager.offering)
                                {

                                    player.instantiateOffering();
                                    score++;
                                    gamePaused = true;
                                }
                            }
                            if (finalScore >= highScore)
                            {
                                highScore = finalScore;
                                hsScoreText.format(HSSCOREKM_FORMAT, highScore);
                            }
                        }
                        switch(player.position.X)
                        {
                            case < 352:
                                exScore += 1;
                                break;
                            case < 704:
                                exScore += 2;
                                break;
                            case < 1056:
                                exScore += 4;
                                break;
                            case < 1408:
                                exScore += 6;
                                break;
                        }
                        player.gamePaused = gamePaused;
                        obstacleManager.gamePaused = gamePaused;
                        player.Update();
                        player.characterSprite.Update(gameTime);
                        obstacleManager.Update();
                    }
                }
            }
        }
        public override void Draw(GameTime gameTime)
        {
            bg.Draw(Core.SpriteBatch, bg.position);
            if (titleScreen)
            {
                titleText.DrawText(titleFont);
                inputText.DrawText(scoreFont);
                hsScoreText.DrawText(scoreFont);
            }
            else
            {
                bg.Draw(Core.SpriteBatch, bg.position);
                hsScoreText.DrawText(scoreFont);
                scoreText.DrawText(scoreFont);
                healthText.DrawText(scoreFont);
                gameOverText.DrawText(scoreFont);
                player.offerText.DrawText(scoreFont);
                obstacleManager.DrawLoop();
            }
            player.characterSprite.Draw(Core.SpriteBatch, player.position);
        }


        private void CheckKeyboardInput()
        {
            switch (player.currentOffer)
            {
                case "None":
                    break;
                case "Speed-Up":
                    if (Core.Input.Keyboard.WasKeyJustPressed(Keys.P) && !player.speedUpActivated)
                    {
                        obstacleManager.speed += 6;
                        scoreIncrement = 2;
                        player.speedUpActivated = true;
                    }
                    else if (Core.Input.Keyboard.WasKeyJustPressed(Keys.P) && player.speedUpActivated)
                    {
                        obstacleManager.speed -= 6;
                        scoreIncrement = 1;
                        player.speedUpActivated = false;
                    }
                        break;
                case "Freemove":
                    if (Core.Input.Keyboard.WasKeyJustPressed(Keys.P) && !player.FreeMoveActivated)
                    {
                        player.FreeMoveActivated = true;
                    }
                    else if (Core.Input.Keyboard.WasKeyJustPressed(Keys.P) && player.FreeMoveActivated)
                    {
                        player.FreeMoveActivated = false;
                    }
                    break;
            }
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter) && !transition && titleScreen)
            {
                transition = true;
            }
            if (Core.Input.Keyboard.IsKeyDown(Keys.F12))
            {
                int i = 0;
                i++;
            }
            if (bufferOffering <= 0 && obstacleManager.offering)
            {
                if (Core.Input.Keyboard.IsKeyDown(Keys.P))
                {
                    player.SetOffer();
                    obstacleManager.offering = false;
                    bufferOffering = 1.5f;
                    gamePaused = false;
                    player.offerText.Position = new Vector2(2000, 2000);
                }
                if (Core.Input.Keyboard.IsKeyDown(Keys.Space))
                {
                    obstacleManager.offering = false;
                    bufferOffering = 1.5f;
                    gamePaused = false;
                    player.offerText.Position = new Vector2(2000, 2000);
                }
            }
            else if (bufferOffering > 0 && obstacleManager.offering)
            {
                bufferOffering -= Core.Deltatime;
            }
            if (player.dead)
            {
                if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
                {
                    Core.ChangeScene(new Runner());
                }
            }
            if (Core.Input.Keyboard.IsKeyDown(Keys.Escape))
            {
                Core.ExitGame = true;
            }
        }
        void initializeTitle()
        {
            titleFont = Core.Content.Load<SpriteFont>("Font/Title");
            titleTextPosition = new Vector2(5, 0);
            titleText = new SpriteFontText(TITLE_STRING, titleTextPosition,Vector2.Zero);
            titleText.Color = Color.Black;

            Vector2 temp = scoreFont.MeasureString(INPUT_STRING);
            inputTextPosition = new Vector2(0, 250);
            inputText = new SpriteFontText(INPUT_STRING, inputTextPosition, Vector2.Zero);
            inputText.Color = Color.Black;
            Vector2 temp2 = scoreFont.MeasureString(hsScoreText.textValue);
            hsScoreText.Position = new Vector2(0,200);
            player.gamePaused = true;
            obstacleManager.gamePaused = true;
        }
        void initializeGame()
        {
            // TODO: Add your initialization logic here
            bg = new Sprite(new TextureRegion(Content.Load<Texture2D>("Art Assets/BG"), 0, 0, 1920, 1080));
            bg.position = Vector2.Zero;
            //Initialize all Obstacles here
            _obatlas = TextureAtlas.FromFile(Core.Content, "Obstacle.xml");
            obstacleManager = new ObstacleManager(_obatlas);
            obstacleManager.instatiateObstacle(new Obstacle(_obatlas, new Vector2(1960, ground.Top - 128), new Vector2(128, 128), "Rock"));
            obstacleManager.instatiateObstacle(new Obstacle(_obatlas, new Vector2(1960, ground.Top - 128), new Vector2(128, 128), "Rock"));
            obstacleManager.instatiateObstacle(new Obstacle(_obatlas, new Vector2(1960, ground.Top - 128), new Vector2(128, 128), "Rock"));
            obstacleManager.instatiateObstacle(new Obstacle(_obatlas, new Vector2(1960, ground.Top - 128), new Vector2(128, 128), "Rock"));
            obstacleManager.instatiateObstacle(new Obstacle(_obatlas, new Vector2(1960, ground.Top - 128), new Vector2(128, 128), "Rock"));
            obstacleManager.instatiateObstacle(new Obstacle(_obatlas, new Vector2(1960, ground.Top - 128), new Vector2(128, 128), "Rock"));
            //Initialize the player
            player = new Player(TextureAtlas.FromFile(Core.Content, "RunnerXML.xml"), new Vector2(10, 1080-512), new Vector2(256, 512), new Vector2(256 / 2, 7), "Run", animations);
            //Initialize the text
            scoreFont = Core.Content.Load<SpriteFont>("Font/Gameplay");
            hsScoreText = new SpriteFontText(SCORE_STRING, scoreTextPosition, Color.Black);
            highScore = SaveManager.instance.sd.highScore;
            hsScoreText.format(HSSCOREKM_FORMAT, highScore);
            scoreText = new SpriteFontText(SCORE_STRING, scoreTextPosition + new Vector2(0, 50), Color.Black);
            healthText = new SpriteFontText(SCORE_STRING, scoreTextPosition + new Vector2(0, 100), Color.Black);
            gameOverText = new SpriteFontText("Game Over\nPress Enter to Restart", new Vector2(1920 * 2, 1080 * 2), Color.Black);
            player.offerText = new SpriteFontText("Press Jump to accept the offer\n", new Vector2(1920 * 2, 1080 * 2), Color.Black);
        }

        void gameTransiton()
        {
            transparency-=5;
            hsScoreText.Color = new Color(0, 0, 0, transparency);
            titleText.Color = new Color(0, 0, 0, transparency);
            inputText.Color = new Color(0, 0, 0, transparency);
        }
    }
}
