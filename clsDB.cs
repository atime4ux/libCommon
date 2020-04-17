using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;

namespace libCommon
{
	public class clsDB
	{
		public clsDB()
		{
		}

		/// <summary>
		/// dscheck
		/// </summary>
		/// <param name="DS"></param>
		/// <returns></returns>
		public bool DScheck(DataSet DS)
		{
			bool flag;
			if (DS.Tables.Count == 0)
			{
				flag = false;
			}
			else
			{
				flag = (DS.Tables[0].Rows.Count == 0 ? false : true);
			}
			return flag;
		}

		public DataSet ExecuteDSQuery(SqlConnection dbCon, string Query)
		{
			DataSet dataSet;
			DataSet dataSet1 = new DataSet();
			clsUtil _clsUtil = new clsUtil();
			if (dbCon.State != ConnectionState.Open)
			{
				_clsUtil.writeLog("Connection FAIL");
				dataSet = new DataSet();
			}
			else
			{
				try
				{
					SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
					{
						SelectCommand = new SqlCommand(Query, dbCon)
					};
					sqlDataAdapter.Fill(dataSet1);
					_clsUtil.writeLog(string.Concat("DB SUCCESS : ", Query));
					dataSet = dataSet1;
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					_clsUtil.writeLog(string.Concat("DBEXEC ERR : ", Query));
					_clsUtil.writeLog(string.Concat("DBEXEC ERR : ", exception.ToString()));
					dataSet = new DataSet();
				}
			}
			return dataSet;
		}

		public DataSet ExecuteDSQuery(SqlConnection dbCon, SqlTransaction tran, string Query)
		{
			DataSet dataSet;
			DataSet dataSet1 = new DataSet();
			clsUtil _clsUtil = new clsUtil();
			if (dbCon.State != ConnectionState.Open)
			{
				_clsUtil.writeLog("Connection FAIL");
				dataSet = new DataSet();
			}
			else
			{
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
				{
					SelectCommand = new SqlCommand(Query, dbCon, tran)
				};
				sqlDataAdapter.Fill(dataSet1);
				_clsUtil.writeLog(string.Concat("DB SUCCESS : ", Query));
				dataSet = dataSet1;
			}
			return dataSet;
		}

		public string ExecuteNonQuery(SqlConnection dbCon, string Query)
		{
			string str;
			clsUtil _clsUtil = new clsUtil();
			if (dbCon.State != ConnectionState.Open)
			{
				str = "FAIL::Connection Not Opened";
			}
			else
			{
				SqlCommand sqlCommand = new SqlCommand(Query, dbCon);
				try
				{
					sqlCommand.ExecuteNonQuery();
					_clsUtil.writeLog(string.Concat("DB SUCCESS : ", Query));
					str = "SUCCESS";
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					_clsUtil.writeLog(string.Concat("DBEXECN ERR : ", Query));
					str = string.Concat("FAIL::", exception.ToString());
				}
			}
			return str;
		}

		public string ExecuteNonQuery(SqlConnection dbCon, SqlTransaction tran, string Query)
		{
			string str;
			clsUtil _clsUtil = new clsUtil();
			if (dbCon.State != ConnectionState.Open)
			{
				str = "FAIL::Connection Not Opened";
			}
			else
			{
				(new SqlCommand(Query, dbCon, tran)).ExecuteNonQuery();
				_clsUtil.writeLog(string.Concat("DB SUCCESS : ", Query));
				str = "SUCCESS";
			}
			return str;
		}

		public SqlConnection GetConnection()
		{
			SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["conStr"]);
			try
			{
				sqlConnection.Open();
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine(exception.ToString());
			}
			return sqlConnection;
		}

		public SqlConnection GetConnection_custum(string Str)
		{
			SqlConnection sqlConnection = new SqlConnection(Str);
			try
			{
				sqlConnection.Open();
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine(exception.ToString());
			}
			return sqlConnection;
		}
	}
}