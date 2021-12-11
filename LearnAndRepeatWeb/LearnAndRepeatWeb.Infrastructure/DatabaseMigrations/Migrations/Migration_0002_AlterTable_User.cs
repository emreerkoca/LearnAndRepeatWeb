using FluentMigrator;

namespace LearnAndRepeatWeb.Infrastructure.DatabaseMigrations.Migrations
{
    [Migration(2)]
    public class Migration_0002_AlterTable_User : Migration
    {
        public override void Down()
        {
            Delete.Column("IsEmailConfirmed").FromTable("User");
        }

        public override void Up()
        {
            Alter.Table("User")
                .AddColumn("IsEmailConfirmed")
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(0);
        }
    }
}
