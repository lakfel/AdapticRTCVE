 #include <Wire.h>
#include <Adafruit_BNO055.h>
#include <Adafruit_Sensor.h>
#include <utility/imumaths.h>
#include <EEPROM.h>
#include <Servo.h>

/* Set the delay between fresh samples */
#define BNO055_SAMPLERATE_DELAY_MS (100)

Adafruit_BNO055 bno = Adafruit_BNO055(55);

Servo myservo00;
Servo myservo01;
Servo myservo02;
Servo myservo03;
Servo myservo04;
Servo myservo05;
Servo myservo06;
Servo myservo07;
Servo myservo08;
Servo myservo09;
Servo myservo10;
Servo myservo11;

float pos00 = 90;
float pos01 = 90;
float pos02 = 90;
float pos03 = 90;
float pos04 = 90;
float pos05 = 90;

float maxAng;
//float pos06 = 90;
//float pos07 = 90;
//float pos08 = 90;
//float pos09 = 90;
//float pos10 = 90;
//float pos11 = 90;

float Aread00;
float Aread01;
float Aread02;
float Aread03;
float Aread04;
float Aread05;

bool detach04 = false;
bool detach06 = false;
/*
 * Read the actual action. -99 is the command for change mode
 */
float action;

const byte numChars = 64;
char receivedChars[numChars];
char tempChars[numChars];        // temporary array for use when parsing

// variables to hold the parsed data
char messageFromPC[numChars] = {0};

boolean newData = false;

int counter = 0;

float jointNumber;


/**
 * Main menu
 */

/**
 * Variable that selects the current mode
 * Mode 1 -> Moving joint separetly
 *    <Attach/Move 1 OR Detach 2,NumberMotor:0-11, Detach all 3, Angle if 1 Selected in par 1>
 * Mode 2 -> Pre set shapes
 * Mode 3 -> Moving motors
 */
 int mode;

void setup(void)
{
  //bno.begin(bno.OPERATION_MODE_IMUPLUS);

  Serial.begin(115200);
  //  while(!Serial || millis()<10000);
  //Serial.println("Orientation Sensor Test"); Serial.println("");

  /* Initialise the sensor */
  if (!bno.begin())
  {
    /* There was a problem detecting the BNO055 ... check your connections */
    Serial.print("Ooops, no BNO055 detected ... Check your wiring or I2C ADDR!");
    //while (1);
  }


  Aread00 = analogRead(A23);
  Aread01 = analogRead(A16);
  Aread02 = analogRead(A15);
  Aread03 = analogRead(A17);
  Aread04 = analogRead(A18);
  Aread05 = analogRead(A19);
  maxAng = 180;
  //angle = 90;
  jointNumber = -1;
  mode = 2;
  action = -1;
 // bno.setExtCrystalUse(true);
  detachJoint(1);
          detachJoint(2);
          detachJoint(3);
          detachJoint(4);
          detachJoint(5);
          detachJoint(6);
}





