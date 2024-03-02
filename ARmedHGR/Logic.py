import cv2
import mediapipe as mp
import time
import numpy as np
import math

print("Libraries imported successfully")

class HandDetector:
    def __init__(self, mode=False, maxHands=2, detectionConfidence=0.5, trackConfidence=0.5):
        self.mode = mode
        self.maxHands = maxHands
        self.detectionConfidence = detectionConfidence
        self.trackConfidence = trackConfidence

        self.mpHands = mp.solutions.hands
        # Replace the following line with the modified version
        self.hands = self.mpHands.Hands(static_image_mode=False, max_num_hands=self.maxHands, min_detection_confidence=self.detectionConfidence, min_tracking_confidence=self.trackConfidence)
        self.mpDraw = mp.solutions.drawing_utils

    def findHands(self, img, draw=True):
        imgRGB = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        self.results = self.hands.process(imgRGB)
        if self.results.multi_hand_landmarks:
            for handLms in self.results.multi_hand_landmarks:
                if draw:
                    self.mpDraw.draw_landmarks(img, handLms, self.mpHands.HAND_CONNECTIONS)
        return img

    def findPosition(self, img, handNo=0, draw=True):
        lmList = []
        if self.results.multi_hand_landmarks and len(self.results.multi_hand_landmarks) > handNo:
            myHand = self.results.multi_hand_landmarks[handNo]
            for id, lm in enumerate(myHand.landmark):
                h, w, c = img.shape
                cx, cy = int(lm.x * w), int(lm.y * h)
                lmList.append([id, cx, cy])
                if draw:
                    cv2.circle(img, (cx, cy), 5, (255, 0, 255), cv2.FILLED)
        return lmList

def zoom():
    cap = cv2.VideoCapture(0)
    detector = HandDetector(detectionConfidence=0.7, maxHands=2)
    pTime = 1

    while True:
        success, img = cap.read()
        img = detector.findHands(img)
        lmList0 = detector.findPosition(img, draw=False, handNo=0)
        lmList1 = detector.findPosition(img, draw=False, handNo=1)

        if len(lmList0) != 0 and len(lmList1) != 0:
            pointer_x1, pointer_y1 = lmList0[8][1], lmList0[8][2]
            pointer_x2, pointer_y2 = lmList1[8][1], lmList1[8][2]

            # Print coordinates of index fingers
            print("Index Finger 1:", pointer_x1, pointer_y1)
            print("Index Finger 2:", pointer_x2, pointer_y2)

            thumb_x1, thumb_y1 = lmList0[4][1], lmList0[4][2]
            thumb_x2, thumb_y2 = lmList1[4][1], lmList1[4][2]

            cx1, cy1 = (pointer_x1 + thumb_x1) // 2, (pointer_y1 + thumb_y1) // 2
            cx2, cy2 = (pointer_x2 + thumb_x2) // 2, (pointer_y2 + thumb_y2) // 2

            cv2.line(img, (cx1, cy1), (cx2, cy2), (255, 255, 255))

            length = math.hypot(cx2 - cx1, cy2 - cy1)
            yield length

        cTime = time.time()
        fps = 1 / (cTime - pTime)
        pTime = cTime

        cv2.imshow("Img", img)
        key = cv2.waitKey(1)
        if key == 27:
            break

for length in zoom():
        print(length)
   