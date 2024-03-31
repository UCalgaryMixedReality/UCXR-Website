def zoom():
    import HandTrack as ht
    # import HandTrack as ht
    import cv2
    import mediapipe as mp
    import time
    import numpy as np
    import math

    cap = cv2.VideoCapture(1) # change integer if u get an error, basically changes what camera to use. Built in cameras for laptops are usually defaulted by 0. External cameras are usually 1.
    
    detector = ht.HandDetector(detectionConfidence=0.7, maxHands=2)
    pTime = 1

    while True:
        success, img = cap.read()
        img = detector.findHands(img)
        # img = cv2.flip(img, 1) # UNCOMMENT THIS IF YOU GET ANNOYED BY THE FLIPPED VIDEO, WILL MESS UP THE DRAW LINE FEATURE THO
        lmList0 = detector.findPosition(img, draw=False, handNo=0) 
        lmList1 = detector.findPosition(img, draw=False, handNo=1)

        if len(lmList0) != 0 and len(lmList1) != 0:
            pointer_x1, pointer_y1 = lmList0[8][1], lmList0[8][2]
            pointer_x2, pointer_y2 = lmList1[8][1], lmList1[8][2]

            thumb_x1, thumb_y1 = lmList0[4][1], lmList0[4][2] 
            thumb_x2, thumb_y2 = lmList1[4][1], lmList1[4][2]

            cx1, cy1 = (pointer_x1 + thumb_x1) // 2, (pointer_y1 + thumb_y1) // 2
            cx2, cy2 = (pointer_x2 + thumb_x2) // 2, (pointer_y2 + thumb_y2) // 2

            cv2.line(img, (cx1, cy1), (cx2, cy2), (255, 255, 255))

            length = math.hypot(cx2 - cx1, cy2 - cy1)
            yield length

        cTime = time.time()
        fps = 1/(cTime-pTime)
        pTime = cTime

        # cv2.putText(img, f'FPS: {int(fps)}', (40, 50), cv2.FONT_HERSHEY_COMPLEX, 1, (255, 0, 0), 3)
        cv2.imshow("Img", img)
        key = cv2.waitKey(1)
        if key == 27:
            break

# for length in zoom():
#     print(length)
