(function () {
    const API_BASE_URL = '/api';

    /** Samples:
      
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
    const selectedOption = await chat.ask([
        { value: true, label: 'Ja, habe ich.' },
        { value: false, label: 'Nein' },
    ]);
    console.log(`Decision: ${selectedOption.value}`);
    */
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
            const $dialog = $(html);
            this.container.append($dialog);
            const promise = new Promise((resolve, reject) => {
                $(".answer", $dialog).click((event) => {
                    const $button = $(event.target);
                    const index = $button.data("index");
                    const option = answerOptions[index];
                    this.addMessage({
                        category: 'user',
                        alignment: 'right',
                        content: option.label
                    });
                    resolve(option);
                    $dialog.remove();
                });
            });
            return promise;
        }
    }

    async function main() {
        Handlebars.registerHelper('breaklines', function (text) {
            text = Handlebars.Utils.escapeExpression(text);
            text = text.replace(/(\r\n|\n|\r)/gm, '<br>');
            return new Handlebars.SafeString(text);
        });

        console.log(model);
        const chat = new Chat("#chat-root");

        const response = await fetch(API_BASE_URL + '/events');
        const json = await response.json();
        console.log(json);

        chat.addMessage({
            category: 'warning',
            iconCssClass: 'far fa-question-circle',
            content: 'Hast du die Einverständniserklärung deiner Eltern dabei?'
        });

        const selectedOption = await chat.ask([
            { value: true, label: 'Ja, habe ich.' },
            { value: false, label: 'Nein' },
        ]);
        console.log(selectedOption);

        setTimeout(function () {
            if (selectedOption.value) {
                chat.addMessage({
                    category: 'success',
                    iconCssClass: 'far fa-check-circle',
                    content: 'Check-in erfolgreich'
                });
            } else {
                chat.addMessage({
                    category: 'danger',
                    iconCssClass: 'far fa-check-circle',
                    content: 'Leider gibt es ein Problem beim Check-in. Bitte gehe zur Support Line, unsere Mitarbeiter dort helfen dir weiter.'
                });
            }
        }, 1000)
    }
    main();
})();