namespace Legion.EntityFrameworkCore.Interceptors;

internal enum RowLockHints
{
	/// <summary>
	/// Waits for the lock to be released.
	/// <para><strong>PostgreSQL</strong> - SELECT * FROM table <strong>FOR UPDATE;</strong></para>
	/// <para><strong>SqlServer</strong> - SELECT * FROM table <strong>WITH (UPDLOCK)</strong></para>
	/// </summary>
	LEGION_FOR_UPDATE,

	/// <summary>
	/// Throws an error immediately.
	/// <para><strong>PostgreSQL</strong> - SELECT * FROM table <strong>FOR UPDATE NOWAIT;</strong></para>
	/// <para><strong>SqlServer</strong> - SELECT * FROM table <strong>WITH (UPDLOCK, NOWAIT)</strong></para>
	/// </summary>
	LEGION_NOWAIT,

	/// <summary>
	/// Skips the locked rows.
	/// <para><strong>PostgreSQL</strong> - SELECT * FROM table <strong>FOR UPDATE SKIP LOCKED;</strong></para>
	/// <para><strong>SqlServer</strong> - SELECT * FROM table <strong>WITH (UPDLOCK, READPAST)</strong></para>
	/// </summary>
	LEGION_SKIP_LOCKED,

	/// <summary>
	/// Acquires a shared lock on rows, allowing other transactions to read the rows but not modify them.
	/// <para><strong>PostgreSQL</strong> - SELECT * FROM table <strong>FOR SHARE;</strong></para>
	/// <para><strong>SqlServer</strong> - SELECT * FROM table <strong>WITH (ROWLOCK, HOLDLOCK)</strong></para>
	/// </summary>
	LEGION_FOR_SHARE
}
