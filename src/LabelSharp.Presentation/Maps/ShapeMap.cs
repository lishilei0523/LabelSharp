using LabelSharp.Presentation.Models;
using SD.Infrastructure.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;

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

        #region # 映射点 —— static PointL ToPointL(this IList<double[]> mePoints)
        /// <summary>
        /// 映射点
        /// </summary>
        public static PointL ToPointL(this IList<double[]> mePoints)
        {
            int x = (int)Math.Ceiling(mePoints[0][0]);
            int y = (int)Math.Ceiling(mePoints[0][1]);
            PointL pointL = new PointL(x, y);

            return pointL;
        }
        #endregion

        #region # 映射线 —— static LineL ToLineL(this IList<double[]> mePoints)
        /// <summary>
        /// 映射线
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

        #region # 映射矩形 —— static RectangleL ToRectangleL(this IList<double[]> mePoints)
        /// <summary>
        /// 映射矩形
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

        #region # 映射圆形 —— static CircleL ToCircleL(this IList<double[]> mePoints)
        /// <summary>
        /// 映射圆形
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

        #region # 映射椭圆形 —— static EllipseL ToEllipseL(this IList<double[]> mePoints)
        /// <summary>
        /// 映射椭圆形
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

        #region # 映射多边形 —— static PolygonL ToPolygonL(this IList<double[]> mePoints)
        /// <summary>
        /// 映射多边形
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

        #region # 映射多边形 —— static PolylineL ToPolylineL(this IList<double[]> mePoints)
        /// <summary>
        /// 映射多边形
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


        public static Annotation ToAnnotation(this MeShape meShape)
        {
            //Shape shape;
            //if (meShape.ShapeType==Constants.MePoint)
            //{
            //    PointL pointL = meShape.Points.ToPointL();
            //    shape = new PointVisual2D
            //    {
            //        X = pointL.X,
            //        Y = pointL.Y,
            //        Fill = new SolidColorBrush(Colors.Black),
            //        Stroke = new SolidColorBrush(this.BorderColor!.Value),
            //        RenderTransform = canvas.MatrixTransform,
            //        Tag = pointL
            //    };
            //}

            Annotation annotation = new Annotation(meShape.Label, meShape.GroupId, meShape.Truncated, meShape.Difficult, null, meShape.Description);

            return annotation;
        }
    }
}
