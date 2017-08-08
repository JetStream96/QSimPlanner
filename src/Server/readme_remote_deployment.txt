Deployment (tested on raspberry pi):
1. Locally, copy the directory containing this file to another place. Go to the copied directory.
2. Complete step 1 to 3 of readme_local_testing.txt.
3. Go to sub-directory server-ip-tracker. Complete step 1 to 9 of readme_local_testing.txt.
4. Connect via SSH using PuTTY.
5. Make sure the correct node version is installed. Check this via:
/usr/bin/node -v

6. Go to directory of the server:
cd /opt/Server

7. Backup the previous log flies: log.txt and server-ip-tracker/log.txt
8. Delete old version of Server:
rm -rf Server

9. On local machine, copy the copied directory to Server. E.g. on Windows:
pscp -r "C:\Users\Xyz\Desktop\Server" pi@123.45.67.8:'/tmp/Server' 

Use scp instead of pscp on linux. Copying to /tmp is to prevent permission error.

10. Via SSH, move the directory:
mv /tmp/Server /opt/Server

11. Run:
cd /opt/Server/server-ip-tracker/tmp
git push origin master

12. Start the processes in background:
cd ..
nohup node ip-updater.js > /dev/null 2>&1 &
cd ..
nohup node server.js > /dev/null 2>&1 &

13. Make sure the processes are running.
