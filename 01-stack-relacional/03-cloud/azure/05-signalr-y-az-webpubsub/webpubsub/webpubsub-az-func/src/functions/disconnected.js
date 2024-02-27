module.exports = function(context, request) {
    context.bindings.actions = [];
    context.bindings.actions.push({
        "actionName": "sendToAll",
        "data": JSON.stringify({
            from: '[System]',
            content: `${context.bindingData.connectionContext.userId} disconnected.`
        }),
        "dataType": "json"
    });
    context.done();
};