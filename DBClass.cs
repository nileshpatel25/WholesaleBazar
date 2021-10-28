using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.CSharp;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.Mail;
using System.IO;
using System.Linq;

namespace DiligenceVault
{
    public class DBClass
    {
        public DBClass()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #region "Globle Variable Declaration"

        public SqlConnection pubGeneralConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
        public string StringConn;
        public string SQLMessage;
        public SqlCommand msqldbCommand;
        public SqlDataAdapter msqldbAdapter;
        public DataSet msqldbDS;
        public DataTable msqldbDT;
        public SqlDataReader msqldbDR;
        public SqlTransaction msqldbTransaction;
        private string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        #endregion


        #region "Open connection"
        public Boolean OpenConnection()
        {
            //'----------------------------------------------------------------
            //'This sub will check the existance and status of connection
            //'and try to open the connection
            //'If connection opens successfully returns true else false
            //'----------------------------------------------------------------
            //'
            try
            {
                if (pubGeneralConn != null)
                {
                    if (pubGeneralConn.State != ConnectionState.Open)
                    {
                        pubGeneralConn.Open();
                        return true;
                    }
                    else
                    {
                        return true;
                    }

                }
                else
                {

                    pubGeneralConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WholesaleBazarConnectionString"].ToString());
                    pubGeneralConn.Open();
                    return true;
                }
            }
            catch (System.Exception ex)
            {

                return false;
            }
        }
        #endregion

        #region " Connection Close "
        public void CloseConnection()
        {
            //'----------------------------------------------------------
            //' This function always close the connection and dispose it
            //'----------------------------------------------------------

            try
            {
                if (pubGeneralConn.State != ConnectionState.Closed)
                {
                    pubGeneralConn.Close();
                    pubGeneralConn.Dispose();
                }

            }
            catch (System.Exception ex)
            {

            }
        }
        #endregion

        #region " Execute Query "

        public Boolean GetExecuteQuery(string strSQL)
        {
            //'----------------------------------------------------------
            //' This function try to execute query and returns true if 
            //' Query execution is sucessfull
            //'----------------------------------------------------------

            try
            {
                //'Check input
                if (strSQL == "")
                    return false;

                //'Check for database connection
                if (OpenConnection() == false)
                {
                    SQLMessage = "Database could not be connected";
                    return false;
                }

                //'Create command object and execute SQL

                msqldbCommand = new SqlCommand();
                msqldbCommand.Connection = pubGeneralConn;
                msqldbCommand.CommandText = strSQL;
                msqldbCommand.ExecuteNonQuery();
                return true;
            }
            catch (System.Exception ex)
            {
                SQLMessage = ex.Message;
                return false;
            }
        }
        #endregion

        #region " Get Scalar Value "
        public Boolean GetScalarValue(string strSQL, out Int64 ReturnValue)
        {

            try
            {
                //'Default returnvalue is zero
                ReturnValue = 0;

                //'Check input

                if (strSQL == "")
                    return false;

                //'Check for database connection

                if (OpenConnection() == false)
                {
                    SQLMessage = "Database could not be connected";
                    return false;
                }

                //'Create command object and execute SQL
                msqldbCommand = new SqlCommand();
                msqldbCommand.Connection = pubGeneralConn;
                msqldbCommand.CommandText = strSQL;
                //ReturnValue = (long)moledbCommand.ExecuteScalar();
                ReturnValue = System.Convert.ToInt64(msqldbCommand.ExecuteScalar());
                return true;
            }
            catch (System.Exception ex)
            {
                SQLMessage = ex.Message;
                ReturnValue = 0;
                return false;
            }
        }
        #endregion

        #region " Get Data Table "
        public DataTable GetDataTable(string strSQL)
        {
            //'----------------------------------------------------------------
            //' This function accept query string and return datatable
            //' If anything goes wrong, it returns nothing
            //' Usage: if GetDataTable("Select * from tab") is nothing then exit
            //'----------------------------------------------------------------

            try
            {

                //'Check input
                if (strSQL == "")
                    return null;


                //'Check for database connection

                if (OpenConnection() == false)
                {
                    SQLMessage = "Database could not be connected";
                    return null;
                }

                msqldbDT = new DataTable();
                msqldbCommand = new SqlCommand(strSQL, pubGeneralConn);
                msqldbAdapter = new SqlDataAdapter(msqldbCommand);
                msqldbAdapter.Fill(msqldbDT);
                return msqldbDT;
            }
            catch (System.Exception ex)
            {
                SQLMessage = ex.Message;
                return null;
            }

        }
        #endregion