void loop() {
  //bno.setMode(bno.OPERATION_MODE_IMUPLUS);
  // put your main code here, to run repeatedly:
  String input = "";

  //imu::Quaternion quat = bno.getQuat();

  //uint8_t system, gyro, accel, mag;
  //system = gyro = accel = mag = 0;
  //bno.getCalibration(&system, &gyro, &accel, &mag);

  Aread00 = analogRead(A9);
  Aread01 = analogRead(A8);
  Aread02 = analogRead(A7);
  Aread03 = analogRead(A6);
  Aread04 = analogRead(A5);
  Aread05 = analogRead(A3);

  recvWithStartEndMarkers();
  if (newData == true) {
    strcpy(tempChars, receivedChars);
    // this temporary copy is necessary to protect the original data
    //   because strtok() used in parseData() replaces the commas with \0
    parseData();
    newData = false;
  }


  //Actions depend on the mode selected.
  if(action != -1) 
  {
    input = String(action);
    //myservo00.detach();
    //myservo00.write(action);
   // delay(500);
//myservo00.detach();
    //Serial.println("DON");
    //input = String(action);
    //Serial.println(input);
    
    if(action == -99)
    {
      mode = pos00;
    }
    else
    {
      if( mode == 1)
      {
        if(action ==1)
        {
          moveJoint(pos00, pos01);
        }
        else if(action ==2)
        {
          detachJoint(pos00);
        }
        else if(action ==3)
        {
          detachJoint(1);
          detachJoint(2);
          detachJoint(3);
          detachJoint(4);
          detachJoint(5);
          detachJoint(6);
        }
      }
      else if( mode == 2)
      {
        // FLAT
        if(action ==1)
        {
          presetFlat();
        }
        else if(action ==2)
        {
          presetCylinder();
        }
        else if(action ==3)
        {
          presetBook();
        }
        input += "OK";

        delay(1000);
        Serial.println(input);
        delay(1000);
        Serial.flush();
      }
      else if( mode == 3)
      {
        moveMotor(action, pos00  );
      }
      else if( mode == 4)
      {
        if(action ==1)
        {
          presetFlatSlow();
        }
        else if(action ==2)
        {
          presetCylinderSlow();
        }
        else if(action ==3)
        {
          presetBookSlow();
        }
        input = "OK";

        delay(1000);
        Serial.println(input);
        delay(1000);
        Serial.flush();
      }
    }
  }
  
  action = -1;
  pos00 = -1;
  pos01 = -1;
  pos02 = -1;
  pos03 = -1;
  
  //TESTING each motor
 /* if(jointNumber == -1)
  {
    if(bending >= 0)
    {
      jointNumber = bending;
      //myservo00.attach(jointNumber);
      bending = 0;
    }
  }
  else
  {
    if(bending >=0)
    {
      //myservo00.write(bending);
      moveJoint(jointNumber, bending);
    }
    else if ((bending == -1))
    {
      myservo00.detach();
      bending = -1;
      jointNumber = -1;
      
    }
    else
    {
      myservo00.detach();
      myservo01.detach();
      myservo02.detach();
      myservo03.detach();
      myservo04.detach();
      myservo05.detach();
      myservo05.detach();
      myservo06.detach();
      myservo07.detach();
      myservo08.detach();
      myservo09.detach();
      myservo10.detach();
      myservo11.detach();
    }
  }
*/
input += "&";

  input += String(Aread00);
  input += ",";
  input += String(Aread01);
  input += ",";
  input += String(Aread02);
  input += ",";
  input += String(Aread03);
  input += ",";
  input += String(Aread04);
  input += ",";
  input += String(Aread05);
  input += ",";

  /* Display the quat data */
/*  input += String(quat.w(), 4);
  input += ",";
  input += String(quat.y(), 4);
  input += ",";
  input += String(quat.x(), 4);
  input += ",";
  input += String(quat.z(), 4);
  input += ",";
*/
  /* Display the individual values */
/*  input += String(system, DEC);
  input += ",";
  input += String(gyro, DEC);
  input += ",";
  input += String(accel, DEC);
  input += ",";
  input += String(mag, DEC);
  input += ",";
  input += String(bending);
  input += ",";
  input += String(pos00);
  input += ",";
  input += String(pos01);
  input += ",";
  input += String(pos02);
  input += ",";
  input += String(pos03);
  input += ",";
  input += String(pos04);
  input += ",";
  input += String(pos05);
*/
  input += "$";

  Serial.println(input);
  delay(1000);
  Serial.flush();

}


/* Detach only one motor per joint, in order to mantain the adaptic rigid but avoiding consume too much current
 */
void detachHalf()
{
  myservo11.detach();
  myservo09.detach();
  myservo07.detach();
  myservo05.detach();
  myservo03.detach();
  myservo01.detach(); 
}

