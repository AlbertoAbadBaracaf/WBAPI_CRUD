using System.Net.Mail;
using System.Net;
using WBAPI_CRUD.Repositorio.IRepositorio;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Hosting.Server;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace WBAPI_CRUD.Repositorio
{
    public class UtilsRepositorio : IUtilsRepositorio
    {
        public string getConfiguration(string valor)
        {
            var builder = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            return configuration[valor];
        }
    }
}
