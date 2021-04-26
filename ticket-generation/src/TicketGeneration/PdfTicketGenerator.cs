using iTextSharp.text;
using iTextSharp.text.pdf;
using QRCoder;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace EventManagement.TicketGeneration
{
    public class PdfTicketGenerator
    {
        /// <summary>
        /// Generate a ticket as pdf.
        /// </summary>
        /// <param name="values">Values for the variables in the ticket.</param>
        /// <param name="outputStream">Stream to write the generated file data.</param>
        public void GenerateTicket(TicketData values, Stream outputStream)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            Document document = new Document(PageSize.A4, 30, 30, 40, 30);
            var writer = PdfWriter.GetInstance(document, outputStream);
            document.Open();

            // Add MetaData
            document.AddTitle("TICKET - " + values.EventName);
            document.AddSubject(values.EventName);
            document.AddKeywords("Ticket\n" + values.TicketType + "\n" + values.Host + "\n" + values.EventLocation);
            document.AddAuthor(values.Host);

            // Generate Fonts
            Font titlefont = FontFactory.GetFont("Arial", 30);
            Font titleboldfont = FontFactory.GetFont("Arial", 30, Font.BOLD);
            Font subtitleboldfont = FontFactory.GetFont("Arial", 20, Font.BOLD);
            Font standardfont = FontFactory.GetFont("Arial", 13);
            Font standardboldfont = FontFactory.GetFont("Arial", 13, Font.BOLD);
            Font smallfont = FontFactory.GetFont("Arial", 10);
            Font redfont = FontFactory.GetFont("Arial", 13, Font.BOLD, new BaseColor(16711680));
            Font greenfont = FontFactory.GetFont("Arial", 13, Font.BOLD, new BaseColor(51200));
            Font yellowfont = FontFactory.GetFont("Arial", 13, Font.BOLD, new BaseColor(16763904));
            Font smallitalicfont = FontFactory.GetFont("Arial", 8, Font.ITALIC);

            // Generate QR Code
            QRCodeGenerator qrgenerator = new QRCodeGenerator();
            QRCodeData qrcodedata = qrgenerator.CreateQrCode(values.QrValue, QRCodeGenerator.ECCLevel.Q);
            QRCode qrcode = new QRCode(qrcodedata);

            // Format Document

            // Generate Header Table
            #region
            PdfPTable headertable = new PdfPTable(3);
            headertable.HorizontalAlignment = 1;
            headertable.WidthPercentage = 100;
            headertable.SetWidths(new float[] { 25f, 50f, 25f });
            headertable.SpacingAfter = 10f;

            PdfPCell qrcell = new PdfPCell();
            qrcell.Image = Image.GetInstance(qrcode.GetGraphic(3), ImageFormat.Png);
            qrcell.HorizontalAlignment = 0;
            qrcell.BorderWidth = 0;
            qrcell.Rowspan = 2;
            headertable.AddCell(qrcell);

            PdfPCell titlecell = new PdfPCell(new Phrase(new Chunk("TICKET", titleboldfont)));
            titlecell.HorizontalAlignment = 1;
            titlecell.VerticalAlignment = 1;
            titlecell.BorderWidth = 0;
            headertable.AddCell(titlecell);

            PdfPCell logocell = new PdfPCell();
            logocell.Image = Image.GetInstance(values.EventLogo);
            logocell.HorizontalAlignment = 2;
            logocell.VerticalAlignment = 1;
            logocell.FixedHeight = 120;
            logocell.BorderWidth = 0;
            logocell.Rowspan = 2;
            headertable.AddCell(logocell);

            PdfPCell ticketidcell = new PdfPCell(new Phrase(new Chunk("Ticket-Nr." + values.TicketId, smallfont)));
            ticketidcell.HorizontalAlignment = 1;
            ticketidcell.VerticalAlignment = 1;
            ticketidcell.BorderWidth = 0;
            headertable.AddCell(ticketidcell);

            PdfPCell importantnotecell = new PdfPCell(new Phrase(new Chunk("Bitte zeigen Sie den QR-Code gut lesbar beim Einlass vor.", yellowfont)));
            importantnotecell.HorizontalAlignment = 0;
            importantnotecell.VerticalAlignment = 1;
            importantnotecell.BorderWidth = 0;
            headertable.AddCell(importantnotecell);

            PdfPCell hostcell = new PdfPCell(new Phrase(new Chunk("Veranstalter: " + values.Host, standardfont)));
            hostcell.HorizontalAlignment = 2;
            hostcell.VerticalAlignment = 1;
            hostcell.BorderWidth = 0;
            hostcell.Colspan = 2;
            headertable.AddCell(hostcell);

            PdfPCell whitecell = new PdfPCell();
            whitecell.BorderWidth = 0;
            headertable.AddCell(whitecell);

            headertable.AddCell(whitecell);

            document.Add(headertable);
            #endregion

            // Generate Event Title Table
            #region
            PdfPTable eventtitletable = new PdfPTable(1);
            eventtitletable.HorizontalAlignment = 1;
            eventtitletable.WidthPercentage = 100;
            //leave a gap before and after the table
            eventtitletable.SpacingBefore = 10f;
            eventtitletable.SpacingAfter = 10f;

            PdfPCell eventtitlecell = new PdfPCell(new Phrase(new Chunk(values.EventName, titleboldfont)));
            eventtitlecell.BorderColor = new BaseColor(13158600);
            eventtitlecell.BorderWidthBottom = 0;
            eventtitlecell.PaddingTop = 5;
            eventtitlecell.HorizontalAlignment = 1;
            eventtitlecell.VerticalAlignment = 1;
            eventtitletable.AddCell(eventtitlecell);

            PdfPCell eventdatecell = new PdfPCell(new Phrase(new Chunk((values.EventDate + ", " + values.EventLocation), standardfont)));
            eventdatecell.BorderColor = new BaseColor(13158600);
            eventdatecell.BorderWidthTop = 0;
            eventdatecell.PaddingBottom = 15;
            eventdatecell.HorizontalAlignment = 1;
            eventdatecell.VerticalAlignment = 1;
            eventtitletable.AddCell(eventdatecell);

            document.Add(eventtitletable);
            #endregion

            // Generate Event Info Table
            #region
            PdfPTable eventtable = new PdfPTable(2);
            eventtable.HorizontalAlignment = 1;
            eventtable.WidthPercentage = 100;
            eventtable.SetWidths(new float[] { 70, 30 });
            //leave a gap before and after the table
            eventtable.SpacingBefore = 10f;
            eventtable.SpacingAfter = 10f;

            PdfPCell eventtypecell = new PdfPCell(new Phrase(new Chunk(values.TicketType, subtitleboldfont)));
            eventtypecell.BorderWidth = 0;
            eventtypecell.Rowspan = 2;
            eventtypecell.HorizontalAlignment = 0;
            eventtypecell.VerticalAlignment = 0;
            eventtable.AddCell(eventtypecell);

            PdfPCell eventstarttitlecell = new PdfPCell(new Phrase(new Chunk("Event-Start", standardboldfont)));
            eventstarttitlecell.BorderWidth = 0;
            eventstarttitlecell.HorizontalAlignment = 0;
            eventstarttitlecell.VerticalAlignment = 2;
            eventtable.AddCell(eventstarttitlecell);

            PdfPCell eventstartcell = new PdfPCell(new Phrase(new Chunk(string.Format("{0}\nEinlass: {1}\nBeginn: {2}", values.EventDate, values.EntranceTime, values.BeginTime), standardfont)));
            eventstartcell.BorderWidth = 0;
            eventstartcell.HorizontalAlignment = 0;
            eventstartcell.VerticalAlignment = 0;
            eventstartcell.PaddingBottom = 20;
            eventtable.AddCell(eventstartcell);

            var sb = new StringBuilder();
            if (values.Price != null)
            {
                sb.AppendLine(values.Price);
            }
            else
            {
                sb.AppendLine();
            }

            PdfPCell eventpricecell = new PdfPCell(new Phrase(new Chunk(sb.ToString(), standardfont)));
            eventpricecell.BorderWidth = 0;
            eventpricecell.HorizontalAlignment = 0;
            eventpricecell.VerticalAlignment = 2;
            eventtable.AddCell(eventpricecell);

            PdfPCell eventlocationtitlecell = new PdfPCell(new Phrase(new Chunk("Event-Location", standardboldfont)));
            eventlocationtitlecell.BorderWidth = 0;
            eventlocationtitlecell.HorizontalAlignment = 0;
            eventlocationtitlecell.VerticalAlignment = 2;
            eventtable.AddCell(eventlocationtitlecell);


            sb = new StringBuilder();

            if (values.Transmissible == "true")
            {
                sb.AppendLine("Dieses Ticket ist übertragbar.");
            }
            else
            {
                sb.AppendLine("Dieses Ticket ist nicht übertragbar.");
            }

            PdfPCell ticketinfocell = new PdfPCell(new Phrase(new Chunk(sb.ToString(), smallfont)));
            ticketinfocell.BorderWidth = 0;
            ticketinfocell.HorizontalAlignment = 0;
            ticketinfocell.VerticalAlignment = 2;
            eventtable.AddCell(ticketinfocell);


            string address = string.Join("\n", values.Address ?? new string[0]);
            PdfPCell eventlocationcell = new PdfPCell(new Phrase(new Chunk(address, standardfont)));
            eventlocationcell.BorderWidth = 0;
            eventlocationcell.HorizontalAlignment = 0;
            eventlocationcell.VerticalAlignment = 0;
            eventtable.AddCell(eventlocationcell);

            document.Add(eventtable);

            #endregion

            // Generate Traffic Ticket
            #region
            if (values.QrTrafficImageUrl != null)
            {
                //Generate Traffic Table
                PdfPTable traffictable = new PdfPTable(2);
                traffictable.HorizontalAlignment = 1;
                traffictable.WidthPercentage = 100;
                traffictable.SetWidths(new float[] { 20, 80 });
                //leave a gap before and after the table
                traffictable.SpacingBefore = 10f;
                traffictable.SpacingAfter = 10f;

                PdfPCell traffictitlecell = new PdfPCell(new Phrase(new Chunk("Verkehrsticket", subtitleboldfont)));
                traffictitlecell.BorderWidth = 0;
                traffictitlecell.BorderWidthTop = 2;
                traffictitlecell.HorizontalAlignment = 0;
                traffictitlecell.PaddingTop = 10;
                traffictitlecell.PaddingBottom = 10;
                traffictitlecell.Colspan = 2;
                traffictable.AddCell(traffictitlecell);

                PdfPCell trafficqrcell = new PdfPCell();
                trafficqrcell.BorderWidth = 0;
                trafficqrcell.Image = Image.GetInstance(values.QrTrafficImageUrl);
                trafficqrcell.HorizontalAlignment = 1;
                trafficqrcell.VerticalAlignment = 1;
                trafficqrcell.PaddingRight = 10;
                trafficqrcell.Rowspan = 2;
                traffictable.AddCell(trafficqrcell);

                PdfPCell trafficinfo = new PdfPCell(new Phrase(new Chunk("Die Karte berechtigt zur Fahrt zum Veranstaltungsort mit den öffentlichen Verkehrsmitteln (2. Klasse) ab 3 Stunden vor Veranstaltungsbeginn und zur Rückfahrt bis 6 Uhr des Folgetages.", standardfont)));
                trafficinfo.BorderWidth = 0;
                trafficinfo.VerticalAlignment = 1;
                traffictable.AddCell(trafficinfo);

                PdfPCell trafficnote = new PdfPCell(new Phrase(new Chunk("Das Verkehrsticket ist nicht übertragbar!", redfont)));
                trafficnote.BorderWidth = 0;
                trafficnote.VerticalAlignment = 1;
                traffictable.AddCell(trafficnote);

                document.Add(traffictable);
            }
            #endregion

            // Generate Booking Table
            #region
            PdfPTable bookingtable = new PdfPTable(2);
            bookingtable.HorizontalAlignment = 1;
            bookingtable.WidthPercentage = 100;
            bookingtable.SetWidths(new float[] { 30, 70 });
            //leave a gap before and after the table
            bookingtable.SpacingBefore = 10f;
            bookingtable.SpacingAfter = 10f;


            var sb1 = new StringBuilder();
            sb1.AppendLine("Bestellt von");
            sb1.AppendLine("Buchungsdatum:");
            var sb2 = new StringBuilder();
            sb2.AppendLine(values.Buyer);
            sb2.AppendLine(values.BookingDate);

            if (values.BookingNumber != null)
            {
                sb1.AppendLine("Buchungsnummer:");
                sb2.AppendLine(values.BookingNumber);
            }
            

            PdfPCell bookinginfocell = new PdfPCell(new Phrase(new Chunk(sb1.ToString(), smallfont)));
            bookinginfocell.BorderWidth = 0;
            bookinginfocell.BorderWidthTop = 2;
            bookinginfocell.PaddingTop = 10;
            bookinginfocell.HorizontalAlignment = 0;
            bookinginfocell.VerticalAlignment = 0;
            bookingtable.AddCell(bookinginfocell);

            
            PdfPCell bookingdatacell = new PdfPCell(new Phrase(new Chunk(sb2.ToString(), smallfont)));
            bookingdatacell.BorderWidth = 0;
            bookingdatacell.BorderWidthTop = 2;
            bookingdatacell.PaddingTop = 10;
            bookingdatacell.HorizontalAlignment = 0;
            bookingdatacell.VerticalAlignment = 0;
            bookingtable.AddCell(bookingdatacell);

            document.Add(bookingtable);

            #endregion

            document.Close();
        }
    }
}
