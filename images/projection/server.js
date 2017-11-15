const _ = require('underscore'),
    fs = require('fs'),
    amqp = require('amqplib/callback_api'),
    mysql = require('mysql'),
    configPath = './config.json',
    envPath = "./config." + process.env.NODE_ENVIRONMENT + ".json",
    secretPath = "./secrets/config.json";

var config = JSON.parse(fs.readFileSync(configPath));
if (fs.existsSync(envPath)) config = _.extend(config, JSON.parse(fs.readFileSync(envPath)));
if (fs.existsSync(secretPath)) config = _.extend(config, JSON.parse(fs.readFileSync(secretPath)));

const db = mysql.createConnection(config.MySQL.ConnectionString);
db.connect(function(err) {
    if (err) {
        console.error('error connecting to database: ' + err.stack);
        return;
    } else {
        console.log('connected to database');
    }

    amqp.connect(config.RabbitMQ.ConnectionString, function(err, conn) {
        if (err) {
            console.error('error connecting to message bus: ' + err.stack);
            return;
        } else {
            console.log('connected to message bus');
        }

        conn.createChannel(function(err, ch) {
            var ex = config.RabbitMQ.Exchange;
            ch.assertExchange(ex, 'topic', {durable: false});    
            
            ch.assertQueue('ColorTracker', {durable: true}, function(err, q) {
                ch.bindQueue(q.queue, ex, 'Foo.FooCreated.*');
                ch.bindQueue(q.queue, ex, 'Foo.ColorChanged.*');
                ch.consume(q.queue, function(msg) {
                    var e = JSON.parse(msg.content.toString(), 'utf-8');
                    switch (e.EventType) {
                        case "Foo.FooCreated":
                            db.query("INSERT INTO ColorTracker (FooId, Color) VALUES (?,?)", [ e.ModelId, e.Event.Color ]);
                            break;
                        case "Foo.ColorChanged":
                            db.query("UPDATE ColorTracker SET Color = ? WHERE FooId = ?", [ e.Event.To, e.ModelId ]);
                            break;
                        default:
                            console.error("unknown event type: " + e.EventType);
                            break;
                    }
                }, {noAck: true});
            });
        });
    });
});
