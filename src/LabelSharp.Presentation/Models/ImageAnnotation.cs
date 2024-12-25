using Caliburn.Micro;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Caliburn.Aspects;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LabelSharp.Presentation.Models
{
    /// <summary>
    /// 图像标注
    /// </summary>
    public class ImageAnnotation : PropertyChangedBase
    {
        #region # 字段及构造器

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
        /// <param name="imageFolder">图像文件夹</param>
        /// <param name="imagePath">图像路径</param>
        /// <param name="imageName">图像名称</param>
        /// <param name="imageIndex">图像索引</param>
        public ImageAnnotation(string imageFolder, string imagePath, string imageName, int imageIndex)
            : this()
        {
            this.ImageFolder = imageFolder;
            this.ImagePath = imagePath;
            this.ImageName = imageName;
            this.ImageIndex = imageIndex;
        }

        #endregion

        #region # 属性

        #region 图像文件夹 —— string ImageFolder
        /// <summary>
        /// 图像文件夹
        /// </summary>
        [DependencyProperty]
        public string ImageFolder { get; set; }
        #endregion

        #region 图像路径 —— string ImagePath
        /// <summary>
        /// 图像路径
        /// </summary>
        [DependencyProperty]
        public string ImagePath { get; set; }
        #endregion

        #region 图像名称 —— string ImageName
        /// <summary>
        /// 图像名称
        /// </summary>
        [DependencyProperty]
        public string ImageName { get; set; }
        #endregion

        #region 图像索引 —— int ImageIndex
        /// <summary>
        /// 图像索引
        /// </summary>
        [DependencyProperty]
        public int ImageIndex { get; set; }
        #endregion

        #region 形状列表 —— ObservableCollection<Shape> Shapes
        /// <summary>
        /// 形状列表
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<Shape> Shapes { get; set; }
        #endregion

        #region 形状数据列表 —— ObservableCollection<ShapeL> ShapeLs
        /// <summary>
        /// 形状数据列表
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<ShapeL> ShapeLs { get; set; }
        #endregion

        #region 已选标注信息 —— Annotation SelectedAnnotation
        /// <summary>
        /// 已选标注信息
        /// </summary>
        [DependencyProperty]
        public Annotation SelectedAnnotation { get; set; }
        #endregion

        #region 标注信息列表 —— ObservableCollection<Annotation> Annotations
        /// <summary>
        /// 标注信息列表
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<Annotation> Annotations { get; set; }
        #endregion

        #region 只读属性 - 图像 —— BitmapSource Image
        /// <summary>
        /// 只读属性 - 图像
        /// </summary>
        public BitmapSource Image
        {
            get
            {
                using Mat matrix = Cv2.ImRead(this.ImagePath);
                BitmapSource bitmapSource = matrix.ToBitmapSource();

                return bitmapSource;
            }
        }
        #endregion 

        #endregion
    }
}
