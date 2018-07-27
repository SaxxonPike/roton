# Roton (with Lyon frontend)

[![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)](https://raw.githubusercontent.com/SaxxonPike/roton/master/LICENSE)

##### Description

Roton is an emulation of the ZZT and Super ZZT game creation engines written in C#. Lyon is the
executable frontend that presents the emulation via SDL.

### Build instructions

All platforms need the 
[.NET Core SDK](https://github.com/dotnet/core/blob/master/release-notes/download-archive.md).
We developed with 2.0, but newer versions of v2.x should work fine.

##### Windows

Clone the repository. Modern versions of 
[Visual Studio](https://visualstudio.microsoft.com/) 
and 
[Jetbrains Rider](https://www.jetbrains.com/rider/) 
should work. Restore packages, build and run as usual.

It can also be built and run from the command line. While in the directory where Roton.sln exists, run
`dotnet build` to resolve dependencies and build the project, and `dotnet run --project Lyon` to start it.
No IDE necessary if you do it this way, just make sure you have the .NET Core SDK linked above.

##### macOS and Linux

After cloning the repository, enter the directory with Roton.sln and type
`dotnet build`. The .NET Core SDK will then retrieve all of the packages that it needs to build Lyon and Roton.
After the build process completes, type `dotnet run --project Lyon` to run the newly built project.

IDEs that support .NET Core, such as JetBrains Rider or Visual Studio Code, should be able to manage the build
process as long as the SDK is installed.

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


