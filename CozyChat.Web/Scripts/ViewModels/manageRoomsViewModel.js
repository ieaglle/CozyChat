function ManageRoomsViewModel(baseUrl) {

    var self = this;
    self.chatRooms = ko.observableArray();

    self.activate = function() {
        $.ajax({
            type: 'GET',
            url: baseUrl + 'Home/GetAllChatRooms',
            success: function(content) {
                self.chatRooms(content);
            }
        });
    };

    self.addRoom = function() {
        var name = prompt('Room Name');
        if (name) {
            $.ajax({
                type: 'POST',
                url: baseUrl + 'Home/CreateChatRoom',
                data: { roomName: name },
                success: function(newRoom) {
                    self.chatRooms.push(newRoom);
                }
            });
        }
    };

    self.deleteRoom = function(room) {
        $.ajax({
            type: 'DELETE',
            url: baseUrl + 'Home/DeleteChatRoom',
            data: { roomId: room.Id },
            success: function(succ) {
                if (succ);
                    self.chatRooms.remove(room);
            }
        });
    };

    self.subscribe = function(room) {
        $.ajax({
            type: 'POST',
            url: baseUrl + 'Home/Subscribe',
            data: { roomId: room.Id },
            success: function(succ) {
                if (succ);
                    self.activate();
            }
        });
    };

    self.unSubscribe = function(room) {
        $.ajax({
            type: 'POST',
            url: baseUrl + 'Home/UnSubscribe',
            data: { roomId: room.Id },
            success: function(succ) {
                if (succ);
                    self.activate();
            }
        });
    };
}