import socketio
from datetime import datetime
import socket
import threading
import keyboard

# standard Python
sio = socketio.Client()
sendServer = None
UserID = None
RoomID = None
unityChatSocket = None


@sio.on('connect')
def connect_handler():
    print('Connected!')


@sio.event
def roomStatus(data):
    global RoomID, UserID
    if data['status'] == 'true':
        RoomID = data['RoomID']
        UserID = data['UserID']
        msg = 'true,'+RoomID+','+UserID
        sendServer.send(msg.encode('utf-8'))
        sio.emit('refreshplayers', {'RoomID': RoomID})

    else:
        sendServer.send('false,'.encode('utf-8'))
    print(data)


@sio.event
def GameStarted():
    msg = '/Startgame, '
    sendServer.send(msg.encode('utf-8'))


@sio.event
def CoordBroadcast(data):
    global sendServer
    msg = '/NCoord,'+data['UserID']+','+data['msg']
    sendServer.send(msg.encode('utf-8'))


@sio.event
def refresh(data):
    global sendServer
    print(data['playerIDs'])
    msg = '/Joined,'+data['playerIDs']
    # Send received data from server to unity
    sendServer.send(msg.encode('utf-8'))


@sio.event
def ChatBroadcast(data):
    global unityChatSocket
    print("the data received from sio")
    msg = '/Msg,'+data['msg']
    # Send received data from server to unity
    unityChatSocket.send(msg.encode('utf-8'))


@sio.on('*')
def catch_all(event, data):
    print('all handlers', event, data)


@sio.event
def createRoomStatus(data):
    global sendServer, RoomID
    if data['status'] == 'true':
        msg = "true,"+data['RoomID']+","+data['UserID']
        sendServer.send(msg.encode('utf-8'))
        RoomID = data['RoomID']
    elif data['status'] == 'false':
        msg = "false,"+data['RoomID']
        sendServer.send(msg.encode('utf-8'))


# Press PAGE UP then PAGE DOWN to type "foobar".
def emitting(value):
    now = datetime.now()
    sio.emit('my message', {'msg': value})
    current_time = now.strftime("%H:%M:%S")
    print("Current Time =", current_time)


def unityReceive():
    global UserID,RoomID
    try:
        while True:
            ADRESS = "127.0.0.1"
            PORTReceive = 3003
            S = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            S.bind((ADRESS, PORTReceive))
            S.listen(5)
            conn, adress = S.accept()
            sendServer = conn
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
                            sio.emit('CreateRoom', {'RoomID': data})
                        elif func == '/Join':
                            print(data)
                            sio.emit('joinRoom', {'RoomID': data})
                        elif func == "/Start":
                            sio.emit('StartGame', {'RoomID': RoomID})

                    except:
                        counter += 1
                        print(counter)
                        pass
    except:
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
            print('unity send connected to python')
            counter = 0
            while True:
                ...
                # conn.send('ahmed'.encode('utf-8'))
                # data = conn.recv(1024).decode('utf-8')
                # if data == "OK":
                # print("Ok")
                # counter += 1
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
            print(func, data)
            # broadcast message to all servers
            if func == "/Message":
                print(data)
                sio.emit('ChatRoom', {'RoomID': RoomID, 'msg': data})

            else:
                pass


# https://race-car.onrender.com/
# 'http://localhost:3000'
if __name__ == '__main__':
    sio.connect('https://race-car.onrender.com/')
    thread1 = threading.Thread(target=unityReceive)
    thread2 = threading.Thread(target=unitySend)
    # sio.emit('Chat', {'RoomID': RoomID, 'msg': 'Success my Dude'})
    thread3 = threading.Thread(target=Chat)

    thread1.start()
    thread2.start()
    # chat thread
    thread3.start()


def senddata():
    global unityChatSocket
    sio.emit('Chat', {'RoomID': RoomID, 'msg': 'Success my Dude'})
    print("ahemd")
    counter = 0
