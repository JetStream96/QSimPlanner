// This script keeps the ip address on remote git up-to-date.

const fs = require('fs')
const path = require('path')
const ip = require('ip')
const util = require('../util')
const childProcess = require('child_process')
const configFilePath = path.join(__dirname, 'config.json')
const tmpDir = path.join(__dirname, 'tmp')
const publicIp = require('public-ip')
const SyncFileWriter = require('../sync-file-writer').SyncFileWriter

let ipUpdaterLogger = new SyncFileWriter(__dirname, 'log.txt')

function log(msg) {
    ipUpdaterLogger.add(msg)
}

function readConfig(config) {
    return JSON.parse(fs.readFileSync(configFilePath, 'utf-8'))
}

function updateIp(config) {
    publicIp.v4().then(newIp => {
        util.httpsDownload(config['ip-file-host'], config['ip-file-path'], (ip, err) => {
            if (err) {
                log(err.stack)
                return
            }

            if (newIp !== ip) {
                log('Ip updated to ' + newIp)
                pushUpdate(config, newIp)
            } else {
                log('No ip change.')
            }
        })
    }).catch(reason => log(reason))
}

function pushUpdate(config, ip) {
    fs.writeFileSync(path.join(tmpDir, 'ip.txt'), ip)
    execCmds(['cd "' + tmpDir + '"',
        'git add .',
        'git commit -m "update"',
        'git push origin master'])
}

function execCmds(cmds) {
    if (cmds.length === 1) {
        childProcess.exec(cmds[0], e => {
            if (e) log(e.stack)
        })
        return
    }

    childProcess.exec(cmds[0], e => {
        if (e) {
            log(e.stack)
        } else {
            execCmds(cmds.slice(1))
        }
    })
}

util.repeat(() => updateIp(readConfig()), 60 * 1000)
