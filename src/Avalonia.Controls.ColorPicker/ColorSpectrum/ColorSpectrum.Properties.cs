// This source file is adapted from the WinUI project.
// (https://github.com/microsoft/microsoft-ui-xaml)
//
// Licensed to The Avalonia Project under the MIT License.

using Avalonia.Media;

namespace Avalonia.Controls.Primitives
{
    /// <inheritdoc/>
    public partial class ColorSpectrum
    {
        /// <summary>
        /// Gets or sets the currently selected color in the RGB color model.
        /// </summary>
        /// <remarks>
        /// For control authors use <see cref="HsvColor"/> instead to avoid loss
        /// of precision and color drifting.
        /// </remarks>
        public Color Color
        {
            get => GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        /// <summary>
        /// Defines the <see cref="Color"/> property.
        /// </summary>
        public static readonly StyledProperty<Color> ColorProperty =
            AvaloniaProperty.Register<ColorSpectrum, Color>(
                nameof(Color),
                Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));

        /// <summary>
        /// Gets or sets the two HSV color components displayed by the spectrum.
        /// </summary>
        /// <remarks>
        /// Internally, the <see cref="ColorSpectrum"/> uses the HSV color model.
        /// </remarks>
        public ColorSpectrumComponents Components
        {
            get => GetValue(ComponentsProperty);
            set => SetValue(ComponentsProperty, value);
        }

        /// <summary>
        /// Defines the <see cref="Components"/> property.
        /// </summary>
        public static readonly StyledProperty<ColorSpectrumComponents> ComponentsProperty =
            AvaloniaProperty.Register<ColorSpectrum, ColorSpectrumComponents>(
                nameof(Components),
                ColorSpectrumComponents.HueSaturation);

        /// <summary>
        /// Gets or sets the currently selected color in the HSV color model.
        /// </summary>
        /// <remarks>
        /// This should be used in all cases instead of the <see cref="Color"/> property.
        /// Internally, the <see cref="ColorSpectrum"/> uses the HSV color model and using
        /// this property will avoid loss of precision and color drifting.
        /// </remarks>
        public HsvColor HsvColor
        {
            get => GetValue(HsvColorProperty);
            set => SetValue(HsvColorProperty, value);
        }

        /// <summary>
        /// Defines the <see cref="HsvColor"/> property.
        /// </summary>
        public static readonly StyledProperty<HsvColor> HsvColorProperty =
            AvaloniaProperty.Register<ColorSpectrum, HsvColor>(
                nameof(HsvColor),
                new HsvColor(1, 0, 0, 1));

        /// <summary>
        /// Gets or sets the maximum value of the Hue component in the range from 0..359.
        /// This property must be greater than <see cref="MinHue"/>.
        /// </summary>
        /// <remarks>
        /// Internally, the <see cref="ColorSpectrum"/> uses the HSV color model.
        /// </remarks>
        public int MaxHue
        {
            get => GetValue(MaxHueProperty);
            set => SetValue(MaxHueProperty, value);
        }

        /// <summary>
        /// Defines the <see cref="MaxHue"/> property.
        /// </summary>
        public static readonly StyledProperty<int> MaxHueProperty =
            AvaloniaProperty.Register<ColorSpectrum, int>(nameof(MaxHue), 359);

        /// <summary>
        /// Gets or sets the maximum value of the Saturation component in the range from 0..100.
        /// This property must be greater than <see cref="MinSaturation"/>.
        /// </summary>
        /// <remarks>
        /// Internally, the <see cref="ColorSpectrum"/> uses the HSV color model.
        /// </remarks>
        public int MaxSaturation
        {
            get => GetValue(MaxSaturationProperty);
            set => SetValue(MaxSaturationProperty, value);
        }

        /// <summary>
        /// Defines the <see cref="MaxSaturation"/> property.
        /// </summary>
        public static readonly StyledProperty<int> MaxSaturationProperty =
            AvaloniaProperty.Register<ColorSpectrum, int>(nameof(MaxSaturation), 100);

        /// <summary>
        /// Gets or sets the maximum value of the Value component in the range from 0..100.
        /// This property must be greater than <see cref="MinValue"/>.
        /// </summary>
        /// <remarks>
        /// Internally, the <see cref="ColorSpectrum"/> uses the HSV color model.
        /// </remarks>
        public int MaxValue
        {
            get => GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        /// <summary>
        /// Defines the <see cref="MaxValue"/> property.
        /// </summary>
        public static readonly StyledProperty<int> MaxValueProperty =
            AvaloniaProperty.Register<ColorSpectrum, int>(nameof(MaxValue), 100);

        /// <summary>
        /// Gets or sets the minimum value of the Hue component in the range from 0..359.
        /// This property must be less than <see cref="MaxHue"/>.
        /// </summary>
        /// <remarks>
        /// Internally, the <see cref="ColorSpectrum"/> uses the HSV color model.
        /// </remarks>
        public int MinHue
        {
            get => GetValue(MinHueProperty);
            set => SetValue(MinHueProperty, value);
        }

        /// <summary>
        /// Defines the <see cref="MinHue"/> property.
        /// </summary>
        public static readonly StyledProperty<int> MinHueProperty =
            AvaloniaProperty.Register<ColorSpectrum, int>(nameof(MinHue), 0);

        /// <summary>
        /// Gets or sets the minimum value of the Saturation component in the range from 0..100.
        /// This property must be less than <see cref="MaxSaturation"/>.
        /// </summary>
        /// <remarks>
        /// Internally, the <see cref="ColorSpectrum"/> uses the HSV color model.
        /// </remarks>
        public int MinSaturation
        {
            get => GetValue(MinSaturationProperty);
            set => SetValue(MinSaturationProperty, value);
        }

        /// <summary>
        /// Defines the <see cref="MinSaturation"/> property.
        /// </summary>
        public static readonly StyledProperty<int> MinSaturationProperty =
            AvaloniaProperty.Register<ColorSpectrum, int>(nameof(MinSaturation), 0);

        /// <summary>
        /// Gets or sets the minimum value of the Value component in the range from 0..100.
        /// This property must be less than <see cref="MaxValue"/>.
        /// </summary>
        /// <remarks>
        /// Internally, the <see cref="ColorSpectrum"/> uses the HSV color model.
        /// </remarks>
        public int MinValue
        {
            get => GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        /// <summary>
        /// Defines the <see cref="MinValue"/> property.
        /// </summary>
        public static readonly StyledProperty<int> MinValueProperty =
            AvaloniaProperty.Register<ColorSpectrum, int>(nameof(MinValue), 0);

        /// <summary>
        /// Gets or sets the displayed shape of the spectrum.
        /// </summary>
        public ColorSpectrumShape Shape
        {
            get => GetValue(ShapeProperty);
            set => SetValue(ShapeProperty, value);
        }

        /// <summary>
        /// Defines the <see cref="Shape"/> property.
        /// </summary>
        public static readonly StyledProperty<ColorSpectrumShape> ShapeProperty =
            AvaloniaProperty.Register<ColorSpectrum, ColorSpectrumShape>(
                nameof(Shape),
                ColorSpectrumShape.Box);
    }
}
