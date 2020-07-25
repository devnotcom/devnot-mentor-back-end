using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DevnotMentor.Api.Entities
{
    public partial class MentorDBContext : DbContext
    {
        public MentorDBContext()
        {
        }

        public MentorDBContext(DbContextOptions<MentorDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LinkType> LinkType { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Mentee> Mentee { get; set; }
        public virtual DbSet<MenteeAnswers> MenteeAnswers { get; set; }
        public virtual DbSet<MenteeLinks> MenteeLinks { get; set; }
        public virtual DbSet<MenteeTags> MenteeTags { get; set; }
        public virtual DbSet<Mentor> Mentor { get; set; }
        public virtual DbSet<MentorApplications> MentorApplications { get; set; }
        public virtual DbSet<MentorLinks> MentorLinks { get; set; }
        public virtual DbSet<MentorMenteePairs> MentorMenteePairs { get; set; }
        public virtual DbSet<MentorQuestions> MentorQuestions { get; set; }
        public virtual DbSet<MentorTags> MentorTags { get; set; }
        public virtual DbSet<QuestionType> QuestionType { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-8TSS65S;Database=MentorDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LinkType>(entity =>
            {
                entity.Property(e => e.BaseLink)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Level)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Message)
                    .HasMaxLength(2000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Mentee>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Mentee)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Mentee_User");
            });

            modelBuilder.Entity<MenteeAnswers>(entity =>
            {
                entity.HasIndex(e => e.MenteeId);

                entity.HasIndex(e => e.MentorQuestionId);

                entity.Property(e => e.Answer)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Mentee)
                    .WithMany(p => p.MenteeAnswers)
                    .HasForeignKey(d => d.MenteeId)
                    .HasConstraintName("FK_MenteeAnswers_Mentee");

                entity.HasOne(d => d.MentorQuestion)
                    .WithMany(p => p.MenteeAnswers)
                    .HasForeignKey(d => d.MentorQuestionId)
                    .HasConstraintName("FK_MenteeAnswers_MentorQuestions");
            });

            modelBuilder.Entity<MenteeLinks>(entity =>
            {
                entity.HasIndex(e => e.LinkTypeId);

                entity.HasIndex(e => e.MenteeId);

                entity.Property(e => e.Link)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.LinkType)
                    .WithMany(p => p.MenteeLinks)
                    .HasForeignKey(d => d.LinkTypeId)
                    .HasConstraintName("FK_MenteeLinks_LinkType");

                entity.HasOne(d => d.Mentee)
                    .WithMany(p => p.MenteeLinks)
                    .HasForeignKey(d => d.MenteeId)
                    .HasConstraintName("FK_MenteeLinks_Mentee");
            });

            modelBuilder.Entity<MenteeTags>(entity =>
            {
                entity.HasIndex(e => e.MenteeId);

                entity.HasIndex(e => e.TagId);

                entity.HasOne(d => d.Mentee)
                    .WithMany(p => p.MenteeTags)
                    .HasForeignKey(d => d.MenteeId)
                    .HasConstraintName("FK_MenteeTags_Mentee");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.MenteeTags)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK_MenteeTags_Tag");
            });

            modelBuilder.Entity<Mentor>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Mentor)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Mentor_User");
            });

            modelBuilder.Entity<MentorApplications>(entity =>
            {
                entity.HasIndex(e => e.MenteeId);

                entity.HasIndex(e => e.MentorId);

                entity.Property(e => e.ApllicationNotes)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ApplyDate).HasColumnType("datetime");

                entity.Property(e => e.CompleteDate).HasColumnType("datetime");

                entity.HasOne(d => d.Mentee)
                    .WithMany(p => p.MentorApplications)
                    .HasForeignKey(d => d.MenteeId)
                    .HasConstraintName("FK_MentorApplications_Mentee");

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.MentorApplications)
                    .HasForeignKey(d => d.MentorId)
                    .HasConstraintName("FK_MentorApplications_Mentor");
            });

            modelBuilder.Entity<MentorLinks>(entity =>
            {
                entity.HasIndex(e => e.LinkTypeId);

                entity.HasIndex(e => e.MentorId);

                entity.Property(e => e.Link)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.LinkType)
                    .WithMany(p => p.MentorLinks)
                    .HasForeignKey(d => d.LinkTypeId)
                    .HasConstraintName("FK_MentorLinks_LinkType");

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.MentorLinks)
                    .HasForeignKey(d => d.MentorId)
                    .HasConstraintName("FK_MentorLinks_Mentor");
            });

            modelBuilder.Entity<MentorMenteePairs>(entity =>
            {
                entity.HasIndex(e => e.MenteeId);

                entity.HasIndex(e => e.MentorId);

                entity.Property(e => e.MenteeComment)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.MentorComment)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.MentorEndDate).HasColumnType("datetime");

                entity.Property(e => e.MentorStartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Mentee)
                    .WithMany(p => p.MentorMenteePairs)
                    .HasForeignKey(d => d.MenteeId)
                    .HasConstraintName("FK_MentorMenteePairs_Mentee");

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.MentorMenteePairs)
                    .HasForeignKey(d => d.MentorId)
                    .HasConstraintName("FK_MentorMenteePairs_Mentor");
            });

            modelBuilder.Entity<MentorQuestions>(entity =>
            {
                entity.HasIndex(e => e.MentorId);

                entity.HasIndex(e => e.QuestionTypeId);

                entity.Property(e => e.QuestionNotes)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.QuestionText)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.MentorQuestions)
                    .HasForeignKey(d => d.MentorId)
                    .HasConstraintName("FK_MentorQuestions_Mentor");

                entity.HasOne(d => d.QuestionType)
                    .WithMany(p => p.MentorQuestions)
                    .HasForeignKey(d => d.QuestionTypeId)
                    .HasConstraintName("FK_MentorQuestions_QuestionType");
            });

            modelBuilder.Entity<MentorTags>(entity =>
            {
                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.MentorTags)
                    .HasForeignKey(d => d.MentorId)
                    .HasConstraintName("FK_MentorTags_Mentor");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.MentorTags)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK_MentorTags_Tag");
            });

            modelBuilder.Entity<QuestionType>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.ProfileImageUrl)
                    .HasMaxLength(500)
                    .IsFixedLength();

                entity.Property(e => e.ProfileUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SurName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TokenExpireDate).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
