using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace LittleEreseSystemV3.Helpers
{
    /// <summary>
    /// The only place that loads the PNG files from the Assets\Images folder next to the exe.
    ///
    /// The Designer can never load these files (they are not in the .resx), so every screen must
    /// call this helper from its Load event - never from a constructor.
    /// No method here ever throws: a missing or broken file simply returns null, and every caller
    /// must be ready for a null image.
    /// </summary>
    internal static class IconHelper
    {
        private const string ImagesFolder = "Assets";
        private const string ImagesSubFolder = "Images";

        // Key = name + "|" + size + "|" + color so the same request is only built once.
        private static readonly Dictionary<string, Image> _cache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);
        private static readonly object _cacheLock = new object();

        /// <summary>Loads the PNG at its original size. Returns null when the file is missing.</summary>
        public static Image Load(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;

            string key = name + "|original";
            lock (_cacheLock)
            {
                Image cached;
                if (_cache.TryGetValue(key, out cached)) return cached;

                Image original = LoadFromDisk(name);
                if (original != null) _cache[key] = original;
                return original;
            }
        }

        /// <summary>
        /// High quality copy that fits inside a size x size square and keeps the aspect ratio
        /// (a wide icon stays wide, it is not stretched). Returns null when the file is missing.
        /// </summary>
        public static Image Load(string name, int size)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (size <= 0) return Load(name);

            string key = name + "|" + size.ToString();
            lock (_cacheLock)
            {
                Image cached;
                if (_cache.TryGetValue(key, out cached)) return cached;

                Image original = Load(name);
                Image scaled = original == null ? null : Scale(original, size);
                if (scaled != null) _cache[key] = scaled;
                return scaled;
            }
        }

        /// <summary>
        /// Same as Load(name, size) but every pixel is repainted in "color" while the
        /// transparency is kept. Use it to paint one dark icon white on the dark sidebar.
        /// </summary>
        public static Image Tinted(string name, Color color, int size)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (size <= 0) size = 24;

            string key = name + "|" + size.ToString() + "|" + color.ToArgb().ToString();
            lock (_cacheLock)
            {
                Image cached;
                if (_cache.TryGetValue(key, out cached)) return cached;

                Image original = Load(name);
                Image tinted = original == null ? null : Tint(Scale(original, size), size, color);
                if (tinted != null) _cache[key] = tinted;
                return tinted;
            }
        }

        /// <summary>
        /// The untouched image, used for photos such as the login background and the logos.
        /// Returns null when the file is missing.
        /// </summary>
        public static Image Photo(string name)
        {
            return Load(name);
        }

        // ------------------------------------------------------------------
        // Private helpers
        // ------------------------------------------------------------------

        /// <summary>Reads the file into memory first, so the PNG on disk is never locked.</summary>
        private static Image LoadFromDisk(string name)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImagesFolder, ImagesSubFolder, name + ".png");
                if (!File.Exists(path)) return null;

                byte[] bytes = File.ReadAllBytes(path);
                if (bytes.Length == 0) return null;

                // The MemoryStream stays alive as long as the Image needs it (the Image holds a reference).
                return Image.FromStream(new MemoryStream(bytes));
            }
            catch
            {
                // A corrupt or locked PNG must not stop the app from starting.
                return null;
            }
        }

        /// <summary>Scaled, centered copy inside a size x size transparent square.</summary>
        private static Image Scale(Image source, int size)
        {
            try
            {
                int width = source.Width;
                int height = source.Height;
                if (width <= 0 || height <= 0) return null;

                // Fit inside the square without distorting the picture.
                double ratio = Math.Min((double)size / width, (double)size / height);
                int targetWidth = Math.Max(1, (int)Math.Round(width * ratio));
                int targetHeight = Math.Max(1, (int)Math.Round(height * ratio));
                int offsetX = (size - targetWidth) / 2;
                int offsetY = (size - targetHeight) / 2;

                Bitmap result = new Bitmap(size, size, PixelFormat.Format32bppArgb);
                using (Graphics graphics = Graphics.FromImage(result))
                {
                    graphics.Clear(Color.Transparent);
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.DrawImage(source, new Rectangle(offsetX, offsetY, targetWidth, targetHeight));
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Repaints every visible pixel in "color" using a ColorMatrix (alpha is preserved).</summary>
        private static Image Tint(Image source, int size, Color color)
        {
            if (source == null) return null;

            try
            {
                Bitmap result = new Bitmap(size, size, PixelFormat.Format32bppArgb);
                using (Graphics graphics = Graphics.FromImage(result))
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    graphics.Clear(Color.Transparent);
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;

                    // Zero out the original RGB and add the requested color multiplied by the alpha,
                    // so anti-aliased edges stay soft instead of turning into a hard outline.
                    float r = color.R / 255f;
                    float g = color.G / 255f;
                    float b = color.B / 255f;
                    ColorMatrix matrix = new ColorMatrix(new float[][]
                    {
                        new float[] { 0f, 0f, 0f, 0f, 0f },
                        new float[] { 0f, 0f, 0f, 0f, 0f },
                        new float[] { 0f, 0f, 0f, 0f, 0f },
                        new float[] { 0f, 0f, 0f, 1f, 0f },
                        new float[] { r,  g,  b,  0f, 1f }
                    });
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    graphics.DrawImage(source, new Rectangle(0, 0, size, size), 0, 0, size, size, GraphicsUnit.Pixel, attributes);
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Unicode codes for the Segoe MDL2 Assets font. They are painted with Theme.IconFont,
        /// so buttons do not need any PNG file at all.
        /// </summary>
        public static class Glyph
        {
            public const string Add = "\uE710";
            public const string Edit = "\uE70F";
            public const string Delete = "\uE74D";
            public const string Save = "\uE74E";
            public const string Search = "\uE721";
            public const string Print = "\uE749";
            public const string Refresh = "\uE72C";
            public const string Cancel = "\uE711";
            public const string Back = "\uE72B";
            public const string Forward = "\uE72A";
            public const string ChevronLeft = "\uE76B";
            public const string ChevronRight = "\uE76C";
            public const string View = "\uE7B3";
            public const string Hide = "\uED1A";
            public const string Download = "\uE896";
            public const string Upload = "\uE898";
            public const string Lock = "\uE72E";
            public const string Contact = "\uE77B";
            public const string Accept = "\uE8FB";
            public const string Filter = "\uE71C";
            public const string Calendar = "\uE787";
        }
    }
}
