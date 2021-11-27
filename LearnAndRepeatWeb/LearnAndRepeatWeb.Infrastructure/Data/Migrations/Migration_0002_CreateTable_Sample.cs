using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnAndRepeatWeb.Infrastructure.Data.Migrations
{
    [Migration(2)]
    public class Migration_0002_CreateTable_Sample : Migration
    {
        public override void Down()
        {
            Delete.Table("Sample");
        }

        public override void Up()
        {
            Create.Table("Sample")
                .WithColumn("Id").AsInt64().PrimaryKey().NotNullable()
                .WithColumn("FirstName").AsString().NotNullable()
                .WithColumn("LastName").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("Password").AsString().NotNullable()
                .WithColumn("CreateDate").AsDateTime2().NotNullable()
                .WithColumn("UpdateDate").AsDateTime2().NotNullable();
        }
    }
}
