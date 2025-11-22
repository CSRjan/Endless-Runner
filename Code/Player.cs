using Endless_Runner.UI;
using Game_Library;
using Game_Library.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace Endless_Runner.Code
{
    public class Player
    {
        public bool gamePaused;
        public TextureAtlas _atlas;
        public TextureAtlas _satlas;
        public Vector2 position = Vector2.Zero;
        public AnimatedSprites characterSprite;
        public AnimatedSprites soulSprite;
        public Rectangle collider;
        Vector2 offset = new Vector2(150, 0);
        public Vector2 velocity = Vector2.Zero;
        public float terminalVelocityUpwards = 47;
        public float terminalVelocity = 30;
        public int health = 2;
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
        public bool hasHealthCharge = true;
        public bool HealthChargeActivated = false;
        public float defaultChargeAmount = 2000;
        public float chargeAmountLeft;
        public float horiSpeed = 3;
        public Sprite waxTablet;
        public SpriteFontText offerText;
        public string currentOffer = "Resurrection";
        string newOffer;
        public string description;
        float jumpBufferTimer = .50f;
        /// <summary>
        /// "Run", "CrouchJumpingUp", "JumpingUp", "JumpingDown", "CrouchJumpingDown", "Hurt", "Parry"
        /// </summary>
        public List<string> animationNames;
        int state;
        float downwardsSpeed = 2;
        public Player(TextureAtlas atlas, TextureAtlas satlus, Vector2 Position, Vector2 colliderSize, Vector2 colliderOffset, string animation, List<string> anim) 
        {
            position = Position;
            _atlas = atlas;
            _satlas = satlus;
            waxTablet = new Sprite(new TextureRegion(Core.Content.Load<Texture2D>("Art Assets\\Wax Tablet"), 0, 0, 1280, 720), new Vector2(320,180));
            chargeAmountLeft = defaultChargeAmount;
            characterSprite = _atlas.CreateAnimatedSprite(animation);
            soulSprite = _satlas.CreateAnimatedSprite("Flame1/4");
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
                if (invul)
                {
                    if (health <= 0)
                    {
                        if (currentOffer == "Resurrection")
                        {
                            state = 6;
                            health = maxHealth;
                            hasResurrection = false;
                            currentOffer = "None";
                        }
                        else
                        {
                            state = 6;
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
                        if (HealthChargeActivated)
                        {
                            health = maxHealth;
                            invul = true;
                            chargeAmountLeft = defaultChargeAmount;
                            HealthChargeActivated = false;
                        }
                        else
                        {
                            health--;
                            invul = true;
                        }
                    }
                }
                if (HealthChargeActivated)
                {
                    if(chargeAmountLeft == 0)
                    {
                        chargeAmountLeft = defaultChargeAmount;
                        health++;
                        HealthChargeActivated = false;
                    }
                    else
                    {
                        chargeAmountLeft--;
                        soulAnimManager(string.Empty);
                    }
                }
                if (health < 2)
                {
                    if (dead)
                    {
                        ChangeAnimation("HHurt");
                    }
                    else
                    {
                        animManager("H");
                    }
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
            }
        }

        public void ChangeAnimation(string animation)
        {
            if(characterSprite.Animation.Name != animation)
            {
                characterSprite.swapAnimations(_atlas.GetAnimation(animation));
            }
        }
        public void ChangeSoulAnimation(string animation)
        {
            if (soulSprite.Animation.Name != animation)
            {
                soulSprite.swapAnimations(_satlas.GetAnimation(animation));
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

        void soulAnimManager(string s)
        {
            switch (chargeAmountLeft)
            {
                case < 100:
                    ChangeSoulAnimation("FlameDone");
                    break;
                case < 1000:
                    ChangeSoulAnimation("Flame3/4");
                    break;
                case < 1500:
                    ChangeSoulAnimation("Flame2/4");
                    break;
                case < 2000:
                    ChangeSoulAnimation("Flame1/4");
                    break;
            }
        }
        public void playerDrawCalls()
        {
            characterSprite.Draw(Core.SpriteBatch, position);
            if (HealthChargeActivated)
            {
                soulSprite.Draw(Core.SpriteBatch, position - new Vector2(0,soulSprite.Height));
            }
        }
        public void instantiateOffering()
        {
            offerText.Position = waxTablet.position + new Vector2(57,34);
            int i = Core.Randomizer.Next(0, 100);
            switch (i)
            {
                case <= 50:
                    if(i <= 25)
                    {
                        if (currentOffer == newOffer)
                        {
                            newOffer = "Speed-Up";
                            description = "At will, your scores will increase faster, \nalthough the world itself will speed up as well.";
                        }
                        else
                        {
                            newOffer = "Resurrection";
                            description = "When damaged to death, your wounds will turn a golden bronze\n before disappearing, rejuvinating the user.";
                        }
                    }
                    else if(i > 25)
                    {
                        if (currentOffer == newOffer)
                        {
                            newOffer = "Resurrection";
                            description = "When damaged to death, your wounds will turn a golden bronze\n before disappearing, rejuvinating the user.";
                        }
                        else
                        {
                            newOffer = "Speed-Up";
                            description = "At will, your scores will increase faster, \nalthough the world itself will speed up as well.";
                        }
                    }
                        break;
                case > 50:
                    if (i <= 75)
                    {
                        if (currentOffer == newOffer)
                        {
                            newOffer = "Health Charge";
                            description = "When activated, you will be charge your spirit until your until your spirit burns bright.\n Make the distance without being hit for an extra health, or else lose all your extra health.";
                        }
                        else
                        {
                            newOffer = "Freemove";
                            description = "At will, you will be able to move freely around the area, \nyour score will increase faster the more right you run.";
                        }
                    }
                    else if (i > 75)
                    {
                        if (currentOffer == newOffer)
                        {
                            newOffer = "Freemove";
                            description = "At will, you will be able to move freely around the area, \nyour score will increase faster the more right you run.";
                        }
                        else
                        {
                            newOffer = "Health Charge";
                            description = "When activated, you will be charge your spirit until your until your spirit burns bright.\n Make the distance without being hit for an extra health, or else lose all your extra health.";
                        }
                    }
                    break;
            }
            offerText.formatting("New Offer: {0}\nInfo: {1}", newOffer, description);
        }

        public void SetOffer()
        {
            speedUpActivated = false;
            FreeMoveActivated = false;
            HealthChargeActivated = false;
            currentOffer = newOffer;
            switch (currentOffer)
            {
                case "Resurrection":
                    hasResurrection = true;
                    break;
                case "Speed-Up":
                    hasSpeedUp = true;
                    speedUpActivated = false;
                    break;
                case "Freemove":
                    hasFreeMove = true;
                    break;
                case "Health Charge":
                    hasHealthCharge = true;
                    break;
            }
        }
        public void Jump()
        {
            if (Core.Input.GamePads[0].IsConnected)
            {
                if (Core.Input.GamePads[0].WasButtonJustPressed(Buttons.A) && !invul)
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
            }
            else
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
                if (Core.Input.GamePads[0].IsConnected)
                {
                    if ((Core.Input.GamePads[0].IsButtonDown(Buttons.DPadLeft) || Core.Input.GamePads[0].LeftThumbStick.X < 0) && !invul)
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
                    else if ((Core.Input.GamePads[0].IsButtonDown(Buttons.DPadRight) || Core.Input.GamePads[0].LeftThumbStick.X > 0) && !invul)
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
                    if ((Core.Input.Keyboard.IsKeyDown(Keys.A) && !invul))
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
                    else if ((Core.Input.Keyboard.IsKeyDown(Keys.D) && !invul))
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
