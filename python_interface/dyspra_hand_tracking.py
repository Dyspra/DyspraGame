import cv2
import mediapipe as mp
mp_hands = mp.solutions.hands
from src.classes.encapsulated_cv2 import encapsulated_cv2
from src.encapsulated_mediapipe import draw_image
from src.classes.communication import communication
from time import time

def handtracking() -> None:
    videocap : encapsulated_cv2 = encapsulated_cv2(0)
    communicate : communication = communication(6542, "127.0.0.1")
    if (videocap.isReady == False):
        return
    with mp_hands.Hands(
      model_complexity=0,
      min_detection_confidence=0.5,
      min_tracking_confidence=0.5) as hands:
      while videocap.videocap.isOpened():
        success, image = videocap.videocap.read()
        if not success:
          print("Ignoring empty camera frame.")
          continue
        image.flags.writeable = False
        image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
        results = hands.process(image)
        for idx, landmark in enumerate(results):
              communicate.send_package(landmark.x, landmark.y, landmark.z, idx, time())
        if results.multi_hand_landmarks:
          print(results.multi_hand_landmarks)
          image = draw_image(results, image)
          videocap.display("Dyspra Test", image)
        if cv2.waitKey(5) & 0xFF == 27:
          break
if __name__ == '__main__':
      handtracking()
