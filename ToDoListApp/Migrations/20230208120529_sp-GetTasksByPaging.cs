using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoListApp.Migrations
{
    public partial class spGetTasksByPaging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[GetTasksByPaging]
                    @Skip int,
                    @Take int
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT * 
                    FROM TasksToDo
                    ORDER BY DueDate desc
                    OFFSET (@Skip) ROWS FETCH NEXT (@Take) ROWS ONLY
                END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
