using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeckAndFlashcard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Users_OwnerId",
                table: "Decks");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Flashcards",
                newName: "FrontImageUrl");

            migrationBuilder.RenameColumn(
                name: "AudioUrl",
                table: "Flashcards",
                newName: "FrontAudioUrl");

            migrationBuilder.RenameColumn(
                name: "Front",
                table: "Flashcards",
                newName: "FrontText");

            migrationBuilder.RenameColumn(
                name: "Back",
                table: "Flashcards",
                newName: "BackText");

            migrationBuilder.AddColumn<string>(
                name: "BackAudioUrl",
                table: "Flashcards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Back_ImageUrl",
                table: "Flashcards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Flashcards",
                type: "integer",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "Flag",
                table: "Flashcards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Flashcards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Decks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Users_OwnerId",
                table: "Decks",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Users_OwnerId",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "BackAudioUrl",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "Back_ImageUrl",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "Flag",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Decks");

            migrationBuilder.RenameColumn(
                name: "FrontImageUrl",
                table: "Flashcards",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "FrontAudioUrl",
                table: "Flashcards",
                newName: "AudioUrl");

            migrationBuilder.RenameColumn(
                name: "FrontText",
                table: "Flashcards",
                newName: "Front");

            migrationBuilder.RenameColumn(
                name: "BackText",
                table: "Flashcards",
                newName: "Back");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Users_OwnerId",
                table: "Decks",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
