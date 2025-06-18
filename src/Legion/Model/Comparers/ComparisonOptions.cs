namespace Legion.Model.Comparers;

public enum ComparisonOptions
{
	CompareProperties = 1,
	CompareReferences = 1 << 1,
	CompareAll = CompareProperties | CompareReferences
}
