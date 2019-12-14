const $chatfield = $("#chatfield");
const messageServerTemplate = Handlebars.compile($('#message-server-template').html());

Handlebars.registerHelper('breaklines', function (text) {
    text = Handlebars.Utils.escapeExpression(text);
    text = text.replace(/(\r\n|\n|\r)/gm, '<br>');
    return new Handlebars.SafeString(text);
});

function addMessage(args) {
    const html = messageServerTemplate(
        {
            content: args.content
        });
    $chatfield.append(html);
}

function main() {
    console.log(model);
    addMessage({
        type: 'server',
        content: `${model.firstName} ${model.lastName}\n` +
            `${model.age} Jahre`
    });
}

main();