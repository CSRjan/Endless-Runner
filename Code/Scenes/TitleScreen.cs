using Endless_Runner.UI;
using Game_Library;
using Game_Library.Graphics;
using Game_Library.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
namespace Endless_Runner.Code.Scenes
{
    internal class TitleScreen :Scene
    {
        string TITLE_STRING = "Runner Demo";
        SpriteFont titleFont;
        SpriteFontText titleText;
        Vector2 titleTextPosition;
        string INPUT_STRING = "Press Enter to Start";
        SpriteFontText inputText;
        Vector2 inputTextPosition;
        Player player;
        public override void Initialize()
        {
            // TODO: Add your initialization logic here
            InitializeUI();
            base.Initialize();
            titleFont = Core.Content.Load<SpriteFont>("Title");
            Vector2 temp = titleFont.MeasureString(TITLE_STRING);
            titleTextPosition = new Vector2(1280 / 2, 0 + temp.Y);
            titleText = new SpriteFontText(TITLE_STRING, titleTextPosition, new Vector2(temp.X / 2, temp.Y / 2));
            titleText.Color = Color.Black;

            Vector2 temp2 = titleFont.MeasureString(INPUT_STRING);
            inputTextPosition = new Vector2(1280 / 2, 720/2);
            inputText = new SpriteFontText(INPUT_STRING, inputTextPosition, new Vector2(temp2.X / 2, temp2.Y / 2));
            inputText.Color = Color.Black;
            player = new Player(TextureAtlas.FromFile(Core.Content, "RunnerXML.xml"), new Vector2(10, 1080-512), new Vector2(256, 512), new Vector2(256 / 2, 7), "Run");
            player.gamePaused = true;
        }

        public override void LoadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            CheckKeyboardInput();
            player.characterSprite.Update(gameTime);
            GumService.Default.Update(gameTime);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            titleText.DrawText(titleFont);
            inputText.DrawText(titleFont);
            player.characterSprite.Draw(Core.SpriteBatch, player.position);
        }

        private void InitializeUI()
        {
            // Clear out any previous UI in case we came here from
            // a different screen:
            GumService.Default.Root.Children.Clear();
            CreateTitlePanel();
        }

        
        private void CheckKeyboardInput()
        {
            // If the W or Up keys are down, move the slime up on the screen.
            if (Core.Input.Keyboard.IsKeyDown(Keys.Escape))
            {
                Core.ExitGame = true;
            }

            // If the W or Up keys are down, move the slime up on the screen.
            if (Core.Input.Keyboard.IsKeyDown(Keys.Enter))
            {
                Core.ChangeScene(new Runner());
            }
        }

        private void CreateTitlePanel()
        {
            // Create a container to hold all of our buttons
            
        }
    }
}
