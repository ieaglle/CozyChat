function RoomsViewModel(baseUrl, userId, proxy) {

    var self = this;
    //list of all subscribed chat rooms
    self.chatRooms = ko.observableArray();
    //list of online users in selected chat room
    self.currentRoomUsers = ko.observableArray();
    //list of messages in selected chat room
    self.currentRoomMessages = ko.observableArray();
    //selected room
    self.selectedRoom = ko.observable();
    //message that user is about to submit
    self.newMessage = ko.observable();

    //property which is used to colour selected chat room
    self.roomClassName = function (id) {
        if (self.selectedRoom() && self.selectedRoom().Id == id)
            return 'list-group-item active';
        return 'list-group-item';
    };

    //getting list of subsctibed chat rooms
    self.activate = function () {
        $.ajax({
            type: 'GET',
            url: baseUrl + 'Home/GetSubscribedRooms',
            dataType: "json",
            success: function (content) {
                self.chatRooms(content);
            }
        });
    };

    self.selectRoom = function (room) {
        $.ajax({
            type: 'GET',
            url: baseUrl + 'Home/GetMessagesForRoom',
            data: { userId: userId, roomId: room.Id },
            dataType: "json",
            success: function (content) {
                self.currentRoomMessages(content);

                if (!self.selectedRoom()) {
                    self.selectedRoom(room);
                    proxy.server.join(self.selectedRoom());
                }
                if (self.selectedRoom() && self.selectedRoom().Id != room.Id) {
                    proxy.server.leave(self.selectedRoom());
                    self.selectedRoom(room);
                    proxy.server.join(self.selectedRoom());
                };
                var elem = $('#msgs');
                elem[0].scrollTop = elem[0].scrollHeight;
            }
        });
    };

    self.addNewMessage = function () {
        proxy.server.sendToGroup(self.selectedRoom(), self.newMessage());
        self.newMessage(null);
    };
}