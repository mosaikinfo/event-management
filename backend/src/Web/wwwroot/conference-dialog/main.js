(function() {

    class Chat {

        constructor(containerCssSelector) {
            this.container = $(containerCssSelector);
            this.messageTemplate = Handlebars.compile($('#message-template').html());
            this.answerDialogTemplate = Handlebars.compile($("#answer-dialog-template").html());
        }

        addMessage(options) {
            options = options || {};
            options.alignment = options.alignment || 'left';
            options.category = options.category || 'server'

            const html = this.messageTemplate(options);
            this.container.append(html);
        }

        ask(answerOptions) {
            const html = this.answerDialogTemplate({
                answerOptions: answerOptions,
            });
            let $dialog = $(html);
            this.container.append($dialog);
            let promise = new Promise((resolve, reject) => {
                $(".answer", $dialog).click((event) => {
                    let $button = $(event.target);
                    let index = $button.data("index");
                    let option = answerOptions[index];
                    resolve(option);
                    $dialog.remove();
                });
            });
            return promise;
        }
    }

    function main() {
        Handlebars.registerHelper('breaklines', function (text) {
            text = Handlebars.Utils.escapeExpression(text);
            text = text.replace(/(\r\n|\n|\r)/gm, '<br>');
            return new Handlebars.SafeString(text);
        });

        console.log(model);
        const chat = new Chat("#chat-root");

        chat.addMessage({
            content: `${model.firstName} ${model.lastName}\n` +
                `${model.age} Jahre alt`
        });
        chat.addMessage({
            category: 'user',
            alignment: 'right',
            content: 'Ja, das stimmmt!'
        });
        chat.addMessage({
            category: 'success',
            iconCssClass: 'far fa-check-circle',
            content: 'Check-in erfolgreich'
        });
        chat.addMessage({
            category: 'warning',
            iconCssClass: 'far fa-question-circle',
            content: 'Hast du die Einverständniserklärung deiner Eltern dabei?'
        });
        chat.addMessage({
            category: 'danger',
            iconCssClass: 'far fa-check-circle',
            content: 'Leider gibt es ein Problem beim Check-in. Bitte gehe zur Support Line, unsere Mitarbeiter dort helfen dir weiter.'
        });
        chat.ask([
                { value: true, label: 'Ja, das ist richtig.' },
                { value: false, label: 'Nein, das ist falsch.' },
            ])
            .then((selectedOption) => {
                alert(`Decision: ${selectedOption.value}`);
            });
    }
    main();
})();