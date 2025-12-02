using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Core;

public partial class CuidaPetContext : DbContext
{
    public CuidaPetContext()
    {
    }

    public CuidaPetContext(DbContextOptions<CuidaPetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agendamento> Agendamentos { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Consultum> Consulta { get; set; }

    public virtual DbSet<Doenca> Doencas { get; set; }

    public virtual DbSet<Especialidade> Especialidades { get; set; }

    public virtual DbSet<Especie> Especies { get; set; }

    public virtual DbSet<Estabelecimento> Estabelecimentos { get; set; }

    public virtual DbSet<Funcionario> Funcionarios { get; set; }

    public virtual DbSet<Horariosatendimento> Horariosatendimentos { get; set; }

    public virtual DbSet<Notificacao> Notificacaos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Pedidoproduto> Pedidoprodutos { get; set; }

    public virtual DbSet<Pessoa> Pessoas { get; set; }

    public virtual DbSet<Pessoanotificacao> Pessoanotificacaos { get; set; }

    public virtual DbSet<Pessoapet> Pessoapets { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<Petdoenca> Petdoencas { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<Raca> Racas { get; set; }

    public virtual DbSet<Vacina> Vacinas { get; set; }

    public virtual DbSet<Vacinacao> Vacinacaos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=123456;database=cuidapetdb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agendamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("agendamento");

            entity.HasIndex(e => e.IdTutor, "fk_Agendamento_Pessoa1_idx");

            entity.HasIndex(e => e.IdPet, "idPet");

            entity.HasIndex(e => e.IdFuncionario, "idVeterinario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataConfirmacao)
                .HasColumnType("date")
                .HasColumnName("dataConfirmacao");
            entity.Property(e => e.DataSolicitacao)
                .HasColumnType("date")
                .HasColumnName("dataSolicitacao");
            entity.Property(e => e.Horario)
                .HasColumnType("time")
                .HasColumnName("horario");
            entity.Property(e => e.IdFuncionario).HasColumnName("idFuncionario");
            entity.Property(e => e.IdPet).HasColumnName("idPet");
            entity.Property(e => e.IdTutor).HasColumnName("idTutor");
            entity.Property(e => e.Status)
                .HasComment("S (Solicitado), A (Aprovado), C (Cancelado), R (Realizado)")
                .HasColumnType("enum('S','A','C','R')")
                .HasColumnName("status");

            entity.HasOne(d => d.IdFuncionarioNavigation).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.IdFuncionario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("agendamento_ibfk_2");

            entity.HasOne(d => d.IdPetNavigation).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.IdPet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("agendamento_ibfk_1");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Agendamento_Pessoa1");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao");
            entity.Property(e => e.Nome)
                .HasMaxLength(30)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Consultum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("consulta");

            entity.HasIndex(e => e.IdAgendamento, "fk_Consulta_Agendamento1_idx");

            entity.HasIndex(e => e.IdFuncionario, "fk_Consulta_Funcionario2_idx");

            entity.HasIndex(e => e.IdTutor, "fk_Consulta_Pessoa1_idx");

            entity.HasIndex(e => e.IdPet, "fk_Consulta_Pet1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Anotacoes)
                .HasMaxLength(512)
                .HasColumnName("anotacoes");
            entity.Property(e => e.DataConsulta)
                .HasColumnType("datetime")
                .HasColumnName("dataConsulta");
            entity.Property(e => e.IdAgendamento).HasColumnName("idAgendamento");
            entity.Property(e => e.IdFuncionario).HasColumnName("idFuncionario");
            entity.Property(e => e.IdPet).HasColumnName("idPet");
            entity.Property(e => e.IdTutor).HasColumnName("idTutor");

            entity.HasOne(d => d.IdAgendamentoNavigation).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.IdAgendamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Consulta_Agendamento1");

            entity.HasOne(d => d.IdFuncionarioNavigation).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.IdFuncionario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Consulta_Funcionario2");

            entity.HasOne(d => d.IdPetNavigation).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.IdPet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Consulta_Pet1");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Consulta_Pessoa1");
        });

        modelBuilder.Entity<Doenca>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("doenca");

            entity.HasIndex(e => e.IdEspecie, "idEspecie");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdEspecie).HasColumnName("idEspecie");
            entity.Property(e => e.Nome)
                .HasMaxLength(30)
                .HasColumnName("nome");

            entity.HasOne(d => d.IdEspecieNavigation).WithMany(p => p.Doencas)
                .HasForeignKey(d => d.IdEspecie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("doenca_ibfk_1");
        });

