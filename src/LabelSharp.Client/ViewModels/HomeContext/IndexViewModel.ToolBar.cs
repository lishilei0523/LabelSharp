using SD.Infrastructure.WPF.Caliburn.Aspects;
using SD.Infrastructure.WPF.Enums;

namespace LabelSharp.ViewModels.HomeContext
{
    /// <summary>
    /// 首页视图 - 工具栏部分
    /// </summary>
    public partial class IndexViewModel
    {
        #region # 字段及构造器

        //

        #endregion

        #region # 属性

        #region 操作模式 —— CanvasMode CanvasMode
        /// <summary>
        /// 操作模式
        /// </summary>
        [DependencyProperty]
        public CanvasMode CanvasMode { get; set; }
        #endregion

        #region 选中缩放 —— bool ScaleChecked
        /// <summary>
        /// 选中缩放
        /// </summary>
        [DependencyProperty]
        public bool ScaleChecked { get; set; }
        #endregion

        #region 选中拖拽 —— bool DragChecked
        /// <summary>
        /// 选中拖拽
        /// </summary>
        [DependencyProperty]
        public bool DragChecked { get; set; }
        #endregion

        #region 选中编辑 —— bool ResizeChecked
        /// <summary>
        /// 选中编辑
        /// </summary>
        [DependencyProperty]
        public bool ResizeChecked { get; set; }
        #endregion

        #region 选中点 —— bool PointChecked
        /// <summary>
        /// 选中点
        /// </summary>
        [DependencyProperty]
        public bool PointChecked { get; set; }
        #endregion

        #region 选中线段 —— bool LineChecked
        /// <summary>
        /// 选中线段
        /// </summary>
        [DependencyProperty]
        public bool LineChecked { get; set; }
        #endregion

        #region 选中画刷 —— bool BrushChecked
        /// <summary>
        /// 选中画刷
        /// </summary>
        [DependencyProperty]
        public bool BrushChecked { get; set; }
        #endregion

        #region 选中矩形 —— bool RectangleChecked
        /// <summary>
        /// 选中矩形
        /// </summary>
        [DependencyProperty]
        public bool RectangleChecked { get; set; }
        #endregion

        #region 选中圆形 —— bool CircleChecked
        /// <summary>
        /// 选中圆形
        /// </summary>
        [DependencyProperty]
        public bool CircleChecked { get; set; }
        #endregion

        #region 选中椭圆形 —— bool EllipseChecked
        /// <summary>
        /// 选中椭圆形
        /// </summary>
        [DependencyProperty]
        public bool EllipseChecked { get; set; }
        #endregion

        #region 选中多边形 —— bool PolygonChecked
        /// <summary>
        /// 选中多边形
        /// </summary>
        [DependencyProperty]
        public bool PolygonChecked { get; set; }
        #endregion

        #region 选中折线段 —— bool PolylineChecked
        /// <summary>
        /// 选中折线段
        /// </summary>
        [DependencyProperty]
        public bool PolylineChecked { get; set; }
        #endregion

        #endregion

        #region # 方法

        //Events

        #region 缩放点击事件 —— void OnScaleClick()
        /// <summary>
        /// 缩放点击事件
        /// </summary>
        public void OnScaleClick()
        {
            if (this.ScaleChecked)
            {
                this.CanvasMode = CanvasMode.Scale;

                this.DragChecked = false;
                this.ResizeChecked = false;
                this.PointChecked = false;
                this.LineChecked = false;
                this.BrushChecked = false;
                this.RectangleChecked = false;
                this.CircleChecked = false;
                this.EllipseChecked = false;
                this.PolygonChecked = false;
                this.PolylineChecked = false;
            }
        }
        #endregion

        #region 拖拽点击事件 —— void OnDragClick()
        /// <summary>
        /// 拖拽点击事件
        /// </summary>
        public void OnDragClick()
        {
            if (this.DragChecked)
            {
                this.CanvasMode = CanvasMode.Drag;

                this.ScaleChecked = false;
                this.ResizeChecked = false;
                this.PointChecked = false;
                this.LineChecked = false;
                this.BrushChecked = false;
                this.RectangleChecked = false;
                this.CircleChecked = false;
                this.EllipseChecked = false;
                this.PolygonChecked = false;
                this.PolylineChecked = false;
            }
        }
        #endregion

        #region 编辑点击事件 —— void OnResizeClick()
        /// <summary>
        /// 编辑点击事件
        /// </summary>
        public void OnResizeClick()
        {
            if (this.ResizeChecked)
            {
                this.CanvasMode = CanvasMode.Resize;

                this.ScaleChecked = false;
                this.DragChecked = false;
                this.PointChecked = false;
                this.LineChecked = false;
                this.BrushChecked = false;
                this.RectangleChecked = false;
                this.CircleChecked = false;
                this.EllipseChecked = false;
                this.PolygonChecked = false;
                this.PolylineChecked = false;
            }
        }
        #endregion

