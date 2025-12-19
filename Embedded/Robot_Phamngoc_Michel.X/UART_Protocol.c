#include <xc.h>
#include "UART_Protocol.h"
#include "IO.h"
#include "PWM.h"
#include "CB_TX1.h"

unsigned char UartCalculateChecksum(int msgFunction,
int msgPayloadLength, unsigned char* msgPayload)
{
//Fonction prenant entree la trame et sa longueur pour calculer le checksum
  unsigned char checksum = 0x00;


  checksum ^= 0xFE;

  checksum ^= 0x00;
  checksum ^= msgFunction;

  checksum ^= ((msgPayloadLength >> 8) & 0xFF);
  checksum ^= (msgPayloadLength & 0xFF);

  for (int i = 0; i < msgPayloadLength; i++)
  {
      checksum ^= msgPayload[i];
 
  }

  return checksum;

}
void UartEncodeAndSendMessage(int msgFunction,
int msgPayloadLength, unsigned char* msgPayload)
{
//Fonction d?encodage et d?envoi d?un message
 unsigned char trame[msgPayloadLength+6];
 int a = 0;
 trame[0] = 0xFE;
 trame[1] = 0x00;
 trame[2] = (msgFunction);
 trame[3] = (msgPayloadLength >> 8);
 trame[4] = (msgPayloadLength);

 for (int i = 0; i < msgPayloadLength; i++)
 {
     trame[5 + i] = (unsigned)(msgPayload[i]);
     a++;
 }

 trame[5 + a] = UartCalculateChecksum(msgFunction, msgPayloadLength, msgPayload);
 SendMessage(trame,msgPayloadLength+6);
}

int msgDecodedFunction = 0;
int msgDecodedPayloadLength = 0;
unsigned char msgDecodedPayload[128];
int msgDecodedPayloadIndex = 0;
int receivedChecksum, calculatedChecksum= 0x00;
int rcvState=0;
void UartDecodeMessage(unsigned char c)
{
   
//Fonction prenant en entree un octet et servant a reconstituer les trames
int  b=0;           
           
switch(rcvState)
{
case Waiting:
    
    if (c == 0xFE)
        rcvState = FunctionMSB;


break;
    case FunctionMSB:
                   // if (msgDecodedPayload == "00")
                   //c byte or 00 string
                        if (c == 0x00)
                        {
                  
                        rcvState = FunctionLSB;
                    } else
                        rcvState = Waiting;



break;
case FunctionLSB:
                    msgDecodedFunction = c;
                
                    rcvState = PayloadLengthMSB;
                    break;
case PayloadLengthMSB:
                    msgDecodedPayloadLength = (c<<8);
                    rcvState= PayloadLengthLSB;



break;
case PayloadLengthLSB:
                    msgDecodedPayloadLength +=c;
                    msgDecodedPayload[msgDecodedPayloadLength];
                    rcvState = Payload;
                    break;
case Payload:

                    msgDecodedPayload[msgDecodedPayloadIndex] += c;
                    msgDecodedPayloadIndex++;
                    if (msgDecodedPayloadIndex >= msgDecodedPayloadLength)
                    {
                        rcvState = CheckSum;
                        msgDecodedPayloadIndex = 0;

                    }

                       

                    
break;
case CheckSum:
                
                    
                    receivedChecksum = c;
                    calculatedChecksum = UartCalculateChecksum(msgDecodedFunction, msgDecodedPayloadLength,msgDecodedPayload);

                    if (calculatedChecksum == receivedChecksum)
                    {
                       
                    }
                    else
                        b = 0;
rcvState = Waiting;
break;
default:
rcvState = Waiting;
break;
}
    
    
    
    
    
    
    
}
    
void UartProcessDecodedMessage(int function,
int payloadLength, unsigned char* payload)
{
//Fonction appelee apres le decodage pour executer l?action
//correspondant au message recu

   switch(function)
 {
     case 0x0020:
         if (payload[0]=='1'){
               if (payload[1]=='1')
                 LED_BLANCHE_1=1;
             else
                 LED_BLANCHE_1=0;
         }
           
        if (payload[0] == '2'){
                if (payload[1] == '1')
                LED_BLEUE_1=1;
             else
                LED_BLEUE_1=0;
        }
         
        if (payload[0] == '3'){
             if (payload[1] == '1')
                LED_ORANGE_1=1;
            else
                LED_ORANGE_1=0;
        }
            
        if (payload[0] == '4'){
             if (payload[1] == '1')
                LED_ROUGE_1=1;
             else
                LED_ROUGE_1=0;
        }
           
        if (payload[0] == '5'){
             if (payload[1] == '1')
                LED_VERTE_1=1;
            else
             LED_VERTE_1=0;
        }
           
       
         break;

  
     case 0x0040:
         PWMSetSpeedConsigne(payload[0], MOTEUR_DROIT);
         PWMSetSpeedConsigne(payload[1], MOTEUR_GAUCHE);
         break;



 }



  
}
//
//Fonctions correspondant aux messages
//
