namespace EigMedicoes.Modelo {

    public class Mensagem {

        public static void OpenNewMessage(string htmlBody, string subject, string to, string cc = "", bool modal = false, params string[] attachs) {
            Microsoft.Office.Interop.Outlook.Application olk = new Microsoft.Office.Interop.Outlook.Application();

            Microsoft.Office.Interop.Outlook.MailItem mail = olk.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                        

            foreach (var attc in attachs) {

                if (System.IO.Path.GetExtension(attc).Equals(".jpg", System.StringComparison.OrdinalIgnoreCase)) {

                    var img = mail.Attachments.Add(attc
                       , Microsoft.Office.Interop.Outlook.OlAttachmentType.olEmbeddeditem
                       );

                    img.PropertyAccessor.SetProperty(
                      "http://schemas.microsoft.com/mapi/proptag/0x3712001E"
                     , attc.GetHashCode().ToString()
                     );

                    //<img src=\"cid:{0}\">

                } else {
                    var a = mail.Attachments.Add(attc);
                }
            }

            mail.Display(modal);

            //mail.BodyFormat = Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML;
            mail.HTMLBody = htmlBody + mail.HTMLBody;
            //mail.HTMLBody = htmlBody;
            mail.To = to;
            mail.CC = cc;
            mail.Subject = subject;
            

            

            
        }
    }
}