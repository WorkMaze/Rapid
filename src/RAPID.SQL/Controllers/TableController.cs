using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rapid.SQL.Connectors;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Rapid.SQL.Controllers
{
    [Route("api/[controller]")]
    public class TableController : Controller
    {
        string _connectionString;

        public TableController(IOptions<SQLModel> mySqlModel)
        {
            _connectionString = mySqlModel.Value.ConnectionString;
        }
        
        [HttpGet]
        public string Get()
        {
            return "Table name not specified.";
        }

        
        [HttpGet("{tablename}")]
        public ActionResult Get(string tablename)
        {
            
            ActionResult result = null;

            try
            {
                List<KeyValuePair<string, string>> parameters = GetParameters();

                SQLConnector connector = new SQLConnector(_connectionString); 
                                              

                List<List<KeyValuePair<string,object>>> records = connector.GetRecords(tablename, parameters);

                List<object> objectList = null;


                foreach(List<KeyValuePair<string,object>> record in records)
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();                
                                       
                    foreach (KeyValuePair<string, object> pair in record)
                    {
                        values.Add(pair.Key, pair.Value);
                    }

                    string jsonString = JsonConvert.SerializeObject(values);

                    if (objectList == null)
                        objectList = new List<object>();

                    objectList.Add(JsonConvert.DeserializeObject(jsonString));
                }
                

                result = Ok(objectList);
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new { Error = ex.Message });
            }

            return result;
        }

        
        [HttpPost("{tablename}")]
        public ActionResult Post(string tablename, [FromBody]object value)
        {
            ActionResult result = null;

            try
            {
                JArray array = (JArray)value;

                SQLConnector connector = new SQLConnector(_connectionString);

                foreach (JObject jObject in array.Children())
                {
                    Dictionary<string, string> updateSet = GetValues(jObject);

                    connector.InsertSingleRecord(tablename,  updateSet);
                }



                result = NoContent();
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new { Error = ex.Message });
            }

            return result;
        }

       
        [HttpPut("{tablename}")]
        public ActionResult Put(string tablename, [FromBody]object value)
        {
            ActionResult result = null;

            try
            {
                List<KeyValuePair<string, string>> parameters = GetParameters();
                Dictionary<string, string> updateSet = GetValues(value);

                SQLConnector connector = new SQLConnector(_connectionString);
                connector.UpdateRecords(tablename, parameters, updateSet);

                result = NoContent();
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new { Error = ex.Message });
            }

            return result;
        }

        
        [HttpDelete("{tablename}")]
        public ActionResult Delete(string tablename)
        {
            ActionResult result = null;

            try
            {
                List<KeyValuePair<string, string>> parameters = GetParameters();

                SQLConnector connector = new SQLConnector(_connectionString);

                connector.DeleteRecords(tablename, parameters);
                result = NoContent();
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new { Error = ex.Message });
            }

            return result;
        }

        private List<KeyValuePair<string,string>> GetParameters()
        {
            string queryString = Request.QueryString.Value;


            List<KeyValuePair<string, string>> parameters = null;

            if (!string.IsNullOrEmpty(queryString))
            {
                queryString = queryString.Substring(1);

                string[] args = queryString.Split('&');

                if (args != null && args.Length > 0)
                {
                    foreach (string arg in args)
                    {

                        string[] argPair = arg.Split('=');

                        if (argPair != null && argPair.Length == 2)
                        {
                            if (parameters == null)
                                parameters = new List<KeyValuePair<string, string>>();

                            KeyValuePair<string, string> parameterToSend = new KeyValuePair<string, string>(argPair[0], argPair[1]);

                            parameters.Add(parameterToSend);

                        }
                    }
                }
            }

            return parameters;
        }

        private Dictionary<string, string> GetValues(object jsonObject)
        {


            Dictionary<string, string> values = null;

            if(jsonObject != null)
            {
                string jsonString = JsonConvert.SerializeObject(jsonObject);
                values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            }

            return values;
        }
    }
}
