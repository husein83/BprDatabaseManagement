using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManagement.Models
{
    public class RestoreProcedure : IProcedure
    {
        public string DatabaseName { get; set; } = null!;
        public string RestorePath { get; set; } = null!;

        public string GetProcedureName() => "uspRestoreDatabase";
        public string GetProcedurePath() => "SQL\\Procedure";
    }
}
