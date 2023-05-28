from PyQt5 import uic
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *
import sys
app = QApplication(sys.argv)
import socketio
import keyboard
from datetime import datetime
sio = socketio.Client()
@sio.on('connect')
def connect_handler():
    print('Connected!')

@sio.event
def br(data):
    print(data['msg'])

@sio.event
def test(data):
    print(data['msg'])

    
class Home(QMainWindow):
    def __init__(self) -> None:
        """
        init the main window for the gui

        """
        super(Home, self).__init__()
        uic.loadUi('./client.ui', self)
        # searching for the buttons by ID and referencing them
        self.send = self.findChild(QPushButton, 'send_btn')
        self.msg = self.findChild(QTextEdit, 'message_txt')

        # assign event listeners when the button is clicked to excute a function
        self.send.clicked.connect(self.send_msg)
    
    def send_msg(self):
        value = self.msg.toPlainText()
        self.emitting(value)

    def emitting(self,value):
        now = datetime.now()
        sio.emit('my message',{'msg': value})
        current_time = now.strftime("%H:%M:%S")
        print("Current Time =", current_time)

def run():
    sio.connect('http://localhost:3000')
    home = Home()
    home.show()
    app.exec_()

if __name__ == '__main__':
    
    run()

    
