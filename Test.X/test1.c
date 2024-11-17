/*
 * File:   IMU_Firmware_Setup.c
 * Author: Nicholas Plowman
 *
 * Created on September 9, 2024, 7:06 PM
 */

#include <xc.h>
#include <stdint.h>

#define FCY 40000000UL        // Define instruction cycle frequency (40 MHz example)
#define FSCL 100000           // I2C clock speed (100 kHz)
#define BNO055_ADDR 0x28      // BNO055 I2C Address (0x28 or 0x29 depending on ADR pin)

// Function Prototypes
void I2C_Init(void);
void I2C_Start(void);
void I2C_Stop(void);
void I2C_Write(uint8_t data);
uint8_t I2C_Read(uint8_t ack);
uint8_t I2C_ReadRegister(uint8_t deviceAddr, uint8_t regAddr);
void LED_Init(void);
void LED_On(void);
void LED_Off(void);

int main(void)
{
    I2C_Init();    // Initialize I2C
    LED_Init();    // Initialize LED pin

    // Read the CHIP_ID register (0x00) from the BNO055
    uint8_t chip_id = I2C_ReadRegister(BNO055_ADDR, 0x00);

    // If the CHIP_ID matches 0xA0, light up the LED
    if (chip_id == 0xA0) {
        LED_On();
    } else {
        LED_Off();  // Otherwise, keep the LED off
    }

    while (1);
    return 0;
}

// I2C Functions
void I2C_Init(void)
{
    I2C1BRG = ((FCY / FSCL) - FCY / 10000000) - 1;  // Set I2C clock speed
    I2C1CONLbits.I2CEN = 1;                        // Enable I2C
}

void I2C_Start(void)
{
    I2C1CONLbits.SEN = 1;      // Initiate start condition
    while (I2C1CONLbits.SEN);  // Wait for start condition to complete
}

void I2C_Stop(void)
{
    I2C1CONLbits.PEN = 1;      // Initiate stop condition
    while (I2C1CONLbits.PEN);  // Wait for stop condition to complete
}

void I2C_Write(uint8_t data)
{
    I2C1TRN = data;            // Load data into transmit register
    while (I2C1STATbits.TBF);  // Wait for transmission to complete
    while (I2C1STATbits.ACKSTAT);  // Wait for acknowledgment
}

uint8_t I2C_Read(uint8_t ack)
{
    I2C1CONLbits.RCEN = 1;      // Enable receive mode
    while (!I2C1STATbits.RBF);  // Wait for data to be received
    uint8_t receivedData = I2C1RCV;  // Read the received data

    I2C1CONLbits.ACKDT = (ack) ? 0 : 1;  // Send acknowledgment bit
    I2C1CONLbits.ACKEN = 1;              // Acknowledge sequence enabled
    while (I2C1CONLbits.ACKEN);          // Wait for ACK to complete
    return receivedData;
}

uint8_t I2C_ReadRegister(uint8_t deviceAddr, uint8_t regAddr)
{
    I2C_Start();
    I2C_Write(deviceAddr << 1);    // Send device address with write bit
    I2C_Write(regAddr);            // Send register address
    I2C_Start();                   // Repeat start condition
    I2C_Write((deviceAddr << 1) | 1);  // Send device address with read bit
    uint8_t data = I2C_Read(0);    // Read the register data with NACK
    I2C_Stop();
    return data;
}

// LED Functions
void LED_Init(void)
{
    TRISBbits.TRISB0 = 0;  // Set RB0 as output (assuming LED is connected to RB0)
    LATBbits.LATB0 = 0;    // Start with the LED off
}

void LED_On(void)
{
    LATBbits.LATB0 = 1;    // Set RB0 to high to turn on the LED
}

void LED_Off(void)
{
    LATBbits.LATB0 = 0;    // Set RB0 to low to turn off the LED
}




