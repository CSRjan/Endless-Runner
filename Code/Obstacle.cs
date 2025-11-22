
using Game_Library;
using Game_Library.Graphics;
using Microsoft.Xna.Framework;

namespace Endless_Runner.Code
{
    internal class Obstacle
    {
        public bool gamePaused;
        public Vector2 position = Vector2.Zero;
        public Sprite characterSprite;
        public Rectangle collider;
        public Vector2 velocity = Vector2.Zero;
        public bool resetting;
        public int startPoint = 2000;
        public int endBound = -500;
        float timer;
        public Obstacle(TextureAtlas atlas, Vector2 Position, Vector2 colliderWidth, string region)
        {
            position = Position;
            resetting = true;
            characterSprite = new Sprite(atlas.GetRegion(region));
            collider = new Rectangle(new Point((int)position.X, (int)position.Y + 7), new Point((int)colliderWidth.X, (int)colliderWidth.Y));
            velocity = Vector2.Zero;
        }

        public void Update()
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

        public void randomizeSpot(int beginningSpot, int endingSpot)
        {
            position.X = Core.Randomizer.Next(beginningSpot, endingSpot);
        }
    }
}
