using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DatabaseManagement.Models
{
    public class ColumnProcedure
    {
        public class Create : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public string ColumnName { get; set; } = null!;
            public string DataType { get; set; } = null!;
            public int? Length { get; set; }
            public int? Precision { get; set; }
            public int? Scale { get; set; }
            public bool IsNullable { get; set; } = true;
            public string? DefaultValue { get; set; }
            public string? DefaultName { get; set; }
            public bool IsIdentity { get; set; } = false;
            public int IdentitySeed { get; set; } = 1;
            public int IdentityIncrement { get; set; } = 1;
            public bool IsPrimaryKey { get; set; } = false;
            public string? PKName { get; set; }
            public string? Collation { get; set; }
            public string? ComputedFormula { get; set; }
            public bool IsPersisted { get; set; } = false;
            public string? Description { get; set; }

            public string GetProcedureName() => "uspCreateColumn";
            public string GetProcedurePath() => "SQL\\Procedure\\Column";
        }

        public class Alter : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public string ColumnName { get; set; } = null!;
            public string? DataType { get; set; }
            public int? Length { get; set; }
            public int? Precision { get; set; }
            public int? Scale { get; set; }
            public bool? IsNullable { get; set; }
            public string? DefaultValue { get; set; }
            public string? DefaultName { get; set; }
            public string? Collation { get; set; }
            public string? Description { get; set; }

            public string GetProcedureName() => "uspAlterColumn";
            public string GetProcedurePath() => "SQL\\Procedure\\Column";
        }

        public class Drop : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public string ColumnName { get; set; } = null!;
            public bool IgnoreIfNotExists { get; set; } = true;
            public bool ForceDropDependent { get; set; } = false;

            public string GetProcedureName() => "uspDropColumn";
            public string GetProcedurePath() => "SQL\\Procedure\\Column";
        }

        public class Information : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public string ColumnName { get; set; } = null!;

            public string GetProcedureName() => "uspGetColumnInfo";
            public string GetProcedurePath() => "SQL\\Procedure\\Column";
        }
    }
}
