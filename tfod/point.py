import HandTrack as ht
import cv2
import mediapipe as mp
import time
import numpy as np
import math

def point(img, lmList0, lmList1):
    

    if len(lmList0) == 0 and len(lmList1) == 0:
        print("Both lists recieved contain no values")
        yield
    try:

        if len(lmList1) != 0 and len(lmList0) != 0: # Case 1: Both hands are visible
            x1, y1, x2, y2 = lmList0[0][8][0], lmList0[0][8][1], lmList0[0][8][0], lmList0[0][8][1]
            yield [x1, y1, x2, y2]

        # Case 2: Only One hand is visible

        if len(lmList0) > 0:
            x, y = lmList0[0][8][0], lmList0[0][8][1]
            yield [x, y]

        elif  len(lmList1) > 0:
            x, y = lmList1[0][8][0], lmList1[0][8][1]
            yield [x, y]

        else:
            print("Unknown error")
    
    except IndexError:
        print("Index error here")
        print("Current List:\n")
        print(lmList0, lmList1)