using Game_Library.Graphics;
using Gum.DataTypes;
using Gum.DataTypes.Variables;
using Gum.Graphics.Animation;
using Gum.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Endless_Runner.UI
{
    internal class StaticButton : Button
    {
        NineSliceRuntime nineSliceInstance;
        public StaticButton(TextureAtlas atlas, string idle)
        {
            // Create the top-level container that will hold all visual elements
            // Width is relative to children with extra padding, height is fixed
            ContainerRuntime topLevelContainer = new ContainerRuntime();
            TextureRegion unfocusedTextureRegion = atlas.GetRegion(idle);
            topLevelContainer.Height = unfocusedTextureRegion.Height;
            topLevelContainer.HeightUnits = DimensionUnitType.Absolute;
            topLevelContainer.Width = unfocusedTextureRegion.Height;
            topLevelContainer.WidthUnits = DimensionUnitType.Absolute;

            // Create the nine-slice background that will display the button graphics
            // A nine-slice allows the button to stretch while preserving corner appearance
            nineSliceInstance = new NineSliceRuntime();
            nineSliceInstance.Height = 0f;
            nineSliceInstance.Width = 0f;
            nineSliceInstance.Texture = unfocusedTextureRegion.Texture;
            nineSliceInstance.TextureAddress = TextureAddress.Custom;
            nineSliceInstance.TextureHeight = unfocusedTextureRegion.Height;
            nineSliceInstance.TextureLeft = unfocusedTextureRegion.SourceRectangle.Left;
            nineSliceInstance.TextureTop = unfocusedTextureRegion.SourceRectangle.Top;
            nineSliceInstance.TextureWidth = unfocusedTextureRegion.Width;
            nineSliceInstance.WidthUnits = DimensionUnitType.Absolute;
            nineSliceInstance.HeightUnits = DimensionUnitType.Absolute;
            nineSliceInstance.Dock(Gum.Wireframe.Dock.Fill);
            topLevelContainer.Children.Add(nineSliceInstance);
            // Assign the configured container as this button's visual
            Visual = topLevelContainer;
        }
        public StaticButton(TextureAtlas atlas, string idle, UIText t)
        {
            // Create the top-level container that will hold all visual elements
            // Width is relative to children with extra padding, height is fixed
            ContainerRuntime topLevelContainer = new ContainerRuntime();
            TextureRegion unfocusedTextureRegion = atlas.GetRegion(idle);
            topLevelContainer.Height = unfocusedTextureRegion.Height;
            topLevelContainer.HeightUnits = DimensionUnitType.Absolute;
            topLevelContainer.Width = unfocusedTextureRegion.Height;
            topLevelContainer.WidthUnits = DimensionUnitType.Absolute;

            // Create the nine-slice background that will display the button graphics
            // A nine-slice allows the button to stretch while preserving corner appearance
            nineSliceInstance = new NineSliceRuntime();
            nineSliceInstance.Height = 0f;
            nineSliceInstance.Width = 0f;
            nineSliceInstance.Texture = unfocusedTextureRegion.Texture;
            nineSliceInstance.TextureAddress = TextureAddress.Custom;
            nineSliceInstance.TextureHeight = unfocusedTextureRegion.Height;
            nineSliceInstance.TextureLeft = unfocusedTextureRegion.SourceRectangle.Left;
            nineSliceInstance.TextureTop = unfocusedTextureRegion.SourceRectangle.Top;
            nineSliceInstance.TextureWidth = unfocusedTextureRegion.Width;
            nineSliceInstance.WidthUnits = DimensionUnitType.Absolute;
            nineSliceInstance.HeightUnits = DimensionUnitType.Absolute;
            nineSliceInstance.Dock(Gum.Wireframe.Dock.Fill);
            topLevelContainer.Children.Add(nineSliceInstance);

            // Create the text element that will display the button's label
            TextRuntime textInstance = t;
            // Name is required so it hooks in to the base Button.Text property
            textInstance.Name = "TextInstance";
            textInstance.FontScale = .75f;
            textInstance.Anchor(t.anchor);
            textInstance.X = t.Position.X;
            textInstance.Y = t.Position.Y;
            textInstance.UseCustomFont = true;
            textInstance.CustomFontFile = "Font/Kirsty.fnt";
            topLevelContainer.AddChild(textInstance);
            // Assign the configured container as this button's visual
            Visual = topLevelContainer;
        }
        public StaticButton(TextureAtlas atlas, string idle, string hover)
        {
            // Create the top-level container that will hold all visual elements
            // Width is relative to children with extra padding, height is fixed
            ContainerRuntime topLevelContainer = new ContainerRuntime();
            TextureRegion unfocusedTextureRegion = atlas.GetRegion(idle);
            topLevelContainer.Height = unfocusedTextureRegion.Height;
            topLevelContainer.HeightUnits = DimensionUnitType.Absolute;
            topLevelContainer.Width = unfocusedTextureRegion.Height;
            topLevelContainer.WidthUnits = DimensionUnitType.Absolute;

            // Create the nine-slice background that will display the button graphics
            // A nine-slice allows the button to stretch while preserving corner appearance
            nineSliceInstance = new NineSliceRuntime();
            nineSliceInstance.Height = 0f;
            nineSliceInstance.Width = 0f;
            nineSliceInstance.Texture = unfocusedTextureRegion.Texture;
            nineSliceInstance.TextureAddress = TextureAddress.Custom;
            nineSliceInstance.TextureHeight = unfocusedTextureRegion.Height;
            nineSliceInstance.TextureLeft = unfocusedTextureRegion.SourceRectangle.Left;
            nineSliceInstance.TextureTop = unfocusedTextureRegion.SourceRectangle.Top;
            nineSliceInstance.TextureWidth = unfocusedTextureRegion.Width;
            nineSliceInstance.WidthUnits = DimensionUnitType.Absolute;
            nineSliceInstance.HeightUnits = DimensionUnitType.Absolute;
            nineSliceInstance.Dock(Gum.Wireframe.Dock.Fill);
            topLevelContainer.Children.Add(nineSliceInstance);
            // Create a state category for button states
            StateSaveCategory category = new StateSaveCategory();
            category.Name = Button.ButtonCategoryName;
            topLevelContainer.AddCategory(category);
            StateSave enabledState = new StateSave();
            enabledState.Name = FrameworkElement.EnabledStateName;
            enabledState.Apply = () =>
            {
                switchImage(atlas, idle);
            };
            category.States.Add(enabledState);
            StateSave focusedState = enabledState.Clone();
            focusedState.Name = FrameworkElement.FocusedStateName;
            category.States.Add(focusedState);
            StateSave highlighted = new StateSave();
            highlighted.Name = FrameworkElement.HighlightedFocusedStateName;
            highlighted.Apply = () =>
            {
                switchImage(atlas, hover);
            };
            category.States.Add(highlighted);
            // Assign the configured container as this button's visual
            Visual = topLevelContainer;
        }
        public void switchImage(TextureAtlas atlas, string region)
        {
            TextureRegion unfocusedTextureRegion = atlas.GetRegion(region);
            nineSliceInstance.TextureAddress = TextureAddress.Custom;
            nineSliceInstance.TextureHeight = unfocusedTextureRegion.Height;
            nineSliceInstance.TextureLeft = unfocusedTextureRegion.SourceRectangle.Left;
            nineSliceInstance.TextureTop = unfocusedTextureRegion.SourceRectangle.Top;
            nineSliceInstance.TextureWidth = unfocusedTextureRegion.Width;
        }
    }
}
