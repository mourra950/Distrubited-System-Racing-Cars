var express = require('express');
var router = express.Router();

/* GET home page. */
router.get('/', function(req, res, next) {
  console.log(req.body)
  res.send('Hello ' + req.body.name);
});
router.put('/', function(req, res, next) {
  res.status(404).json({put:{test:'hassan'}});
});



module.exports = router;
