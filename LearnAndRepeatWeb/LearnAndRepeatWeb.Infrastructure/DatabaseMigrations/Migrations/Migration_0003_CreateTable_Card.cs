using FluentMigrator;

namespace LearnAndRepeatWeb.Infrastructure.DatabaseMigrations.Migrations
{
    [Migration(3)]
    public class Migration_0003_CreateTable_Card : Migration
    {
        public override void Down()
        {
            Delete.Table("Card");
        }

        public override void Up()
        {
            Create.Table("Card")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("Header").AsString().NotNullable()
                .WithColumn("Content").AsString().NotNullable()
                .WithColumn("Tag").AsString().Nullable()
                .WithColumn("CreateDate").AsDateTime2().NotNullable()
                .WithColumn("UpdateDate").AsDateTime2().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(0)
                .WithColumn("DeleteDate").AsDateTime2().Nullable();
        }
    }
}
