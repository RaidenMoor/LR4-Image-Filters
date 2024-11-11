using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LR4
{
    public partial class Form1 : Form
    {
        int workingRangeStart = 0, workingRangeEnd = 255;
        bool isGrey = false;
        bool isNoise = false;
        Bitmap bitmap = new Bitmap("D:\\5th semester\\computer graphics\\f3ff4fc7-778d-5288-ad7b-30cb8ee401b7.jpg");
        Bitmap noiseBtm;
        int noiseProbabitily, matrixSize = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = (Image)bitmap;
         
        }

        private void buttonGrayscale_Click(object sender, EventArgs e)
        {
            // Bitmap bitmap = new Bitmap("D:\\2024\\girll.jpg");

            Bitmap grayScale = Filters.GreyScale(bitmap);
            bitmap = grayScale;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = (Image)bitmap;
            pictureBox3.Image = (Image)bitmap;
            pictureBox4.Image = (Image)bitmap;
            isGrey = true;
        }

       
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        
        private void buttonNegative_Click(object sender, EventArgs e)
        {
            if (isGrey)
            {
                Bitmap negative = Filters.Negate(bitmap, workingRangeStart, workingRangeEnd);
                pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox3.Image = (Image)negative;
            }
            else MessageBox.Show("Вы не перевели изображение в черно-белый формат");
        }

        private void buttonMedianFilter_Click(object sender, EventArgs e)
        {
            if (isNoise)
            {
                if (matrixSize == 0) MessageBox.Show("Выберите размер матрицы");
                else pictureBox4.Image = (Image)Filters.MedianFilter(noiseBtm, matrixSize);
            }
        }

        private void buttonSaltPepper_Click(object sender, EventArgs e)
        {
            if (isGrey)
            {             
                isNoise = true;
                noiseBtm = Filters.SaltPepperFilter(bitmap, noiseProbabitily);
                pictureBox4.Image = (Image) noiseBtm;
            }
            else MessageBox.Show("Вы не преобразили изображение в оттенки серого");
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBoxNoise_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Int32.TryParse(textBoxNoise.Text, out noiseProbabitily))
                {
                    if ((noiseProbabitily < 0) || (noiseProbabitily > bitmap.Width * bitmap.Height)) throw new Exception();
                }
                else throw new Exception();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Вы ввели некорректное значение");
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void buttonDiscardChanges_Click(object sender, EventArgs e)
        {
            pictureBox3.Image = (Image)bitmap;
            pictureBox4.Image= (Image)bitmap;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                matrixSize = 3;          
            if (comboBox1.SelectedIndex == 1)
                matrixSize = 5;
            if (comboBox1.SelectedIndex == 2)
                matrixSize = 7;
            if (comboBox1.SelectedIndex == 3)
                matrixSize = 9;
            if (comboBox1.SelectedIndex == 4)
                matrixSize = 11;
        }

        private void textBoxStart_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(textBoxStart.Text) < 0 || Convert.ToInt32(textBoxStart.Text) > 255) throw new Exception();
                else workingRangeStart = Convert.ToInt32(textBoxStart.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Вы ввели некорректное значение");
            }
        }

        private void textBoxEnd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(textBoxEnd.Text) < 0 || Convert.ToInt32(textBoxEnd.Text) > 255) throw new Exception();
                else workingRangeEnd = Convert.ToInt32(textBoxEnd.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Вы ввели некорректное значение");
            }
        }
    }
}
