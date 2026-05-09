using Bookly.Application.Abstractions.Email;
using Bookly.Domain.Bookings;
using Bookly.Domain.Bookings.Events;
using Bookly.Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Application.Bookings.ReserveBooking
{
    internal sealed class BookingReservedDomainEventHandler(
        IEmailService emailService,
        IBookingRepository bookingRepository,
        IUserRepository userRepository) : INotificationHandler<BookingReservedDomainEvent>
    {
        private readonly IEmailService _emailService = emailService;
        private readonly IBookingRepository _bookingRepository = bookingRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(notification.BookingId, cancellationToken);

            if (booking is null)
                return;

            var user = await _userRepository.GetByIdAsync(booking.UserId, cancellationToken);

            if (user is null)
                return;


            await _emailService.SendAsync(user.Email,
                "Booking Reserved",
                $"Your booking with id {booking.Id} has been reserved successfully.");
        }
    }
}
