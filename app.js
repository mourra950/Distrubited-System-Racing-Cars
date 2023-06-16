const express = require("express");
const { createServer } = require("http");
const { Server } = require("socket.io");

const app = express();
const httpServer = createServer(app);
const io = new Server(httpServer, {});
let debug = false;
io.on("connection", (socket) => {
  console.log("a user connected");

  socket.on("my message", (msg) => {
    if (debug == true) {
      console.log(msg);
    }
    socket.broadcast.emit('br', msg);
  });
  socket.on("CreateRoom", (data) => {
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

    if (debug == true)
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
    io.in(data.RoomID).emit('refresh', { 'playerIDs': msg })
    //socket.join();
  })

  socket.on("StartGame", (data) => {
    io.in(data.RoomID).emit('GameStarted')
  })

  socket.on("disconnect", () => {
    if (debug == true)
      console.log("user disconnected");
  });

  socket.on("Coord", (data) => {
    socket.to(data.RoomID).emit('CoordBroadcast', { 'UserID': data.UserID, 'msg': data.msg })
  })


  socket.on("ChatRoom", (data) => {
    const rooms = io.of("/").adapter.rooms;
    if (rooms.has(data.RoomID)) {
      if (rooms.get(data.RoomID).has(socket.id)) {
        socket.to(data.RoomID).emit('ChatBroadcast', { 'msg': socket.id + " :: " + data.msg })
      }
    }
  })

});


httpServer.listen(3000);