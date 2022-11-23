from __future__ import annotations

import os
import sys
import time
from selectors import EVENT_READ, DefaultSelector
from socket import AF_INET, SOCK_DGRAM, socket as Socket
from threading import Event, Thread
from typing import Iterator

import pytest

print(sys.path.insert(0, os.path.dirname(sys.path[0])))


def _launch_udp_server(socket: Socket, shutdown_requested: Event) -> None:
    with DefaultSelector() as selector:
        selector.register(socket, EVENT_READ)
        while not shutdown_requested.is_set():
            if selector.select(0.1):
                data, addr = socket.recvfrom(8192)
                socket.sendto(data, addr)


@pytest.fixture(scope="module")
def udp_server() -> Iterator[tuple[str, int]]:
    shutdown_requested = Event()

    with Socket(AF_INET, SOCK_DGRAM) as s:
        s.bind(("localhost", 0))
        server_thread: Thread = Thread(target=_launch_udp_server, args=(s, shutdown_requested), daemon=True)
        server_thread.start()
        time.sleep(0.1)
        yield s.getsockname()
        shutdown_requested.set()
        server_thread.join(timeout=1)
