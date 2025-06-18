using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Legion.Generators.AppGen.DBAbstractions.Metamodel;

//
// Summary:
//     Indicates how a delete operation is applied to dependent entities in a relationship
//     when the principal is deleted or the relationship is severed.
//     Behaviors in the database are dependent on the database schema being created
//     appropriately. Using Entity Framework Migrations or Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade.EnsureCreated
//     will create the appropriate schema.
//     Note that the in-memory behavior for entities that are currently tracked by thea
//     Microsoft.EntityFrameworkCore.DbContext can be different from the behavior that
//     happens in the database. See the Microsoft.EntityFrameworkCore.DeleteBehavior.ClientSetNull
//     behavior for more details.
[Serializable]
[XmlType(Namespace = "http://generator.appgen.sk/Model/1.0")]
[XmlRoot(Namespace = "http://generator.appgen.sk/Model/1.0", IsNullable = false)]
public enum DeleteBehavior
{
	//
	// Summary:
	//     For entities being tracked by the Microsoft.EntityFrameworkCore.DbContext, the
	//     values of foreign key properties in dependent entities are set to null. This
	//     helps keep the graph of entities in a consistent state while they are being tracked,
	//     such that a fully consistent graph can then be written to the database. If a
	//     property cannot be set to null because it is not a nullable type, then an exception
	//     will be thrown when Microsoft.EntityFrameworkCore.DbContext.SaveChanges is called.
	//     This is the same as the Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull
	//     behavior.
	//     If the database has been created from the model using Entity Framework Migrations
	//     or the Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade.EnsureCreated
	//     method, then the behavior in the database is to generate an error if a foreign
	//     key constraint is violated. This is the same as the Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict
	//     behavior.
	//     This is the default for optional relationships. That is, for relationships that
	//     have nullable foreign keys.
	[JsonProperty]
	ClientSetNull = 0,
	//
	// Summary:
	//     For entities being tracked by the Microsoft.EntityFrameworkCore.DbContext, the
	//     values of foreign key properties in dependent entities are not changed. This
	//     can result in an inconsistent graph of entities where the values of foreign key
	//     properties do not match the relationships in the graph. If a property remains
	//     in this state when Microsoft.EntityFrameworkCore.DbContext.SaveChanges is called,
	//     then an exception will be thrown.
	//     If the database has been created from the model using Entity Framework Migrations
	//     or the Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade.EnsureCreated
	//     method, then the behavior in the database is to generate an error if a foreign
	//     key constraint is violated.
	[JsonProperty]
	Restrict = 1,
	//
	// Summary:
	//     For entities being tracked by the Microsoft.EntityFrameworkCore.DbContext, the
	//     values of foreign key properties in dependent entities are set to null. This
	//     helps keep the graph of entities in a consistent state while they are being tracked,
	//     such that a fully consistent graph can then be written to the database. If a
	//     property cannot be set to null because it is not a nullable type, then an exception
	//     will be thrown when Microsoft.EntityFrameworkCore.DbContext.SaveChanges is called.
	//     If the database has been created from the model using Entity Framework Migrations
	//     or the Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade.EnsureCreated
	//     method, then the behavior in the database is the same as is described above for
	//     tracked entities. Keep in mind that some databases cannot easily support this
	//     behavior, especially if there are cycles in relationships.
	[JsonProperty]
	SetNull = 2,
	//
	// Summary:
	//     For entities being tracked by the Microsoft.EntityFrameworkCore.DbContext, the
	//     dependent entities will also be deleted when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
	//     is called.
	//     If the database has been created from the model using Entity Framework Migrations
	//     or the Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade.EnsureCreated
	//     method, then the behavior in the database is the same as is described above for
	//     tracked entities. Keep in mind that some databases cannot easily support this
	//     behavior, especially if there are cycles in relationships.
	//     This is the default for required relationships. That is, for relationships that
	//     have non-nullable foreign keys.
	[JsonProperty]
	Cascade = 3
}
