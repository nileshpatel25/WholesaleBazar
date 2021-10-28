﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;



[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="WholesaleBazar")]
public partial class LoginManagementDataContext : System.Data.Linq.DataContext
{
	
	private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
	
  #region Extensibility Method Definitions
  partial void OnCreated();
  #endregion
	
	public LoginManagementDataContext() : 
			base(global::System.Configuration.ConfigurationManager.ConnectionStrings["WholesaleBazarConnectionString"].ConnectionString, mappingSource)
	{
		OnCreated();
	}
	
	public LoginManagementDataContext(string connection) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public LoginManagementDataContext(System.Data.IDbConnection connection) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public LoginManagementDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public LoginManagementDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.loginProcess")]
	public ISingleResult<loginProcessResult> loginProcess([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(200)")] string stUserName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(200)")] string stPassword)
	{
		IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), stUserName, stPassword);
		return ((ISingleResult<loginProcessResult>)(result.ReturnValue));
	}
}

public partial class loginProcessResult
{
	
	private int _inEntityId;
	
	private string _stUserName;
	
	private System.Nullable<int> _inEntityTypeId;
	
	private string _stEntityName;
	
	private string _stEntityTypeName;
	
	public loginProcessResult()
	{
	}
	
	[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_inEntityId", DbType="Int NOT NULL")]
	public int inEntityId
	{
		get
		{
			return this._inEntityId;
		}
		set
		{
			if ((this._inEntityId != value))
			{
				this._inEntityId = value;
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_stUserName", DbType="NVarChar(150)")]
	public string stUserName
	{
		get
		{
			return this._stUserName;
		}
		set
		{
			if ((this._stUserName != value))
			{
				this._stUserName = value;
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_inEntityTypeId", DbType="Int")]
	public System.Nullable<int> inEntityTypeId
	{
		get
		{
			return this._inEntityTypeId;
		}
		set
		{
			if ((this._inEntityTypeId != value))
			{
				this._inEntityTypeId = value;
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_stEntityName", DbType="NVarChar(250)")]
	public string stEntityName
	{
		get
		{
			return this._stEntityName;
		}
		set
		{
			if ((this._stEntityName != value))
			{
				this._stEntityName = value;
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_stEntityTypeName", DbType="VarChar(100)")]
	public string stEntityTypeName
	{
		get
		{
			return this._stEntityTypeName;
		}
		set
		{
			if ((this._stEntityTypeName != value))
			{
				this._stEntityTypeName = value;
			}
		}
	}
}
#pragma warning restore 1591