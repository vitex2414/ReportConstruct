using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ReportConstruct.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ReportConstruct.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Params> Params { get; set; }
        public virtual DbSet<Queries> Queries { get; set; }
        public virtual DbSet<ReportFields> ReportFields { get; set; }
        public virtual DbSet<LoginModel> LoginModels { get; set; }
        public virtual DbSet<Email> Emails { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseOracle("DATA SOURCE= 127.0.0.1:1521/mydb; PASSWORD=0000;PERSIST SECURITY INFO=TRUE;USER ID=REPORTUSER");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Params>(entity =>
            {
                entity.ToTable("PARAMS");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(20)
                    .ValueGeneratedOnAdd()
                    .IsUnicode(false);

                entity.Property(e => e.ParamName)
                    .HasColumnName("PARAM_NAME")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ParamValue)
                    .HasColumnName("PARAM_VALUE")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.QueryId)
                    .HasColumnName("QUERY_ID")
                    .HasMaxLength(20)
                    .IsUnicode(false);


            });

            modelBuilder.Entity<Queries>(entity =>
            {
                entity.ToTable("QUERIES");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd()

                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Query)
                    .HasColumnName("QUERY")
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ReportFields>(entity =>
            {
                entity.ToTable("REPORT_FIELDS");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName)
                    .HasColumnName("DISPLAY_NAME")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.QueryField)
                    .HasColumnName("QUERY_FIELD")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.QueryId)
                    .HasColumnName("QUERY_ID")
                    .HasMaxLength(128)
                    .IsUnicode(false);

            });


            modelBuilder.Entity<LoginModel>(entity =>
            {
                entity.ToTable("USERS");

                entity.Property(e => e.id).HasColumnName("ID");
                entity.Property(e => e.Login).HasColumnName("LOGIN");
                entity.Property(e => e.Password).HasColumnName("PASSWORD");
                entity.Property(e => e.Role).HasColumnName("ROLE");
            }
            );

            modelBuilder.Entity<Email>(entity =>
            {
                entity.ToTable("EMAIL");

                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.EmailAddress).HasColumnName("EMAIL");
            }
            );


            modelBuilder.HasSequence("DEVICE_SID_SEQ");

            OnModelCreatingPartial(modelBuilder);


            modelBuilder.Entity<Timetb>(entity =>
            {
                entity.ToTable("TIMETB");

                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.Dates).HasColumnName("DATES");
                entity.Property(e => e.Names).HasColumnName("NAMES");
                entity.Property(e => e.Timecome).HasColumnName("TIMECOME");
                entity.Property(e => e.Late).HasColumnName("LATE");
                entity.Property(e => e.Lunchleave).HasColumnName("LUNCHLEAVE");
                entity.Property(e => e.Lunchcome).HasColumnName("LUNCHCOME");
                entity.Property(e => e.Timeleave).HasColumnName("TIMELEAVE");
                entity.Property(e => e.Groupname).HasColumnName("GROUPNAME");
                entity.Property(e => e.IsChecked).HasColumnName("ISCHECKED");
            }
            );

            modelBuilder.Entity<LateReports>(entity =>
             {
                 entity.ToTable("LATE_REPORTS");

                 entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                 entity.Property(e => e.Id_late_day).HasColumnName("ID_LATE_DAY");
                 entity.Property(e => e.Names).HasColumnName("NAMES");
                 entity.Property(e => e.Path).HasColumnName("PATH");
                 entity.Property(e => e.Comments).HasColumnName("COMMENTS");
             }
           );
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<ReportConstruct.Models.Email> Email { get; set; }

        public DbSet<ReportConstruct.Models.Timetb> Timetb { get; set; }

        public DbSet<ReportConstruct.Models.LateReports> LateReports { get; set; }
    }
}