        #region 点点击事件 —— void OnPointClick()
        /// <summary>
        /// 点点击事件
        /// </summary>
        public void OnPointClick()
        {
            if (this.PointChecked)
            {
                this.CanvasMode = CanvasMode.Draw;

                this.ScaleChecked = false;
                this.DragChecked = false;
                this.ResizeChecked = false;
                this.LineChecked = false;
                this.BrushChecked = false;
                this.RectangleChecked = false;
                this.CircleChecked = false;
                this.EllipseChecked = false;
                this.PolygonChecked = false;
                this.PolylineChecked = false;
            }
        }
        #endregion

        #region 线段点击事件 —— void OnLineClick()
        /// <summary>
        /// 线段点击事件
        /// </summary>
        public void OnLineClick()
        {
            if (this.LineChecked)
            {
                this.CanvasMode = CanvasMode.Draw;

                this.ScaleChecked = false;
                this.DragChecked = false;
                this.ResizeChecked = false;
                this.PointChecked = false;
                this.BrushChecked = false;
                this.RectangleChecked = false;
                this.CircleChecked = false;
                this.EllipseChecked = false;
                this.PolygonChecked = false;
                this.PolylineChecked = false;
            }
        }
        #endregion

        #region 画刷点击事件 —— void OnBrushClick()
        /// <summary>
        /// 画刷点击事件
        /// </summary>
        public void OnBrushClick()
        {
            if (this.BrushChecked)
            {
                this.CanvasMode = CanvasMode.Draw;

                this.ScaleChecked = false;
                this.DragChecked = false;
                this.ResizeChecked = false;
                this.PointChecked = false;
                this.LineChecked = false;
                this.RectangleChecked = false;
                this.CircleChecked = false;
                this.EllipseChecked = false;
                this.PolygonChecked = false;
                this.PolylineChecked = false;
            }
        }
        #endregion

        #region 矩形点击事件 —— void OnRectangleClick()
        /// <summary>
        /// 矩形点击事件
        /// </summary>
        public void OnRectangleClick()
        {
            if (this.RectangleChecked)
            {
                this.CanvasMode = CanvasMode.Draw;

                this.ScaleChecked = false;
                this.DragChecked = false;
                this.ResizeChecked = false;
                this.PointChecked = false;
                this.LineChecked = false;
                this.BrushChecked = false;
                this.CircleChecked = false;
                this.EllipseChecked = false;
                this.PolygonChecked = false;
                this.PolylineChecked = false;
            }
        }
        #endregion

        #region 圆形点击事件 —— void OnCircleClick()
        /// <summary>
        /// 圆形点击事件
        /// </summary>
        public void OnCircleClick()
        {
            if (this.CircleChecked)
            {
                this.CanvasMode = CanvasMode.Draw;

                this.ScaleChecked = false;
                this.DragChecked = false;
                this.ResizeChecked = false;
                this.PointChecked = false;
                this.LineChecked = false;
                this.BrushChecked = false;
                this.RectangleChecked = false;
                this.EllipseChecked = false;
                this.PolygonChecked = false;
                this.PolylineChecked = false;
            }
        }
        #endregion

        #region 椭圆形点击事件 —— void OnEllipseClick()
        /// <summary>
        /// 椭圆形点击事件
        /// </summary>
        public void OnEllipseClick()
        {
            if (this.EllipseChecked)
            {
                this.CanvasMode = CanvasMode.Draw;

                this.ScaleChecked = false;
                this.DragChecked = false;
                this.ResizeChecked = false;
                this.PointChecked = false;
                this.LineChecked = false;
                this.BrushChecked = false;
                this.RectangleChecked = false;
                this.CircleChecked = false;
                this.PolygonChecked = false;
                this.PolylineChecked = false;
            }
        }
        #endregion

        #region 多边形点击事件 —— void OnPolygonClick()
        /// <summary>
        /// 多边形点击事件
        /// </summary>
        public void OnPolygonClick()
        {
            if (this.PolygonChecked)
            {
                this.CanvasMode = CanvasMode.Draw;

                this.ScaleChecked = false;
                this.DragChecked = false;
                this.ResizeChecked = false;
                this.PointChecked = false;
                this.LineChecked = false;
                this.BrushChecked = false;
                this.RectangleChecked = false;
                this.CircleChecked = false;
                this.EllipseChecked = false;
                this.PolylineChecked = false;
            }
        }
        #endregion

        #region 折线段点击事件 —— void OnPolylineClick()
        /// <summary>
        /// 折线段点击事件
        /// </summary>
        public void OnPolylineClick()
        {
            if (this.PolylineChecked)
            {
                this.CanvasMode = CanvasMode.Draw;

                this.ScaleChecked = false;
                this.DragChecked = false;
                this.ResizeChecked = false;
                this.PointChecked = false;
                this.LineChecked = false;
                this.BrushChecked = false;
                this.RectangleChecked = false;
                this.CircleChecked = false;
                this.EllipseChecked = false;
                this.PolygonChecked = false;
            }
        }
        #endregion

        #endregion
    }
}
