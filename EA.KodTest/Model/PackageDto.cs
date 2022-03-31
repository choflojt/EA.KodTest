namespace EA.KodTest
{
    /// <summary>
    /// Package entity DTO class
    /// </summary>
    internal class PackageDto
    {
        /// <summary>
        /// Initialize a new instance of <see cref="PackageDto"/>.
        /// </summary>
        public PackageDto() { }

        /// <summary>
        /// Initialize a new instance of <see cref="PackageDto"/>.
        /// </summary>
        /// <param name="entity">Entity object</param>
        public PackageDto(Package entity)
            : this()
        {
            Number = entity.Number;
            Weight = entity.Weight;
            Length = entity.Length;
            Height = entity.Height;
            Width = entity.Width;
        }

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
