namespace EA.KodTest
{
    /// <summary>
    /// Azure Storage table Package entity
    /// </summary>
    public class Package : TableEntityBase
    {
        /// <summary>
        /// Package number
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Package weight (kg)
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Package length (cm)
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Package height (cm)
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Package width (cm)
        /// </summary>
        public double Width { get; set; }
    }
}
