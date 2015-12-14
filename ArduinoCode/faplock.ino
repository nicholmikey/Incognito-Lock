
#include <Servo.h>
#define BAUD_RATE 9600

Servo myservo;  // create servo object to control a servo
// twelve servo objects can be created on most boards

int pos = 0;    // variable to store the servo position
int message = 0;
bool locked = false;
void setup() {
  myservo.attach(9);  // attaches the servo on pin 9 to the servo object
  Serial.begin(BAUD_RATE);
}



void loop() {


 if (Serial.available() > 0) { // Check to see if there is a new message
    message = Serial.read(); // Put the serial input into the message

    if (message == 'l' && locked == false) { 

   for (pos = 160; pos >= 101; pos -= 1) { // goes from 0 degrees to 180 degrees
       myservo.write(pos);              // tell servo to go to position in variable 'pos'
       delay(5);                       // waits 15ms for the servo to reach the position
   }
   locked = true;

    }

    if (message == 'u' && locked == true) {
   for (pos = 101; pos <=160; pos += 1) { // goes from 0 degrees to 180 degrees
       myservo.write(pos);              // tell servo to go to position in variable 'pos'
        delay(5);                       // waits 15ms for the servo to reach the position
    }
    locked=false;
    }
  }

}

