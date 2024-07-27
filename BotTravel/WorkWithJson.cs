using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotTravel
{
    class WorkWithJson
    {
        public List<T> ReadJsonFile<T>(string filename)
        {
            string content = File.ReadAllText(filename);
            List<T> list = JsonConvert.DeserializeObject<List<T>>(content);
            return list;
        }
    }
}
