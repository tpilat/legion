INSERT INTO hosts."Host"
	("IdHost", "Name", "Description", "CreatedUtc", "IsEnabled", "Configuration", "RowVersion")
VALUES
	('e349b965-e5b1-4039-b24c-392f77c89fed'::uuid, 'MyTestHost', 'IP 192.168.123.123', '2025-07-07 00:00:00.000', true, '{"ErrorDelayTable": [{"Timeout": "00:00:00.0140000", "RetryCount": 1}, {"Timeout": "00:00:01", "RetryCount": 2}, {"Timeout": "00:00:05", "RetryCount": 3}], "HeartbeatInSeconds": 10}'::jsonb, 'e349b965-e5b1-4039-b24c-392f77c89fed'::uuid);


INSERT INTO jobs."Job"
	("IdJob", "Name", "Description", "IdJobRunType", "Namespace", "Properties", "DelayedStartInSeconds", "IdleTimeoutInSeconds", "CronExpression", "CronExpressionIncludeSeconds", "IdDefaultHost", "RequestedToDisable", "TimeoutForProcessingInSeconds", "RowVersion")
VALUES
	('b7ea860f-a6de-4e08-a5ad-ee09a2d5bb5f'::uuid, 'MyTestJob', 'Test job description', '00000001-0000-0000-0000-000000000000'::uuid, 'Test.ESB.Jobs.MyTestJobClass', NULL, NULL, 10, NULL, false, 'e349b965-e5b1-4039-b24c-392f77c89fed'::uuid, false, 60, 'b7ea860f-a6de-4e08-a5ad-ee09a2d5bb5f'::uuid);

