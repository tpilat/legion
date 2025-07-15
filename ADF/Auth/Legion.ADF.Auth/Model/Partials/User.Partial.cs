using Legion.Extensions;
using Legion.Identity;
using System.Security.Claims;

namespace Legion.ADF.Auth.Model;

public partial class User : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	internal static IResult<User> CreateUser(
		IScopeContext scopeContext,
		string login,
		string normalizedLogin)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<User>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, login))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, normalizedLogin))
			return result.Build();

		var user = new User
		{
			__IsNewObject = true,
			IdUser = GlobalContext.Instance.NewGuid(),
			Login = login,
			NormalizedLogin = normalizedLogin,
			TenantIdentifier = scopeContext.TenantIdentifier,
			Email = null,
			NormalizedEmail = null,
			EmailConfirmed = false,
			PasswordHash = null,
			SecurityStamp = null,
			ADDistinguishedName = null,
			Data = null,
			PhoneNumber = null,
			PhoneNumberConfirmed = false,
			MultiFactorEnabled = false,
			LockoutEndUtc = null,
			LockoutEnabled = false,
			AccessFailedCount = 0,
			IsSystemUser = false,
			ConfirmationUrlSlug = null,
			ConfirmationUrlValidToUtc = null,
			AuditCreatedUtc = GlobalContext.Instance.UtcNow,
			AuditModifiedUtc = null,
			IdAuditCreatedBy = scopeContext.IdUser,
			IdAuditModifiedBy = null,
			ConcurrencyToken = GlobalContext.Instance.NewGuid(),
			DeletedUtc = DateTime.MinValue
		};

		var validationResult =
			DefaultDBValidator
				.Validate(user);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.WithData(user).Build();
	}

	internal IResult SetLogin(
		IScopeContext scopeContext,
		string login)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, login))
			return result.Build();

		Login = login;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetNormalizedLogin(
		IScopeContext scopeContext,
		string normalizedLogin)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, normalizedLogin))
			return result.Build();

		NormalizedLogin = normalizedLogin;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetEmail(
		IScopeContext scopeContext,
		string? email)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, email))
			return result.Build();

		Email = email;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetNormalizedEmail(
		IScopeContext scopeContext,
		string? normalizedEmail)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, normalizedEmail))
			return result.Build();

		NormalizedEmail = normalizedEmail;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetEmailConfirmed(
		IScopeContext scopeContext,
		bool emailConfirmed)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		EmailConfirmed = emailConfirmed;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult IncrementAccessFailedCount(IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		AccessFailedCount++;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult ResetAccessFailedCount(IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		AccessFailedCount = 0;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetLockoutEnabled(
		IScopeContext scopeContext,
		bool lockoutEnabled)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		LockoutEnabled = lockoutEnabled;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetLockoutEndUtc(
		IScopeContext scopeContext,
		DateTime? lockoutEndUtc)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		LockoutEndUtc = lockoutEndUtc;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetPasswordHash(
		IScopeContext scopeContext,
		string? passwordHash)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		PasswordHash = passwordHash;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetPhoneNumber(
		IScopeContext scopeContext,
		string? phoneNumber)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		PhoneNumber = phoneNumber;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetPhoneNumberConfirmed(
		IScopeContext scopeContext,
		bool phoneNumberConfirmed)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		PhoneNumberConfirmed = phoneNumberConfirmed;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetSecurityStamp(
		IScopeContext scopeContext,
		string? securityStamp)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		SecurityStamp = securityStamp;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetMultiFactorEnabled(
		IScopeContext scopeContext,
		bool multiFactorEnabled)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		MultiFactorEnabled = multiFactorEnabled;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetSoftDelete(IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		DeletedUtc = GlobalContext.Instance.UtcNow;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = DeletedUtc;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal LegionIdentity ToIdentity(
		Dictionary<Guid, List<Guid>> rolePermissions,
		Guid? superAdminPermissionId,
		object? userData,
		List<Claim>? claims = null)
	{
		Throw.IfArgumentNull(rolePermissions);

		var roleIds = new Dictionary<Guid, List<Guid>>();
		var permissionIds = new Dictionary<Guid, List<Guid>>();

		if (0 < UserRoles?.Count)
			roleIds = UserRoles.Where(ur => ur.DeletedUtc == DateTime.MinValue)
				.GroupBy(x => x.TenantIdentifier)
				.ToDictionary(k => k.Key, v => v.Select(x => x.IdRole).ToList());

		if (0 < UserPermissions?.Count)
			permissionIds = UserPermissions.Where(ur => ur.DeletedUtc == DateTime.MinValue)
				.GroupBy(x => x.TenantIdentifier)
				.ToDictionary(k => k.Key, v => v.Select(x => x.IdPermission).ToList());

		foreach (var kvp in roleIds)
		{
			foreach (var roleId in kvp.Value)
			{
				if (rolePermissions.TryGetValue(roleId, out var permissions)
					&& 0 < permissions?.Count)
				{
					foreach (var p in permissions)
					{
						if (permissionIds.TryGetValue(kvp.Key, out var tenantPermissions))
						{
							tenantPermissions.Add(p);
						}
						else
						{
							permissionIds.Add(kvp.Key, [ p ]);
						}
					}
				}
			}
		}

		var tenantIds = new List<Guid>();

		if (TenantIdentifier.HasValue && TenantIdentifier != Guid.Empty)
			tenantIds.Add(TenantIdentifier.Value);

		tenantIds.AddRange(roleIds.Keys);
		tenantIds.AddUniqueRange(permissionIds.Keys);

		var claimsIdentity = 0 < claims?.Count
			? new ClaimsIdentity(claims, "LEGION")
			: new ClaimsIdentity("LEGION");

		var identity = new LegionIdentity(
			claimsIdentity,
			new IdentityData
			{
				IdUser = IdUser,
				Login = Login!,
				CurrentIdTenant = TenantIdentifier,
				DisplayName = null,//TODO VYTIAHNI Z ORIGINAL CLAIMSOV - FACEBOOK TO NASTAVUJE
				Tenants = tenantIds,
				RoleIds = roleIds,
				PermissionIds = permissionIds,
				IsSuperAdmin = permissionIds.Any(kvp => kvp.Value.Any(p => p == superAdminPermissionId)),
				UserData = userData
			},
			true,
			true);

		return identity;
	}

	internal LegionPrincipal ToPrincipal(
		Dictionary<Guid, List<Guid>> rolePermissions,
		Guid? superAdminPermissionId,
		object? userData,
		List<Claim>? claims = null)
	{
		var identity = ToIdentity(
			rolePermissions,
			superAdminPermissionId,
			userData,
			claims);

		var principal = new LegionPrincipal(identity);
		return principal;
	}
}
