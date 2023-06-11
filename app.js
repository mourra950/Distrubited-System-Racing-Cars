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
      socket.emit('createRoomStatus', { 'status': 'true', 'ID': data.RoomID })
    }
    else {
      socket.emit('createRoomStatus', { 'status': 'false', 'ID': data.RoomID })
    }
  })

  socket.on("joinRoom", (data) => {
    const rooms = io.of("/").adapter.rooms;

    console.log(rooms)

    if (rooms.has(data.RoomID)) {
      socket.join(data.RoomID)
      socket.emit('roomStatus', { 'status': 'true', 'RoomID': data.RoomID, 'userID': socket.id })
      msg = ''
      rooms.get(data.RoomID).map((id) => {
        msg += id + ','
      })
      console.log(msg)
      io.in(data.RoomID).emit('playerjoined', { 'playerIDs': msg })

    }
    else {
      socket.emit('roomStatus', { 'status': 'false' })
    }
    //socket.join();
  })

  socket.on("disconnect", () => {
    console.log("user disconnected");
  });

  socket.on("Coord", (data) => {
    socket.to(data.RoomID).emit('CoordBroadcast', { 'carname': socket.id, 'Coord': data.data })
  })


  socket.on("ChatRoom", (data) => {
    const rooms = io.of("/").adapter.rooms;
    console.log(data)
    console.log(rooms)
    console.log(rooms.get(data.RoomID).has(socket.id))
    console.log(data.RoomID)
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
