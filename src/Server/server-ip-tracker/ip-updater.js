// This script keeps the ip address on remote git up-to-date.

const fs = require('fs')
const path = require('path')
const mkdirp = require('mkdirp')
const ip = require('ip')
const util = require('../util')
const childProcess = require('child_process')
const configFilePath = path.join(__dirname, 'config.json')
const tmpDir = path.join(__dirname, 'tmp')
const copyFile = require('quickly-copy-file')

function readConfig(config) {
    return JSON.parse(fs.readFileSync(configFilePath, 'utf-8'))
}

/**
 * If a git repo does not exist, creates a git repo in tmp folder and
 * adds the remote origin. Otherwise, update the ip and commit the change.
 * @param {Error => void} callback 
 */
function createOrUpdateRepo(config, callback) {
    copyDir(err => {
        if (err) {
            callback(err)
        } else {
            if (!fs.existsSync(path.join(__dirname, '.git'))) {
                let cmd = 'cd "' + tmpDir + '"\n' +
                    'git init' +
                    'git remote add origin "' + config['remote'] + '"\n'

                childProcess.exec(cmd, () => commitChanges(callback))
            } else {
                commitChanges(callback)
            }
        }
    })
}

function commitChanges(callback) {
    let cmd = 'cd "' + tmpDir + '"\n' +
        'git add .\n' +
        'git commit -m "update"'

    childProcess.exec(cmd, callback)
}

function copyDir(callback) {
    if (fs.existsSync(tmpDir)) {
        fs.mkdirSync(tmpDir)
    }

    // This function overwrites the existing file.
    copyFile(path.join(__dirname, 'ip.txt'), path.join(tmpDir, 'ip.txt'), callback)
}

function updateIp(config) {
    let newIp = ip.address()
    let config = readConfig()
    util.httpsDownloadUrl(config['ip-file'], (ip, err) => {
        if (err) {
            util.log(err.stack)
        } else if (newIp !== ip) {
            util.log('Ip updated to ' + newIp)
            pushUpdate(config, newIp)
        } else {
            util.log('No ip change.')
        }
    })
}

function pushUpdate(config, ip) {
    createRepo(config, e => {
        if (e) {
            util.log(e.stack)
        } else {

        }
    })
}
