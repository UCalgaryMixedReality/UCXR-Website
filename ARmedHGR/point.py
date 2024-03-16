# Developed for ARMed
# Functionality is complete, just needs an output system. The output system will deliver information on the length of the distance between the tip of your index finger and the thumb.

# Tracks position of the tip of the index finger

import HandTrack as ht
import cv2
import mediapipe as mp
import time
import numpy as np
import math

print("Libraries imported successfuly")

cap = cv2.VideoCapture(0) # change to 0 or to 1 if u get an error, basically changes what camera to use. Built in cameras for laptops are usually defaulted by 0. External cameras are usually 1.

detector = ht.HandDetector(detectionConfidence=0.7 ) # initialize object from HD class with higher than normal detection confidence

pTime = 1 # set to 1 here to avoid float divsion by zero error, will display wrong fps for a ms or so

while True:
    success, img = cap.read()
    img = detector.findHands(img)

    lmList = detector.findPosition(img, draw=False, handNo = 1)

    if len(lmList) != 0: # This way it doesnt display empty lists
        
        # tip of the finger is landmark 8
        x, y = lmList[8][1], lmList[8][2] # Position of the tip of the index - these values will be exported
        
        cv2.circle(img, (x,y), 15, (255,0,255), cv2.FILLED)
        print(x, y)
        
def point(img, lmList0, lmList1):

    if len(lmList0) == 0 or len(lmList1) == 0:
        print("one of the lmLists is empty")
        yield

    elif len(lmList0[0]) >= 8 and len(lmList1[0]) >= 8:
        try:
            x, y = lmList0[0][8][0], lmList1[0][8][1]
            yield [x, y]
            

        except IndexError:
            print("Index error here")
