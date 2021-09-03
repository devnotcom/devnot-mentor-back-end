using DevnotMentor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevnotMentor.Data
{
    public partial class MentorDBContext : DbContext
    {
        public MentorDBContext(DbContextOptions<MentorDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LinkType> LinkTypes { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Mentee> Mentees { get; set; }
        public virtual DbSet<MenteeAnswer> MenteeAnswers { get; set; }
        public virtual DbSet<MenteeLink> MenteeLinks { get; set; }
        public virtual DbSet<MenteeTag> MenteeTags { get; set; }
        public virtual DbSet<Mentor> Mentors { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<MentorLink> MentorLinks { get; set; }
        public virtual DbSet<Mentorship> Mentorships { get; set; }
        public virtual DbSet<MentorQuestion> MentorQuestions { get; set; }
        public virtual DbSet<MentorTag> MentorTags { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }

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

            modelBuilder.Entity<MenteeAnswer>(entity =>
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
                    .HasConstraintName("FK_MenteeAnswers_MentorQuestion");
            });

            modelBuilder.Entity<MenteeLink>(entity =>
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

            modelBuilder.Entity<MenteeTag>(entity =>
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

            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasIndex(e => e.MenteeId);

                entity.HasIndex(e => e.MentorId);

                entity.Property(e => e.Note)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.AppliedAt).HasColumnType("datetime");

                entity.Property(e => e.CompletedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Mentee)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.MenteeId)
                    .HasConstraintName("FK_MentorApplications_Mentee");

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.MentorId)
                    .HasConstraintName("FK_MentorApplications_Mentor");
            });

            modelBuilder.Entity<MentorLink>(entity =>
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

            modelBuilder.Entity<Mentorship>(entity =>
            {
                entity.HasIndex(e => e.MenteeId);

                entity.HasIndex(e => e.MentorId);

                entity.Property(e => e.MenteeComment)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.MentorComment)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.FinishedAt).HasColumnType("datetime");

                entity.Property(e => e.StartedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Mentee)
                    .WithMany(p => p.Mentorships)
                    .HasForeignKey(d => d.MenteeId)
                    .HasConstraintName("FK_Mentorships_Mentee");

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.Mentorships)
                    .HasForeignKey(d => d.MentorId)
                    .HasConstraintName("FK_Mentorships_Mentor");
            });

            modelBuilder.Entity<MentorQuestion>(entity =>
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
                    .WithMany(p => p.MentorQuestion)
                    .HasForeignKey(d => d.MentorId)
                    .HasConstraintName("FK_MentorQuestion_Mentor");

                entity.HasOne(d => d.QuestionType)
                    .WithMany(p => p.MentorQuestion)
                    .HasForeignKey(d => d.QuestionTypeId)
                    .HasConstraintName("FK_MentorQuestion_QuestionType");
            });

            modelBuilder.Entity<MentorTag>(entity =>
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

            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
