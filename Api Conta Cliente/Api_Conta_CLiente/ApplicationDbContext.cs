using Api_Conta_Cliente.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Cliente>? Clientes { get; set; }
    public DbSet<Conta>? Contas { get; set; }
    public DbSet<Agencia>? Agencias { get; set; }
    public DbSet<TipoConta>? TipoContas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(c => c.Cpf);
            entity.Property(c => c.Cpf).HasMaxLength(11);
            entity.Property(c => c.Nome).HasMaxLength(64);
            //entity.HasMany(c => c.Contas).WithOne(c => c.Cliente).HasForeignKey(c => c.Cpf);
        });
        modelBuilder.Entity<Conta>(entity =>
        {
            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasKey(c => c.Numero_Conta);
            //entity.HasOne(c => c.Cliente).WithMany(c => c.Contas).HasForeignKey(c => c.Cpf);
            //entity.HasOne(c => c.Agencia).WithMany(a => a.Contas).HasForeignKey(c => c.Numero_Agencia);
            //entity.HasOne(c => c.TipoConta).WithMany(tc => tc.Contas).HasForeignKey(c => c.Codigo_Tipo_Conta);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
            entity.Property(c => c.Numero_Conta).HasMaxLength(6);
            entity.Property(c => c.Digito).HasMaxLength(1);
        });
        modelBuilder.Entity<Agencia>(entity =>
        {
            entity.HasKey(a => a.Numero_Agencia);
            entity.Property(a => a.Numero_Agencia).HasMaxLength(5);
            entity.Property(a => a.Descricao).HasMaxLength(128);
            //entity.HasMany(c => c.Contas).WithOne(c => c.Agencia);
        });
        modelBuilder.Entity<TipoConta>(entity =>
        {
            entity.HasKey(tp => tp.Codigo_Conta);
            entity.Property(tp => tp.Descricao).HasMaxLength(128);
            //entity.HasMany(c => c.Contas).WithOne(c => c.TipoConta);
        });

    }



}