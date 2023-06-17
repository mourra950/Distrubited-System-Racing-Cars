# imports used during runtime
import socketio
from datetime import datetime
import socket
import threading


# socket io handler
sio = socketio.Client()
# a handler to send data to unity during runtime using low level sockets
sendServer = None

# a handler for unity chat low level socket
unityChatSocket = None

# cache the User ID during game used in excuting certain commands during runtime
UserID = None

# cache the Room ID during game used in excuting certain commands during runtime
RoomID = None


debug = False

# handle event when user connect


@sio.on('connect')
def connect_handler():
    if debug:
        print('Connected!')

# handle event when user try to join a room


@sio.event
def watchStatus(data):
    global RoomID, UserID
# if true return the status and the room id to show it on the lobby
    if data['status'] == 'true':
        RoomID = data['RoomID']
        UserID = data['UserID']
        msg = 'true,'+RoomID+','+UserID
        sendServer.send(msg.encode('utf-8'))
        sio.emit('refreshplayers', {'RoomID': RoomID})
# else return false to prevent joining the room
    else:
        sendServer.send('false,'.encode('utf-8'))
    if debug:
        print(data)


@sio.event
def roomStatus(data):
    global RoomID, UserID
# if true return the status and the room id to show it on the lobby
    if data['status'] == 'true':
        RoomID = data['RoomID']
        UserID = data['UserID']
        msg = 'true,'+RoomID+','+UserID
        sendServer.send(msg.encode('utf-8'))
        sio.emit('refreshplayers', {'RoomID': RoomID})
# else return false to prevent joining the room
    else:
        sendServer.send('false,'.encode('utf-8'))
    if debug:
        print(data)

# event handler to signal to all players when the game has started from the room creator


@sio.event
def GameStarted():
    msg = '/Startgame, '
    sendServer.send(msg.encode('utf-8'))

# event handler to receive all players coordinates during gameplay to update them on unity


@sio.event
def CoordBroadcast(data):
    global sendServer
    msg = '/NCoord,'+data['UserID']+','+data['msg']
    if debug:
        print(msg)
    sendServer.send(msg.encode('utf-8'))

# event handler to refresh the list of players on all machines


@sio.event
def refresh(data):
    global sendServer
    if debug:
        print(data['playerIDs'])
    msg = '/Joined,'+data['playerIDs']
    # Send received data from server to unity
    sendServer.send(msg.encode('utf-8'))

# event handler to get messages during session from all user in the room


@sio.event
def ChatBroadcast(data):
    global unityChatSocket
    if debug:
        print("the data received from sio")
    msg = '/Msg,'+data['msg']
    # Send received data from server to unity
    unityChatSocket.send(msg.encode('utf-8'))


# event handler returns the status when creating a room
@sio.event
def createRoomStatus(data):
    global sendServer, RoomID, UserID
    if data['status'] == 'true':
        msg = "true,"+data['RoomID']+","+data['UserID']
        sendServer.send(msg.encode('utf-8'))
        RoomID = data['RoomID']
        UserID = data['UserID']
    elif data['status'] == 'false':
        msg = "false,"+data['RoomID']
        sendServer.send(msg.encode('utf-8'))


def unityReceive():
    global UserID, RoomID
    try:
        while True:
            ADRESS = "127.0.0.1"
            PORTReceive = 3003
            S = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            S.bind((ADRESS, PORTReceive))
            S.listen(5)
            conn, adress = S.accept()
            sendServer = conn
            if debug:
                print('unity receive connected to python')
            counter = 0
            while True:
                data = conn.recv(1024).decode('utf-8')
                if data:
                    try:
                        func, data = data.split(sep=',', maxsplit=1)

                        if func == "/Coord":
                            sio.emit(
                                'Coord', {
                                    'msg': data,
                                    'RoomID': RoomID,
                                    'UserID': UserID
                                })
                        elif func == '/Create':
                            sio.emit('CreateRoom', {
                                'RoomID': data
                            })
                        elif func == '/Join':
                            if debug:
                                print(data)
                            Room, player = data.split(',')
                            sio.emit('joinRoom', {
                                'RoomID': Room,
                                'player': player
                            })
                        elif func == "/Start":
                            sio.emit('StartGame', {
                                'RoomID': RoomID
                            })

                    except:
                        counter += 1
                        if debug:
                            print(counter)
                        pass
    except:
        if debug:
            print('error')


def unitySend():
    global sendServer
    try:
        while True:
            ADRESS = "127.0.0.1"
            PORTReceive = 3002
            S = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            S.bind((ADRESS, PORTReceive))
            S.listen(5)
            conn, adress = S.accept()
            sendServer = conn
            if debug:
                print('unity send connected to python')
            counter = 0
            # loop to keep the thread active and avoid closing the socket connection
            while True:
                ...
    except:
        pass


def Chat():
    global unityChatSocket
    while True:

        ADRESS = "127.0.0.1"
        PORTReceive = 3004
        S = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        S.bind((ADRESS, PORTReceive))
        S.listen(5)
        conn, adress = S.accept()
        unityChatSocket = conn
        while True:
            data = conn.recv(1024).decode('utf-8')
            func, data = data.split(',', maxsplit=1)
            if debug:
                print(func, data)
            # broadcast message to all servers
            if func == "/Message":
                if debug:
                    print(data)
                sio.emit('ChatRoom', {'RoomID': RoomID, 'msg': data})

            else:
                pass


# Main program entry to start connecting to servers and threading the proccesses
if __name__ == '__main__':

    # options for connecting to the game server either locally or online on AWS/Render
    # https://race-car.onrender.com/
    # http://localhost:3000
    # http://ec2-54-196-9-211.compute-1.amazonaws.com:3000/
    sio.connect('https://race-car.onrender.com/')
    print("connected")
    # thread to receive messages from unity and proccess them
    thread1 = threading.Thread(target=unityReceive)
    # thread to send and proccess messages from the node server to unity endpoint
    thread2 = threading.Thread(target=unitySend)
    # thread dedicated for the chat managing between the client during and before the game
    thread3 = threading.Thread(target=Chat)

    # Start all the threads
    thread1.start()
    thread2.start()
    thread3.start()
