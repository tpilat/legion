INSERT INTO legion_adf_servicebus.hosts.Host
	(IdHost, Name, Description, CreatedUtc, IsEnabled, Configuration, RowVersion)
VALUES
	(N'E349B965-E5B1-4039-B24C-392F77C89FED', N'MyTestHost', N'IP 192.168.123.123', '2025-07-07 00:00:00.000', 1, N'{
  "ErrorDelayTable": [
    {
      "Timeout": "00:00:00.0140000",
      "RetryCount": 1
    },
    {
      "Timeout": "00:00:01",
      "RetryCount": 2
    },
    {
      "Timeout": "00:00:05",
      "RetryCount": 3
    }
  ],
  "HeartbeatInSeconds": 10
}', N'E349B965-E5B1-4039-B24C-392F77C89FED');


INSERT INTO legion_adf_servicebus.jobs.Job
	(IdJob, Name, Description, IdJobRunType, Namespace, Properties, DelayedStartInSeconds, IdleTimeoutInSeconds, CronExpression, CronExpressionIncludeSeconds, IdDefaultHost, RequestedToDisable, TimeoutForProcessingInSeconds, RowVersion)
VALUES
	(N'B7EA860F-A6DE-4E08-A5AD-EE09A2D5BB5F', N'MyTestJob', N'Test job description', N'00000001-0000-0000-0000-000000000000', N'Test.ESB.Jobs.MyTestJobClass', NULL, NULL, 10, NULL, 0, N'E349B965-E5B1-4039-B24C-392F77C89FED', 0, 60, N'B7EA860F-A6DE-4E08-A5AD-EE09A2D5BB5F');