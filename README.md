# Roton

##### Description
Roton is an emulation of the ZZT and Super ZZT game creation engines written in C#. As part of this project, there are also two utlities called Lyon, which is an environment that leverages Roton in order to allow users to play their games, and Torch, which allows users to create and edit game worlds.

##### Technical description
Roton emulates a flat memory block of 64kb for the game engine, not including heap storage used for scripted code. A carefully constructed class heirarchy abstracts details of memory access away from the higher level functions (such as PushThroughTransporter, RemoveActor, etc.) Each of the cores can override most of these functions, making it easy to implement the differences in game engines. The game engines are mostly similar, but there are still several differences in many inconvenient places. Interfaces are provided to separate the main game logic from audio or video output beyond what is required by the game engine itself.

### Why not just spin up DosBox?
There's nothing wrong with using DosBox to emulate ZZT. That is still, in fact, the preferred way to play games to ensure highest compatibility. But like other "vanilla-compatible" source ports of other games and software, additional features not found (or not possible) in the original DOS version will set Roton apart.

### Build instructions

##### All platforms
There are no special instructions, nor is there anything additional you need to install. No external libraries are currently used; all functionality is based off whatever is available from the .NET framework. This may change in the future, especially for non-Windows platforms.

##### Windows (Visual Studio)
As long as you have a development environment that supports .NET Framework 4.0, you should be good to go. The project has been built and tested in the free Visual Studio 2013 Community Edition.

##### Mac OS X and Linux
As long as you have a version of MonoDevelop or Xamarin Studio that supports Visual Studio 2013 projects, NuGet packages, and the .NET Framework 4.0, the process is almost automatic.

The only bit of manual work that will be done in order to run Lyon is to copy the OpenTK.dll file from Roton.OpenGL/bin/Debug (or Release) into the appropriate subdirectory inside of Lyon/bin, as MonoDevelop/Xamarin doesn't appear to properly resolve nested assembly dependencies. This will need to be done every time that the OpenTK library is updated.

The project has been tested in MonoDevelop 5.7.0 under Gentoo Linux (using the dotnet overlay) and Linux Mint 17.1 (using Xamarin's package repository), as well as Xamarin Studio 5.7.0 for Mac OS X. Older versions may work as well.

### Command line arguments
The Lyon frontend currently supports two command line parameters:

- **-opengl**: Forces Lyon to use the OpenGL renderer. This option is the default on Linux and OS X.
- **-winform**: Forces Lyon to use the GDI+ Windows Forms renderer. This option is the default on Windows. Please note that this is not Windows-specific; it will work properly in Mono as well.

### Where can I get more information about ZZT?
Visit http://zzt.org - the original game engines are available for download, as are some games to play on it.

### How can I contribute to Roton?
Feel free to fork the project and add your improvements. Get in touch with the project manager (currently SaxxonPike) once you've finished your work so it can be approved for inclusion in the master branch. Changes to files in the Roton/Emulation directory are not greatly encouraged: if there's something odd in there, it's likely that it's the way the original game engine is coded. All other changes, improvements and additions are appreciated!
