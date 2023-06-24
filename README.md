# Distrubited System Racing Cars

Welcome to our multiplayer distributed racing car game! The primary objective of this project was to develop a game that supports multiple agents competing for shared resources in real-time. Our focus was on ensuring the game could be distributed across multiple clients and server nodes while maintaining robustness. Even in the event of a client crash or disconnection, the game remains functional, allowing the affected player to recover and resume gameplay.

To achieve our goals, we made deliberate choices regarding the technologies utilized. Although we had the freedom to use any packages or languages, we decided to stick with basic libraries and code the system ourselves. This approach enabled us to gain a deeper understanding of socket programming and further enhance our learning experience.

The architecture of our multiplayer distributed racing car game is illustrated in the diagram below:

![Node_Distribution](https://github.com/mourra950/Distrubited-System-Racing-Cars/assets/64339763/69604caf-c0ff-4a14-85c2-a90725dfe8ed)

1. Unity Game Instance: The game itself, developed using Unity game engine.
2. Python Proxy Server: Acts as a middle layer between the Unity game instance and the dedicated game server. It facilitates communication using TCP/IP sockets.
3. Dedicated Game Server: Developed using Node.js and deployed on EC2. This server handles game logic and real-time interactions using the Socket.IO library.
4. MongoDB Server: Stores game information and updates for the game server.
5. Node.js Data Server: Fetches data from the MongoDB server and provides a REST API for the React webpage.
6. React Webpage: Showcases the project, utilizing data from the Node.js data server.
We've designed this architecture to ensure smooth communication between the Unity game instance, the Python proxy server, the dedicated game server, and the web components. It leverages a combination of technologies to provide an engaging and dynamic multiplayer gaming experience.In case you want to check the code you can check the branches.

## Run game

### Prerequisite

* Windows operating system
* Python and Pip installed

### Installation Steps

1. Download the game files by clicking on the following link: [Download](https://drive.google.com/drive/folders/1frWMftkLvu9jjcvbRjIKaBYx2iYEl2MM?usp=sharing)
2. Extract the contents of the downloaded ZIP file to a location of your choice.

### Installing Python proxy server Dependencies

1. From root directory of the project.
2. Look for a file named "Run first time.exe" and double-click on it or run using the terminal

```bash
   pip3 install -r requirements.txt
```

or

```bash
   pip install -r requirements.txt
```
![Uploading Node_Distribution.svg…]()

### Excute the game

1. Navigate to the root directory.
2. Look for a file named "run.exe" and excute it.
3. The proxy script will run in the background and the game will launch.
4. On launch the first scene will be the main menu where you can freely create, join or spectate a game.

### Controls


| Description | Keys |
| :---:        |     :---:      |
| car movment   | ![keyboards](https://github.com/mourra950/Distrubited-System-Racing-Cars/assets/64339763/700a231d-ba02-4b31-8875-6b9192696234) |
| chat control     | ![ctrls](https://github.com/mourra950/Distrubited-System-Racing-Cars/assets/64339763/1ef5efb5-12b0-4865-a5bf-9452f30c0859)   |

#### Car Movment

* The car controls are easy which WASD the norm control for movment in any video game.
* W to move forward, S to move backward.
* A to steer left, D to steer right but the car cant rotate unless the car is moving.

#### Chatting 

* Using sockets and socketio every client is able to broadcast messages with ease.
* In order to send a message the user need to type it in the input field provided during the game or in lobby.
* In order to start typing and send the user need to focus on the input either by mouse or by pressing Lctrl key.
* After writing the message the user needs to press Lctrl when the input field is focused to send the message.

## YouTube Demonstration Video

For a visual guide on how to download, install, and run the multiplayer distributed racing car game, we have provided a demonstration video on YouTube. This video will walk you through the entire process, making it easier to get started with the game.

You can watch our video demonstration on Youtube
[Serene Track Demonstration](https://youtu.be/XfoahjVGTwg)
