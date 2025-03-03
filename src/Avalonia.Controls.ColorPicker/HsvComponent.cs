﻿// This source file is adapted from the WinUI project.
// (https://github.com/microsoft/microsoft-ui-xaml)
//
// Licensed to The Avalonia Project under the MIT License.

using Avalonia.Media;

namespace Avalonia.Controls
{
    /// <summary>
    /// Defines a specific component in the HSV color model.
    /// </summary>
    public enum HsvComponent
    {
        /// <summary>
        /// The Hue component.
        /// </summary>
        /// <remarks>
        /// Also see: <see cref="HsvColor.H"/>
        /// </remarks>
        Hue,

        /// <summary>
        /// The Saturation component.
        /// </summary>
        /// <remarks>
        /// Also see: <see cref="HsvColor.S"/>
        /// </remarks>
        Saturation,

        /// <summary>
        /// The Value component.
        /// </summary>
        /// <remarks>
        /// Also see: <see cref="HsvColor.V"/>
        /// </remarks>
        Value,

        /// <summary>
        /// The Alpha component.
        /// </summary>
        /// <remarks>
        /// Also see: <see cref="HsvColor.A"/>
        /// </remarks>
        Alpha
    };
}
