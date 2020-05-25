using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Htec.Sandbox.Core.UseCases
{
    public class UseCaseManager
    {
        public List<UseCaseModel> LoadUseCases()
        {
            using (StreamReader reader = new StreamReader(Path.Combine(Environment.CurrentDirectory + @"\Htec-Task\Htec.Sandbox.Core\UseCases" + @"\UseCases.json")))
            {
                return JsonConvert.DeserializeObject<List<UseCaseModel>>(reader.ReadToEnd());
            }
        }
    }
}
