using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suap.Common.Contracts;
public interface IResult<T>
{
    T? Data { get; }

    bool IsSuccess {get;}

    string[] ErrorMessages { get; }
}