/**
 * Preset flat shape
 */
void presetFlat ()
{
  detachJoint(1);
  detachJoint(2);
  detachJoint(3);
  detachJoint(4);
  detachJoint(5);
  detachJoint(6);

  moveJoint(1,90);
  moveJoint(2,90);
 moveJoint(3,110);
  moveJoint(4,90);
  moveJoint(5,90);
  moveJoint(6,90);
  

  delay(700);
   detachHalf();
  detachJoint(1);
  detachJoint(2);
 detachJoint(3);
  detachJoint(4);
  detachJoint(5);
  detachJoint(6);
  
}

void presetFlatSlow ()
{
  detachJoint(1);
  detachJoint(2);
  detachJoint(3);
//detachJoint(4);
  detachJoint(5);
  detachJoint(6);

  moveJoint(1,90);
  moveJoint(2,90);
  delay(300);
  detachJoint(1);
  detachJoint(2);
  delay(600);
  moveJoint(3,90);
  moveJoint(5,90);
  delay(300);
  detachJoint(3);
 // moveJoint(4,90);
  detachJoint(5);
  delay(600);
  moveJoint(6,90);
  delay(300);
  detachJoint(6);
  

 /* delay(500);
   detachHalf();
  detachJoint(2);
 detachJoint(3);
 // detachJoint(4);
  detachJoint(5);
  detachJoint(6);*/
  
}

/**
 * Preset cylinder  shape
 */
void presetCylinder ()
{
  detachJoint(1);
  detachJoint(2);
  detachJoint(3);
  detachJoint(4);
  detachJoint(5);
  detachJoint(6);

  moveJoint(1,40);
  moveJoint(2,40);
  moveJoint(3,40);
  moveJoint(4,40);
  moveJoint(5,40);
  moveJoint(6,40);
  
  delay(1000);

  //detachHalf();
  
  detachJoint(1);
  detachJoint(2);
  detachJoint(3);
  detachJoint(4);
  detachJoint(5);
  detachJoint(6);
  
}

void presetCylinderSlow ()
{
  detachJoint(1);
  detachJoint(2);
  detachJoint(3);
 // detachJoint(4);
  detachJoint(5);
  detachJoint(6);

  moveJoint(1,45);
  moveJoint(2,45);
  //delay(300);
  //delay(900);
  moveJoint(3,45);
  
//  delay(300);

  //
  
 // delay(900);
  
  moveJoint(4,40);
 // delay(300);
  
  //delay(900);
  
  moveJoint(5,40);
  moveJoint(6,450);
  delay(700);

  //detachHalf();
  
  detachJoint(1);
  detachJoint(2);
  detachJoint(3);
  detachJoint(4);
  detachJoint(6);
  detachJoint(5);
  
  
}

/**
 * Preset book  shape
 */
void presetBook()
{
  detachJoint(1);
  detachJoint(2);
  detachJoint(3);
  detachJoint(4);
  detachJoint(5);
  detachJoint(6);

  moveJoint(1,90);
  moveJoint(2,90);
  moveJoint(3,0);
  moveJoint(4,0);
  moveJoint(5,90);
  moveJoint(6,110);

  delay(500);
 
  //detachHalf();
  
  detachJoint(1);
  detachJoint(2);
  detachJoint(3);
  detachJoint(4);
  detachJoint(5);
  detachJoint(6);
    
}

/**
 * Preset book  shape
 */
void presetBookSlow()
{
  detachJoint(1);
  detachJoint(2);
  detachJoint(3);
 // detachJoint(4);
  detachJoint(5);
  detachJoint(6);

  moveJoint(1,0);
  moveJoint(2,0);
  delay(300);
  detachJoint(1);
  detachJoint(2);
  delay(600);
  
  moveJoint(3,90);
  moveJoint(4,90);
  delay(300);
  detachJoint(3);
  detachJoint(4);
  delay(600);

  
  moveJoint(5,0);
  moveJoint(6,0);

  delay(500);
 
  //detachHalf();
  

  detachJoint(5);
  detachJoint(6);
    
}

