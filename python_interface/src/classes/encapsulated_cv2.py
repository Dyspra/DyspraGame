from cv2 import VideoCapture, cvtColor, imshow, flip, waitKey

class encapsulated_cv2:
    def __init__(self, camera : int) -> bool:
        self.videocap = VideoCapture(camera)
        if self.videocap == None:
            print('Error: Camera isn\'t correctly initialised')        
            self.isReady = False
        else:
            print('Camera initialized...')
            self.isReady = True
    def __del__(self):
        print('Closing Camera...')
        self.videocap.release()
    def display(self, title_of_screen : str, image):
        imshow(title_of_screen, flip(image, 1))
    