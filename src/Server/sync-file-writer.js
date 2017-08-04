const fs = require('fs')
const server = require('./server')
const path = require('path')
const mkdirp = require('mkdirp')

class SyncFileWriter {
    constructor(directory, fileName) {
        this.pending = []
        this.directory = directory
        this.fileName = fileName
        this.busy = false
    }

    add(content) {
        this.pending.push(content)
        this._startTask()
    }

    _startTask() {
        if (this.pending.length > 0 && !this.busy) {
            this.busy = true
            let p = path.join(this.directory, this.fileName)
            mkdirp(this.directory, e => {
                if (!e) {
                    fs.appendFile(p, this.pending.pop(), e => {
                        if (e) {
                            server.log(e.stack)
                        }
                    })
                } else {
                    server.log(e)
                }
                this.busy = false
                this._startTask()
            });
        }
    }
}

exports.SyncFileWriter = SyncFileWriter
