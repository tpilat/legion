using Legion.Extensions;
using System.Collections.Concurrent;

namespace Legion.Trackers;

/// <summary>
/// Tracks the lifetime of objects by storing weak references to them.
/// <para>Useful for debugging object lifetime issues.</para>
/// <example>
/// <code>
/// public MyClass() {
///		IdMyClass = Guid.NewGuid();
///		ObjectLifetimeTracker.Track(this, IdMyClass.ToString());
/// }
/// </code>
/// </example>
/// <example>
/// <code>
/// var anotherObj = new AnotherClass();
/// ObjectLifetimeTracker.Track(anotherObj, "AnotherClassInstance");
/// var anotherStatus = ObjectLifetimeTracker.GetObjectsLifetimeStatus();
/// </code>
/// </example>
/// </summary>
public static class ObjectLifetimeTracker
{
	private record TypeWithIdentifier(
		Type Type,
		string Identifier,
		bool ManualDisposed)
	{
		public string GetKey()
			=> $"{Type.ToFriendlyFullName()} | {Identifier}";
	}

	public record ObjectLifetimeStatus(
		Type Type,
		string Identifier,
		bool IsDisposed,
		bool ManualDisposed,
		object? Object)
	{
		public override string ToString()
		{
			if (IsDisposed)
				return $"{Type.Name} - {Identifier} | {nameof(ManualDisposed)} = {ManualDisposed}";
			else
				return $"{Type.Name} - {Identifier}";
		}
	}

	public record ObjectsLifetimeStatuses(
		int AliveCount,
		int DisposedCount,
		Dictionary<Type, List<ObjectLifetimeStatus>> Alive,
		Dictionary<Type, List<ObjectLifetimeStatus>> Disposed);

	private static readonly ConcurrentDictionary<WeakReference, TypeWithIdentifier> _references = [];
	private static readonly ConcurrentDictionary<string, WeakReference> _referenceKeys = [];

	/// <summary>
	/// Tracks the specified object with the given identifier.
	/// <para>Call from any constructor to track the object's lifetime</para>
	/// </summary>
	/// <param name="obj">The object to track.</param>
	/// <param name="identifier">The identifier for the object.</param>
	public static void Track(object obj, string identifier)
	{
		Throw.IfArgumentNull(obj);

		var weakReference = new WeakReference(obj);
		var typeWithIdentifier = new TypeWithIdentifier(obj.GetType(), identifier, false);
		var key = typeWithIdentifier.GetKey();

		var added = _referenceKeys.TryAdd(key, weakReference);

		if (!added)
			Throw.InvalidOperationException($"The key is already being tracked | key = {key}.");

		added = _references.TryAdd(weakReference, typeWithIdentifier);

		if (!added)
			Throw.InvalidOperationException("The object is already being tracked.");
	}

	public static void SetDisposed(object obj, string identifier)
	{
		var key = new TypeWithIdentifier(obj.GetType(), identifier, false).GetKey();

		if (_referenceKeys.TryGetValue(key, out var weakReference))
		{
			if (_references.TryGetValue(weakReference, out var typeWithIdentifier))
			{
				_references[weakReference] = new TypeWithIdentifier(typeWithIdentifier.Type, typeWithIdentifier.Identifier, true);
			}
			else
			{

			}
		}
		else
		{

		}
	}

	/// <summary>
	/// Gets the lifetime status of tracked objects.
	/// </summary>
	/// <returns>A tuple containing lists of alive and disposed object identifiers.</returns>
	public static ObjectsLifetimeStatuses GetObjectsLifetimeStatus()
	{
		var alive = new List<ObjectLifetimeStatus>();
		var disposed = new List<ObjectLifetimeStatus>();

		foreach (var kvp in _references)
		{
			if (kvp.Key.IsAlive && !kvp.Value.ManualDisposed)
			{
				alive.Add(new ObjectLifetimeStatus(
					kvp.Value.Type,
					kvp.Value.Identifier,
					false,
					kvp.Value.ManualDisposed,
					kvp.Key.Target));
			}
			else
			{
				disposed.Add(new ObjectLifetimeStatus(
					kvp.Value.Type,
					kvp.Value.Identifier,
					true,
					kvp.Value.ManualDisposed,
					kvp.Key.Target));
			}
		}

		var a = alive.GroupBy(twi => twi.Type).ToDictionary(x => x.Key, x => x.ToList());
		var d = disposed.GroupBy(twi => twi.Type).ToDictionary(x => x.Key, x => x.ToList());

		return new ObjectsLifetimeStatuses(
			a.SelectMany(x => x.Value).Count(),
			d.SelectMany(x => x.Value).Count(),
			a,
			d);
	}

	/// <summary>
	/// Clears all tracked object references.
	/// </summary>
	public static void Clear()
		=> _references.Clear();
}
