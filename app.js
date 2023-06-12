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

  socket.on("CreateRoom", (data) => {
    console.log(socket.id)
    const rooms = io.of("/").adapter.rooms;

    if (!(rooms.has(data.RoomID))) {
      socket.join(data.RoomID)
      socket.emit('createRoomStatus', { 'status': 'true', 'UserID': socket.id, 'RoomID': data.RoomID })
    }
    else {
      socket.emit('createRoomStatus', { 'status': 'false', 'RoomID': data.RoomID })
    }
  })

  socket.on("joinRoom", (data) => {
    const rooms = io.of("/").adapter.rooms;


    console.log(rooms)

    if (rooms.has(data.RoomID)) {
      socket.join(data.RoomID)
      socket.emit('roomStatus', { 'status': 'true', 'RoomID': data.RoomID, 'UserID': socket.id })
    }
    else {
      socket.emit('roomStatus', { 'status': 'false' })
    }
    //socket.join();
  })

  socket.on("refreshplayers", (data) => {
    const rooms = io.of("/").adapter.rooms;
    msg = ''
    rooms.get(data.RoomID).forEach((id) => {
      msg += id + ','
    })
    msg = msg.slice(0, -1)
    console.log(msg)
    io.in(data.RoomID).emit('refresh', { 'playerIDs': msg })
    //socket.join();
  })

  socket.on("StartGame", (data) => {

    io.in(data.RoomID).emit('GameStarted')
    //socket.join();
  })

  socket.on("disconnect", () => {
    console.log("user disconnected");
  });

  socket.on("Coord", (data) => {
    console.log()
    socket.to(data.RoomID).emit('CoordBroadcast', { 'UserID': data.UserID, 'msg': data.msg})
  })


  socket.on("ChatRoom", (data) => {
    const rooms = io.of("/").adapter.rooms;
    console.log(data)
    console.log(rooms)
    if (rooms.has(data.RoomID)) {
      if (rooms.get(data.RoomID).has(socket.id)) {
        console.log("Chat accessed");
        socket.to(data.RoomID).emit('ChatBroadcast', { 'msg': socket.id + " :: " + data.msg })
      }
    }
  })

});
io.of("/").adapter.on("join-room", (room, id) => {
  console.log(`socket ${id} has joined room ${room}`);
});


httpServer.listen(3000);
