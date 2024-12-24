using LabelSharp.Presentation.Models;
using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.CustomControls;
using SD.Infrastructure.WPF.Enums;
using SD.Infrastructure.WPF.Extensions;
using SD.Infrastructure.WPF.Visual2Ds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LabelSharp.ViewModels.HomeContext
{
    /// <summary>
    /// 首页视图模型 - 绘图部分
    /// </summary>
    public partial class IndexViewModel
    {
        #region # 字段及构造器

        /// <summary>
        /// 线段
        /// </summary>
        private Line _line;

        /// <summary>
        /// 画刷
        /// </summary>
        private Polyline _brush;

        /// <summary>
        /// 矩形
        /// </summary>
        private RectangleVisual2D _rectangle;

        /// <summary>
        /// 圆形
        /// </summary>
        private CircleVisual2D _circle;

        /// <summary>
        /// 椭圆形
        /// </summary>
        private EllipseVisual2D _ellipse;

        /// <summary>
        /// 实时锚线
        /// </summary>
        private Line _realAnchorLine;

        /// <summary>
        /// 锚点集
        /// </summary>
        private IList<PointVisual2D> _polyAnchors;

        /// <summary>
        /// 锚线集
        /// </summary>
        private IList<Line> _polyAnchorLines;

        #endregion

        #region # 方法

        //Events

        #region 拖拽元素事件 —— void OnDragElement(CanvasEx canvas)
        /// <summary>
        /// 拖拽元素事件
        /// </summary>
        public void OnDragElement(CanvasEx canvas)
        {
            double leftMargin = canvas.GetRectifiedLeft(canvas.SelectedVisual);
            double topMargin = canvas.GetRectifiedTop(canvas.SelectedVisual);

            if (canvas.SelectedVisual is PointVisual2D point)
            {
                this.RebuildPoint(point, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is Line line)
            {
                this.RebuildLine(line, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is RectangleVisual2D rectangle)
            {
                this.RebuildRectangle(rectangle, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is CircleVisual2D circle)
            {
                this.RebuildCircle(circle, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is EllipseVisual2D ellipse)
            {
                this.RebuildEllipse(ellipse, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is Polygon polygon)
            {
                this.RebuildPolygon(polygon, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is Polyline polyline)
            {
                this.RebuildPolyline(polyline, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is Shape shape)
            {
                Annotation annotation = this.SelectedImageAnnotation.Annotations.SingleOrDefault(x => x.Shape == shape);
                if (annotation != null)
                {
                    annotation.ShapeL = (ShapeL)shape.Tag;
                }
            }
        }
        #endregion

        #region 改变元素尺寸事件 —— void OnResizeElement(CanvasEx canvas)
        /// <summary>
        /// 改变元素尺寸事件
        /// </summary>
        public void OnResizeElement(CanvasEx canvas)
        {
            double leftMargin = canvas.GetRectifiedLeft(canvas.SelectedVisual);
            double topMargin = canvas.GetRectifiedTop(canvas.SelectedVisual);

            if (canvas.SelectedVisual is Line line)
            {
                Vector vectorA = canvas.RectifiedMousePosition!.Value - new Point(line.X1 + leftMargin, line.Y1 + topMargin);
                Vector vectorB = canvas.RectifiedMousePosition!.Value - new Point(line.X2 + leftMargin, line.Y2 + topMargin);
                if (vectorA.Length < vectorB.Length)
                {
                    line.X1 = canvas.RectifiedMousePosition!.Value.X - leftMargin;
                    line.Y1 = canvas.RectifiedMousePosition!.Value.Y - topMargin;
                }
                else
                {
                    line.X2 = canvas.RectifiedMousePosition!.Value.X - leftMargin;
                    line.Y2 = canvas.RectifiedMousePosition!.Value.Y - topMargin;
                }

                this.RebuildLine(line, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is RectangleVisual2D rectangle)
            {
                Point retifiedVertex = new Point(rectangle.Location.X + leftMargin, rectangle.Location.Y + topMargin);
                double width = canvas.RectifiedMousePosition!.Value.X - retifiedVertex.X;
                double height = canvas.RectifiedMousePosition!.Value.Y - retifiedVertex.Y;
                if (width > 0 && height > 0)
                {
                    rectangle.Size = new Size(width, height);
                }

                this.RebuildRectangle(rectangle, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is CircleVisual2D circle)
            {
                Point retifiedCenter = new Point(circle.Center.X + leftMargin, circle.Center.Y + topMargin);
                Vector vector = retifiedCenter - canvas.RectifiedMousePosition!.Value;
                circle.Radius = Math.Abs(vector.Length);

                this.RebuildCircle(circle, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is EllipseVisual2D ellipse)
            {
                Point retifiedCenter = new Point(ellipse.Center.X + leftMargin, ellipse.Center.Y + topMargin);
                ellipse.RadiusX = Math.Abs(retifiedCenter.X - canvas.RectifiedMousePosition!.Value.X);
                ellipse.RadiusY = Math.Abs(retifiedCenter.Y - canvas.RectifiedMousePosition!.Value.Y);

                this.RebuildEllipse(ellipse, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is Polygon polygon)
            {
                double minDistance = int.MaxValue;
                Point? nearestPoint = null;
                foreach (Point point in polygon.Points)
                {
                    Vector vector = canvas.RectifiedMousePosition!.Value - new Point(point.X + leftMargin, point.Y + topMargin);
                    if (vector.Length < minDistance)
                    {
                        minDistance = vector.Length;
                        nearestPoint = point;
                    }
                }
                if (nearestPoint.HasValue)
                {
                    int index = polygon.Points.IndexOf(nearestPoint.Value);
                    polygon.Points.Remove(nearestPoint.Value);
                    Point newPoint = new Point(canvas.RectifiedMousePosition!.Value.X - leftMargin, canvas.RectifiedMousePosition!.Value.Y - topMargin);
                    polygon.Points.Insert(index, newPoint);
                }

                this.RebuildPolygon(polygon, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is Polyline polyline)
            {
                double minDistance = int.MaxValue;
                Point? nearestPoint = null;
                foreach (Point point in polyline.Points)
                {
                    Vector vector = canvas.RectifiedMousePosition!.Value - new Point(point.X + leftMargin, point.Y + topMargin);
                    if (vector.Length < minDistance)
                    {
                        minDistance = vector.Length;
                        nearestPoint = point;
                    }
                }
                if (nearestPoint.HasValue)
                {
                    int index = polyline.Points.IndexOf(nearestPoint.Value);
                    polyline.Points.Remove(nearestPoint.Value);
                    Point newPoint = new Point(canvas.RectifiedMousePosition!.Value.X - leftMargin, canvas.RectifiedMousePosition!.Value.Y - topMargin);
                    polyline.Points.Insert(index, newPoint);
                    polyline.Points = polyline.Points;
                }

                this.RebuildPolyline(polyline, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is Shape shape)
            {
                Annotation annotation = this.SelectedImageAnnotation.Annotations.SingleOrDefault(x => x.Shape == shape);
                if (annotation != null)
                {
                    annotation.ShapeL = (ShapeL)shape.Tag;
                }
            }
        }
        #endregion

        #region 绘制开始事件 —— void OnDraw(CanvasEx canvas)
        /// <summary>
        /// 绘制开始事件
        /// </summary>
        public void OnDraw(CanvasEx canvas)
        {
            if (this.PointChecked)
            {
                this.DrawPoint(canvas);
            }
            if (this.PolygonChecked || this.PolylineChecked)
            {
                if (canvas.SelectedVisual is PointVisual2D element && this._polyAnchors.Any() && element == this._polyAnchors[0])
                {
                    //多边形
                    if (this.PolygonChecked)
                    {
                        this.DrawPolygon(canvas);
                    }
                    //折线段
                    if (this.PolylineChecked)
                    {
                        this.DrawPolyline(canvas);
                    }
                }
                else
                {
                    //锚点
                    this.DrawPolyAnchor(canvas);
                }
            }
        }
        #endregion

        #region 绘制中事件 —— void OnDrawing(CanvasEx canvas)
        /// <summary>
        /// 绘制中事件
        /// </summary>
        public void OnDrawing(CanvasEx canvas)
        {
            #region # 验证

            if (this.SelectedImageAnnotation == null)
            {
                return;
            }

            #endregion

            if (this.LineChecked)
            {
                this.DrawLine(canvas);
            }
            if (this.BrushChecked)
            {
                this.DrawBrush(canvas);
            }
            if (this.RectangleChecked)
            {
                this.DrawRectangle(canvas);
            }
            if (this.CircleChecked)
            {
                this.DrawCircle(canvas);
            }
            if (this.EllipseChecked)
            {
                this.DrawEllipse(canvas);
            }
        }
        #endregion

        #region 绘制完成事件 —— void OnDrawn(CanvasEx canvas)
        /// <summary>
        /// 绘制完成事件
        /// </summary>
        public void OnDrawn(CanvasEx canvas)
        {
            if (this._line != null)
            {
                int x1 = (int)Math.Ceiling(this._line.X1);
                int y1 = (int)Math.Ceiling(this._line.Y1);
                int x2 = (int)Math.Ceiling(this._line.X2);
                int y2 = (int)Math.Ceiling(this._line.Y2);
                LineL lineL = new LineL(new PointL(x1, y1), new PointL(x2, y2));

                this._line.Tag = lineL;
                lineL.Tag = this._line;
                this.SelectedImageAnnotation.ShapeLs.Add(lineL);
                this.SelectedImageAnnotation.Shapes.Add(this._line);
                this.OnDrawCompleted(this._line, lineL);
            }
            if (this._brush != null)
            {
                //构建点集
                IList<PointL> pointIs = new List<PointL>();
                foreach (Point point in this._brush.Points)
                {
                    int x = (int)Math.Ceiling(point.X);
                    int y = (int)Math.Ceiling(point.Y);
                    PointL pointI = new PointL(x, y);
                    pointIs.Add(pointI);
                }

                PolylineL polylineL = new PolylineL(pointIs);
                this._brush.Tag = polylineL;
                polylineL.Tag = this._brush;
                this.SelectedImageAnnotation.ShapeLs.Add(polylineL);
                this.SelectedImageAnnotation.Shapes.Add(this._brush);
                this.OnDrawCompleted(this._brush, polylineL);
            }
            if (this._rectangle != null)
            {
                int x = (int)Math.Ceiling(this._rectangle.Location.X);
                int y = (int)Math.Ceiling(this._rectangle.Location.Y);
                int width = (int)Math.Ceiling(this._rectangle.Size.Width);
                int height = (int)Math.Ceiling(this._rectangle.Size.Height);
                RectangleL rectangleL = new RectangleL(x, y, width, height);

                this._rectangle.Tag = rectangleL;
                rectangleL.Tag = this._rectangle;
                this.SelectedImageAnnotation.ShapeLs.Add(rectangleL);
                this.SelectedImageAnnotation.Shapes.Add(this._rectangle);
                this.OnDrawCompleted(this._rectangle, rectangleL);
            }
            if (this._circle != null)
            {
                int x = (int)Math.Ceiling(this._circle.Center.X);
                int y = (int)Math.Ceiling(this._circle.Center.Y);
                int radius = (int)Math.Ceiling(this._circle.Radius);
                CircleL circleL = new CircleL(x, y, radius);

                this._circle.Tag = circleL;
                circleL.Tag = this._circle;
                this.SelectedImageAnnotation.ShapeLs.Add(circleL);
                this.SelectedImageAnnotation.Shapes.Add(this._circle);
                this.OnDrawCompleted(this._circle, circleL);
            }
            if (this._ellipse != null)
            {
                int x = (int)Math.Ceiling(this._ellipse.Center.X);
                int y = (int)Math.Ceiling(this._ellipse.Center.Y);
                int radiusX = (int)Math.Ceiling(this._ellipse.RadiusX);
                int radiusY = (int)Math.Ceiling(this._ellipse.RadiusY);
                EllipseL ellipseL = new EllipseL(x, y, radiusX, radiusY);

                this._ellipse.Tag = ellipseL;
                ellipseL.Tag = this._ellipse;
                this.SelectedImageAnnotation.ShapeLs.Add(ellipseL);
                this.SelectedImageAnnotation.Shapes.Add(this._ellipse);
                this.OnDrawCompleted(this._ellipse, ellipseL);
            }

            this._line = null;
            this._brush = null;
            this._rectangle = null;
            this._circle = null;
            this._ellipse = null;
        }
        #endregion

        #region 形状鼠标左击事件 —— void OnShapeMouseLeftDown(object sender...
        /// <summary>
        /// 形状鼠标左击事件
        /// </summary>
        public void OnShapeMouseLeftDown(object sender, MouseButtonEventArgs eventArgs)
        {
            if (this.CanvasMode != CanvasMode.Draw)
            {
                Shape shape = (Shape)sender;
                Annotation annotation = this.SelectedImageAnnotation.Annotations.SingleOrDefault(x => x.Shape == shape);
                if (annotation != null)
                {
                    this.SelectedImageAnnotation.SelectedAnnotation = null;
                    this.SelectedImageAnnotation.SelectedAnnotation = annotation;
                }
            }
        }
        #endregion

        #region 画布鼠标移动事件 —— void OnCanvasMouseMove(CanvasEx canvas...
        /// <summary>
        /// 画布鼠标移动事件
        /// </summary>
        public void OnCanvasMouseMove(CanvasEx canvas, MouseEventArgs eventArgs)
        {
            //十字参考线
            if (this.SelectedImageAnnotation != null)
            {
                Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;
                this.MousePositionX = (int)Math.Ceiling(rectifiedPosition.X);
                this.MousePositionY = (int)Math.Ceiling(rectifiedPosition.Y);

                //参考线坐标调整
                BitmapSource currentImage = this.SelectedImageAnnotation.Image;
                this.HorizontalLineY = rectifiedPosition.Y > currentImage.Height
                    ? currentImage.Height
                    : rectifiedPosition.Y < 0 ? 0 : rectifiedPosition.Y;
                this.VerticalLineX = rectifiedPosition.X > currentImage.Width
                    ? currentImage.Width
                    : rectifiedPosition.X < 0 ? 0 : rectifiedPosition.X;
                if (!canvas.ScaledRatio.Equals(0))
                {
                    this.GuideLineThickness = 2 / canvas.ScaledRatio;
                }

                //设置光标
                Mouse.OverrideCursor = Cursors.Cross;
            }
            //实时锚线
            if ((this.PolygonChecked || this.PolylineChecked) && this._polyAnchors.Any())
            {
                if (this._realAnchorLine == null)
                {
                    this._realAnchorLine = new Line
                    {
                        Fill = new SolidColorBrush(Colors.Transparent),
                        Stroke = new SolidColorBrush(this.BorderColor),
                        StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                        RenderTransform = canvas.MatrixTransform
                    };
                    canvas.Children.Add(this._realAnchorLine);
                }
                PointVisual2D startPoint = this._polyAnchors.Last();
                Point endPoint = canvas.RectifiedMousePosition!.Value;
                this._realAnchorLine.X1 = startPoint.X;
                this._realAnchorLine.Y1 = startPoint.Y;
                this._realAnchorLine.X2 = endPoint.X;
                this._realAnchorLine.Y2 = endPoint.Y;
            }
        }
        #endregion

        #region 画布鼠标滚轮事件 —— void OnCanvasMouseWheel(CanvasEx canvas)
        /// <summary>
        /// 画布鼠标滚轮事件
        /// </summary>
        public void OnCanvasMouseWheel(CanvasEx canvas)
        {
            //参考线粗细调整
            if (!canvas.ScaledRatio.Equals(0))
            {
                this.GuideLineThickness = Constants.GuideLineThickness / canvas.ScaledRatio;
            }
            //图形边框粗细调整
            if (this.SelectedImageAnnotation != null)
            {
                foreach (Shape shape in canvas.Children.OfType<Shape>())
                {
                    shape.StrokeThickness = this.BorderThickness / canvas.ScaledRatio;
                }
                foreach (PointVisual2D pointVisual2D in canvas.Children.OfType<PointVisual2D>())
                {
                    pointVisual2D.Thickness = Constants.PointThickness / canvas.ScaledRatio;
                }
            }
        }
        #endregion

        #region 画布鼠标离开事件 —— void OnCanvasMouseLeave()
        /// <summary>
        /// 画布鼠标离开事件
        /// </summary>
        public void OnCanvasMouseLeave()
        {
            //设置光标
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        #endregion

        #region 画布鼠标左键松开事件 —— void OnCanvasMouseLeftUp()
        /// <summary>
        /// 画布鼠标左键松开事件
        /// </summary>
        public void OnCanvasMouseLeftUp()
        {
            if ((this.CanvasMode == CanvasMode.Drag || this.CanvasMode == CanvasMode.Resize))
            {
                this.SaveAnnotations();
            }
        }
        #endregion


        //Private

        #region 重建点 —— void RebuildPoint(PointVisual2D point, double leftMargin, double topMargin)
        /// <summary>
        /// 重建点
        /// </summary>
        /// <param name="point">点</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildPoint(PointVisual2D point, double leftMargin, double topMargin)
        {
            PointL pointL = (PointL)point.Tag;
            int index = this.SelectedImageAnnotation.ShapeLs.IndexOf(pointL);
            if (index != -1)
            {
                this.SelectedImageAnnotation.ShapeLs.Remove(pointL);

                int x = (int)Math.Ceiling(point.X + leftMargin);
                int y = (int)Math.Ceiling(point.Y + topMargin);
                PointL newPointL = new PointL(x, y);

                point.Tag = newPointL;
                newPointL.Tag = point;
                this.SelectedImageAnnotation.ShapeLs.Insert(index, newPointL);
            }
        }
        #endregion

        #region 重建线段 —— void RebuildLine(Line line, double leftMargin, double topMargin)
        /// <summary>
        /// 重建线段
        /// </summary>
        /// <param name="line">线段</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildLine(Line line, double leftMargin, double topMargin)
        {
            LineL lineL = (LineL)line.Tag;
            int index = this.SelectedImageAnnotation.ShapeLs.IndexOf(lineL);
            if (index != -1)
            {
                this.SelectedImageAnnotation.ShapeLs.Remove(lineL);

                int x1 = (int)Math.Ceiling(line.X1 + leftMargin);
                int y1 = (int)Math.Ceiling(line.Y1 + topMargin);
                int x2 = (int)Math.Ceiling(line.X2 + leftMargin);
                int y2 = (int)Math.Ceiling(line.Y2 + topMargin);
                LineL newLineL = new LineL(new PointL(x1, y1), new PointL(x2, y2));

                line.Tag = newLineL;
                newLineL.Tag = line;
                this.SelectedImageAnnotation.ShapeLs.Insert(index, newLineL);
            }
        }
        #endregion

        #region 重建矩形 —— void RebuildRectangle(RectangleVisual2D rectangle, double leftMargin, double topMargin)
        /// <summary>
        /// 重建矩形
        /// </summary>
        /// <param name="rectangle">矩形</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildRectangle(RectangleVisual2D rectangle, double leftMargin, double topMargin)
        {
            RectangleL rectangleL = (RectangleL)rectangle.Tag;
            int index = this.SelectedImageAnnotation.ShapeLs.IndexOf(rectangleL);
            if (index != -1)
            {
                this.SelectedImageAnnotation.ShapeLs.Remove(rectangleL);

                int x = (int)Math.Ceiling(rectangle.Location.X + leftMargin);
                int y = (int)Math.Ceiling(rectangle.Location.Y + topMargin);
                int width = (int)Math.Ceiling(rectangle.Size.Width);
                int height = (int)Math.Ceiling(rectangle.Size.Height);
                RectangleL newRectangleL = new RectangleL(x, y, width, height);

                rectangle.Tag = newRectangleL;
                newRectangleL.Tag = rectangle;
                this.SelectedImageAnnotation.ShapeLs.Insert(index, newRectangleL);
            }
        }
        #endregion

        #region 重建圆形 —— void RebuildCircle(CircleVisual2D circle, double leftMargin, double topMargin)
        /// <summary>
        /// 重建圆形
        /// </summary>
        /// <param name="circle">圆形</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildCircle(CircleVisual2D circle, double leftMargin, double topMargin)
        {
            CircleL circleL = (CircleL)circle.Tag;
            int index = this.SelectedImageAnnotation.ShapeLs.IndexOf(circleL);
            if (index != -1)
            {
                this.SelectedImageAnnotation.ShapeLs.Remove(circleL);

                int x = (int)Math.Ceiling(circle.Center.X + leftMargin);
                int y = (int)Math.Ceiling(circle.Center.Y + topMargin);
                int radius = (int)Math.Ceiling(circle.Radius);
                CircleL newCircleL = new CircleL(x, y, radius);

                circle.Tag = newCircleL;
                newCircleL.Tag = circle;
                this.SelectedImageAnnotation.ShapeLs.Insert(index, newCircleL);
            }
        }
        #endregion

        #region 重建椭圆形 —— void RebuildEllipse(EllipseVisual2D ellipse, double leftMargin, double topMargin)
        /// <summary>
        /// 重建椭圆形
        /// </summary>
        /// <param name="ellipse">椭圆形</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildEllipse(EllipseVisual2D ellipse, double leftMargin, double topMargin)
        {
            EllipseL ellipseL = (EllipseL)ellipse.Tag;
            int index = this.SelectedImageAnnotation.ShapeLs.IndexOf(ellipseL);
            if (index != -1)
            {
                this.SelectedImageAnnotation.ShapeLs.Remove(ellipseL);

                int x = (int)Math.Ceiling(ellipse.Center.X + leftMargin);
                int y = (int)Math.Ceiling(ellipse.Center.Y + topMargin);
                int radiusX = (int)Math.Ceiling(ellipse.RadiusX);
                int radiusY = (int)Math.Ceiling(ellipse.RadiusY);
                EllipseL newEllipseL = new EllipseL(x, y, radiusX, radiusY);

                ellipse.Tag = newEllipseL;
                newEllipseL.Tag = ellipse;
                this.SelectedImageAnnotation.ShapeLs.Insert(index, newEllipseL);
            }
        }
        #endregion

        #region 重建多边形 —— void RebuildPolygon(Polygon polygon, double leftMargin, double topMargin)
        /// <summary>
        /// 重建多边形
        /// </summary>
        /// <param name="polygon">多边形</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildPolygon(Polygon polygon, double leftMargin, double topMargin)
        {
            PolygonL polygonL = (PolygonL)polygon.Tag;
            int index = this.SelectedImageAnnotation.ShapeLs.IndexOf(polygonL);
            if (index != -1)
            {
                this.SelectedImageAnnotation.ShapeLs.Remove(polygonL);

                IList<PointL> pointIs = new List<PointL>();
                foreach (Point point in polygon.Points)
                {
                    int x = (int)Math.Ceiling(point.X + leftMargin);
                    int y = (int)Math.Ceiling(point.Y + topMargin);
                    PointL pointI = new PointL(x, y);
                    pointIs.Add(pointI);
                }
                PolygonL newPolygonL = new PolygonL(pointIs);

                polygon.Tag = newPolygonL;
                newPolygonL.Tag = polygon;
                this.SelectedImageAnnotation.ShapeLs.Insert(index, newPolygonL);
            }
        }
        #endregion

        #region 重建折线段 —— void RebuildPolyline(Polyline polyline, double leftMargin, double topMargin)
        /// <summary>
        /// 重建折线段
        /// </summary>
        /// <param name="polyline">折线段</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildPolyline(Polyline polyline, double leftMargin, double topMargin)
        {
            PolylineL polylineL = (PolylineL)polyline.Tag;
            int index = this.SelectedImageAnnotation.ShapeLs.IndexOf(polylineL);
            if (index != -1)
            {
                this.SelectedImageAnnotation.ShapeLs.Remove(polylineL);

                IList<PointL> pointIs = new List<PointL>();
                foreach (Point point in polyline.Points)
                {
                    int x = (int)Math.Ceiling(point.X + leftMargin);
                    int y = (int)Math.Ceiling(point.Y + topMargin);
                    PointL pointI = new PointL(x, y);
                    pointIs.Add(pointI);
                }
                PolylineL newPolylineL = new PolylineL(pointIs);

                polyline.Tag = newPolylineL;
                newPolylineL.Tag = polyline;
                this.SelectedImageAnnotation.ShapeLs.Insert(index, newPolylineL);
            }
        }
        #endregion

        #region 绘制点 —— void DrawPoint(CanvasEx canvas)
        /// <summary>
        /// 绘制点
        /// </summary>
        private void DrawPoint(CanvasEx canvas)
        {
            Point rectifiedVertex = canvas.RectifiedStartPosition!.Value;
            int x = (int)Math.Ceiling(rectifiedVertex.X);
            int y = (int)Math.Ceiling(rectifiedVertex.Y);

            PointL pointL = new PointL(x, y);
            PointVisual2D point = new PointVisual2D
            {
                X = rectifiedVertex.X,
                Y = rectifiedVertex.Y,
                Thickness = Constants.PointThickness / canvas.ScaledRatio,
                Fill = new SolidColorBrush(Colors.Black),
                Stroke = new SolidColorBrush(this.BorderColor),
                StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                RenderTransform = canvas.MatrixTransform,
                Tag = pointL
            };
            canvas.Children.Add(point);

            pointL.Tag = point;
            this.SelectedImageAnnotation.ShapeLs.Add(pointL);
            this.SelectedImageAnnotation.Shapes.Add(point);
            point.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
            this.OnDrawCompleted(point, pointL);
        }
        #endregion

        #region 绘制线段 —— void DrawLine(CanvasEx canvas)
        /// <summary>
        /// 绘制线段
        /// </summary>
        private void DrawLine(CanvasEx canvas)
        {
            if (this._line == null)
            {
                this._line = new Line
                {
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = new SolidColorBrush(this.BorderColor),
                    StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                    RenderTransform = canvas.MatrixTransform
                };
                canvas.Children.Add(this._line);
            }

            Point rectifiedVertex = canvas.RectifiedStartPosition!.Value;
            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;
            this._line.X1 = rectifiedVertex.X;
            this._line.Y1 = rectifiedVertex.Y;
            this._line.X2 = rectifiedPosition.X;
            this._line.Y2 = rectifiedPosition.Y;
            this._line.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
        }
        #endregion

        #region 绘制画刷 —— void DrawBrush(CanvasEx canvas)
        /// <summary>
        /// 绘制画刷
        /// </summary>
        private void DrawBrush(CanvasEx canvas)
        {
            if (this._brush == null)
            {
                this._brush = new Polyline
                {
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = new SolidColorBrush(this.BorderColor),
                    StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                    RenderTransform = canvas.MatrixTransform
                };
                this._brush.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                canvas.Children.Add(this._brush);
            }

            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;
            this._brush.Points.Add(rectifiedPosition);
        }
        #endregion

        #region 绘制矩形 —— void DrawRectangle(CanvasEx canvas)
        /// <summary>
        /// 绘制矩形
        /// </summary>
        private void DrawRectangle(CanvasEx canvas)
        {
            if (this._rectangle == null)
            {
                this._rectangle = new RectangleVisual2D()
                {
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = new SolidColorBrush(this.BorderColor),
                    StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                    RenderTransform = canvas.MatrixTransform
                };
                this._rectangle.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                canvas.Children.Add(this._rectangle);
            }

            Point rectifiedVertex = canvas.RectifiedStartPosition!.Value;
            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;

            int width = (int)Math.Round(Math.Abs(rectifiedPosition.X - rectifiedVertex.X));
            int height = (int)Math.Round(Math.Abs(rectifiedPosition.Y - rectifiedVertex.Y));
            this._rectangle.Location = rectifiedVertex;
            this._rectangle.Size = new Size(width, height);
        }
        #endregion

        #region 绘制圆形 —— void DrawCircle(CanvasEx canvas)
        /// <summary>
        /// 绘制圆形
        /// </summary>
        private void DrawCircle(CanvasEx canvas)
        {
            if (this._circle == null)
            {
                this._circle = new CircleVisual2D
                {
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = new SolidColorBrush(this.BorderColor),
                    StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                    RenderTransform = canvas.MatrixTransform
                };
                this._circle.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                canvas.Children.Add(this._circle);
            }

            Point rectifiedCenter = canvas.RectifiedStartPosition!.Value;
            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;
            Vector vector = Point.Subtract(rectifiedPosition, rectifiedCenter);

            this._circle.Center = rectifiedCenter;
            this._circle.Radius = vector.Length;
        }
        #endregion

        #region 绘制椭圆形 —— void DrawEllipse(CanvasEx canvas)
        /// <summary>
        /// 绘制椭圆形
        /// </summary>
        private void DrawEllipse(CanvasEx canvas)
        {
            if (this._ellipse == null)
            {
                this._ellipse = new EllipseVisual2D()
                {
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = new SolidColorBrush(this.BorderColor),
                    StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                    RenderTransform = canvas.MatrixTransform
                };
                this._ellipse.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                canvas.Children.Add(this._ellipse);
            }

            Point rectifiedCenter = canvas.RectifiedStartPosition!.Value;
            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;

            this._ellipse.Center = rectifiedCenter;
            this._ellipse.RadiusX = Math.Abs(rectifiedPosition.X - rectifiedCenter.X);
            this._ellipse.RadiusY = Math.Abs(rectifiedPosition.Y - rectifiedCenter.Y);
        }
        #endregion

        #region 绘制锚点 —— void DrawPolyAnchor(CanvasEx canvas)
        /// <summary>
        /// 绘制锚点
        /// </summary>
        private void DrawPolyAnchor(CanvasEx canvas)
        {
            Point rectifiedPoint = canvas.RectifiedStartPosition!.Value;
            Brush fillBrush = new SolidColorBrush(Colors.Black);
            Brush borderBrush = this._polyAnchors.Any()
                ? new SolidColorBrush(this.BorderColor)
                : new SolidColorBrush(Colors.Yellow);
            PointVisual2D anchor = new PointVisual2D
            {
                X = rectifiedPoint.X,
                Y = rectifiedPoint.Y,
                Thickness = Constants.PointThickness / canvas.ScaledRatio,
                Fill = fillBrush,
                Stroke = borderBrush,
                StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                RenderTransform = canvas.MatrixTransform
            };

            if (this._polyAnchors.Any())
            {
                PointVisual2D lastAnchor = this._polyAnchors.Last();
                Line polyAnchorLine = new Line
                {
                    X1 = lastAnchor.X,
                    Y1 = lastAnchor.Y,
                    X2 = anchor.X,
                    Y2 = anchor.Y,
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = new SolidColorBrush(this.BorderColor),
                    StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                    RenderTransform = canvas.MatrixTransform
                };
                this._polyAnchorLines.Add(polyAnchorLine);
                canvas.Children.Add(polyAnchorLine);
            }
            else
            {
                Panel.SetZIndex(anchor, short.MaxValue);
            }

            this._polyAnchors.Add(anchor);
            canvas.Children.Add(anchor);
        }
        #endregion

        #region 绘制多边形 —— void DrawPolygon(CanvasEx canvas)
        /// <summary>
        /// 绘制多边形
        /// </summary>
        private void DrawPolygon(CanvasEx canvas)
        {
            //点集排序
            IEnumerable<Point> point2ds =
                from anchor in this._polyAnchors
                let leftMargin = canvas.GetRectifiedLeft(anchor)
                let topMargin = canvas.GetRectifiedTop(anchor)
                select new Point(anchor.X + leftMargin, anchor.Y + topMargin);
            PointCollection points = new PointCollection(point2ds);
            points = points.Sequentialize();

            //构建点集
            IEnumerable<PointL> pointIs =
                from point in points
                let x = (int)Math.Ceiling(point.X)
                let y = (int)Math.Ceiling(point.Y)
                select new PointL(x, y);

            PolygonL polygonL = new PolygonL(pointIs);
            Polygon polygon = new Polygon
            {
                Fill = new SolidColorBrush(Colors.Transparent),
                Stroke = new SolidColorBrush(this.BorderColor),
                StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                Points = points,
                RenderTransform = canvas.MatrixTransform,
                Tag = polygonL
            };

            polygonL.Tag = polygon;
            this.SelectedImageAnnotation.ShapeLs.Add(polygonL);
            this.SelectedImageAnnotation.Shapes.Add(polygon);

            //清空锚点、锚线
            foreach (PointVisual2D anchor in this._polyAnchors)
            {
                canvas.Children.Remove(anchor);
            }
            foreach (Line anchorLine in this._polyAnchorLines)
            {
                canvas.Children.Remove(anchorLine);
            }
            if (this._realAnchorLine != null)
            {
                canvas.Children.Remove(this._realAnchorLine);
                this._realAnchorLine = null;
            }
            this._polyAnchors.Clear();
            this._polyAnchorLines.Clear();

            //事件处理
            polygon.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
            this.OnDrawCompleted(polygon, polygonL);
        }
        #endregion

        #region 绘制折线段 —— void DrawPolyline(CanvasEx canvas)
        /// <summary>
        /// 绘制折线段
        /// </summary>
        private void DrawPolyline(CanvasEx canvas)
        {
            //构建点集
            IEnumerable<Point> point2ds =
                from anchor in this._polyAnchors
                let leftMargin = canvas.GetRectifiedLeft(anchor)
                let topMargin = canvas.GetRectifiedTop(anchor)
                select new Point(anchor.X + leftMargin, anchor.Y + topMargin);
            PointCollection points = new PointCollection(point2ds);
            IEnumerable<PointL> pointIs =
                from point in points
                let x = (int)Math.Ceiling(point.X)
                let y = (int)Math.Ceiling(point.Y)
                select new PointL(x, y);

            PolylineL polylineL = new PolylineL(pointIs);
            Polyline polyline = new Polyline
            {
                Fill = new SolidColorBrush(Colors.Transparent),
                Stroke = new SolidColorBrush(this.BorderColor),
                StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                Points = points,
                RenderTransform = canvas.MatrixTransform,
                Tag = polylineL
            };

            polylineL.Tag = polyline;
            this.SelectedImageAnnotation.ShapeLs.Add(polylineL);
            this.SelectedImageAnnotation.Shapes.Add(polyline);

            //清空锚点、锚线
            foreach (PointVisual2D anchor in this._polyAnchors)
            {
                canvas.Children.Remove(anchor);
            }
            foreach (Line anchorLine in this._polyAnchorLines)
            {
                canvas.Children.Remove(anchorLine);
            }
            if (this._realAnchorLine != null)
            {
                canvas.Children.Remove(this._realAnchorLine);
                this._realAnchorLine = null;
            }
            this._polyAnchors.Clear();
            this._polyAnchorLines.Clear();

            //事件处理
            polyline.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
            this.OnDrawCompleted(polyline, polylineL);
        }
        #endregion

        #endregion
    }
}
