using Azure;
using Azure.Data.Tables;
using System;

namespace EA.KodTest
{
    /// <summary>
    /// Base class for Azure Storage table entities
    /// </summary>
    public abstract class TableEntityBase : ITableEntity
    {
        /// <summary>
        /// Partition key
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// Row key
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// Time stamp
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// HTTP ETag
        /// </summary>
        public ETag ETag { get; set; }
    }
}
