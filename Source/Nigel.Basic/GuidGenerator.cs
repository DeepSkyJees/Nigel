using System;

namespace Nigel.Basic
{
    /// <summary>
    ///     Class GuidGenerator.
    /// </summary>
    public static class GuidGenerator
    {
        // number of bytes in guid
        /// <summary>
        ///     The byte array size
        /// </summary>
        public const int ByteArraySize = 16;

        // multiplex variant info
        /// <summary>
        ///     The variant byte
        /// </summary>
        public const int VariantByte = 8;

        /// <summary>
        ///     The variant byte mask
        /// </summary>
        public const int VariantByteMask = 0x3f;

        /// <summary>
        ///     The variant byte shift
        /// </summary>
        public const int VariantByteShift = 0x80;

        // multiplex version info
        /// <summary>
        ///     The version byte
        /// </summary>
        public const int VersionByte = 7;

        /// <summary>
        ///     The version byte mask
        /// </summary>
        public const int VersionByteMask = 0x0f;

        /// <summary>
        ///     The version byte shift
        /// </summary>
        public const int VersionByteShift = 4;

        // indexes within the guid array for certain boundaries
        /// <summary>
        ///     The timestamps byte
        /// </summary>
        private const byte TimestampsByte = 0;

        /// <summary>
        ///     The unique identifier clock sequence byte
        /// </summary>
        private const byte GuidClockSequenceByte = 8;

        /// <summary>
        ///     The node byte
        /// </summary>
        private const byte NodeByte = 10;

        // offset to move from 1/1/0001, which is 0-time for .NET, to gregorian 0-time of 10/15/1582
        /// <summary>
        ///     The gregorian calendar start
        /// </summary>
        private static readonly DateTimeOffset GregorianCalendarStart =
            new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.Zero);


        /// <summary>
        ///     The see date time
        /// </summary>
        private static DateTime? seedDateTime;

        /// <summary>
        ///     Initializes static members of the <see cref="GuidGenerator" /> class.
        /// </summary>
        static GuidGenerator()
        {
            DefaultClockSequence = new byte[2];
            DefaultNode = new byte[6];

            var random = new Random();
            random.NextBytes(DefaultClockSequence);
            random.NextBytes(DefaultNode);
        }

        // random clock sequence and node
        /// <summary>
        ///     Gets or sets the default clock sequence.
        /// </summary>
        /// <value>The default clock sequence.</value>
        public static byte[] DefaultClockSequence { get; set; }

        /// <summary>
        ///     Gets or sets the default node.
        /// </summary>
        /// <value>The default node.</value>
        public static byte[] DefaultNode { get; set; }

        /// <summary>
        ///     Gets the version.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>GuidVersion.</returns>
        public static GuidVersion GetVersion(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            return (GuidVersion) ((bytes[VersionByte] & 0xFF) >> VersionByteShift);
        }

        /// <summary>
        ///     Gets the date time offset.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>DateTimeOffset.</returns>
        public static DateTimeOffset GetDateTimeOffset(Guid guid)
        {
            var bytes = guid.ToByteArray();

            // reverse the version
            bytes[VersionByte] &= VersionByteMask;
            bytes[VersionByte] |= (byte) GuidVersion.TimeBased >> VersionByteShift;

            var timestampsBytes = new byte[8];
            Array.Copy(bytes, TimestampsByte, timestampsBytes, 0, 8);

            var timestamps = BitConverter.ToInt64(timestampsBytes, 0);
            var ticks = timestamps + GregorianCalendarStart.Ticks;

            return new DateTimeOffset(ticks, TimeSpan.Zero);
        }

        /// <summary>
        ///     Gets the date time.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>DateTime.</returns>
        public static DateTime GetDateTime(Guid guid)
        {
            return GetDateTimeOffset(guid).DateTime;
        }

        /// <summary>
        ///     Gets the local date time.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>DateTime.</returns>
        public static DateTime GetLocalDateTime(Guid guid)
        {
            return GetDateTimeOffset(guid).LocalDateTime;
        }

