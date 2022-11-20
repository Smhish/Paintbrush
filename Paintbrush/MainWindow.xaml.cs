using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paintbrush
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //選擇的圖形名稱(預設圖形 -> Line)
        string shape_Name = "Line";
        //選擇的筆刷顏色、填滿顏色(預設筆刷顏色 -> 黑, 預設填滿顏色 -> 無色)
        Color stroke_Color = Colors.Black, fill_Color = Colors.Transparent;
        //更改Extended.Wpf.Toolkit套件的ColorPicker物件的筆刷顏色、填滿顏色
        Brush stroke_ColorPicker, fill_ColorPicker;
        //選擇的滑鼠按住、滑鼠移動的座標
        Point again, end;
        //選擇的筆刷粗細(預設大小 -> 1)
        int stroke_Thickness = 1;

        public MainWindow()
        {
            InitializeComponent();

            //預設Extended.Wpf.Toolkit套件的ColorPicker物件的筆刷顏色 -> stroke_Color
            stroke.SelectedColor = stroke_Color;
            //預設筆刷顏色
            stroke_ColorPicker = new SolidColorBrush(stroke_Color);
            //預設Extended.Wpf.Toolkit套件的ColorPicker物件的填滿顏色 -> fill_Color
            fill.SelectedColor = fill_Color;
            //預設填滿顏色
            fill_ColorPicker = new SolidColorBrush(fill_Color);
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //滑鼠左鍵按住，觸發事件
        {
            //更改canvas畫布範圍中的滑鼠形狀 -> Cross十字型游標
            canvas.Cursor = Cursors.Cross;
            //取得滑鼠目前(按住位置)的座標
            again = e.GetPosition(canvas);

            //選擇指定圖形，設定繪圖過程中的初始屬性
            switch (shape_Name) 
            {
                case "Line":
                    DrawLine(Brushes.Gray, 1);
                    break;
                case "Rectangle":
                    DrawRectangle(Brushes.Gray, Brushes.LightGray, 1);
                    break;
                case "Ellipse":
                    DrawEllipse(Brushes.Gray, Brushes.LightGray, 1);
                    break;
                case "Polyline":
                    DrawPolyline(Brushes.Gray, Brushes.Transparent, 1);
                    break;
            }
            //偵測Canvas畫布裡面的滑鼠繪圖座標、每個圖形數量
            DisplayStatus(); 
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e) //滑鼠移動時，觸發事件
        {
            end = e.GetPosition(canvas); //取得滑鼠目前(移動位置)的座標

            //如果e.LeftButton滑鼠左鍵的狀態，有MouseButtonState.Pressed滑鼠已按住時，執行動作
            if (e.LeftButton == MouseButtonState.Pressed) 
            {
                //Children.OfType<Line>().LastOrDefault()，在多個項目中，篩選指定的類別Line，並選擇最後一個項目
                //Children，多個項目
                //OfType<TResult>(IEnumerable)，篩選指定的類別TResult
                //LastOrDefault()，選擇最後一個項目

                //選擇指定圖形，設定繪圖過程中的座標定位
                switch (shape_Name) 
                {
                    case "Line":
                        var line = canvas.Children.OfType<Line>().LastOrDefault();
                        if (line != null)
                        {
                            line.X2 = end.X;
                            line.Y2 = end.Y;
                        }
                        break;

                    case "Rectangle":
                        var rect = canvas.Children.OfType<Rectangle>().LastOrDefault();
                        if (rect != null) UpdateShape(rect);
                        break;

                    case "Ellipse":
                        var ellipse = canvas.Children.OfType<Ellipse>().LastOrDefault();
                        if (ellipse != null) UpdateShape(ellipse);
                        break;

                    case "Polyline":
                        var polyline = canvas.Children.OfType<Polyline>().LastOrDefault();
                        if (polyline != null)
                        {
                            polyline.Points.Add(end);
                        }
                        break;
                }
            }
            //偵測Canvas畫布裡面的滑鼠繪圖座標、每個圖形數量
            DisplayStatus(); 
        }
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) //滑鼠左鍵按住後放開，觸發事件
        {
            //Children.OfType<Line>().LastOrDefault()，在多個項目中，篩選指定的類別Line，並選擇最後一個項目
            //Children，多個項目
            //OfType<TResult>(IEnumerable)，篩選指定的類別TResult
            //LastOrDefault()，選擇最後一個項目

            //選擇指定圖形，設定繪圖過程中的最終輸出屬性
            switch (shape_Name) 
            {
                case "Line":
                    var line = canvas.Children.OfType<Line>().LastOrDefault();
                    if (line != null) UpdateShape(line, stroke_ColorPicker, fill_ColorPicker, stroke_Thickness);
                    break;
                case "Rectangle":
                    var rect = canvas.Children.OfType<Rectangle>().LastOrDefault();
                    if (rect != null) UpdateShape(rect, stroke_ColorPicker, fill_ColorPicker, stroke_Thickness);
                    break;
                case "Ellipse":
                    var ellipse = canvas.Children.OfType<Ellipse>().LastOrDefault();
                    if (ellipse != null) UpdateShape(ellipse, stroke_ColorPicker, fill_ColorPicker, stroke_Thickness);
                    break;
                case "Polyline":
                    var polyline = canvas.Children.OfType<Polyline>().LastOrDefault();
                    if (polyline != null) UpdateShape(polyline, stroke_ColorPicker, fill_ColorPicker, stroke_Thickness);
                    break;
            }
            //更改canvas畫布範圍中的滑鼠形狀 -> Arrow箭號游標(原來預設圖形)
            canvas.Cursor = Cursors.Arrow;
            //如果此元素物件保持捕獲，釋放滑鼠捕獲(可重新進行繪圖動作)
            canvas.ReleaseMouseCapture();
            //偵測Canvas畫布裡面的滑鼠繪圖座標、每個圖形數量
            DisplayStatus();               
        }
        private void DrawLine(Brush stroke, int thickness) //建構Line圖形
        {
            Line line = new Line()          //參數化建構圖形
            {
                Stroke = stroke,            //筆刷色彩
                X1 = again.X,               //滑鼠按住的X初始座標
                Y1 = again.Y,               //滑鼠按住的Y初始座標
                X2 = end.X,                 //滑鼠移動的X座標
                Y2 = end.Y,                 //滑鼠移動的Y座標
                StrokeThickness = thickness //筆刷粗細
            };
            canvas.Children.Add(line);      //canvas畫布添加圖形
        }
        private void DrawRectangle(Brush stroke, Brush fill, int thickness) //建構Rectangle圖形
        {
            Rectangle rect = new Rectangle() //參數化建構圖形
            {
                Stroke = stroke,             //筆刷色彩
                Fill = fill,                 //填滿顏色
                StrokeThickness = thickness  //筆刷粗細
            };
            canvas.Children.Add(rect);       //canvas畫布添加圖形
        }
        private void DrawEllipse(Brush stroke, Brush fill, int thickness) //建構Ellipse圖形
        {
            Ellipse ellipse = new Ellipse() //參數化建構圖形
            {
                Stroke = stroke,            //筆刷色彩
                Fill = fill,                //填滿顏色
                StrokeThickness = thickness //筆刷粗細
            };
            canvas.Children.Add(ellipse);   //canvas畫布添加圖形
        }
        private void DrawPolyline(Brush stroke, Brush fill, int thickness) //建構Polyline圖形
        {
            Polyline polyline = new Polyline()      //參數化建構圖形
            {
                StrokeDashCap = PenLineCap.Square,  //設定繪圖的"線段兩端點的區段形狀"
                StrokeLineJoin = PenLineJoin.Round, //設定繪圖的"線段連結頂點的轉彎形狀"
                Stroke = stroke,                    //筆刷色彩
                Fill = fill,                        //填滿顏色
                StrokeThickness = thickness         //筆刷粗細
            };
            polyline.Points.Add(again);             //Add first point -> 添加滑鼠按住時的起點座標
            canvas.Children.Add(polyline);          //canvas畫布添加圖形
        }
        private void UpdateShape(Shape shape) //設定Rectangle、Ellipse圖形，當滑鼠按住移動時的繪圖座標定位
        {
            Point origin;                                  //設定Point座標
            origin.X = Math.Min(again.X, end.X);           //找出兩個X座標最小值，得出圖形左上角的X座標
            origin.Y = Math.Min(again.Y, end.Y);           //找出兩個Y座標最小值，得出圖形左上角的Y座標
            double width = Math.Abs(again.X - end.X);      //求X座標相減的絕對值，得出圖形的寬度
            double height = Math.Abs(again.Y - end.Y);     //求X座標相減的絕對值，得出圖形的高度

            shape.Width = width;                           //取得新的width寬度
            shape.Height = height;                         //取得新的height寬度
            shape.SetValue(Canvas.LeftProperty, origin.X); //取得新的SetValue指定Canvas的LeftProperty，定位滑鼠移動時的Canvas畫布從左到右與圖形最小距離的origin.X座標，實現滑鼠拖移圖形時不受限制碰壁
            shape.SetValue(Canvas.TopProperty, origin.Y);  //取得新的SetValue指定Canvas的TopProperty，定位滑鼠移動時的Canvas畫布從上到下與圖形最小距離的origin.Y座標，實現滑鼠拖移圖形時不受限制碰壁
        }
        private void UpdateShape(Shape shape, Brush stroke, Brush fill, int thickness) //設定每個圖形要最終輸入的屬性
        {
            shape.Stroke = stroke;               //修改後的筆刷色彩
            shape.Fill = fill;                   //修改後的填滿色彩
            shape.StrokeThickness = thickness;   //修改後的筆刷粗細
        }
        private void DisplayStatus() //偵測Canvas畫布裡面的滑鼠繪圖座標、每個圖形數量
        {
            //偵測每個圖形數量
            int line_Count = canvas.Children.OfType<Line>().Count();
            int rect_Count = canvas.Children.OfType<Rectangle>().Count();
            int ellipse_Count = canvas.Children.OfType<Ellipse>().Count();
            int polyline_Count = canvas.Children.OfType<Polyline>().Count();

            //印出滑鼠繪圖座標、每個圖形數量 
            label_Point.Content = $"座標點:【滑鼠點擊({Math.Round(again.X)},{Math.Round(again.Y)})  滑鼠滑動({Math.Round(end.X)},{Math.Round(end.Y)})】";
            label_Count.Content = $"圖形數量:【Line({line_Count}個)  Rectangle({rect_Count}個)  Ellipse({ellipse_Count}個)  Polyline({polyline_Count}個)】";
        }
        private void StrokeColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) //更改Stroke筆刷的ColorPicker物件的Color顏色時，觸發事件
        {
            if (e.NewValue == null) return;                         //如果沒有此實質型別項目，跳出迴圈
            stroke_Color = (Color)e.NewValue;                       //取得更改後的顏色
            stroke_ColorPicker = new SolidColorBrush(stroke_Color); //Brush類別的stroke_ColorPicker筆刷顏色，取代為更改後的stroke_Color顏色        
        }
        private void FillColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) //更改Fill筆刷的ColorPicker物件的Color顏色時，觸發事件
        {
            if (e.NewValue == null) return;                     //如果沒有此實質型別項目，跳出迴圈
            fill_Color = (Color)e.NewValue;                     //取得更改後的顏色
            fill_ColorPicker = new SolidColorBrush(fill_Color); //Brush類別的fill_ColorPicker填滿顏色，取代為更改後的fill_Color顏色
        }
        private void RadioButton_Click(object sender, RoutedEventArgs e) //按下RadioButton核取按鈕，觸發事件
        {
            //取得RadioButton物件控制項
            var radioButton = sender as RadioButton; 
            //如果此物件為Null，跳出迴圈
            if (radioButton == null) return;
            //取得物件的Content屬性字串名稱
            var name = radioButton.Content.ToString();
            //如果此名稱為Null，跳出迴圈
            if(name == null) return;
            //取得選擇的圖形名稱
            shape_Name = name;
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) //進行滑動Slider物件，觸發事件
        {
            //取得更改後的筆刷粗細
            stroke_Thickness = (int)e.NewValue; 
        }
    }
}