        modelBuilder.Entity<Especialidade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("especialidade");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao");
            entity.Property(e => e.Nome)
                .HasMaxLength(30)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Especie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("especie");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Estabelecimento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("estabelecimento");

            entity.HasIndex(e => e.Cnpj, "CNPJ").IsUnique();

            entity.HasIndex(e => e.IdGerente, "fk_Estabelecimento_Pessoa1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(50)
                .HasColumnName("bairro");
            entity.Property(e => e.Cidade)
                .HasMaxLength(100)
                .HasColumnName("cidade");
            entity.Property(e => e.Cnpj)
                .HasMaxLength(14)
                .IsFixedLength()
                .HasColumnName("CNPJ");
            entity.Property(e => e.Complemento)
                .HasMaxLength(50)
                .HasColumnName("complemento");
            entity.Property(e => e.Estado)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("estado");
            entity.Property(e => e.IdGerente).HasColumnName("idGerente");
            entity.Property(e => e.Logradouro)
                .HasMaxLength(100)
                .HasColumnName("logradouro");
            entity.Property(e => e.Nome)
                .HasMaxLength(30)
                .HasColumnName("nome");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .HasColumnName("numero");
            entity.Property(e => e.Telefone)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("telefone");
            entity.Property(e => e.Tipo)
                .HasComment("C(Clínica), P(Petshop), A(Ambos)")
                .HasColumnType("enum('C','P','A')")
                .HasColumnName("tipo");

            entity.HasOne(d => d.IdGerenteNavigation).WithMany(p => p.Estabelecimentos)
                .HasForeignKey(d => d.IdGerente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Estabelecimento_Pessoa1");
        });

        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("funcionario");

            entity.HasIndex(e => e.Crmv, "crmv").IsUnique();

            entity.HasIndex(e => e.IdEstabelecimento, "fk_Funcionario_Estabelecimento1_idx");

