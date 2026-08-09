using System;

namespace Alethic.Auth0.Operator.RateLimiting
{

    /// <summary>
    /// Thrown before a request is sent when the circuit for the target Auth0 domain is open because a previous
    /// request received a 429. Callers should reschedule for after <see cref="OpenUntil"/> rather than retry.
    /// </summary>
    public sealed class Auth0CircuitOpenException : Exception
    {

        /// <summary>
        /// Walks the exception chain for an <see cref="Auth0CircuitOpenException"/>, unwrapping aggregate and
        /// wrapper exceptions the SDK may introduce.
        /// </summary>
        /// <param name="exception"></param>
        public static Auth0CircuitOpenException? Find(Exception? exception)
        {
            for (var e = exception; e is not null; e = e.InnerException)
                if (e is Auth0CircuitOpenException circuitOpen)
                    return circuitOpen;

            if (exception is AggregateException aggregate)
                foreach (var inner in aggregate.InnerExceptions)
                    if (Find(inner) is { } found)
                        return found;

            return null;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="openUntil"></param>
        public Auth0CircuitOpenException(string host, DateTimeOffset openUntil) :
            base($"Auth0 rate limit circuit for {host} is open until {openUntil:O}; not sending request.")
        {
            Host = host;
            OpenUntil = openUntil;
        }

        /// <summary>
        /// Gets the Auth0 domain whose circuit is open.
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// Gets the time at which the circuit closes and requests flow again.
        /// </summary>
        public DateTimeOffset OpenUntil { get; }

    }

}
