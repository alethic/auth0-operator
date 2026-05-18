using Auth0.ManagementApi.Connections;
var t = typeof(ConnectionOptionsAuth0);
foreach (var p in t.GetProperties())
    Console.WriteLine(p.Name);