void moveMotor(int motor, float angle)
{
  myservo00.detach();
  myservo00.attach(motor);
  //myservo00.write(min(max(angle,16),169));
  myservo00.write(angle);
  delay(1300);
  myservo00.detach();
}

void detachJoint(int joint)
{
  if(joint == 1)
  {
    myservo00.detach();
    myservo01.detach();
  }
  if(joint == 2)
  {
    myservo02.detach();
    myservo03.detach();
  }
  if(joint == 3)
  {
    myservo04.detach();
    myservo05.detach();
  }
  if(joint == 4)
  {
    myservo06.detach();
    myservo07.detach();
  }
  if(joint == 5)
  {
    myservo08.detach();
    myservo09.detach();
  }
  if(joint == 6)
  {
    
    myservo10.detach();
    myservo11.detach();
  }
}

void moveJoint(int joint, float angle)
{
  if(joint == 1)
  {
    myservo00.attach(0);
    myservo01.attach(1);
    //myservo0.write(min(max(180 - angle,16),169));
    //myservo1.write(min(max(angle,16),169));
    myservo00.write(maxAng - angle);
    myservo01.write( angle);
  }
  if(joint == 2)
  {
    myservo02.attach(2);
    myservo03.attach(3);
    myservo02.write(maxAng - angle);
    myservo03.write(maxAng - angle);
  }
  if(joint == 3)
  {
    myservo04.attach(4);
    myservo05.attach(5);
    myservo04.write(maxAng - angle);
    myservo05.write(maxAng - angle);
  }
  if(joint == 4)
  {
    myservo06.attach(6);
    myservo07.attach(7);
    myservo06.write(angle);
    myservo07.write(angle);
  }
  if(joint == 5)
  {
    myservo08.attach(8);
    myservo09.attach(9);  
    myservo08.write(angle);
    myservo09.write(angle);
  }
  if(joint == 6)
  {
    
    myservo10.attach(10);
    myservo11.attach(11);
    myservo10.write(angle);
    myservo11.write(maxAng - angle);
  }
}


void recvWithStartEndMarkers() {
  static boolean recvInProgress = false;
  static byte ndx = 0;
  char startMarker = '<';
  char endMarker = '>';
  char rc;

  while (Serial.available() > 0 && newData == false) {
    rc = Serial.read();

    if (recvInProgress == true) {
      if (rc != endMarker) {
        receivedChars[ndx] = rc;
        ndx++;
        if (ndx >= numChars) {
          ndx = numChars - 1;
        }
      }
      else {
        receivedChars[ndx] = '\0'; // terminate the string
        recvInProgress = false;
        ndx = 0;
        newData = true;
      }
    }

    else if (rc == startMarker) {
      recvInProgress = true;
    }
  }
}


void parseData() {      // split the data into its parts

  char * strtokIndx; // this is used by strtok() as an index

  strtokIndx = strtok(tempChars, ","); // this continues where the previous call left off
  action = atoi(strtokIndx);     // convert this part to an integer

  strtokIndx = strtok(NULL, ",");
  pos00 = atof(strtokIndx);     // convert this part to a float

  strtokIndx = strtok(NULL, ",");
  pos01 = atof(strtokIndx);     // convert this part to a float

  strtokIndx = strtok(NULL, ",");
  pos02 = atof(strtokIndx);     // convert this part to a float

  strtokIndx = strtok(NULL, ",");
  pos03 = atof(strtokIndx);     // convert this part to a float

  strtokIndx = strtok(NULL, ",");
  pos04 = atof(strtokIndx);     // convert this part to a float

  strtokIndx = strtok(NULL, ",");
  pos05 = atof(strtokIndx);     // convert this part to a float
}
