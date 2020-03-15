namespace Easycharts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SkiaSharp;

    /// <summary>
    /// 指数图表
    /// </summary>
    public class PointChart : Chart
    {
        #region Properties

        public float PointSize { get; set; } = 14;

        public PointMode PointMode { get; set; } = PointMode.Circle;

        public byte PointAreaAlpha { get; set; } = 100;

        private float ValueRange => this.MaxValue - this.MinValue;

        #endregion

        #region Methods


        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemHeight"></param>
        /// <param name="headerHeight"></param>
        /// <returns></returns>
        public float CalculateYOrigin(float itemHeight, float headerHeight)
        {
            if (this.MaxValue <= 0)
            {
                return headerHeight;
            } 

            if (this.MinValue > 0)
            {
                return headerHeight + itemHeight;
            }

            return headerHeight + ((this.MaxValue / this.ValueRange) * itemHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            var valueLabelSizes = this.MeasureValueLabels();
            var footerHeight = this.CalculateFooterHeight(valueLabelSizes);
            var headerHeight = this.CalculateHeaderHeight(valueLabelSizes);
            var itemSize = this.CalculateItemSize(width, height, footerHeight, headerHeight);
            var origin = this.CalculateYOrigin(itemSize.Height, headerHeight);
            var points = this.CalculatePoints(itemSize, origin, headerHeight);

            this.DrawPointAreas(canvas, points, origin);
            this.DrawPoints(canvas, points);
            this.DrawFooter(canvas, points, itemSize, height, footerHeight);
            this.DrawValueLabel(canvas, points, itemSize, height, valueLabelSizes);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="footerHeight"></param>
        /// <param name="headerHeight"></param>
        /// <returns></returns>
        protected SKSize CalculateItemSize(int width, int height, float footerHeight, float headerHeight)
        {
            var total = this.Entries.Count();
            var w = (width - ((total + 1) * this.Margin)) / total;
            var h = height - this.Margin - footerHeight - headerHeight;
            return new SKSize(w, h);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemSize"></param>
        /// <param name="origin"></param>
        /// <param name="headerHeight"></param>
        /// <returns></returns>
        protected SKPoint[] CalculatePoints(SKSize itemSize, float origin, float headerHeight)
        {
            var result = new List<SKPoint>();

            for (int i = 0; i < this.Entries.Count(); i++)
            {
                var entry = this.Entries.ElementAt(i);

                var x = this.Margin + (itemSize.Width / 2) + (i * (itemSize.Width + this.Margin));
                var y = headerHeight + (((this.MaxValue - entry.Value) / this.ValueRange) * itemSize.Height);
                var point = new SKPoint(x, y);
                result.Add(point);
            }

            return result.ToArray();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="points"></param>
        /// <param name="itemSize"></param>
        /// <param name="height"></param>
        /// <param name="footerHeight"></param>
        protected void DrawFooter(SKCanvas canvas, SKPoint[] points, SKSize itemSize, int height, float footerHeight)
        {
            this.DrawLabels(canvas, points, itemSize, height, footerHeight);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="points"></param>
        /// <param name="itemSize"></param>
        /// <param name="height"></param>
        /// <param name="footerHeight"></param>
        protected void DrawLabels(SKCanvas canvas, SKPoint[] points, SKSize itemSize, int height, float footerHeight)
        {
            for (int i = 0; i < this.Entries.Count(); i++)
            {
                var entry = this.Entries.ElementAt(i);
                var point = points[i];

                if (!string.IsNullOrEmpty(entry.Label))
                {
                    using (var paint = new SKPaint())
                    {
                        paint.TextSize = this.LabelTextSize;
                        paint.IsAntialias = true;
                        paint.Color = entry.TextColor;
                        paint.IsStroke = false;

                        var fontManager = SKFontManager.Default;
                        var emojiTypeface = fontManager.MatchCharacter('雪');
                        paint.Typeface = emojiTypeface;

                        var bounds = new SKRect();
                        var text = entry.Label;
                        paint.MeasureText(text, ref bounds);

                        if (bounds.Width > itemSize.Width)
                        {
                            text = text.Substring(0, Math.Min(3, text.Length));
                            paint.MeasureText(text, ref bounds);
                        }

                        if (bounds.Width > itemSize.Width)
                        {
                            text = text.Substring(0, Math.Min(1, text.Length));
                            paint.MeasureText(text, ref bounds);
                        }

                        canvas.DrawText(text, point.X - (bounds.Width / 2), height - (this.Margin + (this.LabelTextSize / 2)), paint);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="points"></param>
        protected void DrawPoints(SKCanvas canvas, SKPoint[] points)
        {
            if (points.Length > 0 && PointMode != PointMode.None)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var entry = this.Entries.ElementAt(i);
                    var point = points[i];
                    canvas.DrawPoint(point, entry.Color, this.PointSize, this.PointMode);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="points"></param>
        /// <param name="origin"></param>
        protected void DrawPointAreas(SKCanvas canvas, SKPoint[] points, float origin)
        {
            if (points.Length > 0 && this.PointAreaAlpha > 0)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var entry = this.Entries.ElementAt(i);
                    var point = points[i];
                    var y = Math.Min(origin, point.Y);

                    using (var shader = SKShader.CreateLinearGradient(new SKPoint(0, origin), new SKPoint(0, point.Y), new[] { entry.Color.WithAlpha(this.PointAreaAlpha), entry.Color.WithAlpha((byte)(this.PointAreaAlpha / 3)) }, null, SKShaderTileMode.Clamp))
                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = entry.Color.WithAlpha(this.PointAreaAlpha),
                    })
                    {
                        paint.Shader = shader;
                        var height = Math.Max(2, Math.Abs(origin - point.Y));
                        canvas.DrawRect(SKRect.Create(point.X - (this.PointSize / 2), y, this.PointSize, height), paint);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="points"></param>
        /// <param name="itemSize"></param>
        /// <param name="height"></param>
        /// <param name="valueLabelSizes"></param>
        protected void DrawValueLabel(SKCanvas canvas, SKPoint[] points, SKSize itemSize, float height, SKRect[] valueLabelSizes)
        {
            if (points.Length > 0)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var entry = this.Entries.ElementAt(i);
                    var point = points[i];
                    var isAbove = point.Y > (this.Margin + (itemSize.Height / 2));

                    if (!string.IsNullOrEmpty(entry.ValueLabel))
                    {
                        using (new SKAutoCanvasRestore(canvas))
                        {
                            using (var paint = new SKPaint())
                            {
                                paint.TextSize = this.LabelTextSize;
                                paint.FakeBoldText = true;
                                paint.IsAntialias = true;
                                paint.Color = entry.Color;
                                paint.IsStroke = false;

                                var bounds = new SKRect();
                                var text = entry.ValueLabel;
                                paint.MeasureText(text, ref bounds);

                                canvas.RotateDegrees(90);
                                canvas.Translate(this.Margin, -point.X + (bounds.Height / 2));

                                canvas.DrawText(text, 0, 0, paint);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueLabelSizes"></param>
        /// <returns></returns>
        protected float CalculateFooterHeight(SKRect[] valueLabelSizes)
        {
            var result = this.Margin;

            if (this.Entries.Any(e => !string.IsNullOrEmpty(e.Label)))
            {
                result += this.LabelTextSize + this.Margin;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueLabelSizes"></param>
        /// <returns></returns>
        protected float CalculateHeaderHeight(SKRect[] valueLabelSizes)
        {
            var result = this.Margin;

            if (this.Entries.Any())
            {
                var maxValueWidth = valueLabelSizes.Max(x => x.Width);
                if (maxValueWidth > 0)
                {
                    result += maxValueWidth + this.Margin;
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected SKRect[] MeasureValueLabels()
        {
            using (var paint = new SKPaint())
            {
                paint.TextSize = this.LabelTextSize;
                return this.Entries.Select(e =>
                {
                    if (string.IsNullOrEmpty(e.ValueLabel))
                    {
                        return SKRect.Empty;
                    }

                    var bounds = new SKRect();
                    var text = e.ValueLabel;
                    paint.MeasureText(text, ref bounds);
                    return bounds;
                }).ToArray();
            }
        }


        #endregion



        #region 水平

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueLabelSizes"></param>
        /// <returns></returns>
        protected float CalculateLeftWidth(SKRect[] valueLabelSizes)
        {
            var result = this.Margin;//20

            if (this.Entries.Any(e => !string.IsNullOrEmpty(e.Label)))
            {
                result += this.LabelTextSize + this.Margin;
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueLabelSizes"></param>
        /// <returns></returns>
        protected float CalculateRightWidth(SKRect[] valueLabelSizes)
        {
            var result = this.Margin;

            if (this.Entries.Any())
            {
                var maxValueHeight= valueLabelSizes.Max(x => x.Height);
                if (maxValueHeight > 0)
                {
                    result += maxValueHeight + this.Margin;
                }
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemWidth"></param>
        /// <param name="rightWidth"></param>
        /// <returns></returns>
        public float CalculateXOrigin(float itemWidth, float rightWidth)
        {
            if (this.MaxValue <= 0)
            {
                return rightWidth;
            }

            if (this.MinValue > 0)
            {
                return rightWidth + itemWidth;
            }

            return rightWidth + ((this.MaxValue / this.ValueRange) * itemWidth);
        }

        /*
         //Margin:20
        //width:1080 
        //height: 368
        //leftWidth: 56
        //rightWidth: 65
        */

        protected SKSize CalculateLRItemSize(int width, int height, float leftWidth, float rightWidth)
        {
            var total = this.Entries.Count();
            var w = width - this.Margin - leftWidth - rightWidth;
            //var h = (height - ((total + 1) * this.Margin)) / total - (this.Margin / 2);
            var h = (height - ((total + 1) * this.Margin)) / total ;
            return new SKSize(w, h);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemSize"></param>
        /// <param name="origin"></param>
        /// <param name="leftWidth"></param>
        /// <returns></returns>
        protected SKPoint[] CalculateLRPoints(SKSize itemSize, float origin, float leftWidth)
        {
            var result = new List<SKPoint>();
            //939 154 1004 56
            for (int i = 0; i < this.Entries.Count(); i++)
            {
                var entry = this.Entries.ElementAt(i);

                var x = leftWidth + (((this.MaxValue - entry.Value) / this.ValueRange) * itemSize.Width);
                var y = this.Margin + (itemSize.Height / 2) + (i * (itemSize.Height + this.Margin));
                //192 97
                //56 271
                /*
                    var x = this.Margin + (itemSize.Width / 2) + (i * (itemSize.Width + this.Margin));
                    var y = headerHeight + (((this.MaxValue - entry.Value) / this.ValueRange) * itemSize.Height);
                 */

                var point = new SKPoint(x, y);
                result.Add(point);
            }

            return result.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="points"></param>
        /// <param name="itemSize"></param>
        /// <param name="width"></param>
        /// <param name="leftWidth"></param>
        protected void DrawLeft(SKCanvas canvas, SKPoint[] points, SKSize itemSize, int width, float leftWidth)
        {
            this.DrawLRLabels(canvas, points, itemSize, width, leftWidth);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="points"></param>
        /// <param name="itemSize"></param>
        /// <param name="width"></param>
        /// <param name="leftWidth"></param>
        protected void DrawLRLabels(SKCanvas canvas, SKPoint[] points, SKSize itemSize, int width, float leftWidth)
        {
            for (int i = 0; i < this.Entries.Count(); i++)
            {
                var entry = this.Entries.ElementAt(i);
                var point = points[i];

                if (!string.IsNullOrEmpty(entry.Label))
                {
                    using (var paint = new SKPaint())
                    {
                        paint.TextSize = this.LabelTextSize;
                        paint.IsAntialias = true;
                        paint.Color = SKColors.White;
                        paint.IsStroke = false;
                        //Noto SansCJK

                        var fontManager = SKFontManager.Default;
                        var emojiTypeface = fontManager.MatchCharacter('雪');

                        paint.Typeface = emojiTypeface;

                        var bounds = new SKRect();
                        var text = entry.Label;
                        paint.MeasureText(text, ref bounds);

                        if (bounds.Height > itemSize.Height)
                        {
                            text = text.Substring(0, Math.Min(3, text.Length));
                            paint.MeasureText(text, ref bounds);
                        }

                        if (bounds.Height > itemSize.Height)
                        {
                            text = text.Substring(0, Math.Min(1, text.Length));
                            paint.MeasureText(text, ref bounds);
                        }
                        //
                        //canvas.DrawText(text, point.X - (bounds.Width / 2), height - (this.Margin + (this.LabelTextSize / 2)), paint);
                        //width - (this.Margin + (this.LabelTextSize / 2))
                        //SKTextEncoding.
                        //var txt = StringUtilities.GetEncodedText(text, SKEncoding.Utf8);
                        canvas.DrawText(text, 40, point.Y - (bounds.Height / 2) + (this.Margin / 2), paint);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="points"></param>
        protected void DrawLRPoints(SKCanvas canvas, SKPoint[] points)
        {
            if (points.Length > 0 && PointMode != PointMode.None)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var entry = this.Entries.ElementAt(i);
                    var point = points[i];
                    canvas.DrawPoint(point, entry.Color, this.PointSize, this.PointMode);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="points"></param>
        /// <param name="itemSize"></param>
        /// <param name="width"></param>
        /// <param name="leftWidth"></param>
        /// <param name="rightWidth"></param>
        protected void DrawLRValueLabel(SKCanvas canvas, SKPoint[] points, SKSize itemSize, float width, float leftWidth, float rightWidth)
        {
            if (points.Length > 0)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var entry = this.Entries.ElementAt(i);
                    var point = points[i];
                    var isAbove = point.X > (this.Margin + (itemSize.Width / 2));

                    if (!string.IsNullOrEmpty(entry.ValueLabel))
                    {
                        using (new SKAutoCanvasRestore(canvas))
                        {
                            using (var paint = new SKPaint())
                            {
                                paint.TextSize = this.LabelTextSize;
                                paint.FakeBoldText = true;
                                paint.IsAntialias = true;
                                paint.Color = entry.Color;
                                paint.IsStroke = false;

                                var bounds = new SKRect();
                                var text = entry.ValueLabel;
                                paint.MeasureText(text, ref bounds);

                                //canvas.RotateDegrees(45);
                                //canvas.Translate(-point.Y + (bounds.Width / 2), this.Margin);
                                //itemSize.Width
                                //canvas.DrawText(text, width - (2 * this.Margin + (this.LabelTextSize / 2)+ leftWidth + rightWidth), point.Y - (bounds.Height / 2) + (this.Margin / 2), paint);
                                canvas.DrawText(text, (width - leftWidth - rightWidth) + this.Margin, point.Y - (bounds.Height / 2) + (this.Margin / 2), paint);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
