# Developed for ARMed
# Functionality is complete, just needs an output system. The output system will deliver information on the length of the distance between the tip of your index finger and the thumb.

import HandTrack as ht
import cv2
import mediapipe as mp
import time
import numpy as np
import math
print("Libraries imported successfuly")

cap = cv2.VideoCapture(1) # change to 0 or to 1 if u get an error, basically changes what camera to use. Built in cameras for laptops are usually defaulted by 0. External cameras are usually 1.

detector = ht.HandDetector(detectionConfidence=0.7) # initialize object from HD class with higher than normal detection confidence

pTime = 1 # set to 1 here to avoid float divsion by zero error, will display wrong fps for a ms or so
 
while True:
    success, img = cap.read()
    img = detector.findHands(img)

    lmList = detector.findPosition(img, draw=False)

    if len(lmList) != 0: # This way it doesnt display empty lists

        # the zoom feature requires the tip of the thumb and tip of the index (landmarks 4 and 8 respectively)
    
        x1, y1 = lmList[4][1], lmList[4][2]
        x2, y2 = lmList[8][1], lmList[8][2]
        cv2.circle(img, (x1,y1), 15, (255,0,255), cv2.FILLED)
        cv2.circle(img, (x2,y2), 15, (255,0,255), cv2.FILLED)

        cv2.line(img, (x1,y1), (x2,y2), (255,0,255)) # draws a line between the fingers

        cx, cy = (x1 + x2) // 2, (y1 + y2) // 2 # calculate the location of the center of the line

        cv2.circle(img, (cx, cy), 15, (255, 0, 255), cv2.FILLED)

        length = math.hypot(x2 - x1, y2 - y1) # calculate the length of the line
        print(length)

        if length < 50: # this value will need to be changed when the headset is 
            cv2.circle(img, (cx, cy), 15, (0, 255, 0), cv2.FILLED) # if the tips of the fingers are touching, change the colour (kind of like a button)


    cTime = time.time()

    fps = 1/(cTime-pTime) # calculate fps
    pTime = cTime # set pTime to cTime after fps is calcualated to avoid float division by zero error
 
    cv2.putText(img, f'FPS: {int(fps)}', (40, 50), cv2.FONT_HERSHEY_COMPLEX, 1, (255, 0, 0), 3) # writes the fps to screen. putText takes in paramaters, what window (img in this case), what to write (fps in this case), location on window, font (can make it whatever you want), size, colour, thickness
    
    cv2.imshow("Img", img)
    key = cv2.waitKey(1)
    if key == 27:
        break
