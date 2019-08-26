Notes on deploying updates

1. Build the application by following the instructions in src\Installer\readme.txt
2. Upload the 3 files in src\Installer\Results\ as a release on github. Tag that release as, e.g. v0.4.8.
3. Update info.xml, so that the URI is the one for src\Installer\Results\[version].zip. Also version needs to be correct (Format: Major.Minor.Build).
4. Upload info.xml to those URIs listed in src\QSP\updater.xml.
