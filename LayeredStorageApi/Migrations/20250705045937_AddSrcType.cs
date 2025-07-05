using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayeredStorageApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSrcType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sourceType",
                table: "DataStores",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sourceType",
                table: "DataStores");
        }
    }
}
