using Game_Library.Graphics;
using Microsoft.Xna.Framework;

namespace Endless_Runner.Code
{
    internal class Cloud
    {
        public bool gamePaused;
        public Vector2 position = Vector2.Zero;
        public Sprite characterSprite;
        public Vector2 velocity = Vector2.Zero;
        float terminalVelocity = 30;
        public Cloud(TextureAtlas atlas, Vector2 Position, string region)
        {
            position = Position;
            characterSprite = new Sprite(atlas.GetRegion(region));
            velocity = Vector2.Zero;
        }

        public void Update()
        {
            position.X += velocity.X;
            velocity.X = clamp(velocity.X, -terminalVelocity, 0);
            position.Y -= velocity.Y;
        }

        float clamp(float input, float min, float max)
        {
            if (input < min)
            {
                return min;
            }
            else if (input > max)
            {
                return max;
            }
            else
            {
                return input;
            }
        }
    }
}
