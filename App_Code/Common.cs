using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net.Mail;
using System.Drawing.Drawing2D;

public class Common
{
    #region Variables & Fields
    private static byte[] key = { };
    private static byte[] IV = { 38, 55, 206, 48, 28, 64, 20, 16 };
    private static string stringKey = "!5663a#KN";
    #endregion

    #region Public Common Methods

    /// <summary>
    ///  For Encrypt QueryString 
    /// used on Notify Seller page
    /// Developed By Foram Shah
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Encrypt(string text)
    {
        try
        {
            key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] byteArray = Encoding.UTF8.GetBytes(text);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
            cryptoStream.Write(byteArray, 0, byteArray.Length);
            cryptoStream.FlushFinalBlock();
            return Convert.ToBase64String(memoryStream.ToArray());
        }
        catch (Exception ex)
        {
            // Handle Exception Here
            throw ex;
        }
        //return string.Empty;
    }

    /// <summary>
    /// For Decrypt QueryString 
    /// used on Notify Seller page
    /// Developed By Foram Shah
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Decrypt(string text)
    {
        try
        {
            key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] byteArray = Convert.FromBase64String(text);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
            cryptoStream.Write(byteArray, 0, byteArray.Length);
            cryptoStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
        catch (Exception ex)
        {
            // Handle Exception Here
            throw ex;
        }
        // return string.Empty;
    }

    /// <summary>
    ///Function for display fix DigitFormat 
    ///(eg=0.000,0.00) use in grid use at resulr page
    /// </summary>
    /// <param name="str"></param>
    /// <param name="Precision"></param>
    /// <returns></returns>
    public static string FixedPoint(string str, int Precision)
    {
        string[] ValPart = str.Split('.');
        if (ValPart.Length > 1)
        {
            if (ValPart[1].ToString().Length >= Precision)
                return ValPart[0] + "." + ValPart[1].ToString().Substring(0, Precision);
            else
            {
                for (int i = ValPart[1].ToString().Length; i < Precision; i++)
                {
                    ValPart[1] = ValPart[1] + "0";
                }
                return ValPart[0] + "." + ValPart[1];
            }
        }
        else
        {
            string valpart = String.Empty;
            for (int i = 0; i < Precision; i++)
            {
                valpart = valpart + "0";
            }
            return str + "." + valpart;
        }
    }

    /// <summary>
    /// Fiiling Headers
    /// for Export to Excel,PDF functionality
    /// Made By Foram
    /// </summary>
    /// <param name="gridvw"></param>
    /// <returns></returns>
    public static string[] DynamicHeader(GridView gridvw)
    {

        string[] _strHeader;
        int count = 0;
        try
        {
            for (int i = 1; i < gridvw.Columns.Count; i++)//For Getting Length of String Array
            {
                if ((gridvw.Columns[i].Visible) && (!gridvw.Columns[i].HeaderText.Equals("")) && (!gridvw.Columns[i].SortExpression.Equals("")))
                    count++;
            }
            _strHeader = new string[count];
            count = 0;
            for (int i = 1; i < gridvw.Columns.Count; i++)//for Filling String Array Header
            {
                if ((gridvw.Columns[i].Visible) && (!gridvw.Columns[i].HeaderText.Equals("")) && (!gridvw.Columns[i].SortExpression.Equals("")))
                {
                    _strHeader.SetValue(gridvw.Columns[i].HeaderText, count); //For Header
                    count++;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return _strHeader;
    }

    /// <summary>
    /// Fiiling  BoundFields 
    /// for Export to Excel,PDF functionality
    /// Made By Foram
    /// </summary>
    /// <param name="gridvw"></param>
    /// <returns></returns>
    public static string[] DynamicFieldName(GridView gridvw)
    {
        string[] _strFieldName;
        int count = 0;
        try
        {
            for (int i = 1; i < gridvw.Columns.Count; i++)//For Getting Length of String Array
            {
                if ((gridvw.Columns[i].Visible) && (!gridvw.Columns[i].HeaderText.Equals("")) && (!gridvw.Columns[i].SortExpression.Equals("")))
                    count++;
            }
            _strFieldName = new string[count];
            count = 0;
            for (int i = 1; i < gridvw.Columns.Count; i++)//for Filling String Array BoundField
            {
                if ((gridvw.Columns[i].Visible) && (!gridvw.Columns[i].HeaderText.Equals("")) && (!gridvw.Columns[i].SortExpression.Equals("")))
                {
                    _strFieldName.SetValue(gridvw.Columns[i].SortExpression, count); //For BoundField
                    count++;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return _strFieldName;
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="p_dtTemp"></param>
    ///// <param name="p_headerName"></param>
    ///// <param name="p_fieldName"></param>
    ///// <param name="p_fileName"></param>
    //public static void ConvertToPdf(DataTable p_dtTemp, string[] p_headerName, string[] p_fieldName, string p_fileName)
    //{
    //    try
    //    {
    //        Document doc = new Document();
    //        PdfWriter.GetInstance(doc, new FileStream(p_fileName, FileMode.Create));
    //        doc.Open();

    //        PdfPTable tblResultData = new PdfPTable(p_headerName.Length);

    //        PdfPCell cellHeader;
    //        PdfPCell cellContent;

    //        for (int i = 0; i < p_headerName.Length; i++)
    //        {
    //            cellHeader = new PdfPCell(new Phrase(p_headerName[i], new Font(iTextSharp.text.Font.BOLD, 9)));
    //            tblResultData.AddCell(cellHeader);
    //        }
    //        double dblTemp;
    //        if (p_dtTemp != null)
    //        {
    //            if (p_dtTemp.Rows.Count > 0)
    //            {
    //                foreach (DataRow _dr in p_dtTemp.Rows)
    //                {
    //                    int FieldIndex = 0;
    //                    for (int i = 0; i < p_fieldName.Length; i++)
    //                    {
    //                        for (int j = 0; j < p_dtTemp.Columns.Count; j++)
    //                        {
    //                            if (p_dtTemp.Columns[j].ColumnName.Equals(p_fieldName[FieldIndex], StringComparison.InvariantCultureIgnoreCase))
    //                            {
    //                                if (!string.IsNullOrEmpty(_dr[j].ToString()))
    //                                {
    //                                    cellContent = new PdfPCell(new Phrase(_dr[j].ToString(), new Font(iTextSharp.text.Font.NORMAL, 8)));

    //                                    if (double.TryParse(_dr[j].ToString(), out dblTemp))
    //                                        cellContent.HorizontalAlignment = iTextSharp.text.Cell.ALIGN_RIGHT;

    //                                    tblResultData.AddCell(cellContent);
    //                                }
    //                                else
    //                                {
    //                                    //cellContent = new PdfPCell(new Phrase());
    //                                    tblResultData.AddCell(new PdfPCell(new Phrase(" ", new Font(iTextSharp.text.Font.NORMAL, 8))));
    //                                }
    //                                FieldIndex++;
    //                                break;
    //                            }
    //                        }
    //                    }
    //                }
    //                doc.Add(tblResultData);
    //                doc.Close();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p_dtTemp"></param>
    /// <param name="p_headerName"></param>
    /// <param name="p_fieldName"></param>
    /// <returns></returns>
    public static string ConvertToExcel(DataTable p_dtTemp, string[] p_headerName, string[] p_fieldName)
    {
        string _strExcelData = null;
        try
        {
            //By Hardik...
            //string sep = "";
            string sep = string.Empty;
            for (int i = 0; i < p_headerName.Length; i++)
            {
                _strExcelData += sep + p_headerName[i].ToString();
                sep = "\t";
            }
            _strExcelData += "\n";

            if (p_dtTemp != null)
            {
                if (p_dtTemp.Rows.Count > 0)
                {
                    foreach (DataRow _dr in p_dtTemp.Rows)
                    {
                        //By Hardik...
                        //sep = "";
                        sep = string.Empty;
                        int FieldIndex = 0;
                        for (int i = 0; i < p_fieldName.Length; i++)
                        {
                            for (int j = 0; j < p_dtTemp.Columns.Count; j++)
                            {
                                if (p_dtTemp.Columns[j].ColumnName.Equals(p_fieldName[FieldIndex], StringComparison.InvariantCultureIgnoreCase))
                                {
                                    _strExcelData += sep + _dr[j].ToString();
                                    sep = "\t";
                                    FieldIndex++;
                                    break;
                                }
                            }
                        }
                        _strExcelData += "\n";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return _strExcelData;
    }

    ///// <summary>
    ///// Zip a file
    ///// </summary>
    ///// <param name="SrcFile">source file path</param>
    ///// <param name="DstFile">zipped file path</param>
    //public static void Zip(string SourceFile, string DestinationFile)
    //{
    //    FileStream fileStreamIn = new FileStream(SourceFile, FileMode.Open, FileAccess.Read);
    //    FileStream fileStreamOut = new FileStream(DestinationFile, FileMode.Create, FileAccess.Write);
    //    ZipOutputStream zipOutStream = new ZipOutputStream(fileStreamOut);

    //    try
    //    {
    //        byte[] buffer = new byte[4096];

    //        ZipEntry entry = new ZipEntry(Path.GetFileName(SourceFile));
    //        zipOutStream.PutNextEntry(entry);

    //        int size;
    //        do
    //        {
    //            size = fileStreamIn.Read(buffer, 0, buffer.Length);
    //            zipOutStream.Write(buffer, 0, size);
    //        } while (size > 0);

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        zipOutStream.Close();
    //        fileStreamOut.Close();
    //        fileStreamIn.Close();
    //    }
    //}

    ///// <summary>
    ///// UnZip all files
    ///// </summary>
    ///// <param name="SrcFile">source file path</param>
    ///// <param name="DstFile">unzipped file path</param>
    //public static void UnZipAll(string SourceFile, string DestinationPath)
    //{
    //    FileStream fileStreamIn = new FileStream(SourceFile, FileMode.Open, FileAccess.Read);
    //    ZipInputStream zipInStream = new ZipInputStream(fileStreamIn);

    //    try
    //    {
    //        do
    //        {
    //            ZipEntry entry = zipInStream.GetNextEntry();
    //            if (entry == null)
    //            {
    //                break;
    //            }
    //            FileStream fileStreamOut = new FileStream(DestinationPath + @"\" + entry.Name, FileMode.Create, FileAccess.Write);
    //            int size;
    //            byte[] buffer = new byte[4096];
    //            do
    //            {
    //                size = zipInStream.Read(buffer, 0, buffer.Length);
    //                fileStreamOut.Write(buffer, 0, size);
    //            } while (size > 0);
    //            fileStreamOut.Close();
    //        }
    //        while (true);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        zipInStream.Close();
    //        fileStreamIn.Close();
    //    }
    //}

    ///// <summary>
    ///// UnZip all files
    ///// </summary>
    ///// <param name="SrcFile">source file path</param>
    ///// <param name="DstFile">unzipped file path</param>
    //public static void UnZipSingleFile(string SourceFile, string FileNameToExtract, string FileToExtractPrefix, string DestinationPath)
    //{
    //    FileStream fileStreamIn = new FileStream(SourceFile, FileMode.Open, FileAccess.Read);
    //    ZipInputStream zipInStream = new ZipInputStream(fileStreamIn);

    //    try
    //    {
    //        do
    //        {
    //            ZipEntry entry = zipInStream.GetNextEntry();
    //            if (entry == null)
    //            {
    //                break;
    //            }
    //            if (entry.Name.Equals(FileNameToExtract, StringComparison.InvariantCulture))
    //            {
    //                FileStream fileStreamOut = new FileStream(DestinationPath + @"\" + FileToExtractPrefix + entry.Name, FileMode.Create, FileAccess.Write);
    //                int size;
    //                byte[] buffer = new byte[4096];
    //                do
    //                {
    //                    size = zipInStream.Read(buffer, 0, buffer.Length);
    //                    fileStreamOut.Write(buffer, 0, size);
    //                } while (size > 0);
    //                fileStreamOut.Close();
    //            }
    //        }
    //        while (true);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        zipInStream.Close();
    //        fileStreamIn.Close();
    //    }
    //}

    /// <summary>
    ///  Developed By Hardik Desai, for counting total rows in a table in GrideView
    /// </summary>
    /// <param name="_dt"></param>
    /// <returns></returns>
    private int rowCount(DataTable _dt)
    {
        int _rowCount = 0;
        try
        {
            if (_dt != null && _dt.Rows.Count > 0)
            {
                _rowCount = _dt.Rows.Count;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            _dt = null;
        }

        return _rowCount;
    }

    /// <summary>
    /// Function for add ToolTip in any LstControl
    /// </summary>
    /// <param name="lc"></param>
    public static void BindToolTip(ListControl lc)
    {
        for (int i = 0; i < lc.Items.Count; i++)
        {
            lc.Items[i].Attributes.Add("title", lc.Items[i].Text);
        }
    }

    public static void AddRowSpecial(ref DataTable p_dtTemp, string p_RowCaption, string p_TextField, string p_ValueField)
    {
        try
        {
            DataRow _dr;
            _dr = p_dtTemp.NewRow();
            _dr[p_ValueField] = -1;
            _dr[p_TextField] = p_RowCaption;
            p_dtTemp.Rows.InsertAt(_dr, 0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// </summary>
    /// <param name="p_dtTemp"></param>
    /// <param name="p_RowCaption"></param>
    /// <param name="p_TextField"></param>
    /// <param name="p_ValueField"></param>
    public static void AddRow(ref DataTable p_dtTemp, string p_RowCaption, string p_TextField, string p_ValueField)
    {
        try
        {
            DataRow _dr;
            _dr = p_dtTemp.NewRow();
            _dr[p_ValueField] = 0;
            _dr[p_TextField] = p_RowCaption;
            p_dtTemp.Rows.InsertAt(_dr, 0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void AddRow(ref DataTable p_dtTemp, string p_RowCaption, string p_TextField, string p_ValueField1, string p_ValueField2)
    {
        try
        {
            DataRow _dr;
            _dr = p_dtTemp.NewRow();
            _dr[p_ValueField1] = 0;
            _dr[p_ValueField2] = 0;
            _dr[p_TextField] = p_RowCaption;
            p_dtTemp.Rows.InsertAt(_dr, 0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void AddRow(ref DataTable p_dtTemp, string p_RowCaption, string p_TextField, string p_ValueField1, string p_ValueField2, string p_ValueField3)
    {
        try
        {
            DataRow _dr;
            _dr = p_dtTemp.NewRow();
            _dr[p_ValueField1] = 0;
            _dr[p_ValueField2] = 0;
            _dr[p_ValueField3] = 0;
            _dr[p_TextField] = p_RowCaption;
            p_dtTemp.Rows.InsertAt(_dr, 0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void AddRow(ref DataTable p_dtTemp, string p_RowCaption, string p_TextField, string p_ValueField1, string p_ValueField2, string p_ValueField3, string p_ValueField4)
    {
        try
        {
            DataRow _dr;
            _dr = p_dtTemp.NewRow();
            _dr[p_ValueField1] = 0;
            _dr[p_ValueField2] = 0;
            _dr[p_ValueField3] = 0;
            _dr[p_ValueField4] = 0;
            _dr[p_TextField] = p_RowCaption;
            p_dtTemp.Rows.InsertAt(_dr, 0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void AddRow(ref DataTable p_dtTemp, string p_RowCaption, string p_TextField, string p_ValueField1, string p_ValueField2, string p_ValueField3, string p_ValueField4, string p_ValueField5)
    {
        try
        {
            DataRow _dr;
            _dr = p_dtTemp.NewRow();
            _dr[p_ValueField1] = 0;
            _dr[p_ValueField2] = 0;
            _dr[p_ValueField3] = 0;
            _dr[p_ValueField4] = 0;
            _dr[p_ValueField5] = 0;
            _dr[p_TextField] = p_RowCaption;
            p_dtTemp.Rows.InsertAt(_dr, 0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void AddRow(ref DataTable p_dtTemp, string p_RowCaption, string p_TextField, string p_ValueField1, string p_ValueField2, string p_ValueField3, string p_ValueField4, string p_ValueField5, string p_ValueField6)
    {
        try
        {
            DataRow _dr;
            _dr = p_dtTemp.NewRow();
            _dr[p_ValueField1] = 0;
            _dr[p_ValueField2] = 0;
            _dr[p_ValueField3] = 0;
            _dr[p_ValueField4] = 0;
            _dr[p_ValueField5] = 0;
            _dr[p_ValueField6] = 0;
            _dr[p_TextField] = p_RowCaption;
            p_dtTemp.Rows.InsertAt(_dr, 0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void AddRow(ref DataTable p_dtTemp, string p_RowCaption, string p_TextField, string p_ValueField1, string p_ValueField2, string p_ValueField3, string p_ValueField4, string p_ValueField5, string p_ValueField6, string p_ValueField7)
    {
        try
        {
            DataRow _dr;
            _dr = p_dtTemp.NewRow();
            _dr[p_ValueField1] = 0;
            _dr[p_ValueField2] = 0;
            _dr[p_ValueField3] = 0;
            _dr[p_ValueField4] = 0;
            _dr[p_ValueField5] = 0;
            _dr[p_ValueField6] = 0;
            _dr[p_ValueField7] = 0;
            _dr[p_TextField] = p_RowCaption;
            p_dtTemp.Rows.InsertAt(_dr, 0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void AddRow(ref DataTable p_dtTemp, string p_RowCaption, string p_TextField, string p_ValueField1, string p_ValueField2, string p_ValueField3, string p_ValueField4, string p_ValueField5, string p_ValueField6, string p_ValueField7, string p_ValueField8)
    {
        try
        {
            DataRow _dr;
            _dr = p_dtTemp.NewRow();
            _dr[p_ValueField1] = 0;
            _dr[p_ValueField2] = 0;
            _dr[p_ValueField3] = 0;
            _dr[p_ValueField4] = 0;
            _dr[p_ValueField5] = 0;
            _dr[p_ValueField6] = 0;
            _dr[p_ValueField7] = 0;
            _dr[p_ValueField8] = 0;
            _dr[p_TextField] = p_RowCaption;
            p_dtTemp.Rows.InsertAt(_dr, 0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region Search related Functions
    //Function for GetString for Where Condition of Selected items of ListBoxes 
    public static string GetSelListITEM(ListControl _lst, string _FieldName)
    {

        string _retVal = string.Empty;
        try
        {
            for (int i = 0; i < _lst.Items.Count; i++)
            {
                if (_lst.Items[i].Text.Trim().ToUpper() != "ALL" && _lst.Items[i].Selected)
                    _retVal += "'" + _lst.Items[i] + "',";
            }
            if (_retVal != string.Empty)
            {
                _retVal = _retVal.Substring(0, _retVal.Length - 1);
                _retVal = " " + _FieldName + " in (" + _retVal + ") And ";
            }

        }
        catch (Exception ex)
        {

            throw ex;

        }
        return _retVal;
    }
    public static string GetSelControlITEM(ListControl _lst, string _FieldName)
    {

        string _retVal = string.Empty;
        try
        {

            foreach (ListItem _li in _lst.Items)
            {
                if (_li.Selected == true)
                {
                    _retVal += "'" + _li.Value + "',";
                }
            }
            if (_retVal != string.Empty)
            {
                _retVal = _retVal.Substring(0, _retVal.Length - 1);
                _retVal = " " + _FieldName + " in (" + _retVal + ") And ";
            }
        }
        catch (Exception ex)
        {

            throw ex;

        }
        return _retVal;
    }
    //Function for Ckeck "ALL" value in LIST and SingleCombo  Box
    public static bool ChkAllAvail(object _Obj, string _cnttyp)
    {
        ListBox _lst;
        DropDownList _ddl;
        TextBox _txt;
        CheckBox _chk;
        bool retVal = false;

        try
        {


            if (_cnttyp.Trim().ToUpper().Equals("L"))//for listbox
            {
                _lst = (ListBox)_Obj;
                for (int i = 0; i < _lst.Items.Count; i++)
                {
                    if (_lst.Items[i].Text.Trim().ToUpper() == "ALL" && _lst.Items[i].Selected)
                        retVal = true;
                }
            }
            else if (_cnttyp.Trim().ToUpper().ToUpper().Equals("D"))//for ComboBox
            {
                _ddl = (DropDownList)_Obj;

                if (_ddl.SelectedItem.Text.Trim().ToUpper() == "ALL" || _ddl.SelectedItem.Text.Trim().ToUpper() == "")
                    retVal = true;

            }
            else if (_cnttyp.Trim().ToUpper().Equals("T"))//for TextBox
            {
                _txt = (TextBox)_Obj;

                if ((string.IsNullOrEmpty(_txt.Text.Trim())) || (_txt.Text.Trim() == ","))//due to Comma problem in Update panel
                    retVal = true;

            }
            else if (_cnttyp.Trim().ToUpper().Equals("C"))//for CheckBox
            {
                _chk = (CheckBox)_Obj;

                if (!_chk.Checked)
                    retVal = true;

            }

        }
        catch (Exception ex)
        {
            throw ex;

        }
        return retVal;
    }
    //Function for GetString For Where Condition for Single TextBox
    public static string GetSelTextEqualCondition(TextBox _txt, string _FieldName)
    {
        string _retVal = string.Empty;
        try
        {
            if (!ChkAllAvail(_txt, "T"))
            {
                _retVal += " " + "(" + _FieldName + " = " + "'" + _txt.Text.Replace(",", "").Trim() +"'" + ")" + " And ";
            }
        }
        catch (Exception ex)
        {

            throw ex;

        }
        return _retVal;
    }
    //Function for GetString For Where Condition for Single TextBox
    public static string GetSelTextLikecondotion(TextBox _txt, string _FieldName)
    {
        string _retVal = string.Empty;
        try
        {
            if (!ChkAllAvail(_txt, "T"))
            {
                _retVal += " " + "(" + _FieldName + " Like " + "'" + _txt.Text.Replace(",", "").Trim() +"%'" + ")" + " And ";
            }
        }
        catch (Exception ex)
        {

            throw ex;

        }
        return _retVal;
    }
    //Function for GetString For Where Condition for Single DropDownlist
    public static string GetSelComboEqualCondition(DropDownList _ddl, string _FieldName)
    {
        string _retVal = string.Empty;
        try
        {
            if (!ChkAllAvail(_ddl, "D"))
            {
                _retVal += " " + "(" + _FieldName + " = " + "'" + _ddl.SelectedValue + "'" + ")" + " And ";
            }


        }
        catch (Exception ex)
        {

            throw ex;

        }
        return _retVal;
    }
    public static string GetSelComboEqualConditionText(DropDownList _ddl, string _FieldName)
    {
        string _retVal = string.Empty;
        try
        {
            if (!ChkAllAvail(_ddl, "D"))
            {
                _retVal += " " + "(" + _FieldName + " = " + "'" + _ddl.SelectedItem.Text.Trim() + "'" + ")" + " And ";
            }
        }
        catch (Exception ex)
        {

            throw ex;

        }
        return _retVal;
    }
    //Function for GetString For Where Condition for Single CheckBox
    public static string GetSelCheckBoxEqualCondition(CheckBox _chk, string _FieldName, string _PassValue)
    {
        string _retVal = string.Empty;
        try
        {
            if (!ChkAllAvail(_chk, "C"))
            {
                _retVal += " " + "(" + _FieldName + " = " + "'" + _PassValue + "'" + ")" + " And ";
            }
        }
        catch (Exception ex)
        {

            throw ex;

        }
        return _retVal;
    }
    //Function for GetString For Batch Condition for Single DropDownlist
    public static string GetSelComboForBatch(DropDownList _ddl, string _FieldName)
    {
        string _retVal = string.Empty;
        try
        {
            if (!ChkAllAvail(_ddl, "D"))
            {
                _retVal += " " + "(" + "refSeqNo_StudentStandardMas in ( Select refSeqNo_StudentStandardMas From Student_Batch_Mas Where " + _FieldName + " = " + "'" + _ddl.SelectedValue + "'" + "))" + " And ";
            }
        }
        catch (Exception ex)
        {

            throw ex;

        }
        return _retVal;
    }
    #endregion

    #region Get Concated String
    //Dont Chnage due to its effect in Sp
    public static string ConcatedListControlItemsForGroupID(ListControl _lst)
    {

        string _retVal = string.Empty;

        try
        {
            foreach (ListItem _lstItem in _lst.Items)
            {
                if (_lstItem.Selected == true)
                {
                    string _strTempGroup = _lstItem.Value;
                    string[] _strGroup = _strTempGroup.Split('-');
                    string _strGroupID;
                    _strGroupID = _strGroup[0];
                    _retVal = _retVal + _strGroupID + ",";
                }
            }
            //if (!string.IsNullOrEmpty(_retVal))
            //{
            //    _retVal = _retVal.Substring(0, _retVal.Length - 1);
            //}
        }
        catch (Exception ex)
        {

            throw ex;

        }
        return _retVal;
    }
    //Dont Chnage due to its effect in Sp
    public static string ConcatedListControlItemsForNodeID(ListControl _lst)
    {

        string _retVal = string.Empty;

        try
        {
            foreach (ListItem _lstItem in _lst.Items)
            {
                if (_lstItem.Selected == true)
                {
                    string _strTempNode = _lstItem.Value;
                    string[] _strNode = _strTempNode.Split('-');
                    string _strNodeID;
                    _strNodeID = _strNode[1];
                    _retVal = _retVal + _strNodeID + ",";
                }
            }
            //if (!string.IsNullOrEmpty(_retVal))
            //{
            //    _retVal = _retVal.Substring(0, _retVal.Length - 1);
            //}
        }
        catch (Exception ex)
        {

            throw ex;

        }
        return _retVal;
    }
    //Dont Chnage due to its effect in Sp
    public static string ConcatedListControlItemsAdmissionYear(ListControl _lst)
    {

        string _retVal = string.Empty;
        try
        {
            foreach (ListItem _lstItem in _lst.Items)
            {
                if (_lstItem.Selected == true)
                {
                    _retVal = _retVal + _lstItem.Text + ",";
                }
            }
            //if (!string.IsNullOrEmpty(_retVal))
            //{
            //    _retVal = _retVal.Substring(0, _retVal.Length - 1);
            //}
        }
        catch (Exception ex)
        {

            throw ex;

        }
        return _retVal;
    }


    public static void fillListControl(ListControl _lst, string _strlist)
    {
        try
        {
            string[] _str;
            string _strtemp = _strlist;
            _str = _strtemp.Split(',');
            foreach (ListItem _lstItem in _lst.Items)
            {
                for (int i = 0; i < _str.Length; i++)
                {
                    if (_lstItem.Text.Trim() == _str[i].ToString().Trim())
                        _lstItem.Selected = true;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }

    public static void Unselected(ListControl _lstControl)
    {
        try
        {
            foreach (ListItem _lst in _lstControl.Items)
            {
                _lst.Selected = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// For Display no of Records of Grid
    /// </summary>
    /// <param name="p_gv"></param>
    /// <returns></returns>
    public static string GvTotalRowCount(GridView p_gv)
    {
        string _str = string.Empty;
        try
        {
            if (p_gv.DataSource != null)
            {
                if (p_gv.DataSource is DataTable)
                {
                    if (((DataTable)p_gv.DataSource).Rows.Count > 0)
                        _str = "Total Record(s) : " + ((DataTable)p_gv.DataSource).Rows.Count.ToString();
                    else
                        _str = "No record found.";
                }
                if (p_gv.DataSource is DataSet)
                {
                    if (((DataSet)p_gv.DataSource).Tables[0].Rows.Count > 0)
                        _str = "Total Record(s) : " + ((DataSet)p_gv.DataSource).Tables[0].Rows.Count.ToString();
                    else
                        _str = "No record found.";
                }
                if (p_gv.DataSource is DataView)
                {
                    if ((((DataView)p_gv.DataSource).ToTable()).Rows.Count > 0)
                        _str = "Total Record(s) : " + ((DataView)p_gv.DataSource).ToTable().Rows.Count.ToString();
                    else
                        _str = "No record found.";

                }

            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
        return _str;
    }

    #endregion

    #region SMS

    public static void Sms(string _Message, string _Mobile, string _SmsId)
    {
        StreamReader sr;
        StreamReader mMsg;
        string strURL;
        string mSesId;


        strURL = "http://sms.smartsmssolution.net.in/SendSms.aspx?username=caremax&password=88634581&to=" + _Mobile + "&from=RahulSirSD&message=" + _Message;
        WebRequest wReq = WebRequest.Create(strURL);
        wReq.Method = "GET";
        WebResponse wRes = wReq.GetResponse();
        sr = new StreamReader(wRes.GetResponseStream());
        mSesId = sr.ReadToEnd().Trim();
        sr.Close();
    }
    #endregion

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
    public static void sendMail(String fsTo, String fsFrom, String fsSubject, String fsBody, bool fbIsErrorMail, string fsAttachmentFile)
    {
        String lsSMTPUsername = string.Empty;
        String lsSMTPPassword = string.Empty;
        String lsSMTPServer = string.Empty;
        String lsSMTPEnableSSL = string.Empty;
        try
        {

            //DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary().TryGetValue("SMTPUsername", out lsSMTPUsername);
            //DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary().TryGetValue("SMTPPassword", out lsSMTPPassword);
            //DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary().TryGetValue("SMTPServer", out lsSMTPServer);
            //DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary().TryGetValue("SMTPEnableSSL", out lsSMTPEnableSSL);

            if (string.IsNullOrEmpty(fsFrom) == true)
            {
                fsFrom = Convert.ToString(lsSMTPUsername);
            }

            //if (DotNetNuke.Common.Globals.GetHostPortalSettings() != null)
            //    fsFrom = DotNetNuke.Common.Globals.GetHostPortalSettings().PortalName + "<" + fsFrom + ">";

            SmtpClient loSmtpClient;
            if (!String.IsNullOrEmpty(Convert.ToString(ConfigurationManager.AppSettings["Port"])))
                loSmtpClient = new SmtpClient(Convert.ToString(lsSMTPServer), Convert.ToInt32(ConfigurationManager.AppSettings["Port"]));
            else
                loSmtpClient = new SmtpClient(Convert.ToString(lsSMTPServer), 25);

            MailMessage loMailMessage = new System.Net.Mail.MailMessage(fsFrom, fsTo, fsSubject, fsBody);

            if (!string.IsNullOrEmpty(fsAttachmentFile))
                loMailMessage.Attachments.Add(new Attachment(fsAttachmentFile));

            loMailMessage.IsBodyHtml = true;
            loSmtpClient.Credentials = new System.Net.NetworkCredential(lsSMTPUsername, lsSMTPPassword);
            loSmtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            loSmtpClient.EnableSsl = lsSMTPEnableSSL == "Y" ? true : false;

            String lsBCCAddress = Convert.ToString(ConfigurationManager.AppSettings["BCCMailID"]);
            if (!String.IsNullOrEmpty(lsBCCAddress) && !fbIsErrorMail)
                loMailMessage.Bcc.Add(lsBCCAddress);

            loSmtpClient.Send(loMailMessage);
        }
        catch (Exception)
        {

        }
    }
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

    #region Image Generation
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
        else
        {
            loTransformedImage = CreateThumbnail(fsSourceImage, fiWidth, fiHeight);
            if (loTransformedImage != null)
                loTransformedImage.Save(fsDestinationImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        loTransformedImage.Dispose();
        loTransformedImage = null;
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
    //public static void sendErrorMail(String fsToAddress, String fsSubject, String fsBody, String fsBCCAddress)
    //{
    //    try
    //    {
    //        //String lsFromAddress = Convert.ToString(DotNetNuke.Common.Globals.HostSettings["SMTPUsername"]);
    //        //String lsFromPassword = Convert.ToString(DotNetNuke.Common.Globals.HostSettings["SMTPPassword"]);
    //        //SmtpClient loSmtpClient = new SmtpClient(Convert.ToString(DotNetNuke.Common.Globals.HostSettings["SMTPServer"]));
    //        //string lsFrom = DotNetNuke.Common.Globals.GetHostPortalSettings().PortalName + "<" + Convert.ToString(DotNetNuke.Common.Globals.HostSettings["SMTPUsername"]) + ">";
    //        MailMessage loMailMessage = new MailMessage(lsFrom, fsToAddress, fsSubject, fsBody);
    //        loMailMessage.IsBodyHtml = true;
    //        loMailMessage.BodyEncoding = Encoding.GetEncoding("utf-8");

    //        if (!String.IsNullOrEmpty(fsBCCAddress))
    //            loMailMessage.Bcc.Add(fsBCCAddress);

    //        loSmtpClient.Credentials = new System.Net.NetworkCredential(lsFromAddress, lsFromPassword);
    //        loSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
    //        loSmtpClient.EnableSsl = Convert.ToString(DotNetNuke.Common.Globals.HostSettings["SMTPEnableSSL"]) == "Y" ? true : false;
    //        loSmtpClient.Send(loMailMessage);
    //    }
    //    catch (Exception feException)
    //    {
    //        throw feException;
    //    }
    //}
    #endregion
}
