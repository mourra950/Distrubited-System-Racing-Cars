import socketio
import keyboard
from datetime import datetime
# standard Python
sio = socketio.Client()


@sio.on('connect')
def connect_handler():
    print('Connected!')
    
# Press PAGE UP then PAGE DOWN to type "foobar".
def emitting():
    now = datetime.now()
    sio.emit('my message')
    current_time = now.strftime("%H:%M:%S")
    print("Current Time =", current_time)
keyboard.add_hotkey('a', emitting )

sio.connect('https://race-car.onrender.com')