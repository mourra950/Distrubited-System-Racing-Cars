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

    console.log(rooms)

    if (rooms.has(data.RoomID)) {
      socket.join(data.RoomID)
      socket.emit('roomStatus', { 'status': 'true', 'RoomID': data.RoomID })
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
    console.log(rooms)
    if (rooms.has(data.RoomID)) {
      if (rooms.get(data.RoomID).has(socket.id)) {
        console.log("Chat accessed");
        socket.broadcast.to(data.RoomID).emit('ChatBroadcast', { 'msg': data.msg })
      }
    }
  })

});
io.of("/").adapter.on("join-room", (room, id) => {
  console.log(`socket ${id} has joined room ${room}`);
});


httpServer.listen(3000);
