const util = require('./util')
const defaultTokenCount = 120
const tokensPerHour = 120

class AntiSpamList {
    constructor() {
        this.users = {}
        util.repeat(() => this._incrementToken(), Math.floor(60 * 60 * 1000 / tokensPerHour))
    }

    _incrementToken() {
        let u = this.users
        Object.keys(u).forEach(k => {
            if (u[k]++ >= defaultTokenCount) {
                delete u[k]
            }
        })
    }

    /**
     * @param {string} ip 
     * @returns {boolean} Whether the ip is spammer.
     */
    decrementToken(ip) {
        let u = this.users
        let count = u[ip]
        if (count === undefined) {
            u[ip] = defaultTokenCount
        }

        if (count === 0) {
            return true
        } else {
            u[ip]--
            return false
        }
    }

    isSpammer(ip) {
        return this.users[ip] === 0
    }
}

exports.AntiSpamList = AntiSpamList
