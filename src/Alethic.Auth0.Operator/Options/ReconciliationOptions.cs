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
        public TimeSpan ErrorInterval { get; set; } = TimeSpan.FromMinutes(5);

    }

}