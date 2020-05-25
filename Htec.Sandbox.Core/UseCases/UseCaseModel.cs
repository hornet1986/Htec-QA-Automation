using System.Collections.Generic;

namespace Htec.Sandbox.Core.UseCases
{
    public class UseCaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ExpectedResult { get; set; }
        public bool isAutomated { get; set; }
        public List<string> Steps { get; set; }
    }
}
