const express = require("express");
const { createServer } = require("http");
const { Server } = require("socket.io");

const app = express();
const httpServer = createServer(app);
const io = new Server(httpServer, {});

io.on("connection", (socket) => {
  console.log("a user connected");
  socket.emit('test',{'msg':"I'm here"})
  socket.on("my message", (msg) => {
    console.log(msg);
    socket.broadcast.emit('br',msg);
  });
  socket.on("disconnect", () => {
    console.log("user disconnected");
  });
});

app.get('/', (req, res) => {
  res.send('Hello World!')
})
app.listen(3001, () => {
  console.log(`Example app listening on port ${port}`)
})
httpServer.listen(3000);
