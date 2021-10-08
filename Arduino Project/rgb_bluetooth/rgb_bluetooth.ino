#include <SoftwareSerial.h>
int red_light_pin= 11;
int green_light_pin = 10;
int blue_light_pin = 9;
int led = 13;
SoftwareSerial btSerial(2, 3);
int rComponent;
int gComponent;
int bComponent;

void readColorFromBT()
{
  parseStringToInt(rComponent);
  parseStringToInt(gComponent);
  parseStringToInt(bComponent);
  RGB_color();
}
void StopListenMessageFromAndroid()
{  
  btSerial.println("Complite! " + String(rComponent) + "|" + String(gComponent) + "|" + String(bComponent));
  delay(500);
  digitalWrite(led, LOW);
}
void parseStringToInt(int &value)
{
  value = 0;
  for (int i = 0; i < 3;)
  {
    if (btSerial.available() > 0)
    {
      char symbol = btSerial.read();
      if (symbol >= '0' && symbol <= '9')
      {
        int insertValue = (int)symbol - '0';
        if (i == 2 && value >= 10) insertValue++;
        value = value + insertValue * pow(10, 2 - i);
        i++;
      }
      delay(10);
    }
  }
}
void RGB_color()
 {
  analogWrite(red_light_pin, rComponent);
  analogWrite(green_light_pin, gComponent);
  analogWrite(blue_light_pin, bComponent);
}
void setup()
{
  pinMode(red_light_pin, OUTPUT);
  pinMode(green_light_pin, OUTPUT);
  pinMode(blue_light_pin, OUTPUT);
  Serial.begin(9600);
  btSerial.begin(9600);
  pinMode(led, OUTPUT);
}
void loop()
{
  if (btSerial.available() > 0)
  {
    char state = btSerial.read();
    if (state == 'C')
    {
      digitalWrite(led, HIGH);
      readColorFromBT();
      StopListenMessageFromAndroid();
    }
    else if (state == 'R')
    {
      digitalWrite(led, HIGH);
      parseStringToInt(rComponent);
      RGB_color();
      StopListenMessageFromAndroid();
    }
    else if (state == 'G')
    {
      digitalWrite(led, HIGH);
      parseStringToInt(gComponent);
      RGB_color();
      StopListenMessageFromAndroid();
    }
    else if (state == 'B')
    {
      digitalWrite(led, HIGH);
      parseStringToInt(bComponent);
      RGB_color();
      StopListenMessageFromAndroid();
    }
  }
}
