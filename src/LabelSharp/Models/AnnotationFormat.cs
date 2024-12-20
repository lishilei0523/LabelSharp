using System.ComponentModel;

namespace LabelSharp.Models
{
    /// <summary>
    /// 标注格式
    /// </summary>
    public enum AnnotationFormat
    {
        /// <summary>
        /// Pascal VOC
        /// </summary>
        [Description("PascalVOC")]
        PascalVoc = 0,

        /// <summary>
        /// COCO
        /// </summary>
        [Description("COCO")]
        Coco = 1,

        /// <summary>
        /// COCO
        /// </summary>
        [Description("YOLO")]
        Yolo = 2
    }
}
