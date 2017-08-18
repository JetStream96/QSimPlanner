<Another file ../readme_remote_deployment depends on this file. Update that file in case steps in this file is changed.>

1. Make sure config.json is set.
2. Delete log.txt if exists.
3. Setup a remote git repository, if you haven't.
4. If the remote repo is new and empty, go to step 5. Otherwise, go to step 8.
5. Create a folder /tmp.
6. Create a file, ip.txt in that folder.
7. Run: 
git init

8. If the remote origin has not been added, Run the following commands, with <remote> replaced by the value in config.json:
git remote add origin <remote>

9. Run:
git pull origin master
git add .
git commit -m "update"

10. Run:
git push origin master
node ../ip-updater.js