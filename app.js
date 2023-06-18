const express = require("express");
const { createServer } = require("http");
const { Server } = require("socket.io");

const app = express();
const httpServer = createServer(app);
const io = new Server(httpServer, {});
let debug = false;


const MongoClient = require('mongodb').MongoClient
const uri = "mongodb+srv://omarmayousef:G7IQyLiT1OKcn0Lj@cluster0.shg8nan.mongodb.net/?retryWrites=true&w=majority";




io.on("connection", (socket) => {
  console.log("a user connected");

  socket.on("CreateRoom", async(data) => {
    const rooms = io.of("/").adapter.rooms;
    if (!(rooms.has(data.RoomID)) && !(rooms.has(data.RoomID + "WatchRoom"))) {
      socket.join(data.RoomID)
      socket.join(data.RoomID + "WatchRoom")

      const client = await MongoClient.connect(uri);
      console.log("DB connected");
      const db = client.db('analysis_db');
      const collection = db.collection('analysis_coll');
      const result= await collection.findOneAndUpdate({item:'rooms'},{$inc:{number:1}});
      const resul2t= await collection.findOneAndUpdate({item:'players'},{$inc:{number:1}});
      client.close();
      
      socket.emit('createRoomStatus', { 'status': 'true', 'UserID': socket.id, 'RoomID': data.RoomID })
    }
    else {
      socket.emit('createRoomStatus', { 'status': 'false', 'RoomID': data.RoomID })
    }
  })



  socket.on("joinRoom", async(data) => {
    const rooms = io.of("/").adapter.rooms;

    if (debug == true)
      console.log(rooms)

    if (rooms.has(data.RoomID)) {
      if (data.player == 'true') {
        socket.join(data.RoomID)
        socket.join(data.RoomID + "WatchRoom")

        const client = await MongoClient.connect(uri);
        console.log("DB connected");
        const db = client.db('analysis_db');
        const collection = db.collection('analysis_coll');
        const result= await collection.findOneAndUpdate({item:'players'},{$inc:{number:1}});
        client.close();
        socket.emit('roomStatus', { 'status': 'true', 'RoomID': data.RoomID, 'UserID': socket.id })
      }
      else if (data.player == 'false') {
        socket.join(data.RoomID + "WatchRoom")
        socket.emit('watchStatus', { 'status': 'true', 'RoomID': data.RoomID, 'UserID': socket.id })
      }
    }
    else {
      socket.emit('roomStatus', { 'status': 'false' })
    }
    //socket.join();
  })


  //watch all 
  //room players
  socket.on("refreshplayers", (data) => {
    const rooms = io.of("/").adapter.rooms;
    msg = ''
    rooms.get(data.RoomID).forEach((id) => {
      msg += id + ','
    })
    msg = msg.slice(0, -1)
    io.in(data.RoomID + "WatchRoom").emit('refresh', { 'playerIDs': msg })
    //socket.join();
  })

  socket.on("StartGame", (data) => {
    io.in(data.RoomID + "WatchRoom").emit('GameStarted')
  })

  socket.on("disconnect", () => {
    if (debug == true)
      console.log("user disconnected");
  });

  socket.on("Coord", (data) => {
    socket.to(data.RoomID + "WatchRoom").emit('CoordBroadcast', { 'UserID': data.UserID, 'msg': data.msg })
  })


  socket.on("ChatRoom", (data) => {
    const rooms = io.of("/").adapter.rooms;
    if (rooms.has(data.RoomID + "WatchRoom")) {
      if (rooms.get(data.RoomID + "WatchRoom").has(socket.id)) {
        socket.to(data.RoomID + "WatchRoom").emit('ChatBroadcast', { 'msg': socket.id + " :: " + data.msg })
      }
    }
  })

});


httpServer.listen(3000);