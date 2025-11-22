using Game_Library.Graphics;
using Microsoft.Xna.Framework;

namespace Endless_Runner.Code
{
    internal class Plant
    {
        public bool gamePaused;
        public Vector2 position = Vector2.Zero;
        public Sprite characterSprite;
        public Rectangle collider;
        public Vector2 velocity = Vector2.Zero;
        float terminalVelocity = 35;
        public bool resetting = true;
        public int endBound = -500;
        public Plant(TextureAtlas atlas, Vector2 Position, string region)
        {
            position = Position;
            characterSprite = atlas.CreateSprite(region);
            velocity = Vector2.Zero;
        }

        public void Update()
        {
            if (!gamePaused)
            {
                if (resetting)
                {

                }
                else
                {
                    if (position.X <= endBound)
                    {
                        velocity = Vector2.Zero;
                    }
                    else
                    {
                        position.X += velocity.X;
                        position.Y -= velocity.Y;
                        collider.X = (int)position.X;
                        collider.Y = (int)position.Y;
                    }
                }
            }
        }
    }
}
