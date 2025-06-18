INSERT INTO log.[LogLevel]
	([IdLogLevel], [Code], [Name], [ItemCode])
VALUES
	('00000001-0000-0000-0000-000000000000', 'Trace', 'Trace', 0),
	('00000002-0000-0000-0000-000000000000', 'Debug', 'Debug', 1),
	('00000003-0000-0000-0000-000000000000', 'Info', 'Info', 2),
	('00000004-0000-0000-0000-000000000000', 'Warning', 'Warning', 3),
	('00000005-0000-0000-0000-000000000000', 'Error', 'Error', 4),
	('00000006-0000-0000-0000-000000000000', 'Critical', 'Critical', 5);



INSERT INTO log.[EventCounterCategory]
	([IdEventCounterCategory], [Source], [DisplayName])
VALUES
	('00000001-0000-0000-0000-000000000000', 'System.Runtime', 'System.Runtime'),
	('00000002-0000-0000-0000-000000000000', 'Microsoft.AspNetCore.Hosting', 'Microsoft.AspNetCore.Hosting'),
	('00000003-0000-0000-0000-000000000000', 'Microsoft.AspNetCore.Http.Connections', 'Microsoft.AspNetCore.Http.Connections'),
	('00000004-0000-0000-0000-000000000000', 'Microsoft-AspNetCore-Server-Kestrel', 'Microsoft-AspNetCore-Server-Kestrel'),
	('00000005-0000-0000-0000-000000000000', 'System.Net.Http', 'System.Net.Http');



INSERT INTO log.[EventCounter]
	([IdEventCounter], [IdEventCounterCategory], [Code], [Name], [DisplayName], [CounterType], [DisplayRateTimeScale], [Metadata], [DisplayUnits])
