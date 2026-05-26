using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManagement.Models
{
    public class BackupProcedure : IProcedure
    {
        public string DatabaseName { get; set; } = null!;
        public string BackupPath { get; set; } = null!;

        public string GetProcedureName() => "uspBackupDatabase";
        public string GetProcedurePath() => "SQL\\Procedure";
    }
}
