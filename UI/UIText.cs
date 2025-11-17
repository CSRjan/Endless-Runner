using Game_Library;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.GueDeriving;

namespace Endless_Runner.UI
{
    public class UIText : TextRuntime
    {
        public string TextFormat { get; set; }
        public string displayText { get; set; } = "Text";
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public float LayerDepth { get; set; } = 0.0f;
        public Anchor anchor;
        public UIText(string givenText, Vector2 position)
        {
            Text = givenText;
            displayText = givenText;
            Position = position;
        }
        public void format(string format, object? argument0)
        {
            TextFormat = format;
            Text = string.Format(TextFormat, argument0);
        }
        public void formatting(string format, object? argument0)
        {
            TextFormat = format;
            Text = string.Format(TextFormat, displayText, argument0);
        }
        public void formatting(string format, object? argument0, object? argument1)
        {
            TextFormat = format;
            displayText = string.Format(TextFormat, Text, argument0, argument1);
        }
        public void formatting(string format, object? argument0, object? argument1, object? argument2)
        {
            TextFormat = format;
            displayText = string.Format(TextFormat, Text, argument0, argument1, argument2);
        }

        public void DrawText(SpriteFont font)
        {
            if(TextFormat == null)
            {
                Core.SpriteBatch.DrawString(font, Text, Position, Color, Rotation, Origin, Scale, Effects, LayerDepth);
            }
            else
            {
                Core.SpriteBatch.DrawString(font, displayText, Position, Color, Rotation, Origin, Scale, Effects, LayerDepth);
            }
        }
    }
}
