namespace ApiClient.Exception
{
    /// <summary>
    ///     Base Api Exception
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ApiException : System.Exception
    {
        public ApiException(string message, System.Exception innerEx = null) :
            base(message, innerEx)
        {
        }
    }
}
