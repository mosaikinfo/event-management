(function () {
    const TEXT_SUPPORT_LINE = 'Bitte gehe zur Support-Line. Unsere Mitarbeiter dort helfen dir weiter.';

    const ticket = window.model;
    if (!ticket) {
        throw "Ticket data is missing.";
    }

    class Chat {
        constructor(containerCssSelector) {
            this.container = $(containerCssSelector);
            this.messageTemplate = Handlebars.compile($('#message-template').html());
            this.answerDialogTemplate = Handlebars.compile($("#answer-dialog-template").html());
        }

        addMessage(options) {
            options = options || {};
            options.alignment = options.alignment || 'left';
            options.category = options.category || 'server';

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

        showSupportLineMessage(reason) {
            this.addMessage({
                category: 'danger',
                iconCssClass: 'far fa-times-circle',
                content: reason ? `${reason}. ${TEXT_SUPPORT_LINE}` : TEXT_SUPPORT_LINE
            });
        }
    }

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

    function postJson(url, data) {
        return http(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
    }

    async function goToSupportLine(reason) {
        await await postJson('/checkin/failed', {
            ticketId: ticket.ticketId,
            reason: reason
        });
        chat.showSupportLineMessage(reason);
    }

    async function main() {
        Handlebars.registerHelper('breaklines', function (text) {
            text = Handlebars.Utils.escapeExpression(text);
            text = text.replace(/(\r\n|\n|\r)/gm, '<br>');
            return new Handlebars.SafeString(text);
        });

        console.log(ticket);

        if (ticket.validated) {
            goToSupportLine('Das Ticket wurde bereits verwendet');
            return;
        }

        ticket.firstName = ticket.firstName || '';
        ticket.lastName = ticket.lastName || '';
        let name = `${ticket.firstName} ${ticket.lastName}`;
        name = name.trim();
        name = name || 'Name unbekannt';
        let age = ticket.age;
        age = age ? `${ticket.age} Jahre alt` : 'Alter unbekannt';
        chat.addMessage({ content: `${name}\n${age}` });

        if (ticket.age < 18) {
            if (ticket.termsAccepted) {
                chat.addMessage({
                    content: 'Die Einverständniserklärung der Eltern wurde bereits abgegeben.'
                });
            } else {
                chat.addMessage({
                    category: 'warning',
                    iconCssClass: 'far fa-question-circle',
                    content: 'Hast du die Einverständniserklärung deiner Eltern dabei?'
                });

                const answer = await chat.ask([
                    { value: true, label: 'Ja, habe ich' },
                    { value: false, label: 'Nein, habe ich nicht' }
                ]);

                if (answer.value) {
                    chat.addMessage({
                        category: 'warning',
                        iconCssClass: 'fas fa-hand-holding',
                        content: 'Nimm die Einverständniserklärung entgegen.'
                    });
                    await chat.ask([{ label: 'Erledigt' }]);
                    await postJson(
                        '/checkin/setTermsAccepted', { ticketId: ticket.ticketId });
                } else {
                    goToSupportLine("Die Einverständniserklärung der Eltern fehlt");
                    return;
                }
            }
        }

        const roomNumber = ticket.roomNumber || 'unbekannt';
        chat.addMessage({
            content:
                `Ticket-Typ: ${ticket.ticketTypeName}\n` +
                `Zimmernummer: ${roomNumber}`
        });

        if (ticket.paymentStatus !== 'Paid') {
            goToSupportLine("Das Ticket ist noch nicht vollständig bezahlt");
            return;
        }

        await postJson(
            '/checkin/complete', { ticketId: ticket.ticketId });

        chat.addMessage({
            category: 'success',
            iconCssClass: 'far fa-check-circle',
            content: 'Check-in erfolgreich'
        });
    }
    main();
})();