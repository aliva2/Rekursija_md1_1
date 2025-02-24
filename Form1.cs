using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Rekursija_md1_1
{
    public partial class Form1 : Form
    {
        Graphics g;
        Pen circlePen = new Pen(Color.Purple, 2); 
        Pen squarePen = new Pen(Color.DarkGreen, 2);
        int x, y;
        int w = 175; // platums sākumā
        int h = 175; // augstums sākumā
        public Form1()
        {
            InitializeComponent();
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button2.Click += new System.EventHandler(this.button2_Click);

            x = (panel1.Width - w) / 2; ; // X poz sākumā
            y = (panel1.Height - h) / 2; ; // Y poz sākumā

            g = panel1.CreateGraphics(); // veido vietu kur zīmēt, abus vienā
        }

        // zīmēšanas rekursija aplim
        public void DrawCircleRecursion(int fx, int fy, int fw, int fh, int depth)
        {
            // zīmē apli
            g.DrawEllipse(circlePen, new Rectangle(fx, fy, fw, fh));

            // ja 2 vai 3 līmenis tad katrus 90 grādus zīmē jaunu apli kas 1/2 mazāka izmēra
            if (depth > 1)
            {
                int smallerRadiusW = fw / 2;
                int smallerRadiusH = fh / 2;

                //zīmē 4 jaunus apļus katrus 90 grādus
                DrawCircleRecursion(fx + fw / 2 - smallerRadiusW / 2, fy - smallerRadiusH / 2, smallerRadiusW, smallerRadiusH, depth - 1);  // 0 
                DrawCircleRecursion(fx + fw - smallerRadiusW / 2, fy + fh / 2 - smallerRadiusH / 2, smallerRadiusW, smallerRadiusH, depth - 1);  // 90
                DrawCircleRecursion(fx + fw / 2 - smallerRadiusW / 2, fy + fh - smallerRadiusH / 2, smallerRadiusW, smallerRadiusH, depth - 1);  // 180 
                DrawCircleRecursion(fx - smallerRadiusW / 2, fy + fh / 2 - smallerRadiusH / 2, smallerRadiusW, smallerRadiusH, depth - 1);  // 270 
                 
            }
        }

        // aplis
        private void button1_Click(object sender, EventArgs e)
        {
            int depth = int.Parse(textBox1.Text);
            if (depth >= 1 && depth <= 5) // rekursijas līmeņi 1-5
            {
                g.Clear(Form1.DefaultBackColor); // notīrīt pirms zīmēt jaunu
                DrawCircleRecursion(x, y, w, h, depth); // zīmēšanas rekursijas funkc aplim
            }
        }

        // kvadrāts
        private void button2_Click(object sender, EventArgs e)
        {
            int depth = int.Parse(textBox1.Text);
            if (depth >= 1 && depth <= 8) // rekursijas līmeņi 1-8
            {
                g.Clear(Form1.DefaultBackColor); // notīrīt pirms zīmēt jaunu
                DrawSquareRecursion(x, y, w, h, depth); // zīmēšanas rekursijas funkc kvadrātam
            }
        }

        // kvadrāta rekursija
        public void DrawSquareRecursion(int fx, int fy, int fw, int fh, int depth)
        {
            // kvadrāta centrs
            float cx = fx + fw / 2;
            float cy = fy + fh / 2;

            // ceļš kvadrāta zīmējumam
            GraphicsPath path = new GraphicsPath();

            // pievieno to ceļam, konvertējot float uz int
            path.AddRectangle(new Rectangle(
                Convert.ToInt32(fx),
                Convert.ToInt32(fy),
                Convert.ToInt32(fw),
                Convert.ToInt32(fh)
            ));

            // pagriež pa 45 grādiem * (depth - 1)
            float angle = 45f + 45f * (depth - 1);

            Matrix matrix = new Matrix();
            matrix.RotateAt(angle, new PointF(cx, cy));  // griež apkārt centra punktam
            path.Transform(matrix);  // pieliek rotāciju ceļam

            // uzzīmē kvadrātu
            g.DrawPath(squarePen, path);

            // ja 2+ līmenis zīmē kvadrātu
            if (depth > 1)
            {
                // katrs nākamais kvadrāts būs mazāks 
                float scaleFactor = (float)Math.Sqrt(2); // par cik samazināt
                float smallerSizeW = fw / scaleFactor; // platuma samazināšana
                float smallerSizeH = fh / scaleFactor; // augstuma samazināšana

                // jaunā kvadrāta novietojums
                float offsetX = (fw - smallerSizeW) / 2; // horizontālā nobīde
                float offsetY = (fh - smallerSizeH) / 2; // vertikālā nobīde

                // mazāko zīmē pagrieztu 45 grādus, visus pārvērš par int, jo float neder
                DrawSquareRecursion(
                    Convert.ToInt32(fx + offsetX), 
                    Convert.ToInt32(fy + offsetY), 
                    Convert.ToInt32(smallerSizeW), 
                    Convert.ToInt32(smallerSizeH), 
                    depth - 1
                );
            }
        }



    }




}
