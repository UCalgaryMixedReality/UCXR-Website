import numpy as np
import math
import cv2
import time

def zoom(img, lmList0, lmList1):
    # print("lmList0 = ")
    # print(lmList0)

    # print("lmList1 = ")
    # print(lmList1)

    if len(lmList0) == 0 or len(lmList1) == 0:
        print("one of the lmLists is empty")
        yield

    elif len(lmList0[0]) >= 8 and len(lmList1[0]) >= 8:
        try:

            # find coordinates of pointer and thumbs of each hand
            pointer_x1, pointer_y1 = lmList0[0][8][0], lmList0[0][8][1]
            pointer_x2, pointer_y2 = lmList1[0][8][0], lmList1[0][8][1]
            thumb_x1, thumb_y1 = lmList0[0][4][0], lmList0[0][4][1]
            thumb_x2, thumb_y2 = lmList1[0][4][0], lmList1[0][4][1]

            # calculate center point
            cx1, cy1 = (pointer_x1 + thumb_x1) // 2, (pointer_y1 + thumb_y1) // 2
            cx2, cy2 = (pointer_x2 + thumb_x2) // 2, (pointer_y2 + thumb_y2) // 2

            cv2.line(img, (cx1, cy1), (cx2, cy2), (255, 255, 255))

            # calculate the length of the line
            length = math.hypot(cx2 - cx1, cy2 - cy1)
            yield length

        except IndexError:
            print("Index error here")

    elif len(lmList0) != len(lmList1):
        print("The lists are not equal in length")

    else:
        print("Unknown error")