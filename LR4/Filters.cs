using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace LR4
{
    internal class Filters
    {
        public Filters() { }
        public static Bitmap GreyScale (Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    int r = (int)pixelColor.R;
                    int g = (int)pixelColor.G;
                    int b = (int)pixelColor.B;
                    int avg = (int)(0.3*r + 0.59*g + 0.11*b);

                    Color getPixelColor = Color.FromArgb(avg, avg, avg);
                    bitmap.SetPixel(x, y, getPixelColor);
                }
            }
            return bitmap;
        }

        public static Bitmap Negate(Bitmap image, int workingRangeStart = 0, int workingRangeEnd = 255, int threshold = 128)
        {
            if (workingRangeStart < 0 || workingRangeStart > 255 ||
                workingRangeEnd < 0 || workingRangeEnd > 255 ||
                workingRangeStart >= workingRangeEnd ||
                threshold < 0 || threshold > 255)
            {
                throw new ArgumentException("Неверные значения параметров.");
            }

            int Ymin = 255, Ymax = 0;
            double a = (Ymax-Ymin)/(workingRangeEnd-workingRangeStart);
            double b = (Ymin*workingRangeEnd - Ymax*workingRangeStart)/ (workingRangeEnd - workingRangeStart);

            Bitmap clone = (Bitmap)image.Clone();
            //negative
            for (int y = 0; y < clone.Height; y++)
            {
                for (int x = 0; x < clone.Width; x++)
                {
                    
                    Color p = clone.GetPixel(x, y);
                                
                    int r = p.R;

                    if (r < workingRangeStart) r = Color.White.R;
                    else if (r > workingRangeEnd) r = Color.Black.R;

                    else 
                    {
                        r = (int)(a * r + b);

                    }
                    if (r < 0 )
                    {
                        r = 0;
                    }
                    else if (r > 255)
                    {
                        r = 255;
                    }                 


                        //set new ARGB value in pixel
                        clone.SetPixel(x, y, Color.FromArgb(r, r, r));
                }
            }
                    return clone;
        }
        public static Bitmap SaltPepperFilter(Bitmap modified, int count, int maxSize = 3)
        {
            Bitmap clone = (Bitmap)modified.Clone();
            // Минимумы и максимумы, в которых выбираются координаты точек для шума
            int minX = 0;
            int maxX = modified.Width - maxSize;

            int minY = 0;
            int maxY = modified.Height - maxSize;

            Random random = new Random();

            // Создание рандомного шума (в количестве count)
            for (int i = 0; i < count; i++)
            {
                // Рандомная координата
                int x = random.Next(minX, maxX);
                int y = random.Next(minY, maxY);

                // Рандомный размер
                int width = random.Next(1, maxSize + 1);
                int height = random.Next(1, maxSize + 1);

                Graphics g = Graphics.FromImage(clone);

                // Цвет, чтобы сделать точку - черная или белая
                Color color = i % 2 == 0
                    ? Color.Black
                    : Color.White;

                g.FillRectangle(new SolidBrush(color), x, y, width, height);
            }

            return clone;
        }


     
        public static Bitmap MedianFilter(Bitmap original, int matrixSize)
        {
            // Размер смещения
            int ms = (matrixSize - 1) / 2;

            // Размеры изображения
            int WIDTH = original.Width;
            int HEIGHT = original.Height;
            Bitmap modified = original;


    

            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                   
                    List<int> pixels = new List<int>();

                    // В список попадают пиксели, находящиеся вокруг текущего.
                    // Собираются все пиксели, которые находятся слева/справа/сверху/снизу и по диагоналям, 
                    // от текущего пикселя. В список добавляется текущий пиксель.
                    for (int i = x - ms; i <= x + ms; i++)
                    {
                        for (int j = y - ms; j <= y + ms; j++)
                        {
                            if (i < 0 || j < 0 || i > WIDTH - 1 || j > HEIGHT - 1)
                            {
                                continue;
                            }

                            // Получение яркости пикселя (вместо ToArgb())
                            int brightness = original.GetPixel(i, j).R; // Используем канал R для черно-белого
                            pixels.Add(brightness);
                        }
                    }

                    // Сортировка пикселей
                    pixels.Sort();
                    int medianIndex;
                    int newBrightness;
                    if(pixels.Count % 2 == 0)
                    {
                        medianIndex = pixels.Count / 2;
                        newBrightness = (pixels[medianIndex] + pixels[medianIndex - 1])/2;
                    }
                    else
                    {
                        // Индекс пикселя медианы
                        medianIndex = (pixels.Count - 1) / 2;
                        newBrightness = (pixels[medianIndex]);
                    }
                    

                    // В результирующее изображение устанавливается найденный медианный пиксель
                    modified.SetPixel(x, y, Color.FromArgb(newBrightness, newBrightness, newBrightness));
                }
            }

            return modified;
        }
      
    }


}