        #region " Get Data Set "
        public DataSet GetDataSet(string strSQL)
        {
            //'----------------------------------------------------------------
            //' This function accept query string and return dataset
            //' If anything goes wrong, it returns nothing
            //' Usage: if GetDataSet("Select * from tab") is nothing then exit
            //'----------------------------------------------------------------

            try
            {

                //  'Check input
                if (strSQL == "")
                    return null;

                //'Check for database connection
                if (OpenConnection() == false)
                {
                    SQLMessage = "Database could not be connected";
                    return null;
                }

                msqldbDS = new DataSet();
                msqldbCommand = new SqlCommand(strSQL, pubGeneralConn);
                msqldbAdapter = new SqlDataAdapter(msqldbCommand);
                msqldbAdapter.Fill(msqldbDS);
                return msqldbDS;
            }
            catch (System.Exception ex)
            {
                SQLMessage = ex.Message;
                return null;
            }
        }
        #endregion

        public DataSet ExecuteCommand(string procedureName, NameValueCollection sqlParams)
        {
            // create the database connection
            pubGeneralConn = new SqlConnection(_connectionString);

            // open my database connection
            pubGeneralConn.Open();

            try
            {
                SqlCommand command = new SqlCommand();
                SqlDataAdapter dataAdapter;
                DataSet tempDataSet = new DataSet();

                // set the connection for my command
                command.Connection = pubGeneralConn;

                // set the timeout for the command object
                //command.CommandTimeout

                // Set the command type to a stored procedure
                command.CommandType = CommandType.StoredProcedure;

                // set which procedure to use
                command.CommandText = procedureName;

                // set the paramaters
                addParams(ref command, sqlParams);

                // execute the sql statement
                dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(tempDataSet);


                // return the tempDataSet
                return tempDataSet;
            }
            finally
            {
                pubGeneralConn.Close();
            }
        }

        public DataSet ExecuteDataSetCommand(string procedureName, SqlParameter[] sqlParams)
        {
            // create the database connection
            pubGeneralConn = new SqlConnection(_connectionString);

            // open my database connection
         
         //  pubGeneralConn.Open();

            try
            {
                SqlCommand command = new SqlCommand();
                SqlDataAdapter dataAdapter;
                DataSet tempDataSet = new DataSet();


                // set the connection for my command
                command.Connection = pubGeneralConn;

                // set the timeout for the command object
                //command.CommandTimeout

                // Set the command type to a stored procedure
                command.CommandType = CommandType.StoredProcedure;

                // set which procedure to use
                command.CommandText = procedureName;

                // set the paramaters
                addParams(ref command, sqlParams);

                // execute the sql statement
                dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(tempDataSet);

                return tempDataSet;
            }
            catch
            {
             //   DataSet tempDataSet = new DataSet();
                return null;
            }
             finally
            {
                pubGeneralConn.Close();
            }
        }
        public DataSet ExecuteCommandTblName(string tblName, string procedureName, SqlParameter[] sqlParams)
        {
            // create the database connection
            pubGeneralConn = new SqlConnection(_connectionString);

            // open my database connection
            pubGeneralConn.Open();

            try
            {
                SqlCommand command = new SqlCommand();
                SqlDataAdapter dataAdapter;
                DataSet tempDataSet = new DataSet();


                // set the connection for my command
                command.Connection = pubGeneralConn;

                // set the timeout for the command object
                //command.CommandTimeout

                // Set the command type to a stored procedure
                command.CommandType = CommandType.StoredProcedure;

                // set which procedure to use
                command.CommandText = procedureName;

                // set the paramaters
                addParams(ref command, sqlParams);

                // execute the sql statement
                dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(tempDataSet, tblName);

                return tempDataSet;
            }
            finally
            {
                pubGeneralConn.Close();
            }
        }

        public DataSet ExecuteCommand(string procedureName)
        {
            // create the database connection
            pubGeneralConn = new SqlConnection(_connectionString);

            // open my database connection
            pubGeneralConn.Open();

            try
            {
                SqlCommand command = new SqlCommand();
                SqlDataAdapter dataAdapter;
                DataSet tempDataSet = new DataSet();

                // set the connection for my command
                command.Connection = pubGeneralConn;

                // set the timeout for the command object
                //command.CommandTimeout

                // Set the command type to a stored procedure
                command.CommandType = CommandType.StoredProcedure;

                // set which procedure to use
                command.CommandText = procedureName;

                // execute the sql statement
                dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(tempDataSet);



                // return the data set
                return tempDataSet;
            }
            finally
            {
                pubGeneralConn.Close();
            }
        }

