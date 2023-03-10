using FluentValidation;
using MediatR;

namespace NetHub.Application.Features.Public.Users.Profile;

public record UpdateUserProfileRequest(string FirstName, string LastName, string? MiddleName, string? Description) : IRequest;

public class ChangeFirstNameValidator : AbstractValidator<UpdateUserProfileRequest>
{
	public ChangeFirstNameValidator()
	{
		RuleFor(r => r.FirstName).NotNull().NotEmpty().WithMessage("First name is required");
		RuleFor(r => r.LastName).NotNull().NotEmpty().WithMessage("Last name is required");
	}
}