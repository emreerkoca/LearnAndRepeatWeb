using FluentMigrator;

namespace LearnAndRepeatWeb.Infrastructure.DatabaseMigrations.Migrations
{
    [Migration(2)]
    public class Migration_0002_CreateTable_UserToken : Migration
    {
        public override void Down()
        {
            Delete.Table("UserToken");
        }

        public override void Up()
        {
            Create.Table("UserToken")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("UserTokenType").AsInt32().NotNullable()
                .WithColumn("TokenValue").AsString().NotNullable()
                .WithColumn("IsUsed").AsBoolean().NotNullable().WithDefaultValue(0)
                .WithColumn("CreateDate").AsDateTime2().NotNullable()
                .WithColumn("ExpireDate").AsDateTime2().NotNullable();
        }
    }
}
