using NetHub.Application.Features.Public.Users.Sso;

namespace NetHub.Application.Interfaces;

public interface IAuthProviderValidator
{
	ProviderType Type { get; }

	Task<bool> ValidateAsync(SsoEnterRequest request, SsoType type);
}

public enum SsoType
{
	Register,
	Login
}