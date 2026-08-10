<#
.SYNOPSIS
Regenerates Auth0ManagementApiRoutes.cs from Auth0's published OpenAPI specification.

.DESCRIPTION
The rate pacer classifies each outgoing request into a route template so that all requests against one
route share a learned rate limit bucket binding. The templates come from the Management API's public
specification rather than from guessing which path segments are identifiers.

Run this when Auth0 publishes new endpoints, then review the diff and rebuild.
#>
[CmdletBinding()]
param(
    [string] $SpecUrl = "https://auth0.com/docs/api/management/openapi.json",
    [string] $OutputPath = (Join-Path $PSScriptRoot "Auth0ManagementApiRoutes.cs")
)

$ErrorActionPreference = "Stop"

Write-Host "Downloading $SpecUrl"
$spec = Invoke-RestMethod -Uri $SpecUrl -TimeoutSec 120

# the specification's server url carries the /api/v2 prefix that its path keys omit; request paths have it
$routes = $spec.paths.PSObject.Properties.Name |
    ForEach-Object { "/api/v2" + ($_ -replace '\{[^}]+\}', '*') } |
    Sort-Object -Unique

Write-Host "Found $($routes.Count) route templates (spec version $($spec.info.version))"

$sb = [System.Text.StringBuilder]::new()
[void]$sb.AppendLine("namespace Alethic.Auth0.Operator.RateLimiting")
[void]$sb.AppendLine("{")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("    /// <summary>")
[void]$sb.AppendLine("    /// The Auth0 Management API route templates, with identifier segments as <c>*</c>. Generated from the")
[void]$sb.AppendLine("    /// official OpenAPI specification published at")
[void]$sb.AppendLine("    /// <c>$SpecUrl</c> (info.version $($spec.info.version)); regenerate with")
[void]$sb.AppendLine("    /// <c>Update-Auth0ManagementApiRoutes.ps1</c> rather than editing by hand.")
[void]$sb.AppendLine("    /// </summary>")
[void]$sb.AppendLine("    static class Auth0ManagementApiRoutes")
[void]$sb.AppendLine("    {")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("        /// <summary>")
[void]$sb.AppendLine("        /// The route templates, one string per route.")
[void]$sb.AppendLine("        /// </summary>")
[void]$sb.AppendLine("        public static readonly string[] All =")
[void]$sb.AppendLine("        [")

foreach ($route in $routes)
{
    [void]$sb.AppendLine("            `"$route`",")
}

[void]$sb.AppendLine("        ];")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("    }")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("}")

[System.IO.File]::WriteAllText($OutputPath, $sb.ToString())
Write-Host "Wrote $OutputPath"
