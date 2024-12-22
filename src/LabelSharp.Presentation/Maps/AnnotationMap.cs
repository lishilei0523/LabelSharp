using LabelSharp.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

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
            MeAnnotation meAnnotation = new MeAnnotation(imageAnnotation.ImagePath, imageWidth, imageHeight, meShapes);

            return meAnnotation;
        }
        #endregion
    }
}
