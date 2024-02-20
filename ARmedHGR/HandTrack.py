import cv2
import mediapipe as mp
import time

class HandDetector():
    def __init__(self, mode=False, maxHands=2, modelC=1, detectionConfidence=0.5, trackConfidence=0.5):
        # Constructor to initialize the hand detector object
        self.mode = mode
        self.maxHands = maxHands
        self.modelC = modelC
        self.detectionConfidence = detectionConfidence
        self.trackConfidence = trackConfidence
        self.mphands = mp.solutions.hands
        self.hands = self.mphands.Hands(self.mode, self.maxHands, self.modelC, self.detectionConfidence, self.trackConfidence)
        self.mpDraw = mp.solutions.drawing_utils

    def findHands(self, img, draw=True):
        # Method to find hands in the input image and draw landmarks and connections if required
        imgRGB = cv2.cvtColor(img, cv2.COLOR_RGB2BGR)
        self.results = self.hands.process(imgRGB)

        if self.results.multi_hand_landmarks:
            for handLms in self.results.multi_hand_landmarks:
                if draw: # set as true in function definition
                    self.mpDraw.draw_landmarks(img, handLms, self.mphands.HAND_CONNECTIONS)
        return img
    
    def findPosition(self, img, handNo, draw=True):
        lmList = [] # List to store and return landmark positions
        # You can iterate through lmList with [] to obtain the positions of certian landmarks
        # For example lmList[4] returns the position of the tip of your thumb


        if self.results.multi_hand_landmarks:

            # In this if else conditional, we are handling the case where one hand is taken away from the frame. 
            # The if statement works like this:
            # self.results.multi_hand_landmarks is a list that has 0-2 elements depending on how many hands are visible.
            # handNo is 0 indexed, so the len(self.results.multi_hand_landmarks) will always be more than handNo if it is working correctly.
            # If an object calls upon this function with handNo=2, but a second hand is not visible in frame, instead of breaking the function will just return the last available landmark position of said hand and return an empty list from there on.
            if len(self.results.multi_hand_landmarks) > handNo:
                myHand = self.results.multi_hand_landmarks[handNo]
            else:
                myHand = None
                return lmList
            # myHand = self.results.multi_hand_landmarks[handNo]
            for id, lm in enumerate(myHand.landmark):
                h, w, c = img.shape # Get height width and channel of image
                cx, cy = int(lm.x * w), int(lm.y * h) # Calculate pixel coordinates of landmark
                lmList.append([id, cx, cy])
                if draw:
                    cv2.circle(img, (cx, cy), 7, (255, 0, 0), cv2.FILLED) # Will draw a filled circle at each landmark so itll be easier to see them
        return lmList


def main(): 
    # Main function for running the hand tracking application

    # Open the video capture device (camera)
    cap = cv2.VideoCapture(1) # change to zero if error happens

    # Variables for tracking time and calculating frames per second (fps)
    pTime = 0
    cTime = 0

    # Create a HandDetector object
    detector = HandDetector()

    # Main loop for capturing and processing video frames
    while True:
        # Read a frame from the camera
        success, img = cap.read()

        # Use the HandDetector to find and draw hands in the frame
        img = detector.findHands(img)


        # Use the HandDetector to find the position of the hands in the frame and send the coordinates to lmList
        lmList = detector.findPosition(img)
        if lmList != []: # this is so it doesnt't flood our output with blank arrays
            print(lmList) # we can index this variable to obtain the position of each joint/part of the hand, refer to landmark map in documentation

        # Calculate and display frames per second
        cTime = time.time()
        fps = 1 / (cTime - pTime)
        pTime = cTime
        cv2.putText(img, str(int(fps)), (10, 70), cv2.FONT_HERSHEY_PLAIN, 3, (255, 0, 255), 3)

        # Display the frame with the detected hands
        cv2.imshow("Image", img)

        # Exit the program if the 'Esc' key is pressed
        if cv2.waitKey(1) == 27:
            break

if __name__ == "__main__":
    # Run the main function if this script is executed directly
    main()
