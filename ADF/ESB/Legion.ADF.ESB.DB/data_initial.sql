SET client_encoding = 'UTF8';
--set statement_timeout to 60000; commit; --1min
--show statement_timeout;

INSERT INTO comp."AdapterStatus"
	("IdAdapterStatus", "Code", "Name")
VALUES
	('00000001-0000-0000-0000-000000000000', 'Disabled', 'Disabled'),
	('00000002-0000-0000-0000-000000000000', 'Offline', 'Offline'),
	('00000003-0000-0000-0000-000000000000', 'Active', 'Active'),
	('00000004-0000-0000-0000-000000000000', 'Error', 'Error'),
	('00000005-0000-0000-0000-000000000000', 'Suspended', 'Suspended');
	


INSERT INTO comp."JobType"
	("IdJobType", "Code", "Name")
VALUES
	('00000001-0000-0000-0000-000000000000', 'SequentialIntervalTimer', 'SequentialIntervalTimer'),
	('00000002-0000-0000-0000-000000000000', 'ExactPeriodicTimer', 'ExactPeriodicTimer'),
	('00000003-0000-0000-0000-000000000000', 'CronTimer', 'CronTimer');



INSERT INTO comp."JobStatus"
	("IdJobStatus", "Code", "Name")
VALUES
	('00000001-0000-0000-0000-000000000000', 'Disabled', 'Disabled'),
	('00000002-0000-0000-0000-000000000000', 'Offline', 'Offline'),
	('00000003-0000-0000-0000-000000000000', 'Running', 'Running'),
	('00000004-0000-0000-0000-000000000000', 'Idle', 'Idle'),
	('00000005-0000-0000-0000-000000000000', 'Error', 'Error'),
	('00000006-0000-0000-0000-000000000000', 'Suspended', 'Suspended');



INSERT INTO orch."OrchestrationStatus"
	("IdOrchestrationStatus", "Code", "Name")
VALUES
	('00000001-0000-0000-0000-000000000000', 'Disabled', 'Disabled'),
	('00000002-0000-0000-0000-000000000000', 'Offline', 'Offline'),
	('00000003-0000-0000-0000-000000000000', 'Running', 'Running'),
	('00000004-0000-0000-0000-000000000000', 'Error', 'Error'),
	('00000005-0000-0000-0000-000000000000', 'Succeeded', 'Succeeded'),
	('00000006-0000-0000-0000-000000000000', 'Suspended', 'Suspended');



INSERT INTO orch."OrchestrationStepStatus"
	("IdOrchestrationStepStatus", "Code", "Name")
VALUES
	('00000001-0000-0000-0000-000000000000', 'Idle', 'Idle'),
	('00000002-0000-0000-0000-000000000000', 'Running', 'Running'),
	('00000003-0000-0000-0000-000000000000', 'Error', 'Error'),
	('00000004-0000-0000-0000-000000000000', 'Succeeded', 'Succeeded'),
	('00000005-0000-0000-0000-000000000000', 'Suspended', 'Suspended'),
	('00000006-0000-0000-0000-000000000000', 'Skipped', 'Skipped');
	


INSERT INTO mbox."MessageStatus"
	("IdMessageStatus", "Code", "Name")
VALUES
	('00000001-0000-0000-0000-000000000000', 'Published', 'Published'),
	('00000002-0000-0000-0000-000000000000', 'Delivered', 'Delivered'),
	('00000003-0000-0000-0000-000000000000', 'CannotDeliver', 'CannotDeliver'),
	('00000004-0000-0000-0000-000000000000', 'Dropped', 'Dropped');
	


INSERT INTO mbox."MessageProcessingStatus"
	("IdMessageProcessingStatus", "Code", "Name")
VALUES
	('00000001-0000-0000-0000-000000000000', 'Delivered', 'Delivered'),
	('00000002-0000-0000-0000-000000000000', 'Processing', 'Processing'),
	('00000003-0000-0000-0000-000000000000', 'Processed', 'Processed'),
	('00000004-0000-0000-0000-000000000000', 'Terminated', 'Terminated');
	


--CONFIGURATION
INSERT INTO conf."ConfigurationKeyValue"
	("IdConfigurationKeyValue", "Key", "Value", "AuditCreatedUtc", "AuditModifiedUtc", "IdAuditCreatedBy", "IdAuditModifiedBy", "ConcurrencyToken")
