using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManagement.Models
{
    public class TableProcedure
    {
        public class Create : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public string FileGroup { get; set; } = "PRIMARY";
            public string? Description { get; set; }

            public string GetProcedureName() => "uspCreateTable";
            public string GetProcedurePath() => "SQL\\Procedure\\Table";
        }

        public class Alter : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public string? Description { get; set; }

            public string GetProcedureName() => "uspAlterTable";
            public string GetProcedurePath() => "SQL\\Procedure\\Table";
        }

        public class Drop : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public bool IgnoreIfNotExists { get; set; } = true;
            public bool ForceDropDependent { get; set; } = false;

            public string GetProcedureName() => "uspDropTable";
            public string GetProcedurePath() => "SQL\\Procedure\\Table";
        }

        public class Information : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;

            public string GetProcedureName() => "uspGetTableInfo";
            public string GetProcedurePath() => "SQL\\Procedure\\Table";
        }
    }
}
