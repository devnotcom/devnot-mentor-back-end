using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevnotMentor.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LinkTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    HasUsername = table.Column<bool>(type: "bit", nullable: true),
                    BaseLink = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ImageUrl = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Message = table.Column<string>(type: "varchar(2000)", unicode: false, maxLength: 2000, nullable: true),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "varchar(512)", unicode: false, maxLength: 512, nullable: true),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    SurName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "nchar(500)", fixedLength: true, maxLength: 500, nullable: true),
                    UserNameConfirmed = table.Column<bool>(type: "bit", nullable: true),
                    UserState = table.Column<int>(type: "int", nullable: true),
                    Token = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    TokenExpireDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProfileUrl = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    SecurityKeyExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SecurityKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mentees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mentees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mentee_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mentor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mentor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mentor_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenteeLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenteeId = table.Column<int>(type: "int", nullable: true),
                    LinkTypeId = table.Column<int>(type: "int", nullable: true),
                    Link = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenteeLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenteeLinks_LinkType",
                        column: x => x.LinkTypeId,
                        principalTable: "LinkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MenteeLinks_Mentee",
                        column: x => x.MenteeId,
                        principalTable: "Mentees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenteeTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenteeId = table.Column<int>(type: "int", nullable: true),
                    TagId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenteeTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenteeTags_Mentee",
                        column: x => x.MenteeId,
                        principalTable: "Mentees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MenteeTags_Tag",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MentorApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenteeId = table.Column<int>(type: "int", nullable: true),
                    MentorId = table.Column<int>(type: "int", nullable: true),
                    AppliedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Note = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorApplications_Mentee",
                        column: x => x.MenteeId,
                        principalTable: "Mentees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MentorApplications_Mentor",
                        column: x => x.MentorId,
                        principalTable: "Mentor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MentorLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MentorId = table.Column<int>(type: "int", nullable: true),
                    LinkTypeId = table.Column<int>(type: "int", nullable: true),
                    Link = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorLinks_LinkType",
                        column: x => x.LinkTypeId,
                        principalTable: "LinkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MentorLinks_Mentor",
                        column: x => x.MentorId,
                        principalTable: "Mentor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MentorQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MentorId = table.Column<int>(type: "int", nullable: true),
                    QuestionText = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    QuestionNotes = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    QuestionTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorQuestion_Mentor",
                        column: x => x.MentorId,
                        principalTable: "Mentor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MentorQuestion_QuestionType",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mentorships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MentorId = table.Column<int>(type: "int", nullable: true),
                    MenteeId = table.Column<int>(type: "int", nullable: true),
                    MentorStartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    MentorEndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    State = table.Column<int>(type: "int", nullable: true),
                    MentorScore = table.Column<byte>(type: "tinyint", nullable: true),
                    MentorComment = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    MenteeScore = table.Column<byte>(type: "tinyint", nullable: true),
                    MenteeComment = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mentorships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mentorships_Mentee",
                        column: x => x.MenteeId,
                        principalTable: "Mentees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mentorships_Mentor",
                        column: x => x.MentorId,
                        principalTable: "Mentor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MentorTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MentorId = table.Column<int>(type: "int", nullable: true),
                    TagId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorTags_Mentor",
                        column: x => x.MentorId,
                        principalTable: "Mentor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MentorTags_Tag",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenteeAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenteeId = table.Column<int>(type: "int", nullable: true),
                    MentorQuestionId = table.Column<int>(type: "int", nullable: true),
                    Answer = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenteeAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenteeAnswers_Mentee",
                        column: x => x.MenteeId,
                        principalTable: "Mentees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MenteeAnswers_MentorQuestion",
                        column: x => x.MentorQuestionId,
                        principalTable: "MentorQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenteeAnswers_MenteeId",
                table: "MenteeAnswers",
                column: "MenteeId");

            migrationBuilder.CreateIndex(
                name: "IX_MenteeAnswers_MentorQuestionId",
                table: "MenteeAnswers",
                column: "MentorQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_MenteeLinks_LinkTypeId",
                table: "MenteeLinks",
                column: "LinkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MenteeLinks_MenteeId",
                table: "MenteeLinks",
                column: "MenteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Mentees_UserId",
                table: "Mentees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MenteeTags_MenteeId",
                table: "MenteeTags",
                column: "MenteeId");

            migrationBuilder.CreateIndex(
                name: "IX_MenteeTags_TagId",
                table: "MenteeTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Mentor_UserId",
                table: "Mentor",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorApplications_MenteeId",
                table: "MentorApplications",
                column: "MenteeId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorApplications_MentorId",
                table: "MentorApplications",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorLinks_LinkTypeId",
                table: "MentorLinks",
                column: "LinkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorLinks_MentorId",
                table: "MentorLinks",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorQuestion_MentorId",
                table: "MentorQuestion",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorQuestion_QuestionTypeId",
                table: "MentorQuestion",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Mentorships_MenteeId",
                table: "Mentorships",
                column: "MenteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Mentorships_MentorId",
                table: "Mentorships",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorTags_MentorId",
                table: "MentorTags",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorTags_TagId",
                table: "MentorTags",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "MenteeAnswers");

            migrationBuilder.DropTable(
                name: "MenteeLinks");

            migrationBuilder.DropTable(
                name: "MenteeTags");

            migrationBuilder.DropTable(
                name: "MentorApplications");

            migrationBuilder.DropTable(
                name: "MentorLinks");

            migrationBuilder.DropTable(
                name: "Mentorships");

            migrationBuilder.DropTable(
                name: "MentorTags");

            migrationBuilder.DropTable(
                name: "MentorQuestion");

            migrationBuilder.DropTable(
                name: "LinkTypes");

            migrationBuilder.DropTable(
                name: "Mentees");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Mentor");

            migrationBuilder.DropTable(
                name: "QuestionTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
