using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // 🔹 ENTIDADES PRINCIPAIS
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Habilidade> Habilidades { get; set; }
        public DbSet<Troca> Trocas { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🟣 CONFIGURAÇÃO PARA ORACLE
            if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                // Exemplo: criar sequências no Oracle caso necessário
                modelBuilder.HasSequence<long>("SEQ_USUARIOS", schema: "RM556834");
                modelBuilder.HasSequence<long>("SEQ_HABILIDADES", schema: "RM556834");
                modelBuilder.HasSequence<long>("SEQ_TROCAS", schema: "RM556834");
                modelBuilder.HasSequence<long>("SEQ_TRANSACOES", schema: "RM556834");
                modelBuilder.HasSequence<long>("SEQ_AVALIACOES", schema: "RM556834");

                // Se quiser usar sequências do Oracle:
                // modelBuilder.Entity<Usuario>()
                //     .Property(u => u.Id)
                //     .HasDefaultValueSql("RM556834.SEQ_USUARIOS.NEXTVAL");
            }

            // 🔹 CONFIGURAÇÃO DE RELACIONAMENTOS ENTRE ENTIDADES

            // USUÁRIO → HABILIDADES (1:N)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Habilidades)
                .WithOne(h => h.Usuario)
                .HasForeignKey(h => h.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // USUÁRIO → TROCAS COMO MENTOR (1:N)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.TrocasComoMentor)
                .WithOne(t => t.Mentor)
                .HasForeignKey(t => t.MentorId)
                .OnDelete(DeleteBehavior.Restrict);

            // USUÁRIO → TROCAS COMO ALUNO (1:N)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.TrocasComoAluno)
                .WithOne(t => t.Aluno)
                .HasForeignKey(t => t.AlunoId)
                .OnDelete(DeleteBehavior.Restrict);

            // TROCA → HABILIDADE (1:N)
            modelBuilder.Entity<Troca>()
                .HasOne(t => t.Habilidade)
                .WithMany()
                .HasForeignKey(t => t.HabilidadeId);

            // TROCA → AVALIAÇÕES (1:N)
            modelBuilder.Entity<Troca>()
                .HasMany(t => t.Avaliacoes)
                .WithOne(a => a.Troca)
                .HasForeignKey(a => a.TrocaId);

            // TROCA → TRANSAÇÕES (1:N)
            modelBuilder.Entity<Troca>()
                .HasMany(t => t.Transacoes)
                .WithOne(tr => tr.Troca)
                .HasForeignKey(tr => tr.TrocaId);

            // USUÁRIO → AVALIAÇÕES FEITAS (1:N)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.AvaliacoesFeitas)
                .WithOne(a => a.Avaliador)
                .HasForeignKey(a => a.AvaliadorId)
                .OnDelete(DeleteBehavior.Restrict);

            // USUÁRIO → AVALIAÇÕES RECEBIDAS (1:N)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.AvaliacoesRecebidas)
                .WithOne(a => a.Avaliado)
                .HasForeignKey(a => a.AvaliadoId)
                .OnDelete(DeleteBehavior.Restrict);

            // USUÁRIO → TRANSAÇÕES COMO REMETENTE (1:N)
            modelBuilder.Entity<Usuario>()
                .HasMany<Usuario>()
                .WithMany();

            // Usuário como remetente
            modelBuilder.Entity<Transacao>()
                .HasOne(t => t.Remetente)
                .WithMany()
                .HasForeignKey(t => t.RemetenteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Usuário como destinatário
            modelBuilder.Entity<Transacao>()
                .HasOne(t => t.Destinatario)
                .WithMany()
                .HasForeignKey(t => t.DestinatarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
