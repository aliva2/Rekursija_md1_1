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
        //grafika
        Graphics g;
        //Aplim violeta, kvadrātam oranža krāsa
        Pen circlePen = new Pen(Color.Purple, 2); 
        Pen squarePen = new Pen(Color.DarkOrange, 2);
        int x, y;
        int w = 175; // platums sākumā
        int h = 175; // augstums sākumā
        public Form1()
        {
            InitializeComponent();
            // ar kodu uzliek events, kad poga noklikšķināta (button click)
            // bet var to dar;it arī design daļā vnk uzklikšķinot attiecīgai pogai
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button2.Click += new System.EventHandler(this.button2_Click);

            x = (panel1.Width - w) / 2; ; // X poz sākumā
            y = (panel1.Height - h) / 2; ; // Y poz sākumā

            g = panel1.CreateGraphics(); // veido vietu kur zīmēt, abus vienā
        }

        // zīmēšanas rekursija aplim x, y, platums, garums, rekursija līmenis
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
                DrawCircleRecursion(fx + fw / 2 - smallerRadiusW / 2, fy - smallerRadiusH / 2, smallerRadiusW, smallerRadiusH, depth - 1);  // 0 grādi
                DrawCircleRecursion(fx + fw - smallerRadiusW / 2, fy + fh / 2 - smallerRadiusH / 2, smallerRadiusW, smallerRadiusH, depth - 1);  // 90 grādi
                DrawCircleRecursion(fx + fw / 2 - smallerRadiusW / 2, fy + fh - smallerRadiusH / 2, smallerRadiusW, smallerRadiusH, depth - 1);  // 180 grādi
                DrawCircleRecursion(fx - smallerRadiusW / 2, fy + fh / 2 - smallerRadiusH / 2, smallerRadiusW, smallerRadiusH, depth - 1);  // 270 grādi
                 
            }
        }

        // aplis
        private void button1_Click(object sender, EventArgs e)
        {
            int depth = int.Parse(textBox1.Text); // nosaka rekursijas līmeni
            if (depth >= 1 && depth <= 5) // rekursijas līmeņi 1-5
            {
                g.Clear(Form1.DefaultBackColor); // notīrīt pirms zīmēt jaunu
                DrawCircleRecursion(x, y, w, h, depth); // zīmēšanas rekursijas funkc aplim
            }
        }

        // kvadrāts
        private void button2_Click(object sender, EventArgs e)
        {
            // deklarē sākotnējo dziļumu. ko ievada lietotājs
            int initialDepth = int.Parse(textBox1.Text);

            if (initialDepth >= 1 && initialDepth <= 8) // rekursijas līmeņi 1-8
            {
                g.Clear(Form1.DefaultBackColor); // notīrīt pirms zīmēt jaunu
                DrawSquareRecursion(x, y, w, h, initialDepth, initialDepth);
            }
        }

        // kvadrāta rekursija !!! pieliek klāt pašreizējo līmeni un sākuma līmeni, nevis tikai vienu līmeni !!!
        // tad ārējais kvadrāts būs vienmēr pagriezts 90 grādos
        public void DrawSquareRecursion(float x, float y, float w, float h, int currentDepth, int initialDepth)
        {
            if (currentDepth < 1) return; // bāze

            // aprēķina centra punktus rotācijai
            float centerX = x + w / 2f;
            float centerY = y + h / 2f;

            // GraphicsPath klase, ļauj veidot ar grafiskus ceļus, piemēram, kvadrātu,
            // lai pievienotu taisnstūri (kvadrātu) ceļam, kuru vēlāk zīmēt uz ekrāna
            using (GraphicsPath path = new GraphicsPath())
            // Matrix ir klase, lai veiktu matemātiskas transformācijas (rotāciju uc.)
            // uz grafiskiem objektiem. Pagriež kvadrātu par 45 leņķi
            using (Matrix transform = new Matrix())
            {
                // pievieno taisnstūri ceļam
                path.AddRectangle(new RectangleF(x, y, w, h));

                // aprēķina leņķi: 90° + 45° par katru jaunu rekursijas līmeni
                // (līm ko ievada - pašreiz) lai pagrieztu par 45 vai ne (45 0 45 0 utt.)
                float angle = 90f + 45f * (initialDepth - currentDepth);

                // pielieto rotāciju ap centru
                transform.RotateAt(angle, new PointF(centerX, centerY));
                // piemēro pagriešanu ceļam pirms zīmēšanas
                path.Transform(transform);

                // zīmē kvadrātu ar oranžo zīmuli
                g.DrawPath(squarePen, path);

                // aprēķina nākamo izmēru, par kvadrātsakni ar 2 mazāku, lai ielīstu iepriekšējā kvadrātā mpagriezies
                float scale = (float)Math.Sqrt(2);
                float newW = w / scale;
                float newH = h / scale;

                // centrē mazāko kvadrātu
                float changeX = (w - newW) / 2f;
                float changeY = (h - newH) / 2f;

                // rekursija ar atjaunotiem parametriem
                DrawSquareRecursion(x + changeX, y + changeY, newW, newH, currentDepth - 1, initialDepth);

            }
        }
    }
}
