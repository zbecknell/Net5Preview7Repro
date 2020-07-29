namespace Net5Preview7Repro
{
	public class BusinessKey
	{
		public BusinessId BusinessId { get; set; } = null!;
		public string Key { get; set; } = null!;
	}

	public class BusinessKeyViewModel : IMapTo<BusinessKey>
	{
		public BusinessId BusinessId { get; set; } = null!;
		public string Key { get; set; } = null!;
		public bool IsPrimary { get; set; }
	}
}
