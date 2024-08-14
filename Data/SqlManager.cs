using Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Xml;
using ConfigurationManager = System.Configuration.ConfigurationManager;


namespace Data
{
    public class SqlManager : IDisposable
    {
        private SqlConnection _connection;

        public SqlConnection Connection
        {
            get { return _connection; }
        }

        private SqlCommand _command;

        public SqlCommand Command
        {
            get { return _command; }
        }

        private SqlTransaction _transaction;

        public SqlTransaction Transaction
        {
            get { return _transaction; }
        }

        private string _authority;

        public string Authority
        {
            get { return _authority; }
            set { _authority = value; }
        }

        private List<SqlParameter> _parameters = new List<SqlParameter>();

        public List<SqlParameter> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public SqlManager(string constr)
        {
            //var constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            _connection = new SqlConnection(constr);
            _command = new SqlCommand();
            _connection.Open();
        }



        public int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, CommandType.Text);
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            _command.Connection = _connection;
            _command.Transaction = _transaction;
            _command.CommandType = commandType;
            _command.CommandText = commandText;
            _command.Parameters.Clear();
            _command.Parameters.AddRange(_parameters.ToArray());



            return _command.ExecuteNonQuery();
        }

        public object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, CommandType.Text);
        }

        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            _command.Connection = _connection;
            _command.Transaction = _transaction;
            _command.CommandType = commandType;
            _command.CommandText = commandText;
            _command.Parameters.Clear();
            _command.Parameters.AddRange(_parameters.ToArray());

            return _command.ExecuteScalar();
        }

        public IDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, CommandType.Text);
        }

        public IDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            _command.Connection = _connection;
            _command.CommandType = commandType;
            _command.CommandText = commandText;
            _command.Parameters.Clear();
            _command.Parameters.AddRange(_parameters.ToArray());

            return _command.ExecuteReader();
        }

        public XmlReader ExecuteXml(string commandText)
        {
            return ExecuteXml(commandText, CommandType.Text);
        }

        public XmlReader ExecuteXml(string commandText, CommandType commandType)
        {
            _command.Connection = _connection;
            _command.CommandType = commandType;
            _command.CommandText = commandText;
            _command.Parameters.Clear();
            _command.Parameters.AddRange(_parameters.ToArray());

            return _command.ExecuteXmlReader();
        }

        public DataTable ExecuteDataTable(string commandText)
        {
            return ExecuteDataTable(commandText, CommandType.Text);
        }

        public DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            _command.Connection = _connection;
            _command.CommandType = commandType;
            _command.CommandText = commandText;
            _command.Parameters.Clear();
            _command.Parameters.AddRange(_parameters.ToArray());

            var dt = new DataTable();
            dt.Load(_command.ExecuteReader());

            return dt;
        }

        public void AddParameter(string paraName, object objectValue)
        {
            AddParameter(paraName, objectValue, ParameterDirection.Input);
        }

        public void AddParameter(string paraName, object objectValue, ParameterDirection direction)
        {
            var para = new SqlParameter();
            para.ParameterName = paraName;
            para.Direction = direction;
            para.Value = objectValue;
            _parameters.Add(para);
        }

        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }

            _connection = null;
            _command = null;
            _transaction = null;
        }
    }
}