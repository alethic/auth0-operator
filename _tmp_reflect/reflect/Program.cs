using System.Reflection;
var asm = Assembly.LoadFrom(@"D:\auth0-operator\src\Alethic.Auth0.Operator\bin\Debug\net10.0\Auth0.ManagementApi.dll");
foreach (var t in new[]{"ConnectionDecryptionKeySaml","ConnectionFieldsMapSamlValue"}) {
    var type = asm.GetTypes().FirstOrDefault(x => x.Name == t);
    Console.WriteLine($"=== {t} ===");
    foreach (var m in type.GetMethods(BindingFlags.Public|BindingFlags.Static|BindingFlags.Instance))
        Console.WriteLine($"  {(m.IsStatic?"static":"")} {m.ReturnType.Name} {m.Name}({string.Join(", ",m.GetParameters().Select(p=>p.ParameterType.Name+" "+p.Name))})");
    foreach (var p in type.GetProperties(BindingFlags.Public|BindingFlags.Instance))
        Console.WriteLine($"  prop {p.PropertyType.Name} {p.Name} get={p.CanRead} set={p.CanWrite}");
}
