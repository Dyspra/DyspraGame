#include <Wire.h>
#include "Adafruit_DRV2605.h"
#include <SoftwareSerial.h>

#define RxD 8
#define TxD 7 

SoftwareSerial blueToothSerial(RxD,TxD);
Adafruit_DRV2605 drv;

void setup() {
  Serial.begin(9600, "COM6");
  pinMode(RxD, INPUT);
  pinMode(TxD, OUTPUT);
  if (! drv.begin()) {
    while (1) delay(10);
  }
  drv.selectLibrary(1);
  drv.setMode(DRV2605_MODE_INTTRIG); 
}

uint8_t effect = 1;

void loop() {
  char test = 0;
  if(blueToothSerial.read()>0){
    test = blueToothSerial.read();
    if (test > 1) {
      drv.setWaveform(0, effect);  // play effect 
      drv.setWaveform(1, 0);       // end waveform
      drv.go();
      delay(500);
      effect++;
      if (effect > 117) test = 0;
    } else if (test < 0) {

    } else {
      delay(500);
    }
  }
}
