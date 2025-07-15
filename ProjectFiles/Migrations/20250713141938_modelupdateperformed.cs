using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroTechProject.Migrations
{
    /// <inheritdoc />
    public partial class modelupdateperformed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"ALTER TABLE ""Bookings"" 
                  ALTER COLUMN ""Status"" 
                  TYPE integer 
                  USING CASE 
                      WHEN ""Status"" = 'Pending' THEN 0
                      WHEN ""Status"" = 'Approved' THEN 1
                      WHEN ""Status"" = 'Rejected' THEN 2
                      ELSE 0
                  END;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"ALTER TABLE ""Bookings"" 
                  ALTER COLUMN ""Status"" 
                  TYPE character varying(10) 
                  USING CASE 
                      WHEN ""Status"" = 0 THEN 'Pending'
                      WHEN ""Status"" = 1 THEN 'Approved'
                      WHEN ""Status"" = 2 THEN 'Rejected'
                      ELSE 'Pending'
                  END;"
            );
        }
    }
}