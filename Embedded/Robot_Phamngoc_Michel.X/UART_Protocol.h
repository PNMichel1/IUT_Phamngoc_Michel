/* 
 * File:   UART_Protocol.h
 * Author: E306-PC2
 *
 * Created on 19 décembre 2025, 08:12
 */

#ifndef UART_PROTOCOL_H
#define	UART_PROTOCOL_H

unsigned char UartCalculateChecksum(int msgFunction,
int msgPayloadLength, unsigned char* msgPayload);
void UartEncodeAndSendMessage(int msgFunction,
int msgPayloadLength, unsigned char* msgPayload);
void UartDecodeMessage(unsigned char c);
void UartProcessDecodedMessage(int function,
int payloadLength, unsigned char* payload);

#define Waiting 0
#define FunctionMSB 1
#define FunctionLSB  2
#define PayloadLengthMSB  3 
#define PayloadLengthLSB  4
#define Payload 5
#define CheckSum 6

#ifdef	__cplusplus
extern "C" {
#endif




#ifdef	__cplusplus
}
#endif

#endif	/* UART_PROTOCOL_H */

