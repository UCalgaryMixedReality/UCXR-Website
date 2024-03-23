#This file is the control algorithm that collects data from the IMU and Time-Of-Flight sensors

#Sourced from:
#1. Adafruit Industries: https://github.com/adafruit/Adafruit_BNO055
#2. Kevin Mc Aleer: https://github.com/kevinmcaleer/vl53l0x

# Copyright (c) 2024 Toluwanimi Oladiti for Augmented Reality medicine.

import machine
from machine import I2C,Pin
import time
from bno055 import *
from vl53l0x import VL53L0X

#i2c_imu = machine.I2C(0, sda=machine.Pin(4), scl=machine.Pin(5))  # EIO error almost immediately
i2cImu = machine.SoftI2C(sda=machine.Pin(4), scl=machine.Pin(5), timeout=1_000)
#i2c_tof = machine.I2C(1, sda=machine.Pin(14), scl=machine.Pin(15))
i2cTof = machine.SoftI2C(sda=machine.Pin(14), scl=machine.Pin(15), timeout=1_000)
print(i2cImu.scan())
print(i2cTof.scan())
print("Version1.0")

time.sleep(1)
imu = BNO055_BASE(i2cImu)
tof = VL53L0X(i2cTof)


##ToF Setup
def setupTof():
    budget = tof.measurement_timing_budget_us
    print("Budget was:", budget)
    tof.set_measurement_timing_budget(40000)
    tof.set_Vcsel_pulse_period(tof.vcsel_period_type[0], 12)
    tof.set_Vcsel_pulse_period(tof.vcsel_period_type[1], 8)
    

setupTof()

calibrated = False
while True:
    time.sleep(1)
    if not calibrated:
        calibrated = imu.calibrated()
        print('Calibration required: sys {} gyro {} accel {} mag {}'.format(*imu.cal_status()))
    print('Temperature {}°C'.format(imu.temperature()))
    print('Mag       x {:5.0f}    y {:5.0f}     z {:5.0f}'.format(*imu.mag()))
    print('Gyro      x {:5.0f}    y {:5.0f}     z {:5.0f}'.format(*imu.gyro()))
    print('Accel     x {:5.1f}    y {:5.1f}     z {:5.1f}'.format(*imu.accel()))
    print('Lin acc.  x {:5.1f}    y {:5.1f}     z {:5.1f}'.format(*imu.lin_acc()))
    print('Gravity   x {:5.1f}    y {:5.1f}     z {:5.1f}'.format(*imu.gravity()))
    print('Heading     {:4.0f} roll {:4.0f} pitch {:4.0f}'.format(*imu.euler()))
    print("Time-of-Flight distnace reading: ", tof.ping()-50, "mm")

