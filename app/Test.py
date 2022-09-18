from socket import socket, AF_INET, SOCK_DGRAM

PORT: int = 5000

with socket(AF_INET, SOCK_DGRAM) as s:
    s.bind(("", PORT))
    print("Start serving")
    msg, sender = s.recvfrom(1024)
    print(f"Recieved {msg}")
    s.sendto(b"Hello back", sender)