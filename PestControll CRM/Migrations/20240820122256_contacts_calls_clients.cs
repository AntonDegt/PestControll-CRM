﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PestControll_CRM.Migrations
{
    /// <inheritdoc />
    public partial class contacts_calls_clients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CallResultTypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallResultTypes", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CallTypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallTypes", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ContactStatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StatusName = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatusColor = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactStatus", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TaxSystems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxSystems", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    contactstatus_id = table.Column<int>(type: "int", nullable: true),
                    pib = table.Column<string>(type: "varchar(60)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(40)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    notes = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Contacts_ContactStatus_contactstatus_id",
                        column: x => x.contactstatus_id,
                        principalTable: "ContactStatus",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LegalPersons",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(40)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    edrpou = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    taxsystem_id = table.Column<int>(type: "int", nullable: false),
                    pdv = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    current_account = table.Column<string>(type: "varchar(40)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(40)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalPersons", x => x.id);
                    table.ForeignKey(
                        name: "FK_LegalPersons_TaxSystems_taxsystem_id",
                        column: x => x.taxsystem_id,
                        principalTable: "TaxSystems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Calls",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    call_type_id = table.Column<int>(type: "int", nullable: true),
                    contact_id = table.Column<int>(type: "int", nullable: true),
                    date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    call_result_type_id = table.Column<int>(type: "int", nullable: true),
                    comment = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calls", x => x.id);
                    table.ForeignKey(
                        name: "FK_Calls_CallResultTypes_call_result_type_id",
                        column: x => x.call_result_type_id,
                        principalTable: "CallResultTypes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Calls_CallTypes_call_type_id",
                        column: x => x.call_type_id,
                        principalTable: "CallTypes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Calls_Contacts_contact_id",
                        column: x => x.contact_id,
                        principalTable: "Contacts",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NaturalPersons",
                columns: table => new
                {
                    contact_id = table.Column<int>(type: "int", nullable: false),
                    ipn = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalPersons", x => x.contact_id);
                    table.ForeignKey(
                        name: "FK_NaturalPersons_Contacts_contact_id",
                        column: x => x.contact_id,
                        principalTable: "Contacts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PhoneNumber",
                columns: table => new
                {
                    phone_number = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contact_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => x.phone_number);
                    table.ForeignKey(
                        name: "FK_PhoneNumber_Contacts_contact_id",
                        column: x => x.contact_id,
                        principalTable: "Contacts",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlannedCalls",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    contact_id = table.Column<int>(type: "int", nullable: true),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    time = table.Column<TimeOnly>(type: "time", nullable: true),
                    goal = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedCalls", x => x.id);
                    table.ForeignKey(
                        name: "FK_PlannedCalls_Contacts_contact_id",
                        column: x => x.contact_id,
                        principalTable: "Contacts",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    contact_id = table.Column<int>(type: "int", nullable: false),
                    legalperson_id = table.Column<int>(type: "int", nullable: false),
                    priority_contact = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    position = table.Column<string>(type: "varchar(40)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.contact_id);
                    table.ForeignKey(
                        name: "FK_Positions_Contacts_contact_id",
                        column: x => x.contact_id,
                        principalTable: "Contacts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Positions_LegalPersons_legalperson_id",
                        column: x => x.legalperson_id,
                        principalTable: "LegalPersons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_call_result_type_id",
                table: "Calls",
                column: "call_result_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_call_type_id",
                table: "Calls",
                column: "call_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_contact_id",
                table: "Calls",
                column: "contact_id");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_contactstatus_id",
                table: "Contacts",
                column: "contactstatus_id");

            migrationBuilder.CreateIndex(
                name: "IX_LegalPersons_taxsystem_id",
                table: "LegalPersons",
                column: "taxsystem_id");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumber_contact_id",
                table: "PhoneNumber",
                column: "contact_id");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedCalls_contact_id",
                table: "PlannedCalls",
                column: "contact_id");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_legalperson_id",
                table: "Positions",
                column: "legalperson_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calls");

            migrationBuilder.DropTable(
                name: "NaturalPersons");

            migrationBuilder.DropTable(
                name: "PhoneNumber");

            migrationBuilder.DropTable(
                name: "PlannedCalls");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "CallResultTypes");

            migrationBuilder.DropTable(
                name: "CallTypes");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "LegalPersons");

            migrationBuilder.DropTable(
                name: "ContactStatus");

            migrationBuilder.DropTable(
                name: "TaxSystems");
        }
    }
}
