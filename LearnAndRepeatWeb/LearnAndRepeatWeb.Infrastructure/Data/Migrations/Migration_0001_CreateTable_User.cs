using FluentMigrator;

namespace LearnAndRepeatWeb.Infrastructure.Data.Migrations
{
    [Migration(1)]
    public class Migration_0001_CreateTable_User : Migration
    {
        public override void Down()
        {
            Delete.Table("User");
        }

        public override void Up()
        {
            Create.Table("User")
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
