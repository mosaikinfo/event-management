(function () {
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
        iconCssClass: 'far fa-times-circle',
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

    class CheckInDialog extends Chat {
        constructor(containerCssSelector) {
            super(containerCssSelector);
        }

        showTechnicalIssue() {
            this.addMessage({
                category: 'danger',
                iconCssClass: 'far fa-times-circle',
                content: 'Leider gibt es ein technisches Problem. Bitte gehe zur Support-Line. Unsere Mitarbeiter dort helfen dir weiter.'
            });
        }

        showConnectionIssues() {
            this.addMessage({
                category: 'danger',
                iconCssClass: 'far fa-times-circle',
                content: 'Verbindungsprobleme. Bitte überprüfe deine Internetverbindung.'
            });
        }

    }

    const API_BASE_URL = '/api';
    const chat = new CheckInDialog("#chat-root");

    /**
     * Improved secure implementation of the fetch() method.
     * 
     * The standard behavior of the fetch() method is that it doesn't fail,
     * when a http status code 4xx or 5xx is returned from the server.
     * This method returns a Promise that will perform the http request
     * with the fetch() method. It will reject the Promise, when a HTTP
     * status code is returned that indiciates a failure.
     * Furthermore it does some additional error handling.
     */
    function http(input, init) {
        return new Promise((resolve, reject) => {
            fetch(input, init).then(response => {
                // response only can be ok in range of 2XX
                if (response.ok) {
                    // you can call response.json() here too if you want to return json
                    resolve(response);
                } else {
                    chat.showTechnicalIssue();
                    reject(response);
                }
            })
                .catch(error => {
                    // it will be invoked mostly for network errors
                    console.log(error);
                    chat.showConnectionIssues();
                    reject(error);
                });
        });
    }

    async function main() {
        Handlebars.registerHelper('breaklines', function (text) {
            text = Handlebars.Utils.escapeExpression(text);
            text = text.replace(/(\r\n|\n|\r)/gm, '<br>');
            return new Handlebars.SafeString(text);
        });

        console.log(model);

        const response = await http(API_BASE_URL + '/events');
        const json = await response.json();
        console.log(json);

        chat.addMessage({
            content: `${model.firstName} ${model.lastName}\n` +
                `${model.age} Jahre alt`
        });

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
                chat.showTechnicalIssue();
            }
        }, 1000)
    }
    main();
})();