using Caliburn.Micro;
using LabelSharp.Presentation.Maps;
using LabelSharp.Presentation.Models;
using LabelSharp.ViewModels.CommonContext;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using SD.Common;
using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Caliburn.Aspects;
using SD.Infrastructure.WPF.Caliburn.Base;
using SD.Infrastructure.WPF.Visual2Ds;
using SD.IOC.Core.Mediators;
using SD.OpenCV.OnnxRuntime.Models;
using SD.OpenCV.OnnxRuntime.Values;
using SD.Toolkits.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace LabelSharp.ViewModels.HomeContext
{
    /// <summary>
    /// 首页视图模型 - 主体部分
    /// </summary>
    public partial class IndexViewModel : ScreenBase
    {
        #region # 字段及构造器

        /// <summary>
        /// 窗体管理器
        /// </summary>
        private readonly IWindowManager _windowManager;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        public IndexViewModel(IWindowManager windowManager)
        {
            this._windowManager = windowManager;
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

        #region 背景颜色 —— SolidColorBrush BackgroundColor
        /// <summary>
        /// 背景颜色
        /// </summary>
        [DependencyProperty]
        public SolidColorBrush BackgroundColor { get; set; }
        #endregion

        #region 边框颜色 —— Color BorderColor
        /// <summary>
        /// 边框颜色
        /// </summary>
        [DependencyProperty]
        public Color BorderColor { get; set; }
        #endregion

        #region 边框粗细 —— int BorderThickness
        /// <summary>
        /// 边框粗细
        /// </summary>
        [DependencyProperty]
        public int BorderThickness { get; set; }
        #endregion

        #region 参考线粗细 —— double GuideLineThickness
        /// <summary>
        /// 参考线粗细
        /// </summary>
        [DependencyProperty]
        public double GuideLineThickness { get; set; }
        #endregion

        #region 水平参考线Y坐标 —— double HorizontalLineY
        /// <summary>
        /// 水平参考线Y坐标
        /// </summary>
        [DependencyProperty]
        public double HorizontalLineY { get; set; }
        #endregion

        #region 垂直参考线X坐标 —— double VerticalLineX
        /// <summary>
        /// 垂直参考线X坐标
        /// </summary>
        [DependencyProperty]
        public double VerticalLineX { get; set; }
        #endregion

        #region 显示参考线 —— bool ShowGuideLines
        /// <summary>
        /// 显示参考线
        /// </summary>
        [DependencyProperty]
        public bool ShowGuideLines { get; set; }
        #endregion

        #region 参考线可见性 —— Visibility GuideLinesVisibility
        /// <summary>
        /// 参考线可见性
        /// </summary>
        [DependencyProperty]
        public Visibility GuideLinesVisibility { get; set; }
        #endregion

        #region 鼠标X坐标 —— int? MousePositionX
        /// <summary>
        /// 鼠标X坐标
        /// </summary>
        [DependencyProperty]
        public int? MousePositionX { get; set; }
        #endregion

        #region 鼠标Y坐标 —— int? MousePositionY
        /// <summary>
        /// 鼠标Y坐标
        /// </summary>
        [DependencyProperty]
        public int? MousePositionY { get; set; }
        #endregion

        #endregion

        #region # 方法

        //Initializations

        #region 初始化 —— Task OnInitializeAsync(CancellationToken cancellationToken)
        /// <summary>
        /// 初始化
        /// </summary>
        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            //默认值
            this._polyAnchors = new List<PointVisual2D>();
            this._polyAnchorLines = new List<Line>();
            this.BackgroundColor = new SolidColorBrush(Colors.LightGray);
            this.BorderColor = Colors.Red;
            this.BorderThickness = 2;
            this.GuideLineThickness = 2;
            this.ShowGuideLines = true;
            this.GuideLinesVisibility = Visibility.Visible;
            this.ScaleChecked = true;
            this.Labels = new ObservableCollection<string>();

            return base.OnInitializeAsync(cancellationToken);
        }
        #endregion


        //常用

        #region 切换参考线 —— void SwitchGuideLinesVisibility()
        /// <summary>
        /// 切换参考线
        /// </summary>
        public void SwitchGuideLinesVisibility()
        {
            this.GuideLinesVisibility = this.ShowGuideLines ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion

        #region 重置 —— void Reset()
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                MessageBox.Show("当前未加载图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            MessageBoxResult result = MessageBox.Show("确定要重置吗？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                this.ClearAnnotations();
            }
        }
        #endregion

        #region 设置样式 —— async void SetStyle()
        /// <summary>
        /// 设置样式
        /// </summary>
        public async void SetStyle()
        {
            StyleViewModel viewModel = ResolveMediator.Resolve<StyleViewModel>();
            viewModel.BackgroundColor = this.BackgroundColor.Color;
            viewModel.BorderColor = this.BorderColor;
            viewModel.BorderThickness = this.BorderThickness;
            bool? result = await this._windowManager.ShowDialogAsync(viewModel);
            if (result == true)
            {
                this.BackgroundColor.Color = viewModel.BackgroundColor!.Value;
                this.BorderColor = viewModel.BorderColor!.Value;
                this.BorderThickness = viewModel.BorderThickness!.Value;
            }
        }
        #endregion


        //文件

        #region 打开文件 —— async void OpenImage()
        /// <summary>
        /// 打开文件
        /// </summary>
        public async void OpenImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "请选择图像",
                Filter = "图片文件(*.jpg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                AddExtension = true,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                this.ImageFolder = openFileDialog.FileName.Replace(Path.GetFileName(openFileDialog.FileName), string.Empty);

                //加载标签
                await this.LoadLabels();

                string imagePath = openFileDialog.FileName;
                string imageName = Path.GetFileName(imagePath);
                ImageAnnotation imageAnnotation = new ImageAnnotation(this.ImageFolder, imagePath, imageName, 1);
                this.ImageAnnotations = new ObservableCollection<ImageAnnotation>(new[] { imageAnnotation });
                this.SelectedImageAnnotation = imageAnnotation;
            }
        }
        #endregion

        #region 打开文件夹 —— async void OpenImageFolder()
        /// <summary>
        /// 打开文件夹
        /// </summary>
        public async void OpenImageFolder()
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog
            {
                Title = "请选择图像文件夹",
                IsFolderPicker = true
            };
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.Busy();

                this.ImageFolder = folderDialog.FileName;

                //加载标签
                await this.LoadLabels();

                string[] imagePaths = Directory.GetFiles(this.ImageFolder);

                #region # 验证

                if (!imagePaths.Any())
                {
                    MessageBox.Show("当前文件夹为空！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                #endregion

                this.ImageAnnotations = new ObservableCollection<ImageAnnotation>();
                int sort = 1;
                foreach (string imagePath in imagePaths)
                {
                    string fileExtension = Path.GetExtension(imagePath);
                    if (Constants.AvailableImageFormats.Contains(fileExtension))
                    {
                        string imageName = Path.GetFileName(imagePath);
                        ImageAnnotation imageAnnotation = new ImageAnnotation(this.ImageFolder, imagePath, imageName, sort);
                        this.ImageAnnotations.Add(imageAnnotation);
                        if (this.SelectedImageAnnotation == null)
                        {
                            this.SelectedImageAnnotation = imageAnnotation;
                        }
                        sort++;
                    }
                }

                this.Idle();
            }
        }
        #endregion

        #region 关闭全部 —— void CloseAll()
        /// <summary>
        /// 关闭全部
        /// </summary>
        public void CloseAll()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                MessageBox.Show("当前未加载图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            MessageBoxResult result = MessageBox.Show("确定要关闭吗？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                this.ImageFolder = null;
                this.SelectedImageAnnotation = null;
                this.ImageAnnotations = new ObservableCollection<ImageAnnotation>();
            }
        }
        #endregion

        #region 保存 —— async void SaveAnnotations()
        /// <summary>
        /// 保存
        /// </summary>
        public async void SaveAnnotations()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                MessageBox.Show("当前未加载图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            this.Busy();

            //保存JSON
            string annotationName = Path.GetFileNameWithoutExtension(this.SelectedImageAnnotation.ImagePath);
            string annotationPath = $"{this.ImageFolder}/{annotationName}.json";
            MeAnnotation meAnnotation = this.SelectedImageAnnotation.ToMeAnnotation();
            string meAnnotationJson = meAnnotation.ToJson();
            await Task.Run(() => File.WriteAllText(annotationPath, meAnnotationJson));

            //保存标签
            await this.SaveLabels();

            this.Idle();
            this.ToastSuccess("已保存！");
        }
        #endregion

        #region 另存为 —— async void SaveAsAnnotations()
        /// <summary>
        /// 另存为
        /// </summary>
        public async void SaveAsAnnotations()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                MessageBox.Show("当前未加载图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "(*.json)|*.json",
                FileName = Path.GetFileNameWithoutExtension(this.SelectedImageAnnotation.ImagePath),
                AddExtension = true,
                RestoreDirectory = true
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                this.Busy();

                //保存JSON
                MeAnnotation meAnnotation = this.SelectedImageAnnotation.ToMeAnnotation();
                string meAnnotationJson = meAnnotation.ToJson();
                await Task.Run(() => File.WriteAllText(saveFileDialog.FileName, meAnnotationJson));

                this.Idle();
                this.ToastSuccess("已保存！");
            }
        }
        #endregion


        //编辑

        #region 切割图像 —— async void CutImage()
        /// <summary>
        /// 切割图像
        /// </summary>
        public async void CutImage()
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog
            {
                Title = "请选择目标文件夹",
                IsFolderPicker = true
            };
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.Busy();

                using Mat image = this.SelectedImageAnnotation.Image.ToMat();
                IList<Mat> results = new List<Mat>();
                foreach (ShapeL shapeL in this.SelectedImageAnnotation.ShapeLs)
                {
                    const int thickness = -1;
                    if (shapeL is RectangleL rectangleL)
                    {
                        //生成掩膜
                        using Mat mask = Mat.Zeros(image.Size(), MatType.CV_8UC1);
                        OpenCvSharp.Rect rect = new OpenCvSharp.Rect(rectangleL.X, rectangleL.Y, rectangleL.Width, rectangleL.Height);
                        await Task.Run(() => mask.Rectangle(rect, Scalar.White, thickness));

                        //适用掩膜
                        using Mat canvas = new Mat();
                        image.CopyTo(canvas, mask);

                        //提取有效区域
                        Mat result = canvas[rect];
                        results.Add(result);
                    }
                    if (shapeL is CircleL circleL)
                    {
                        //生成掩膜
                        using Mat mask = Mat.Zeros(image.Size(), MatType.CV_8UC1);
                        await Task.Run(() => mask.Circle(circleL.X, circleL.Y, circleL.Radius, Scalar.White, thickness));

                        //适用掩膜
                        using Mat canvas = new Mat();
                        image.CopyTo(canvas, mask);

                        //提取有效区域
                        int x = circleL.X - circleL.Radius;
                        int y = circleL.Y - circleL.Radius;
                        int sideSize = circleL.Radius * 2;
                        OpenCvSharp.Rect boundingRect = new OpenCvSharp.Rect(x, y, sideSize, sideSize);
                        Mat result = canvas[boundingRect];
                        results.Add(result);
                    }
                    if (shapeL is EllipseL ellipseL)
                    {
                        //生成掩膜
                        using Mat mask = Mat.Zeros(image.Size(), MatType.CV_8UC1);
                        Point2f center = new Point2f(ellipseL.X, ellipseL.Y);
                        Size2f size = new Size2f(ellipseL.RadiusX * 2, ellipseL.RadiusY * 2);
                        RotatedRect rect = new RotatedRect(center, size, 0);
                        await Task.Run(() => mask.Ellipse(rect, Scalar.White, thickness));

                        //适用掩膜
                        using Mat canvas = new Mat();
                        image.CopyTo(canvas, mask);

                        //提取有效区域
                        int x = ellipseL.X - ellipseL.RadiusX;
                        int y = ellipseL.Y - ellipseL.RadiusY;
                        int width = ellipseL.RadiusX * 2;
                        int height = ellipseL.RadiusY * 2;
                        OpenCvSharp.Rect boundingRect = new OpenCvSharp.Rect(x, y, width, height);
                        Mat result = canvas[boundingRect];
                        results.Add(result);
                    }
                    if (shapeL is PolygonL polygonL)
                    {
                        //生成掩膜
                        using Mat mask = Mat.Zeros(image.Size(), MatType.CV_8UC1);
                        OpenCvSharp.Point[] contour = new OpenCvSharp.Point[polygonL.Points.Count];
                        for (int index = 0; index < polygonL.Points.Count; index++)
                        {
                            PointL pointL = polygonL.Points.ElementAt(index);
                            contour[index] = new OpenCvSharp.Point(pointL.X, pointL.Y);
                        }
                        await Task.Run(() => mask.DrawContours(new[] { contour }, 0, Scalar.White, thickness));

                        //适用掩膜
                        using Mat canvas = new Mat();
                        image.CopyTo(canvas, mask);

                        //提取有效区域
                        OpenCvSharp.Rect boundingRect = Cv2.BoundingRect(contour);
                        Mat result = canvas[boundingRect];
                        results.Add(result);
                    }
                    if (shapeL is PolylineL polylineL)
                    {
                        //生成掩膜
                        using Mat mask = Mat.Zeros(image.Size(), MatType.CV_8UC1);
                        OpenCvSharp.Point[] contour = new OpenCvSharp.Point[polylineL.Points.Count];
                        for (int index = 0; index < polylineL.Points.Count; index++)
                        {
                            PointL pointL = polylineL.Points.ElementAt(index);
                            contour[index] = new OpenCvSharp.Point(pointL.X, pointL.Y);
                        }
                        await Task.Run(() => mask.DrawContours(new[] { contour }, 0, Scalar.White, thickness));

                        //适用掩膜
                        using Mat canvas = new Mat();
                        image.CopyTo(canvas, mask);

                        //提取有效区域
                        OpenCvSharp.Rect boundingRect = Cv2.BoundingRect(contour);
                        Mat result = canvas[boundingRect];
                        results.Add(result);
                    }
                }

                string imageName = Path.GetFileNameWithoutExtension(this.SelectedImageAnnotation.ImagePath);
                string imageExtension = Path.GetExtension(this.SelectedImageAnnotation.ImagePath);
                for (int index = 0; index < results.Count; index++)
                {
                    using Mat imagePart = results[index];
                    string imagePartPath = $@"{folderDialog.FileName}\{imageName}-Part{index}{imageExtension}";
                    await Task.Run(() => imagePart.SaveImage(imagePartPath));
                }

                this.Idle();
                this.ToastSuccess("已保存！");
            }
        }
        #endregion

        #region 保存掩膜 —— async void SaveMask()
        /// <summary>
        /// 保存掩膜
        /// </summary>
        public async void SaveMask()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                MessageBox.Show("当前未加载图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "(*.jpg)|*.jpg|(*.png)|*.png|(*.bmp)|*.bmp",
                FileName = $"{Path.GetFileNameWithoutExtension(this.SelectedImageAnnotation.ImagePath)}_Mask",
                AddExtension = true,
                RestoreDirectory = true
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                this.Busy();

                BitmapSource image = this.SelectedImageAnnotation.Image;
                OpenCvSharp.Size maskSize = new OpenCvSharp.Size(image.Width, image.Height);
                using Mat mask = Mat.Zeros(maskSize, MatType.CV_8UC1);
                foreach (ShapeL shapeL in this.SelectedImageAnnotation.ShapeLs)
                {
                    const int thickness = -1;
                    if (shapeL is RectangleL rectangleL)
                    {
                        OpenCvSharp.Rect rect = new OpenCvSharp.Rect(rectangleL.X, rectangleL.Y, rectangleL.Width, rectangleL.Height);
                        await Task.Run(() => mask.Rectangle(rect, Scalar.White, thickness));
                    }
                    if (shapeL is CircleL circleL)
                    {
                        await Task.Run(() => mask.Circle(circleL.X, circleL.Y, circleL.Radius, Scalar.White, thickness));
                    }
                    if (shapeL is EllipseL ellipseL)
                    {
                        Point2f center = new Point2f(ellipseL.X, ellipseL.Y);
                        Size2f size = new Size2f(ellipseL.RadiusX * 2, ellipseL.RadiusY * 2);
                        RotatedRect rect = new RotatedRect(center, size, 0);
                        await Task.Run(() => mask.Ellipse(rect, Scalar.White, thickness));
                    }
                    if (shapeL is PolygonL polygonL)
                    {
                        OpenCvSharp.Point[] contour = new OpenCvSharp.Point[polygonL.Points.Count];
                        for (int index = 0; index < polygonL.Points.Count; index++)
                        {
                            PointL pointL = polygonL.Points.ElementAt(index);
                            contour[index] = new OpenCvSharp.Point(pointL.X, pointL.Y);
                        }
                        await Task.Run(() => mask.DrawContours(new[] { contour }, 0, Scalar.White, thickness));
                    }
                    if (shapeL is PolylineL polylineL)
                    {
                        OpenCvSharp.Point[] contour = new OpenCvSharp.Point[polylineL.Points.Count];
                        for (int index = 0; index < polylineL.Points.Count; index++)
                        {
                            PointL pointL = polylineL.Points.ElementAt(index);
                            contour[index] = new OpenCvSharp.Point(pointL.X, pointL.Y);
                        }
                        await Task.Run(() => mask.DrawContours(new[] { contour }, 0, Scalar.White, thickness));
                    }
                }

                await Task.Run(() => mask.SaveImage(saveFileDialog.FileName));

                this.Idle();
                this.ToastSuccess("已保存！");
            }
        }
        #endregion

        #region 导入标签 —— async void ImportLabels()
        /// <summary>
        /// 导入标签
        /// </summary>
        public async void ImportLabels()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "请选择标签文件",
                Filter = "标注文件(*.txt)|*.txt",
                AddExtension = true,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                this.Busy();

                string[] labels = await Task.Run(() => File.ReadAllLines(openFileDialog.FileName));
                foreach (string label in labels)
                {
                    if (!this.Labels.Contains(label))
                    {
                        this.Labels.Add(label);
                    }
                }
                await this.SaveLabels();

                this.Idle();
                this.ToastSuccess("已保存！");
            }
        }
        #endregion

        #region 导入PascalVOC —— async void ImportPascal()
        /// <summary>
        /// 导入PascalVOC
        /// </summary>
        public async void ImportPascal()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                MessageBox.Show("当前未加载图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "请选择PascalVOI标注文件",
                Filter = "标注文件(*.xml)|*.xml",
                AddExtension = true,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                this.Busy();

                string pascalAnnotationXml = await Task.Run(() => File.ReadAllText(openFileDialog.FileName));
                PascalAnnotation pascalAnnotation = pascalAnnotationXml.AsXmlTo<PascalAnnotation>();
                IList<Annotation> annotations = pascalAnnotation.FromPascalAnnotation();
                foreach (Annotation annotation in annotations)
                {
                    annotation.Shape.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                    this.SelectedImageAnnotation.Shapes.Add(annotation.Shape);
                    this.SelectedImageAnnotation.ShapeLs.Add(annotation.ShapeL);
                    this.SelectedImageAnnotation.Annotations.Add(annotation);
                    if (!this.Labels.Contains(annotation.Label))
                    {
                        this.Labels.Add(annotation.Label);
                    }
                }

                this.Idle();
                this.ToastSuccess("导入成功！");
            }
        }
        #endregion

        #region 导入YOLO-det —— async void ImportYoloDet()
        /// <summary>
        /// 导入YOLO-det
        /// </summary>
        public async void ImportYoloDet()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                MessageBox.Show("当前未加载图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "请选择YOLO目标检测标注",
                Filter = "标注文件(*.txt)|*.txt",
                AddExtension = true,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                this.Busy();

                BitmapSource image = this.SelectedImageAnnotation.Image;
                string[] lines = await Task.Run(() => File.ReadAllLines(openFileDialog.FileName));
                IList<Annotation> annotations = lines.FromYoloDetections(image.Width, image.Height, this.Labels);
                foreach (Annotation annotation in annotations)
                {
                    annotation.Shape.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                    this.SelectedImageAnnotation.Shapes.Add(annotation.Shape);
                    this.SelectedImageAnnotation.ShapeLs.Add(annotation.ShapeL);
                    this.SelectedImageAnnotation.Annotations.Add(annotation);
                    if (!this.Labels.Contains(annotation.Label))
                    {
                        this.Labels.Add(annotation.Label);
                    }
                }

                this.Idle();
                this.ToastSuccess("导入成功！");
            }
        }
        #endregion

        #region 导入YOLO-seg —— async void ImportYoloSeg()
        /// <summary>
        /// 导入YOLO-seg
        /// </summary>
        public async void ImportYoloSeg()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                MessageBox.Show("当前未加载图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "请选择YOLO图像分割标注",
                Filter = "标注文件(*.txt)|*.txt",
                AddExtension = true,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                this.Busy();

                BitmapSource image = this.SelectedImageAnnotation.Image;
                string[] lines = await Task.Run(() => File.ReadAllLines(openFileDialog.FileName));
                IList<Annotation> annotations = lines.FromYoloSegmentations(image.Width, image.Height, this.Labels);
                foreach (Annotation annotation in annotations)
                {
                    annotation.Shape.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                    this.SelectedImageAnnotation.Shapes.Add(annotation.Shape);
                    this.SelectedImageAnnotation.ShapeLs.Add(annotation.ShapeL);
                    this.SelectedImageAnnotation.Annotations.Add(annotation);
                    if (!this.Labels.Contains(annotation.Label))
                    {
                        this.Labels.Add(annotation.Label);
                    }
                }

                this.Idle();
                this.ToastSuccess("导入成功！");
            }
        }
        #endregion

        #region 导出PascalVOC —— async void ExportPascal()
        /// <summary>
        /// 导出PascalVOC
        /// </summary>
        public async void ExportPascal()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                MessageBox.Show("当前未加载图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "(*.xml)|*.xml",
                FileName = Path.GetFileNameWithoutExtension(this.SelectedImageAnnotation.ImagePath),
                AddExtension = true,
                RestoreDirectory = true
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                this.Busy();

                PascalAnnotation pascalAnnotation = this.SelectedImageAnnotation.ToPascalAnnotation();
                string pascalAnnotationXml = pascalAnnotation.ToXml();
                await Task.Run(() => File.WriteAllText(saveFileDialog.FileName, pascalAnnotationXml));

                this.Idle();
                this.ToastSuccess("已保存");
            }
        }
        #endregion

        #region 导出YOLO-det —— async void ExportYoloDet()
        /// <summary>
        /// 导出YOLO-det
        /// </summary>
        public async void ExportYoloDet()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                MessageBox.Show("当前未加载图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "(*.txt)|*.txt",
                FileName = Path.GetFileNameWithoutExtension(this.SelectedImageAnnotation.ImagePath),
                AddExtension = true,
                RestoreDirectory = true
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                this.Busy();

                string[] lines = this.SelectedImageAnnotation.ToYoloDetenctions(this.Labels);
                await Task.Run(() => File.WriteAllLines(saveFileDialog.FileName, lines));

                this.Idle();
                this.ToastSuccess("已保存");
            }
        }
        #endregion

        #region 导出YOLO-seg —— async void ExportYoloSeg()
        /// <summary>
        /// 导出YOLO-seg
        /// </summary>
        public async void ExportYoloSeg()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                MessageBox.Show("当前未加载图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "(*.txt)|*.txt",
                FileName = Path.GetFileNameWithoutExtension(this.SelectedImageAnnotation.ImagePath),
                AddExtension = true,
                RestoreDirectory = true
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                this.Busy();

                string[] lines = this.SelectedImageAnnotation.ToYoloSegmentations(this.Labels);
                await Task.Run(() => File.WriteAllLines(saveFileDialog.FileName, lines));

                this.Idle();
                this.ToastSuccess("已保存");
            }
        }
        #endregion


        //工具

        #region YOLO目标检测 —— async void YoloDetect()
        /// <summary>
        /// YOLO目标检测
        /// </summary>
        public async void YoloDetect()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                MessageBox.Show("当前未加载图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            this.Busy();

            ThresholdViewModel viewModel = ResolveMediator.Resolve<ThresholdViewModel>();
            bool? result = await this._windowManager.ShowDialogAsync(viewModel);
            if (result == true)
            {
                //初始化模型
                const string modelPath = "Content/Models/yolo11n.onnx";
                using YoloDetector yoloDetector = await Task.Run(() => new YoloDetector(modelPath));
                await Task.Run(() => yoloDetector.StartSession());

                //执行检测
                using Mat originalImage = this.SelectedImageAnnotation.Image.ToMat();
                using Mat image = originalImage.Channels() == 4
                    ? originalImage.CvtColor(ColorConversionCodes.BGRA2BGR)
                    : originalImage.Channels() == 1
                        ? originalImage.CvtColor(ColorConversionCodes.GRAY2BGR)
                        : originalImage;
                Detection[] detections = await Task.Run(() => yoloDetector.Infer(image, (float)viewModel.Threshold));

                //增加标注
                foreach (Detection detection in detections)
                {
                    RectangleVisual2D rectangle = new RectangleVisual2D()
                    {
                        Location = new Point(detection.Box.X, detection.Box.Y),
                        Size = new Size(detection.Box.Width, detection.Box.Height),
                        Fill = new SolidColorBrush(Colors.Transparent),
                        Stroke = new SolidColorBrush(this.BorderColor),
                        StrokeThickness = this.BorderThickness
                    };
                    rectangle.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                    RectangleL rectangleL = new RectangleL(detection.Box.X, detection.Box.Y, detection.Box.Width, detection.Box.Height);
                    rectangle.Tag = rectangleL;
                    rectangleL.Tag = rectangle;

                    Annotation annotation = new Annotation(detection.Label, null, false, false, rectangleL, string.Empty);

                    this.SelectedImageAnnotation.Shapes.Add(annotation.Shape);
                    this.SelectedImageAnnotation.ShapeLs.Add(annotation.ShapeL);
                    this.SelectedImageAnnotation.Annotations.Add(annotation);
                    if (!this.Labels.Contains(detection.Label))
                    {
                        this.Labels.Add(detection.Label);
                    }
                }
            }

            this.Idle();
        }
        #endregion

        #region YOLO图像分割 —— async void YoloSegment()
        /// <summary>
        /// YOLO图像分割
        /// </summary>
        public async void YoloSegment()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                MessageBox.Show("当前未加载图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            this.Busy();

            ThresholdViewModel viewModel = ResolveMediator.Resolve<ThresholdViewModel>();
            bool? result = await this._windowManager.ShowDialogAsync(viewModel);
            if (result == true)
            {
                //初始化模型
                const string modelPath = "Content/Models/yolo11n-seg.onnx";
                using YoloSegmenter yoloSegmenter = await Task.Run(() => new YoloSegmenter(modelPath));
                await Task.Run(() => yoloSegmenter.StartSession());

                //执行分割
                using Mat originalImage = this.SelectedImageAnnotation.Image.ToMat();
                using Mat image = originalImage.Channels() == 4
                    ? originalImage.CvtColor(ColorConversionCodes.BGRA2BGR)
                    : originalImage.Channels() == 1
                        ? originalImage.CvtColor(ColorConversionCodes.GRAY2BGR)
                        : originalImage;
                Segmentation[] segmentations = await Task.Run(() => yoloSegmenter.Infer(image, (float)viewModel.Threshold));

                //增加标注
                foreach (Segmentation segmentation in segmentations)
                {
                    IList<Point> points = new List<Point>();
                    IList<PointL> pointLs = new List<PointL>();
                    for (int index = 0; index < segmentation.Contour.Length; index += 2)
                    {
                        OpenCvSharp.Point point2F = segmentation.Contour[index];
                        Point point = new Point(point2F.X, point2F.Y);
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
                    polygon.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                    PolygonL polygonL = new PolygonL(pointLs);
                    polygon.Tag = polygonL;
                    polygonL.Tag = polygon;

                    Annotation annotation = new Annotation(segmentation.Label, null, false, false, polygonL, string.Empty);

                    this.SelectedImageAnnotation.Shapes.Add(annotation.Shape);
                    this.SelectedImageAnnotation.ShapeLs.Add(annotation.ShapeL);
                    this.SelectedImageAnnotation.Annotations.Add(annotation);
                    if (!this.Labels.Contains(segmentation.Label))
                    {
                        this.Labels.Add(segmentation.Label);
                    }
                }
            }

            this.Idle();
        }
        #endregion

        #region PascalVOC转换CSV —— async void PascalToCsv()
        /// <summary>
        /// PascalVOC转换CSV
        /// </summary>
        public async void PascalToCsv()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "请选择PascalVOC标注文件",
                Filter = "标注文件(*.xml)|*.xml",
                Multiselect = true,
                AddExtension = true,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "(*.csv)|*.csv",
                    AddExtension = true,
                    RestoreDirectory = true
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    this.Busy();

                    IList<string> csvLines = new List<string>();
                    csvLines.Add("image_id,width,height,class,x,y,w,h,source");
                    foreach (string fileName in openFileDialog.FileNames)
                    {
                        string pascalAnnotationXml = await Task.Run(() => File.ReadAllText(fileName));
                        PascalAnnotation pascalAnnotation = pascalAnnotationXml.AsXmlTo<PascalAnnotation>();
                        foreach (PascalAnnotationInfo pascalAnnotationInfo in pascalAnnotation.Annotations)
                        {
                            StringBuilder csvBuilder = new StringBuilder();
                            csvBuilder.Append($"{pascalAnnotation.Filename},");
                            csvBuilder.Append($"{pascalAnnotation.ImageSize.Width},");
                            csvBuilder.Append($"{pascalAnnotation.ImageSize.Height},");
                            csvBuilder.Append($"{pascalAnnotationInfo.Name},");
                            csvBuilder.Append($"{pascalAnnotationInfo.Location.XMin},");
                            csvBuilder.Append($"{pascalAnnotationInfo.Location.YMin},");
                            csvBuilder.Append($"{pascalAnnotationInfo.Location.XMax - pascalAnnotationInfo.Location.XMin},");
                            csvBuilder.Append($"{pascalAnnotationInfo.Location.YMax - pascalAnnotationInfo.Location.YMin},");
                            csvBuilder.Append($"{pascalAnnotation.Source.Database}");
                            csvLines.Add(csvBuilder.ToString());
                        }
                    }

                    await Task.Run(() => File.WriteAllLines(saveFileDialog.FileName, csvLines));

                    this.Idle();
                    this.ToastSuccess("已保存！");
                }
            }
        }
        #endregion

        #region PascalVOC转换YOLO —— async void PascalToYolo()
        /// <summary>
        /// PascalVOC转换YOLO
        /// </summary>
        public async void PascalToYolo()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "请选择PascalVOC标注文件",
                Filter = "标注文件(*.xml)|*.xml",
                Multiselect = true,
                AddExtension = true,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                CommonOpenFileDialog folderDialog = new CommonOpenFileDialog
                {
                    Title = "请选择目标文件夹",
                    IsFolderPicker = true
                };
                if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    this.Busy();

                    foreach (string fileName in openFileDialog.FileNames)
                    {
                        string pascalAnnotationXml = await Task.Run(() => File.ReadAllText(fileName));
                        PascalAnnotation pascalAnnotation = pascalAnnotationXml.AsXmlTo<PascalAnnotation>();
                        IList<Annotation> annotations = pascalAnnotation.FromPascalAnnotation();
                        string[] yoloAnnotations = annotations.ToYoloDetenctions(pascalAnnotation.ImageSize.Width, pascalAnnotation.ImageSize.Height, this.Labels);

                        string yoloFileName = Path.GetFileNameWithoutExtension(fileName);
                        string yoloFilePath = $@"{folderDialog.FileName}\{yoloFileName}.txt";
                        await Task.Run(() => File.WriteAllLines(yoloFilePath, yoloAnnotations));
                    }

                    this.Idle();
                    this.ToastSuccess("已保存！");
                }
            }
        }
        #endregion


        //帮助

        #region 操作手册 —— void Manual()
        /// <summary>
        /// 操作手册
        /// </summary>
        public void Manual()
        {
            StringBuilder docBuilder = new StringBuilder();
            docBuilder.AppendLine("Ctrl + S: 保存");
            docBuilder.AppendLine("←/→: 切换图像");
            docBuilder.AppendLine("鼠标滚轮滚动: 缩放视口");
            docBuilder.AppendLine("鼠标滚轮按下拖动: 移动视口");
            docBuilder.AppendLine("列表功能: 鼠标右键");

            MessageBox.Show(docBuilder.ToString(), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region 技术支持 —— void Support()
        /// <summary>
        /// 技术支持
        /// </summary>
        public void Support()
        {
            Process.Start("https://gitee.com/lishilei0523/LabelSharp");
        }
        #endregion

        #endregion
    }
}
