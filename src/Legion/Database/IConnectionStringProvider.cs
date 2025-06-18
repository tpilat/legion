namespace Legion.Database;

public interface IConnectionStringProvider
{
	string GetDefaultConncetionString();

	string GetConncetionString(string storeId);

	string GetConncetionString(IInvocationContext invocationContext);
}
