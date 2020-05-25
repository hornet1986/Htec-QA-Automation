using System;
using System.Collections.Generic;
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
