using Legion.Converters;
using Legion.Serializer;

namespace Legion.Model.Audit;

public class ApplicationEntryToken : IDictionaryObject
{
	public Guid IdApplicationEntryToken { get; }
	public string Token { get; }
	public int Version { get; }
	public string SourceFilePath { get; }
	public string? MethodInfo { get; set; }
	public string? MainEntityName { get; set; }
	public string? Description { get; set; }
	public string? TokenHistory { get; set; }

	public ApplicationEntryToken(Guid idApplicationEntryToken, string token, int version, string sourceFilePath)
	{
		if (string.IsNullOrWhiteSpace(token))
			throw new ArgumentNullException(nameof(token));

		if (string.IsNullOrWhiteSpace(sourceFilePath))
			throw new ArgumentNullException(nameof(sourceFilePath));

		IdApplicationEntryToken = idApplicationEntryToken;
		Token = token;
		Version = version;
		SourceFilePath = sourceFilePath;
	}

	public ApplicationEntryToken(string token, int version, string sourceFilePath)
	{
		if (string.IsNullOrWhiteSpace(token))
			throw new ArgumentNullException(nameof(token));

		if (string.IsNullOrWhiteSpace(sourceFilePath))
			throw new ArgumentNullException(nameof(sourceFilePath));

		Token = token;
		Version = version;
		SourceFilePath = sourceFilePath;
		IdApplicationEntryToken = GuidConverter.ToGuid($"{Token}_{Version}_{SourceFilePath}");
	}

	public ApplicationEntryToken WriteToHistory()
	{
		if (string.IsNullOrWhiteSpace(TokenHistory))
		{
			var tokenHistory = new List<ApplicationEntryTokenHistory>
			{
				new() {
					MethodInfo = MethodInfo,
					MainEntityName = MainEntityName,
					Description = Description
				}
			};

			TokenHistory = JsonSerializerHelper.Serialize(tokenHistory);
		}
		else
		{
			var historyList = JsonSerializerHelper.Deserialize<List<ApplicationEntryTokenHistory>>(TokenHistory!);
			if (historyList == null)
			{
				TokenHistory = JsonSerializerHelper.Serialize(new List<ApplicationEntryTokenHistory>
				{
					new()
					{
						MethodInfo = MethodInfo,
						MainEntityName = MainEntityName,
						Description = Description
					}
				});
			}
			else
			{
				if (!historyList.Any(x => x.MethodInfo == MethodInfo && x.MainEntityName == MainEntityName && x.Description == Description))
				{
					historyList.Add(
						new()
						{
							MethodInfo = MethodInfo,
							MainEntityName = MainEntityName,
							Description = Description
						});

					TokenHistory = JsonSerializerHelper.Serialize(historyList);
				}
			}
		}

		return this;
	}

	public IDictionary<string, object?> ToDictionary(ISerializer? serializer = null)
	{
		var dict = new Dictionary<string, object?>
		{
			{ nameof(IdApplicationEntryToken), IdApplicationEntryToken },
			{ nameof(Version), Version }
		};

		if (!string.IsNullOrWhiteSpace(Token))
			dict.Add(nameof(Token), Token);

		if (!string.IsNullOrWhiteSpace(SourceFilePath))
			dict.Add(nameof(SourceFilePath), SourceFilePath);

		if (!string.IsNullOrWhiteSpace(MethodInfo))
			dict.Add(nameof(MethodInfo), MethodInfo);

		if (!string.IsNullOrWhiteSpace(MainEntityName))
			dict.Add(nameof(MainEntityName), MainEntityName);

		if (!string.IsNullOrWhiteSpace(Description))
			dict.Add(nameof(Description), Description);

		if (!string.IsNullOrWhiteSpace(TokenHistory))
			dict.Add(nameof(TokenHistory), TokenHistory);

		return dict;
	}

	public override string? ToString()
		=> Token;
}
