from __future__ import annotations

import cv2
import mediapipe as mp

mp_hands = mp.solutions.hands
from src.classes.encapsulated_cv2 import encapsulated_cv2
from src.encapsulated_mediapipe import draw_image
from src.classes.communication import communication
from time import time, sleep
# from keyboard import is_pressed
from google.protobuf.json_format import MessageToDict

# from src.encapsulated_mediapipe import draw_image
from src.classes.communication import communication
from src.classes.encapsulated_cv2 import encapsulated_cv2
from sys import argv


def handtracking(port : str, address : str, camera_idx = 0, frequency = 10) -> None:
    videocap: encapsulated_cv2 = encapsulated_cv2(camera_idx)
    communicate: communication = communication(int(port), address)
    date: float = 0
    data: str = ""
    

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
          sleep((1 /frequency))
          continue
        image.flags.writeable = False
        image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
        # process the image with mediapipe
        results = hands.process(image)
        if results.multi_hand_landmarks:
          date = time()
          # Loop on all hands detected on the image
          #print(results.multi_hand_landmarks)
          for idx_hand, hand in enumerate(results.multi_hand_landmarks):
              label = MessageToDict(results.multi_handedness[idx_hand])['classification'][-1]['label']
              #print(label)
              for idx, landmark in enumerate(hand.landmark):
                if idx <= 41:
                  if (label == "Left"):
                    # Send the package for the left hand idx range = 0:21 
                    data += str(round(landmark.x,2)) + ',' + str(round(landmark.y,2)) + ',' + str(round(landmark.z,2)) + ',' + str(idx) + '|'
                    # communicate.send_package(landmark.x, landmark.y, landmark.z, idx, date)
                  else:
                    # Send the package for the right hand idx range = 21:42 
                    data += str(round(landmark.x,2)) + ',' + str(round(landmark.y,2)) + ',' + str(round(landmark.z,2)) + ',' + str(21 + idx) + '|'
                    # 
                    # communicate.send_package(landmark.x, landmark.y, landmark.z, 21 + idx, date)
              communicate.send_all_package(data + str(round(date, 0)))
          data = ""
        sleep((1 / frequency))
if __name__ == '__main__':
  if len(argv) <= 1:
      exit(84)
  elif len(argv) < 4 and len(argv) > 1:
      handtracking(argv[1], argv[2])
  else:
      handtracking(argv[1], argv[2], argv[3])