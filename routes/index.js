const express = require("express");
const { createServer } = require("http");
const { Server } = require("socket.io");

const app = express();
const httpServer = createServer(app);
const io = new Server(httpServer, {});

io.on("connection", (socket) => {
  console.log("someone connected")
});

httpServer.listen(3000);




/*
var router = express.Router();
const { MongoClient, ServerApiVersion } = require('mongodb');

import { createServer } from "http";
import { Server } from "socket.io";

const httpServer = createServer();
const io = new Server(httpServer, {
  // options
});

io.on("connection", (socket) => {
  // ...
});

httpServer.listen(3000);
*/

/*
const uri = "mongodb+srv://omarmayousef:G7IQyLiT1OKcn0Lj@cluster0.shg8nan.mongodb.net/?retryWrites=true&w=majority";
// Create a MongoClient with a MongoClientOptions object to set the Stable API version
const client = new MongoClient(uri, {
  serverApi: {
    version: ServerApiVersion.v1,
    strict: true,
    deprecationErrors: true,
  }
});
*/


/* GET home page. */
/*
router.get('/', function (req, res, next) {

  client.connect()
  console.log("connected");
  const db = client.db('testdb');
  const collection = db.collection('testcollection');
  collection.insertOne({ 'name': req.body.name })
  res.send('Hello ' + req.body.name);

});

router.post('/', function (req, res, next) {
  console.log(req.body)
  res.send('Hello ' + req.body.name);
});

router.get('/test', function (req, res, next) {
  res.status(404).json({ put: { test: 'hassan' } });
});


*/
module.exports = router;
