using Caliburn.Micro;
using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Caliburn.Aspects;
using System.Windows.Shapes;

namespace LabelSharp.Presentation.Models
{
    /// <summary>
    /// 标注信息
    /// </summary>
    public class Annotation : PropertyChangedBase
    {
        #region # 字段及构造器

        /// <summary>
        /// 无参构造器
        /// </summary>
        public Annotation()
        {

        }

        /// <summary>
        /// 创建标注信息构造器
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="truncated">是否截断</param>
        /// <param name="difficult">是否困难</param>
        /// <param name="shape">形状</param>
        /// <param name="description">描述</param>
        public Annotation(string label, int? groupId, bool truncated, bool difficult, Shape shape, string description)
            : this()
        {
            this.Label = label;
            this.GroupId = groupId;
            this.Truncated = truncated;
            this.Difficult = difficult;
            this.Shape = shape;
            this.Description = description;
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

        #region 形状 —— Shape Shape
        /// <summary>
        /// 形状
        /// </summary>
        private Shape _shape;

        /// <summary>
        /// 形状
        /// </summary>
        public Shape Shape
        {
            get => this._shape;
            set
            {
                this.Set(ref this._shape, value);
                this.ShapeText = this.ShapeL.Text;
            }
        }
        #endregion

        #region 形状文本 —— string ShapeText
        /// <summary>
        /// 形状文本
        /// </summary>
        [DependencyProperty]
        public string ShapeText { get; set; }
        #endregion

        #region 描述 —— string Description
        /// <summary>
        /// 描述
        /// </summary>
        [DependencyProperty]
        public string Description { get; set; }
        #endregion 

        #region 只读属性 - 形状数据 —— ShapeL ShapeL
        /// <summary>
        /// 只读属性 - 形状数据
        /// </summary>
        public ShapeL ShapeL
        {
            get => (ShapeL)this.Shape.Tag;
        }
        #endregion 

        #endregion
    }
}
