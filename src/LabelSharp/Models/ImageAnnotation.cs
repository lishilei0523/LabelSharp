using Caliburn.Micro;
using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Caliburn.Aspects;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LabelSharp.Models
{
    /// <summary>
    /// 图像标注
    /// </summary>
    public class ImageAnnotation : PropertyChangedBase
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        public ImageAnnotation()
        {
            this.Shapes = new ObservableCollection<Shape>();
            this.ShapeLs = new ObservableCollection<ShapeL>();
            this.Annotations = new ObservableCollection<Annotation>();
        }

        /// <summary>
        /// 创建图像标注构造器
        /// </summary>
        /// <param name="image">图像</param>
        /// <param name="imagePath">图像路径</param>
        /// <param name="imageName">图像名称</param>
        /// <param name="imageIndex">图像索引</param>
        public ImageAnnotation(BitmapSource image, string imagePath, string imageName, int imageIndex)
            : this()
        {
            this.Image = image;
            this.ImagePath = imagePath;
            this.ImageName = imageName;
            this.ImageIndex = imageIndex;
        }

        /// <summary>
        /// 图像
        /// </summary>
        [DependencyProperty]
        public BitmapSource Image { get; set; }

        /// <summary>
        /// 图像路径
        /// </summary>
        [DependencyProperty]
        public string ImagePath { get; set; }

        /// <summary>
        /// 图像名称
        /// </summary>
        [DependencyProperty]
        public string ImageName { get; set; }

        /// <summary>
        /// 图像索引
        /// </summary>
        [DependencyProperty]
        public int ImageIndex { get; set; }

        /// <summary>
        /// 形状集
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<Shape> Shapes { get; set; }

        /// <summary>
        /// 形状数据集
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<ShapeL> ShapeLs { get; set; }

        /// <summary>
        /// 已选标注信息
        /// </summary>
        [DependencyProperty]
        public Annotation SelectedAnnotation { get; set; }

        /// <summary>
        /// 标注信息列表
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<Annotation> Annotations { get; set; }
    }
}
