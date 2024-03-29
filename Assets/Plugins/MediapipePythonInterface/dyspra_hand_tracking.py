from __future__ import annotations

import cv2
import mediapipe as mp

mp_hands = mp.solutions.hands
from src.classes.encapsulated_cv2 import encapsulated_cv2
from src.encapsulated_mediapipe import draw_image
from src.classes.communication import communication
from time import time
# from keyboard import is_pressed
from google.protobuf.json_format import MessageToDict

# from src.encapsulated_mediapipe import draw_image
from src.classes.communication import communication
from src.classes.encapsulated_cv2 import encapsulated_cv2
from sys import argv


def handtracking(port : str, address : str) -> None:
    videocap: encapsulated_cv2 = encapsulated_cv2(0)
    communicate: communication = communication(int(port), address)
    date: float = 0

    if videocap.isReady == False:
        return
    with mp_hands.Hands(
      model_complexity=0,
      min_detection_confidence=0.5,
      min_tracking_confidence=0.5) as hands:
      while videocap.videocap.isOpened():
        # Read the image and get if the capture is successful
        success, image = videocap.videocap.read()
        if not success:
          continue
        image.flags.writeable = False
        image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
        # process the image with mediapipe
        results = hands.process(image)
        if results.multi_hand_landmarks:
          date = time()
          # Loop on all hands detected on the image
          for idx_hand, hand in enumerate(results.multi_hand_landmarks):
              label = MessageToDict(results.multi_handedness[idx_hand])['classification'][-1]['label']
              print(label)
              for idx, landmark in enumerate(hand.landmark):
                if idx <= 41:
                  if (label == "Left"):
                    # Send the package for the left hand idx range = 0:21 
                    print(landmark)
                    communicate.send_package(landmark.x, landmark.y, landmark.z, idx, date)
                  else:
                    # Send the package for the right hand idx range = 21:42 
                    print(landmark)
                    communicate.send_package(landmark.x, landmark.y, landmark.z, 21 + idx, date)
          # image = draw_image(results, image)
          # videocap.display("Dyspra Debug", image)
        # if is_pressed("x"):
        #   break
if __name__ == '__main__':
      handtracking(argv[1], argv[2])