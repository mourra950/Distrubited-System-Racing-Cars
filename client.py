import socketio
import keyboard
from datetime import datetime
from main import run
# standard Python
sio = socketio.Client()


@sio.on('connect')
def connect_handler():
    print('Connected!')
    run()
    
# Press PAGE UP then PAGE DOWN to type "foobar".
def emitting(value):
    now = datetime.now()
    sio.emit('my message',{'msg': value})
    current_time = now.strftime("%H:%M:%S")
    print("Current Time =", current_time)
# keyboard.add_hotkey('a', emitting )

# 'http://localhost:3000'
sio.connect('http://localhost:3000')