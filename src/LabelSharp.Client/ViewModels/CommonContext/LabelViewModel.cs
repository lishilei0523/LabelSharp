using SD.Infrastructure.WPF.Caliburn.Aspects;
using SD.Infrastructure.WPF.Caliburn.Base;
using System.Windows;

namespace LabelSharp.ViewModels.CommonContext
{
    /// <summary>
    /// 标签视图模型
    /// </summary>
    public class LabelViewModel : ScreenBase
    {
        #region # 字段及构造器

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        public LabelViewModel()
        {

        }

        #endregion

        #region # 属性

        #region 标签 —— string Label
        /// <summary>
        /// 标签
        /// </summary>
        [DependencyProperty]
        public string Label { get; set; }
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

            if (string.IsNullOrWhiteSpace(this.Label))
            {
                MessageBox.Show("标签不可为空！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            await base.TryCloseAsync(true);
        }
        #endregion

        #endregion
    }
}
