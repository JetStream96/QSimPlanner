const http = require('http')

function handleRequest(request, response) {
    response.end("Hello world !!!!!!")
}

let server = http.createServer(handleRequest)
server.listen(8080, '127.0.0.1', () => {
    console.log('server is running!!!!!')
})


