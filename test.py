import socketio
from datetime import datetime
import socket
import threading
import keyboard

# standard Python
sio = socketio.Client()
sendServer = None
SessionID = '123'
unityChatSocket = None


@sio.on('connect')
def connect_handler():
    print('Connected!')


@sio.event
def roomStatus(data):
    print(data)


@sio.event
def ChatBroadcast(data):
    print("the data received from chat")
    


@sio.event
def createRoomStatus(data):
    print(data)


# Press PAGE UP then PAGE DOWN to type "foobar".
def emitting(value):
    now = datetime.now()
    sio.emit('my message', {'msg': value})
    current_time = now.strftime("%H:%M:%S")
    print("Current Time =", current_time)






# https://race-car.onrender.com
# 'http://localhost:3000'
if __name__ == '__main__':
    sio.connect('http://localhost:3000')
    # thread1 = threading.Thread(target=unityReceive)
    # thread2 = threading.Thread(target=unitySend)
    # thread3 = threading.Thread(target=Chat)

    keyboard.add_hotkey('space', lambda: senddata())

    # thread1.start()
    # thread2.start()
    # thread3.start()


def senddata():
    global unityChatSocket
    sio.emit('Chat', {'RoomID': SessionID, 'msg': 'Success my Dude'})
    print("sending")
    counter = 0
