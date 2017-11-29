namespace DigiKey.Api.Models
{
    /// <summary>
    ///     Very simple version of Keyword Search request for WebApp and IntegrationExams
    /// </summary>
    public class KeywordSearchRequest
    {
        /// <summary>
        ///     Gets or sets the keywords.
        /// </summary>
        /// <value>
        ///     The keywords.
        /// </value>
        public string Keywords { get; set; }

        /// <summary>
        ///     Gets or sets the record count.
        /// </summary>
        /// <value>
        ///     The record count.
        /// </value>
        public int RecordCount { get; set; }
    }
}
