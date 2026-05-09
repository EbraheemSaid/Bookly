using Bookly.Application.Abstractions.Clock;
using Bookly.Application.Abstractions.Messaging;
using Bookly.Domain.Abstractions;
using Bookly.Domain.Apartments;
using Bookly.Domain.Bookings;
using Bookly.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Application.Bookings.ReserveBooking
{
    internal sealed class ReserveBookingCommandHandler(
        IUserRepository userRepository,
        IApartmentRepository apartmentRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        PricingService pricingService
        ) : ICommandHandler<ReserveBookingCommand, Guid>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IApartmentRepository _apartmentRepository = apartmentRepository;
        private readonly IBookingRepository _bookingRepository = bookingRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly PricingService _pricingService = pricingService;

        public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }
            var apartment = await _apartmentRepository.GetByIdAsync(request.ApartmentId, cancellationToken);
            if (apartment is null)
            {
                return Result.Failure<Guid>(ApartmentErrors.NotFound);
            }

            var duration = DateRange.Create(request.StartDate, request.EndDate);

            if (await _bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken))
            {
                return Result.Failure<Guid>(BookingErrors.Overlap);
            }

            var booking = Booking.Reserve(
                    apartment,
                    user.Id,
                    duration,
                    utcNow: _dateTimeProvider.UtcNow,
                    _pricingService);

            _bookingRepository.Add(booking);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return booking.Id;
        }
    }
}
