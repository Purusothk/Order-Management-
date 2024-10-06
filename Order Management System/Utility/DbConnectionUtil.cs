using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Utility
{
     
        internal class DbConnectionUtil
        {
            private static IConfiguration _iconfiguration;

            static DbConnectionUtil()
            {
                GetConnString();
            }

            public static string GetConnString()
            {
                var builder = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                _iconfiguration = builder.Build();

                var connectionString = _iconfiguration.GetConnectionString("AppSettingsDbContext");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'AppSettingsDbContext' is not configured in appsettings.json.");
                }

                return connectionString;
            }
        }
    }


