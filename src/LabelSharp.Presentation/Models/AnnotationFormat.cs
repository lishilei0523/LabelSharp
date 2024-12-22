using System.ComponentModel;
using System.Runtime.Serialization;

namespace LabelSharp.Presentation.Models
{
    /// <summary>
    /// 标注格式
    /// </summary>
    [DataContract]
    public enum AnnotationFormat
    {
        /// <summary>
        /// Pascal VOC
        /// </summary>
        [DataMember]
        [Description("PascalVOC")]
        PascalVoc = 0,

        /// <summary>
        /// COCO
        /// </summary>
        [DataMember]
        [Description("COCO")]
        Coco = 1,

        /// <summary>
        /// COCO
        /// </summary>
        [DataMember]
        [Description("YOLO")]
        Yolo = 2
    }
}
