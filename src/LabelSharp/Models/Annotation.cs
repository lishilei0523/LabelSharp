using Caliburn.Micro;
using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Caliburn.Aspects;
using System.Windows.Shapes;

namespace LabelSharp.Models
{
    /// <summary>
    /// 标注信息
    /// </summary>
    public class Annotation : PropertyChangedBase
    {
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
        /// <param name="truncated">是否截断</param>
        /// <param name="difficult">是否困难</param>
        /// <param name="shape">形状</param>
        public Annotation(string label, bool truncated, bool difficult, Shape shape)
            : this()
        {
            this.Label = label;
            this.Truncated = truncated;
            this.Difficult = difficult;
            this.Shape = shape;
        }

        /// <summary>
        /// 标签
        /// </summary>
        [DependencyProperty]
        public string Label { get; set; }

        /// <summary>
        /// 是否截断
        /// </summary>
        [DependencyProperty]
        public bool Truncated { get; set; }

        /// <summary>
        /// 是否困难
        /// </summary>
        [DependencyProperty]
        public bool Difficult { get; set; }

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

        /// <summary>
        /// 形状数据
        /// </summary>
        public ShapeL ShapeL
        {
            get => (ShapeL)this.Shape.Tag;
        }

        /// <summary>
        /// 形状文本
        /// </summary>
        [DependencyProperty]
        public string ShapeText { get; set; }
    }
}
