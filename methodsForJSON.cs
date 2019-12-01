using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office.CustomUI;
using Newtonsoft.Json;

namespace InventorProcess
{
    class methodsForJSON
    {
        public static void readJSONfile()
        {
            // process JSON file containg patent documents that might belong to client.
            using (StreamReader r = new StreamReader("sentinelresults-onerecord.json"))
            {
                string json = r.ReadToEnd();
                //List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
                List<List<JSONResponse>> myDeserializedObjList = 
    JsonConvert.DeserializeObject<List<List<JSONResponse>>>(json);
            }
        }
    }
}
