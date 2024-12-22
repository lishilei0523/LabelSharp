using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LabelSharp.Presentation.Models
{
    /// <summary>
    /// LabelMe标注
    /// </summary>
    public class MeAnnotation
    {
        #region # 字段及构造器

        /// <summary>
        /// 无参构造器
        /// </summary>
        public MeAnnotation()
        {
            this.Version = "2.4.4";
            this.Flags = new Dictionary<string, string>();
            this.ImageData = null;
            this.Description = string.Empty;
        }

        /// <summary>
        /// 创建LabelMe标注构造器
        /// </summary>
        /// <param name="imagePath">图像路径</param>
        /// <param name="imageWidth">图像宽度</param>
        /// <param name="imageHeight">图像高度</param>
        /// <param name="shapes">LabelMe形状列表</param>
        public MeAnnotation(string imagePath, int imageWidth, int imageHeight, IList<MeShape> shapes)
            : this()
        {
            this.ImagePath = imagePath;
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
            this.Shapes = shapes;
        }

        #endregion

        #region # 属性

        #region 版本号 —— string Version
        /// <summary>
        /// 版本号
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; }
        #endregion

        #region 标记字典 —— IDictionary<string, string> Flags
        /// <summary>
        /// 标记字典
        /// </summary>
        [JsonPropertyName("flags")]
        public IDictionary<string, string> Flags { get; set; }
        #endregion

        #region 形状列表 —— IList<MeShape> Shapes
        /// <summary>
        /// 形状列表
        /// </summary>
        [JsonPropertyName("shapes")]
        public IList<MeShape> Shapes { get; set; }
        #endregion

        #region 图像路径 —— string ImagePath
        /// <summary>
        /// 图像路径
        /// </summary>
        [JsonPropertyName("imagePath")]
        public string ImagePath { get; set; }
        #endregion

        #region 图像数据 —— string ImageData
        /// <summary>
        /// 图像数据
        /// </summary>
        [JsonPropertyName("imageData")]
        public string ImageData { get; set; }
        #endregion

        #region 图像高度 —— int ImageHeight
        /// <summary>
        /// 图像高度
        /// </summary>
        [JsonPropertyName("imageHeight")]
        public int ImageHeight { get; set; }
        #endregion

        #region 图像宽度 —— int ImageWidth
        /// <summary>
        /// 图像宽度
        /// </summary>
        [JsonPropertyName("imageWidth")]
        public int ImageWidth { get; set; }
        #endregion

        #region 描述 —— string Description
        /// <summary>
        /// 描述
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        #endregion 

        #endregion
    }
}
