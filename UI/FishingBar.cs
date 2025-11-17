using Game_Library.Graphics;
using System;
using Gum.DataTypes;
using Gum.DataTypes.Variables;
using Gum.Managers;
using Microsoft.Xna.Framework;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;

namespace Endless_Runner.UI
{
    public class FishingBar : Slider
    {

        // Reference to the rectangle that visually represents the current value
        private ColoredRectangleRuntime _fillRectangle;

        ContainerRuntime topLevelContainer;
        ContainerRuntime innerContainer;
        NineSliceRuntime middleBackground;
        ContainerRuntime trackInstance;
        private bool sideways;

        /// <summary>
        /// Creates a new OptionsSlider instance using graphics from the specified texture atlas.
        /// </summary>
        /// <param name="atlas">The texture atlas containing slider graphics.</param>
        public FishingBar(TextureAtlas atlas, int width, int height, Vector2 pos, Color fillColor, bool onSide, string region)
        {
            sideways = onSide;
            if (sideways)
            {
                // Create the top-level container for all visual elements
                topLevelContainer = new ContainerRuntime();
                topLevelContainer.Height = height;
                topLevelContainer.Width = width;
                topLevelContainer.X = pos.X;
                topLevelContainer.Y = pos.Y;

                // Create the container for the slider track and decorative elements
                innerContainer = new ContainerRuntime();
                innerContainer.Height = topLevelContainer.Height;
                innerContainer.Width = topLevelContainer.Width;
                innerContainer.X = 0f;
                innerContainer.Y = 0f;
                topLevelContainer.AddChild(innerContainer);
                TextureRegion middleBackgroundRegion = atlas.GetRegion(region);
                // Create the middle track portion of the slider
                middleBackground = new NineSliceRuntime();
                middleBackground.Dock(Gum.Wireframe.Dock.FillVertically);
                middleBackground.Texture = middleBackgroundRegion.Texture;
                middleBackground.TextureAddress = TextureAddress.Custom;
                middleBackground.TextureHeight = middleBackgroundRegion.Height;
                middleBackground.TextureLeft = middleBackgroundRegion.SourceRectangle.Left;
                middleBackground.TextureTop = middleBackgroundRegion.SourceRectangle.Top;
                middleBackground.TextureWidth = middleBackgroundRegion.Width;
                middleBackground.Width = topLevelContainer.Width;
                middleBackground.Height = topLevelContainer.Height;
                middleBackground.WidthUnits = DimensionUnitType.Absolute;
                middleBackground.Dock(Gum.Wireframe.Dock.Left);
                // Create the interactive track that responds to clicks
                // The special name "TrackInstance" is required for Slider functionality
                trackInstance = new ContainerRuntime();
                trackInstance.Name = "TrackInstance";
                trackInstance.Dock(Gum.Wireframe.Dock.Fill);
                trackInstance.Height = -2f;
                trackInstance.Width = -2f;
                innerContainer.AddChild(trackInstance);
                // Create the fill rectangle that visually displays the current value
                _fillRectangle = new ColoredRectangleRuntime();
                _fillRectangle.Color = fillColor;
                _fillRectangle.Dock(Gum.Wireframe.Dock.Left);
                _fillRectangle.Width = topLevelContainer.Width - 4; // Default to 90% - will be updated by value changes
                _fillRectangle.Height = topLevelContainer.Height - 4;
                _fillRectangle.WidthUnits = DimensionUnitType.PercentageOfParent;
                _fillRectangle.HeightUnits = DimensionUnitType.Absolute;
            }
            else
            {
                // Create the top-level container for all visual elements
                topLevelContainer = new ContainerRuntime();
                topLevelContainer.Height = height;
                topLevelContainer.Width = width;
                topLevelContainer.X = pos.X;
                topLevelContainer.Y = pos.Y;

                // Create the container for the slider track and decorative elements
                innerContainer = new ContainerRuntime();
                innerContainer.Height = topLevelContainer.Height;
                innerContainer.Width = topLevelContainer.Width;
                innerContainer.X = 0f;
                innerContainer.Y = 0f;
                topLevelContainer.AddChild(innerContainer);
                TextureRegion middleBackgroundRegion = atlas.GetRegion(region);
                // Create the middle track portion of the slider
                middleBackground = new NineSliceRuntime();
                middleBackground.Dock(Gum.Wireframe.Dock.FillHorizontally);
                middleBackground.Texture = middleBackgroundRegion.Texture;
                middleBackground.TextureAddress = TextureAddress.Custom;
                middleBackground.TextureHeight = middleBackgroundRegion.Height;
                middleBackground.TextureLeft = middleBackgroundRegion.SourceRectangle.Left;
                middleBackground.TextureTop = middleBackgroundRegion.SourceRectangle.Top;
                middleBackground.TextureWidth = middleBackgroundRegion.Width;
                middleBackground.Width = topLevelContainer.Width;
                middleBackground.Height = topLevelContainer.Height;
                middleBackground.WidthUnits = DimensionUnitType.Absolute;
                middleBackground.Dock(Gum.Wireframe.Dock.Bottom);
                // Create the interactive track that responds to clicks
                // The special name "TrackInstance" is required for Slider functionality
                trackInstance = new ContainerRuntime();
                trackInstance.Name = "TrackInstance";
                trackInstance.Dock(Gum.Wireframe.Dock.Fill);
                trackInstance.Height = 0;
                trackInstance.Width = 0;
                innerContainer.AddChild(trackInstance);
                // Create the fill rectangle that visually displays the current value
                _fillRectangle = new ColoredRectangleRuntime();
                _fillRectangle.Color = fillColor;
                _fillRectangle.Dock(Gum.Wireframe.Dock.Bottom);
                _fillRectangle.Width = topLevelContainer.Width;
                _fillRectangle.Height = topLevelContainer.Height - 4; // Default to 90% - will be updated by value changes
                _fillRectangle.WidthUnits = DimensionUnitType.Absolute;
                _fillRectangle.HeightUnits = DimensionUnitType.PercentageOfParent;
            }
            trackInstance.AddChild(_fillRectangle);
            trackInstance.AddChild(middleBackground);
            // Assign the configured container as this slider's visual
            Visual = topLevelContainer;

            // Enable click-to-point functionality for the slider
            // This allows users to click anywhere on the track to jump to that value
            IsMoveToPointEnabled = false;
            SmallChange = 0;
            LargeChange = 0;

            // Add event handlers
            ValueChanged += HandleValueChanged;
        }
        /// <summary>
        /// Updates the fill rectangle width to visually represent the current value
        /// </summary>
        private void HandleValueChanged(object sender, EventArgs e)
        {
            // Calculate the ratio of the current value within its range
            double ratio = (Value - Minimum) / (Maximum - Minimum);

            if (sideways)
            {
                // Update the fill rectangle width as a percentage
                // _fillRectangle uses percentage width units, so we multiply by 100
                _fillRectangle.Width = 100 * (float)ratio;
            }
            else
            {
                // Update the fill rectangle width as a percentage
                // _fillRectangle uses percentage width units, so we multiply by 100
                _fillRectangle.Height = 100 * (float)ratio;
            }
        }
    }

}
