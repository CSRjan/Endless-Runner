using Game_Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endless_Runner.UI
{
    public class SpriteFontText
    {
        public string textValue { get; set; }
        public string TextFormat { get; set; }
        public string displayText { get; set; } = "Text";
        public Color Color { get; set; } = Color.Black;
        public float Rotation { get; set; } = 0.0f;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public float LayerDepth { get; set; } = 0.0f;
        public SpriteFontText(string givenText, Vector2 position)
        {
            textValue = givenText;
            displayText = givenText;
            Position = position;
        }
        public SpriteFontText(string givenText, Vector2 position, Color color)
        {
            textValue = givenText;
            displayText = givenText;
            Position = position;
            Color = color;
        }

        public SpriteFontText(string givenText, Vector2 position, Vector2 origin)
        {
            textValue = givenText;
            Origin = origin;
            displayText = givenText;
            Position = position;
        }
        public void format(string format, object? argument0)
        {
            TextFormat = format;
            textValue = string.Format(TextFormat, argument0);
        }
        public void formatting(string format, object? argument0)
        {
            TextFormat = format;
            textValue = string.Format(TextFormat, argument0);
        }
        public void formatting(string format, object? argument0, object? argument1)
        {
            TextFormat = format;
            textValue = string.Format(TextFormat, argument0, argument1);
        }
        public void formatting(string format, object? argument0, object? argument1, object? argument2)
        {
            TextFormat = format;
            textValue = string.Format(TextFormat, argument0, argument1, argument2);
        }

        public void DrawText(SpriteFont font)
        {
            Core.SpriteBatch.DrawString(font, textValue, Position, Color, Rotation, Origin, Scale, Effects, LayerDepth);
        }
    }
}
