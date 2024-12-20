using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Caliburn.Aspects;
using SD.Infrastructure.WPF.CustomControls;
using SD.Infrastructure.WPF.Enums;
using SD.Infrastructure.WPF.Extensions;
using SD.Infrastructure.WPF.Visual2Ds;
using SourceChord.FluentWPF.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Point = System.Windows.Point;

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
        private Rectangle _rectangle;

        /// <summary>
        /// 圆形
        /// </summary>
        private CircleVisual2D _circle;

        /// <summary>
        /// 椭圆形
        /// </summary>
        private EllipseVisual2D _ellipse;

        /// <summary>
        /// 锚点集
        /// </summary>
        private IList<PointVisual2D> _polyAnchors;

        #endregion

        #region # 属性

        #region 已选形状数据 —— ShapeL SelectedShapeL
        /// <summary>
        /// 已选形状数据
        /// </summary>
        [DependencyProperty]
        public ShapeL SelectedShapeL { get; set; }
        #endregion

        #region 形状集 —— ObservableCollection<Shape> Shapes
        /// <summary>
        /// 形状集
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<Shape> Shapes { get; set; }
        #endregion

        #region 形状数据集 —— ObservableCollection<ShapeL> ShapeLs
        /// <summary>
        /// 形状数据集
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<ShapeL> ShapeLs { get; set; }
        #endregion

        #endregion

        #region # 方法

        //Actions

        #region 复制形状 —— void CopyShape()
        /// <summary>
        /// 复制形状
        /// </summary>
        public void CopyShape()
        {
            if (this.SelectedShapeL != null)
            {
                Clipboard.SetText(this.SelectedShapeL.Text);
                base.ToastSuccess("已复制剪贴板！");
            }
        }
        #endregion

        #region 删除形状 —— void RemoveShape()
        /// <summary>
        /// 删除形状
        /// </summary>
        public void RemoveShape()
        {
            if (this.SelectedShapeL != null)
            {
                Shape shape = (Shape)this.SelectedShapeL.Tag;
                CanvasEx canvas = (CanvasEx)shape.Parent;

                this.ShapeLs.Remove(this.SelectedShapeL);
                this.Shapes.Remove(shape);
                canvas.Children.Remove(shape);
            }
        }
        #endregion


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
            if (canvas.SelectedVisual is Rectangle rectangle)
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

                return;
            }
            if (canvas.SelectedVisual is Rectangle rectangle)
            {
                Point retifiedVertex = new Point(leftMargin, topMargin);
                double width = canvas.RectifiedMousePosition!.Value.X - retifiedVertex.X;
                double height = canvas.RectifiedMousePosition!.Value.Y - retifiedVertex.Y;

                if (width > 0)
                {
                    rectangle.Width = width;
                }
                if (height > 0)
                {
                    rectangle.Height = height;
                }

                this.RebuildRectangle(rectangle, leftMargin, topMargin);

                return;
            }
            if (canvas.SelectedVisual is CircleVisual2D circle)
            {
                Point retifiedCenter = new Point(circle.Center.X + leftMargin, circle.Center.Y + topMargin);
                Vector vector = retifiedCenter - canvas.RectifiedMousePosition!.Value;
                circle.Radius = Math.Abs(vector.Length);

                this.RebuildCircle(circle, leftMargin, topMargin);

                return;
            }
            if (canvas.SelectedVisual is EllipseVisual2D ellipse)
            {
                Point retifiedCenter = new Point(ellipse.Center.X + leftMargin, ellipse.Center.Y + topMargin);
                ellipse.RadiusX = Math.Abs(retifiedCenter.X - canvas.RectifiedMousePosition!.Value.X);
                ellipse.RadiusY = Math.Abs(retifiedCenter.Y - canvas.RectifiedMousePosition!.Value.Y);

                this.RebuildEllipse(ellipse, leftMargin, topMargin);

                return;
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

                return;
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

                return;
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

            if (this.CurrentImage == null)
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
                this.ShapeLs.Add(lineL);
                this.Shapes.Add(this._line);
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
                this.ShapeLs.Add(polylineL);
                this.Shapes.Add(this._brush);
            }
            if (this._rectangle != null)
            {
                int x = (int)Math.Ceiling(canvas.GetRectifiedLeft(this._rectangle));
                int y = (int)Math.Ceiling(canvas.GetRectifiedTop(this._rectangle));
                int width = (int)Math.Ceiling(this._rectangle.Width);
                int height = (int)Math.Ceiling(this._rectangle.Height);
                RectangleL rectangleL = new RectangleL(x, y, width, height);

                this._rectangle.Tag = rectangleL;
                rectangleL.Tag = this._rectangle;
                this.ShapeLs.Add(rectangleL);
                this.Shapes.Add(this._rectangle);
            }
            if (this._circle != null)
            {
                int x = (int)Math.Ceiling(this._circle.Center.X);
                int y = (int)Math.Ceiling(this._circle.Center.Y);
                int radius = (int)Math.Ceiling(this._circle.Radius);
                CircleL circleL = new CircleL(x, y, radius);

                this._circle.Tag = circleL;
                circleL.Tag = this._circle;
                this.ShapeLs.Add(circleL);
                this.Shapes.Add(this._circle);
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
                this.ShapeLs.Add(ellipseL);
                this.Shapes.Add(this._ellipse);
            }

            this._line = null;
            this._brush = null;
            this._rectangle = null;
            this._circle = null;
            this._ellipse = null;
        }
        #endregion

        #region 选中形状事件 —— void OnSelectShape()
        /// <summary>
        /// 选中形状事件
        /// </summary>
        public void OnSelectShape()
        {
            if (this.SelectedShapeL != null)
            {
                Shape shape = (Shape)this.SelectedShapeL.Tag;
                if (shape.Stroke is SolidColorBrush brush)
                {
                    BrushAnimation brushAnimation = new BrushAnimation
                    {
                        From = new SolidColorBrush(brush.Color.Invert()),
                        To = shape.Stroke,
                        Duration = new Duration(TimeSpan.FromSeconds(2))
                    };
                    Storyboard storyboard = new Storyboard();
                    Storyboard.SetTarget(brushAnimation, shape);
                    Storyboard.SetTargetProperty(brushAnimation, new PropertyPath(Shape.StrokeProperty));
                    storyboard.Children.Add(brushAnimation);
                    storyboard.Begin();
                }
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
            int index = this.ShapeLs.IndexOf(pointL);
            if (index != -1)
            {
                this.ShapeLs.Remove(pointL);

                int x = (int)Math.Ceiling(point.X + leftMargin);
                int y = (int)Math.Ceiling(point.Y + topMargin);
                PointL newPointL = new PointL(x, y);

                point.Tag = newPointL;
                newPointL.Tag = point;
                this.ShapeLs.Insert(index, newPointL);
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
            int index = this.ShapeLs.IndexOf(lineL);
            if (index != -1)
            {
                this.ShapeLs.Remove(lineL);

                int x1 = (int)Math.Ceiling(line.X1 + leftMargin);
                int y1 = (int)Math.Ceiling(line.Y1 + topMargin);
                int x2 = (int)Math.Ceiling(line.X2 + leftMargin);
                int y2 = (int)Math.Ceiling(line.Y2 + topMargin);
                LineL newLineL = new LineL(new PointL(x1, y1), new PointL(x2, y2));

                line.Tag = newLineL;
                newLineL.Tag = line;
                this.ShapeLs.Insert(index, newLineL);
            }
        }
        #endregion

        #region 重建矩形 —— void RebuildRectangle(Rectangle rectangle, double leftMargin, double topMargin)
        /// <summary>
        /// 重建矩形
        /// </summary>
        /// <param name="rectangle">矩形</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildRectangle(Rectangle rectangle, double leftMargin, double topMargin)
        {
            RectangleL rectangleL = (RectangleL)rectangle.Tag;
            int index = this.ShapeLs.IndexOf(rectangleL);
            if (index != -1)
            {
                this.ShapeLs.Remove(rectangleL);

                int x = (int)Math.Ceiling(leftMargin);
                int y = (int)Math.Ceiling(topMargin);
                int width = (int)Math.Ceiling(rectangle.Width);
                int height = (int)Math.Ceiling(rectangle.Height);
                RectangleL newRectangleL = new RectangleL(x, y, width, height);

                rectangle.Tag = newRectangleL;
                newRectangleL.Tag = rectangle;
                this.ShapeLs.Insert(index, newRectangleL);
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
            int index = this.ShapeLs.IndexOf(circleL);
            if (index != -1)
            {
                this.ShapeLs.Remove(circleL);

                int x = (int)Math.Ceiling(circle.Center.X + leftMargin);
                int y = (int)Math.Ceiling(circle.Center.Y + topMargin);
                int radius = (int)Math.Ceiling(circle.Radius);
                CircleL newCircleL = new CircleL(x, y, radius);

                circle.Tag = newCircleL;
                newCircleL.Tag = circle;
                this.ShapeLs.Insert(index, newCircleL);
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
            int index = this.ShapeLs.IndexOf(ellipseL);
            if (index != -1)
            {
                this.ShapeLs.Remove(ellipseL);

                int x = (int)Math.Ceiling(ellipse.Center.X + leftMargin);
                int y = (int)Math.Ceiling(ellipse.Center.Y + topMargin);
                int radiusX = (int)Math.Ceiling(ellipse.RadiusX);
                int radiusY = (int)Math.Ceiling(ellipse.RadiusY);
                EllipseL newEllipseL = new EllipseL(x, y, radiusX, radiusY);

                ellipse.Tag = newEllipseL;
                newEllipseL.Tag = ellipse;
                this.ShapeLs.Insert(index, newEllipseL);
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
            int index = this.ShapeLs.IndexOf(polygonL);
            if (index != -1)
            {
                this.ShapeLs.Remove(polygonL);

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
                this.ShapeLs.Insert(index, newPolygonL);
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
            int index = this.ShapeLs.IndexOf(polylineL);
            if (index != -1)
            {
                this.ShapeLs.Remove(polylineL);

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
                this.ShapeLs.Insert(index, newPolylineL);
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
                Fill = new SolidColorBrush(Colors.Black),
                Stroke = new SolidColorBrush(this.BorderColor!.Value),
                RenderTransform = canvas.MatrixTransform,
                Tag = pointL
            };
            canvas.Children.Add(point);

            pointL.Tag = point;
            this.ShapeLs.Add(pointL);
            this.Shapes.Add(point);
            point.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
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
                    Stroke = new SolidColorBrush(this.BorderColor!.Value),
                    StrokeThickness = this.BorderThickness!.Value
                };
                canvas.Children.Add(this._line);
            }

            Point rectifiedVertex = canvas.RectifiedStartPosition!.Value;
            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;
            this._line.X1 = rectifiedVertex.X;
            this._line.Y1 = rectifiedVertex.Y;
            this._line.X2 = rectifiedPosition.X;
            this._line.Y2 = rectifiedPosition.Y;
            this._line.RenderTransform = canvas.MatrixTransform;
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
                    Stroke = new SolidColorBrush(this.BorderColor!.Value),
                    StrokeThickness = this.BorderThickness!.Value
                };
                canvas.Children.Add(this._brush);
            }

            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;
            this._brush.Points.Add(rectifiedPosition);
            this._brush.RenderTransform = canvas.MatrixTransform;
            this._brush.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
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
                this._rectangle = new Rectangle
                {
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = new SolidColorBrush(this.BorderColor!.Value),
                    StrokeThickness = this.BorderThickness!.Value
                };
                canvas.Children.Add(this._rectangle);
            }

            Point rectifiedVertex = canvas.RectifiedStartPosition!.Value;
            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;
            int width = (int)Math.Round(Math.Abs(rectifiedPosition.X - rectifiedVertex.X));
            int height = (int)Math.Round(Math.Abs(rectifiedPosition.Y - rectifiedVertex.Y));
            this._rectangle.Width = width;
            this._rectangle.Height = height;
            if (rectifiedPosition.X > rectifiedVertex.X && rectifiedPosition.Y > rectifiedVertex.Y)
            {
                Canvas.SetLeft(this._rectangle, rectifiedVertex.X * canvas.ScaledRatio);
                Canvas.SetTop(this._rectangle, rectifiedVertex.Y * canvas.ScaledRatio);
            }
            if (rectifiedPosition.X > rectifiedVertex.X && rectifiedPosition.Y < rectifiedVertex.Y)
            {
                Canvas.SetLeft(this._rectangle, rectifiedVertex.X * canvas.ScaledRatio);
                Canvas.SetTop(this._rectangle, rectifiedPosition.Y * canvas.ScaledRatio);
            }
            if (rectifiedPosition.X < rectifiedVertex.X && rectifiedPosition.Y > rectifiedVertex.Y)
            {
                Canvas.SetLeft(this._rectangle, rectifiedPosition.X * canvas.ScaledRatio);
                Canvas.SetTop(this._rectangle, rectifiedVertex.Y * canvas.ScaledRatio);
            }
            if (rectifiedPosition.X < rectifiedVertex.X && rectifiedPosition.Y < rectifiedVertex.Y)
            {
                Canvas.SetLeft(this._rectangle, rectifiedPosition.X * canvas.ScaledRatio);
                Canvas.SetTop(this._rectangle, rectifiedPosition.Y * canvas.ScaledRatio);
            }
            this._rectangle.RenderTransform = canvas.MatrixTransform;
            this._rectangle.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
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
                    Stroke = new SolidColorBrush(this.BorderColor!.Value),
                    StrokeThickness = this.BorderThickness!.Value
                };
                canvas.Children.Add(this._circle);
            }

            Point rectifiedCenter = canvas.RectifiedStartPosition!.Value;
            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;
            Vector vector = Point.Subtract(rectifiedPosition, rectifiedCenter);

            this._circle.Center = rectifiedCenter;
            this._circle.Radius = vector.Length;
            this._circle.RenderTransform = canvas.MatrixTransform;
            this._circle.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
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
                    Stroke = new SolidColorBrush(this.BorderColor!.Value),
                    StrokeThickness = this.BorderThickness!.Value
                };
                canvas.Children.Add(this._ellipse);
            }

            Point rectifiedCenter = canvas.RectifiedStartPosition!.Value;
            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;

            this._ellipse.Center = rectifiedCenter;
            this._ellipse.RadiusX = Math.Abs(rectifiedPosition.X - rectifiedCenter.X);
            this._ellipse.RadiusY = Math.Abs(rectifiedPosition.Y - rectifiedCenter.Y);
            this._ellipse.RenderTransform = canvas.MatrixTransform;
            this._ellipse.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
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
                ? new SolidColorBrush(this.BorderColor!.Value)
                : new SolidColorBrush(Colors.Yellow);
            PointVisual2D anchor = new PointVisual2D
            {
                X = rectifiedPoint.X,
                Y = rectifiedPoint.Y,
                Fill = fillBrush,
                Stroke = borderBrush,
                RenderTransform = canvas.MatrixTransform
            };
            canvas.Children.Add(anchor);
            this._polyAnchors.Add(anchor);
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
                Stroke = new SolidColorBrush(this.BorderColor!.Value),
                StrokeThickness = this.BorderThickness!.Value,
                Points = points,
                RenderTransform = canvas.MatrixTransform,
                Tag = polygonL
            };

            polygonL.Tag = polygon;
            this.ShapeLs.Add(polygonL);
            this.Shapes.Add(polygon);
            canvas.Children.Add(polygon);

            //清空锚点
            foreach (PointVisual2D anchor in this._polyAnchors)
            {
                canvas.Children.Remove(anchor);
            }
            this._polyAnchors.Clear();
            polygon.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
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
                Stroke = new SolidColorBrush(this.BorderColor!.Value),
                StrokeThickness = this.BorderThickness!.Value,
                Points = points,
                RenderTransform = canvas.MatrixTransform,
                Tag = polylineL
            };

            polylineL.Tag = polyline;
            this.ShapeLs.Add(polylineL);
            this.Shapes.Add(polyline);
            canvas.Children.Add(polyline);

            //清空锚点
            foreach (PointVisual2D anchor in this._polyAnchors)
            {
                canvas.Children.Remove(anchor);
            }
            this._polyAnchors.Clear();
            polyline.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
        }
        #endregion

        #region 形状鼠标左击事件 —— void OnShapeMouseLeftDown(object sender...
        /// <summary>
        /// 形状鼠标左击事件
        /// </summary>
        private void OnShapeMouseLeftDown(object sender, MouseButtonEventArgs eventArgs)
        {
            if (this.CanvasMode != CanvasMode.Draw)
            {
                Shape shape = (Shape)sender;
                ShapeL shapeL = (ShapeL)shape.Tag;
                this.SelectedShapeL = null;
                this.SelectedShapeL = shapeL;
            }
        }
        #endregion

        #endregion
    }
}
