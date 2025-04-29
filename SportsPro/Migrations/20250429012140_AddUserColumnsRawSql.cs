using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsPro.Migrations
{
    public partial class AddUserColumnsRawSql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Use raw SQL to add columns if they don't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'Firstname')
                BEGIN
                    ALTER TABLE AspNetUsers ADD Firstname nvarchar(max) NULL;
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'Lastname')
                BEGIN
                    ALTER TABLE AspNetUsers ADD Lastname nvarchar(max) NULL;
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'RoleNames')
                BEGIN
                    ALTER TABLE AspNetUsers ADD RoleNames nvarchar(max) NULL;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Use raw SQL to drop columns if they exist
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'Firstname')
                BEGIN
                    ALTER TABLE AspNetUsers DROP COLUMN Firstname;
                END

                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'Lastname')
                BEGIN
                    ALTER TABLE AspNetUsers DROP COLUMN Lastname;
                END

                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'RoleNames')
                BEGIN
                    ALTER TABLE AspNetUsers DROP COLUMN RoleNames;
                END
            ");
        }
    }
}
