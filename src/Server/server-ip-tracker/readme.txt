1. Setup a remote git repository, if you haven't.
2. If the remote repo is new and empty, go to step 3. Otherwise, go to step 6.
3. Create a folder /tmp.
4. Create a file, ip.txt in that folder.
5. Run: 
git init

6. If the remote origin has not been added, Run the following commands, with <remote> replaced by the value in config.json:
git remote add origin <remote>

7. Run:
git pull origin master
git add .
git commit -m "update"
git push origin master
node ../ip-updater.js
