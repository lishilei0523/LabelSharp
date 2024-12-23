using SD.Infrastructure.WPF.Caliburn.Aspects;
using SD.Infrastructure.WPF.Caliburn.Base;

namespace LabelSharp.ViewModels.CommonContext
{
    /// <summary>
    /// 阈值视图模型
    /// </summary>
    public class ThresholdViewModel : ScreenBase
    {
        #region # 字段及构造器

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        public ThresholdViewModel()
        {
            this.Threshold = 0.50;
        }

        #endregion

        #region # 属性

        #region 阈值 —— double Threshold
        /// <summary>
        /// 阈值
        /// </summary>
        [DependencyProperty]
        public double Threshold { get; set; }
        #endregion

        #endregion

        #region # 方法

        #region 提交 —— async void Submit()
        /// <summary>
        /// 提交
        /// </summary>
        public async void Submit()
        {
            await base.TryCloseAsync(true);
        }
        #endregion

        #endregion
    }
}
