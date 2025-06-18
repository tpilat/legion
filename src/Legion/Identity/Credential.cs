using System.Net;
using System.Text;

namespace Legion.Identity;

public class Credential
{
	public string? UserName { get; set; }

	public string? Password { get; set; }

	public string? Domain { get; set; }

	public Credential(string? userName, string? password, string? domain)
	{
		UserName = userName;
		Password = password;
		Domain = domain;
	}

	public string AsBasicAuthenticationToken()
	{
		var token = string.IsNullOrWhiteSpace(Domain)
			? Convert.ToBase64String(Encoding.ASCII.GetBytes($"{UserName}:{Password}"))
			: Convert.ToBase64String(Encoding.ASCII.GetBytes($"{UserName}@{Domain}:{Password}"));

		return $"Basic {token}";
	}

	public NetworkCredential AsNetworkCredential()
		=> new(UserName ?? string.Empty, Password ?? string.Empty, Domain ?? string.Empty);
}
