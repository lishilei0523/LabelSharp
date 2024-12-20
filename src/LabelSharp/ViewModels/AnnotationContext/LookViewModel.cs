using SD.Infrastructure.Shapes;
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

        #endregion

        #region # 方法

        #region 加载 —— void Load(string label, bool truncated...
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="truncated">是否截断</param>
        /// <param name="difficult">是否困难</param>
        /// <param name="shapeL">形状数据</param>
        public void Load(string label, bool truncated, bool difficult, ShapeL shapeL)
        {
            this.Label = label;
            this.Truncated = truncated;
            this.Difficult = difficult;
            this.ShapeL = shapeL;
        }
        #endregion

        #endregion
    }
}