        /// <summary>
        ///     Gets the UTC date time.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>DateTime.</returns>
        public static DateTime GetUtcDateTime(Guid guid)
        {
            return GetDateTimeOffset(guid).UtcDateTime;
        }

        /// <summary>
        ///     Generates the time based unique identifier.
        /// </summary>
        /// <returns>Guid.</returns>
        public static Guid GenerateTimeBasedGuid()
        {
            return GenerateTimeBasedGuid(DateTimeOffset.UtcNow, DefaultClockSequence, DefaultNode);
        }

        /// <summary>
        ///     Generates the time based unique identifier.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>Guid.</returns>
        public static Guid GenerateTimeBasedGuid(DateTime dateTime)
        {
            return GenerateTimeBasedGuid(dateTime, DefaultClockSequence, DefaultNode);
        }

        /// <summary>
        ///     Generates the time based unique identifier.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>Guid.</returns>
        public static Guid GenerateTimeBasedGuid(DateTimeOffset dateTime)
        {
            return GenerateTimeBasedGuid(dateTime, DefaultClockSequence, DefaultNode);
        }

        /// <summary>
        ///     Generates the time based unique identifier.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="clockSequence">The clock sequence.</param>
        /// <param name="node">The node.</param>
        /// <returns>Guid.</returns>
        public static Guid GenerateTimeBasedGuid(DateTime dateTime, byte[] clockSequence, byte[] node)
        {
            return GenerateTimeBasedGuid(new DateTimeOffset(dateTime), clockSequence, node);
        }


        /// <summary>
        ///     Generates the time based unique identifier.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="clockSequence">The clock sequence.</param>
        /// <param name="node">The node.</param>
        /// <param name="needReverse">if set to <c>true</c> [need reverse].</param>
        /// <returns>Guid.</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     clockSequence
        ///     or
        ///     node
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     clockSequence - The clockSequence must be 2 bytes.
        ///     or
        ///     node - The node must be 6 bytes.
        /// </exception>
        public static Guid GenerateTimeBasedGuid(DateTimeOffset dateTime, byte[] clockSequence, byte[] node)
        {
            if (seedDateTime == null) seedDateTime = dateTime.DateTime;

            if (seedDateTime.Value.Second != dateTime.Second) seedDateTime = dateTime.DateTime;
            dateTime = seedDateTime.Value.AddTicks(1);
            if (clockSequence == null)
                throw new ArgumentNullException(nameof(clockSequence));

            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (clockSequence.Length != 2)
                throw new ArgumentOutOfRangeException(nameof(clockSequence), "The clockSequence must be 2 bytes.");

            if (node.Length != 6)
                throw new ArgumentOutOfRangeException(nameof(node), "The node must be 6 bytes.");

            var ticks = (dateTime - GregorianCalendarStart).Ticks;
            var guid = new byte[ByteArraySize];
            var timestamps = BitConverter.GetBytes(ticks);


            // copy node
            Array.Copy(node, 0, guid, NodeByte, Math.Min(6, node.Length));

            // copy clock sequence
            Array.Copy(clockSequence, 0, guid, GuidClockSequenceByte, Math.Min(2, clockSequence.Length));

            // copy time stamp
            Array.Copy(timestamps, 0, guid, TimestampsByte, Math.Min(8, timestamps.Length));
            // set the variant
            guid[VariantByte] &= VariantByteMask;
            guid[VariantByte] |= VariantByteShift;

            // set the version
            guid[VersionByte] &= VersionByteMask;
            guid[VersionByte] |= (byte) GuidVersion.TimeBased << VersionByteShift;
            seedDateTime = seedDateTime.Value.AddTicks(1);
            return new Guid(guid);
        }
    }

    /// <summary>
    ///     Enum GuidVersion
    /// </summary>
    public enum GuidVersion
    {
        /// <summary>
        ///     The time based
        /// </summary>
        TimeBased = 0x01,

        /// <summary>
        ///     The reserved
        /// </summary>
        Reserved = 0x02,

        /// <summary>
        ///     The name based
        /// </summary>
        NameBased = 0x03,

        /// <summary>
        ///     The random
        /// </summary>
        Random = 0x04
    }
}