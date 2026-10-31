using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class IncreaseUsernameLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE [dbo].[Users] " +
                "ALTER COLUMN [UserName] NVARCHAR(50) NOT NULL;"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intentionally left empty.
            // Reverting to NVARCHAR(10) could truncate existing usernames.
        }
    }
}
