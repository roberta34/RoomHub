using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoomHub.Api.Domain.Bookings;
using System.Globalization;

namespace RoomHub.Api.Infrastructure.Persistence.Configurations;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");
        builder.HasKey(booking => booking.Id);
        builder.Property(booking => booking.Id).ValueGeneratedNever();
        builder.Property(booking => booking.RoomId).UsePropertyAccessMode(PropertyAccessMode.PreferField);
        builder.Property(booking => booking.BookedBy)
            .IsRequired()
            .UsePropertyAccessMode(PropertyAccessMode.PreferField);
        builder.Property(booking => booking.Attendees)
            .UsePropertyAccessMode(PropertyAccessMode.PreferField);
        builder.Property(booking => booking.Status)
            .HasConversion<string>()
            .UsePropertyAccessMode(PropertyAccessMode.PreferField);

        builder.Ignore(booking => booking.StartUtc);
        builder.Ignore(booking => booking.EndUtc);

        builder.ComplexProperty(booking => booking.Range, rangeBuilder =>
        {
            rangeBuilder.Property(range => range.StartUtc)
                .HasColumnName("StartUtc")
                .HasConversion(ToUtcString());
            rangeBuilder.Property(range => range.EndUtc)
                .HasColumnName("EndUtc")
                .HasConversion(ToUtcString());
        });
    }

    private static ValueConverter<DateTime, string> ToUtcString() =>
        new(
            value => value.ToString("O", CultureInfo.InvariantCulture),
            value => DateTime.SpecifyKind(
                DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                DateTimeKind.Utc));
}
