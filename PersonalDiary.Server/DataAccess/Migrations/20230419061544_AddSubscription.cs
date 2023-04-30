using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriberFID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObservableFID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_UserIdentities_ObservableFID",
                        column: x => x.ObservableFID,
                        principalTable: "UserIdentities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subscriptions_UserIdentities_SubscriberFID",
                        column: x => x.SubscriberFID,
                        principalTable: "UserIdentities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_ObservableFID",
                table: "Subscriptions",
                column: "ObservableFID");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubscriberFID_ObservableFID",
                table: "Subscriptions",
                columns: new[] { "SubscriberFID", "ObservableFID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");
        }
    }
}
