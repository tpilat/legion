using Legion.Extensions;

namespace Legion.IOUtils;

public static class DirectoryHelper
{
	public static readonly string DirectorySeparatorCharAsString = $"{Path.DirectorySeparatorChar}";

	public static List<string>? GetParentsList(string path, int depth)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(path);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (depth < 0)
			throw new ArgumentOutOfRangeException(nameof(depth));

		path = path.Trim();

		if (depth == 0)
			return [];

		if (depth == 1)
		{
			var p = Directory.GetParent(path)?.Name;
			return string.IsNullOrWhiteSpace(p)
				? []
				: [p!];
		}

		var currentDepth = 1;
		var parts = new List<string>();
		var parent = Directory.GetParent(path);
		while (parent != null && currentDepth <= depth)
		{
			parts.Add(parent.Name);
			parent = parent.Parent;
			currentDepth++;
		}

		parts.Reverse();
		return parts;
	}

	public static string? GetParents(string path, int depth, string delimiter)
	{
		var parts = GetParentsList(path, depth);

		if (parts == null)
			return null;

		return string.Join(delimiter, parts);
	}

	public static string CombinePaths(string path1, string path2, bool allowMovingUpPath2ToParents = false)
	{
		if (string.IsNullOrWhiteSpace(path1))
			return path2;

		if (string.IsNullOrWhiteSpace(path2))
			return path1;

		if (allowMovingUpPath2ToParents)
		{
			var path2Split = path2.Split([DirectorySeparatorCharAsString], StringSplitOptions.None);
			var moveUpCount = 0;
			if (path2Split[moveUpCount] == "..")
			{
				do
				{
					moveUpCount++;
				} while (moveUpCount < path2Split.Length && path2Split[moveUpCount] == "..");

				var path1Split = path1.Split([DirectorySeparatorCharAsString], StringSplitOptions.None);

				if (moveUpCount == path1Split.Length)
				{
					return string.Join(DirectorySeparatorCharAsString, path2Split.Skip(moveUpCount));
				}
				else if (moveUpCount < path1Split.Length)
				{
					path1 = string.Join(DirectorySeparatorCharAsString, path1Split.Take(path1Split.Length - moveUpCount));
					path2 = string.Join(DirectorySeparatorCharAsString, path2Split.Skip(moveUpCount));

					if (string.IsNullOrWhiteSpace(path1))
						return path2;

					if (string.IsNullOrWhiteSpace(path2))
						return path1;

					return Path.Combine(path1, path2);
				}
				else
				{
					Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.DirectoryException.UnableToMoveUp(path1, moveUpCount));
				}
			}
		}

		return Path.Combine(
			path1.TrimPostfix(DirectorySeparatorCharAsString),
			path2.TrimPrefix(DirectorySeparatorCharAsString));
	}
}
