using FluentMigrator;

namespace LearnAndRepeatWeb.Infrastructure.DatabaseMigrations.Migrations
{
    [Migration(3)]
    public class Migration_0003_CreateTable_UserToken : Migration
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
                .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(0)
                .WithColumn("CreateDate").AsDateTime2().NotNullable()
                .WithColumn("UpdateDate").AsDateTime2()
                .WithColumn("ExpireDate").AsDateTime2().NotNullable();
        }
    }
}
