'use strict';

var ChatMessages = React.createClass({

    componentDidMount: function () {
        console.log("ChatMessages_componentDidMount");
        var self = this;
    },

    messageReceived: function (message) {
        console.log("ChatMessages_messageReceived");
        this.setState({ messages: [] });
    },

    render: function render() {

        var chatMessages = this.props.messages.map(function (chat) {
            return (
              React.createElement("div", { "className": "chatMessage", "key": chat.id }, 
                React.createElement("div", { "className": "name" }, chat.name),
                React.createElement("div", { "className": "message" }, chat.message),
                React.createElement("div", { "className": "date" }, chat.date.toString())
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


setInterval(function () {
    ReactDOM.render(
        React.createElement(ChatMessages, { messages: chatMessages }), document.getElementById('0000000'));
}, 1000);