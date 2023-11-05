from zlib import crc32
from socket import socket, AF_INET, SOCK_DGRAM, SOCK_STREAM
from json import dumps
from struct import pack
from src.classes.interface import interface
import sys
from json import dumps
from socket import AF_INET, SOCK_DGRAM, SOCK_STREAM, socket

from src.classes.interface import interface


class communication(interface):
    def __init__(self, port, hostname, test_mode=False):
        self.socket = socket(AF_INET, SOCK_DGRAM)
        if test_mode:
            self.socket.bind((hostname, port))
            client = self.socket.recvfrom(1024)
            self.game = client[1]
        else:
            self.game = (hostname, port)
        print("Connection established with the client :", hostname, port, sep=" ")

    def __del__(self):
        self.socket.sendto(bytes("Exit Success", encoding="utf-8"), self.game)
        self.socket.close()
        print("Socket closed")

    def send_package(self, x, y, z, landmark, date):
        data = str(x) + ',' + str(y) + ',' + str(z) + ',' + str(landmark) + ',' + str(date)
        # data = dumps({"x": x, "y": y, "z": z, "landmark": landmark, "date": date})
        packet_infos = data.encode()
        header = pack("!IIII", 5000, self.game[1], len(packet_infos), crc32(packet_infos))
        try:
            # print(data)
            self.socket.sendto((header + bytes(data, encoding="utf-8")), self.game)
        except (ValueError, OSError) as e:
            print(e)

    def send_all_package(self, packet):
        packet_infos = packet.encode()
        header = pack("!IIII", 5000, self.game[1], len(packet_infos), crc32(packet_infos))
        try:
            #print(packet)
            #print(len(packet))
            self.socket.sendto((header + bytes(packet, encoding="utf-8")), self.game)
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
    def recv_package(self, size: int):
        return self.socket.recvfrom(size)
