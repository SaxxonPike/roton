# Roton

[![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)](https://raw.githubusercontent.com/SaxxonPike/roton/master/LICENSE)

##### Description

Roton is an emulation of the ZZT and Super ZZT game creation engines written in C#.

### Build instructions

##### Windows (Visual Studio)

Any modern version of Visual Studio or Jetbrains Rider can be used to build everything. There are no special
instructions; just build and go.

##### Mac OS X and Linux

*This information may be out of date. Gotta nudge Spectere to fill this out.*

As long as you have a version of MonoDevelop or Xamarin Studio that supports Visual Studio 2013 projects, NuGet
packages, and the .NET Framework 4.0, the process is almost automatic.

The only bit of manual work that will be done in order to run Lyon is to copy the OpenTK.dll file from
Roton.OpenGL/bin/Debug (or Release) into the appropriate subdirectory inside of Lyon/bin, as MonoDevelop/Xamarin doesn't
appear to properly resolve nested assembly dependencies. This will need to be done every time that the OpenTK library
is updated.

The project has been tested in MonoDevelop 5.7.0 under Gentoo Linux (using the dotnet overlay) and Linux Mint 17.1
(using Xamarin's package repository), as well as Xamarin Studio 5.7.0 for Mac OS X. Older versions may work as well.

### Where can I learn more about ZZT?

Visit http://zzt.org - the original game engines are available for download, as are some games to play on it.

### How can I contribute to Roton?

Feel free to fork the project and add your improvements. Get in touch with the project manager (currently SaxxonPike)
once you've finished your work so it can be approved for inclusion in the master branch.