VALUES 
	('00000001-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime cpu usage', 'cpu-usage', 'CPU Usage', 'Mean', NULL, NULL, '%'),
	('00000002-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime working set', 'working-set', 'Working Set', 'Mean', NULL, NULL, 'MB'),
	('00000003-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime gc heap size', 'gc-heap-size', 'GC Heap Size', 'Mean', NULL, NULL, 'MB'),
	('00000004-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime gen 0 gc count', 'gen-0-gc-count', 'Gen 0 GC Count', 'Sum', '00:01:00', NULL, NULL),
	('00000005-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime gen 1 gc count', 'gen-1-gc-count', 'Gen 1 GC Count', 'Sum', '00:01:00', NULL, NULL),
	('00000006-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime gen 2 gc count', 'gen-2-gc-count', 'Gen 2 GC Count', 'Sum', '00:01:00', NULL, NULL),
	('00000007-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime threadpool thread count', 'threadpool-thread-count', 'ThreadPool Thread Count', 'Mean', NULL, NULL, NULL),
	('00000008-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime monitor lock contention count', 'monitor-lock-contention-count', 'Monitor Lock Contention Count', 'Sum', '00:00:01', NULL, NULL),
	('00000009-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime threadpool queue length', 'threadpool-queue-length', 'ThreadPool Queue Length', 'Mean', NULL, NULL, NULL),
	('0000000A-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime threadpool completed items count', 'threadpool-completed-items-count', 'ThreadPool Completed Work Item Count', 'Sum', '00:00:01', NULL, NULL),
	('0000000B-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime alloc rate', 'alloc-rate', 'Allocation Rate', 'Sum', '00:00:01', NULL, 'B'),
	('0000000C-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime active timer count', 'active-timer-count', 'Number of Active Timers', 'Mean', NULL, NULL, NULL),
	('0000000D-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime gc fragmentation', 'gc-fragmentation', 'GC Fragmentation', 'Mean', NULL, NULL, '%'),
	('0000000E-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime exception count', 'exception-count', 'Exception Count', 'Sum', '00:00:01', NULL, NULL),
	('0000000F-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime time in gc', 'time-in-gc', '% Time in GC since last GC', 'Mean', NULL, NULL, '%'),
	('00000010-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime gen 0 size', 'gen-0-size', 'Gen 0 Size', 'Mean', NULL, NULL, 'B'),
	('00000011-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime gen 1 size', 'gen-1-size', 'Gen 1 Size', 'Mean', NULL, NULL, 'B'),
	('00000012-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime gen 2 size', 'gen-2-size', 'Gen 2 Size', 'Mean', NULL, NULL, 'B'),
	('00000013-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime loh size', 'loh-size', 'LOH Size', 'Mean', NULL, NULL, 'B'),
	('00000014-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime poh size', 'poh-size', 'POH (Pinned Object Heap) Size', 'Mean', NULL, NULL, 'B'),
	('00000015-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime assembly count', 'assembly-count', 'Number of Assemblies Loaded', 'Mean', NULL, NULL, NULL),
	('00000016-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime il bytes jitted', 'il-bytes-jitted', 'IL Bytes Jitted', 'Mean', NULL, NULL, 'B'),
	('00000017-0000-0000-0000-000000000000', '00000001-0000-0000-0000-000000000000', 'System Runtime methods jitted count', 'methods-jitted-count', 'Number of Methods Jitted', 'Mean', NULL, NULL, NULL),
	('00000018-0000-0000-0000-000000000000', '00000002-0000-0000-0000-000000000000', 'Microsoft AspNetCore Hosting current requests', 'current-requests', 'Current Requests', 'Mean', NULL, NULL, NULL),
	('00000019-0000-0000-0000-000000000000', '00000002-0000-0000-0000-000000000000', 'Microsoft AspNetCore Hosting failed requests', 'failed-requests', 'Failed Requests', 'Mean', NULL, NULL, NULL),
	('0000001A-0000-0000-0000-000000000000', '00000002-0000-0000-0000-000000000000', 'Microsoft AspNetCore Hosting requests per second', 'requests-per-second', 'Request Rate', 'Sum', '00:00:01', NULL, NULL),
	('0000001B-0000-0000-0000-000000000000', '00000002-0000-0000-0000-000000000000', 'Microsoft AspNetCore Hosting total requests', 'total-requests', 'Total Requests', 'Mean', NULL, NULL, NULL),
	('0000001C-0000-0000-0000-000000000000', '00000003-0000-0000-0000-000000000000', 'Microsoft AspNetCore Http Connections connections duration', 'connections-duration', 'Average Connection Duration', 'Mean', NULL, NULL, 'ms'),
	('0000001D-0000-0000-0000-000000000000', '00000003-0000-0000-0000-000000000000', 'Microsoft AspNetCore Http Connections current connections', 'current-connections', 'Current Connections', 'Mean', NULL, NULL, NULL),
	('0000001E-0000-0000-0000-000000000000', '00000003-0000-0000-0000-000000000000', 'Microsoft AspNetCore Http Connections connections started', 'connections-started', 'Total Connections Started', 'Mean', NULL, NULL, NULL),
	('0000001F-0000-0000-0000-000000000000', '00000003-0000-0000-0000-000000000000', 'Microsoft AspNetCore Http Connections connections stopped', 'connections-stopped', 'Total Connections Stopped', 'Mean', NULL, NULL, NULL),
	('00000020-0000-0000-0000-000000000000', '00000003-0000-0000-0000-000000000000', 'Microsoft AspNetCore Http Connections connections timed out', 'connections-timed-out', 'Total Connections Timed Out', 'Mean', NULL, NULL, NULL),
	('00000021-0000-0000-0000-000000000000', '00000004-0000-0000-0000-000000000000', 'Microsoft-AspNetCore-Server-Kestrel connections per second', 'connections-per-second', 'Connection Rate', 'Sum', '00:00:01', NULL, NULL),
	('00000022-0000-0000-0000-000000000000', '00000004-0000-0000-0000-000000000000', 'Microsoft-AspNetCore-Server-Kestrel tls handshakes per second', 'tls-handshakes-per-second', 'TLS Handshake Rate', 'Sum', '00:00:01', NULL, NULL),
	('00000023-0000-0000-0000-000000000000', '00000004-0000-0000-0000-000000000000', 'Microsoft-AspNetCore-Server-Kestrel total connections', 'total-connections', 'Total Connections', 'Mean', NULL, NULL, NULL),
	('00000024-0000-0000-0000-000000000000', '00000004-0000-0000-0000-000000000000', 'Microsoft-AspNetCore-Server-Kestrel total tls handshakes', 'total-tls-handshakes', 'Total TLS Handshakes', 'Mean', NULL, NULL, NULL),
	('00000025-0000-0000-0000-000000000000', '00000004-0000-0000-0000-000000000000', 'Microsoft-AspNetCore-Server-Kestrel current tls handshakes', 'current-tls-handshakes', 'Current TLS Handshakes', 'Mean', NULL, NULL, NULL),
	('00000026-0000-0000-0000-000000000000', '00000004-0000-0000-0000-000000000000', 'Microsoft-AspNetCore-Server-Kestrel failed tls handshakes', 'failed-tls-handshakes', 'Failed TLS Handshakes', 'Mean', NULL, NULL, NULL),
	('00000027-0000-0000-0000-000000000000', '00000004-0000-0000-0000-000000000000', 'Microsoft-AspNetCore-Server-Kestrel current connections', 'current-connections', 'Current Connections', 'Mean', NULL, NULL, NULL),
	('00000028-0000-0000-0000-000000000000', '00000004-0000-0000-0000-000000000000', 'Microsoft-AspNetCore-Server-Kestrel connection queue length', 'connection-queue-length', 'Connection Queue Length', 'Mean', NULL, NULL, NULL),
	('00000029-0000-0000-0000-000000000000', '00000004-0000-0000-0000-000000000000', 'Microsoft-AspNetCore-Server-Kestrel request queue length', 'request-queue-length', 'Request Queue Length', 'Mean', NULL, NULL, NULL),
	('0000002A-0000-0000-0000-000000000000', '00000004-0000-0000-0000-000000000000', 'Microsoft-AspNetCore-Server-Kestrel current upgraded requests', 'current-upgraded-requests', 'Current Upgraded Requests (WebSockets)', 'Mean', NULL, NULL, NULL),
	('0000002B-0000-0000-0000-000000000000', '00000005-0000-0000-0000-000000000000', 'System Net Http requests started rate', 'requests-started-rate', 'Requests Started Rate', 'Sum', '00:00:01', NULL, NULL),
	('0000002C-0000-0000-0000-000000000000', '00000005-0000-0000-0000-000000000000', 'System Net Http requests failed rate', 'requests-failed-rate', 'Requests Failed Rate', 'Sum', '00:00:01', NULL, NULL),
	('0000002D-0000-0000-0000-000000000000', '00000005-0000-0000-0000-000000000000', 'System Net Http requests started', 'requests-started', 'Requests Started', 'Mean', NULL, NULL, NULL),
	('0000002E-0000-0000-0000-000000000000', '00000005-0000-0000-0000-000000000000', 'System Net Http requests failed', 'requests-failed', 'Requests Failed', 'Mean', NULL, NULL, NULL),
	('0000002F-0000-0000-0000-000000000000', '00000005-0000-0000-0000-000000000000', 'System Net Http current requests', 'current-requests', 'Current Requests', 'Mean', NULL, NULL, NULL),
	('00000030-0000-0000-0000-000000000000', '00000005-0000-0000-0000-000000000000', 'System Net Http http11 connections current total', 'http11-connections-current-total', 'Current Http 1 1 Connections', 'Mean', NULL, NULL, NULL),
	('00000031-0000-0000-0000-000000000000', '00000005-0000-0000-0000-000000000000', 'System Net Http http20 connections current total', 'http20-connections-current-total', 'Current Http 2 0 Connections', 'Mean', NULL, NULL, NULL),
	('00000032-0000-0000-0000-000000000000', '00000005-0000-0000-0000-000000000000', 'System Net Http http11 requests queue duration', 'http11-requests-queue-duration', 'HTTP 1 1 Requests Queue Duration', 'Mean', NULL, NULL, 'ms'),
	('00000033-0000-0000-0000-000000000000', '00000005-0000-0000-0000-000000000000', 'System Net Http http20 requests queue duration', 'http20-requests-queue-duration', 'HTTP 2 0 Requests Queue Duration', 'Mean', NULL, NULL, 'ms')
	;
