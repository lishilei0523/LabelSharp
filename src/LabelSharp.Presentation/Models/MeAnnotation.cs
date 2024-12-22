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


    /// <summary>
    /// LabelMe形状
    /// </summary>
    public class MeShape
    {
        #region # 字段及构造器

        /// <summary>
        /// 无参构造器
        /// </summary>
        public MeShape()
        {
            this.Score = null;
            this.Flags = new Dictionary<string, string>();
            this.Attributes = new Dictionary<string, string>();
            this.KieLinks = new List<string>();
        }

        /// <summary>
        /// 创建LabelMe形状构造器
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="truncated">是否截断</param>
        /// <param name="difficult">是否困难</param>
        /// <param name="shapeType">形状类型</param>
        /// <param name="description">描述</param>
        /// <param name="points">点集</param>
        public MeShape(string label, int? groupId, bool truncated, bool difficult, string shapeType, string description, IList<double[]> points)
            : this()
        {
            this.Label = label;
            this.GroupId = groupId;
            this.Truncated = truncated;
            this.Difficult = difficult;
            this.ShapeType = shapeType;
            this.Description = description;
            this.Points = points;
        }

        #endregion

        #region # 属性

        #region 标签 —— string Label
        /// <summary>
        /// 标签
        /// </summary>
        [JsonPropertyName("label")]
        public string Label { get; set; }
        #endregion

        #region 分值 —— float? Score
        /// <summary>
        /// 分值
        /// </summary>
        [JsonPropertyName("score")]
        public float? Score { get; set; }
        #endregion

        #region 点集 —— IList<double[]> Points
        /// <summary>
        /// 点集
        /// </summary>
        [JsonPropertyName("points")]
        public IList<double[]> Points { get; set; }
        #endregion

        #region 分组Id —— int? GroupId
        /// <summary>
        /// 分组Id
        /// </summary>
        [JsonPropertyName("group_id")]
        public int? GroupId { get; set; }
        #endregion

        #region 描述 —— string Description
        /// <summary>
        /// 描述
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        #endregion

        #region 是否截断 —— bool Truncated
        /// <summary>
        /// 是否截断
        /// </summary>
        [JsonPropertyName("truncated")]
        public bool Truncated { get; set; }
        #endregion

        #region 是否困难 —— bool Difficult
        /// <summary>
        /// 是否困难
        /// </summary>
        [JsonPropertyName("difficult")]
        public bool Difficult { get; set; }
        #endregion

        #region 形状类型 —— string ShapeType
        /// <summary>
        /// 形状类型
        /// </summary>
        [JsonPropertyName("shape_type")]
        public string ShapeType { get; set; }
        #endregion

        #region 标记字典 —— IDictionary<string, string> Flags
        /// <summary>
        /// 标记字典
        /// </summary>
        [JsonPropertyName("flags")]
        public IDictionary<string, string> Flags { get; set; }
        #endregion

        #region 特性字典 —— IDictionary<string, string> Attributes
        /// <summary>
        /// 特性字典
        /// </summary>
        [JsonPropertyName("attributes")]
        public IDictionary<string, string> Attributes { get; set; }
        #endregion

        #region 关键信息链接列表 —— IList<string> KieLinks
        /// <summary>
        /// 关键信息链接列表
        /// </summary>
        [JsonPropertyName("kie_linking")]
        public IList<string> KieLinks { get; set; }
        #endregion 

        #endregion
    }
}
