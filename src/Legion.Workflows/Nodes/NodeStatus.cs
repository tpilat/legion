namespace Legion.Workflows.Nodes;

public enum NodeStatus
{
	NotStarted = 1,
	InProgress = 2,
	Completed = 3,
	Failed = 4,
	WaitingForEvent = 5,
	//Delayed = 6
}
