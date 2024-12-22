using SD.Infrastructure.WPF.Caliburn.Aspects;
using SD.Infrastructure.WPF.Caliburn.Base;
using System.Collections.ObjectModel;
using System.Windows;

namespace LabelSharp.ViewModels.AnnotationContext
{
    /// <summary>
    /// 标注信息修改视图模型
    /// </summary>
    public class UpdateViewModel : ScreenBase
    {
        #region # 字段及构造器

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        public UpdateViewModel()
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

        #region 分组Id —— int? GroupId
        /// <summary>
        /// 分组Id
        /// </summary>
        [DependencyProperty]
        public int? GroupId { get; set; }
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

        #region 描述 —— string Description
        /// <summary>
        /// 描述
        /// </summary>
        [DependencyProperty]
        public string Description { get; set; }
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

        #region 加载 —— void Load(string label, bool truncated...
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="truncated">是否截断</param>
        /// <param name="difficult">是否困难</param>
        /// <param name="description">描述</param>
        /// <param name="labels">标签列表</param>
        public void Load(string label, int? groupId, bool truncated, bool difficult, string description, ObservableCollection<string> labels)
        {
            this.Label = label;
            this.GroupId = groupId;
            this.Truncated = truncated;
            this.Difficult = difficult;
            this.Description = description;
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
