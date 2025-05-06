using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WholeDBCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryItem_Trip_TripId",
                table: "ItineraryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelerType_Users_UserId",
                table: "TravelerType");

            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Users_UserId",
                table: "Trip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trip",
                table: "Trip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TravelerType",
                table: "TravelerType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItineraryItem",
                table: "ItineraryItem");

            migrationBuilder.RenameTable(
                name: "Trip",
                newName: "Trips");

            migrationBuilder.RenameTable(
                name: "TravelerType",
                newName: "TravelerTypes");

            migrationBuilder.RenameTable(
                name: "ItineraryItem",
                newName: "ItineraryItems");

            migrationBuilder.RenameIndex(
                name: "IX_Trip_UserId",
                table: "Trips",
                newName: "IX_Trips_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TravelerType_UserId",
                table: "TravelerTypes",
                newName: "IX_TravelerTypes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ItineraryItem_TripId",
                table: "ItineraryItems",
                newName: "IX_ItineraryItems_TripId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trips",
                table: "Trips",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TravelerTypes",
                table: "TravelerTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItineraryItems",
                table: "ItineraryItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryItems_Trips_TripId",
                table: "ItineraryItems",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelerTypes_Users_UserId",
                table: "TravelerTypes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_UserId",
                table: "Trips",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryItems_Trips_TripId",
                table: "ItineraryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelerTypes_Users_UserId",
                table: "TravelerTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_UserId",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trips",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TravelerTypes",
                table: "TravelerTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItineraryItems",
                table: "ItineraryItems");

            migrationBuilder.RenameTable(
                name: "Trips",
                newName: "Trip");

            migrationBuilder.RenameTable(
                name: "TravelerTypes",
                newName: "TravelerType");

            migrationBuilder.RenameTable(
                name: "ItineraryItems",
                newName: "ItineraryItem");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_UserId",
                table: "Trip",
                newName: "IX_Trip_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TravelerTypes_UserId",
                table: "TravelerType",
                newName: "IX_TravelerType_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ItineraryItems_TripId",
                table: "ItineraryItem",
                newName: "IX_ItineraryItem_TripId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trip",
                table: "Trip",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TravelerType",
                table: "TravelerType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItineraryItem",
                table: "ItineraryItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryItem_Trip_TripId",
                table: "ItineraryItem",
                column: "TripId",
                principalTable: "Trip",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelerType_Users_UserId",
                table: "TravelerType",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Users_UserId",
                table: "Trip",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
