using System;
using System.Data;
using Syncfusion.Pdf;
using System.Drawing;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Graphics;
using System.Web;

namespace PDFCreation
{
    public partial class WebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 20.00f);
            string str = "My first pdf document created using Syncfusion Library.";
            graphics.DrawString(str, font, PdfBrushes.Red, new PointF(0, 0));
            document.Save("First.pdf", HttpContext.Current.Response, HttpReadType.Save);
            document.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;
            PdfBitmap image = new PdfBitmap(Server.MapPath("~/Images/Yogesh.jpg"));

            //graphics.DrawImage(image, new PointF(0, 0)); 
            //graphics.DrawImage(image, 0, 0);
            //graphics.DrawImage(image, new PointF(100, 100), new Size(200, 200));
            graphics.DrawImage(image, 100, 50, 300, 200);
            document.Save("Second.pdf", HttpContext.Current.Response, HttpReadType.Save);
            document.Close();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();
            DataTable dt = new DataTable();
            dt.Columns.Add("ID"); dt.Columns.Add("Name");
            dt.Columns.Add("Job"); dt.Columns.Add("Salary");
            dt.Rows.Add(1001, "Scott", "CEO", 50000);
            dt.Rows.Add(1002, "Smith", "President", 40000);
            dt.Rows.Add(1003, "Blake", "Manager", 30000);
            dt.Rows.Add(1004, "Jane", "Analyst", 15000);
            dt.Rows.Add(1005, "Thomas", "Salesman", 10000);
            dt.Rows.Add(1006, "Stefen", "Salesman", 10000);
            dt.Rows.Add(1007, "Mary", "Admin", 8000);
            dt.Rows.Add(1008, "Diana", "Admin", 8000);
            dt.Rows.Add(1009, "Peter", "Clerk", 5000);
            dt.Rows.Add(1010, "David", "Clerk", 5000);
            PdfGrid grid = new PdfGrid();
            grid.DataSource = dt;
            grid.Draw(page, new PointF(0, 0));
            document.Save("Thrid.pdf", HttpContext.Current.Response, HttpReadType.Save);
            document.Close();
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            PdfDocument document = new PdfDocument();
            document.PageSettings.Orientation = PdfPageOrientation.Landscape;
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new PointF(0, 0));
            document.Save("Fourth.pdf", HttpContext.Current.Response, HttpReadType.Save);
            document.Close();
        }
    }
}