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
    socket.emit('createRoomStatus', { 'ID': socket.id })
  })

  socket.on("joinRoom", (data) => {
    const rooms = io.of("/").adapter.rooms;

    console.log(rooms.has(data.roomID))

    if (rooms.has(data.roomID)) {
      socket.join(data.roomID)
      socket.emit('roomStatus', { 'status': 'true', 'RoomID': data.roomID })
    }
    else {
      socket.emit('roomStatus', { 'status': 'false' })
    }
    //socket.join();
  })

  socket.on("disconnect", () => {
    console.log("user disconnected");
  });




  socket.on("ChatRoom", (data) => {
    const rooms = io.of("/").adapter.rooms;
    console.log(data)
    socket.join(data.RoomID)
    console.log()
    if (rooms.has(data.RoomID)) {
      if (rooms.get(data.RoomID).has(socket.id)) {

        console.log("Chat accessed");
        socket.broadcast.to(data.RoomID).emit('ChatBroadcast', { 'msg': data.msg })

      }
    }
  })

});



httpServer.listen(3000);
