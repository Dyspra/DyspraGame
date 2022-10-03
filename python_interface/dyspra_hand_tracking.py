from __future__ import annotations

import cv2
import mediapipe as mp

mp_hands = mp.solutions.hands
from src.classes.encapsulated_cv2 import encapsulated_cv2
from src.encapsulated_mediapipe import draw_image
from src.classes.communication import communication
from time import time
from keyboard import is_pressed

# from src.encapsulated_mediapipe import draw_image
from src.classes.communication import communication
from src.classes.encapsulated_cv2 import encapsulated_cv2


def handtracking() -> None:
    videocap: encapsulated_cv2 = encapsulated_cv2(0)
    communicate: communication = communication(6542, "127.0.0.1")
    date: float = 0

    if videocap.isReady == False:
        return
    with mp_hands.Hands(
      model_complexity=0,
      min_detection_confidence=0.5,
      min_tracking_confidence=0.5) as hands:
      while videocap.videocap.isOpened():
        success, image = videocap.videocap.read()
        if not success:
          continue
        image.flags.writeable = False
        image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
        results = hands.process(image)
        if results.multi_hand_landmarks:
          date = time()
          for hand in results.multi_hand_landmarks:
              for idx, landmark in enumerate(hand.landmark):
                if idx <= 41:
                  if (results.multi_handedness == 0):
                    communicate.send_package(landmark.x, landmark.y, landmark.z, idx + 21, date)
                  else:
                    print(landmark)
                    communicate.send_package(landmark.x, landmark.y, landmark.z, idx, date)
          # image = draw_image(results, image)
          # videocap.display("Dyspra Debug", image)
        if is_pressed("x"):
          break
if __name__ == '__main__':
      handtracking()
