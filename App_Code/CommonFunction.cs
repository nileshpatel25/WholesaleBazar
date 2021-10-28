using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.Data;
using System.Collections;

namespace CommonUtilities
{
    public class CommonFunction
    {
        #region GetyyyMMddDateFormate In string
        public static String getyyyymmddFormatFromddmmyyyy(String fsDate)
        {
            string lsDD = fsDate.Substring(0, 2);
            string lsMM = fsDate.Substring(3, 2);
            string lsYYYY = fsDate.Substring(6, 4);

            return lsYYYY + "-" + lsMM + "-" + lsDD;
        }
        #endregion

        #region Send E-mails
        #endregion Send E-mails

        #region SetEnterEvent
        public static void setEnterEvent(TextBox foTextBox, Control foImageButton)
        {
            foTextBox.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)){document.getElementById('" + foImageButton.ClientID + "').click();return false;}}else {return true;}");
        }
        #endregion


        #region  Client-side alert() Message and redirect
        public static void showAsyncMsg(String fsMsg, Page foPage)
        {
            ScriptManager.RegisterStartupScript(foPage, foPage.GetType(), "AsyncMessage" + (new Random().Next()).ToString(), "alert('" + fsMsg.Replace("'", "\\'") + "');", true);
        }
        public static void showAsyncMsg(String fsMsg, Page foPage, String fsPath)
        {
            showAsyncMsg(fsMsg, foPage);
            redirectAsync(fsPath, foPage);
        }
        public static void showMsg(String fsMsg, Page foPage)
        {
            foPage.ClientScript.RegisterStartupScript(foPage.GetType(), "Message" + (new Random().Next()).ToString(), "alert('" + fsMsg.Replace("'", "\\'") + "');", true);
        }
        public static void showMsg(String fsMsg, Page foPage, String fsPath)
        {
            showMsg(fsMsg, foPage);
            redirect(fsPath, foPage);
        }
        public static void redirect(String fsPath, Page foPage)
        {
            foPage.ClientScript.RegisterStartupScript(foPage.GetType(), "Redirect" + (new Random().Next()).ToString(), "window.location.replace('" + fsPath.Replace("'", "\\'") + "');", true);
        }
        public static void redirectAsync(String fsPath, Page foPage)
        {
            ScriptManager.RegisterStartupScript(foPage, foPage.GetType(), "Redirect" + (new Random().Next()).ToString(), "window.location.href = '" + fsPath.Replace("'", "\\'") + "';", true);
        }
        #endregion

        #region Set Control Focus Client Side Method
        public static void setControlFocus(TextBox foTextBox, Page foPage)
        {
            ScriptManager.RegisterStartupScript(foPage, foPage.GetType(), "Show Windows" + new Random().Next().ToString(), "setTimeout(\"document.getElementById('" + foTextBox.ClientID + "').focus();\",200);", true);
        }

        public static void setControlFocus(String fsClientID, Page foPage)
        {
            foPage.ClientScript.RegisterStartupScript(System.Type.GetType("System.String"), "Startup", "<script language=\"javascript\" type=\"text/javascript\"> document.forms[0]['" + fsClientID + "'].focus();</script>");
        }

        public static void setFocusInModalPopup(string fsControlClientID, Page foPage)
        {
            foPage.ClientScript.RegisterStartupScript(foPage.GetType(), "Startup", "<script language=\"javascript\" type=\"text/javascript\">document.forms[0]." + fsControlClientID + ".focus();</script>");
        }
        #endregion

        #region CheckFileIsImage
        public static bool checkFileIsImage(string fsFileName)
        {
            if (!string.IsNullOrEmpty(fsFileName))
            {
                string lsImageName = fsFileName.Substring(0, fsFileName.IndexOf(".")).Replace(" ", "");
                string lsImageExtension = fsFileName.Substring(fsFileName.IndexOf(".")).Replace(" ", "");
                if (lsImageExtension.ToLower() == ".jpg" || lsImageExtension.ToLower() == ".jpeg" || lsImageExtension.ToLower() == ".png" || lsImageExtension.ToLower() == ".gif" || lsImageExtension.ToLower() == ".bmp")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        #endregion

        #region StringLimit
        public static string stringLimit(object Desc, int length)
        {
            StringBuilder strDesc = new StringBuilder();
            strDesc.Insert(0, Desc.ToString());
            if (strDesc.Length > length)
                return strDesc.ToString().Substring(0, length) + "...";
            else
                return strDesc.ToString();
        }
        #endregion StringLimit

        #region MessageTypeEnum
        public enum MessageType
        {
            Success = 1,
            Info = 2,
            Error = 3,
            Warning = 4
        }
        #endregion

        #region dropdown Tooltip
        public static DropDownList setdropdownTooltip(DropDownList loddl)
        {
            foreach (System.Web.UI.WebControls.ListItem curItem in loddl.Items)
            {
                curItem.Attributes.Add("title", HttpUtility.HtmlDecode(curItem.Text));
            }
            return loddl;
        }
        #endregion

        #region Image Generation - Ajay Vaddoriya
        public enum SizeOption
        {
            GenerateWidth,
            GenerateHeight,
            GenerateHeightWidth
        }
        public static string createFileName(string fsFileName)
        {
            if (!string.IsNullOrEmpty(fsFileName))
            {
                //string lsExt = System.IO.Path.GetExtension(fsFileName);
                //string lsImageName = fsFileName.Substring(0, fsFileName.IndexOf(lsExt)).Replace(" ", "");
                //string lsImageExtension = fsFileName.Substring(fsFileName.IndexOf(lsExt)).Replace(" ", "");
                string lsFileNameWithTimeStamp = DateTime.Now.ToFileTimeUtc().ToString() + ".jpg";
                fsFileName = lsFileNameWithTimeStamp;
            }
            return fsFileName;
        }

        public static string createPdfFileName()
        {
            return DateTime.Now.ToFileTimeUtc().ToString() + ".pdf";
        }

        public static string createOtherFileName(string fsFileName)
        {
            if (!string.IsNullOrEmpty(fsFileName))
            {
                string lsFileExtension = fsFileName.Substring(fsFileName.IndexOf(".")).Replace(" ", "");
                string lsFileNameWithTimeStamp = DateTime.Now.ToFileTimeUtc().ToString() + lsFileExtension;
                fsFileName = lsFileNameWithTimeStamp;
            }
            return fsFileName;
        }

        public static string createFolder(string fsPath)
        {
            if (Directory.Exists(fsPath) == false)
            {
                Directory.CreateDirectory(fsPath);
                return fsPath;
            }
            else
            {
                return fsPath;
            }
        }
        private static ImageCodecInfo getEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        public static bool uploadFile(string fsSourcePath, string fsDestinationPath, string fsImageName, int fiWidth, int fiHeight, string fsPrifix, string fsImageType)
        {
            createFolder(fsDestinationPath);
            Boolean lbImage = false;

            if (fsImageType == "FixedWidth")
            {
                generateThumbnail(fsSourcePath + fsImageName, fsDestinationPath + @"\" + fsPrifix + fsImageName, fiWidth, fiHeight, SizeOption.GenerateHeight);

            }
            else if (fsImageType == "FixedHeight")
            {
                generateThumbnail(fsSourcePath + fsImageName, fsDestinationPath + @"\" + fsPrifix + fsImageName, fiWidth, fiHeight, SizeOption.GenerateWidth);
            }
            else if (fsImageType == "FixedWidthHeight")
            {
                generateThumbnail(fsSourcePath + fsImageName, fsDestinationPath + @"\" + fsPrifix + fsImageName, fiWidth, fiHeight, SizeOption.GenerateHeightWidth);
            }
            else
            {
                generateThumbnail(fsSourcePath + fsImageName, fsDestinationPath + @"\" + fsPrifix + fsImageName, fiWidth, fiHeight, SizeOption.GenerateHeightWidth);
            }
            lbImage = true;

            return lbImage;
        }
        public static Bitmap CreateThumbnailWithFixedHeight(String fsImagePath, int fiHeight)
        {
            Bitmap loBitmap = null;
            if (fiHeight == 0)
                fiHeight = 100;
            int liNewWidth = 0;
            int liNewHeight = 0;
            System.Drawing.Image loImage;

            FileInfo loFileInfo = new FileInfo(fsImagePath);
            if (loFileInfo.Exists)
            {
                loImage = System.Drawing.Image.FromFile(fsImagePath);

                decimal ldHeightRatio = 1;

                if (loImage.Height > fiHeight)
                    ldHeightRatio = (decimal)fiHeight / loImage.Height;

                if (ldHeightRatio != 1)
                {
                    liNewHeight = fiHeight;
                    decimal ldTemp = loImage.Width * ldHeightRatio;
                    liNewWidth = (int)ldTemp;
                }
                else
                {
                    liNewHeight = loImage.Height;
                    liNewWidth = loImage.Width;
                }
                if (ldHeightRatio == 1)
                {
                    loBitmap = new Bitmap(loImage);
                    //Graphics loGraphics = Graphics.FromImage(loBitmap);
                    //loGraphics.DrawImage(loImage, 0, 0, liNewWidth, liNewHeight);
                    //loGraphics.Dispose();
                    loImage.Dispose();
                }
                else if (liNewWidth != 0 && liNewHeight != 0)
                {
                    if (liNewWidth > 170)
                    {
                        loImage.Dispose();
                        loBitmap = CreateThumbnailWithFixedWidth(fsImagePath, 100);
                    }
                    else
                    {
                        loBitmap = new Bitmap(liNewWidth, liNewHeight);
                        Graphics loGraphics = Graphics.FromImage(loBitmap);
                        loGraphics.SmoothingMode = SmoothingMode.HighQuality;
                        loGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        loGraphics.CompositingQuality = CompositingQuality.HighQuality;
                        loGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        loGraphics.DrawImage(loImage, 0, 0, liNewWidth, liNewHeight);
                        loGraphics.Dispose();
                        loImage.Dispose();
                    }
                }
            }
            return loBitmap;
        }
        public static Bitmap CreateThumbnailWithFixedWidth(String fsImagePath, int fiWidth)
        {
            Bitmap loBitmap1 = null;
            if (fiWidth == 0)
                fiWidth = 100;
            int liNewWidth = 0;
            int liNewHeight = 0;
            System.Drawing.Image loImage;

            FileInfo loFileInfo = new FileInfo(fsImagePath);
            if (loFileInfo.Exists)
            {
                loImage = System.Drawing.Image.FromFile(fsImagePath);

                decimal ldWidthRatio = 1;

                if (loImage.Width > fiWidth)
                    ldWidthRatio = (decimal)fiWidth / loImage.Width;

                if (ldWidthRatio != 1)
                {
                    liNewWidth = fiWidth;
                    decimal ldTemp = loImage.Height * ldWidthRatio;
                    liNewHeight = (int)ldTemp;
                }
                else
                {
                    liNewHeight = loImage.Height;
                    liNewWidth = loImage.Width;
                }
                if (ldWidthRatio == 1)
                {
                    loBitmap1 = new Bitmap(loImage);
                    //Graphics loGraphics = Graphics.FromImage(loBitmap);
                    //loGraphics.DrawImage(loImage, 0, 0, liNewWidth, liNewHeight);
                    //loGraphics.Dispose();
                    loImage.Dispose();
                }
                else if (liNewWidth != 0 && liNewHeight != 0)
                {
                    loBitmap1 = new Bitmap(liNewWidth, liNewHeight);
                    Graphics loGraphics = Graphics.FromImage(loBitmap1);
                    loGraphics.SmoothingMode = SmoothingMode.HighQuality;
                    loGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    loGraphics.CompositingQuality = CompositingQuality.HighQuality;
                    loGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    loGraphics.DrawImage(loImage, 0, 0, liNewWidth, liNewHeight);
                    loGraphics.Dispose();
                    loImage.Dispose();
                }
            }
            return loBitmap1;

        }
        public static Bitmap CreateThumbnailWithFixedWidthHeight(String fsImagePath, int fiWidth, int fiHeight)
        {
            Bitmap loBitmap1 = null;
            if (fiWidth == 0)
                fiWidth = 100;
            int liNewWidth = 0;
            int liNewHeight = 0;
            System.Drawing.Image loImage;

            FileInfo loFileInfo = new FileInfo(fsImagePath);
            if (loFileInfo.Exists)
            {
                loImage = System.Drawing.Image.FromFile(fsImagePath);

                decimal ldWidthRatio = 1;

                if (loImage.Width > fiWidth)
                    ldWidthRatio = (decimal)fiWidth / loImage.Width;

                if (ldWidthRatio != 1)
                {
                    liNewWidth = fiWidth;
                    decimal ldTemp = loImage.Height * ldWidthRatio;
                    liNewHeight = (int)ldTemp;
                }
                else
                {
                    liNewHeight = loImage.Height;
                    liNewWidth = loImage.Width;
                }
                if (ldWidthRatio == 1)
                {
                    loBitmap1 = new Bitmap(loImage);
                    //Graphics loGraphics = Graphics.FromImage(loBitmap);
                    //loGraphics.DrawImage(loImage, 0, 0, liNewWidth, liNewHeight);
                    //loGraphics.Dispose();
                    loImage.Dispose();
                }
                else if (liNewWidth != 0 && liNewHeight != 0)
                {
                    liNewWidth = fiWidth;
                    liNewHeight = fiHeight;
                    loBitmap1 = new Bitmap(liNewWidth, liNewHeight);
                    Graphics loGraphics = Graphics.FromImage(loBitmap1);
                    loGraphics.SmoothingMode = SmoothingMode.HighQuality;
                    loGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    loGraphics.CompositingQuality = CompositingQuality.HighQuality;
                    loGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    loGraphics.DrawImage(loImage, 0, 0, liNewWidth, liNewHeight);
                    loGraphics.Dispose();
                    loImage.Dispose();
                }
            }
            return loBitmap1;

        }
        public static Bitmap CreateThumbnail(String fsImagePath, int fiWidth, int fiHeight)
        {
            Bitmap loBitmap = null;
            if (fiWidth == 0)
                fiWidth = 100;
            if (fiHeight == 0)
                fiHeight = 100;
            int liNewWidth = 0;
            int liNewHeight = 0;
            System.Drawing.Image loImage;

            FileInfo loFileInfo = new FileInfo(fsImagePath);
            if (loFileInfo.Exists)
            {
                loImage = System.Drawing.Image.FromFile(fsImagePath);

                decimal ldWidthRatio = 1, ldHeightRatio = 1;
                if (loImage.Width > fiWidth)
                    ldWidthRatio = (decimal)fiWidth / loImage.Width;
                if (loImage.Height > fiHeight)
                    ldHeightRatio = (decimal)fiHeight / loImage.Height;

                if (ldWidthRatio != 1 || ldHeightRatio != 1)
                {
                    if (ldWidthRatio < ldHeightRatio)
                    {
                        liNewWidth = fiWidth;
                        decimal ldTemp = loImage.Height * ldWidthRatio;
                        liNewHeight = (int)Math.Ceiling(ldTemp);
                    }
                    else
                    {
                        liNewHeight = fiHeight;
                        decimal ldTemp = loImage.Width * ldHeightRatio;
                        liNewWidth = (int)Math.Ceiling(ldTemp);
                    }
                }
                else
                {
                    liNewHeight = loImage.Height;
                    liNewWidth = loImage.Width;
                }

                if (ldWidthRatio == 1 && ldHeightRatio == 1)
                {
                    loBitmap = new Bitmap(loImage);
                    //Graphics loGraphics = Graphics.FromImage(loBitmap);
                    //loGraphics.DrawImage(loImage, 0, 0, liNewWidth, liNewHeight);
                    //loGraphics.Dispose();
                    loImage.Dispose();
                }
                else if (liNewWidth != 0 && liNewHeight != 0)
                {
                    loBitmap = new Bitmap(liNewWidth, liNewHeight);
                    Graphics loGraphics = Graphics.FromImage(loBitmap);
                    loGraphics.SmoothingMode = SmoothingMode.HighQuality;
                    loGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    loGraphics.CompositingQuality = CompositingQuality.HighQuality;
                    loGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    loGraphics.DrawImage(loImage, 0, 0, liNewWidth, liNewHeight);
                    loGraphics.Dispose();
                    loImage.Dispose();

                }
            }
            return loBitmap;

        }
        private static void generateThumbnail(string fsSourceImage, string fsDestinationImagePath, int fiWidth, int fiHeight, SizeOption foSizeOption)
        {
            System.Drawing.Image loTransformedImage = null;

            string lsExt = System.IO.Path.GetExtension(fsDestinationImagePath);
            fsDestinationImagePath = fsDestinationImagePath.Replace(lsExt, "") + ".jpg";

            if (foSizeOption == SizeOption.GenerateHeight)
            {
                loTransformedImage = CreateThumbnailWithFixedWidth(fsSourceImage, fiWidth);
                if (loTransformedImage != null)
                    loTransformedImage.Save(fsDestinationImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else if (foSizeOption == SizeOption.GenerateWidth)
            {
                loTransformedImage = CreateThumbnailWithFixedHeight(fsSourceImage, fiHeight);
                if (loTransformedImage != null)
                    loTransformedImage.Save(fsDestinationImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else if (foSizeOption == SizeOption.GenerateHeightWidth)
            {
                loTransformedImage = CreateThumbnailWithFixedWidthHeight(fsSourceImage, fiWidth, fiHeight);
                if (loTransformedImage != null)
                    loTransformedImage.Save(fsDestinationImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                loTransformedImage = CreateThumbnail(fsSourceImage, fiWidth, fiHeight);
                if (loTransformedImage != null)
                    loTransformedImage.Save(fsDestinationImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            if (loTransformedImage != null)
            {
                loTransformedImage.Dispose();
                loTransformedImage = null;
            }
        }
        public static void deleteImage(string fsImagePath)
        {
            FileInfo loOriginalImage;
            loOriginalImage = new FileInfo(fsImagePath);
            if (loOriginalImage.Exists)
            {
                loOriginalImage.Delete();
            }
        }
        #endregion Image Generation - Ajay

        #region Send Error Mail
        public static void sendErrorMail(String fsToAddress, String fsSubject, String fsBody, String fsBCCAddress)
        {
            try
            {
                //String lsFromAddress = Convert.ToString("SMTPUsername");
                //String lsFromPassword = Convert.ToString("SMTPPassword");
                //SmtpClient loSmtpClient = new SmtpClient("SMTPServer"));
                //string lsFrom = DotNetNuke.Common.Globals.GetHostPortalSettings().PortalName + "<" + Convert.ToString(DotNetNuke.Common.Globals.HostSettings["SMTPUsername"]) + ">";
                //MailMessage loMailMessage = new MailMessage(lsFrom, fsToAddress, fsSubject, fsBody);
                //loMailMessage.IsBodyHtml = true;
                //loMailMessage.BodyEncoding = Encoding.GetEncoding("utf-8");

                //if (!String.IsNullOrEmpty(fsBCCAddress))
                //    loMailMessage.Bcc.Add(fsBCCAddress);

                //loSmtpClient.Credentials = new System.Net.NetworkCredential(lsFromAddress, lsFromPassword);
                //loSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                //loSmtpClient.EnableSsl = Convert.ToString(DotNetNuke.Common.Globals.HostSettings["SMTPEnableSSL"]) == "Y" ? true : false;
                //loSmtpClient.Send(loMailMessage);
            }
            catch (Exception feException)
            {
                throw feException;
            }
        }
        #endregion

        public static void showAsyncMsg(string p, string p_2, Page page)
        {
            throw new NotImplementedException();
        }
    }
}
