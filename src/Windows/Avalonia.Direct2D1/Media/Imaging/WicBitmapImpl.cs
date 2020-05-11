using System;
using System.IO;
using System.Security.Cryptography;
using Avalonia.Win32.Interop;
using SharpDX.WIC;
using APixelFormat = Avalonia.Platform.PixelFormat;
using D2DBitmap = SharpDX.Direct2D1.Bitmap;

namespace Avalonia.Direct2D1.Media
{
    /// <summary>
    /// A WIC implementation of a <see cref="Avalonia.Media.Imaging.Bitmap"/>.
    /// </summary>
    public class WicBitmapImpl : BitmapImpl
    {
        private BitmapDecoder _decoder;

        private static BitmapInterpolationMode ConvertInterpolationMode(Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode interpolationMode)
        {
            switch (interpolationMode)
            {
                case Visuals.Media.Imaging.BitmapInterpolationMode.Default:
                    return BitmapInterpolationMode.Fant;

                case Visuals.Media.Imaging.BitmapInterpolationMode.LowQuality:
                    return BitmapInterpolationMode.NearestNeighbor;

                case Visuals.Media.Imaging.BitmapInterpolationMode.MediumQuality:
                    return BitmapInterpolationMode.Fant;

                default:
                case Visuals.Media.Imaging.BitmapInterpolationMode.HighQuality:
                    return BitmapInterpolationMode.HighQualityCubic;

            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WicBitmapImpl"/> class.
        /// </summary>
        /// <param name="fileName">The filename of the bitmap to load.</param>
        public WicBitmapImpl(string fileName)
        {
            using (BitmapDecoder decoder = new BitmapDecoder(Direct2D1Platform.ImagingFactory, fileName, DecodeOptions.CacheOnDemand))
            {
                WicImpl = new Bitmap(Direct2D1Platform.ImagingFactory, decoder.GetFrame(0), BitmapCreateCacheOption.CacheOnDemand);
                Dpi = new Vector(96, 96);
            }
        }

        private WicBitmapImpl(Bitmap bmp)
        {
            WicImpl = bmp;
            Dpi = new Vector(96, 96);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WicBitmapImpl"/> class.
        /// </summary>
        /// <param name="stream">The stream to read the bitmap from.</param>
        public WicBitmapImpl(Stream stream)
        {
            // https://stackoverflow.com/questions/48982749/decoding-image-from-stream-using-wic/48982889#48982889
            _decoder = new BitmapDecoder(Direct2D1Platform.ImagingFactory, stream, DecodeOptions.CacheOnLoad);

            WicImpl = new Bitmap(Direct2D1Platform.ImagingFactory, _decoder.GetFrame(0), BitmapCreateCacheOption.CacheOnLoad);
            Dpi = new Vector(96, 96);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WicBitmapImpl"/> class.
        /// </summary>
        /// <param name="size">The size of the bitmap in device pixels.</param>
        /// <param name="dpi">The DPI of the bitmap.</param>
        /// <param name="pixelFormat">Pixel format</param>
        public WicBitmapImpl(PixelSize size, Vector dpi, APixelFormat? pixelFormat = null)
        {
            if (!pixelFormat.HasValue)
            {
                pixelFormat = APixelFormat.Bgra8888;
            }

            PixelFormat = pixelFormat;
            WicImpl = new Bitmap(
                Direct2D1Platform.ImagingFactory,
                size.Width,
                size.Height,
                pixelFormat.Value.ToWic(),
                BitmapCreateCacheOption.CacheOnLoad);

            Dpi = dpi;
        }

        public WicBitmapImpl(APixelFormat format, IntPtr data, PixelSize size, Vector dpi, int stride)
        {
            WicImpl = new Bitmap(Direct2D1Platform.ImagingFactory, size.Width, size.Height, format.ToWic(), BitmapCreateCacheOption.CacheOnDemand);
            WicImpl.SetResolution(dpi.X, dpi.Y);

            PixelFormat = format;
            Dpi = dpi;

            using (var l = WicImpl.Lock(BitmapLockFlags.Write))
            {
                for (var row = 0; row < size.Height; row++)
                {
                    UnmanagedMethods.CopyMemory(
                        (l.Data.DataPointer + row * l.Stride),
                        (data + row * stride),
                        (UIntPtr)l.Data.Pitch);
                }
            }
        }

        public WicBitmapImpl(Stream stream, int decodeSize, bool horizontal, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode interpolationMode)
        {
            _decoder = new BitmapDecoder(Direct2D1Platform.ImagingFactory, stream, DecodeOptions.CacheOnLoad);

            var bmp = new Bitmap(Direct2D1Platform.ImagingFactory, _decoder.GetFrame(0), BitmapCreateCacheOption.CacheOnLoad);

            // now scale that to the size that we want
            var realScale = horizontal ? ((double)bmp.Size.Height / bmp.Size.Width) : ((double)bmp.Size.Width / bmp.Size.Height);

            PixelSize desired;

            if (horizontal)
            {
                desired = new PixelSize(decodeSize, (int)(realScale * decodeSize));
            }
            else
            {
                desired = new PixelSize((int)(realScale * decodeSize), decodeSize);
            }

            if (bmp.Size.Width != desired.Width || bmp.Size.Height != desired.Height)
            {
                using (var scaler = new BitmapScaler(Direct2D1Platform.ImagingFactory))
                {
                    scaler.Initialize(bmp, desired.Width, desired.Height, ConvertInterpolationMode(interpolationMode));

                    var image = new Bitmap(Direct2D1Platform.ImagingFactory, scaler, BitmapCreateCacheOption.CacheOnDemand);
                    bmp.Dispose();
                    bmp = image;
                }
            }

            WicImpl = bmp;

            Dpi = new Vector(96, 96);
        }

        public override Vector Dpi { get; }

        public override PixelSize PixelSize => WicImpl.Size.ToAvalonia();

        protected APixelFormat? PixelFormat { get; }

        public override void Dispose()
        {
            WicImpl.Dispose();
            _decoder?.Dispose();
        }

        /// <summary>
        /// Gets the WIC implementation of the bitmap.
        /// </summary>
        public Bitmap WicImpl { get; }

        /// <summary>
        /// Gets a Direct2D bitmap to use on the specified render target.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        /// <returns>The Direct2D bitmap.</returns>
        public override OptionalDispose<D2DBitmap> GetDirect2DBitmap(SharpDX.Direct2D1.RenderTarget renderTarget)
        {
            FormatConverter converter = new FormatConverter(Direct2D1Platform.ImagingFactory);
            converter.Initialize(WicImpl, SharpDX.WIC.PixelFormat.Format32bppPBGRA);
            return new OptionalDispose<D2DBitmap>(D2DBitmap.FromWicBitmap(renderTarget, converter), true);
        }

        public unsafe WicBitmapImpl CreateScaledBitmap(PixelSize size, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode interpolationMode)
        {
            using (var scaler = new BitmapScaler(Direct2D1Platform.ImagingFactory))
            {
                scaler.Initialize(WicImpl, size.Width, size.Height, ConvertInterpolationMode(interpolationMode));

                return new WicBitmapImpl(new Bitmap(Direct2D1Platform.ImagingFactory, scaler, BitmapCreateCacheOption.CacheOnDemand));
            }
        }

        public override void Save(Stream stream)
        {
            using (var encoder = new PngBitmapEncoder(Direct2D1Platform.ImagingFactory, stream))
            using (var frame = new BitmapFrameEncode(encoder))
            {
                frame.Initialize();
                frame.WriteSource(WicImpl);
                frame.Commit();
                encoder.Commit();
            }
        }
    }
}
