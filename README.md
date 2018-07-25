# Roton

[![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)](https://raw.githubusercontent.com/SaxxonPike/roton/master/LICENSE)

##### Description

Roton is an emulation of the ZZT and Super ZZT game creation engines written in C#.

### Build instructions

##### Windows (Visual Studio)

Any modern version of Visual Studio or Jetbrains Rider can be used to build everything. There are no special
instructions; just build and go.

##### macOS and Linux

In order to compile Roton on Linux or macOS, the only thing you need to install is the
[.NET Core SDK v2.0](https://github.com/dotnet/core/blob/master/release-notes/download-archives/2.1.2-download.md)
(newer versions should also work). After cloning the repository, enter the directory with Roton.sln and type
`dotnet build`. The .NET Core SDK will then retrieve all of the packages that it needs to build Lyon and Roton.
After the build process completes, type `dotnet run --project Lyon` to run the newly built project.

IDEs that support .NET Core, such as JetBrains Rider or Visual Studio Code, should be able to manage the build
process as long as the SDK is installed.

### Where can I learn more about ZZT?

Visit http://zzt.org - the original game engines are available for download, as are some games to play on it.

### How can I contribute to Roton?

Feel free to fork the project and add your improvements. Get in touch with the project manager (currently SaxxonPike)
once you've finished your work so it can be approved for inclusion in the master branch.
