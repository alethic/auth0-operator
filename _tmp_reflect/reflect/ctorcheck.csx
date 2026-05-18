using System.Reflection;
var asm = Assembly.LoadFrom(@"D:\auth0-operator\src\Alethic.Auth0.Operator\bin\Debug\net10.0\Auth0.ManagementApi.dll");
foreach (var typeName in new[]{"ConnectionDecryptionKeySaml","ConnectionFieldsMapSamlValue"}) {
    var t = asm.GetTypes().FirstOrDefault(x => x.Name == typeName);
    if (t == null) { Console.WriteLine(typeName + ": not found"); continue; }
    Console.WriteLine("=== " + t.FullName + " ===");
    foreach (var c in t.GetConstructors(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance))
        Console.WriteLine("  ctor(" + string.Join(", ", c.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name)) + ")");
    foreach (var p in t.GetProperties(BindingFlags.Public|BindingFlags.Instance))
        Console.WriteLine("  prop " + p.Name + ": " + p.PropertyType.Name + " set=" + p.CanWrite);
}
