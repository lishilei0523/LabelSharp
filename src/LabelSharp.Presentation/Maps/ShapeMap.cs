using SD.Infrastructure.Shapes;
using System.Collections.Generic;

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
    }
}
