using Legion.Database.Metamodel;

namespace Legion.Database.SqlServer;

public enum SqlServerForeignKeyUpdateDeleteActionsEnum
{
	NO_ACTION = 1,
	//Restrict = 2,
	CASCADE = 3,
	SET_NULL = 4,
	SET_DEFAULT = 5
}

public static class SqlServerForeignKeyUpdateDeleteActionsEnumExtensions
{
	public static ReferentialAction ConvertToReferentialAction(this SqlServerForeignKeyUpdateDeleteActionsEnum sqlServerForeignKeyUpdateDeleteActionsEnum)
		=> sqlServerForeignKeyUpdateDeleteActionsEnum switch
		{
			SqlServerForeignKeyUpdateDeleteActionsEnum.NO_ACTION => ReferentialAction.NoAction,
			SqlServerForeignKeyUpdateDeleteActionsEnum.CASCADE => ReferentialAction.Cascade,
			SqlServerForeignKeyUpdateDeleteActionsEnum.SET_NULL => ReferentialAction.SetNull,
			SqlServerForeignKeyUpdateDeleteActionsEnum.SET_DEFAULT => ReferentialAction.SetDefault,
			_ => throw new NotSupportedException(),
		};
}
