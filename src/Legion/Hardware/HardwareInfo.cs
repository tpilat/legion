using Legion.Collections;
using Legion.Extensions;
using Legion.Infrastructure;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Legion.Hardware;

public class HardwareInfo : Serializer.IDictionaryObject, Serializer.ITextSerializer
{
	private string? _thumbprint;

	public static Guid RuntimeUniqueKey => EnvironmentInfo.RUNTIME_UNIQUE_KEY;
	public string HWThumbprint => _thumbprint ??= GetHWThumbprint();
	public string? ComputerSystemUUID { get; set; }
	public Bios? Bios { get; set; }
	public List<GraphicsCard>? GraphicsCards { get; set; }
	public HardDrives? HardDrives { get; set; }
	public Memory? Memory { get; set; }
	public MotherBoard? MotherBoard { get; set; }
	public List<NetworkAdapter>? NetworkAdapters { get; set; }
	public OS? OS { get; set; }
	public List<Processor>? Processors { get; set; }
	public List<SecurityProduct>? SecurityProducts { get; set; }
	public List<UserAccount>? UserAccounts { get; set; }

	public FlatHardwareInfo ToFlatHardwareInfo()
		=> new()
		{
			HWThumbprint = HWThumbprint,
			TotalMemoryCapacityGB = Memory?.TotalMemoryCapacityGB,
			MemoryAvailableGB = ByteHelper.ConvertToRoundedGigaBytes(Memory?.AvailableBytes),
			MemoryPercentUsed = Memory?.PercentUsed,
#pragma warning disable CS8629 // Nullable value type may be null.
			PercentProcessorIdleTime = (Processors != null && 0 < Processors.Count && Processors[0].PercentIdleTime.HasValue) ? Convert.ToDouble(Processors[0].PercentIdleTime.Value) : (double?)null,
			PercentProcessorTime = (Processors != null && 0 < Processors.Count && Processors[0].PercentProcessorTime.HasValue) ? Convert.ToDouble(Processors[0].PercentProcessorTime.Value) : (double?)null,
#pragma warning restore CS8629 // Nullable value type may be null.
			OS = OS?.ToString(),

			HWJson = Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
		};

	public IDictionary<string, object?> ToDictionary(Serializer.ISerializer? serializer = null)
	{
		var dict = new DictionaryBuilder<string>()
			.TryAdd(nameof(RuntimeUniqueKey), RuntimeUniqueKey, out _)
			.AddIfNotWhiteSpace(nameof(HWThumbprint), HWThumbprint, out _)
			.AddIfNotWhiteSpace(nameof(ComputerSystemUUID), ComputerSystemUUID, out _)
			.AddIfNotNull(nameof(OS), OS?.ToDictionary(), out _)
			.AddIfNotNull(nameof(Bios), Bios?.ToDictionary(), out _)
			.AddIfNotNull(nameof(HardDrives), HardDrives?.ToDictionary(), out _)
			.AddIfNotNull(nameof(Memory), Memory?.ToDictionary(), out _)
			.AddIfNotNull(nameof(MotherBoard), MotherBoard?.ToDictionary(), out _);

		if (GraphicsCards != null)
			for (int i = 0; i < GraphicsCards.Count; i++)
				dict.Add($"{nameof(GraphicsCards)}[{i}]", GraphicsCards[i]?.ToDictionary());

		if (NetworkAdapters != null)
			for (int i = 0; i < NetworkAdapters.Count; i++)
				dict.Add($"{nameof(NetworkAdapters)}[{i}]", NetworkAdapters[i]?.ToDictionary());

		if (Processors != null)
			for (int i = 0; i < Processors.Count; i++)
				dict.Add($"{nameof(Processors)}[{i}]", Processors[i]?.ToDictionary());

		if (SecurityProducts != null)
			for (int i = 0; i < SecurityProducts.Count; i++)
				dict.Add($"{nameof(SecurityProducts)}[{i}]", SecurityProducts[i]?.ToDictionary());

		if (UserAccounts != null)
			for (int i = 0; i < UserAccounts.Count; i++)
				dict.Add($"{nameof(UserAccounts)}[{i}]", UserAccounts[i]?.ToDictionary());

		return dict
			.ToObject();
	}

	public override string? ToString()
	{
		return HWThumbprint ?? base.ToString() ?? "";
	}

	public void WriteTo(StringBuilder sb, string? before = null, string? after = null)
	{
		sb
			.AppendLineSafe(before)
			.AppendLineSafe($"RuntimeUniqueKey = {RuntimeUniqueKey}")
			.AppendLineSafe(!string.IsNullOrWhiteSpace(HWThumbprint), () => $"HW = {this}")
			.AppendLineSafe(!string.IsNullOrWhiteSpace(ComputerSystemUUID), () => $"ComputerSystemUUID = {ComputerSystemUUID}");

		Bios?.WriteTo(sb);

		if (GraphicsCards != null)
			foreach (var gpu in GraphicsCards)
				gpu.WriteTo(sb);

		HardDrives?.WriteTo(sb);

		Memory?.WriteTo(sb);

		MotherBoard?.WriteTo(sb);

		if (NetworkAdapters != null)
			foreach (var net in NetworkAdapters)
				net.WriteTo(sb);

		OS?.WriteTo(sb);

		if (Processors != null)
			foreach (var cpu in Processors)
				cpu.WriteTo(sb);

		if (SecurityProducts != null)
			foreach (var sec in SecurityProducts)
				sec.WriteTo(sb);

		if (UserAccounts != null)
			foreach (var user in UserAccounts)
				user.WriteTo(sb);

		sb.AppendLineSafe(after);
	}

	private string GetHWThumbprint()
	{
		var thumbprintChunks = Processors?.Select(cpu => cpu.Id).ToList() ?? [];
		
		if (!string.IsNullOrWhiteSpace(ComputerSystemUUID))
			thumbprintChunks.Add(ComputerSystemUUID);

		var thumbprintBase = string.Join("_", thumbprintChunks);

		if (string.IsNullOrWhiteSpace(thumbprintBase))
			thumbprintBase = "0";

		return GetHash(thumbprintBase);
	}

	private static string GetHash(string source)
	{
		var csp = MD5.Create();
		byte[] raw = Encoding.ASCII.GetBytes(source);
		return GetHexString(csp.ComputeHash(raw));
	}

	private static string GetHexString(IList<byte> bt)
	{
		var sb = new StringBuilder();
		for (int i = 0; i < bt.Count; i++)
		{
			byte b = bt[i];
			int n = b;
			int n1 = n & 15;
			int n2 = (n >> 4) & 15;
			if (n2 > 9)
				sb.Append(((char)(n2 - 10 + 'A')).ToString(CultureInfo.InvariantCulture));
			else
				sb.Append(n2.ToString(CultureInfo.InvariantCulture));

			if (n1 > 9)
				sb.Append(((char)(n1 - 10 + 'A')).ToString(CultureInfo.InvariantCulture));
			else
				sb.Append(n1.ToString(CultureInfo.InvariantCulture));

			if ((i + 1) != bt.Count && (i + 1) % 2 == 0)
				sb.Append('-');
		}

		return sb.ToString();
	}
}
