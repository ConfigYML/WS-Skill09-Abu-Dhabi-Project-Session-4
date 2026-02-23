using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Session_4_Dennis_Hilfinger;

public partial class AirlineContext : DbContext
{
    public AirlineContext()
    {
    }

    public AirlineContext(DbContextOptions<AirlineContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<CabinType> CabinTypes { get; set; }

    public virtual DbSet<Survey> Surveys { get; set; }

    public virtual DbSet<SurveyResult> SurveyResults { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Session4;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Airport__3214EC074F3AE387");

            entity.ToTable("Airport");

            entity.Property(e => e.IataCode)
                .HasMaxLength(3)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CabinType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CabinTyp__3214EC079A721749");

            entity.ToTable("CabinType");

            entity.Property(e => e.CabinName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Survey__3214EC0738E46706");

            entity.ToTable("Survey");
        });

        modelBuilder.Entity<SurveyResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07BE6A8666");

            entity.ToTable("SurveyResult");

            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Question1).HasColumnName("Question_1");
            entity.Property(e => e.Question2).HasColumnName("Question_2");
            entity.Property(e => e.Question3).HasColumnName("Question_3");
            entity.Property(e => e.Question4).HasColumnName("Question_4");

            entity.HasOne(d => d.ArrivalAirport).WithMany(p => p.SurveyResultArrivalAirports)
                .HasForeignKey(d => d.ArrivalAirportId)
                .HasConstraintName("FK_SurveyResult_ToArrivalAirport");

            entity.HasOne(d => d.CabinType).WithMany(p => p.SurveyResults)
                .HasForeignKey(d => d.CabinTypeId)
                .HasConstraintName("FK_SurveyResult_ToCabinType");

            entity.HasOne(d => d.DepartureAirport).WithMany(p => p.SurveyResultDepartureAirports)
                .HasForeignKey(d => d.DepartureAirportId)
                .HasConstraintName("FK_SurveyResult_ToDepartureAirport");

            entity.HasOne(d => d.Survey).WithMany(p => p.SurveyResults)
                .HasForeignKey(d => d.SurveyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SurveyResult_ToSurvey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
