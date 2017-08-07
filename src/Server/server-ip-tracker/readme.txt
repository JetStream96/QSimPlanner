1. Create a folder /tmp.
2. Create a file, ip.txt in that folder.
3. Run the following commands, with <remote> replaced by the value in config.json:
git init
git remote add origin <remote>
git add .
git commit -m "first commit"
git push origin master
node ../ip-updater.js
