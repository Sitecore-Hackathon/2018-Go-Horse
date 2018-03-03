using System.IO;
using Sitecore.HashTagMonitor.Api.Model;

namespace Sitecore.HashTagMonitor.ModelBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = XConnect.Serialization.XdbModelWriter.Serialize(CollectionModel.Model);
            File.WriteAllText(CollectionModel.Model.FullName + ".json", model);
        }
    }
}
