Follow these steps to build the installer:

1. Install Inno Setup. Download the installer from http://www.jrsoftware.org/ . Remember to tick "Install Inno Setup Preprocessor".
2. Install Inno Download Plugin.  the installer from https://code.google.com/archive/p/inno-download-plugin/ . The destination folder must be the same as the one for Inno Setup. Tick the option "Add IDP include path to ISPPBuiltins.iss".
3. Compile InstallerBuilder.csproj in debug mode. The complilation output should be in InstallerBuilder\bin\Debug.
4. Open InstallerBuilder\bin\Debug\paths.xml. Edit that file.  InnoSetupDirectory should be the folder Inno Setup installed into, i.e. the folder which contains ISCC.exe. Also, MsBuildExePath should be the path of MSBuild.exe (which needs to be able to compile .NET 4.5).
5. Run InstallerBuilder.exe. The installer will appear in Results folder.
