Notes on deploying updates

1. Make sure src\QSP\updater.xml contains the correct files
2. Build the application by following the instructions in src\Installer\readme.txt
3. Upload src\Installer\Results\[version].zip. 
4. Update info.xml, so that the uri is the one of the zip file. Also version needs to be correct (Format: Major.Minor.Build).
5. Upload info.xml to those uris listed in src\QSP\updater.xml.
