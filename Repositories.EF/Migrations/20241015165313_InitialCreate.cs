using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Repositories.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsPrimitive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Types__3214EC27154A2D2D", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DeleteBehaviours",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Deletion__737584F761214E1A", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Ancestries",
                columns: table => new
                {
                    ClassID = table.Column<int>(type: "int", nullable: false),
                    ParentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ancestri__CF8260B6E108D3A6", x => new { x.ClassID, x.ParentID });
                    table.ForeignKey(
                        name: "FK__Ancestrie__Paren__7DCDAAA2",
                        column: x => x.ParentID,
                        principalTable: "Classes",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__Ancestrie__TypeI__7CD98669",
                        column: x => x.ClassID,
                        principalTable: "Classes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Objects",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Entities__3214EC2769EAA14C", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Entities__TypeID__0A338187",
                        column: x => x.ClassID,
                        principalTable: "Classes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassID = table.Column<int>(type: "int", nullable: false),
                    PropertyClassID = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Relation__3214EC27248D1913", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Relations__Desti__02925FBF",
                        column: x => x.PropertyClassID,
                        principalTable: "Classes",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__Relations__Origi__019E3B86",
                        column: x => x.ClassID,
                        principalTable: "Classes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AbstractPropertyDetails",
                columns: table => new
                {
                    PropertyID = table.Column<int>(type: "int", nullable: false),
                    Min = table.Column<int>(type: "int", nullable: true),
                    Max = table.Column<int>(type: "int", nullable: true),
                    DeleteBehaviour = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Relation__70C9A755AD2F208E", x => x.PropertyID);
                    table.ForeignKey(
                        name: "FK__RelationD__OnDel__04459E07",
                        column: x => x.PropertyID,
                        principalTable: "Properties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__RelationD__OnDel__0539C240",
                        column: x => x.DeleteBehaviour,
                        principalTable: "DeleteBehaviours",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateTable(
                name: "StringValues",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectID = table.Column<int>(type: "int", nullable: false),
                    PropertyID = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StringVa__3214EC2753E7643C", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StringValues_Attribute",
                        column: x => x.PropertyID,
                        principalTable: "Properties",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__StringVal__Entit__1F2E9E6D",
                        column: x => x.ObjectID,
                        principalTable: "Objects",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbstractPropertyDetails_DeleteBehaviour",
                table: "AbstractPropertyDetails",
                column: "DeleteBehaviour");

            migrationBuilder.CreateIndex(
                name: "IX_Ancestries_ParentID",
                table: "Ancestries",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_ClassID",
                table: "Objects",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ClassID",
                table: "Properties",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyClassID",
                table: "Properties",
                column: "PropertyClassID");

            migrationBuilder.CreateIndex(
                name: "IX_StringValues_ObjectID",
                table: "StringValues",
                column: "ObjectID");

            migrationBuilder.CreateIndex(
                name: "IX_StringValues_PropertyID",
                table: "StringValues",
                column: "PropertyID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbstractPropertyDetails");

            migrationBuilder.DropTable(
                name: "Ancestries");

            migrationBuilder.DropTable(
                name: "StringValues");

            migrationBuilder.DropTable(
                name: "DeleteBehaviours");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Objects");

            migrationBuilder.DropTable(
                name: "Classes");
        }
    }
}
