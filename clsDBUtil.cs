using System;
using System.Collections;
using System.Text;

namespace libCommon
{
	public class clsDBUtil
	{
		private QueryType qtProc;

		public ArrayList arrCols;

		public ArrayList arrVals;

		public ArrayList arrWCols;

		public ArrayList arrWVals;

		public ArrayList arrGCols;

		public string Table;

		public string Order;

		public int Page;

		public int PageCnt;

		public QueryType Type
		{
			get
			{
				return this.qtProc;
			}
			set
			{
				this.qtProc = value;
			}
		}

		public clsDBUtil()
		{
			this.arrCols = new ArrayList();
			this.arrVals = new ArrayList();
			this.arrWCols = new ArrayList();
			this.arrWVals = new ArrayList();
			this.arrGCols = new ArrayList();
			this.Order = "";
			this.Page = 0;
			this.PageCnt = 0;
		}

		public string BuildQuery()
		{
			int i;
			string str;
			string str1 = "";
			StringBuilder stringBuilder = new StringBuilder();
			clsUtil _clsUtil = new clsUtil();
			str1 = this.ValidateMe();
			if (!_clsUtil.Left(str1, 4).Equals("FAIL"))
			{
				if (this.qtProc == QueryType.SELECT)
				{
					stringBuilder.Append("SELECT ");
					if ((this.Page == 0 ? false : this.PageCnt != 0))
					{
						int page = this.Page * this.PageCnt;
						stringBuilder.Append(string.Concat(" TOP ", page.ToString()));
					}
					int num = 0;
					for (i = 0; i < this.arrCols.Count; i++)
					{
						if (this.arrCols[i].ToString().Length > 0)
						{
							if (num > 0)
							{
								stringBuilder.Append(", ");
							}
							stringBuilder.Append(this.arrCols[i].ToString());
							num++;
						}
					}
					stringBuilder.Append(" FROM ");
					stringBuilder.Append(this.Table);
					stringBuilder.Append(" WHERE 1 = 1 ");
					for (i = 0; i < this.arrWCols.Count; i++)
					{
						stringBuilder.Append(" AND ");
						stringBuilder.Append(this.arrWCols[i].ToString());
						stringBuilder.Append(string.Concat(" = '", this.arrWVals[i].ToString(), "'"));
					}
					if (this.arrGCols.Count > 0)
					{
						for (i = 0; i < this.arrGCols.Count; i++)
						{
							if (i > 0)
							{
								stringBuilder.Append(", ");
							}
							stringBuilder.Append(this.arrGCols[i].ToString());
						}
					}
					stringBuilder.Append(this.Order);
				}
				if (this.qtProc == QueryType.INSERT)
				{
					stringBuilder.Append("INSERT INTO ");
					stringBuilder.Append(this.Table);
					stringBuilder.Append("(");
					for (i = 0; i < this.arrCols.Count; i++)
					{
						if (i > 0)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(this.arrCols[i].ToString());
					}
					stringBuilder.Append(")");
					stringBuilder.Append(" VALUES ");
					stringBuilder.Append("(");
					for (i = 0; i < this.arrVals.Count; i++)
					{
						if (i > 0)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(this.arrVals[i].ToString());
					}
					stringBuilder.Append(")");
				}
				if (this.qtProc == QueryType.UPDATE)
				{
					stringBuilder.Append("UPDATE ");
					stringBuilder.Append(this.Table);
					stringBuilder.Append(" SET ");
					for (i = 0; i < this.arrCols.Count; i++)
					{
						if (i > 0)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(this.arrCols[i].ToString());
						stringBuilder.Append(" = ");
						stringBuilder.Append(this.arrVals[i].ToString());
					}
					stringBuilder.Append(" WHERE 1 = 1 ");
					for (i = 0; i < this.arrWCols.Count; i++)
					{
						stringBuilder.Append(" AND ");
						stringBuilder.Append(this.arrWCols[i].ToString());
						stringBuilder.Append(string.Concat(" = '", this.arrWVals[i].ToString(), "'"));
					}
				}
				if (this.qtProc == QueryType.DELETE)
				{
					stringBuilder.Append("DELETE FROM ");
					stringBuilder.Append(this.Table);
					stringBuilder.Append(" WHERE 1 = 1 ");
					for (i = 0; i < this.arrWCols.Count; i++)
					{
						stringBuilder.Append(" AND ");
						stringBuilder.Append(this.arrWCols[i].ToString());
						stringBuilder.Append(string.Concat(" = '", this.arrWVals[i].ToString(), "'"));
					}
				}
				str = stringBuilder.ToString();
			}
			else
			{
				str = str1;
			}
			return str;
		}

		public string ValidateMe()
		{
			string str = "";
			if ((this.Type == QueryType.SELECT ? true : this.arrCols.Count != this.arrVals.Count))
			{
				str = "FAIL:Unmatch Cols and Vals";
			}
			if (this.arrWCols.Count != this.arrWVals.Count)
			{
				str = "FAIL:Unmatch WhereCols and WhereVals";
			}
			if ((this.Type != QueryType.SELECT ? false : this.arrVals.Count > 0))
			{
				str = "FAIL:SELECT can't set values";
			}
			if ((this.Type == QueryType.SELECT ? false : this.arrGCols.Count > 0))
			{
				str = "FAIL:Group by Option is Only for SELECT";
			}
			if ((this.Type == QueryType.SELECT ? false : this.Order.Length > 0))
			{
				str = "FAIL:Order by Option is Only for SELECT";
			}
			if (this.Type == QueryType.INSERT)
			{
				if ((this.arrWCols.Count > 0 ? true : this.arrWVals.Count > 0))
				{
					str = "FAIL:INSERT can't set where phrase";
				}
			}
			if (this.Type != QueryType.INSERT)
			{
				if ((this.arrWCols.Count == 0 ? true : this.arrWVals.Count == 0))
				{
					str = "FAIL:SELECT, UPDATE, DELETE can't excute without where phrase";
				}
			}
			if (str.Length == 0)
			{
				str = "SUCCESS";
			}
			return str;
		}
	}
}