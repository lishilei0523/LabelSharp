using LabelSharp.Presentation.Models;
using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Visual2Ds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LabelSharp.Presentation.Maps
{
    /// <summary>
    /// 形状映射
    /// </summary>
    public static class ShapeMap
    {
        #region # 获取形状类型 —— static string GetShapeType(this ShapeL shape)
        /// <summary>
        /// 获取形状类型
        /// </summary>
        public static string GetShapeType(this ShapeL shapeL)
        {
            if (shapeL is PointL)
            {
                return Constants.MePoint;
            }
            if (shapeL is LineL)
            {
                return Constants.MeLine;
            }
            if (shapeL is RectangleL)
            {
                return Constants.MeRectangle;
            }
            if (shapeL is CircleL)
            {
                return Constants.MeCircle;
            }
            if (shapeL is EllipseL)
            {
                return Constants.MeEllipse;
            }
            if (shapeL is PolygonL)
            {
                return Constants.MePolygon;
            }
            if (shapeL is PolylineL)
            {
                return Constants.MePolyline;
            }

            return string.Empty;
        }
        #endregion

        #region # 映射LabelMe点集 —— static IList<double[]> ToMePoints(this ShapeL shapeL)
        /// <summary>
        /// 映射LabelMe点集
        /// </summary>
        public static IList<double[]> ToMePoints(this ShapeL shapeL)
        {
            if (shapeL is PointL pointL)
            {
                return pointL.ToMePoints();
            }
            if (shapeL is LineL lineL)
            {
                return lineL.ToMePoints();
            }
            if (shapeL is RectangleL rectangleL)
            {
                return rectangleL.ToMePoints();
            }
            if (shapeL is CircleL circleL)
            {
                return circleL.ToMePoints();
            }
            if (shapeL is EllipseL ellipseL)
            {
                return ellipseL.ToMePoints();
            }
            if (shapeL is PolygonL polygonL)
            {
                return polygonL.ToMePoints();
            }
            if (shapeL is PolylineL polylineL)
            {
                return polylineL.ToMePoints();
            }

            return new List<double[]>();
        }
        #endregion

        #region # 映射LabelMe点集 —— static IList<double[]> ToMePoints(this PointL pointL)
        /// <summary>
        /// 映射LabelMe点集
        /// </summary>
        public static IList<double[]> ToMePoints(this PointL pointL)
        {
            IList<double[]> mePoints = new List<double[]>();
            mePoints.Add(new[] { (double)pointL.X, (double)pointL.Y });

            return mePoints;
        }
        #endregion

        #region # 映射LabelMe点集 —— static IList<double[]> ToMePoints(this LineL lineL)
        /// <summary>
        /// 映射LabelMe点集
        /// </summary>
        public static IList<double[]> ToMePoints(this LineL lineL)
        {
            IList<double[]> mePoints = new List<double[]>();
            mePoints.Add(new[] { (double)lineL.A.X, (double)lineL.A.Y });
            mePoints.Add(new[] { (double)lineL.B.X, (double)lineL.B.Y });

            return mePoints;
        }
        #endregion

        #region # 映射LabelMe点集 —— static IList<double[]> ToMePoints(this RectangleL rectangleL)
        /// <summary>
        /// 映射LabelMe点集
        /// </summary>
        public static IList<double[]> ToMePoints(this RectangleL rectangleL)
        {
            IList<double[]> mePoints = new List<double[]>();
            mePoints.Add(new[] { (double)rectangleL.TopLeft.X, (double)rectangleL.TopLeft.Y });
            mePoints.Add(new[] { (double)rectangleL.TopRight.X, (double)rectangleL.TopRight.Y });
            mePoints.Add(new[] { (double)rectangleL.BottomRight.X, (double)rectangleL.BottomRight.Y });
            mePoints.Add(new[] { (double)rectangleL.BottomLeft.X, (double)rectangleL.BottomLeft.Y });

            return mePoints;
        }
        #endregion

        #region # 映射LabelMe点集 —— static IList<double[]> ToMePoints(this CircleL circleL)
        /// <summary>
        /// 映射LabelMe点集
        /// </summary>
        public static IList<double[]> ToMePoints(this CircleL circleL)
        {
            IList<double[]> mePoints = new List<double[]>();
            mePoints.Add(new[] { (double)circleL.X, (double)circleL.Y });
            mePoints.Add(new[] { (double)(circleL.X + (double)circleL.Radius), (double)circleL.Y });

            return mePoints;
        }
        #endregion

        #region # 映射LabelMe点集 —— static IList<double[]> ToMePoints(this EllipseL ellipseL)
        /// <summary>
        /// 映射LabelMe点集
        /// </summary>
        public static IList<double[]> ToMePoints(this EllipseL ellipseL)
        {
            IList<double[]> mePoints = new List<double[]>();
            mePoints.Add(new[] { (double)ellipseL.X, (double)ellipseL.Y });
            mePoints.Add(new[] { (double)ellipseL.X + (double)ellipseL.RadiusX, (double)ellipseL.Y });
            mePoints.Add(new[] { (double)ellipseL.X, (double)ellipseL.Y + (double)ellipseL.RadiusY });

            return mePoints;
        }
        #endregion

        #region # 映射LabelMe点集 —— static IList<double[]> ToMePoints(this PolygonL polygonL)
        /// <summary>
        /// 映射LabelMe点集
        /// </summary>
        public static IList<double[]> ToMePoints(this PolygonL polygonL)
        {
            IList<double[]> mePoints = new List<double[]>();
            foreach (PointL pointL in polygonL.Points)
            {
                mePoints.Add(new[] { (double)pointL.X, (double)pointL.Y });
            }

            return mePoints;
        }
        #endregion

        #region # 映射LabelMe点集 —— static IList<double[]> ToMePoints(this PolylineL polylineL)
        /// <summary>
        /// 映射LabelMe点集
        /// </summary>
        public static IList<double[]> ToMePoints(this PolylineL polylineL)
        {
            IList<double[]> mePoints = new List<double[]>();
            foreach (PointL pointL in polylineL.Points)
            {
                mePoints.Add(new[] { (double)pointL.X, (double)pointL.Y });
            }

            return mePoints;
        }
        #endregion

        #region # LabelMe点集映射点 —— static PointL ToPointL(this IList<double[]> mePoints)
        /// <summary>
        /// LabelMe点集映射点
        /// </summary>
        public static PointL ToPointL(this IList<double[]> mePoints)
        {
            int x = (int)Math.Ceiling(mePoints[0][0]);
            int y = (int)Math.Ceiling(mePoints[0][1]);
            PointL pointL = new PointL(x, y);

            return pointL;
        }
        #endregion

        #region # LabelMe点集映射线 —— static LineL ToLineL(this IList<double[]> mePoints)
        /// <summary>
        /// LabelMe点集映射线
        /// </summary>
        public static LineL ToLineL(this IList<double[]> mePoints)
        {
            int x1 = (int)Math.Ceiling(mePoints[0][0]);
            int y1 = (int)Math.Ceiling(mePoints[0][1]);
            int x2 = (int)Math.Ceiling(mePoints[1][0]);
            int y2 = (int)Math.Ceiling(mePoints[1][1]);
            PointL pointA = new PointL(x1, y1);
            PointL pointB = new PointL(x2, y2);
            LineL lineL = new LineL(pointA, pointB);

            return lineL;
        }
        #endregion

        #region # LabelMe点集映射矩形 —— static RectangleL ToRectangleL(this IList<double[]> mePoints)
        /// <summary>
        /// LabelMe点集映射矩形
        /// </summary>
        public static RectangleL ToRectangleL(this IList<double[]> mePoints)
        {
            int x1 = (int)Math.Ceiling(mePoints[0][0]);
            int y1 = (int)Math.Ceiling(mePoints[0][1]);
            int x3 = (int)Math.Ceiling(mePoints[2][0]);
            int y3 = (int)Math.Ceiling(mePoints[2][1]);
            RectangleL rectangleL = new RectangleL(x1, y1, x3 - x1, y3 - y1);

            return rectangleL;
        }
        #endregion

        #region # LabelMe点集映射圆形 —— static CircleL ToCircleL(this IList<double[]> mePoints)
        /// <summary>
        /// LabelMe点集映射圆形
        /// </summary>
        public static CircleL ToCircleL(this IList<double[]> mePoints)
        {
            int x1 = (int)Math.Ceiling(mePoints[0][0]);
            int y1 = (int)Math.Ceiling(mePoints[0][1]);
            int x2 = (int)Math.Ceiling(mePoints[1][0]);
            CircleL circleL = new CircleL(x1, y1, x2 - x1);

            return circleL;
        }
        #endregion

        #region # LabelMe点集映射椭圆形 —— static EllipseL ToEllipseL(this IList<double[]> mePoints)
        /// <summary>
        /// LabelMe点集映射椭圆形
        /// </summary>
        public static EllipseL ToEllipseL(this IList<double[]> mePoints)
        {
            int x1 = (int)Math.Ceiling(mePoints[0][0]);
            int y1 = (int)Math.Ceiling(mePoints[0][1]);
            int x2 = (int)Math.Ceiling(mePoints[1][0]);
            int y3 = (int)Math.Ceiling(mePoints[2][1]);
            EllipseL ellipseL = new EllipseL(x1, y1, x2 - x1, y3 - y1);

            return ellipseL;
        }
        #endregion

        #region # LabelMe点集映射多边形 —— static PolygonL ToPolygonL(this IList<double[]> mePoints)
        /// <summary>
        /// LabelMe点集映射多边形
        /// </summary>
        public static PolygonL ToPolygonL(this IList<double[]> mePoints)
        {
            IEnumerable<PointL> pointLs =
                from mePoint in mePoints
                let x = (int)Math.Ceiling(mePoint[0])
                let y = (int)Math.Ceiling(mePoint[1])
                select new PointL(x, y);
            PolygonL polygonL = new PolygonL(pointLs);

            return polygonL;
        }
        #endregion

        #region # LabelMe点集映射多边形 —— static PolylineL ToPolylineL(this IList<double[]> mePoints)
        /// <summary>
        /// LabelMe点集映射多边形
        /// </summary>
        public static PolylineL ToPolylineL(this IList<double[]> mePoints)
        {
            IEnumerable<PointL> pointLs =
                from mePoint in mePoints
                let x = (int)Math.Ceiling(mePoint[0])
                let y = (int)Math.Ceiling(mePoint[1])
                select new PointL(x, y);
            PolylineL polylineL = new PolylineL(pointLs);

            return polylineL;
        }
        #endregion

        #region # LabelMe点集映射标注信息 —— static Annotation ToAnnotation(this MeShape meShape)
        /// <summary>
        /// LabelMe点集映射标注信息
        /// </summary>
        public static Annotation ToAnnotation(this MeShape meShape)
        {
            ShapeL shapeL;
            if (meShape.ShapeType == Constants.MePoint)
            {
                PointL pointL = meShape.Points.ToPointL();
                PointVisual2D shape = new PointVisual2D
                {
                    X = pointL.X,
                    Y = pointL.Y,
                    Fill = new SolidColorBrush(Colors.Black),
                    Stroke = new SolidColorBrush(Colors.Red)
                };
                shape.Tag = pointL;
                pointL.Tag = shape;
                shapeL = pointL;
            }
            else if (meShape.ShapeType == Constants.MeLine)
            {
                LineL lineL = meShape.Points.ToLineL();
                Line shape = new Line
                {
                    X1 = lineL.A.X,
                    Y1 = lineL.A.Y,
                    X2 = lineL.B.X,
                    Y2 = lineL.B.Y,
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 2
                };
                shape.Tag = lineL;
                lineL.Tag = shape;
                shapeL = lineL;
            }
            else if (meShape.ShapeType == Constants.MeRectangle)
            {
                RectangleL rectangleL = meShape.Points.ToRectangleL();
                RectangleVisual2D shape = new RectangleVisual2D
                {
                    Location = new Point(rectangleL.X, rectangleL.Y),
                    Size = new Size(rectangleL.Width, rectangleL.Height),
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 2
                };
                shape.Tag = rectangleL;
                rectangleL.Tag = shape;
                shapeL = rectangleL;
            }
            else if (meShape.ShapeType == Constants.MeCircle)
            {
                CircleL circleL = meShape.Points.ToCircleL();
                CircleVisual2D shape = new CircleVisual2D
                {
                    Center = new Point(circleL.X, circleL.Y),
                    Radius = circleL.Radius,
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 2
                };
                shape.Tag = circleL;
                circleL.Tag = shape;
                shapeL = circleL;
            }
            else if (meShape.ShapeType == Constants.MeEllipse)
            {
                EllipseL ellipseL = meShape.Points.ToEllipseL();
                EllipseVisual2D shape = new EllipseVisual2D
                {
                    Center = new Point(ellipseL.X, ellipseL.Y),
                    RadiusX = ellipseL.RadiusX,
                    RadiusY = ellipseL.RadiusY,
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 2
                };
                shape.Tag = ellipseL;
                ellipseL.Tag = shape;
                shapeL = ellipseL;
            }
            else if (meShape.ShapeType == Constants.MePolygon)
            {
                PolygonL polygonL = meShape.Points.ToPolygonL();
                IEnumerable<Point> points = polygonL.Points.Select(pointL => new Point(pointL.X, pointL.Y));
                Polygon shape = new Polygon
                {
                    Points = new PointCollection(points),
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 2
                };
                shape.Tag = polygonL;
                polygonL.Tag = shape;
                shapeL = polygonL;
            }
            else if (meShape.ShapeType == Constants.MePolyline)
            {
                PolylineL polylineL = meShape.Points.ToPolylineL();
                IEnumerable<Point> points = polylineL.Points.Select(pointL => new Point(pointL.X, pointL.Y));
                Polyline shape = new Polyline
                {
                    Points = new PointCollection(points),
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 2
                };
                shape.Tag = polylineL;
                polylineL.Tag = shape;
                shapeL = polylineL;
            }
            else
            {
                throw new NotSupportedException("不支持的形状！");
            }

            Annotation annotation = new Annotation(meShape.Label, meShape.GroupId, meShape.Truncated, meShape.Difficult, shapeL, meShape.Description);

            return annotation;
        }
        #endregion
    }
}
