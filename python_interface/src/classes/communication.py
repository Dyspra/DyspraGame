from socket import socket, AF_INET, SOCK_DGRAM, SOCK_STREAM
from json import dumps
from src.classes.interface import interface
import sys

class communication(interface):
    def __init__(self, port, hostname):
        self.socket = socket(AF_INET, SOCK_DGRAM)
        self.socket.bind((hostname, port))
        client = self.socket.recvfrom(1024)
        self.game = client[1]
        print("Connection established with the client :", client[1])
    def __del__(self):
        self.socket.sendto(bytes("Exit Success", encoding="utf-8"), self.game)
        self.socket.close()
        print("Socket closed")
    def send_package(self, x, y, z, landmark, date):
        print(x, y, z, landmark)
        data = dumps({"x": x, "y": y, "z": z, "landmark": landmark, "date": date})
        try:
            self.socket.sendto(bytes(data, encoding="utf-8"), self.game)
        except (ValueError, OSError) as e:
            print(e)
    def send_ping(self):
        data = self.recv_package(1024)
        if data and data[0] and data[1] == self.game:
            try:
                self.socket.sendto(data[0], self.game)
            except (ValueError, OSError) as e:
                print(e)
                self.__del__()
                sys.exit()
    def recv_package(self, size : int):
        return self.socket.recvfrom(size)