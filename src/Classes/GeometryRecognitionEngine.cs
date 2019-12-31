using codessentials.CGM.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Classes
{
    /// <summary>
    /// Engine to recognize geograpical objects
    /// </summary>
    public class GeometryRecognitionEngine
    {
        /// <summary>
        /// Gets all rectangles of the given file
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static List<CGMRectangle> GetRectangles(CGMFile file)
        {
            var result = new List<CGMRectangle>();

            // sort all polylines by their points
            var polylines = file.Commands.Where(c => c.ElementClass == ClassCode.GraphicalPrimitiveElements && (c.ElementId == 1)).Cast<Polyline>().ToList(); //Polyline

            // now we have a list with such items
            // LINE(125.2167, 136.3638) (125.2167, 145.3638);
            // LINE(125.2167, 136.3638) (150.8740, 136.3638);
            // LINE(150.8741, 145.3638) (125.2167, 145.3638);
            // LINE(150.8740, 136.3638) (150.8740, 145.3638);
            //
            // this example (5 simple lines) above describes a rectangle

            // internaly we see all points in a sorted way like that
            // (81.3296,95.3243)
            // (81.3296,105.3844)
            // (81.3296,105.3844)
            // (101.4332,95.3243)
            // (101.4332,105.3844)
            //
            // this example (5 points) above describes a rectangle (third line can be ommited)

            var simpleLines = polylines.Where(l => l.IsSimpleLine);
            var rectangleCanditates = polylines.Where(l => l.Points.Length == 5);

            result.AddRange(FindRectangleInSimpleLines(simpleLines));
            result.AddRange(FindRectangleInPolygons(rectangleCanditates));   
          

            return result;
        }

        private static IEnumerable<CGMRectangle> FindRectangleInPolygons(IEnumerable<Polyline> rectangleCanditates)
        {
            var result = new List<CGMRectangle>();
            foreach (var line in rectangleCanditates)
            {
                CGMRectangle rectangle = GetRectangle(line);

                if (!rectangle.IsEmpty)
                    result.Add(rectangle);
            }

            return result;
        }

        private static IEnumerable<CGMRectangle> FindRectangleInSimpleLines(IEnumerable<Polyline> simpleLines)
        {
            // get all horizontal lines
            var horizontalLines = simpleLines.Where(l => IsHorizontalLine(l.Points[0], l.Points[1])).Select(l => new CGMLine(l.Points[0], l.Points[1]));
            var verticalLines = simpleLines.Where(l => IsVerticalLine(l.Points[0], l.Points[1])).Select(l => new CGMLine(l.Points[0], l.Points[1]));
            var rects = new List<RectanglePoints>();

            // loop through horizontal lines and find the two parelles each
            foreach (var horzLine in horizontalLines)
            {
                var others = horizontalLines.Where(l => CGMPoint.IsSame(l.A.X, horzLine.A.X) && l.A.Y > horzLine.A.Y);

                if (others.Any())
                {
                    var nearest = others.OrderBy(l => l.A.Y).Last();
                    rects.Add(new RectanglePoints(horzLine, nearest));
                }
            }

            // loop the vertical lines and find the ones linking to the horizontal ones
            foreach (var verticalLine in verticalLines)
            {
                var l = rects.FirstOrDefault(h => h.IsUpperLeft(verticalLine.A));

                if (l != null && l.IsLowerLeft(verticalLine.B))
                {
                    l.SetLowerLeft(verticalLine.B);
                    continue;
                }

                l = rects.FirstOrDefault(h => h.IsUpperRight(verticalLine.A));

                if (l != null && l.IsLowerRight(verticalLine.B))
                {
                    l.SetLowerRight(verticalLine.B);
                    continue;
                }
            }

            return rects.Where(r => r.IsValid).Select(r => r.GetRectangle());
        }

        private static bool IsHorizontalLine(CGMPoint a, CGMPoint b)
        {
            return CGMPoint.IsSame(a.Y, b.Y) && !CGMPoint.IsSame(a.X, b.X);
        }

        private static bool IsVerticalLine(CGMPoint a, CGMPoint b)
        {
            return CGMPoint.IsSame(a.X, b.X) && !CGMPoint.IsSame(a.Y, b.Y);
        }

       
        public static CGMRectangle GetRectangle(Polyline polyline)
        {
            if (polyline.Points.Length == 5)
            {
                var points = polyline.Points;

                // internaly we see all points in a sorted way like that
                // (81.3296,95.3243)
                // (81.3296,105.3844)
                // (81.3296,105.3844)
                // (101.4332,95.3243)
                // (101.4332,105.3844)
                //
                // this example (5 points) above describes a rectangle (third line can be ommited)

                // last should close the path
                if (points[0].Equals(points[4]))
                {
                    // rectangle is descriped counter clock-wise starting right
                    if (CGMPoint.IsSame(points[0].Y, points[1].Y) && CGMPoint.IsSame(points[1].X, points[2].X) && CGMPoint.IsSame(points[2].Y, points[3].Y))
                    {
                        if (points[1].Y < points[2].Y)
                            return CGMRectangle.FromPoints(points[1], points[0], points[2], points[3]);
                        else if (points[0].X < points[1].X) // starting left
                            return CGMRectangle.FromPoints(points[3], points[2], points[0], points[1]);
                        else
                            return CGMRectangle.FromPoints(points[2], points[3], points[1], points[0]);
                    }

                    // rectangle is described clock wise 
                    if (CGMPoint.IsSame(points[0].X, points[1].X) && CGMPoint.IsSame(points[1].Y, points[2].Y) && CGMPoint.IsSame(points[2].X, points[3].X))
                    {
                        return CGMRectangle.FromPoints(points[4], points[0], points[3], points[1]);
                    }
                }
            }

            return CGMRectangle.Empty;
        }

        /// <summary>
        /// Determines whether point A is near point b
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointWithinRange">The point with have to be within the range of the start point.</param>
        /// <param name="rangeDistance">The range distance.</param>
        public static bool IsNearBy(CGMPoint pointA, CGMPoint pointWithinRange, float rangeDistance)
        {
            CGMRectangle rect = new CGMRectangle((float)pointA.X - rangeDistance, (float)pointA.Y - rangeDistance, rangeDistance * 2, rangeDistance * 2);

            return rect.Contains(pointWithinRange);
        }

        private class RectanglePoints
        {
            CGMLine _topLine;
            CGMLine _bottomLine;
            CGMPoint _leftLowerCorner;
            CGMPoint _rightLowerCorner;

            public bool IsValid
            {
                get { return _leftLowerCorner != null && _rightLowerCorner != null && GetIsValid(_topLine.A, _topLine.B, _leftLowerCorner, _rightLowerCorner); }
            }

            public RectanglePoints(CGMLine topLine, CGMLine bottomLine)
            {
                _topLine = topLine;
                _bottomLine = bottomLine;
            }

            public bool IsUpperLeft(CGMPoint p)
            {
                return _topLine.A.CompareTo(p) == 0;
            }

            public bool IsLowerLeft(CGMPoint p)
            {
                return _bottomLine.A.CompareTo(p) == 0;
            }

            public bool IsUpperRight(CGMPoint p)
            {
                return _topLine.B.CompareTo(p) == 0;
            }

            public bool IsLowerRight(CGMPoint p)
            {
                return _bottomLine.B.CompareTo(p) == 0;
            }

            public void SetLowerLeft(CGMPoint p)
            {
                _leftLowerCorner = p;
            }

            public void SetLowerRight(CGMPoint p)
            {
                _rightLowerCorner = p;
            }

            private static bool GetIsValid(CGMPoint leftUpperCorner, CGMPoint rightUpperCorner, CGMPoint leftLowerCorner, CGMPoint rightLowerCorner)
            {
                if (leftUpperCorner.Y != rightUpperCorner.Y)
                    return false;

                if (leftLowerCorner.Y != rightLowerCorner.Y)
                    return false;

                if (leftUpperCorner.X != leftLowerCorner.X)
                    return false;

                if (rightUpperCorner.X != rightLowerCorner.X)
                    return false;

                return true;
            }

            public CGMRectangle GetRectangle()
            {
                return CGMRectangle.FromPoints(_topLine.A, _topLine.B, _leftLowerCorner, _rightLowerCorner);
            }
        }
    }
}
