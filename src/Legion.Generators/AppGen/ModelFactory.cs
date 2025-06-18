using Legion.Database.Metamodel;
using Legion.Generators.AppGen.Internal;
using Legion.Generators.AppGen.Model;
using Legion.Generators.AppGen.Model.Internal.DB;

namespace Legion.Generators.AppGen;

public class ModelFactory
{
	public DatabaseProviderType ProviderType { get; }
	public string ConnectionString { get; }

	public ModelFactory(string connectionString, DatabaseProviderType providerType)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
			throw new ArgumentNullException(nameof(connectionString));

		ProviderType = providerType;
		ConnectionString = connectionString;
	}

	public ModelBase CreateModel(out ModelResult result, ModelBase? modelExtension)
	{
		DBAbstractions.Metamodel.Model dbModel = new DBModelFactory().CreateModel(ConnectionString, ProviderType);

		var modelMapper = new ModelMapper(modelExtension, ProviderType);
		result = modelMapper.ModelResult;
		dbModel.Validate(result);

		foreach (var table in dbModel.TableEntities.OrderBy(n => n.TableName))
			modelMapper.AddTableEntity(table);

		foreach (var view in dbModel.ViewEntities.OrderBy(n => n.TableName))
			modelMapper.AddViewEntity(view);

		//******SetReferences******
		foreach (var table in dbModel.TableEntities)
		{
			var entityMapper = modelMapper.TableEntityMappers[table];

			foreach (var nav in table.Navigations)
			{
				var navigation = entityMapper.Navigations[nav];
				navigation.TargetType = modelMapper.TableEntityMappers[nav.TargetType].Entity;
			}

			foreach (var nav in table.BackNavigations)
			{
				var backNavigation = entityMapper.BackNavigations[nav];
				backNavigation.TargetType = modelMapper.TableEntityMappers[nav.TargetType].Entity;
			}

			foreach (var fk in table.ForeignKeys)
			{
				var principalEntityMapper = modelMapper.TableEntityMappers[fk.PrincipalEntity];

				var foreignKey = entityMapper.ForeignKeys[fk];
				foreignKey.PrincipalEntity = principalEntityMapper.Entity;
				foreignKey.DependentToPrincipal = entityMapper.Navigations[fk.DependentToPrincipal];
				foreignKey.PrincipalToDependent = principalEntityMapper.BackNavigations[fk.PrincipalToDependent];
				foreignKey.PrincipalKey = principalEntityMapper.Entity.PrimaryKey;

				foreignKey.DependentToPrincipal.ForeignKey = foreignKey;
				foreignKey.PrincipalToDependent.ForeignKey = foreignKey;
			}

			entityMapper.AddAllIndexes(table);
		}

		if (modelMapper.Extension?.Packages != null)
		{
			var removedPackageExts = new List<PackageBase>();
			foreach (var packageExt in modelMapper.Extension.Packages)
			{
				var package = modelMapper.Model.GetPackage(packageExt.ID);
				if (package == null)
				{
					removedPackageExts.Add(packageExt);
					continue;
				}

				if (packageExt.Parent != null)
				{
					var parent = modelMapper.Model.GetPackage(packageExt.Parent.ID);
					package.Parent = parent;
				}
				if (packageExt.ChildPackages != null)
				{
					foreach (var childPackageExt in packageExt.ChildPackages)
					{
						var child = modelMapper.Model.GetPackage(childPackageExt.ID);
						if (package.ChildPackages == null)
							package.ChildPackages = new List<PackageBase>();

						package.ChildPackages.Add(child);
					}
				}
			}

			foreach (var removedPackageExt in removedPackageExts)
				modelMapper.Extension.Packages.Remove(removedPackageExt);
		}

		foreach (var seq in dbModel.Sequences.OrderBy(s => s.Schema).ThenBy(s => s.Name))
			modelMapper.AddSequence(seq);

		foreach (var entity in modelMapper.TableEntityMappers.Values.Select(x => x.Entity))
		{
			entity.Build();
			foreach (var kvp in entity.ModelContextNames)
			{
				if (modelMapper.Model.ModelContextDict.TryGetValue(kvp.Key, out var contextName))
				{
					if (kvp.Value != contextName)
						throw new InvalidOperationException($"{entity.ID}: Invalid context name {kvp.Value} | Expected {contextName}");

					if (modelMapper.Model.ModelEntitiesDict.TryGetValue(kvp.Key, out var modelEntities))
					{
						modelEntities.Add(entity);
					}
					else
					{
						throw new InvalidOperationException($"{entity.ID}: not found in {nameof(modelMapper.Model.ModelEntitiesDict)}");
					}
				}
				else
				{
					modelMapper.Model.ModelContextDict.Add(kvp.Key, kvp.Value);
					modelMapper.Model.ModelEntitiesDict.Add(kvp.Key, [entity]);
				}
			}
		}

		foreach (var queryEntity in modelMapper.ViewEntityMappers.Values.Select(x => x.QueryEntity))
		{
			queryEntity.Build();
			foreach (var kvp in queryEntity.QueryModelContextNames)
			{
				if (modelMapper.Model.QueryModelContextDict.TryGetValue(kvp.Key, out var contextName))
				{
					if (kvp.Value != contextName)
						throw new InvalidOperationException($"{queryEntity.ID}: Invalid context name {kvp.Value} | Expected {contextName}");

					if (modelMapper.Model.QueryModelEntitiesDict.TryGetValue(kvp.Key, out var queryModelEntities))
					{
						queryModelEntities.Add(queryEntity);
					}
					else
					{
						throw new InvalidOperationException($"{queryEntity.ID}: not found in {nameof(modelMapper.Model.QueryModelEntitiesDict)}");
					}
				}
				else
				{
					modelMapper.Model.QueryModelContextDict.Add(kvp.Key, kvp.Value);
					modelMapper.Model.QueryModelEntitiesDict.Add(kvp.Key, [queryEntity]);
				}
			}
		}

		return modelMapper.Model;
	}
}
