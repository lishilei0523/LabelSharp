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
        /// <param name="shapeL">形状数据</param>
        public Annotation(string label, bool truncated, bool difficult, Shape shape, ShapeL shapeL)
            : this()
        {
            this.Label = label;
            this.Truncated = truncated;
            this.Difficult = difficult;
            this.Shape = shape;
            this.ShapeL = shapeL;
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
        [DependencyProperty]
        public Shape Shape { get; set; }

        /// <summary>
        /// 形状数据
        /// </summary>
        [DependencyProperty]
        public ShapeL ShapeL { get; set; }
    }
}
