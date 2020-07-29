using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;

namespace Net5Preview7Repro
{
	/// <summary>
	/// A strong type Business identifier. Directly converts to/from an integer in the database.
	/// </summary>
	[TypeConverter(typeof(BusinessIdTypeConverter))]
	[DataContract]
	public class BusinessId : IComparable<BusinessId>, IEquatable<BusinessId>
	{
		/// <summary>
		/// The database value of the business.
		/// </summary>
		[DataMember(Order = 1)]
		public int Value { get; }

		/// <summary>
		/// The URL key for the business, if present.
		/// </summary>
		[DataMember(Order = 2)]
		public string? Key { private set; get; }


		public BusinessId(string? keyOrValue)
		{
			if (int.TryParse(keyOrValue, out var value))
			{
				Value = value;
			}
		}

		public BusinessId(int value)
		{
			Value = value;
		}

		public int CompareTo(BusinessId? other)
		{
			return Value.CompareTo(other?.Value);
		}

		public bool Equals(BusinessId? other)
		{
			return Value.Equals(other?.Value);
		}

		public override bool Equals(object? obj)
		{
			if (obj is null) return false;

			return obj is BusinessId other && Equals(other);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override string ToString()
		{
			return Key ?? Value.ToString();
		}

		public static bool operator ==(BusinessId? a, BusinessId? b)
		{
			if (a is null && b is null)
			{
				return true;
			}

			if (a is null && !(b is null))
			{
				return false;
			}

			if (b is null && !(a is null))
			{
				return false;
			}

			return a?.CompareTo(b) == 0;
		}
		public static bool operator !=(BusinessId? a, BusinessId? b) => !(a == b);

		public static implicit operator int(BusinessId? b) => b?.Value ?? 0;
		public static implicit operator BusinessId(int i) => new BusinessId(i);

		public static explicit operator string(BusinessId? b) => b?.Key ?? b?.Value.ToString()!;
		public static explicit operator BusinessId(string? i) => new BusinessId(i!);
	}

	class BusinessIdTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			var stringValue = value as string;
			if (!string.IsNullOrEmpty(stringValue)
				&& int.TryParse(stringValue, out var intValue))
			{
				return new BusinessId(intValue);
			}
			else
			{
				return new BusinessId(stringValue!);
			}
		}
	}
}
