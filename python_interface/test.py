import socket
import keyboard

msgFromClient       = "test"
bytesToSend         = str.encode(msgFromClient)
serverAddressPort   = ("127.0.0.1", 6542)
bufferSize          = 1024

UDPClientSocket = socket.socket(family=socket.AF_INET, type=socket.SOCK_DGRAM)
UDPClientSocket.sendto(bytesToSend, serverAddressPort)

while True:
    if (keyboard.is_pressed('x')):
        break
    msgFromServer = UDPClientSocket.recvfrom(bufferSize)
    msg = "Message from Server {}".format(msgFromServer[0])
    print(msg)
UDPClientSocket.close()