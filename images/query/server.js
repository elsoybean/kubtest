'use strict';

const _ = require('underscore'),
    fs = require('fs'),
    express = require('express'),
    mysql = require('mysql'),
    configPath = './config.json',
    envPath = "./config." + process.env.NODE_ENVIRONMENT + ".json",
    secretPath = "./secrets/config.json";

// Constants
const PORT = 80;
const HOST = '0.0.0.0';

// App
var config = JSON.parse(fs.readFileSync(configPath));
if (fs.existsSync(envPath)) config = _.extend(config, JSON.parse(fs.readFileSync(envPath)));
if (fs.existsSync(secretPath)) config = _.extend(config, JSON.parse(fs.readFileSync(secretPath)));

const db = mysql.createConnection(config.MySQL.ConnectionString);
db.connect((err) => {
    if (err) {
        console.error('error connecting to database: ' + err.stack);
        return;
    } else {
        console.log('connected to database');
    }

	const app = express();
	app.get('/:fooId', (req, res) => {
		db.query("SELECT Color FROM ColorTracker WHERE FooId = ?", [req.params.fooId], (error, results) => {
			if (error) res.status(500).send(error);
			if (results.length == 0) res.status(404).send("Not found");
			else res.send({color: results[0].Color });			
		});
	});

	app.get('/', (req, res) => {
		db.query("SELECT FooId, Color FROM ColorTracker", [], (error, results) => {
			if (error) { res.status(500).send(error); }
			else {
				var data = {};
				_.each(results, (r) => { data[r.FooId] = { color: r.Color }; });
				res.send(data);
			}
		});
	});

	app.listen(PORT, HOST);
	console.log(`Running on http://${HOST}:${PORT}`);
});