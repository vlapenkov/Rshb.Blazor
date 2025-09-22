using Suap.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suap.Common.Errors.Exceptions;


public class SuapValidationException : AppException
{

    public string Name { get; }


    public SuapValidationException(string name, string message) : base(message)
    {
        Name = name;

    }


}
