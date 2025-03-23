using LabelSharp.Presentation.Models;
using OpenCvSharp;
using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Visual2Ds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace LabelSharp.Presentation.Maps
{
    /// <summary>
    /// 标注映射
    /// </summary>
    public static class AnnotationMap
    {
        #region # 映射LabelMe形状 —— static MeShape ToMeShape(this Annotation annotation)
        /// <summary>
        /// 映射LabelMe形状
        /// </summary>
        public static MeShape ToMeShape(this Annotation annotation)
        {
            string shapeType = annotation.ShapeL.GetShapeType();
            IList<double[]> mePoints = annotation.ShapeL.ToMePoints();
            MeShape meShape = new MeShape(annotation.Label, annotation.GroupId, annotation.Truncated, annotation.Difficult, shapeType, annotation.Description, mePoints);

            return meShape;
        }
        #endregion

        #region # 映射LabelMe标注 —— static MeAnnotation ToMeAnnotation(this ImageAnnotation...
        /// <summary>
        /// 映射LabelMe标注
        /// </summary>
        public static MeAnnotation ToMeAnnotation(this ImageAnnotation imageAnnotation)
        {
            int imageWidth = imageAnnotation.ImageWidth;
            int imageHeight = imageAnnotation.ImageHeight;
            IList<MeShape> meShapes = imageAnnotation.Annotations.Select(x => x.ToMeShape()).ToList();
            MeAnnotation meAnnotation = new MeAnnotation(imageAnnotation.ImageName, imageWidth, imageHeight, meShapes);

            return meAnnotation;
        }
        #endregion

        #region # 映射PascalVOC标注 —— static PascalAnnotation ToPascalAnnotation(this ImageAnnotation...
        /// <summary>
        /// 映射PascalVOC标注
        /// </summary>
        public static PascalAnnotation ToPascalAnnotation(this ImageAnnotation imageAnnotation)
        {
            IList<PascalAnnotationInfo> pascalAnnotationInfos = new List<PascalAnnotationInfo>();
            foreach (Annotation annotation in imageAnnotation.Annotations)
            {
                if (annotation.ShapeL is RectangleL rectangleL)
                {
                    PascalAnnotationInfo pascalAnnotationInfo = new PascalAnnotationInfo
                    {
                        Name = annotation.Label,
                        Pose = "Unspecified",
                        Truncated = Convert.ToInt32(annotation.Truncated),
                        Difficult = Convert.ToInt32(annotation.Difficult),
                        Location = new Location
                        {
                            XMin = rectangleL.TopLeft.X,
                            YMin = rectangleL.TopLeft.Y,
                            XMax = rectangleL.BottomRight.X,
                            YMax = rectangleL.BottomRight.Y
                        }
                    };
                    pascalAnnotationInfos.Add(pascalAnnotationInfo);
                }
            }

            PascalAnnotation pascalAnnotation = new PascalAnnotation
            {
                Folder = imageAnnotation.ImageFolder,
                Filename = imageAnnotation.ImageName,
                Path = imageAnnotation.ImagePath,
                Source = new AnnotationSource
                {
                    Database = "Unknown"
                },
                ImageSize = new ImageSize
                {
                    Width = imageAnnotation.ImageWidth,
                    Height = imageAnnotation.ImageHeight,
                    Depth = imageAnnotation.ImageChannelsCount
                },
                Segmented = 0,
                Annotations = pascalAnnotationInfos.ToArray()
            };

            return pascalAnnotation;
        }
        #endregion

        #region # PascalVOC标注映射标注信息 —— static IList<Annotation> FromPascalAnnotation(this PascalAnnotation...
        /// <summary>
        /// PascalVOC标注映射标注信息
        /// </summary>
        public static IList<Annotation> FromPascalAnnotation(this PascalAnnotation pascalAnnotation)
        {
            IList<Annotation> annotations = new List<Annotation>();
            foreach (PascalAnnotationInfo pascalAnnotationInfo in pascalAnnotation.Annotations)
            {
                int x = pascalAnnotationInfo.Location.XMin;
                int y = pascalAnnotationInfo.Location.YMin;
                int width = pascalAnnotationInfo.Location.XMax - pascalAnnotationInfo.Location.XMin;
                int height = pascalAnnotationInfo.Location.YMax - pascalAnnotationInfo.Location.YMin;
                RectangleVisual2D rectangle = new RectangleVisual2D()
                {
                    Location = new Point(x, y),
                    Size = new Size(width, height)
                };
                RectangleL rectangleL = new RectangleL(x, y, width, height);
                rectangle.Tag = rectangleL;
                rectangleL.Tag = rectangle;

                bool truncated = Convert.ToBoolean(pascalAnnotationInfo.Truncated);
                bool difficult = Convert.ToBoolean(pascalAnnotationInfo.Difficult);
                Annotation annotation = new Annotation(pascalAnnotationInfo.Name, null, truncated, difficult, rectangleL, string.Empty);
                annotations.Add(annotation);
            }

            return annotations;
        }
        #endregion

        #region # 映射YOLO目标检测标注 —— static string[] ToYoloDetections(this ImageAnnotation imageAnnotation...
        /// <summary>
        /// 映射YOLO目标检测标注
        /// </summary>
        public static string[] ToYoloDetections(this ImageAnnotation imageAnnotation, IList<string> labels)
        {
            int imageWidth = imageAnnotation.ImageWidth;
            int imageHeight = imageAnnotation.ImageHeight;
            string[] lines = imageAnnotation.Annotations.ToYoloDetections(imageWidth, imageHeight, labels);

            return lines;
        }
        #endregion

        #region # 映射YOLO目标检测标注 —— static string[] ToYoloDetections(this IList<Annotation> annotations...
        /// <summary>
        /// 映射YOLO目标检测标注
        /// </summary>
        public static string[] ToYoloDetections(this IList<Annotation> annotations, double imageWidth, double imageHeight, IList<string> labels)
        {
            IList<string> lines = new List<string>();
            foreach (Annotation annotation in annotations)
            {
                if (annotation.ShapeL is RectangleL rectangleL)
                {
                    StringBuilder lineBuilder = new StringBuilder();
                    int labelIndex = labels.IndexOf(annotation.Label);
                    lineBuilder.Append($"{labelIndex} ");

                    float scaledCenterX = (rectangleL.X + rectangleL.Width / 2.0f) / (float)imageWidth;
                    float scaledCenterY = (rectangleL.Y + rectangleL.Height / 2.0f) / (float)imageHeight;
                    float scaledWidth = rectangleL.Width / (float)imageWidth;
                    float scaledHeight = rectangleL.Height / (float)imageHeight;
                    lineBuilder.Append($"{scaledCenterX} ");
                    lineBuilder.Append($"{scaledCenterY} ");
                    lineBuilder.Append($"{scaledWidth} ");
                    lineBuilder.Append($"{scaledHeight} ");

                    lines.Add(lineBuilder.ToString().Trim());
                }
            }

            return lines.ToArray();
        }
        #endregion

        #region # 映射YOLO图像分割标注 —— static string[] ToYoloSegmentations(this ImageAnnotation imageAnnotation...
        /// <summary>
        /// 映射YOLO图像分割标注
        /// </summary>
        public static string[] ToYoloSegmentations(this ImageAnnotation imageAnnotation, IList<string> labels)
        {
            int imageWidth = imageAnnotation.ImageWidth;
            int imageHeight = imageAnnotation.ImageHeight;
            string[] lines = imageAnnotation.Annotations.ToYoloSegmentations(imageWidth, imageHeight, labels);

            return lines;
        }
        #endregion

        #region # 映射YOLO图像分割标注 —— static string[] ToYoloSegmentations(this IList<Annotation> annotations...
        /// <summary>
        /// 映射YOLO图像分割标注
        /// </summary>
        public static string[] ToYoloSegmentations(this IList<Annotation> annotations, double imageWidth, double imageHeight, IList<string> labels)
        {
            IList<string> lines = new List<string>();
            foreach (Annotation annotation in annotations)
            {
                if (annotation.ShapeL is PolygonL || annotation.ShapeL is PolylineL)
                {
                    StringBuilder lineBuilder = new StringBuilder();
                    int labelIndex = labels.IndexOf(annotation.Label);
                    lineBuilder.Append($"{labelIndex} ");

                    ICollection<PointL> pointLs = annotation.ShapeL switch
                    {
                        PolygonL polygonL => polygonL.Points,
                        PolylineL polylineL => polylineL.Points,
                        _ => throw new NotSupportedException()
                    };
                    IEnumerable<Point2f> contour = pointLs.Select(pointL => new Point2f(pointL.X, pointL.Y));
                    Rect boundingBox = Cv2.BoundingRect(contour);

                    float scaledCenterX = (boundingBox.X + boundingBox.Width / 2.0f) / (float)imageWidth;
                    float scaledCenterY = (boundingBox.Y + boundingBox.Height / 2.0f) / (float)imageHeight;
                    float scaledWidth = boundingBox.Width / (float)imageWidth;
                    float scaledHeight = boundingBox.Height / (float)imageHeight;
                    lineBuilder.Append($"{scaledCenterX} ");
                    lineBuilder.Append($"{scaledCenterY} ");
                    lineBuilder.Append($"{scaledWidth} ");
                    lineBuilder.Append($"{scaledHeight} ");
                    foreach (PointL pointL in pointLs)
                    {
                        float scaledX = pointL.X / (float)imageWidth;
                        float scaledY = pointL.Y / (float)imageHeight;
                        lineBuilder.Append($"{scaledX} ");
                        lineBuilder.Append($"{scaledY} ");
                    }

                    lines.Add(lineBuilder.ToString().Trim());
                }
            }

            return lines.ToArray();
        }
        #endregion

        #region # 映射YOLO定向目标检测标注 —— static string[] ToYoloObbDetections(this IList<Annotation> annotations...
        /// <summary>
        /// 映射YOLO定向目标检测标注
        /// </summary>
        public static string[] ToYoloObbDetections(this ImageAnnotation imageAnnotation, IList<string> labels)
        {
            int imageWidth = imageAnnotation.ImageWidth;
            int imageHeight = imageAnnotation.ImageHeight;
            string[] lines = imageAnnotation.Annotations.ToYoloObbDetections(imageWidth, imageHeight, labels);

            return lines;
        }
        #endregion

        #region # 映射YOLO定向目标检测标注 —— static string[] ToYoloObbDetections(this IList<Annotation> annotations...
        /// <summary>
        /// 映射YOLO定向目标检测标注
        /// </summary>
        public static string[] ToYoloObbDetections(this IList<Annotation> annotations, double imageWidth, double imageHeight, IList<string> labels)
        {
            IList<string> lines = new List<string>();
            foreach (Annotation annotation in annotations)
            {
                if (annotation.ShapeL is RotatedRectangleL rotatedRectangleL)
                {
                    StringBuilder lineBuilder = new StringBuilder();
                    int labelIndex = labels.IndexOf(annotation.Label);
                    lineBuilder.Append($"{labelIndex} ");

                    float x1 = rotatedRectangleL.TopLeft.X / (float)imageWidth;
                    float y1 = rotatedRectangleL.TopLeft.Y / (float)imageHeight;
                    float x2 = rotatedRectangleL.TopRight.X / (float)imageWidth;
                    float y2 = rotatedRectangleL.TopRight.Y / (float)imageHeight;
                    float x3 = rotatedRectangleL.BottomRight.X / (float)imageWidth;
                    float y3 = rotatedRectangleL.BottomRight.Y / (float)imageHeight;
                    float x4 = rotatedRectangleL.BottomLeft.X / (float)imageWidth;
                    float y4 = rotatedRectangleL.BottomLeft.Y / (float)imageHeight;
                    lineBuilder.Append($"{x1} ");
                    lineBuilder.Append($"{y1} ");
                    lineBuilder.Append($"{x2} ");
                    lineBuilder.Append($"{y2} ");
                    lineBuilder.Append($"{x3} ");
                    lineBuilder.Append($"{y3} ");
                    lineBuilder.Append($"{x4} ");
                    lineBuilder.Append($"{y4} ");

                    lines.Add(lineBuilder.ToString().Trim());
                }
            }

            return lines.ToArray();
        }
        #endregion

        #region # YOLO目标检测标注映射标注信息 —— static IList<Annotation> FromYoloDetections(this string[] lines...
        /// <summary>
        /// YOLO目标检测标注映射标注信息
        /// </summary>
        public static IList<Annotation> FromYoloDetections(this string[] lines, double imageWidth, double imageHeight, IList<string> labels)
        {
            IList<Annotation> annotations = new List<Annotation>();
            foreach (string line in lines)
            {
                string[] words = line.Split(' ');

                //标签索引
                int labelIndex = int.Parse(words[0]);
                string label = labels.Count > labelIndex ? labels[labelIndex] : labelIndex.ToString();

                //矩形
                if (words.Length == 5)
                {
                    float scaledCenterX = float.Parse(words[1]);
                    float scaledCenterY = float.Parse(words[2]);
                    float scaledWidth = float.Parse(words[3]);
                    float scaledHeight = float.Parse(words[4]);
                    int boxWidth = (int)Math.Ceiling(scaledWidth * imageWidth);
                    int boxHeight = (int)Math.Ceiling(scaledHeight * imageHeight);
                    int x = (int)Math.Ceiling(scaledCenterX * imageWidth - boxWidth / 2.0f);
                    int y = (int)Math.Ceiling(scaledCenterY * imageHeight - boxHeight / 2.0f);

                    RectangleVisual2D rectangle = new RectangleVisual2D()
                    {
                        Location = new Point(x, y),
                        Size = new Size(boxWidth, boxHeight)
                    };
                    RectangleL rectangleL = new RectangleL(x, y, boxWidth, boxHeight);
                    rectangle.Tag = rectangleL;
                    rectangleL.Tag = rectangle;

                    Annotation annotation = new Annotation(label, null, false, false, rectangleL, string.Empty);
                    annotations.Add(annotation);
                }
            }

            return annotations;
        }
        #endregion

        #region # YOLO图像分割标注映射标注信息 —— static IList<Annotation> FromYoloSegmentations(this string[] lines...
        /// <summary>
        /// YOLO图像分割标注映射标注信息
        /// </summary>
        public static IList<Annotation> FromYoloSegmentations(this string[] lines, double imageWidth, double imageHeight, IList<string> labels)
        {
            IList<Annotation> annotations = new List<Annotation>();
            foreach (string line in lines)
            {
                string[] words = line.Split(' ');

                //标签索引
                int labelIndex = int.Parse(words[0]);
                string label = labels.Count > labelIndex ? labels[labelIndex] : labelIndex.ToString();

                //多边形
                if (words.Length > 5)
                {
                    string[] polygonTextArray = new string[words.Length - 5];
                    Array.Copy(words, 5, polygonTextArray, 0, words.Length - 5);
                    double[] polygonArray = polygonTextArray.Select(double.Parse).ToArray();

                    PointCollection points = new PointCollection();
                    IList<PointL> pointLs = new List<PointL>();
                    for (int index = 0; index < polygonArray.Length; index += 2)
                    {
                        Point point = new Point(polygonArray[index] * imageWidth, polygonArray[index + 1] * imageHeight);
                        PointL pointL = new PointL((int)Math.Ceiling(point.X), (int)Math.Ceiling(point.Y));
                        points.Add(point);
                        pointLs.Add(pointL);
                    }

                    Polygon polygon = new Polygon
                    {
                        Points = points,
                        Fill = new SolidColorBrush(Colors.Transparent)
                    };
                    PolygonL polygonL = new PolygonL(pointLs);
                    polygon.Tag = polygonL;
                    polygonL.Tag = polygon;

                    Annotation annotation = new Annotation(label, null, false, false, polygonL, string.Empty);
                    annotations.Add(annotation);
                }
            }

            return annotations;
        }
        #endregion

        #region # YOLO定向目标检测标注映射标注信息 —— static IList<Annotation> FromYoloObbDetections(this string[] lines...
        /// <summary>
        /// YOLO定向目标检测标注映射标注信息
        /// </summary>
        public static IList<Annotation> FromYoloObbDetections(this string[] lines, double imageWidth, double imageHeight, IList<string> labels)
        {
            IList<Annotation> annotations = new List<Annotation>();
            foreach (string line in lines)
            {
                string[] words = line.Split(' ');

                //标签索引
                int labelIndex = int.Parse(words[0]);
                string label = labels.Count > labelIndex ? labels[labelIndex] : labelIndex.ToString();

                //旋转矩形
                if (words.Length == 9)
                {
                    Point2f point1 = new Point2f(float.Parse(words[1]) * (float)imageWidth, float.Parse(words[2]) * (float)imageHeight);
                    Point2f point2 = new Point2f(float.Parse(words[3]) * (float)imageWidth, float.Parse(words[4]) * (float)imageHeight);
                    Point2f point3 = new Point2f(float.Parse(words[5]) * (float)imageWidth, float.Parse(words[6]) * (float)imageHeight);
                    Point2f point4 = new Point2f(float.Parse(words[7]) * (float)imageWidth, float.Parse(words[8]) * (float)imageHeight);
                    RotatedRect rotatedRect = Cv2.MinAreaRect(new[] { point1, point2, point3, point4 });

                    int centerX = (int)Math.Ceiling(rotatedRect.Center.X);
                    int centerY = (int)Math.Ceiling(rotatedRect.Center.Y);
                    int boxWidth = (int)Math.Ceiling(rotatedRect.Size.Width);
                    int boxHeight = (int)Math.Ceiling(rotatedRect.Size.Height);
                    RotatedRectangleVisual2D rotatedRectangle = new RotatedRectangleVisual2D()
                    {
                        Center = new Point(rotatedRect.Center.X, rotatedRect.Center.Y),
                        Size = new Size(rotatedRect.Size.Width, rotatedRect.Size.Height),
                        Angle = rotatedRect.Angle
                    };
                    RotatedRectangleL rotatedRectangleL = new RotatedRectangleL(centerX, centerY, boxWidth, boxHeight, rotatedRect.Angle);
                    rotatedRectangle.Tag = rotatedRectangleL;
                    rotatedRectangleL.Tag = rotatedRectangle;

                    Annotation annotation = new Annotation(label, null, false, false, rotatedRectangleL, string.Empty);
                    annotations.Add(annotation);
                }
            }

            return annotations;
        }
        #endregion
    }
}
