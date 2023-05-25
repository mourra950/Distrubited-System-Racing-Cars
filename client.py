import socketio

# standard Python
sio = socketio.Client()


@sio.event
def connect():
    print("I'm connected!")

sio.connect('http://localhost:5000')