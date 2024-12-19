using Caliburn.Micro;
using LabelSharp.ViewModels.CommonContext;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using SD.Infrastructure.WPF.Caliburn.Aspects;
using SD.Infrastructure.WPF.Caliburn.Base;
using SD.IOC.Core.Mediators;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LabelSharp.ViewModels.HomeContext
{
    /// <summary>
    /// 首页视图模型
    /// </summary>
    public class IndexViewModel : ScreenBase
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

        #region 背景颜色 —— Brush BackgroundColor
        /// <summary>
        /// 背景颜色
        /// </summary>
        [DependencyProperty]
        public Brush BackgroundColor { get; set; }
        #endregion

        #region 图像路径列表 —— ObservableCollection<string> ImagePaths
        /// <summary>
        /// 图像路径列表
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<string> ImagePaths { get; set; }
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
            this.BackgroundColor = new SolidColorBrush(Colors.LightGray);

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

        #region 设置背景颜色 —— async void SetBackgroundColor()
        /// <summary>
        /// 设置背景颜色
        /// </summary>
        public async void SetBackgroundColor()
        {
            SelectColorViewModel viewModel = ResolveMediator.Resolve<SelectColorViewModel>();
            bool? result = await this._windowManager.ShowDialogAsync(viewModel);
            if (result == true)
            {
                this.BackgroundColor = new SolidColorBrush(viewModel.Color!.Value);
            }
        }
        #endregion

        #region 技术支持 —— void Support()
        /// <summary>
        /// 技术支持
        /// </summary>
        public void Support()
        {
            Process.Start("https://gitee.com/lishilei0523/OpenCV-Studio");
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


        //事件

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
