namespace Nigel.Extensions.Swashbuckle
{
    /// <summary>
    /// Class SwaggerOptions.
    /// </summary>
    public class SwaggerOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerOptions"/> class.
        /// </summary>
        public SwaggerOptions()
        {
            Enabled = false;
            ReDocEnabled = false;
            Name = string.Empty;
            Title = string.Empty;
            Version = string.Empty;
            RoutePrefix = string.Empty;
            IncludeSecurity = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SwaggerOptions"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [re document enabled].
        /// </summary>
        /// <value><c>true</c> if [re document enabled]; otherwise, <c>false</c>.</value>
        public bool ReDocEnabled { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the route prefix.
        /// </summary>
        /// <value>The route prefix.</value>
        public string RoutePrefix { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [include security].
        /// </summary>
        /// <value><c>true</c> if [include security]; otherwise, <c>false</c>.</value>
        public bool IncludeSecurity { get; set; }

        /// <summary>
        /// Gets or sets the XML files.
        /// </summary>
        /// <value>The XML files.</value>
        public string XmlFile { get; set; }
    }
}