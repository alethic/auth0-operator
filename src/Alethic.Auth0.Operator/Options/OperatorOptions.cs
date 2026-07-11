namespace Alethic.Auth0.Operator.Options
{

    public class OperatorOptions
    {

        /// <summary>
        /// Limit the operator to resources within the specified namespace.
        /// TODO
        /// </summary>
        public string? Namespace { get; set; }

        /// <summary>
        /// Options related to reconciliation of resources.
        /// </summary>
        public ReconciliationOptions Reconciliation { get; set; } = new ReconciliationOptions();

        /// <summary>
        /// Options related to client-side rate limiting of Auth0 Management API requests.
        /// </summary>
        public RateLimitOptions RateLimit { get; set; } = new RateLimitOptions();

    }

}
