﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        private readonly List<Rectangle> placedRectangles;
        private readonly Spiral spiral;

        public CircularCloudLayouter(Point center)
        {
            spiral = new Spiral(center);
            placedRectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.IsEmpty)
                throw new ArgumentException();
            foreach (var point in spiral)
            {
                var topLeftCorner = new Point();
                topLeftCorner.X = point.X - rectangleSize.Width / 2;
                topLeftCorner.Y = point.Y - rectangleSize.Height / 2;
                var rectangle = new Rectangle(topLeftCorner, rectangleSize);
                if (IsRectangleIntersects(rectangle)) continue;
                placedRectangles.Add(rectangle);
                AddUsedPointsToSpiral(rectangle);
                return rectangle;
            }

            return new Rectangle();
        }

        private bool IsRectangleIntersects(Rectangle rectangle)
        {
            foreach (var placedRectangle in placedRectangles)
                if (rectangle.IntersectsWith(placedRectangle))
                    return true;

            return false;
        }

        private void AddUsedPointsToSpiral(Rectangle rectangle)
        {
            var usedPoints = new List<Point>();
            for (var i = 0; i < rectangle.Width; i++)
            for (var j = 0; j < rectangle.Height; j++)
                usedPoints.Add(new Point(rectangle.Left + i, rectangle.Top + j));
            spiral.AddUsedPoints(usedPoints);
        }
    }
}