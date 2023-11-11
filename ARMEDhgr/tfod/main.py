import cv2
import mediapipe as mp
import time


class HandDetector():
    def __init__(self,mode=False,maxHands=2, detectionConfidence = 0.5, trackConfidence = 0.5):
        self.mode = mode




mphands = mp.solutions.hands
hands = mphands.Hands()
mpDraw = mp.solutions.drawing_utils

imgRGB = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
results = hands.process(imgRGB)

if results.multi_hand_landmarks:
    for handLms in results.multi_hand_landmarks:
        for id, lm in enumerate(handLms.landmark):
            h, w, c = img.shape
            cx, cy = int(lm.x * w), int(lm.y * h)
            print(id, cx, cy)

            cv2.circle(img, (cx, cy), 15, (255, 0, 255), cv2.FILLED)

        mpDraw.draw_landmarks(img, handLms, mphands.HAND_CONNECTIONS)

def main(): 

    cap = cv2.VideoCapture(0)
    pTime = 0
    cTime = 0

    while True:
        success, img = cap.read()
        

        cTime = time.time()
        fps = 1 / (cTime - pTime)
        pTime = cTime


        cv2.putText(img, str(int(fps)), (10, 70), cv2.FONT_HERSHEY_PLAIN, 3, (255, 0, 255), 3)

        cv2.imshow("Image", img)
        cv2.waitKey(1)

if __name__ == "__main__":
    main()
