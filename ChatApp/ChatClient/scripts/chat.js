var chatMessages = [];

var ChatMessages = React.createClass({

    render: function render() {

        var chatMessages = this.props.messages.map(function (chat) {
            return (
              React.createElement("div", { "className": "chatMessage", "key": chat.id },
                React.createElement("div", { "className": "name" }, chat.name),
                React.createElement("div", { "className": "message" }, htmlDecode(chat.message)),
                React.createElement("div", { "className": "date" }, chat.date)
                )
              );
        });

        return React.createElement(
            'div',
            null,
            chatMessages
        );
    }
});

function htmlDecode(html) {
    var decodedString = $.parseHTML(html)[0].nodeValue;
    return $.parseHTML(decodedString)[0].nodeValue;
}