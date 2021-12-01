using FluentMigrator;

namespace LearnAndRepeatWeb.Infrastructure.DatabaseMigrations.Migrations
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
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("FirstName").AsString().NotNullable()
                .WithColumn("LastName").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable().Unique()
                .WithColumn("Password").AsString().NotNullable()
                .WithColumn("Salt").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(0)
                .WithColumn("CreateDate").AsDateTime2().NotNullable()
                .WithColumn("UpdateDate").AsDateTime2().NotNullable();
        }
    }
}
