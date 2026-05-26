using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManagement.Models
{
    public interface IProcedure
    {
        string GetProcedureName();
        string GetProcedurePath();
    }
}
