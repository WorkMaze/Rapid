using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Rapid.SQL.Connectors
{
    public class SQLConnector
    {
        string _connectionString;

        public SQLConnector(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<List<KeyValuePair<string, object>>> GetRecords(string tableName,List<KeyValuePair<string,string>> parameters)
        {

            List <List<KeyValuePair<string, object>>> instances = null;

            SqlConnection connection = null;

            try
            {
                string text = "SELECT * FROM [" + tableName + "]";

                if(parameters != null && parameters.Count > 0)
                {
                    int i = 0;

                    foreach(KeyValuePair<string,string> parameter in parameters)
                    {
                        if (i == 0)
                            text = text + " WHERE [" + parameter.Key + "] = '" + parameter.Value + "'";
                        else
                            text = text + " AND [" + parameter.Key + "] = '" + parameter.Value + "'";

                        i++;
                    }
                }

                connection = new SqlConnection(_connectionString);

                SqlCommand command = new SqlCommand(text, connection);
                command.CommandType = CommandType.Text;

                
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();



                while (reader.Read())
                {
                    List<KeyValuePair<string, object>> instance = new List<KeyValuePair<string, object>>();

                    for (int i=0; i<reader.FieldCount; i++)
                    {
                        
                        string name = reader.GetName(i);
                        object value = reader.GetValue(i);
                        KeyValuePair<string, object> pair = new KeyValuePair<string, object>(name,value);
                        
                        instance.Add(pair);
                    }

                    if (instances == null)
                        instances = new List<List<KeyValuePair<string, object>>>();

                    instances.Add(instance);
                }

              

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return instances;
        }

        public void DeleteRecords(string tableName, List<KeyValuePair<string, string>> parameters)
        {
            SqlConnection connection = null;

            try
            {
                string text = "DELETE FROM [" + tableName + "]";

                if (parameters != null && parameters.Count > 0)
                {
                    int i = 0;

                    foreach (KeyValuePair<string, string> parameter in parameters)
                    {
                        if (i == 0)
                            text = text + " WHERE [" + parameter.Key + "] = '" + parameter.Value + "'";
                        else
                            text = text + " AND [" + parameter.Key + "] = '" + parameter.Value + "'";

                        i++;
                    }
                }

                connection = new SqlConnection(_connectionString);

                SqlCommand command = new SqlCommand(text, connection);
                command.CommandType = CommandType.Text;


                connection.Open();

                command.ExecuteNonQuery();              



            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            
        }


        public void UpdateRecords(string tableName, List<KeyValuePair<string, string>> parameters,
            Dictionary<string, string> updateSet)
        {
            SqlConnection connection = null;

            try
            {
                string text = "UPDATE [" + tableName + "]";


                if (updateSet != null && updateSet.Count > 0)
                {
                    int i = 0;

                    foreach (KeyValuePair<string, string> set in updateSet)
                    {
                        if (i == 0)
                            text = text + " SET [" + set.Key + "] = '" + set.Value + "'";
                        else
                            text = text + " , [" + set.Key + "] = '" + set.Value + "'";

                        i++;
                    }
                }
                else
                    throw new Exception("Field(s) not specified for update");

                if (parameters != null && parameters.Count > 0)
                {
                    int i = 0;

                    foreach (KeyValuePair<string, string> parameter in parameters)
                    {
                        if (i == 0)
                            text = text + " WHERE [" + parameter.Key + "] = '" + parameter.Value + "'";
                        else
                            text = text + " AND [" + parameter.Key + "] = '" + parameter.Value + "'";

                        i++;
                    }
                }

                connection = new SqlConnection(_connectionString);

                SqlCommand command = new SqlCommand(text, connection);
                command.CommandType = CommandType.Text;


                connection.Open();

                command.ExecuteNonQuery();



            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

        }

        
        public void InsertSingleRecord(string tableName, Dictionary<string, string> updateSet)
        {
            SqlConnection connection = null;

            try
            {
                string text = "INSERT INTO [" + tableName + "] (";


                if (updateSet != null && updateSet.Count > 0)
                {
                    int i = 0;
                    foreach (KeyValuePair<string, string> set in updateSet)
                    {
                        if (i == (updateSet.Count - 1))
                            text = text + "["  + set.Key + "]) VALUES (";
                        else
                            text = text + "[" + set.Key + "],";
                        i++;
                    }


                    i = 0;
                    foreach (KeyValuePair<string, string> set in updateSet)
                    {
                        if (i == (updateSet.Count - 1))
                            text = text + "'" +set.Value + "')";
                        else
                            text = text + "'" + set.Value + "',";
                        i++;
                    }
                }
                else
                    throw new Exception("Field(s) not specified for update");

                

                connection = new SqlConnection(_connectionString);

                SqlCommand command = new SqlCommand(text, connection);
                command.CommandType = CommandType.Text;


                connection.Open();

                command.ExecuteNonQuery();



            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

        }
    }
}
