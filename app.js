const express = require("express");
const { createServer } = require("http");
const { Server } = require("socket.io");

const app = express();
const httpServer = createServer(app);
const io = new Server(httpServer, {});

io.on("connection", (socket) => {

  console.log("a user connected");
  socket.on("my message", (msg) => {
    console.log(msg);
    socket.broadcast.emit('br', msg);
  });
  socket.on("testunity", (msg) => {
    console.log(msg);
  });

  socket.on("CreateRoom", () => {
    console.log(socket.id)
    socket.join(socket.id)
    socket.emit('createRoomStatus', { ID: socket.id })
  })

  socket.on("joinRoom", (data) => {
    const rooms = io.of("/").adapter.rooms;

    console.log(rooms.has(data.roomID))
    console.log(rooms.get(data.roomID).size)

    if (rooms.has(data.roomID)) {
      socket.join(data.roomID)
      console.log(rooms)
      socket.emit('roomStatus', { status: 'true' })

    }
    else {
      socket.emit('roomStatus', { status: 'false' })
    }
    //socket.join();
  })

  socket.on("disconnect", () => {
    console.log("user disconnected");
  });




  socket.on("Chat", (data) => {
    const rooms = io.of("/").adapter.rooms;
    console.log(socket.id)

    console.log(rooms.has(data.RoomID))
    socket.join(data.RoomID)
    console.log("Chat accessed");
    socket.broadcast.to(data.RoomID).emit('ChatBroadcast', { 'msg': data.msg })
  })

});



httpServer.listen(3000);