using Caliburn.Micro;
using LabelSharp.Models;
using LabelSharp.ViewModels.AnnotationContext;
using LabelSharp.ViewModels.CommonContext;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

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

        #region 当前图像 —— BitmapSource CurrentImage
        /// <summary>
        /// 当前图像
        /// </summary>
        [DependencyProperty]
        public BitmapSource CurrentImage { get; set; }
        #endregion

        #region 当前图像路径 —— string CurrentImagePath
        /// <summary>
        /// 当前图像路径
        /// </summary>
        [DependencyProperty]
        public string CurrentImagePath { get; set; }
        #endregion

        #region 当前图像名称 —— string CurrentImageName
        /// <summary>
        /// 当前图像名称
        /// </summary>
        [DependencyProperty]
        public string CurrentImageName { get; set; }
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

        #region 图像路径列表 —— ObservableCollection<string> ImagePaths
        /// <summary>
        /// 图像路径列表
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<string> ImagePaths { get; set; }
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

        #region 标签列表 —— ObservableCollection<string> Labels
        /// <summary>
        /// 标签列表
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<string> Labels { get; set; }
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
            this.BackgroundColor = new SolidColorBrush(Colors.LightGray);
            this.BorderColor = Colors.Red;
            this.BorderThickness = 2;
            this.ScaleChecked = true;
            this.Shapes = new ObservableCollection<Shape>();
            this.ShapeLs = new ObservableCollection<ShapeL>();
            this.SelectedAnnotationFormat = AnnotationFormat.Yolo;
            this.AnnotationFormats = typeof(AnnotationFormat).GetEnumMembers();
            this.Labels = new ObservableCollection<string>();
            this.Annotations = new ObservableCollection<Annotation>();

            return base.OnInitializeAsync(cancellationToken);
        }
        #endregion


        //常用

        #region 重置 —— async void Reset()
        /// <summary>
        /// 重置
        /// </summary>
        public async void Reset()
        {
            this.Busy();



            this.Idle();
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

        #region 打开文件 —— void OpenImage()
        /// <summary>
        /// 打开文件
        /// </summary>
        public void OpenImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "请选择图像",
                Filter = "(*.jpg)|*.jpg|(*.png)|*.png|(*.bmp)|*.bmp",
                AddExtension = true,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                this.ImageFolder = null;
                this.ImagePaths = new ObservableCollection<string>(new[] { openFileDialog.FileName });
                this.CurrentImage = new BitmapImage(new Uri(openFileDialog.FileName));
                this.CurrentImagePath = openFileDialog.FileName;
                this.CurrentImageName = Path.GetFileName(this.CurrentImagePath);
            }
        }
        #endregion

        #region 打开文件夹 —— void OpenImageFolder()
        /// <summary>
        /// 打开文件夹
        /// </summary>
        public void OpenImageFolder()
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog
            {
                Title = "请选择图像文件夹",
                IsFolderPicker = true
            };
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.ImageFolder = folderDialog.FileName;
                string[] imagePaths = Directory.GetFiles(this.ImageFolder);
                if (!imagePaths.Any())
                {
                    MessageBox.Show("当前文件夹为空！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                this.ImagePaths = new ObservableCollection<string>(imagePaths);
                this.CurrentImage = new BitmapImage(new Uri(this.ImagePaths[0]));
                this.CurrentImagePath = this.ImagePaths[0];
                this.CurrentImageName = Path.GetFileName(this.CurrentImagePath);
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
                this.ImagePaths = new ObservableCollection<string>();
                this.CurrentImage = null;
                this.CurrentImagePath = null;
                this.CurrentImageName = null;
            }
        }
        #endregion


        //Actions

        #region 查看标注信息 —— async void LookAnnotation()
        /// <summary>
        /// 查看标注信息
        /// </summary>
        public async void LookAnnotation()
        {
            if (this.SelectedAnnotation != null)
            {
                LookViewModel viewModel = ResolveMediator.Resolve<LookViewModel>();
                viewModel.Load(this.SelectedAnnotation.Label, this.SelectedAnnotation.Truncated, this.SelectedAnnotation.Difficult, this.SelectedAnnotation.ShapeL);
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
            if (this.SelectedAnnotation != null)
            {
                UpdateViewModel viewModel = ResolveMediator.Resolve<UpdateViewModel>();
                viewModel.Load(this.SelectedAnnotation.Label, this.SelectedAnnotation.Truncated, this.SelectedAnnotation.Difficult, this.Labels);
                bool? result = await this._windowManager.ShowDialogAsync(viewModel);
                if (result == true)
                {
                    this.SelectedAnnotation.Label = viewModel.Label;
                    this.SelectedAnnotation.Truncated = viewModel.Truncated;
                    this.SelectedAnnotation.Difficult = viewModel.Difficult;
                    this.ToastSuccess("修改成功！");
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
            if (this.SelectedAnnotation != null)
            {
                MessageBoxResult result = MessageBox.Show("确定要删除吗？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    CanvasEx canvasEx = (CanvasEx)this.SelectedAnnotation.Shape.Parent;
                    canvasEx.Children.Remove(this.SelectedAnnotation.Shape);
                    this.Shapes.Remove(this.SelectedAnnotation.Shape);
                    this.ShapeLs.Remove(this.SelectedAnnotation.ShapeL);
                    this.Annotations.Remove(this.SelectedAnnotation);
                    this.ToastSuccess("删除成功！");
                }
            }
        }
        #endregion


        //Events

        #region 图像选中事件 —— void OnImageSelect()
        /// <summary>
        /// 图像选中事件
        /// </summary>
        public void OnImageSelect()
        {
            if (!string.IsNullOrWhiteSpace(this.CurrentImagePath))
            {
                this.CurrentImage = new BitmapImage(new Uri(this.CurrentImagePath));
                this.CurrentImageName = Path.GetFileName(this.CurrentImagePath);
            }
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
                Annotation annotation = new Annotation(viewModel.Label, viewModel.Truncated, viewModel.Difficult, shape);
                this.Annotations.Add(annotation);
                if (!this.Labels.Contains(annotation.Label))
                {
                    this.Labels.Add(annotation.Label);
                }
                this.ToastSuccess("创建成功！");
            }
            else
            {
                CanvasEx canvasEx = (CanvasEx)shape.Parent;
                canvasEx.Children.Remove(shape);
                this.Shapes.Remove(shape);
                this.ShapeLs.Remove(shapeL);
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
            if (this.SelectedAnnotation != null)
            {
                Shape shape = this.SelectedAnnotation.Shape;
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

        #region 键盘按下事件 —— void OnKeyDown()
        /// <summary>
        /// 键盘按下事件
        /// </summary>
        public void OnKeyDown()
        {
            if (Keyboard.IsKeyDown(Key.F5))
            {

            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
            {

            }
        }
        #endregion

        #endregion
    }
}
