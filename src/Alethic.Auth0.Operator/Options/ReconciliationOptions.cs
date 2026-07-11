using System;

namespace Alethic.Auth0.Operator.Options
{

    /// <summary>
    /// Configuration for reconciliation behavior.
    /// </summary>
    public class ReconciliationOptions
    {

        /// <summary>
        /// The interval between periodic reconciliation cycles.
        /// </summary>
        public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// The interval between periodic reconciliation cycles when an error occurs.
        /// </summary>
        public TimeSpan RetryInterval { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// The maximum number of resources reconciled in parallel. Bounds the burst of Auth0 API calls issued when
        /// many resources reconcile at once (e.g. on operator startup), complementing the HTTP-level rate limiter.
        /// </summary>
        public int MaxParallelReconciliations { get; set; } = 4;

    }

}