        public void ExecuteNonQuery(string procedureName, SqlParameter[] sqlParams)
        {
            // create the database connection
            pubGeneralConn = new SqlConnection(_connectionString);

            // open my database connection
            pubGeneralConn.Open();

            try
            {
                SqlCommand command = new SqlCommand();
                SqlDataAdapter dataAdapter;
                DataSet tempDataSet = new DataSet();

                // set the connection for my command
                command.Connection = pubGeneralConn;

                // set the timeout for the command object
                //command.CommandTimeout

                // Set the command type to a stored procedure
                command.CommandType = CommandType.StoredProcedure;

                // set which procedure to use
                command.CommandText = procedureName;

                // set the paramaters
                addParams(ref command, sqlParams);

                try
                {
                    // execute the sql statement
                    command.ExecuteNonQuery();
                }
                catch
                {
                    string s = string.Empty;
                }

            }
            finally
            {
                pubGeneralConn.Close();
            }
        }

        public object ExecuteScalarCommand(string procedureName, SqlParameter[] sqlParams)
        {
            //using (TransactionScope ts = new TransactionScope())
            //{
            // create the database connection
            pubGeneralConn = new SqlConnection(_connectionString);

            // open my database connection
            pubGeneralConn.Open();

            try
            {
                SqlCommand command = new SqlCommand();

                // set the connection for my command
                command.Connection = pubGeneralConn;

                // Set the command type to a stored procedure
                command.CommandType = CommandType.StoredProcedure;

                // set which procedure to use
                command.CommandText = procedureName;

                // set the paramaters
                addParams(ref command, sqlParams);

                //execute the command and return the rows affected
                object returnValue = command.ExecuteScalar();
                // complete the transaction
                //ts.Complete();
                // return the value
                return returnValue;
            }
            finally
            {
                // close the db connection no matter what
                pubGeneralConn.Close();
            }
            //}
        }

        public object ExecuteScalarCommand(string procedureName)
        {
            // create the database connection
            pubGeneralConn = new SqlConnection(_connectionString);

            // open my database connection
            pubGeneralConn.Open();

            try
            {
                SqlCommand command = new SqlCommand();

                // set the connection for my command
                command.Connection = pubGeneralConn;

                // Set the command type to a stored procedure
                command.CommandType = CommandType.StoredProcedure;

                // set which procedure to use
                command.CommandText = procedureName;

                //execute the command and return the rows affected
                object returnValue = command.ExecuteScalar();

                // return the value
                return returnValue;
            }
            finally
            {
                // close the db connection no matter what
                pubGeneralConn.Close();
            }
        }

        private void addParams(ref SqlCommand command, NameValueCollection sqlParams)
        {
            for (int i = 0; i < sqlParams.Count; i++)
            {
                command.Parameters.AddWithValue(sqlParams.Keys[i], sqlParams[i]);
            }
        }

        private void addParams(ref SqlCommand command, SqlParameter[] sqlParams)
        {
            for (int i = 0; i < sqlParams.Length; i++)
            {
                command.Parameters.Add(sqlParams[i]);
            }
        }



        //////////////////////////////
        public string fnExecuteScalar(string strSql)
        {
            string strRtn = "";
            try
            {
                pubGeneralConn.Open();
                msqldbCommand = new SqlCommand(strSql, pubGeneralConn);
                msqldbDR = msqldbCommand.ExecuteReader();
                if (msqldbDR.Read())
                {
                    strRtn = msqldbDR[0].ToString().Trim();
                }
            }
            catch (Exception ex)
            { //strRtn = ex.ToString(); 
            }
            finally
            {
                if (pubGeneralConn != null) pubGeneralConn.Close();
                if (msqldbDR != null) msqldbDR.Close();
            }
            return strRtn;
        }


        public void SendMail(string mailmsg, string strsubject, string strfrom, string strto, string strcc, string strbcc, string strattachment)
        {
            try
            {
                SmtpMail.SmtpServer = System.Configuration.ConfigurationManager.AppSettings["MailServer"].ToString();
                MailMessage msg = new MailMessage();
                msg.Body = mailmsg;
                msg.To = strto;
                msg.From = strfrom;
                msg.Subject = strsubject;
                if (strcc != "")
                    msg.Cc = strcc;
                if (strbcc != "")
                    msg.Bcc = strbcc;
                if (strattachment != "")
                {
                    MailAttachment att = new MailAttachment(strattachment);
                    msg.Attachments.Add(att);
                }
                msg.BodyFormat = MailFormat.Html;
                SmtpMail.Send(msg);
            }
            catch (Exception ex)
            {
                //Response.Write(ex.ToString());
            }
            finally { }
        }


        public string formatDate(string date)
        {
            return string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(date));
        }
        public string formatDateinSQLFormat(string date)
        {
            return string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(date));
        }
    }
}