// <copyright file="Arc.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Memento
{
    /// <summary>
    /// Creates arc on a circle.
    /// </summary>
    public class Arc : Shape
    {
        /// <summary>
        /// Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc), new UIPropertyMetadata(0.0, new PropertyChangedCallback(UpdateArc)));

        /// <summary>
        /// Using a DependencyProperty as the backing store for EndAngle.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc), new UIPropertyMetadata(90.0, new PropertyChangedCallback(UpdateArc)));

        /// <summary>
        /// field that shows whether or not the progress bar goes clockwise or counterclockwise.
        /// </summary>
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(SweepDirection), typeof(Arc), new UIPropertyMetadata(SweepDirection.Clockwise));

        /// <summary>
        /// rotate the start/endpoint of the arc a certain number of degree in the direction
        /// ie. if you wanted it to be at 12:00 that would be 270 Clockwise or 90 counterclockwise.
        /// </summary>
        public static readonly DependencyProperty OriginRotationDegreesProperty =
            DependencyProperty.Register("OriginRotationDegrees", typeof(double), typeof(Arc), new UIPropertyMetadata(270.0, new PropertyChangedCallback(UpdateArc)));

        /// <summary>
        /// Gets or sets start angle in a circle progress bar.
        /// </summary>
        public double StartAngle
        {
            get { return (double)this.GetValue(StartAngleProperty); }
            set { this.SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets end angle in a circle progress bar.
        /// </summary>
        public double EndAngle
        {
            get { return (double)this.GetValue(EndAngleProperty); }
            set { this.SetValue(EndAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets this controls whether or not the progress bar goes clockwise or counterclockwise.
        /// </summary>
        public SweepDirection Direction
        {
            get { return (SweepDirection)this.GetValue(DirectionProperty); }
            set { this.SetValue(DirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets rotate the start/endpoint of the arc a certain number of degree in the direction
        /// ie. if you wanted it to be at 12:00 that would be 270 Clockwise or 90 counterclockwise.
        /// </summary>
        public double OriginRotationDegrees
        {
            get { return (double)this.GetValue(OriginRotationDegreesProperty); }
            set { this.SetValue(OriginRotationDegreesProperty, value); }
        }

        /// <summary>
        /// Gets arc geometry.
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get { return this.GetArcGeometry(); }
        }

        /// <summary>
        /// Updates the value of arc.
        /// </summary>
        /// <param name="d">defines a new value for arc.</param>
        /// <param name="e">defines event to run.</param>
        protected static void UpdateArc(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Arc arc = d as Arc;
            arc.InvalidateVisual();
        }

        /// <summary>
        /// Draws arc.
        /// </summary>
        /// <param name="drawingContext">Arc to draw.</param>
        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            drawingContext.DrawGeometry(null, new Pen(this.Stroke, this.StrokeThickness), this.GetArcGeometry());
        }

        private Geometry GetArcGeometry()
        {
            Point startPoint = this.PointAtAngle(Math.Min(this.StartAngle, this.EndAngle), this.Direction);
            Point endPoint = this.PointAtAngle(Math.Max(this.StartAngle, this.EndAngle), this.Direction);

            Size arcSize = new Size(
                Math.Max(0, (this.RenderSize.Width - this.StrokeThickness) / 2),
                Math.Max(0, (this.RenderSize.Height - this.StrokeThickness) / 2));
            bool isLargeArc = Math.Abs(this.EndAngle - this.StartAngle) > 180;

            StreamGeometry geom = new StreamGeometry();
            using (StreamGeometryContext context = geom.Open())
            {
                context.BeginFigure(startPoint, false, false);
                context.ArcTo(endPoint, arcSize, 0, isLargeArc, this.Direction, true, false);
            }

            geom.Transform = new TranslateTransform(this.StrokeThickness / 2, this.StrokeThickness / 2);
            return geom;
        }

        private Point PointAtAngle(double angle, SweepDirection sweep)
        {
            double translatedAngle = angle + this.OriginRotationDegrees;
            double radAngle = translatedAngle * (Math.PI / 180);
            double xr = (this.RenderSize.Width - this.StrokeThickness) / 2;
            double yr = (this.RenderSize.Height - this.StrokeThickness) / 2;

            double x = xr + (xr * Math.Cos(radAngle));
            double y = yr * Math.Sin(radAngle);

            if (sweep == SweepDirection.Counterclockwise)
            {
                y = yr - y;
            }
            else
            {
                y = yr + y;
            }

            return new Point(x, y);
        }
    }
}
