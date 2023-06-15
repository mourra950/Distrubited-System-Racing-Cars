const express = require("express");
const { createServer } = require("http");
const { Server } = require("socket.io");

const app = express();
const httpServer = createServer(app);
const io = new Server(httpServer, {});
const MongoClient = require('mongodb').MongoClient

const uri = "mongodb+srv://omarmayousef:G7IQyLiT1OKcn0Lj@cluster0.shg8nan.mongodb.net/?retryWrites=true&w=majority";
// Create a MongoClient with a MongoClientOptions object to set the Stable API version
const DBClient = new MongoClient(uri);
DBClient.connect()
console.log("DB connected");
const db = DBClient.db('uuid_db');
const uuid_collection = db.collection('uuid_coll');
const room_collection = db.collection('room_coll');

//collection.findOne({'uuid':uuid});






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
    const rooms = io.of("/").adapter.rooms;

    if (!(rooms.has(data.RoomID))) {
      socket.join(data.RoomID)
      const doc = {
        'RoomID':data.RoomID,
        "UserID": data.UserID
      }
      room_collection.insertOne(doc)
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
      const doc = {
        'RoomID':data.RoomID,
        "UserID": data.UserID
      }
      room_collection.insertOne(doc)
      socket.emit('roomStatus', { 'status': 'true', 'RoomID': data.RoomID, 'UserID': socket.id })
    }
    else {
      socket.emit('roomStatus', { 'status': 'false' })
    }
    //socket.join();
  })

  socket.on("refreshplayers", (data) => {
    const rooms = io.of("/").adapter.rooms;
    const query = { "RoomID": data.RoomID };
    res = room_collection.find(query)
    console.log(res)
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
    //socket.join();
  })

  socket.on("disconnect", () => {
    console.log("user disconnected");
  });

  socket.on("Coord", (data) => {
    socket.to(data.RoomID).emit('CoordBroadcast', { 'UserID': data.UserID, 'msg': data.msg})
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
io.of("/").adapter.on("join-room", (room, id) => {
  console.log(`socket ${id} has joined room ${room}`);
});


httpServer.listen(3000);
