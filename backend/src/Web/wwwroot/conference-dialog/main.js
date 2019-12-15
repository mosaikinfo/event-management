(function main() {
    const $chatfield = $(".chat-panel");
    const messageTemplate = Handlebars.compile($('#message-template').html());

    Handlebars.registerHelper('breaklines', function (text) {
        text = Handlebars.Utils.escapeExpression(text);
        text = text.replace(/(\r\n|\n|\r)/gm, '<br>');
        return new Handlebars.SafeString(text);
    });

    function addMessage(args) {
        const html = messageTemplate({
            content: args.content
        });
        $chatfield.append(html);
    }

    console.log(model);
    addMessage({
        type: 'server',
        content: `${model.firstName} ${model.lastName}\n` +
            `${model.age} Jahre alt`
    });
})();