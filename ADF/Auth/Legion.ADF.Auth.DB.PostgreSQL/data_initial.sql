SET client_encoding = 'UTF8';
--set statement_timeout to 60000; commit; --1min
--show statement_timeout;

INSERT INTO auth."Permission"
	("IdPermission", "Code", "Name", "Description", "ClaimValue", "IsSystemPermission")
VALUES
	('00000001-0000-0000-0000-000000000000', 'Access', 'Access', 'Access', 'Access', true);
	
	
INSERT INTO auth."LoginProvider"
	("IdLoginProvider", "Code", "Name", "DisabledUtc")
VALUES
	('00000001-0000-0000-0000-000000000000', 'ActiveDirectory', 'ActiveDirectory', null),
	('00000002-0000-0000-0000-000000000000', 'FormAuthentication', 'FormAuthentication - Cookie', null),
	('00000003-0000-0000-0000-000000000000', 'Facebook', 'Facebook', null),
	('00000004-0000-0000-0000-000000000000', 'Google', 'Google', null),
	('00000005-0000-0000-0000-000000000000', 'Microsoft', 'Microsoft', null),
	('00000006-0000-0000-0000-000000000000', 'Twitter', 'Twitter', null),
	('00000007-0000-0000-0000-000000000000', 'AuthenticatorApp', 'AuthenticatorApp', null);


INSERT INTO auth."Role"
	("IdRole", "Name", "NormalizedName", "ADGroupDistinguishedName", "Data", "Description", "HasConstantPermissions", "HasConstantUsers", "IsSystemRole", "AuditCreatedUtc", "AuditModifiedUtc", "IdAuditCreatedBy", "IdAuditModifiedBy", "ConcurrencyToken", "DeletedUtc")
VALUES
	('00000001-0000-0000-0000-000000000000', 'SuperAdmin', 'SUPERADMIN', null, null, 'Super admin', false, false, false, '2024-01-01', null, null, null, '00000001-0000-0000-0000-000000000000', '-infinity');

