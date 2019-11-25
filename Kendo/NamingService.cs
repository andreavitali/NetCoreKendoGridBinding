using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artech.AspNetCore.Kendo
{
    public interface INamingService
    {
        string Convert(string name);
    }

    public class SnakeCaseToPascalCaseNamingService : INamingService
    {
        public string Convert(string name)
        {
            return name.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries)
               .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
               .Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }
    }

    public class IdentityNamingService : INamingService
    {
        public string Convert(string name)
        {
            return name;
        }
    }
}
