using Todo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Todo.Data.Mappings
{
    public class TodoMap : IEntityTypeConfiguration<TodoModel>
    {
        public void Configure(EntityTypeBuilder<TodoModel> builder)
        {
            // Tabela
            builder.ToTable("Todo");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(); // IDENTITY(1, 1)

            // Propriedades
            builder.Property(x => x.Title)
                .IsRequired() // NOT NULL
                .HasColumnName("Title")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.Done)
               .IsRequired() // NOT NULL
               .HasColumnName("Done")
               .HasColumnType("BIT");

            builder.Property(x => x.CreatedAt)
               .IsRequired()
                .HasColumnName("CreatedAt")
                .HasColumnType("SMALLDATETIME")
                .HasDefaultValueSql("datetime('now')");
            //.HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.User) //Todo tem um User
                .WithMany(x => x.Todos) //O User tem muitos Todos
                .HasConstraintName("FK_Post_User")
                .OnDelete(DeleteBehavior.Cascade);// Quando o Todo for deletado, deleta também o User

        }
    }
}
