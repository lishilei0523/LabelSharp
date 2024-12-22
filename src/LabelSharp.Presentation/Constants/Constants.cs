// ReSharper disable once CheckNamespace
namespace LabelSharp
{
    /// <summary>
    /// 常量
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// LabelMe点
        /// </summary>
        public const string MePoint = "point";

        /// <summary>
        /// LabelMe线段
        /// </summary>
        public const string MeLine = "line";

        /// <summary>
        /// LabelMe矩形
        /// </summary>
        public const string MeRectangle = "rectangle";

        /// <summary>
        /// LabelMe圆形
        /// </summary>
        public const string MeCircle = "circle";

        /// <summary>
        /// LabelMe椭圆形
        /// </summary>
        public const string MeEllipse = "ellipse";

        /// <summary>
        /// LabelMe多边形
        /// </summary>
        public const string MePolygon = "polygon";

        /// <summary>
        /// LabelMe折线段
        /// </summary>
        public const string MePolyline = "linestrip";

        /// <summary>
        /// 可用图像格式列表
        /// </summary>
        public static readonly string[] AvailableImageFormats = { ".jpg", ".jpeg", ".png", ".bmp" };
    }
}
