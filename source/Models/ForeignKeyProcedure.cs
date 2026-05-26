using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManagement.Models
{
    public class ForeignKeyProcedure
    {
        public class Create : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public string ColumnName { get; set; } = null!;
            public string RefSchemaName { get; set; } = null!;
            public string RefTableName { get; set; } = null!;
            public string RefColumnName { get; set; } = null!;
            public string OnDelete { get; set; } = "NO ACTION";
            public string OnUpdate { get; set; } = "NO ACTION";
            public bool IsNotForReplication { get; set; } = false;
            public bool Enabled { get; set; } = true;

            public string GetProcedureName() => "uspCreateForeignKey";
            public string GetProcedurePath() => "SQL\\Procedure\\ForeignKey";
        }

        public class Alter : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public string ColumnName { get; set; } = null!;
            public string? RefSchemaName { get; set; }
            public string? RefTableName { get; set; }
            public string? RefColumnName { get; set; }
            public string? OnDelete { get; set; }
            public string? OnUpdate { get; set; }
            public bool? IsNotForReplication { get; set; }
            public bool? Enabled { get; set; }

            public string GetProcedureName() => "uspAlterForeignKey";
            public string GetProcedurePath() => "SQL\\Procedure\\ForeignKey";
        }

        public class Drop : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public string? ColumnName { get; set; }
            public string? RefSchemaName { get; set; }
            public string? RefTableName { get; set; }
            public string? RefColumnName { get; set; }
            public string? FKName { get; set; }
            public bool IgnoreIfNotExists { get; set; } = true;

            public string GetProcedureName() => "uspDropForeignKey";
            public string GetProcedurePath() => "SQL\\Procedure\\ForeignKey";
        }

        public class Information : IProcedure
        {
            public string DatabaseName { get; set; } = null!;
            public string SchemaName { get; set; } = null!;
            public string TableName { get; set; } = null!;
            public string ColumnName { get; set; } = null!;

            public string GetProcedureName() => "uspGetForeignKeyInfo";
            public string GetProcedurePath() => "SQL\\Procedure\\ForeignKey";
        }
    }
}
