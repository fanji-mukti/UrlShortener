namespace UrlShortener.Core.Services
{
    /// <summary> 
    /// Generates unique identifiers using the Snowflake algorithm.
    /// </summary>
    public sealed class SnowflakeIdGenerator : IIdGenerator
    {
        private const long Epoch = 1735689600000L; // Custom epoch (January 1, 2025)
        private const int DatacenterIdBits = 5;
        private const int WorkerIdBits = 5;
        private const int SequenceBits = 12;

        private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);
        private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);
        private const long MaxSequence = -1L ^ (-1L << SequenceBits);

        private const int WorkerIdShift = SequenceBits;
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
        private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;

        private readonly object _lock = new object();

        private readonly long _datacenterId;
        private readonly long _workerId;
        private readonly ITimeProvider _timeProvider;

        private long _lastTimestamp = -1L;
        private long _sequence = 0L;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnowflakeIdGenerator"/> class.
        /// </summary>
        /// <param name="datacenterId">The datacenter ID (0 to MaxDatacenterId).</param>
        /// <param name="workerId">The worker ID (0 to MaxWorkerId).</param>
        /// <param name="timeProvider">The time provider to use for generating timestamps.</param>
        /// <exception cref="ArgumentException">Thrown when datacenterId or workerId are out of bounds.</exception>
        public SnowflakeIdGenerator(long datacenterId, long workerId, ITimeProvider timeProvider)
        {
            if (datacenterId > MaxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException($"Datacenter ID must be between 0 and {MaxDatacenterId}");
            }
            if (workerId > MaxWorkerId || workerId < 0)
            {
                throw new ArgumentException($"Worker ID must be between 0 and {MaxWorkerId}");
            }

            _datacenterId = datacenterId;
            _workerId = workerId;
            _timeProvider = timeProvider;
        }

        /// <summary> 
        /// Generates a unique identifier using the Snowflake algorithm.
        /// </summary>
        /// <returns>A unique identifier as a long value.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the system clock moves backwards.</exception>
        public long GenerateId()
        {
            lock (_lock)
            {
                var timestamp = _timeProvider.GetCurrentTimeMilliseconds();

                if (timestamp < _lastTimestamp)
                {
                    throw new InvalidOperationException("Clock moved backwards. Refusing to generate id.");
                }

                if (timestamp == _lastTimestamp)
                {
                    _sequence = (_sequence + 1) & MaxSequence;
                    if (_sequence == 0)
                    {
                        timestamp = WaitForNextMillisecond(_lastTimestamp);
                    }
                }
                else
                {
                    _sequence = 0;
                }

                _lastTimestamp = timestamp;

                return ((timestamp - Epoch) << TimestampLeftShift) |
                       (_datacenterId << DatacenterIdShift) |
                       (_workerId << WorkerIdShift) |
                       _sequence;
            }
        }

        private long WaitForNextMillisecond(long lastTimestamp)
        {
            var timestamp = _timeProvider.GetCurrentTimeMilliseconds();
            while (timestamp <= lastTimestamp)
            {
                timestamp = _timeProvider.GetCurrentTimeMilliseconds();
            }
            return timestamp;
        }
    }

}