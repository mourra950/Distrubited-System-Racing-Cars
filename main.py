from PyQt5 import uic
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *
import sys
import socketio
import keyboard
from datetime import datetime

chat_list = []
app = QApplication(sys.argv)
sio = socketio.Client()


@sio.on('connect')
def connect_handler():
    print('Connected!')


@sio.event
def br(data):
    current_time = datetime.utcnow().strftime("%H:%M:%S:%f")[:-3]
    chat_list.append((str(data['msg']), str(current_time)))


@sio.event
def roomStatus(data):
    print(data)


@sio.event
def createRoomStatus(data):
    print(data)


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
        self.chat = self.findChild(QTextEdit, 'chat_txt')
        self.msg = self.findChild(QTextEdit, 'message_txt')
        self.re = self.findChild(QPushButton, 'refresh')

        # assign event listeners when the button is clicked to excute a function
        self.send.clicked.connect(self.send_msg)
        self.re.clicked.connect(self.inserting)

    def send_msg(self):
        value = self.msg.toPlainText()
        self.emitting(value)

    def inserting(self):
        self.chat.clear()
        for i in chat_list:
            self.chat.insertPlainText(str(i))
            self.chat.insertPlainText('\n')

    def emitting(self, value):
        current_time = datetime.utcnow().strftime("%H:%M:%S:%f")[:-3]
        chat_list.append((str(value), str(current_time)))
        sio.emit('my message', {'msg': value})
        sio.emit('joinRoom', {'roomID': value})

        self.inserting()
        # print("Current Time =", current_time)

# http://localhost:3000
# https://race-car.onrender.com


def run():
    sio.connect('http://localhost:3000')
    sio.emit('CreateRoom')

    home = Home()
    home.show()
    app.exec_()


if __name__ == '__main__':

    run()
