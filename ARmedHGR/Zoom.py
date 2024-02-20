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

detector = ht.HandDetector(detectionConfidence=0.7, maxHands=2) # initialize object from HD class with higher than normal detection confidence

pTime = 1 # set to 1 here to avoid float divsion by zero error, will display wrong fps for a ms or so
 
while True:
    success, img = cap.read()
    img = detector.findHands(img)

    lmList0 = detector.findPosition(img, draw=False, handNo=0) 
    lmList1 = detector.findPosition(img, draw=False, handNo=1)

    if len(lmList0) != 0 and len(lmList1) != 0: # This way it doesnt display empty lists / get index errors

        # the zoom feature requires the tip of the thumb and tip of the index (landmarks 4 and 8 respectively)

        pointer_x1, pointer_y1 = lmList0[8][1], lmList0[8][2] # x and y cords of hand 0 pointer finger, refer to documentation to see how land marks are organized
        pointer_x2, pointer_y2 = lmList1[8][1], lmList1[8][2] # x and y cords of hand 1 pointer finger

        thumb_x1, thumb_y1 = lmList0[4][1], lmList0[4][2] # x and y cords of hand 0 thumb
        thumb_x2, thumb_y2 = lmList1[4][1], lmList1[4][2] # x and y cords of hand 1 thumb

        cx1, cy1 = (pointer_x1 + thumb_x1) // 2, (pointer_y1 + thumb_y1) // 2 # calculate the location of the center
        cx2, cy2 = (pointer_x2 + thumb_x2) // 2, (pointer_y2 + thumb_y2) // 2

        cv2.line(img, (cx1, cy1), (cx2, cy2), (255, 0, 255))

        length = math.hypot(cx2 - cx1, cy2 - cy1)
        print(length)
        # length = math.hypot(x2 - x1, y2 - y1) # calculate the length of the line
        # print(length)

        # if length < 50: # this value will need to be changed when the headset is created
        #     cv2.circle(img, (cx, cy), 15, (0, 255, 0), cv2.FILLED) # if the tips of the fingers are touching, change the colour (kind of like a button)


    cTime = time.time()

    fps = 1/(cTime-pTime) # calculate fps
    pTime = cTime # set pTime to cTime after fps is calcualated to avoid float division by zero error
 
    cv2.putText(img, f'FPS: {int(fps)}', (40, 50), cv2.FONT_HERSHEY_COMPLEX, 1, (255, 0, 0), 3) # writes the fps to screen. putText takes in paramaters, what window (img in this case), what to write (fps in this case), location on window, font (can make it whatever you want), size, colour, thickness
    
    cv2.imshow("Img", img)
    key = cv2.waitKey(1)
    if key == 27:
        break
