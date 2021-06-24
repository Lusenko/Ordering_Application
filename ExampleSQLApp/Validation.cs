using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExampleSQLApp
{
    class Validation
    {
        public bool Symbol(string inp, int min, int max, string exp = @"^[a-zA-Z]{3,18}$") => (Regex.IsMatch(inp, exp) && inp.Length >= min && inp.Length <= max) ? true : false;
        public bool Numer(string inp, string exp = @"^([+]380)[0-9]{9}$") => Regex.IsMatch(inp, exp) ? true : false;
        public bool Mail(string inp, string exp = @"^[A-Za-z0-9._-]+@[A-Za-z0-9]+\.[A-Za-z]{2,5}$") => Regex.IsMatch(inp, exp) ? true : false;
    }
}
