using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Caliburn.Aspects;
using SD.Infrastructure.WPF.Caliburn.Base;

namespace LabelSharp.ViewModels.AnnotationContext
{
    /// <summary>
    /// 标注信息查看视图模型
    /// </summary>
    public class LookViewModel : ScreenBase
    {
        #region # 字段及构造器

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        public LookViewModel()
        {

        }

        #endregion

        #region # 属性

        #region 标签 —— string Label
        /// <summary>
        /// 标签
        /// </summary>
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
        public bool Truncated { get; set; }
        #endregion

        #region 是否困难 —— bool Difficult
        /// <summary>
        /// 是否困难
        /// </summary>
        public bool Difficult { get; set; }
        #endregion

        #region 形状数据 —— ShapeL ShapeL
        /// <summary>
        /// 形状数据
        /// </summary>
        public ShapeL ShapeL { get; set; }
        #endregion

        #region 描述 —— string Description
        /// <summary>
        /// 描述
        /// </summary>
        [DependencyProperty]
        public string Description { get; set; }
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
        /// <param name="shapeL">形状数据</param>
        /// <param name="description">描述</param>
        public void Load(string label, int? groupId, bool truncated, bool difficult, ShapeL shapeL, string description)
        {
            this.Label = label;
            this.GroupId = groupId;
            this.Truncated = truncated;
            this.Difficult = difficult;
            this.ShapeL = shapeL;
            this.Description = description;
        }
        #endregion

        #endregion
    }
}
