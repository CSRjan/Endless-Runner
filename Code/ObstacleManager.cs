using Game_Library;
using Game_Library.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Endless_Runner.Code
{
    internal class ObstacleManager
    {
        List<Obstacle> allObstacles = new List<Obstacle>();
        TextureAtlas rock;
        TextureAtlas bgPiece;
        List<Plant> plants = new List<Plant>();
        int i = 0;
        int iC = 0;
        public int currentState;
        float randomizerTimer;
        float cloudRandomizerTimer;
        int omc = 1;
        int obstacleMax;
        float terminalVelocity = 35;
        public bool offering;
        public float speed = 15;
        public bool gamePaused = false;
        public ObstacleManager(TextureAtlas rT)
        {
            rock = rT;
            randomizerTimer = Core.Randomizer.Next(2000, 4000)/1000;
            cloudRandomizerTimer = Core.Randomizer.Next(1000, 11000) / 1000;
            bgPiece = TextureAtlas.FromFile(Core.Content, "BGElement.xml");
            plants.Add(new Plant(bgPiece,new Vector2(2000, 1080-256), "Plant" + Core.Randomizer.Next(1,12)));
            plants.Add(new Plant(bgPiece, new Vector2(2000, 1080 - 256), "Plant" + Core.Randomizer.Next(1, 12)));
            plants.Add(new Plant(bgPiece, new Vector2(2000, 1080 - 256), "Plant" + Core.Randomizer.Next(1, 12)));
            plants.Add(new Plant(bgPiece, new Vector2(2000, 1080 - 256), "Plant" + Core.Randomizer.Next(1, 12)));
            plants.Add(new Plant(bgPiece, new Vector2(2000, 1080 - 256), "Plant" + Core.Randomizer.Next(1, 12)));
            plants.Add(new Plant(bgPiece, new Vector2(2000, 1080 - 256), "Plant" + Core.Randomizer.Next(1, 12)));
        }

        public void instatiateObstacle(Obstacle o)
        {
            allObstacles.Add(o);
        }

        public void Update()
        {
            if (!gamePaused)
            {
                if (randomizerTimer <= 0)
                {
                    i = Core.Randomizer.Next(0, 5);
                    int im = Core.Randomizer.Next(0, 15 * omc);
                    if(im <= 15)
                    {
                        allObstacles[i].characterSprite.Region = rock.GetRegion("Rock");
                        allObstacles[i].collider.Size = new Point(128, 128);
                    }
                    else
                    {
                        allObstacles[i].characterSprite.Region = rock.GetRegion("WideRock");
                        allObstacles[i].collider.Size = new Point(384, 128);
                    }
                    allObstacles[i].resetting = false;
                    randomizerTimer = Core.Randomizer.Next(2000, 4000) / 1000;
                }
                else
                {
                    randomizerTimer -= Core.Deltatime;
                }
                if (cloudRandomizerTimer <= 0)
                {
                    int ix = Core.Randomizer.Next(1, 12);
                    iC = Core.Randomizer.Next(0, 5);
                    while (!plants[iC].resetting)
                    {
                        iC = Core.Randomizer.Next(0, 5);
                    }
                    if (randomizerTimer > .9f)
                    {
                        plants[iC].resetting = false;
                        plants[iC].characterSprite.Region = bgPiece.GetRegion("Plant" + ix);
                        cloudRandomizerTimer = Core.Randomizer.Next(1500, 4000) / 1000;
                    }
                }
                else
                {
                    cloudRandomizerTimer -= Core.Deltatime;
                }
                foreach (Obstacle o in allObstacles)
                {
                    o.Update();
                    if (o.position.X > o.endBound)
                    {
                        o.velocity = (Vector2.UnitX * -speed);
                    }
                    else
                    {
                        o.position.X = o.startPoint;
                        o.resetting = true;
                    }
                }
                foreach (Plant o in plants)
                {
                    o.Update();
                    if (o.position.X > o.endBound)
                    {
                        o.velocity = (Vector2.UnitX * -speed);
                    }
                    else
                    {
                        o.position.X = 2000;
                        o.resetting = true;
                    }
                }
                speed = clamp(speed, 0, terminalVelocity);
            }
        }

        public bool CollisionCheck(Rectangle collision)
        {
            foreach (Obstacle o in allObstacles)
            {
                if (collision.Intersects(o.collider))
                {
                    return true;
                }
            }
            return false;
        }

        public void DrawLoop()
        {
            foreach (Plant obstacle in plants)
            {
                obstacle.characterSprite.Draw(Core.SpriteBatch, obstacle.position);
            }
            foreach (Obstacle obstacle in allObstacles)
            {
                obstacle.characterSprite.Draw(Core.SpriteBatch, obstacle.position);
            }
        }

        public void updateState()
        {
            currentState++;
            switch (currentState)
            {
                case 5:
                    if (omc != obstacleMax)
                    {
                        omc++;
                    }
                break;
                case 10:
                    speed++;
                    offering = true;
                    currentState = 0;
                    break;
            }
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
