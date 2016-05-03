var chatMessages = [];

var ChatMessages = React.createClass({

    render: function render() {

        var chatMessages = this.props.messages.map(function (chat) {
            return (
              React.createElement("div", { "className": "chatMessage", "key": chat.id },
                React.createElement("div", { "className": "name" }, chat.name),
                React.createElement("div", { "className": "message" }, chat.message),
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