let summary = require('./mini-test').runAll()
console.log(`\n${summary[0]} test(s) passed, ${summary[1]} test(s) failed.`)