using System;
using System.IO;
using System.Data;
using Syncfusion.Pdf;
using System.Drawing;
using Syncfusion.Pdf.Grid;
using System.Data.SqlClient;
using Syncfusion.Pdf.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDFCreation
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        DataSet ds;
        SqlConnection con;
        SqlDataAdapter da;

        protected void Page_Load(object sender, EventArgs e)
        {
            con = new SqlConnection("Data Source=DESKTOP-J65VTNS;Database=PaintCompany;Integrated Security=True");
            if (!IsPostBack)
            {
                da = new SqlDataAdapter("Select Distinct InvoiceID From Invoice", con);
                ds = new DataSet(); da.Fill(ds, "Invoice");
                ddlInvoice.DataSource = ds;
                ddlInvoice.DataTextField = "InvoiceID";
                ddlInvoice.DataValueField = "InvoiceID";
                ddlInvoice.DataBind();
                ddlInvoice.Items.Insert(0, "-Select Invoice-");
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.Pages.Add();
                PdfGraphics graphics = page.Graphics;

                //Drawing the logo image on the top
                PdfBitmap image = new PdfBitmap(Server.MapPath("~/Images/Logo.jpg"));
                graphics.DrawImage(image, 110, 0, 300, 100);

                //Drawing a rectangle below the logo in Coral color
                RectangleF bounds = new RectangleF(0, 100, graphics.ClientSize.Width, 30);
                PdfBrush brush = new PdfSolidBrush(new PdfColor(Color.Coral));
                graphics.DrawRectangle(brush, bounds);

                //Loading data from Database into DataSet by calling the GetInvoiceDetails method
                ds = GetInvoiceDetails(ddlInvoice.SelectedValue);

                //Creating PdfTextElement objects and placing them on the document
                PdfFont textFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
                PdfTextElement element = new PdfTextElement("INVOICE ID: " + ddlInvoice.SelectedValue, textFont);
                element.Brush = PdfBrushes.White;
                PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));

                string invoiceDate = "INVOICE DATE: " + ds.Tables[0].Rows[0][0];
                SizeF textSize = textFont.MeasureString(invoiceDate);
                element = new PdfTextElement(invoiceDate, textFont);
                element.Brush = PdfBrushes.White;
                result = element.Draw(page, bounds.Width - textSize.Width - 10, bounds.Top + 8);

                element = new PdfTextElement("BILL TO:", textFont);
                element.Brush = PdfBrushes.Blue;
                result = element.Draw(page, 10, result.Bounds.Bottom + 20);

                byte[] imageData = (byte[])ds.Tables[1].Rows[0][3];
                MemoryStream ms = new MemoryStream(imageData);
                PdfBitmap customerImage = new PdfBitmap(ms);
                graphics.DrawImage(customerImage, 410, result.Bounds.Bottom - 14, 100, 100);

                element = new PdfTextElement(ds.Tables[1].Rows[0][0].ToString(), textFont);
                result = element.Draw(page, 15, result.Bounds.Bottom);
                element = new PdfTextElement(ds.Tables[1].Rows[0][1].ToString(), textFont);
                result = element.Draw(page, 15, result.Bounds.Bottom);
                element = new PdfTextElement(ds.Tables[1].Rows[0][2].ToString(), textFont);
                result = element.Draw(page, 15, result.Bounds.Bottom);

                PdfPen linePen = new PdfPen(new PdfColor(Color.Coral), 0.75f);
                PointF startPoint = new PointF(0, result.Bounds.Bottom + 50);
                PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 50);
                graphics.DrawLine(linePen, startPoint, endPoint);
                DataTable invoiceDetails = ds.Tables[2];
                PdfGrid grid = new PdfGrid();
                grid.DataSource = invoiceDetails;

                PdfGridCellStyle headerStyle = new PdfGridCellStyle();
                headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
                headerStyle.TextBrush = PdfBrushes.White;
                headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 14f, PdfFontStyle.Regular);

                PdfGridRow header = grid.Headers[0];
                for (int i = 0; i < header.Cells.Count; i++)
                {
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                }
                header.ApplyStyle(headerStyle);

                PdfGridColumnCollection columns = grid.Columns;
                columns[2].Format.Alignment = PdfTextAlignment.Right;
                columns[3].Format.Alignment = PdfTextAlignment.Right;
                columns[4].Format.Alignment = PdfTextAlignment.Right;

                PdfGridStyle cellStyle = new PdfGridStyle();
                cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f, PdfFontStyle.Regular);
                cellStyle.TextBrush = PdfBrushes.DarkGreen;
                grid.Style = cellStyle;

                PdfGridLayoutResult gridResult = grid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 60),
              new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)));
                linePen = new PdfPen(new PdfColor(Color.Coral), 0.75f);
                startPoint = new PointF((gridResult.Bounds.Width / 5) * 4, gridResult.Bounds.Bottom + 20);
                endPoint = new PointF(graphics.ClientSize.Width, gridResult.Bounds.Bottom + 20);
                graphics.DrawLine(linePen, startPoint, endPoint);

                element = new PdfTextElement("TotalBill:", textFont);
                result = element.Draw(page, new PointF((gridResult.Bounds.Width / 5) * 3, gridResult.Bounds.Bottom + 25));

                string billAmount = ds.Tables[3].Rows[0][0].ToString();
                textSize = textFont.MeasureString(billAmount);
                element = new PdfTextElement(billAmount, textFont);
                result = element.Draw(page, new PointF(graphics.ClientSize.Width - textSize.Width, gridResult.Bounds.Bottom + 25));

                linePen = new PdfPen(new PdfColor(Color.Coral), 0.75f);
                startPoint = new PointF((gridResult.Bounds.Width / 5) * 4, gridResult.Bounds.Bottom + 45);
                endPoint = new PointF(graphics.ClientSize.Width, gridResult.Bounds.Bottom + 45);
                graphics.DrawLine(linePen, startPoint, endPoint);
                string signature = "Signature";
                textSize = textFont.MeasureString(signature);
                element = new PdfTextElement(signature, textFont);
                result = element.Draw(page, new PointF(graphics.ClientSize.Width - textSize.Width - 20,
              gridResult.Bounds.Bottom + 100));

                linePen = new PdfPen(new PdfColor(Color.Coral), 0.75f);
                startPoint = new PointF(0, graphics.ClientSize.Height - 25);
                endPoint = new PointF(graphics.ClientSize.Width, graphics.ClientSize.Height - 25);
                graphics.DrawLine(linePen, startPoint, endPoint);

                graphics.DrawString("13-74/292, Diamond Towers, Jubliee Hills - 33. Phone: 2381 9999, Fax: 2381 8899",
              new PdfStandardFont(PdfFontFamily.Courier, 10), PdfBrushes.DarkCyan,
              new PointF(0, graphics.ClientSize.Height - 20));

                graphics.DrawString("Email: PaintCompany@gmail.com, WebSite: www.paintcompany.com",
              new PdfStandardFont(PdfFontFamily.Courier, 10), PdfBrushes.DarkCyan,
              new PointF(0, graphics.ClientSize.Height - 10));

                PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
                layoutFormat.Layout = PdfLayoutType.Paginate;

                document.Save("Invoice.pdf", HttpContext.Current.Response, HttpReadType.Save);
            };


        }

        public DataSet GetInvoiceDetails(string InvoiceID)
        {
            da = new SqlDataAdapter("GetInvoiceDetails", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@InvoiceID", InvoiceID);
            ds = new DataSet(); da.Fill(ds); return ds;
        }
    }
}