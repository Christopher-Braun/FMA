using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMA.Contracts;

namespace FMA.UnitTests.Helpers
{
    public static class Extensions
    {
        public static string GetDisplayText(this MaterialField field)
        {
            return field.Uppper ? field.DefaultValue.ToUpper() : field.DefaultValue;
        }
    }
}
