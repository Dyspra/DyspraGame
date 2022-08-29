class interface:
    def __init__(self, port, hostname):
        pass
    def __del__(self):
        pass
    def send_package(self, x, y, z, landmark, image, date):
        pass
    def recv_package(self, size):
        pass