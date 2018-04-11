Follow these steps to build the installer and a portable version.

The application can be run and debugged after simply compiling the project src/QSP/QSP.csproj. However, some features are missing, such as automatic updater, viewing manual and license.

After finishing the section 'Setup the environment' below, you only need to do the single step in 'Build' section every time building the application.

This only works on Windows.

1. Setup the environment

1.1. Install Inno Setup. The installer is in innosetup-5.5.9 folder. Remember to tick "Install Inno Setup Preprocessor".

1.2. Install Inno Download Plugin. The installer is in idpsetup-1.5.0 folder. The destination folder must be the same as the one for Inno Setup. Tick the option "Add IDP include path to ISPPBuiltins.iss".

1.3. Compile InstallerBuilder.csproj in debug mode. The compliled executable should be in InstallerBuilder\bin\x86\Debug. This is important, otherwise the installer may fail to build.

1.4. Open InstallerBuilder\bin\Debug\paths.xml. Edit that file.  InnoSetupDirectory should be the folder Inno Setup installed into, i.e. the folder which contains ISCC.exe. Also, MsBuildExePath should be the path of MSBuild.exe (which needs to be able to compile .NET 4.5).
	
2. Build

Run InstallerBuilder.exe. The installer is src/Installer/Results/QSimPlanner_[version]_setup.exe. 
The portable version is in src/Installer/Output folder. 
The zipped portable version is src/Installer/Results/QSimPlanner_[version]_portable.zip.

If you come across errors like 'access to xxx folder is denied', or 'xxx file is open in another process', make sure no file inside Output folder is being used by any process (including file explorer).

3. Others

For procedures to deploy updates to server, see src/QSP/Updates/readme.txt.
