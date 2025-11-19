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
        List<Cloud> clouds = new List<Cloud>();
        int i = 0;
        public int currentState;
        float randomizerTimer;
        int omc = 1;
        int obstacleMax;
        public bool offering;
        public float speed = 15;
        public bool gamePaused = false;
        List<string> cloudRegionNames = new List<string> {"Cloud1", "Cloud2" , "Cloud3" , "Cloud4" , "Cloud5" , "Cloud6" };
        public ObstacleManager(TextureAtlas rT)
        {
            rock = rT;
            randomizerTimer = Core.Randomizer.Next(2000, 4000)/1000;
            bgPiece = TextureAtlas.FromFile(Core.Content, "BGElement.xml");
            clouds.Add(new Cloud(bgPiece,new Vector2(Core.Randomizer.Next(2000,5000), Core.Randomizer.Next(50, 128)), cloudRegionNames[Core.Randomizer.Next(0,5)]));
            clouds.Add(new Cloud(bgPiece, new Vector2(Core.Randomizer.Next(2000, 5000), Core.Randomizer.Next(50, 128)), cloudRegionNames[Core.Randomizer.Next(0, 5)]));
            clouds.Add(new Cloud(bgPiece, new Vector2(Core.Randomizer.Next(2000, 5000), Core.Randomizer.Next(50, 128)), cloudRegionNames[Core.Randomizer.Next(0, 5)]));
            clouds.Add(new Cloud(bgPiece, new Vector2(Core.Randomizer.Next(2000, 5000), Core.Randomizer.Next(50, 128)), cloudRegionNames[Core.Randomizer.Next(0, 5)]));
            clouds.Add(new Cloud(bgPiece, new Vector2(Core.Randomizer.Next(2000, 5000), Core.Randomizer.Next(50, 128)), cloudRegionNames[Core.Randomizer.Next(0, 5)]));
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
            foreach (Obstacle obstacle in allObstacles)
            {
                obstacle.characterSprite.Draw(Core.SpriteBatch, obstacle.position);
            }
            foreach (Cloud obstacle in clouds)
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
                    currentState = 0;
                break;
            }
        }
    }
}
