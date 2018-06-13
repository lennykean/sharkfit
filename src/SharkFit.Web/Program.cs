using System.IO;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SharkFit.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dataDir = new DirectoryInfo("data");
            dataDir.Create();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
