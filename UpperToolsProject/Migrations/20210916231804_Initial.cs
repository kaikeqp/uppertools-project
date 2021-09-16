using Microsoft.EntityFrameworkCore.Migrations;

namespace UpperToolsProject.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    Cnpj = table.Column<string>(type: "varchar(767)", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    DataSituacao = table.Column<string>(type: "text", nullable: true),
                    MotivoSituacao = table.Column<string>(type: "text", nullable: true),
                    Tipo = table.Column<string>(type: "text", nullable: true),
                    Telefone = table.Column<string>(type: "text", nullable: true),
                    Situacao = table.Column<string>(type: "text", nullable: true),
                    Porte = table.Column<string>(type: "text", nullable: true),
                    Abertura = table.Column<string>(type: "text", nullable: true),
                    NaturezaJuridica = table.Column<string>(type: "text", nullable: true),
                    UltimaAtualizacao = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Fantasia = table.Column<string>(type: "text", nullable: true),
                    Logradouro = table.Column<string>(type: "text", nullable: true),
                    Numero = table.Column<string>(type: "text", nullable: true),
                    Complemento = table.Column<string>(type: "text", nullable: true),
                    Cep = table.Column<string>(type: "text", nullable: true),
                    Bairro = table.Column<string>(type: "text", nullable: true),
                    Municipio = table.Column<string>(type: "text", nullable: true),
                    Uf = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Efr = table.Column<string>(type: "text", nullable: true),
                    SituacaoEspecial = table.Column<string>(type: "text", nullable: true),
                    DataSituacaoEspecial = table.Column<string>(type: "text", nullable: true),
                    CapitalSocial = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Cnpj);
                });

            migrationBuilder.CreateTable(
                name: "Atividade",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(767)", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    EmpresaCnpj = table.Column<string>(type: "varchar(767)", nullable: true),
                    EmpresaCnpj1 = table.Column<string>(type: "varchar(767)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atividade", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Atividade_Empresa_EmpresaCnpj",
                        column: x => x.EmpresaCnpj,
                        principalTable: "Empresa",
                        principalColumn: "Cnpj",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atividade_Empresa_EmpresaCnpj1",
                        column: x => x.EmpresaCnpj1,
                        principalTable: "Empresa",
                        principalColumn: "Cnpj",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Qsa",
                columns: table => new
                {
                    Nome = table.Column<string>(type: "varchar(767)", nullable: false),
                    Qual = table.Column<string>(type: "text", nullable: true),
                    EmpresaCnpj = table.Column<string>(type: "varchar(767)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qsa", x => x.Nome);
                    table.ForeignKey(
                        name: "FK_Qsa_Empresa_EmpresaCnpj",
                        column: x => x.EmpresaCnpj,
                        principalTable: "Empresa",
                        principalColumn: "Cnpj",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atividade_EmpresaCnpj",
                table: "Atividade",
                column: "EmpresaCnpj");

            migrationBuilder.CreateIndex(
                name: "IX_Atividade_EmpresaCnpj1",
                table: "Atividade",
                column: "EmpresaCnpj1");

            migrationBuilder.CreateIndex(
                name: "IX_Qsa_EmpresaCnpj",
                table: "Qsa",
                column: "EmpresaCnpj");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atividade");

            migrationBuilder.DropTable(
                name: "Qsa");

            migrationBuilder.DropTable(
                name: "Empresa");
        }
    }
}
