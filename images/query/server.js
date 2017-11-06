'use strict';

const express = require('express');

// Constants
const PORT = 80;
const HOST = '0.0.0.0';
const data = {"140ebd4d-b8c4-4113-8850-9e4b20885146": {"color": "red"}};

// App
const app = express();
app.get('/:fooId', (req, res) => {
	if (data[req.params.fooId]) {
		res.send(data[req.params.fooId]);
	} else {
		res.status(404).send("Not found");
	}
});
app.get('/', (req, res) => {
  res.send(data);
});

app.listen(PORT, HOST);
console.log(`Running on http://${HOST}:${PORT}`);