#This file is intended as an initial test to ensure the Raspberry Pi Pico is functional
#Led2 is the built-in led of the Raspberry Pi Pico
#Led1 should be connected to GPIO1 of the Pico and ground to the Pico.


import machine
import time

led1 = machine.Pin(1, machine.Pin.OUT)
led2 = machine.Pin(25, machine.Pin.OUT)

while True:
    led1.toggle()
    led2.toggle()
    time.sleep(0.25)
