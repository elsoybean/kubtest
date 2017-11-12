var _ = require('underscore'),
    fs = require('fs'),
    amqp = require('amqplib/callback_api'),
    config = JSON.parse(fs.readFileSync('./config.json', 'utf-8')),
    envPath = "./config." + process.env.NODE_ENVIRONMENT + ".json",
    secretPath = "./secret/config.json";

if (fs.existsSync(envPath)) {
    var secretJson = JSON.parse(fs.readFileSync(envPath, 'utf-8'));
    config = _.extend(config, secretJson);
}    

if (fs.existsSync(secretPath)) {
    var secretJson = JSON.parse(fs.readFileSync(secretPath, 'utf-8'));
    config = _.extend(config, secretJson);
}

amqp.connect(config.RabbitMQ.ConnectionString, function(err, conn) {
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
		            	var sql = "INSERT INTO ColorTracker (FooId, Color) VALUES ('" + e.ModelId + "', '" + e.Event.Color + "')";
		                console.log(sql);
            			break;
            		case "Foo.ColorChanged":
		            	var sql = "UPDATE ColorTracker SET Color = '" + e.Event.To + "' WHERE FooId = '" + e.ModelId + "'";
		                console.log(sql);
            			break;
            	}
            }, {noAck: true});
        });
    });
});