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

    elif len(lmList0[0]) >= 8:
        try:

            x, y = lmList0[0][8][0], lmList0[0][8][1]
            yield [x, y]

        except IndexError:
            print("Index error here")

    elif  len(lmList1[0]) >= 8:
        try:
            x, y = lmList0[0][8][0], lmList0[0][8][1]
            yield [x, y]

        except IndexError:
            print("Index error here")
    
    else:
        print("Unknown error")