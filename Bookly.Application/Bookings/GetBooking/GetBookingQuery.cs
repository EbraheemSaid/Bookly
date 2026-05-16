using Bookly.Application.Abstractions.Messaging;

namespace Bookly.Application.Bookings.GetBooking;

public sealed record GetBookingQuery(Guid BookingId) : IQuery<BookingResponse>;