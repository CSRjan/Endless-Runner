using Endless_Runner.Code.Scenes;
using Game_Library;
using Game_Library.Graphics;
using Game_Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Endless_Runner.Code;

public class Game1 : Core
{
    public Game1() : base("Runner", 1920, 1080, true)
    {

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
        InitializeGum();
        ChangeScene(new Runner());
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }

    private void InitializeGum()
    {
        
    }

}