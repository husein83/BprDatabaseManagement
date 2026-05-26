using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManagement.Models
{
    public class IndexProcedure
    {
        public class Create : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public string? IndexName { get; set; }
            public string Columns { get; set; } = null!;
            public string? IncludeColumns { get; set; }
            public bool IsUnique { get; set; } = false;
            public string IndexType { get; set; } = "NONCLUSTERED";
            public short FillFactor { get; set; } = 80;
            public bool PadIndex { get; set; } = true;
            public bool AllowRowLocks { get; set; } = true;
            public bool AllowPageLocks { get; set; } = true;
            public string? FilterPredicate { get; set; }
            public string FileGroup { get; set; } = "PRIMARY";

            public string GetProcedureName() => "uspCreateIndex";
            public string GetProcedurePath() => "SQL\\Procedure\\Index";
        }

        public class Alter : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public string IndexName { get; set; } = null!;
            public string? Columns { get; set; }
            public string? IncludeColumns { get; set; }
            public bool? IsUnique { get; set; }
            public string? IndexType { get; set; }
            public short? FillFactor { get; set; }
            public bool? PadIndex { get; set; }
            public bool? AllowRowLocks { get; set; }
            public bool? AllowPageLocks { get; set; }
            public string? FilterPredicate { get; set; }
            public string? FileGroup { get; set; }

            public string GetProcedureName() => "uspAlterIndex";
            public string GetProcedurePath() => "SQL\\Procedure\\Index";
        }

        public class Drop : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public string IndexName { get; set; } = null!;
            public bool IgnoreIfNotExists { get; set; } = true;

            public string GetProcedureName() => "uspDropIndex";
            public string GetProcedurePath() => "SQL\\Procedure\\Index";
        }

        public class Information : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public string IndexName { get; set; } = null!;

            public string GetProcedureName() => "uspGetIndexInfo";
            public string GetProcedurePath() => "SQL\\Procedure\\Index";
        }
    }
}
