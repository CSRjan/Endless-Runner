using Endless_Runner.UI;
using Game_Library;
using Game_Library.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Endless_Runner.Code
{
    public class Player
    {
        public bool gamePaused;
        public TextureAtlas _atlas;
        public Vector2 position = Vector2.Zero;
        public AnimatedSprites characterSprite;
        public Rectangle collider;
        Vector2 offset = new Vector2(150, 0);
        public Vector2 velocity = Vector2.Zero;
        public float terminalVelocityUpwards = 47;
        public float terminalVelocity = 30;
        public int health = 1;
        public int maxHealth = 2;
        public bool dead;
        public bool invul = false;
        public bool jumpingUpwards = false;
        public bool onGround = true;
        bool jumpBuffer;
        public bool hit = false;
        public bool hasResurrection = false;
        public bool hasSpeedUp = true;
        public bool speedUpActivated = false;
        public bool hasFreeMove = true;
        public bool FreeMoveActivated = false;
        public float horiSpeed = 3;
        public SpriteFontText offerText;
        public string currentOffer = "None";
        string newOffer;
        string description;
        float jumpBufferTimer = .50f;
        /// <summary>
        /// "Run", "CrouchJumpingUp", "JumpingUp", "JumpingDown", "CrouchJumpingDown", "Hurt", "Parry"
        /// </summary>
        public List<string> animationNames;
        int state;
        float downwardsSpeed = 2;
        public Player(TextureAtlas atlas, Vector2 Position, Vector2 colliderSize, Vector2 colliderOffset, string animation, List<string> anim) 
        {
            position = Position;
            _atlas = atlas;
            characterSprite = _atlas.CreateAnimatedSprite(animation);
            collider = new Rectangle(new Point((int)position.X + (int)colliderOffset.X, (int)position.Y + (int)colliderOffset.Y), new Point((int)colliderSize.X, (int)colliderSize.Y));
            velocity = Vector2.Zero;
            animationNames = anim;
        }

        public void Update()
        {
            if (!gamePaused)
            {
                Jump();
                freeMove();
                if (health < 2)
                {
                    animManager("H");
                }
                else
                {
                    animManager(String.Empty);
                }
                position.X += velocity.X;
                position.Y -= velocity.Y;
                velocity.Y = clamp(velocity.Y, -terminalVelocity, terminalVelocityUpwards);
                collider.X = (int)position.X + (int)offset.X;
                collider.Y = (int)position.Y + 7;
                if (invul)
                {
                    if (health <= 0)
                    {
                        if(currentOffer == "Resurrection")
                        {
                            state = 6;
                            health = maxHealth;
                            hasResurrection = false;
                            currentOffer = "None";
                        }
                        else
                        {
                            dead = true;
                        }
                    }
                    else
                    {
                        if (!hit)
                        {
                            invul = false;
                        }
                    }
                }
                else
                {
                    if (hit)
                    {
                        health--;
                        invul = true;
                    }
                }
            }
        }

        public void ChangeAnimation(string animation)
        {
            if(characterSprite.Animation.Name != animation)
            {
                characterSprite.swapAnimations(_atlas.GetAnimation(animation));
            }
        }

        float clamp(float input, float min, float max)
        {
            if(input < min)
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

        void animManager(string s)
        {
            switch (state)
            {
                case 0:
                    ChangeAnimation(s + animationNames[0]);
                    if (invul)
                    {
                        state = 5;
                    }
                    break;
                case 2:
                    ChangeAnimation(s + animationNames[2]);
                    if (velocity.Y < 0)
                    {
                        state++;
                    }
                    if (invul)
                    {
                        state = 5;
                    }
                    break;
                case 3:
                    ChangeAnimation(s + animationNames[3]);
                    if (onGround)
                    {
                        state = 4;
                    }
                    if (invul)
                    {
                        state = 5;
                    }
                    break;
                case 4:
                    ChangeAnimation(s + animationNames[4]);
                    if (characterSprite._animation.finished)
                    {
                        state = 0;
                    }
                    if (invul)
                    {
                        state = 5;
                    }
                    break;
                case 5:
                    ChangeAnimation(s + animationNames[5]);
                    if (!invul)
                    {
                        if (velocity.Y > 0)
                        {
                            state = 2;
                        }
                        else if (velocity.Y < 0)
                        {
                            state = 3;
                        }
                        else if (velocity.Y == 0)
                        {
                            state = 0;
                        }
                    }
                    break;
                case 6:
                    ChangeAnimation(s + animationNames[6]);
                    if (!invul)
                    {
                        if (velocity.Y > 0)
                        {
                            state = 2;
                        }
                        else if (velocity.Y < 0)
                        {
                            state = 3;
                        }
                        else if (velocity.Y == 0)
                        {
                            state = 0;
                        }
                    }
                    break;
            }
        }

        public void instantiateOffering()
        {
            offerText.Position = new Vector2(1920/3,1080/3);
            if (hasResurrection)
            {
                currentOffer = "Resurrection";
                newOffer = "Parry";
                description = "Hit the action button within 2 frames of being hit\n to negate any damage.";
            }
            else
            {
                currentOffer = "None";
                int i = Core.Randomizer.Next(0, 50);
                if(i <= 25)
                {
                    newOffer = "Parry";
                    description = "Hit the action button within 2 frames of being hit\n to negate any damage.";
                }
                else
                {
                    newOffer = "Resurrection";
                    description = "Be resurrected upon death, at full health, one use per run.";
                }
            }
            description += "\nPress Jump(Space) to Deny The Blessing\nPress Special(P) to accept it";
            offerText.formatting("Current Offer: {0}\nNew Offer: {1}\nInfo: {2}",currentOffer, newOffer, description);
        }

        public void SetOffer()
        {
            switch (currentOffer)
            {
                case "None":
                    break;
                case "Resurrection":
                    hasResurrection = false;
                    break;
            }
            currentOffer = newOffer;
            switch (currentOffer)
            {
                case "Resurrection":
                    hasResurrection = true;
                    break;
            }
        }
        public void Jump()
        {
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Space) && !invul)
            {
                if (onGround)
                {
                    jumpingUpwards = true;
                    state = 2;
                    velocity.Y = terminalVelocityUpwards;
                }
                else if (!jumpingUpwards)
                {
                    jumpBuffer = true;
                }
            }
            if (jumpBuffer)
            {
                if (jumpBufferTimer <= 0)
                {
                    jumpBuffer = false;
                    jumpBufferTimer = .50f;
                }
                else
                {
                    jumpBufferTimer -= Core.Deltatime;
                    if (onGround)
                    {
                        jumpingUpwards = true;
                        state = 2;
                        velocity.Y = terminalVelocityUpwards;
                        jumpBuffer = false;
                        jumpBufferTimer = .50f;
                    }
                }
            }
            if (jumpingUpwards)
            {
                if (velocity.Y <= 0)
                {
                    jumpingUpwards = false;
                }
                else
                {
                    velocity.Y -= downwardsSpeed;
                }

            }
            else
            {
                if (onGround)
                {
                    velocity.Y = 0;
                }
                else
                {
                    velocity.Y -= downwardsSpeed;
                }
            }
        }

        void freeMove()
        {
            if (FreeMoveActivated)
            {
                if (Core.Input.Keyboard.IsKeyDown(Keys.A) && !invul)
                {
                    if (position.X > 0)
                    {
                        velocity.X = -horiSpeed;
                    }
                    else
                    {
                        velocity.X = 0;
                    }
                }
                else if (Core.Input.Keyboard.IsKeyDown(Keys.D) && !invul)
                {
                    if (position.X < 1408)
                    {
                        velocity.X = horiSpeed;
                    }
                    else
                    {
                        velocity.X = 0;
                    }
                }
                else
                {
                    velocity.X = 0;
                }
            }
            else
            {
                if (position.X > 0)
                {
                    velocity.X = -horiSpeed;
                }
                else
                {
                    velocity.X = 0;
                }
            }
        }
    }
}
