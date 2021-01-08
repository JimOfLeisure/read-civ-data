My VSCode environment is dotnet core targeting .Net Standard, and Godot is Mono targeting .Net framework 4.5.1, so VSCode isn't doing auto completion for me.

I'm sure there's a better way, but if I stuff this in csproj then VSCode is happy but Godot loses its mind. Eventually by deleting this and restarting everything Godot might be ok again.

```xml
  <ItemGroup>
    <PackageReference Include="Microsoft.NETFramework.ReferenceAssemblies" Version="1.0.0" PrivateAssets="All" />
  </ItemGroup>
```
