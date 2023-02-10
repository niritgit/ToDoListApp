using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoListApp.Migrations
{
    public partial class spUpdateTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[GetValidTasksIds]
                    @ids varchar(MAX)
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT ID 
                    FROM TasksToDo
                    WHERE ID in (SELECT convert(int, value) FROM string_split(@ids, ','))
                END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
