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
                return "point";
            }
            if (shapeL is LineL)
            {
                return "line";
            }
            if (shapeL is RectangleL)
            {
                return "rectangle";
            }
            if (shapeL is RotatedRectangleL)
            {
                return "rotation";
            }
            if (shapeL is CircleL)
            {
                return "circle";
            }
            if (shapeL is PolygonL)
            {
                return "polygon";
            }
            if (shapeL is PolylineL)
            {
                return "linestrip";
            }

            return string.Empty;
        }
        #endregion

        #region # 转换COCO点集 —— static IList<float[]> ToCocoPoints(this ShapeL shapeL)
        /// <summary>
        /// 转换COCO点集
        /// </summary>
        public static IList<float[]> ToCocoPoints(this ShapeL shapeL)
        {
            if (shapeL is PointL pointL)
            {
                return pointL.ToCocoPoints();
            }
            if (shapeL is LineL lineL)
            {
                return lineL.ToCocoPoints();
            }
            if (shapeL is RectangleL rectangleL)
            {
                return rectangleL.ToCocoPoints();
            }
            if (shapeL is RotatedRectangleL rotatedRectangleL)
            {
                return rotatedRectangleL.ToCocoPoints();
            }
            if (shapeL is CircleL circleL)
            {
                return circleL.ToCocoPoints();
            }
            if (shapeL is PolygonL polygonL)
            {
                return polygonL.ToCocoPoints();
            }
            if (shapeL is PolylineL polylineL)
            {
                return polylineL.ToCocoPoints();
            }

            return new List<float[]>();
        }
        #endregion

        #region # 转换COCO点集 —— static IList<float[]> ToCocoPoints(this PointL pointL)
        /// <summary>
        /// 转换COCO点集
        /// </summary>
        public static IList<float[]> ToCocoPoints(this PointL pointL)
        {
            IList<float[]> cocoPoints = new List<float[]>();
            cocoPoints.Add(new[] { (float)pointL.X, (float)pointL.Y });

            return cocoPoints;
        }
        #endregion

        #region # 转换COCO点集 —— static IList<float[]> ToCocoPoints(this LineL lineL)
        /// <summary>
        /// 转换COCO点集
        /// </summary>
        public static IList<float[]> ToCocoPoints(this LineL lineL)
        {
            IList<float[]> cocoPoints = new List<float[]>();
            cocoPoints.Add(new[] { (float)lineL.A.X, (float)lineL.A.Y });
            cocoPoints.Add(new[] { (float)lineL.B.X, (float)lineL.B.Y });

            return cocoPoints;
        }
        #endregion

        #region # 转换COCO点集 —— static IList<float[]> ToCocoPoints(this RectangleL rectangleL)
        /// <summary>
        /// 转换COCO点集
        /// </summary>
        public static IList<float[]> ToCocoPoints(this RectangleL rectangleL)
        {
            IList<float[]> cocoPoints = new List<float[]>();
            cocoPoints.Add(new[] { (float)rectangleL.TopLeft.X, (float)rectangleL.TopLeft.Y });
            cocoPoints.Add(new[] { (float)rectangleL.TopRight.X, (float)rectangleL.TopRight.Y });
            cocoPoints.Add(new[] { (float)rectangleL.BottomRight.X, (float)rectangleL.BottomRight.Y });
            cocoPoints.Add(new[] { (float)rectangleL.BottomLeft.X, (float)rectangleL.BottomLeft.Y });

            return cocoPoints;
        }
        #endregion

        #region # 转换COCO点集 —— static IList<float[]> ToCocoPoints(this RotatedRectangleL rotatedRectangleL)
        /// <summary>
        /// 转换COCO点集
        /// </summary>
        public static IList<float[]> ToCocoPoints(this RotatedRectangleL rotatedRectangleL)
        {
            IList<float[]> cocoPoints = new List<float[]>();
            cocoPoints.Add(new[] { (float)rotatedRectangleL.TopLeft.X, (float)rotatedRectangleL.TopLeft.Y });
            cocoPoints.Add(new[] { (float)rotatedRectangleL.TopRight.X, (float)rotatedRectangleL.TopRight.Y });
            cocoPoints.Add(new[] { (float)rotatedRectangleL.BottomRight.X, (float)rotatedRectangleL.BottomRight.Y });
            cocoPoints.Add(new[] { (float)rotatedRectangleL.BottomLeft.X, (float)rotatedRectangleL.BottomLeft.Y });

            return cocoPoints;
        }
        #endregion

        #region # 转换COCO点集 —— static IList<float[]> ToCocoPoints(this CircleL circleL)
        /// <summary>
        /// 转换COCO点集
        /// </summary>
        public static IList<float[]> ToCocoPoints(this CircleL circleL)
        {
            IList<float[]> cocoPoints = new List<float[]>();
            cocoPoints.Add(new[] { (float)circleL.X, (float)circleL.Y });
            cocoPoints.Add(new[] { (float)(circleL.X + circleL.Radius), (float)circleL.Y });

            return cocoPoints;
        }
        #endregion

        #region # 转换COCO点集 —— static IList<float[]> ToCocoPoints(this PolygonL polygonL)
        /// <summary>
        /// 转换COCO点集
        /// </summary>
        public static IList<float[]> ToCocoPoints(this PolygonL polygonL)
        {
            IList<float[]> cocoPoints = new List<float[]>();
            foreach (PointL pointL in polygonL.Points)
            {
                cocoPoints.Add(new[] { (float)pointL.X, (float)pointL.Y });
            }

            return cocoPoints;
        }
        #endregion

        #region # 转换COCO点集 —— static IList<float[]> ToCocoPoints(this PolylineL polylineL)
        /// <summary>
        /// 转换COCO点集
        /// </summary>
        public static IList<float[]> ToCocoPoints(this PolylineL polylineL)
        {
            IList<float[]> cocoPoints = new List<float[]>();
            foreach (PointL pointL in polylineL.Points)
            {
                cocoPoints.Add(new[] { (float)pointL.X, (float)pointL.Y });
            }

            return cocoPoints;
        }
        #endregion
    }
}
