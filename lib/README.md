# lib

Drop `Microsoft.MetadirectoryServicesEx.dll` here before building.

It ships with the MIM Synchronization Service install, typically at:

```
%ProgramFiles%\Microsoft Forefront Identity Manager\2010\Synchronization Service\Bin\Assemblies\Microsoft.MetadirectoryServicesEx.dll
```

This DLL is not redistributable — it's excluded from git (see `.gitignore`). Every dev machine / build agent needs its own copy from a local MIM Sync Service install.
