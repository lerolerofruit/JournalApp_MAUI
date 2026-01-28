using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JournalApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Moods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Streaks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrentStreak = table.Column<int>(type: "INTEGER", nullable: false),
                    LongestStreak = table.Column<int>(type: "INTEGER", nullable: false),
                    LastEntryDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streaks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsCustom = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Salt = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Theme = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PrimaryMoodId = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondaryMood1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    SecondaryMood2Id = table.Column<int>(type: "INTEGER", nullable: true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: true),
                    TagIds = table.Column<string>(type: "TEXT", nullable: false),
                    WordCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntries_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_JournalEntries_Moods_PrimaryMoodId",
                        column: x => x.PrimaryMoodId,
                        principalTable: "Moods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntries_Moods_SecondaryMood1Id",
                        column: x => x.SecondaryMood1Id,
                        principalTable: "Moods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntries_Moods_SecondaryMood2Id",
                        column: x => x.SecondaryMood2Id,
                        principalTable: "Moods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Personal" },
                    { 2, "Professional" },
                    { 3, "Health & Wellness" },
                    { 4, "Relationships" },
                    { 5, "Goals & Dreams" }
                });

            migrationBuilder.InsertData(
                table: "Moods",
                columns: new[] { "Id", "Category", "Name" },
                values: new object[,]
                {
                    { 1, "Positive", "Happy" },
                    { 2, "Positive", "Excited" },
                    { 3, "Positive", "Relaxed" },
                    { 4, "Positive", "Grateful" },
                    { 5, "Positive", "Confident" },
                    { 6, "Neutral", "Calm" },
                    { 7, "Neutral", "Thoughtful" },
                    { 8, "Neutral", "Curious" },
                    { 9, "Neutral", "Nostalgic" },
                    { 10, "Neutral", "Bored" },
                    { 11, "Negative", "Sad" },
                    { 12, "Negative", "Angry" },
                    { 13, "Negative", "Stressed" },
                    { 14, "Negative", "Lonely" },
                    { 15, "Negative", "Anxious" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "IsCustom", "Name" },
                values: new object[,]
                {
                    { 1, false, "Work" },
                    { 2, false, "Career" },
                    { 3, false, "Studies" },
                    { 4, false, "Family" },
                    { 5, false, "Friends" },
                    { 6, false, "Relationships" },
                    { 7, false, "Health" },
                    { 8, false, "Fitness" },
                    { 9, false, "Personal Growth" },
                    { 10, false, "Self-care" },
                    { 11, false, "Hobbies" },
                    { 12, false, "Travel" },
                    { 13, false, "Nature" },
                    { 14, false, "Finance" },
                    { 15, false, "Spirituality" },
                    { 16, false, "Birthday" },
                    { 17, false, "Holiday" },
                    { 18, false, "Vacation" },
                    { 19, false, "Celebration" },
                    { 20, false, "Exercise" },
                    { 21, false, "Reading" },
                    { 22, false, "Writing" },
                    { 23, false, "Cooking" },
                    { 24, false, "Meditation" },
                    { 25, false, "Yoga" },
                    { 26, false, "Music" },
                    { 27, false, "Shopping" },
                    { 28, false, "Parenting" },
                    { 29, false, "Projects" },
                    { 30, false, "Planning" },
                    { 31, false, "Reflection" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_CategoryId",
                table: "JournalEntries",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_Date",
                table: "JournalEntries",
                column: "Date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_PrimaryMoodId",
                table: "JournalEntries",
                column: "PrimaryMoodId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_SecondaryMood1Id",
                table: "JournalEntries",
                column: "SecondaryMood1Id");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_SecondaryMood2Id",
                table: "JournalEntries",
                column: "SecondaryMood2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JournalEntries");

            migrationBuilder.DropTable(
                name: "Streaks");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Moods");
        }
    }
}
