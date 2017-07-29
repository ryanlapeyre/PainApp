using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaintApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum MyShape
        {
            Line, Ellipse, Rectangle, Brush, Erase, RemoveShapes, Select
        }
        private MyShape currentShape = MyShape.Line;

        private Boolean comboClosed = true;
        private int strokeThickness = 4;

        public MainWindow()
        {
            InitializeComponent();
            this.MyCanvas.EditingMode = InkCanvasEditingMode.None;
        }


        private void LineButton_Click(object sender, RoutedEventArgs e)
        {
            this.MyCanvas.EditingMode = InkCanvasEditingMode.None;
            currentShape = MyShape.Line;
        }

        private void EllipseButton_Click(object sender, RoutedEventArgs e)
        {
            this.MyCanvas.EditingMode = InkCanvasEditingMode.None;
            currentShape = MyShape.Ellipse;
        }

        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            this.MyCanvas.EditingMode = InkCanvasEditingMode.None;
            currentShape = MyShape.Rectangle;
        }


        Point start;
        Point end;



        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            start = e.GetPosition(this);
        }

        private void MyCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (currentShape)
            {
                case MyShape.Line:
                    DrawLine();
                    break;
                case MyShape.Ellipse:
                    DrawEllipse();
                    break;
                case MyShape.Rectangle:
                    DrawRectangle();
                    break;
                case MyShape.Brush:
                    break;
                case MyShape.Erase:
                    this.MyCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                    break;
                case MyShape.RemoveShapes:
                    break;
                default:
                    return;
            }
        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                end = e.GetPosition(this);
            }
        }

        private void DrawLine()
        {
            Line newLine = new Line()
            {
                Stroke = Brushes.Blue,
                X1 = start.X,
                Y1 = start.Y - 50,
                X2 = end.X,
                Y2 = end.Y - 50,
                StrokeThickness = strokeThickness
            };
            MyCanvas.Children.Add(newLine);
        }

        private void DrawEllipse()
        {
            Ellipse newEllipse = new Ellipse()
            {
                Fill = Brushes.Red,
                StrokeThickness = strokeThickness,
                Height = 10,
                Width = 10,
            };

            if (end.X >= start.X)
            {
                newEllipse.SetValue(InkCanvas.LeftProperty, start.X);
                newEllipse.Width = end.X - start.X;
            }
            else
            {
                newEllipse.SetValue(InkCanvas.LeftProperty, end.X);
                newEllipse.Width = start.X - end.X;
            }

            if (end.Y >= start.Y)
            {
                newEllipse.SetValue(InkCanvas.LeftProperty, start.Y - 50);
                newEllipse.Height = end.Y - start.Y;
            }
            else
            {
                newEllipse.SetValue(InkCanvas.TopProperty, end.Y - 50);
                newEllipse.Height = start.Y - end.Y;
            }
            MyCanvas.Children.Add(newEllipse);

        }

        private void DrawRectangle()
        {
            Rectangle newRectangle = new Rectangle()
            {
                Fill = Brushes.Red,
                StrokeThickness = strokeThickness,
            };

            if (end.X >= start.X)
            {
                newRectangle.SetValue(InkCanvas.LeftProperty, start.X);
                newRectangle.Width = end.X - start.X;
            }
            else
            {
                newRectangle.SetValue(InkCanvas.LeftProperty, end.X);
                newRectangle.Width = start.X - end.X;
            }

            if (end.Y >= start.Y)
            {
                newRectangle.SetValue(InkCanvas.LeftProperty, start.Y - 50);
                newRectangle.Height = end.Y - start.Y;
            }
            else
            {
                newRectangle.SetValue(InkCanvas.TopProperty, end.Y - 50);
                newRectangle.Height = start.Y - end.Y;
            }
            MyCanvas.Children.Add(newRectangle);

        }



        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            string radioButtonPressed = radioButton.Content.ToString();

            if (radioButtonPressed == "Draw")
            {
                this.MyCanvas.EditingMode = InkCanvasEditingMode.Ink;
                currentShape = MyShape.Brush;
            }
            else if (radioButtonPressed == "Erase")
            {
                this.MyCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                currentShape = MyShape.Erase;
            }
            else if (radioButtonPressed == "Select")
            {
                this.MyCanvas.EditingMode = InkCanvasEditingMode.Select;
                currentShape = MyShape.Select;
            }
            if (radioButtonPressed == "RemoveShape")
            {
                this.MyCanvas.EditingMode = InkCanvasEditingMode.None;
                currentShape = MyShape.RemoveShapes;
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                 saveFileDialog.DefaultExt = ".bmp";
                 saveFileDialog.Filter = "Bitmap Image (*.bmp)|*.bmp";
                 saveFileDialog.ShowDialog();
                 if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                 {
                     SaveCanvas(this, this.MyCanvas, 96, saveFileDialog.FileName);
                 }

           // this.MyCanvas.Strokes.Save(fs);

            /*
                   SaveFileDialog saveDialog = new SaveFileDialog();
                   saveDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
                   if (saveDialog.ShowDialog().Value == true)
                   {
                       RenderTargetBitmap targetBitmap =
                           new RenderTargetBitmap((int)MyCanvas.ActualWidth,
                                                  (int)MyCanvas.ActualHeight,
                                                  96d, 96d,
                                                  PixelFormats.Default);
                       targetBitmap.Render(MyCanvas);

                       BitmapEncoder encoder = new BmpBitmapEncoder();
                       string extension = saveDialog.FileName.Substring(saveDialog.FileName.LastIndexOf('.'));
                       switch (extension.ToLower())
                       {
                           case ".jpg":
                               encoder = new JpegBitmapEncoder();
                               break;
                           case ".bmp":
                               encoder = new BmpBitmapEncoder();
                               break;
                           case ".gif":
                               encoder = new GifBitmapEncoder();
                               break;
                           case ".png":
                               encoder = new PngBitmapEncoder();
                               break;
                       }
                       encoder.Frames.Add(BitmapFrame.Create(targetBitmap));
                       using (FileStream fs = File.Open(saveDialog.FileName, FileMode.OpenOrCreate))
                       {
                           encoder.Save(fs);
                       }


                   }
                   */
        }

            public static void SaveCanvas(Window window, InkCanvas canvas, int dpi, string filename)
            {
               Size size = new Size(window.Width, window.Height);
               canvas.Measure(size);
               //canvas.Arrange(new Rect(size));

               var rtb = new RenderTargetBitmap(
                   (int)window.Width, //width 
                   (int)window.Height, //height 
                   dpi, //dpi x 
                   dpi, //dpi y 
                   PixelFormats.Pbgra32 // pixelformat 
                   );
               rtb.Render(canvas);

               SaveRTBAsPNG(rtb, filename);
           }

           private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
           {
               var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
               enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

               using (var stm = System.IO.File.Create(filename))
               {
                   enc.Save(stm);
               }
           }


        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            /*  try
              {
                  OpenFileDialog openDialog = new OpenFileDialog();
                  if (openDialog.ShowDialog().Value == true)
                  {
                      
                      picture.Source = new BitmapImage(new Uri(openDialog.FileName));
                      MyCanvas.Width = picture.Width;
                      MyCanvas.Height = picture.Height;
                  }

              }
              catch (Exception ex)
              {

                  return;
              }
              */
            Image picture = new Image();
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog().Value == true)
            {
                picture.Source = new BitmapImage(new Uri(openDialog.FileName));
               // MyCanvas.Width = picture.Width;
               // MyCanvas.Height = picture.Height;
               // MyCanvas.Background = picture;
                MyCanvas.Children.Add(picture);
            }
        }
        private void StackPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           if (currentShape == MyShape.RemoveShapes)
            {
                if (e.OriginalSource is Shape)
                {
                    MyCanvas.Children.Remove(e.OriginalSource as UIElement);
                }
                
           }
        }

        private void BrushSize_DropDownClosed(object sender, EventArgs e)
        {
            if (comboClosed)
            {
                ChangeBrushSize();
            }
            comboClosed = true;

        }

        private void ChangeBrushSize()
        {
            try
            {
                string brushSize = comboBrushThickness.Text;
                MessageBox.Show(brushSize);

                strokeThickness = int.Parse(brushSize);
                strokeAttr.Width = strokeAttr.Height = strokeThickness;
            }
            catch
            {

            }
        }


    }

}
