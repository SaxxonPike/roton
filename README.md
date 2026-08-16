# Roton (with Lyon frontend)

[![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)](https://raw.githubusercontent.com/SaxxonPike/roton/master/LICENSE)

This C# project consists of two major components, **Roton** and **Lyon**.

Special thanks to Asiekierka for the [Reconstruction of ZZT](https://github.com/asiekierka/reconstruction-of-zzt) project. Without this, some of the
really edge case stuff probably wouldn't have been implemented.

### Roton

Roton is an emulation of the ZZT and Super ZZT game creation engines.

Target framework is .NET Standard 2.0. This means it should be compatible with any modern version of .NET
and can be linked from .NET Framework 4.6.1+ projects as well.

### Lyon

Lyon is an application that runs an instance of Roton's emulation and presents it via SDL.
We use [ppy/SDL3-CS](https://github.com/ppy/SDL3-CS) for this.

Target framework is .NET 10 or above. If you are able to install .NET 10, it is very likely you
will also be able to run Lyon on your operating system.

### Target Frameworks

- Lyon
  - .NET 10+
- Roton
  - .NET Standard 2.0

### Build instructions

The [.NET Core SDK](https://github.com/dotnet/core/blob/master/release-notes/download-archive.md)
is required. Output goes to `/Deploy`.

Clone the repo:

```
git clone https://github.com/SaxxonPike/roton.git
cd roton
git submodule init
git submodule update
```

Restore NuGet packages and build:

```
dotnet restore
dotnet build
```

Run unit tests:

```
dotnet test Source/Roton.Test
```

Build and run:

```
dotnet run --project Source/Lyon <path-to-game>
```

### Using Roton in your own project

Roton provides a `RotonServices` class that can be used to retrieve the type mapping needed by your favorite
dependency injection library.

AutoFac:

```csharp
// Each concrete type must have all its services registered at the same time
// so that AutoFac knows that they all refer to the same instance.

var map = RotonServices.Get(context, additionalAssemblies)
    .GroupBy(s => s.Implementation);

foreach (var serviceGroup in map)
    builder.RegisterType(serviceGroup.Key)
        .As(serviceGroup.Select(sg => sg.Service).ToArray())
        .AutoActivate()
        .SingleInstance();
```

### Where can I learn more about ZZT?

- https://museumofzzt.com/ - a preservation site for all things ZZT. The original games can be found here, plus a massive library of others from the community over the years. Administered by Dr. Dos. (The developers *really* appreciate this site.)
- http://zzt.org/fora/ - if you want to immerse yourself in the culture and in-jokes, this forum has preserved everything since 2003. Be warned: it's a bit juvenile in there.

### How can I contribute to Roton?

Contributions are accepted differently depending on the nature of the contribution.

##### ZZT emulation fixes

Odd things found in Roton's emulation methods are very likely a product of the reverse engineering process. Changes can be
submitted, but often times it's better to file a bug than to commit code to the emulator core unless you've verified using
the provided [IDA databases](https://www.hex-rays.com/products/ida/). These databases are stored in the [IDBs folder](/IDBs).

##### Lyon, frontend fixes

Fixes to the frontend or SDL integration are very welcome!

##### Process

Get in touch with either @SaxxonPike or @Spectere once you've finished your work so it can be approved for inclusion
in the master branch.
