
#define ERR		-1
#define OK		0

#define PAC_ADDRESS	1021

#define ENQ  0x05//请求连接通信线路(询问).
#define ACK  0x06//确认(握手).
#define NAK  0x15//通信忙.

HANDLE APIENTRY CRT720ROpen(char *Port);
HANDLE APIENTRY CRT720ROpenWithBaut(char *Port, unsigned int Baudrate);
int APIENTRY CRT720RClose(HANDLE ComHandle);

/* Data structure for reply message	
	//replyType=0x50     PositiveReply
	//replyType=0x4e     NegativeReply   
	//replyType=0x10     ReplyReceivingFailure
	//replyType=0x20     CommandCancellation
	//replyType=0x30     ReplyTimeout
*/
int APIENTRY RS232_ExeCommand(HANDLE ComHandle,BYTE TxAddr,BYTE TxCmCode,BYTE TxPmCode,int TxDataLen,BYTE TxData[],BYTE *RxReplyType,BYTE *RxStCode0,BYTE *RxStCode1,BYTE *RxStCode2,int *RxDataLen,BYTE RxData[]);

int APIENTRY InitializeContext(int *ReaderCount);
int APIENTRY GetSCardReaderName(int ReaderSort,int *RxDataLen,BYTE RxData[]);
int APIENTRY GetStatusChange(int ReaderSort);
int APIENTRY ConnectSCardReader(int ReaderSort); 
int APIENTRY GetSCardReaderStatus(int ReaderSort,char ReaderName[],int *ReaderNameLen,BYTE *CardState,BYTE *CardProtocol,BYTE ATR_Data[],int *ATR_DataLen);
int APIENTRY APDU_Transmit(int ReaderSort,int TxDataLen,BYTE TxData[],int *RxDataLen,BYTE RxData[]);
int APIENTRY Extended_Transmit(int ReaderSort,int TxDataLen,BYTE TxData[],int *RxDataLen,BYTE RxData[]);
int APIENTRY DisconnectSCardReader(int ReaderSort); 
int APIENTRY ReleaseContext(); 
int APIENTRY GetSCardNuber(int ReaderSort,int *RxDataLen,BYTE RxData[]);
