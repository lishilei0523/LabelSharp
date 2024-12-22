using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LabelSharp.Presentation.Models
{
    /// <summary>
    /// COCO形状
    /// </summary>
    public class CocoShape
    {
        #region # 字段及构造器

        /// <summary>
        /// 无参构造器
        /// </summary>
        public CocoShape()
        {
            this.Score = null;
            this.Flags = new Dictionary<string, string>();
            this.Attributes = new Dictionary<string, string>();
            this.KieLinks = new List<string>();
        }

        /// <summary>
        /// 创建COCO形状构造器
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="difficult">是否困难</param>
        /// <param name="shapeType">形状类型</param>
        /// <param name="description">描述</param>
        /// <param name="points">点集</param>
        public CocoShape(string label, int? groupId, bool difficult, string shapeType, string description, IList<float[]> points)
            : this()
        {
            this.Label = label;
            this.GroupId = groupId;
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

        #region 点集 —— IList<float[]> Points
        /// <summary>
        /// 点集
        /// </summary>
        [JsonPropertyName("points")]
        public IList<float[]> Points { get; set; }
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
