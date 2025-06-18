namespace Legion.Transactions;

[Flags]
public enum TransactionsControllerStatus
{
	None = 0,
	Idle = 1 << 0,         // 2^0
	Commiting = 1 << 1,    // 2^1
	Commited = 1 << 2,     // 2^2
	Rollingback = 1 << 3,  // 2^3
	Rolledback = 1 << 4,   // 2^4
	Disposing = 1 << 5,    // 2^5
	Disposed = 1 << 6,     // 2^6
	CommitInProgress = Commiting | Commited,
	NotCommitable = Rollingback | Rolledback | Disposing | Disposed,
	NotIdle = Commiting | Commited | Rollingback | Rolledback | Disposing | Disposed
}
