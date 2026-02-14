using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace CuidaPetWeb.Migrations
{
    /// <inheritdoc />
    public partial class alteracao_identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "categoria",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    descricao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "especialidade",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    descricao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "especie",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "notificacao",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    titulo = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    descricao = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    dataEnvio = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pessoa",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cpf = table.Column<string>(type: "char(11)", fixedLength: true, maxLength: 11, nullable: false),
                    nome = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    senha = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    telefone = table.Column<string>(type: "char(11)", fixedLength: true, maxLength: 11, nullable: false),
                    tipo = table.Column<string>(type: "enum('T','G','A','V','Ad')", nullable: false, comment: "T (Tutor), G (Gerente), A (Atendente), V (Veterinário), Ad (Administrador)"),
                    status = table.Column<string>(type: "enum('A','I')", nullable: false, defaultValueSql: "'A'", comment: "A (Ativo), I (Inativo)"),
                    logradouro = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    numero = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    complemento = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    bairro = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    cidade = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    estado = table.Column<string>(type: "char(2)", fixedLength: true, maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "doenca",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    idEspecie = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "doenca_ibfk_1",
                        column: x => x.idEspecie,
                        principalTable: "especie",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "raca",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    idEspecie = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "raca_ibfk_1",
                        column: x => x.idEspecie,
                        principalTable: "especie",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "estabelecimento",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    tipo = table.Column<string>(type: "enum('C','P','A')", nullable: true, comment: "C(Clínica), P(Petshop), A(Ambos)"),
                    CNPJ = table.Column<string>(type: "char(14)", fixedLength: true, maxLength: 14, nullable: false),
                    telefone = table.Column<string>(type: "char(11)", fixedLength: true, maxLength: 11, nullable: false),
                    logradouro = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    numero = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    complemento = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    bairro = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    cidade = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    estado = table.Column<string>(type: "char(2)", fixedLength: true, maxLength: 2, nullable: false),
                    idGerente = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_Estabelecimento_Pessoa1",
                        column: x => x.idGerente,
                        principalTable: "pessoa",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pessoanotificacao",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    statusLida = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "0 - Não lida, 1 - Lida"),
                    idPessoa = table.Column<uint>(type: "int unsigned", nullable: false),
                    idNotificacao = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_Pessoa_has_Notificacao_Notificacao1",
                        column: x => x.idNotificacao,
                        principalTable: "notificacao",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_Pessoa_has_Notificacao_Pessoa1",
                        column: x => x.idPessoa,
                        principalTable: "pessoa",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vacina",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    periodoEmDias = table.Column<ushort>(type: "smallint unsigned", nullable: true),
                    idDoenca = table.Column<uint>(type: "int unsigned", nullable: false),
                    idEspecie = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_Vacina_Especie1",
                        column: x => x.idEspecie,
                        principalTable: "especie",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "vacina_ibfk_1",
                        column: x => x.idDoenca,
                        principalTable: "doenca",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pet",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    sexo = table.Column<string>(type: "enum('M','F')", nullable: false, comment: "M (Macho), F (Fêmea)"),
                    dataNascimento = table.Column<DateTime>(type: "date", nullable: true),
                    idRaca = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_Pet_Raca1",
                        column: x => x.idRaca,
                        principalTable: "raca",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "funcionario",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    crmv = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: true),
                    idPessoa = table.Column<uint>(type: "int unsigned", nullable: false),
                    idEstabelecimento = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_Funcionario_Estabelecimento1",
                        column: x => x.idEstabelecimento,
                        principalTable: "estabelecimento",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "veterinario_ibfk_2",
                        column: x => x.idPessoa,
                        principalTable: "pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "produto",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    preco = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false),
                    status = table.Column<string>(type: "enum('I','D','P')", nullable: true, defaultValueSql: "'D'", comment: "I (Indisponível), D (Disponível), P (Promoção)"),
                    precoPromocao = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: true),
                    descricao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    idCategoria = table.Column<uint>(type: "int unsigned", nullable: false),
                    idEstabelecimento = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_produto_estabelecimento1",
                        column: x => x.idEstabelecimento,
                        principalTable: "estabelecimento",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "produto_ibfk_1",
                        column: x => x.idCategoria,
                        principalTable: "categoria",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pessoapet",
                columns: table => new
                {
                    idPet = table.Column<uint>(type: "int unsigned", nullable: false),
                    idPessoa = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fk_Pet_has_Pessoa_Pessoa1",
                        column: x => x.idPessoa,
                        principalTable: "pessoa",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_Pet_has_Pessoa_Pet1",
                        column: x => x.idPet,
                        principalTable: "pet",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "petdoenca",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    dataDiagnostico = table.Column<DateTime>(type: "date", nullable: true),
                    idPet = table.Column<uint>(type: "int unsigned", nullable: false),
                    idDoenca = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "petdoenca_ibfk_1",
                        column: x => x.idPet,
                        principalTable: "pet",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "petdoenca_ibfk_2",
                        column: x => x.idDoenca,
                        principalTable: "doenca",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "agendamento",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    dataSolicitacao = table.Column<DateTime>(type: "date", nullable: false),
                    dataConfirmacao = table.Column<DateTime>(type: "date", nullable: true),
                    horario = table.Column<TimeSpan>(type: "time", nullable: false),
                    status = table.Column<string>(type: "enum('S','A','C','R')", nullable: false, comment: "S (Solicitado), A (Aprovado), C (Cancelado), R (Realizado)"),
                    idPet = table.Column<uint>(type: "int unsigned", nullable: false),
                    idFuncionario = table.Column<uint>(type: "int unsigned", nullable: false),
                    idTutor = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "agendamento_ibfk_1",
                        column: x => x.idPet,
                        principalTable: "pet",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "agendamento_ibfk_2",
                        column: x => x.idFuncionario,
                        principalTable: "funcionario",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_Agendamento_Pessoa1",
                        column: x => x.idTutor,
                        principalTable: "pessoa",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "funcionarioespecialidade",
                columns: table => new
                {
                    idFuncionario = table.Column<uint>(type: "int unsigned", nullable: false),
                    idEspecialidade = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.idFuncionario, x.idEspecialidade });
                    table.ForeignKey(
                        name: "fk_Funcionario_has_Especialidade_Especialidade1",
                        column: x => x.idEspecialidade,
                        principalTable: "especialidade",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_Funcionario_has_Especialidade_Funcionario1",
                        column: x => x.idFuncionario,
                        principalTable: "funcionario",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "horariosatendimento",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    diaSemana = table.Column<string>(type: "enum('DOM','SEG','TER','QUA','QUI','SEX','SAB')", nullable: false, comment: "\n"),
                    horario = table.Column<TimeSpan>(type: "time", nullable: false),
                    idFuncionario = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "horariosatendimento_ibfk_1",
                        column: x => x.idFuncionario,
                        principalTable: "funcionario",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vacinacao",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    dataVacina = table.Column<DateTime>(type: "date", nullable: false),
                    lote = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    idVacina = table.Column<uint>(type: "int unsigned", nullable: false),
                    idPet = table.Column<uint>(type: "int unsigned", nullable: false),
                    idFuncionario = table.Column<uint>(type: "int unsigned", nullable: false),
                    idTutor = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_VacinaPet_Funcionario1",
                        column: x => x.idFuncionario,
                        principalTable: "funcionario",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_Vacinacao_Pessoa1",
                        column: x => x.idTutor,
                        principalTable: "pessoa",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "vacinapet_ibfk_1",
                        column: x => x.idVacina,
                        principalTable: "vacina",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "vacinapet_ibfk_2",
                        column: x => x.idPet,
                        principalTable: "pet",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "consulta",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    dataConsulta = table.Column<DateTime>(type: "datetime", nullable: false),
                    anotacoes = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true),
                    idTutor = table.Column<uint>(type: "int unsigned", nullable: false),
                    idPet = table.Column<uint>(type: "int unsigned", nullable: false),
                    idFuncionario = table.Column<uint>(type: "int unsigned", nullable: false),
                    idAgendamento = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_Consulta_Agendamento1",
                        column: x => x.idAgendamento,
                        principalTable: "agendamento",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_Consulta_Funcionario2",
                        column: x => x.idFuncionario,
                        principalTable: "funcionario",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_Consulta_Pessoa1",
                        column: x => x.idTutor,
                        principalTable: "pessoa",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_Consulta_Pet1",
                        column: x => x.idPet,
                        principalTable: "pet",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pedido",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    status = table.Column<string>(type: "enum('A','F','C')", nullable: false, comment: "A = Andamento, F = Finalizado, C = Cancelado"),
                    realizadoEm = table.Column<DateTime>(type: "datetime", nullable: false),
                    idTutor = table.Column<uint>(type: "int unsigned", nullable: false),
                    idFuncionario = table.Column<uint>(type: "int unsigned", nullable: false),
                    idAgendamento = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_Pedido_Agendamento1",
                        column: x => x.idAgendamento,
                        principalTable: "agendamento",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_Pedido_Funcionario1",
                        column: x => x.idFuncionario,
                        principalTable: "funcionario",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "pedido_ibfk_1",
                        column: x => x.idTutor,
                        principalTable: "pessoa",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pedidoproduto",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    preco = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false),
                    idProduto = table.Column<uint>(type: "int unsigned", nullable: false),
                    idPedido = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "produtopedido_ibfk_1",
                        column: x => x.idProduto,
                        principalTable: "produto",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "produtopedido_ibfk_2",
                        column: x => x.idPedido,
                        principalTable: "pedido",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "fk_Agendamento_Pessoa1_idx",
                table: "agendamento",
                column: "idTutor");

            migrationBuilder.CreateIndex(
                name: "idPet",
                table: "agendamento",
                column: "idPet");

            migrationBuilder.CreateIndex(
                name: "idVeterinario",
                table: "agendamento",
                column: "idFuncionario");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_Consulta_Agendamento1_idx",
                table: "consulta",
                column: "idAgendamento");

            migrationBuilder.CreateIndex(
                name: "fk_Consulta_Funcionario2_idx",
                table: "consulta",
                column: "idFuncionario");

            migrationBuilder.CreateIndex(
                name: "fk_Consulta_Pessoa1_idx",
                table: "consulta",
                column: "idTutor");

            migrationBuilder.CreateIndex(
                name: "fk_Consulta_Pet1_idx",
                table: "consulta",
                column: "idPet");

            migrationBuilder.CreateIndex(
                name: "idEspecie",
                table: "doenca",
                column: "idEspecie");

            migrationBuilder.CreateIndex(
                name: "CNPJ",
                table: "estabelecimento",
                column: "CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_Estabelecimento_Pessoa1_idx",
                table: "estabelecimento",
                column: "idGerente");

            migrationBuilder.CreateIndex(
                name: "crmv",
                table: "funcionario",
                column: "crmv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_Funcionario_Estabelecimento1_idx",
                table: "funcionario",
                column: "idEstabelecimento");

            migrationBuilder.CreateIndex(
                name: "idUsuario",
                table: "funcionario",
                column: "idPessoa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_Funcionario_has_Especialidade_Especialidade1_idx",
                table: "funcionarioespecialidade",
                column: "idEspecialidade");

            migrationBuilder.CreateIndex(
                name: "fk_Funcionario_has_Especialidade_Funcionario1_idx",
                table: "funcionarioespecialidade",
                column: "idFuncionario");

            migrationBuilder.CreateIndex(
                name: "idVeterinario1",
                table: "horariosatendimento",
                column: "idFuncionario");

            migrationBuilder.CreateIndex(
                name: "fk_Pedido_Agendamento1_idx",
                table: "pedido",
                column: "idAgendamento");

            migrationBuilder.CreateIndex(
                name: "fk_Pedido_Funcionario1_idx",
                table: "pedido",
                column: "idFuncionario");

            migrationBuilder.CreateIndex(
                name: "idUsuario1",
                table: "pedido",
                column: "idTutor");

            migrationBuilder.CreateIndex(
                name: "idPedido",
                table: "pedidoproduto",
                column: "idPedido");

            migrationBuilder.CreateIndex(
                name: "idProduto",
                table: "pedidoproduto",
                column: "idProduto");

            migrationBuilder.CreateIndex(
                name: "cpf_UNIQUE",
                table: "pessoa",
                column: "cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "email",
                table: "pessoa",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "telefone_UNIQUE",
                table: "pessoa",
                column: "telefone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_Pessoa_has_Notificacao_Notificacao1_idx",
                table: "pessoanotificacao",
                column: "idNotificacao");

            migrationBuilder.CreateIndex(
                name: "fk_Pessoa_has_Notificacao_Pessoa1_idx",
                table: "pessoanotificacao",
                column: "idPessoa");

            migrationBuilder.CreateIndex(
                name: "fk_Pet_has_Pessoa_Pessoa1_idx",
                table: "pessoapet",
                column: "idPessoa");

            migrationBuilder.CreateIndex(
                name: "fk_Pet_has_Pessoa_Pet1_idx",
                table: "pessoapet",
                column: "idPet");

            migrationBuilder.CreateIndex(
                name: "fk_Pet_Raca1_idx",
                table: "pet",
                column: "idRaca");

            migrationBuilder.CreateIndex(
                name: "idDoenca",
                table: "petdoenca",
                column: "idDoenca");

            migrationBuilder.CreateIndex(
                name: "idPet1",
                table: "petdoenca",
                column: "idPet");

            migrationBuilder.CreateIndex(
                name: "fk_produto_estabelecimento1_idx",
                table: "produto",
                column: "idEstabelecimento");

            migrationBuilder.CreateIndex(
                name: "idCategoria",
                table: "produto",
                column: "idCategoria");

            migrationBuilder.CreateIndex(
                name: "idEspecie1",
                table: "raca",
                column: "idEspecie");

            migrationBuilder.CreateIndex(
                name: "fk_Vacina_Especie1_idx",
                table: "vacina",
                column: "idEspecie");

            migrationBuilder.CreateIndex(
                name: "idDoenca1",
                table: "vacina",
                column: "idDoenca");

            migrationBuilder.CreateIndex(
                name: "fk_Vacinacao_Pessoa1_idx",
                table: "vacinacao",
                column: "idTutor");

            migrationBuilder.CreateIndex(
                name: "fk_VacinaPet_Funcionario1_idx",
                table: "vacinacao",
                column: "idFuncionario");

            migrationBuilder.CreateIndex(
                name: "idPet2",
                table: "vacinacao",
                column: "idPet");

            migrationBuilder.CreateIndex(
                name: "idVacina",
                table: "vacinacao",
                column: "idVacina");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "consulta");

            migrationBuilder.DropTable(
                name: "funcionarioespecialidade");

            migrationBuilder.DropTable(
                name: "horariosatendimento");

            migrationBuilder.DropTable(
                name: "pedidoproduto");

            migrationBuilder.DropTable(
                name: "pessoanotificacao");

            migrationBuilder.DropTable(
                name: "pessoapet");

            migrationBuilder.DropTable(
                name: "petdoenca");

            migrationBuilder.DropTable(
                name: "vacinacao");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "especialidade");

            migrationBuilder.DropTable(
                name: "produto");

            migrationBuilder.DropTable(
                name: "pedido");

            migrationBuilder.DropTable(
                name: "notificacao");

            migrationBuilder.DropTable(
                name: "vacina");

            migrationBuilder.DropTable(
                name: "categoria");

            migrationBuilder.DropTable(
                name: "agendamento");

            migrationBuilder.DropTable(
                name: "doenca");

            migrationBuilder.DropTable(
                name: "pet");

            migrationBuilder.DropTable(
                name: "funcionario");

            migrationBuilder.DropTable(
                name: "raca");

            migrationBuilder.DropTable(
                name: "estabelecimento");

            migrationBuilder.DropTable(
                name: "especie");

            migrationBuilder.DropTable(
                name: "pessoa");
        }
    }
}