VALUES
	('04dc9f06-ccce-4864-8088-9983a19e99ec', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:SendAuthorizationHeaderInRequest', 'False', '2024-01-01', NULL, NULL, NULL, 'a4880305-f43c-475c-8909-92fe261473ed'),
	('100b85cd-a369-4f6b-b900-2206e9be48fc', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:DefaultTimeoutInSeconds', '60', '2024-01-01', NULL, NULL, NULL, '0b3297ae-182c-40a3-99b4-c97f5e65a778'),
	('148e323a-3378-4d1d-99ba-e6b3e8e7e5f8', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:MaxRequestContentBufferSize', NULL, '2024-01-01', NULL, NULL, NULL, 'b38c942f-4911-4de4-9abf-d3b78c9d2ecf'),
	('16f817ac-b593-424c-ac24-405eecfaf2e3', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:HrefPrefix', '/api/idsp/download/', '2024-01-01', NULL, NULL, NULL, 'fb45cd59-40f4-40c0-a21e-a805ff1d5c61'),
	('171f88ae-9f40-4259-a989-9c0d5281c0c2', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:CheckCertificateRevocationList', NULL, '2024-01-01', NULL, NULL, NULL, '329a1cf9-07ce-401f-8890-8f114b9bea89'),
	('20ed582c-cbe7-4b02-b756-69344ca0a37e', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:SslProtocols', NULL, '2024-01-01', NULL, NULL, NULL, '4029d594-b66f-4c5d-ace9-c299fd5fbaa6'),
	('2503f263-0d59-45d7-8167-5da70046e79e', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:LogResponse', 'True', '2024-01-01', NULL, NULL, NULL, 'e4ce8d4c-2367-4727-816c-bbe0856a69dc'),
	('28bfddcc-0091-422d-8e7c-9267f98c840c', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:LogRequest', 'True', '2024-01-01', NULL, NULL, NULL, '88a906e8-4b06-4129-aa5d-6000cc950d71'),
	('3841805c-2a36-4012-b91c-d94e36e061e9', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:StaticHeaderCollections', NULL, '2024-01-01', NULL, NULL, NULL, '35e6a291-c88b-495a-b63f-90d2afdca03e'),
	('48e1c62a-a835-401e-9e1c-0d0dcaff37be', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:StaticFormData', NULL, '2024-01-01', NULL, NULL, NULL, '863e8e13-03d3-4f1e-94b2-6713e741e2dc'),
	('4af78db1-5b79-4c1f-8f90-070a80eb5fa3', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:AutomaticDecompression', '1', '2024-01-01', NULL, NULL, NULL, 'b4c4a295-ab07-4e7f-9d21-aebc8d35ce19'),
	('53909142-9293-4589-bbc4-adb424305b8e', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:MinLogLevel', '3', '2024-01-01', NULL, NULL, NULL, '9b1b4b6e-95a4-4b74-bc14-103ecedb43b6'),
	('54902ce4-4a36-4a09-acf8-b8b7314ac9e0', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:UsesCookieContainerToStoreServerCookies', 'True', '2024-01-01', NULL, NULL, NULL, '7c6bbfb9-7eb6-48c4-81d5-72ff1b2f400b'),
	('572abf44-00b9-4612-9249-63c0f8944615', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:CredentialCache', NULL, '2024-01-01', NULL, NULL, NULL, '9c29a83e-b5c9-4303-9914-3a7365acb089'),
	('675c6370-f0a7-46cf-b4f4-52874dae0a07', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:ClientName', 'SocPoistHttpClient', '2024-01-01', NULL, NULL, NULL, 'a70a3d26-f55c-4110-a570-a5847b52faad'),
	('6d083be5-fe5f-46d9-b6d6-843888dafe8f', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:LogRequestPayload', 'True', '2024-01-01', NULL, NULL, NULL, 'a9597197-c2b2-4e1e-a530-291b36d61d83'),
	('756a2ce4-c723-44b4-846c-b46095d0415e', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:StaticCookies', NULL, '2024-01-01', NULL, NULL, NULL, 'b1234aef-ebe5-4c0d-bdba-8424a8df80a6'),
	('789c59f8-5b8c-4e12-b1f3-a68ab507094f', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:BaseAddress', 'https://www.socpoist.sk', '2024-01-01', NULL, NULL, NULL, 'c16c2ee8-404c-406d-973f-617c3af17adc'),
	('7c78d201-0000-43e1-9651-743a4a8b3bb5', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:MaxConnectionsPerServer', NULL, '2024-01-01', NULL, NULL, NULL, '2c0179bf-ade3-4124-8a6b-f590feaad859'),
	('91eb6abb-3dbe-45fa-a794-61f512312874', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:StaticHeaders', NULL, '2024-01-01', NULL, NULL, NULL, '710d89a9-f0d3-491b-8537-d9f602c89505'),
	('9e4b9a7e-2e46-4ba4-9365-94c4c39e7822', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:MaxResponseHeadersLength', NULL, '2024-01-01', NULL, NULL, NULL, '67570d1c-27e6-42f5-b8d0-41eaef29acf6'),
	('bab47548-b48f-4a3d-9f48-90e0c16c4b51', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:UseDefaultCredentials', NULL, '2024-01-01', NULL, NULL, NULL, 'ae87c945-a69e-44dc-a1a2-b6c41fabcbb4'),
	('bff274a0-5cff-442c-a85b-c2c959371604', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:SourceSystemName', 'SocPoistHttpClient', '2024-01-01', NULL, NULL, NULL, '4ea8e1f8-23c0-4340-ad74-da90d13d101c'),
	('c6050f7e-c2cb-4544-9618-a0f1277bd6ff', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:Version', '1.0.0.0', '2024-01-01', NULL, NULL, NULL, '5796a811-3130-4cd2-8bd3-ebc4a0e26cb7'),
	('ca469419-bdbc-486a-86d0-784f61666ca3', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:Credentials', NULL, '2024-01-01', NULL, NULL, NULL, 'e5558116-1127-4fb8-bc16-e5c8d5f0f32e'),
	('d21dd8f5-79aa-4663-a795-c1ac9c3e1497', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:ApplyToHttpClientHandler', 'True', '2024-01-01', NULL, NULL, NULL, '0c4e404e-ce5d-4e99-807a-f7827e4a3345'),
	('d84a5a3f-cf4b-459c-b0d0-ae2d89f2de68', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:WebProxySettings', NULL, '2024-01-01', NULL, NULL, NULL, 'd319d521-02be-4525-a149-fb9fbdb38839'),
	('d8bd529d-89c1-4a35-b667-670857683ac8', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:AllowAutoRedirect', NULL, '2024-01-01', NULL, NULL, NULL, '98fdf7d5-5248-40b9-a846-7820c9d2ee1d'),
	('d8e648b7-fcde-40e0-b921-d31c9fb72d5a', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:ClientCertificates', NULL, '2024-01-01', NULL, NULL, NULL, '3225dbf1-cc8b-4c6c-916c-4f7a32f06095'),
	('d97ce07e-204c-49df-9cda-31cdb96a3cd6', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:StaticQueryStrings', NULL, '2024-01-01', NULL, NULL, NULL, '5f87769b-57bc-446c-ab21-63ce7ba2f282'),
	('e26520ee-6f97-4bf6-bd58-d3a4abda0216', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:LogResponsePayload', 'True', '2024-01-01', NULL, NULL, NULL, 'd88084f0-9726-45c2-8521-ed17a8731b38'),
	('e369f406-273a-4253-8c14-b84543b86465', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:ForceStaticQueryStrings', 'False', '2024-01-01', NULL, NULL, NULL, 'f48de06c-8546-4e09-8ed9-b623ac4a9fde'),
	('e3edb89e-3fb2-44e6-a483-f9eb4f21a2bc', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:MaxAutomaticRedirections', NULL, '2024-01-01', NULL, NULL, NULL, '0d1ebe52-bc9d-4f43-9199-11a9772b08c0'),
	('e424d4d7-cef4-4d52-91f0-8c3f41b9606a', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:DefaultProxyCredentials', NULL, '2024-01-01', NULL, NULL, NULL, '0306af64-dfc5-4b2e-81f4-a03c82ee964e'),
	('e5086001-3878-4bea-aa5a-237bd7eb4e94', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:UserAgent', 'SocPoistHttpClient', '2024-01-01', NULL, NULL, NULL, 'cc1deb54-3125-47cd-8680-f67ec04af097'),
	('e7eb9da0-a191-4676-9772-0348bb818617', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:TrustToAllServerCertificates', 'True', '2024-01-01', NULL, NULL, NULL, '3e4af22b-d785-4d6e-8201-948ed610b303'),
	('ed2aeec2-e9ea-4150-9ff6-e65638a2713a', 'TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig:SocPoistHttpClientOptions:LogDisabledUris', NULL, '2024-01-01', NULL, NULL, NULL, '12226761-8a1b-4f53-afe2-b02e5eccac86'),
	('05218455-bf9a-4b93-a5fa-2dfa62896b45', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:StaticHeaders', NULL, '2024-01-01', NULL, NULL, NULL, '91901492-00ba-491b-b1d5-44b36f289cf8'),
	('31d97c15-55f7-4345-8d21-17592df02e4c', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:LogResponsePayload', 'True', '2024-01-01', NULL, NULL, NULL, 'aa5d5e15-8000-4959-9388-638626af08f9'),
	('3a5949ab-9a6b-496e-917b-445758c4bb2d', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:LogDisabledUris', NULL, '2024-01-01', NULL, NULL, NULL, '7cf65c6e-0260-40a3-986f-4a2cd0f9d98f'),
	('3ba08a39-06f4-48c6-8584-5c91f3e54d66', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:UseDefaultCredentials', NULL, '2024-01-01', NULL, NULL, NULL, 'd7fd46f0-5a6c-45a6-b56e-090f219b12af'),
	('4728bf9f-b988-4744-9438-031f7fb86e8b', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:MaxResponseHeadersLength', NULL, '2024-01-01', NULL, NULL, NULL, 'dfcaae2f-55bf-4888-8627-15af16b6781a'),
	('4fc8e8d0-065c-43d6-a487-558ffedb7b08', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:LogRequestPayload', 'True', '2024-01-01', NULL, NULL, NULL, '384779c5-40f2-494c-a44f-98b4b4b5ce0b'),
	('5eb2b5c6-2287-49cb-bee3-7d351c2db60a', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:MinLogLevel', '3', '2024-01-01', NULL, NULL, NULL, '7152f2d6-adda-4198-bd57-fd58560328bc'),
	('6366d581-b356-4513-9c1e-c066d16d76a5', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:CheckCertificateRevocationList', NULL, '2024-01-01', NULL, NULL, NULL, '087e2ad4-cf95-4291-82f6-f0483e03d1f5'),
	('6cd0f94d-0e90-4149-9b0c-5ff160724f43', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:WebProxySettings', NULL, '2024-01-01', NULL, NULL, NULL, '3ff4644c-0483-46f1-b42c-3d8b6df368f0'),
	('6ce7571d-e394-469c-889c-cee77527f175', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:StaticFormData', NULL, '2024-01-01', NULL, NULL, NULL, 'd2b4e064-aa36-43e6-bc81-ce455e64951d'),
	('6f335c48-f5c8-42ec-97bd-1f603b28e560', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:LogResponse', 'True', '2024-01-01', NULL, NULL, NULL, '87689d86-edf5-4f08-9a47-289af536534f'),
	('73abda75-3343-4922-9a29-012fc983f1c5', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:DefaultProxyCredentials', NULL, '2024-01-01', NULL, NULL, NULL, '08161e6d-8598-499a-a8e8-d8de2f5cf39a'),
	('7cd5ab81-f02f-4d81-b7fb-2b5090cc7af7', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:Credentials', NULL, '2024-01-01', NULL, NULL, NULL, '78d70ea8-3cf6-49d7-b553-09ef6de4646c'),
	('80a1a517-c8bc-4cd2-9a83-e92a9be1a6ac', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:ApplyToHttpClientHandler', 'True', '2024-01-01', NULL, NULL, NULL, '60d3a774-3707-43c7-a469-807a73213ea2'),
	('8afd86fc-cbfd-47c3-ab4c-642f4af72b4c', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:MaxConnectionsPerServer', NULL, '2024-01-01', NULL, NULL, NULL, '55670d86-45bd-4fa7-8467-8ce1bb43dccc'),
	('8bff61c4-afc6-447d-8dee-4b3f5f66cd77', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:SourceSystemName', 'RPOHttpClient', '2024-01-01', NULL, NULL, NULL, 'ee09799c-99d0-4fc1-bcba-b87d29066e79'),
	('971cbe43-3dc0-426f-91d8-36ddc775ba31', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:UserAgent', 'RPOHttpClient', '2024-01-01', NULL, NULL, NULL, '22fe585e-f0d7-4666-8489-f4f0cbe2daa6'),
	('987b0a9d-1727-4871-9d9b-607d2ef38e86', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:AllowAutoRedirect', NULL, '2024-01-01', NULL, NULL, NULL, 'c21d4f87-6491-476e-9323-5556639dd3c1'),
	('a4a1d404-02b6-472b-acb5-78249923bd8f', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:AutomaticDecompression', '1', '2024-01-01', NULL, NULL, NULL, '82e90060-d7cb-45d1-94bb-91879c22621c'),
	('a6dce473-7b67-45e0-92c0-f14aa1d54983', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:MaxRequestContentBufferSize', NULL, '2024-01-01', NULL, NULL, NULL, 'd4ce6cb9-53fa-4738-9493-b9d93d3b2603'),
	('a9d4bafa-d7cb-4783-bb91-1dc64185e6b7', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:StaticHeaderCollections', NULL, '2024-01-01', NULL, NULL, NULL, '50159aae-0171-4945-8bf3-e3d8aba0d7f0'),
	('adb01779-f026-42ae-b54c-0a89b3b6d0a5', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:MaxAutomaticRedirections', NULL, '2024-01-01', NULL, NULL, NULL, '53bb1b6a-2061-4997-861d-de559ef60b9e'),
	('af9f2731-0f21-41df-a542-ab23ca5eb159', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:SslProtocols', NULL, '2024-01-01', NULL, NULL, NULL, '4a138e27-c7d8-4063-bc9d-49ef7717b415'),
	('b4040750-6280-4a84-915d-84f84572a2fe', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:BaseAddress', 'https://api.statistics.sk/rpo/v1/', '2024-01-01', NULL, NULL, NULL, '5b8c90c5-8ab2-43d4-9a2e-3c33924373f9'),
	('b6397645-f062-4ce1-ad0b-10d9718d00cf', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:StaticCookies', NULL, '2024-01-01', NULL, NULL, NULL, '9d5baa07-bc19-43d8-ad4b-c2ee5989a1e6'),
	('c9caf1ff-e99a-47ec-bd70-64ed7c60f569', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:CredentialCache', NULL, '2024-01-01', NULL, NULL, NULL, '95466497-0fed-4144-a19a-3bce8c94915e'),
	('d1ba41c2-d51e-49b2-97cf-ed7c14e46a67', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:Version', '1.0.0.0', '2024-01-01', NULL, NULL, NULL, '29b9cb08-90bf-4813-962b-bb9dfa980970'),
	('d57797fa-5829-401c-8568-da06d1825d2a', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:ForceStaticQueryStrings', 'False', '2024-01-01', NULL, NULL, NULL, '6226937e-99a9-4338-9364-801bfd1f561b'),
	('d5f37278-eb86-4826-ad2e-50e91f40facc', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:TrustToAllServerCertificates', 'True', '2024-01-01', NULL, NULL, NULL, 'f9472f62-da09-4efd-8ed1-006afeb95782'),
	('d6cd8879-d441-496f-b055-566c1b73498f', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:DefaultTimeoutInSeconds', '60', '2024-01-01', NULL, NULL, NULL, '9453644c-dba7-4a63-97ba-912bb2116fb2'),
	('decfd4a2-fe0e-48f2-a31a-6e177a74a9fa', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:LogRequest', 'True', '2024-01-01', NULL, NULL, NULL, '2d6e32c6-e72f-4002-b0ba-b640958191f5'),
	('df6f589e-0683-4bb4-9ad5-0040f5bfedef', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:SendAuthorizationHeaderInRequest', 'False', '2024-01-01', NULL, NULL, NULL, '379e87a1-4bbf-456f-97e6-b1192d22e7c9'),
	('f1572f13-1099-47e1-afc7-d78a97288454', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:ClientCertificates', NULL, '2024-01-01', NULL, NULL, NULL, 'ad35651f-df85-40d8-8fe5-52e9ec02234e'),
	('f5a67fa6-8cbb-4005-bf4c-c77b10822bdb', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:UsesCookieContainerToStoreServerCookies', 'True', '2024-01-01', NULL, NULL, NULL, '2d605369-21be-4420-8b9c-862d4f79742e'),
	('f7919238-8deb-46f6-b5c3-4724c6e284ed', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:ClientName', 'RPOHttpClient', '2024-01-01', NULL, NULL, NULL, '62cc5cf5-2c38-49a5-93c7-bf2257da4214'),
	('fbf5f67b-6797-4470-8954-a459227c9995', 'TestEnterpriseServiceBusConfig:RPOClientAdapterConfig:RPOHttpClientOptions:StaticQueryStrings', NULL, '2024-01-01', NULL, NULL, NULL, 'e522a8fe-05a7-4dca-86ca-ef758b28f62c')
;
