import socket
from keyboard import is_pressed
from sys import exit

msgFromClient       = "test"
bytesToSend         = str.encode(msgFromClient)
serverAddressPort   = ("127.0.0.1", 6542)
bufferSize          = 1024

UDPClientSocket = socket.socket(family=socket.AF_INET, type=socket.SOCK_DGRAM)
UDPClientSocket.sendto(bytesToSend, serverAddressPort)

while True:
    if (is_pressed("x")):
        UDPClientSocket.close()
        exit(0)
    msgFromServer = UDPClientSocket.recvfrom(bufferSize)
    msg = "Message from Server {}".format(msgFromServer[0])
    print(msg)
