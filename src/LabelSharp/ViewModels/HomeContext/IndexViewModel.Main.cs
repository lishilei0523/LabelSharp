using Caliburn.Micro;
using LabelSharp.Models;
using LabelSharp.ViewModels.AnnotationContext;
using LabelSharp.ViewModels.CommonContext;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using OpenCvSharp;
using SD.Common;
using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Caliburn.Aspects;
using SD.Infrastructure.WPF.Caliburn.Base;
using SD.Infrastructure.WPF.CustomControls;
using SD.Infrastructure.WPF.Extensions;
using SD.Infrastructure.WPF.Visual2Ds;
using SD.IOC.Core.Mediators;
using SourceChord.FluentWPF.Animations;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Annotation = LabelSharp.Models.Annotation;
using Path = System.IO.Path;
using Point = System.Windows.Point;
using Rect = OpenCvSharp.Rect;
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
        /// 可用图像格式列表
        /// </summary>
        private static readonly string[] _AvailableImageFormats = { ".jpg", ".jpeg", ".png", ".bmp" };

        /// <summary>
        /// 标注格式扩展名字典
        /// </summary>
        private static readonly IDictionary<AnnotationFormat, string> _AnnotationExtensions =
            new Dictionary<AnnotationFormat, string>
            {
                { AnnotationFormat.PascalVoc, "(*.xml)|*.xml" },
                { AnnotationFormat.Coco, "(*.json)|*.json" },
                { AnnotationFormat.Yolo, "(*.txt)|*.txt" },
            };

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

        #region 边框颜色 —— Color? BorderColor
        /// <summary>
        /// 边框颜色
        /// </summary>
        [DependencyProperty]
        public Color? BorderColor { get; set; }
        #endregion

        #region 边框粗细 —— int? BorderThickness
        /// <summary>
        /// 边框粗细
        /// </summary>
        [DependencyProperty]
        public int? BorderThickness { get; set; }
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

        #region 已选图像标注 —— ImageAnnotation SelectedImageAnnotation
        /// <summary>
        /// 已选图像标注
        /// </summary>
        [DependencyProperty]
        public ImageAnnotation SelectedImageAnnotation { get; set; }
        #endregion

        #region 图像标注列表 —— ObservableCollection<ImageAnnotation> ImageAnnotations
        /// <summary>
        /// 图像标注列表
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<ImageAnnotation> ImageAnnotations { get; set; }
        #endregion

        #region 标签列表 —— ObservableCollection<string> Labels
        /// <summary>
        /// 标签列表
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<string> Labels { get; set; }
        #endregion

        #region 已选标注格式 —— AnnotationFormat SelectedAnnotationFormat
        /// <summary>
        /// 已选标注格式
        /// </summary>
        [DependencyProperty]
        public AnnotationFormat SelectedAnnotationFormat { get; set; }
        #endregion

        #region 标注格式字典 —— IDictionary<string, string> AnnotationFormats
        /// <summary>
        /// 标注格式字典
        /// </summary>
        [DependencyProperty]
        public IDictionary<string, string> AnnotationFormats { get; set; }
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
            this.ShowGuideLines = true;
            this.GuideLinesVisibility = Visibility.Visible;
            this.ScaleChecked = true;
            this.SelectedAnnotationFormat = AnnotationFormat.Yolo;
            this.AnnotationFormats = typeof(AnnotationFormat).GetEnumMembers();
            this.Labels = new ObservableCollection<string>();

            return base.OnInitializeAsync(cancellationToken);
        }
        #endregion


        //常用

        #region 重置 —— void Reset()
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            MessageBoxResult result = MessageBox.Show("确定要重置吗？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                this.ClearAnnotations();
            }
        }
        #endregion

        #region 切换参考线 —— void SwitchGuideLinesVisibility()
        /// <summary>
        /// 切换参考线
        /// </summary>
        public void SwitchGuideLinesVisibility()
        {
            this.GuideLinesVisibility = this.ShowGuideLines ? Visibility.Visible : Visibility.Collapsed;
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

        #region 技术支持 —— void Support()
        /// <summary>
        /// 技术支持
        /// </summary>
        public void Support()
        {
            Process.Start("https://gitee.com/lishilei0523/LabelSharp");
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
                ImageAnnotation imageAnnotation = new ImageAnnotation(imagePath, imageName, 1);
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
                    if (_AvailableImageFormats.Contains(fileExtension))
                    {
                        string imageName = Path.GetFileName(imagePath);
                        ImageAnnotation imageAnnotation = new ImageAnnotation(imagePath, imageName, sort);
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
            MessageBoxResult result = MessageBox.Show("确定要关闭吗？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                this.ImageFolder = null;
                this.SelectedImageAnnotation = null;
                this.ImageAnnotations = new ObservableCollection<ImageAnnotation>();
            }
        }
        #endregion

        #region 保存 —— async void Save()
        /// <summary>
        /// 保存
        /// </summary>
        public async void Save()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                MessageBox.Show("当前未加载图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            this.Busy();

            //保存YOLO格式
            if (this.SelectedAnnotationFormat == AnnotationFormat.Yolo)
            {
                string annotationName = Path.GetFileNameWithoutExtension(this.SelectedImageAnnotation.ImagePath);
                await this.SaveYolo($"{this.ImageFolder}/{annotationName}.txt");
            }

            //保存标签
            string labelsPath = $"{this.ImageFolder}/classes.txt";
            await this.SaveLabels(labelsPath);

            this.Idle();
            this.ToastSuccess("已保存！");
        }
        #endregion

        #region 另存为 —— async void SaveAs()
        /// <summary>
        /// 另存为
        /// </summary>
        public async void SaveAs()
        {
            //TODO 实现
        }
        #endregion


        //Actions

        #region 查看标注信息 —— async void LookAnnotation()
        /// <summary>
        /// 查看标注信息
        /// </summary>
        public async void LookAnnotation()
        {
            Annotation annotation = this.SelectedImageAnnotation.SelectedAnnotation;
            if (annotation != null)
            {
                LookViewModel viewModel = ResolveMediator.Resolve<LookViewModel>();
                viewModel.Load(annotation.Label.Trim(), annotation.Truncated, annotation.Difficult, annotation.ShapeL);
                await this._windowManager.ShowDialogAsync(viewModel);
            }
        }
        #endregion

        #region 修改标注信息 —— async void UpdateAnnotation()
        /// <summary>
        /// 修改标注信息
        /// </summary>
        public async void UpdateAnnotation()
        {
            Annotation annotation = this.SelectedImageAnnotation.SelectedAnnotation;
            if (annotation != null)
            {
                UpdateViewModel viewModel = ResolveMediator.Resolve<UpdateViewModel>();
                viewModel.Load(annotation.Label, annotation.Truncated, annotation.Difficult, this.Labels);
                bool? result = await this._windowManager.ShowDialogAsync(viewModel);
                if (result == true)
                {
                    annotation.Label = viewModel.Label.Trim();
                    annotation.Truncated = viewModel.Truncated;
                    annotation.Difficult = viewModel.Difficult;
                    if (!this.Labels.Contains(viewModel.Label.Trim()))
                    {
                        this.Labels.Add(viewModel.Label.Trim());
                    }

                    this.Save();
                }
            }
        }
        #endregion

        #region 删除标注信息 —— void RemoveAnnotation()
        /// <summary>
        /// 删除标注信息
        /// </summary>
        public void RemoveAnnotation()
        {
            Annotation annotation = this.SelectedImageAnnotation.SelectedAnnotation;
            if (annotation != null)
            {
                MessageBoxResult result = MessageBox.Show("确定要删除吗？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    this.SelectedImageAnnotation.Shapes.Remove(annotation.Shape);
                    this.SelectedImageAnnotation.ShapeLs.Remove(annotation.ShapeL);
                    this.SelectedImageAnnotation.Annotations.Remove(annotation);
                    this.Save();
                }
            }
        }
        #endregion

        #region 创建标签 —— async void CreateLabel()
        /// <summary>
        /// 创建标签
        /// </summary>
        public async void CreateLabel()
        {
            LabelViewModel viewModel = ResolveMediator.Resolve<LabelViewModel>();
            bool? result = await this._windowManager.ShowDialogAsync(viewModel);
            if (result == true)
            {
                if (!this.Labels.Contains(viewModel.Label.Trim()))
                {
                    this.Labels.Add(viewModel.Label.Trim());
                    this.Save();
                }
                else
                {
                    this.ToastError("标签已存在！");
                }
            }
        }
        #endregion


        //Events

        #region 图像选中事件 —— async void OnImageSelect()
        /// <summary>
        /// 图像选中事件
        /// </summary>
        public async void OnImageSelect()
        {
            this.ClearAnnotations();
            await this.LoadYolo();
        }
        #endregion

        #region 绘制完成事件 —— async void OnDrawCompleted(Shape shape, ShapeL shapeL)
        /// <summary>
        /// 绘制完成事件
        /// </summary>
        /// <param name="shape">形状</param>
        /// <param name="shapeL">形状数据</param>
        public async void OnDrawCompleted(Shape shape, ShapeL shapeL)
        {
            AddViewModel viewModel = ResolveMediator.Resolve<AddViewModel>();
            viewModel.Load(this.Labels);
            bool? result = await this._windowManager.ShowDialogAsync(viewModel);
            if (result == true)
            {
                Annotation annotation = new Annotation(viewModel.Label.Trim(), viewModel.Truncated, viewModel.Difficult, shape);
                this.SelectedImageAnnotation.Annotations.Add(annotation);
                if (!this.Labels.Contains(annotation.Label.Trim()))
                {
                    this.Labels.Add(annotation.Label.Trim());
                }
                this.Save();
            }
            else
            {
                this.SelectedImageAnnotation.Shapes.Remove(shape);
                this.SelectedImageAnnotation.ShapeLs.Remove(shapeL);
            }

            //设置光标
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        #endregion

        #region 标注信息选中事件 —— void OnAnnotationSelect()
        /// <summary>
        /// 标注信息选中事件
        /// </summary>
        public void OnAnnotationSelect()
        {
            Annotation annotation = this.SelectedImageAnnotation?.SelectedAnnotation;
            if (annotation != null)
            {
                Shape shape = annotation.Shape;
                if (shape.Stroke is SolidColorBrush brush)
                {
                    BrushAnimation brushAnimation = new BrushAnimation
                    {
                        From = new SolidColorBrush(brush.Color.Invert()),
                        To = shape.Stroke,
                        Duration = new Duration(TimeSpan.FromSeconds(2))
                    };
                    Storyboard storyboard = new Storyboard();
                    Storyboard.SetTarget(brushAnimation, shape);
                    Storyboard.SetTargetProperty(brushAnimation, new PropertyPath(Shape.StrokeProperty));
                    storyboard.Children.Add(brushAnimation);
                    storyboard.Begin();
                }
            }
        }
        #endregion

        #region 标注信息勾选事件 —— void OnAnnotationCheck(Annotation annotation)
        /// <summary>
        /// 标注信息勾选事件
        /// </summary>
        public void OnAnnotationCheck(Annotation annotation)
        {
            annotation.Shape.Visibility = Visibility.Visible;
        }
        #endregion

        #region 标注信息取消勾选事件 —— void OnAnnotationUncheck(Annotation annotation)
        /// <summary>
        /// 标注信息取消勾选事件
        /// </summary>
        public void OnAnnotationUncheck(Annotation annotation)
        {
            annotation.Shape.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region 画布鼠标移动事件 —— void OnCanvasMouseMove(CanvasEx canvasEx...
        /// <summary>
        /// 画布鼠标移动事件
        /// </summary>
        public void OnCanvasMouseMove(CanvasEx canvasEx, MouseEventArgs eventArgs)
        {
            if (this.SelectedImageAnnotation != null)
            {
                Point position = eventArgs.GetPosition(canvasEx);
                Point rectifiedPosition = canvasEx.MatrixTransform.Inverse!.Transform(position);
                this.MousePositionX = (int)Math.Ceiling(rectifiedPosition.X);
                this.MousePositionY = (int)Math.Ceiling(rectifiedPosition.Y);

                //参考线坐标调整
                BitmapSource currentImage = this.SelectedImageAnnotation.Image;
                this.HorizontalLineY = rectifiedPosition.Y > currentImage.Height
                    ? currentImage.Height
                    : rectifiedPosition.Y < 0 ? 0 : rectifiedPosition.Y;
                this.VerticalLineX = rectifiedPosition.X > currentImage.Width
                    ? currentImage.Width
                    : rectifiedPosition.X < 0 ? 0 : rectifiedPosition.X;
            }
        }
        #endregion

        #region 键盘按下事件 —— void OnKeyDown()
        /// <summary>
        /// 键盘按下事件
        /// </summary>
        public void OnKeyDown()
        {
            if (Keyboard.IsKeyDown(Key.F5))
            {
                this.Reset();
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
            {
                this.Save();
            }
        }
        #endregion


        //Private

        #region 清空标注信息 —— void ClearAnnotations()
        /// <summary>
        /// 清空标注信息
        /// </summary>
        private void ClearAnnotations()
        {
            if (this.SelectedImageAnnotation != null)
            {
                this.SelectedImageAnnotation.Shapes = new ObservableCollection<Shape>();
                this.SelectedImageAnnotation.ShapeLs = new ObservableCollection<ShapeL>();
                this.SelectedImageAnnotation.Annotations = new ObservableCollection<Annotation>();
                this.SelectedImageAnnotation.SelectedAnnotation = null;
            }
        }
        #endregion

        #region 加载YOLO格式 —— async Task LoadYolo()
        /// <summary>
        /// 加载YOLO格式
        /// </summary>
        public async Task LoadYolo()
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                return;
            }

            #endregion

            string annotationName = Path.GetFileNameWithoutExtension(this.SelectedImageAnnotation.ImagePath);
            string annotationPath = $"{this.ImageFolder}/{annotationName}.txt";

            #region # 验证

            if (!File.Exists(annotationPath))
            {
                return;
            }

            #endregion

            BitmapSource currentImage = this.SelectedImageAnnotation.Image;
            string[] lines = await Task.Run(() => File.ReadAllLines(annotationPath));
            foreach (string line in lines)
            {
                string[] words = line.Split(' ');

                //标签索引
                int labelIndex = int.Parse(words[0]);
                string label = this.Labels.Count > labelIndex ? this.Labels[labelIndex] : labelIndex.ToString();

                //矩形
                if (words.Length == 5)
                {
                    float scaledCenterX = float.Parse(words[1]);
                    float scaledCenterY = float.Parse(words[2]);
                    float scaledWidth = float.Parse(words[3]);
                    float scaledHeight = float.Parse(words[4]);
                    int boxWidth = (int)Math.Ceiling(scaledWidth * currentImage.Width);
                    int boxHeight = (int)Math.Ceiling(scaledHeight * currentImage.Height);
                    int x = (int)Math.Ceiling(scaledCenterX * currentImage.Width - boxWidth / 2.0f);
                    int y = (int)Math.Ceiling(scaledCenterY * currentImage.Height - boxHeight / 2.0f);

                    RectangleVisual2D rectangle = new RectangleVisual2D()
                    {
                        Location = new Point(x, y),
                        Size = new Size(boxWidth, boxHeight),
                        Fill = new SolidColorBrush(Colors.Transparent),
                        Stroke = new SolidColorBrush(this.BorderColor!.Value),
                        StrokeThickness = this.BorderThickness!.Value
                    };
                    RectangleL rectangleL = new RectangleL(x, y, boxWidth, boxHeight);
                    rectangle.Tag = rectangleL;
                    rectangleL.Tag = rectangle;
                    rectangle.MouseLeftButtonDown += this.OnShapeMouseLeftDown;

                    Annotation annotation = new Annotation(label, false, false, rectangle);

                    this.SelectedImageAnnotation.ShapeLs.Add(rectangleL);
                    this.SelectedImageAnnotation.Shapes.Add(rectangle);
                    this.SelectedImageAnnotation.Annotations.Add(annotation);
                }
                //多边形
                if (words.Length > 5)
                {
                    string[] polygonTextArray = new string[words.Length - 5];
                    IList<Point> points = new List<Point>();
                    Array.Copy(words, 5, polygonTextArray, 0, words.Length - 5);
                    IEnumerable<float> polygonArray = polygonTextArray.Select(float.Parse);

                    using Mat mat = Mat.FromArray(polygonArray);
                    using Mat reshapedMat = mat.Reshape(1, polygonTextArray.Length / 2);
                    for (int rowIndex = 0; rowIndex < reshapedMat.Rows; rowIndex++)
                    {
                        float scaledPointX = reshapedMat.At<float>(rowIndex, 0);
                        float scaledPointY = reshapedMat.At<float>(rowIndex, 1);
                        double pointX = scaledPointX * currentImage.Width;
                        double pointY = scaledPointY * currentImage.Height;
                        points.Add(new Point(pointX, pointY));
                    }

                    IEnumerable<PointL> pointIs =
                        from point in points
                        let pointX = (int)Math.Ceiling(point.X)
                        let pointY = (int)Math.Ceiling(point.Y)
                        select new PointL(pointX, pointY);
                    PolygonL polygonL = new PolygonL(pointIs);
                    Polygon polygon = new Polygon
                    {
                        Fill = new SolidColorBrush(Colors.Transparent),
                        Stroke = new SolidColorBrush(this.BorderColor!.Value),
                        StrokeThickness = this.BorderThickness!.Value,
                        Points = new PointCollection(points),
                        Tag = polygonL
                    };
                    polygonL.Tag = polygon;
                    polygon.MouseLeftButtonDown += this.OnShapeMouseLeftDown;

                    Annotation annotation = new Annotation(label, false, false, polygon);

                    this.SelectedImageAnnotation.ShapeLs.Add(polygonL);
                    this.SelectedImageAnnotation.Shapes.Add(polygon);
                    this.SelectedImageAnnotation.Annotations.Add(annotation);
                }
            }
        }
        #endregion

        #region 保存YOLO格式 —— async Task SaveYolo(string fileName)
        /// <summary>
        /// 保存YOLO格式
        /// </summary>
        /// <param name="filePath">文件路径</param>
        private async Task SaveYolo(string filePath)
        {
            BitmapSource currentImage = this.SelectedImageAnnotation.Image;
            string[] lines = new string[this.SelectedImageAnnotation.Annotations.Count];
            for (int index = 0; index < lines.Length; index++)
            {
                StringBuilder lineBuilder = new StringBuilder();
                Annotation annotation = this.SelectedImageAnnotation.Annotations[index];
                int labelIndex = this.Labels.IndexOf(annotation.Label);
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
                if (annotation.ShapeL is PolygonL polygonL)
                {
                    IEnumerable<Point2f> point2Fs = polygonL.Points.Select(point => new Point2f(point.X, point.Y));
                    Rect boundingBox = Cv2.BoundingRect(point2Fs);
                    float scaledCenterX = (boundingBox.X + boundingBox.Width / 2.0f) / (float)currentImage.Width;
                    float scaledCenterY = (boundingBox.Y + boundingBox.Height / 2.0f) / (float)currentImage.Height;
                    float scaledWidth = boundingBox.Width / (float)currentImage.Width;
                    float scaledHeight = boundingBox.Height / (float)currentImage.Height;
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

            await Task.Run(() => File.WriteAllLines(filePath, lines));
        }
        #endregion

        #region 加载标签 —— async Task LoadLabels()
        /// <summary>
        /// 加载标签
        /// </summary>
        public async Task LoadLabels()
        {
            if (!string.IsNullOrWhiteSpace(this.ImageFolder))
            {
                string labelsPath = $"{this.ImageFolder}/classes.txt";
                if (File.Exists(labelsPath))
                {
                    string[] lines = await Task.Run(() => File.ReadAllLines(labelsPath));
                    foreach (string line in lines)
                    {
                        if (!this.Labels.Contains(line))
                        {
                            this.Labels.Add(line);
                        }
                    }
                }
            }
        }
        #endregion

        #region 保存标签 —— async Task SaveLabels(string filePath)
        /// <summary>
        /// 保存标签
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public async Task SaveLabels(string filePath)
        {
            string[] lines = new string[this.Labels.Count];
            for (int index = 0; index < lines.Length; index++)
            {
                lines[index] = this.Labels[index];
            }

            await Task.Run(() => File.WriteAllLines(filePath, lines));
        }
        #endregion

        #endregion
    }
}
