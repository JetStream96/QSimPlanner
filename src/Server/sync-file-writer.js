const fs = require('fs')
const util = require('./util')
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
        // Since node runs on a single thread, when file writing or directory
        // creation is in process, this.busy is guranteed to be true.
        // Therefore there will not be race conditions.
        // Also, suppose add() is called and this.busy is true, causing its call to
        // _startTask to do nothing, the line 'this.busy = false' is guranteed to be
        // executed afterwards, which means the next line is executed and the writing 
        // will be eventually done.
        
        if (this.pending.length > 0 && !this.busy) {
            this.busy = true
            let p = path.join(this.directory, this.fileName)
            mkdirp(this.directory, e => {
                if (!e) {
                    fs.appendFile(p, this.pending.pop(), e => {
                        if (e) util.log(e.stack)
                    })
                } else {
                    util.log(e)
                }
                this.busy = false
                this._startTask()
            });
        }
    }
}

exports.SyncFileWriter = SyncFileWriter
