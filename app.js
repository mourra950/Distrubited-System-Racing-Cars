const express = require("express");
const { createServer } = require("http");
const { Server } = require("socket.io");

const app = express();
const httpServer = createServer(app);
const io = new Server(httpServer, {});

io.on("connection", (socket) => {
  
  socket.on("my message", (...args) => {
    console.log("Emitted")
  });



});


httpServer.listen(3000);