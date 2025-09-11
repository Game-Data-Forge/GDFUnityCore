namespace GDFEditor
{
    /// <summary>
    /// Represents an interface that defines credentials functionality.
    /// </summary>
    public interface ICredentials
    {
        /// <summary>
        /// Gets or sets the name associated with the <see cref="ICredentials"/> implementation.
        /// </summary>
        public string Name { get; set; }
    }
}
