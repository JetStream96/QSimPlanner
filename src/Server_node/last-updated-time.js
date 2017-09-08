const path = require('path')
const fs = require('fs')
const util = require('./util')
const westFile = path.join(__dirname, 'saved/nats/west/last-updated-time.txt')
const eastFile = path.join(__dirname, 'saved/nats/east/last-updated-time.txt')
const mkdirp = require('mkdirp')

/**
 * @NoThrow
 * If either direction failed to load from file, returns 0 for time.
 */
function tryLoadFromFile() {
    try {
        return loadFromFile()
    } catch (e) {
        return [0, 0]
    }
}

/**
 * @Throws
 * Returns last updated time in ms, in the form of [west, east].
 */
function loadFromFile() {
    return [parseInt(fs.readFileSync(westFile, 'utf-8')),
    parseInt(fs.readFileSync(eastFile, 'utf-8'))]
}

// @NoThrow
// callback: err => void
function saveFile(fileName, time, callback) {
    mkdirp(path.dirname(fileName), (e, _) => {
        if (e) {
            util.log(e.stack)
        }

        fs.writeFile(fileName, time, callback)
    })
}

// @NoThrow
// callback: err => void
function saveFileWest(time, callback) {
    saveFile(westFile, time, callback)
}

// @NoThrow
// callback: err => void
function saveFileEast(time, callback) {
    saveFile(eastFile, time, callback)
}

exports.tryLoadFromFile = tryLoadFromFile
exports.saveFileWest = saveFileWest
exports.saveFileEast = saveFileEast
