using SD.Infrastructure.WPF.Caliburn.Aspects;
using SD.Infrastructure.WPF.Caliburn.Base;
using System.Windows;
using System.Windows.Media;

namespace LabelSharp.ViewModels.CommonContext
{
    /// <summary>
    /// 样式视图模型
    /// </summary>
    public class StyleViewModel : ScreenBase
    {
        #region # 字段及构造器

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        public StyleViewModel()
        {
            //默认值
            this.BackgroundColor = Colors.LightGray;
            this.BorderColor = Colors.Red;
            this.BorderThickness = 2;
        }

        #endregion

        #region # 属性

        #region 背景颜色 —— Color? BackgroundColor
        /// <summary>
        /// 背景颜色
        /// </summary>
        [DependencyProperty]
        public Color? BackgroundColor { get; set; }
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

        #endregion

        #region # 方法

        #region 提交 —— async void Submit()
        /// <summary>
        /// 提交
        /// </summary>
        public async void Submit()
        {
            #region # 验证

            if (!this.BackgroundColor.HasValue)
            {
                MessageBox.Show("背景颜色不可为空！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!this.BorderColor.HasValue)
            {
                MessageBox.Show("边框颜色不可为空！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!this.BorderThickness.HasValue)
            {
                MessageBox.Show("边框粗细不可为空！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            await base.TryCloseAsync(true);
        }
        #endregion

        #endregion
    }
}
