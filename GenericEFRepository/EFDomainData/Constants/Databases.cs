
using EFDomainData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDomainData.Constant
{
    public static class Databases
    {
        public const string MySampleDatabase = "MySampleDatabase1";
        public static DbContext GetDatabaseContext(string databaseName, string userName = "")
        {
            switch (databaseName)
            {
                case Databases.MySampleDatabase:
                    return new MySampleDatabaseContext();
                default:
                    break;
            }
            return null!;
        }
    }
}
