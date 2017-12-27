Follow these steps to build the installer:

1. Install Inno Setup. The installer is in innosetup-5.5.9 folder. Remember to tick "Install Inno Setup Preprocessor".
2. Install Inno Download Plugin. The installer is in idpsetup-1.5.0 folder. The destination folder must be the same as the one for Inno Setup. Tick the option "Add IDP include path to ISPPBuiltins.iss".
3. Compile InstallerBuilder.csproj in debug mode. The complilation output should be in InstallerBuilder\bin\Debug.
4. Open InstallerBuilder\bin\Debug\paths.xml. Edit that file.  InnoSetupDirectory should be the folder Inno Setup installed into, i.e. the folder which contains ISCC.exe. Also, MsBuildExePath should be the path of MSBuild.exe (which needs to be able to compile .NET 4.5).
5. Run InstallerBuilder.exe. The installer will appear in Results folder. If you come across errors like 'access to xxx folder is denied', or 'xxx file is open in another process', make sure no file inside Output folder is being used by any process (including file explorer).
6. For procedures to deploy updates, see src\QSP\Updates\readme.txt.