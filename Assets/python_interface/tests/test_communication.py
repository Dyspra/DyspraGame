from __future__ import annotations

import json

from src.classes.communication import communication


def test_send_landmark(udp_server: tuple[str, int]) -> None:
    # Arrange
    host, port = udp_server
    c = communication(port=port, hostname=host, test_mode=True)

    # Act
    c.send_package(5, 10, 25, 0, 123456)
    response: bytes
    response, _ = c.recv_package(8192)

    # Assert
    assert json.loads(response.decode("ascii")) == {"x": 5, "y": 10, "z": 25, "landmark": 0, "date": 123456}
