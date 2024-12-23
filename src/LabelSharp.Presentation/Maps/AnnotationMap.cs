using LabelSharp.Presentation.Models;
using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Visual2Ds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LabelSharp.Presentation.Maps
{
    /// <summary>
    /// 标注映射
    /// </summary>
    public static class AnnotationMap
    {
        #region # 映射LabelMe形状 —— static MeShape ToMeShpe(this Annotation annotation)
        /// <summary>
        /// 映射LabelMe形状
        /// </summary>
        public static MeShape ToMeShpe(this Annotation annotation)
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
            BitmapSource image = imageAnnotation.Image;
            int imageWidth = (int)Math.Ceiling(image.Width);
            int imageHeight = (int)Math.Ceiling(image.Height);
            IList<MeShape> meShapes = imageAnnotation.Annotations.Select(x => x.ToMeShpe()).ToList();
            MeAnnotation meAnnotation = new MeAnnotation(imageAnnotation.ImageName, imageWidth, imageHeight, meShapes);

            return meAnnotation;
        }
        #endregion

        #region # 映射YOLO目标检测标注 —— static string[] ToYoloDetenctions(this ImageAnnotation imageAnnotation...
        /// <summary>
        /// 映射YOLO目标检测标注
        /// </summary>
        public static string[] ToYoloDetenctions(this ImageAnnotation imageAnnotation, IList<string> labels)
        {
            BitmapSource currentImage = imageAnnotation.Image;
            string[] lines = new string[imageAnnotation.Annotations.Count];
            for (int index = 0; index < lines.Length; index++)
            {
                StringBuilder lineBuilder = new StringBuilder();
                Annotation annotation = imageAnnotation.Annotations[index];
                int labelIndex = labels.IndexOf(annotation.Label);
                lineBuilder.Append($"{labelIndex} ");
                if (annotation.ShapeL is RectangleL rectangleL)
                {
                    float scaledCenterX = (rectangleL.X + rectangleL.Width / 2.0f) / (float)currentImage.Width;
                    float scaledCenterY = (rectangleL.Y + rectangleL.Height / 2.0f) / (float)currentImage.Height;
                    float scaledWidth = rectangleL.Width / (float)currentImage.Width;
                    float scaledHeight = rectangleL.Height / (float)currentImage.Height;
                    lineBuilder.Append($"{scaledCenterX} ");
                    lineBuilder.Append($"{scaledCenterY} ");
                    lineBuilder.Append($"{scaledWidth} ");
                    lineBuilder.Append($"{scaledHeight} ");
                }

                lines[index] = lineBuilder.ToString().Trim();
            }

            return lines;
        }
        #endregion

        #region # 映射YOLO图像分割标注 —— static string[] ToYoloSegmentations(this ImageAnnotation imageAnnotation...
        /// <summary>
        /// 映射YOLO图像分割标注
        /// </summary>
        public static string[] ToYoloSegmentations(this ImageAnnotation imageAnnotation, IList<string> labels)
        {
            BitmapSource currentImage = imageAnnotation.Image;
            string[] lines = new string[imageAnnotation.Annotations.Count];
            for (int index = 0; index < lines.Length; index++)
            {
                StringBuilder lineBuilder = new StringBuilder();
                Annotation annotation = imageAnnotation.Annotations[index];
                int labelIndex = labels.IndexOf(annotation.Label);
                lineBuilder.Append($"{labelIndex} ");
                if (annotation.ShapeL is PolygonL polygonL)
                {
                    Rect boundingBox = annotation.Shape.RenderedGeometry.Bounds;
                    float scaledCenterX = (float)(boundingBox.X + boundingBox.Width / 2.0f) / (float)currentImage.Width;
                    float scaledCenterY = (float)(boundingBox.Y + boundingBox.Height / 2.0f) / (float)currentImage.Height;
                    float scaledWidth = (float)boundingBox.Width / (float)currentImage.Width;
                    float scaledHeight = (float)boundingBox.Height / (float)currentImage.Height;
                    lineBuilder.Append($"{scaledCenterX} ");
                    lineBuilder.Append($"{scaledCenterY} ");
                    lineBuilder.Append($"{scaledWidth} ");
                    lineBuilder.Append($"{scaledHeight} ");
                    foreach (PointL pointL in polygonL.Points)
                    {
                        float scaledX = pointL.X / (float)currentImage.Width;
                        float scaledY = pointL.Y / (float)currentImage.Height;
                        lineBuilder.Append($"{scaledX} ");
                        lineBuilder.Append($"{scaledY} ");
                    }
                }

                lines[index] = lineBuilder.ToString().Trim();
            }

            return lines;
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
                        Size = new Size(boxWidth, boxHeight),
                        Fill = new SolidColorBrush(Colors.Transparent),
                        Stroke = new SolidColorBrush(Colors.Red),
                        StrokeThickness = 2
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

                    IList<Point> points = new List<Point>();
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
                        Points = new PointCollection(points),
                        Fill = new SolidColorBrush(Colors.Transparent),
                        Stroke = new SolidColorBrush(Colors.Red),
                        StrokeThickness = 2
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
    }
}
