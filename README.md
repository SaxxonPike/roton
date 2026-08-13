# Roton (with Lyon frontend)

[![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)](https://raw.githubusercontent.com/SaxxonPike/roton/master/LICENSE)

##### Description

Roton is an emulation of the ZZT and Super ZZT game creation engines written in C#.

Lyon is an application that runs an instance of Roton's emulation and presents it via SDL.
We use [ppy/SDL3-CS](https://github.com/ppy/SDL3-CS) for this.

Windows, MacOS and Linux are all supported.

### Target Frameworks

- Lyon
  - .NET 10+
- Roton
  - .NET Standard 2.0

### Build instructions

The [.NET Core SDK](https://github.com/dotnet/core/blob/master/release-notes/download-archive.md)
is required.

Clone the repo:

```
git clone https://github.com/SaxxonPike/roton.git
cd roton
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

Run the game (from source):

```
dotnet run --project Source/Lyon
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
