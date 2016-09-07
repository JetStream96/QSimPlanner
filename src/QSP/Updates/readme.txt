Notes on deploying updates

1. Make sure src\QSP\updater.xml contains the correct files
2. Build the application by following the instructions in src\Installer\readme.txt
3. Take all files in src\Installer\Output\[version]. Zip all contents in to a single file. Make sure the top level of the zip file contains QSimPlanner.exe instead of a folder [version].
4. Upload the zip file. Then upload it. 
5. Update info.xml, so that the uri is the one of the zip file. Also version needs to be correct (Format: Major.Minor.Build).
6. Upload info.xml to some uri listed in src\QSP\updater.xml.