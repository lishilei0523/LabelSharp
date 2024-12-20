using SD.Infrastructure.WPF.Caliburn.Aspects;
using SD.Infrastructure.WPF.Caliburn.Base;
using System.Collections.ObjectModel;
using System.Windows;

namespace LabelSharp.ViewModels.AnnotationContext
{
    /// <summary>
    /// 标注信息创建视图模型
    /// </summary>
    public class AddViewModel : ScreenBase
    {
        #region # 字段及构造器

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        public AddViewModel()
        {
            //默认值
            this.Truncated = false;
            this.Difficult = false;
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

        #region 是否截断 —— bool Truncated
        /// <summary>
        /// 是否截断
        /// </summary>
        [DependencyProperty]
        public bool Truncated { get; set; }
        #endregion

        #region 是否困难 —— bool Difficult
        /// <summary>
        /// 是否困难
        /// </summary>
        [DependencyProperty]
        public bool Difficult { get; set; }
        #endregion

        #region 标签列表 —— ObservableCollection<string> Labels
        /// <summary>
        /// 标签列表
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<string> Labels { get; set; }
        #endregion

        #endregion

        #region # 方法

        #region 加载 —— void Load(ObservableCollection<string> labels)
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="labels">标签列表</param>
        public void Load(ObservableCollection<string> labels)
        {
            this.Labels = labels;
        }
        #endregion

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