            entity.HasIndex(e => e.IdPessoa, "idUsuario").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Crmv)
                .HasMaxLength(7)
                .HasColumnName("crmv");
            entity.Property(e => e.IdEstabelecimento).HasColumnName("idEstabelecimento");
            entity.Property(e => e.IdPessoa).HasColumnName("idPessoa");

            entity.HasOne(d => d.IdEstabelecimentoNavigation).WithMany(p => p.Funcionarios)
                .HasForeignKey(d => d.IdEstabelecimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Funcionario_Estabelecimento1");

            entity.HasOne(d => d.IdPessoaNavigation).WithOne(p => p.Funcionario)
                .HasForeignKey<Funcionario>(d => d.IdPessoa)
                .HasConstraintName("veterinario_ibfk_2");

            entity.HasMany(d => d.IdEspecialidades).WithMany(p => p.IdFuncionarios)
                .UsingEntity<Dictionary<string, object>>(
                    "Funcionarioespecialidade",
                    r => r.HasOne<Especialidade>().WithMany()
                        .HasForeignKey("IdEspecialidade")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_Funcionario_has_Especialidade_Especialidade1"),
                    l => l.HasOne<Funcionario>().WithMany()
                        .HasForeignKey("IdFuncionario")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_Funcionario_has_Especialidade_Funcionario1"),
                    j =>
                    {
                        j.HasKey("IdFuncionario", "IdEspecialidade").HasName("PRIMARY");
                        j.ToTable("funcionarioespecialidade");
                        j.HasIndex(new[] { "IdEspecialidade" }, "fk_Funcionario_has_Especialidade_Especialidade1_idx");
                        j.HasIndex(new[] { "IdFuncionario" }, "fk_Funcionario_has_Especialidade_Funcionario1_idx");
                        j.IndexerProperty<uint>("IdFuncionario").HasColumnName("idFuncionario");
                        j.IndexerProperty<uint>("IdEspecialidade").HasColumnName("idEspecialidade");
                    });
        });

        modelBuilder.Entity<Horariosatendimento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("horariosatendimento");

            entity.HasIndex(e => e.IdFuncionario, "idVeterinario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiaSemana)
                .HasComment("\n")
                .HasColumnType("enum('DOM','SEG','TER','QUA','QUI','SEX','SAB')")
                .HasColumnName("diaSemana");
            entity.Property(e => e.Horario)
                .HasColumnType("time")
                .HasColumnName("horario");
            entity.Property(e => e.IdFuncionario).HasColumnName("idFuncionario");

            entity.HasOne(d => d.IdFuncionarioNavigation).WithMany(p => p.Horariosatendimentos)
                .HasForeignKey(d => d.IdFuncionario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("horariosatendimento_ibfk_1");
        });

        modelBuilder.Entity<Notificacao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("notificacao");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataEnvio)
                .HasColumnType("datetime")
                .HasColumnName("dataEnvio");
            entity.Property(e => e.Descricao)
                .HasMaxLength(150)
                .HasColumnName("descricao");
            entity.Property(e => e.Titulo)
                .HasMaxLength(45)
                .HasColumnName("titulo");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pedido");

            entity.HasIndex(e => e.IdAgendamento, "fk_Pedido_Agendamento1_idx");

            entity.HasIndex(e => e.IdFuncionario, "fk_Pedido_Funcionario1_idx");

            entity.HasIndex(e => e.IdTutor, "idUsuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdAgendamento).HasColumnName("idAgendamento");
            entity.Property(e => e.IdFuncionario).HasColumnName("idFuncionario");
            entity.Property(e => e.IdTutor).HasColumnName("idTutor");
            entity.Property(e => e.RealizadoEm)
                .HasColumnType("datetime")
                .HasColumnName("realizadoEm");
            entity.Property(e => e.Status)
                .HasComment("A = Andamento, F = Finalizado, C = Cancelado")
                .HasColumnType("enum('A','F','C')")
                .HasColumnName("status");

            entity.HasOne(d => d.IdAgendamentoNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdAgendamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pedido_Agendamento1");

            entity.HasOne(d => d.IdFuncionarioNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdFuncionario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pedido_Funcionario1");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pedido_ibfk_1");
        });

        modelBuilder.Entity<Pedidoproduto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pedidoproduto");

            entity.HasIndex(e => e.IdPedido, "idPedido");

            entity.HasIndex(e => e.IdProduto, "idProduto");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.IdProduto).HasColumnName("idProduto");
            entity.Property(e => e.Preco)
                .HasPrecision(10)
                .HasColumnName("preco");
            entity.Property(e => e.Quantidade).HasColumnName("quantidade");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.Pedidoprodutos)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produtopedido_ibfk_2");

            entity.HasOne(d => d.IdProdutoNavigation).WithMany(p => p.Pedidoprodutos)
                .HasForeignKey(d => d.IdProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produtopedido_ibfk_1");
        });

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pessoa");

            entity.HasIndex(e => e.Cpf, "cpf_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.Telefone, "telefone_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(50)
                .HasColumnName("bairro");
            entity.Property(e => e.Cidade)
                .HasMaxLength(100)
                .HasColumnName("cidade");
            entity.Property(e => e.Complemento)
                .HasMaxLength(50)
                .HasColumnName("complemento");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("cpf");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("estado");
            entity.Property(e => e.Logradouro)
                .HasMaxLength(100)
                .HasColumnName("logradouro");
            entity.Property(e => e.Nome)
                .HasMaxLength(30)
                .HasColumnName("nome");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .HasColumnName("numero");
            entity.Property(e => e.Senha)
                .HasMaxLength(150)
                .HasColumnName("senha");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'A'")
                .HasComment("A (Ativo), I (Inativo)")
                .HasColumnType("enum('A','I')")
                .HasColumnName("status");
            entity.Property(e => e.Telefone)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("telefone");
            entity.Property(e => e.Tipo)
                .HasComment("T (Tutor), G (Gerente), A (Atendente), V (Veterinário), Ad (Administrador)")
                .HasColumnType("enum('T','G','A','V','Ad')")
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Pessoanotificacao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pessoanotificacao");

            entity.HasIndex(e => e.IdNotificacao, "fk_Pessoa_has_Notificacao_Notificacao1_idx");

            entity.HasIndex(e => e.IdPessoa, "fk_Pessoa_has_Notificacao_Pessoa1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdNotificacao).HasColumnName("idNotificacao");
            entity.Property(e => e.IdPessoa).HasColumnName("idPessoa");
            entity.Property(e => e.StatusLida)
                .HasComment("0 - Não lida, 1 - Lida")
                .HasColumnName("statusLida");

            entity.HasOne(d => d.IdNotificacaoNavigation).WithMany(p => p.Pessoanotificacaos)
                .HasForeignKey(d => d.IdNotificacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pessoa_has_Notificacao_Notificacao1");

            entity.HasOne(d => d.IdPessoaNavigation).WithMany(p => p.Pessoanotificacaos)
                .HasForeignKey(d => d.IdPessoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pessoa_has_Notificacao_Pessoa1");
        });

        modelBuilder.Entity<Pessoapet>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("pessoapet");

            entity.HasIndex(e => e.IdPessoa, "fk_Pet_has_Pessoa_Pessoa1_idx");

            entity.HasIndex(e => e.IdPet, "fk_Pet_has_Pessoa_Pet1_idx");

            entity.Property(e => e.IdPessoa).HasColumnName("idPessoa");
            entity.Property(e => e.IdPet).HasColumnName("idPet");

            entity.HasOne(d => d.IdPessoaNavigation).WithMany()
                .HasForeignKey(d => d.IdPessoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pet_has_Pessoa_Pessoa1");

            entity.HasOne(d => d.IdPetNavigation).WithMany()
                .HasForeignKey(d => d.IdPet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pet_has_Pessoa_Pet1");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pet");

            entity.HasIndex(e => e.IdRaca, "fk_Pet_Raca1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataNascimento)
                .HasColumnType("date")
                .HasColumnName("dataNascimento");
            entity.Property(e => e.IdRaca).HasColumnName("idRaca");
            entity.Property(e => e.Nome)
                .HasMaxLength(20)
                .HasColumnName("nome");
            entity.Property(e => e.Sexo)
                .HasComment("M (Macho), F (Fêmea)")
                .HasColumnType("enum('M','F')")
                .HasColumnName("sexo");

            entity.HasOne(d => d.IdRacaNavigation).WithMany(p => p.Pets)
                .HasForeignKey(d => d.IdRaca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pet_Raca1");
        });

        modelBuilder.Entity<Petdoenca>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("petdoenca");

            entity.HasIndex(e => e.IdDoenca, "idDoenca");

            entity.HasIndex(e => e.IdPet, "idPet");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataDiagnostico)
                .HasColumnType("date")
                .HasColumnName("dataDiagnostico");
            entity.Property(e => e.IdDoenca).HasColumnName("idDoenca");
            entity.Property(e => e.IdPet).HasColumnName("idPet");

            entity.HasOne(d => d.IdDoencaNavigation).WithMany(p => p.Petdoencas)
                .HasForeignKey(d => d.IdDoenca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("petdoenca_ibfk_2");

            entity.HasOne(d => d.IdPetNavigation).WithMany(p => p.Petdoencas)
                .HasForeignKey(d => d.IdPet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("petdoenca_ibfk_1");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("produto");

            entity.HasIndex(e => e.IdEstabelecimento, "fk_produto_estabelecimento1_idx");

            entity.HasIndex(e => e.IdCategoria, "idCategoria");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao");
            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.IdEstabelecimento).HasColumnName("idEstabelecimento");
            entity.Property(e => e.Nome)
                .HasMaxLength(30)
                .HasColumnName("nome");
            entity.Property(e => e.Preco)
                .HasPrecision(10)
                .HasColumnName("preco");
            entity.Property(e => e.PrecoPromocao)
                .HasPrecision(10)
                .HasColumnName("precoPromocao");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'D'")
                .HasComment("I (Indisponível), D (Disponível), P (Promoção)")
                .HasColumnType("enum('I','D','P')")
                .HasColumnName("status");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_ibfk_1");

            entity.HasOne(d => d.IdEstabelecimentoNavigation).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.IdEstabelecimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_produto_estabelecimento1");
        });

        modelBuilder.Entity<Raca>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("raca");

            entity.HasIndex(e => e.IdEspecie, "idEspecie");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdEspecie).HasColumnName("idEspecie");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");

            entity.HasOne(d => d.IdEspecieNavigation).WithMany(p => p.Racas)
                .HasForeignKey(d => d.IdEspecie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("raca_ibfk_1");
        });

        modelBuilder.Entity<Vacina>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("vacina");

            entity.HasIndex(e => e.IdEspecie, "fk_Vacina_Especie1_idx");

            entity.HasIndex(e => e.IdDoenca, "idDoenca");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdDoenca).HasColumnName("idDoenca");
            entity.Property(e => e.IdEspecie).HasColumnName("idEspecie");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.PeriodoEmDias).HasColumnName("periodoEmDias");

            entity.HasOne(d => d.IdDoencaNavigation).WithMany(p => p.Vacinas)
                .HasForeignKey(d => d.IdDoenca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vacina_ibfk_1");

            entity.HasOne(d => d.IdEspecieNavigation).WithMany(p => p.Vacinas)
                .HasForeignKey(d => d.IdEspecie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Vacina_Especie1");
        });

        modelBuilder.Entity<Vacinacao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("vacinacao");

            entity.HasIndex(e => e.IdFuncionario, "fk_VacinaPet_Funcionario1_idx");

            entity.HasIndex(e => e.IdTutor, "fk_Vacinacao_Pessoa1_idx");

            entity.HasIndex(e => e.IdPet, "idPet");

            entity.HasIndex(e => e.IdVacina, "idVacina");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataVacina)
                .HasColumnType("date")
                .HasColumnName("dataVacina");
            entity.Property(e => e.IdFuncionario).HasColumnName("idFuncionario");
            entity.Property(e => e.IdPet).HasColumnName("idPet");
            entity.Property(e => e.IdTutor).HasColumnName("idTutor");
            entity.Property(e => e.IdVacina).HasColumnName("idVacina");
            entity.Property(e => e.Lote)
                .HasMaxLength(20)
                .HasColumnName("lote");

            entity.HasOne(d => d.IdFuncionarioNavigation).WithMany(p => p.Vacinacaos)
                .HasForeignKey(d => d.IdFuncionario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_VacinaPet_Funcionario1");

            entity.HasOne(d => d.IdPetNavigation).WithMany(p => p.Vacinacaos)
                .HasForeignKey(d => d.IdPet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vacinapet_ibfk_2");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Vacinacaos)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Vacinacao_Pessoa1");

            entity.HasOne(d => d.IdVacinaNavigation).WithMany(p => p.Vacinacaos)
                .HasForeignKey(d => d.IdVacina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vacinapet_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
