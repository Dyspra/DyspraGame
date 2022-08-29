from socket import socket, AF_INET, SOCK_DGRAM
from json import dumps
from src.classes.interface import interface

class communication(interface):
    def __init__(self, port, hostname):
        self.socket = socket(AF_INET, SOCK_DGRAM)
        self.socket.bind((hostname, port))
    def __del__(self):
        self.socket.close()
    def send_package(self, x, y, z, landmark, image, date):
        data = dumps({x: x, y: y, z: z, landmark: landmark, id: image, date: date})
        try:
            self.socket.sendall(bytes(data, encoding="utf-8"))
        except ValueError as e:
            print(e)
    def recv_package(self, size : int):
        return self.socket.recvfrom(size)