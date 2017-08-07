// This script keeps the ip address on remote git up-to-date.

const fs = require('fs')
const path = require('path')
const ip = require('ip')
const util = require('../util')
const childProcess = require('child_process')
const configFilePath = path.join(__dirname, 'config.json')
const tmpDir = path.join(__dirname, 'tmp')

function readConfig(config) {
    return JSON.parse(fs.readFileSync(configFilePath, 'utf-8'))
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
    fs.writeFileSync(path.join(tmpDir, 'ip.txt'), ip)
    let cmd = 'cd "' + tmpDir + '"\n' +
        'git add .\n' +
        'git commit -m "update"\n' +
        'git push origin master'

    childProcess.exec(cmd, () => {
        if (e) {
            util.log(e.stack)
        }
    })
}

util.repeat(() => updateIp(readConfig()), 60 * 1000)
