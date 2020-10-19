// CRT_603_CZ7.h : CRT_603_CZ7 DLL ����ͷ�ļ�
/** *************************************************************************************
 *  @file     	CRT_603_CZ7 DLL
 *  @brief      CRT_603_CZ7��̬��
 *  @details 	�ṩCRT_603_CZ7 PCSC��̬�⼰CRT_603_CZ7 ͨѶЭ���µ������
 *            	1.  
 *				2.
 *				3.
 *  @copyright 	CRT_603_CZ7
 *  @author    	LW <lw157851564@163.com>
 *  @version  	1.0.1
 *  @date      	12/1/2015
 *  @pre      	...
 *  @bug       	...   
 *  @warning   	...     
 ** *************************************************************************************/

#pragma once

//����֤��Ϣ
typedef struct
{
	char szName[31];   //����
	char szSex[3];     //�Ա�
	char szNation[20]; //����
	char szBornDay[10]; //����
	char szAddress[128]; //��ַ
	char szIDNum[20];   //���֤���
	char szIssued[31];  //ǩ������
	char szBeginValidity[10]; //��ʼ��Ч����
	char szEndValidity[10];   //��ֹ��Ч����
	char szImgPath[256];      //ͷ�ļ�·��
}CRTDef_IDInfo, *pCRTDef_IDInf;

//��������þ���֤��Ϣ
typedef struct  
{
	char szEnName[128];  // Ӣ����
	char szSex[3];       // �Ա�
	char szIDNum[31];    // ���þ���֤����
	char szAddress[20];  // ���������ڵ�������
	char szCnName[31];   // ������
	char szBeginValidity[16];   // ֤��ǩ������
	char szEndValidity[16];     // ֤����ֹ����
	char szBornDay[16];         // ��������
	char szVersion[5];          // ֤���汾��
	char szIssued[8];           // ����������ش���
	char szType[2];             // ֤�����ͱ�ʶ
	char szImgPath[256];        // ͷ��·��
}CRTDef_PRTDInfo, *pCRTDef_PRTDInfo;



// ��ѯ����
#define REQUEST_CONTSCT_ICC       0x0002    // �� �� �� �� ʽ IC ��
#define REQUEST_RF_ICC            0x0004    // �� �� �� �� �� ʽ RF ��
#define REQUEST_MAG               0x0008    // �� �� �� �� ��
#define REQUEST_ID_ICC            0x0010    // �� �� ����֤
#define REQUEST_EMID              0x0020    // �� �� EMID

#define REQUEST_ERR_CANCEL        -4     // ȡ������
#define REQUEST_ERR_TIMEOUT       -10    // ��ʱ����
#define REQUEST_ERR_OTHER         -99    // ��������

//����Ѱ��������Ϣ
struct MagTracks //��������Ϣ
{
	char szTrack1[512];
	char szTrack2[512];
	char szTrack3[512];
}; 

struct Icc_atr
{
	int iAtrlen;
	unsigned char ucAtrdata[128];
};

typedef union request_recv_data
{
	struct MagTracks  magtracks;
	struct Icc_atr   icc_atr;
}Request_recv_data;


#ifdef __cplusplus
extern "C" {
#endif


/** 
 * @fn		CRT_OpenConnect 
 * @detail	��CRT���ܶ�����
 * @see		...
 * @param	iListNums:��� ���ӵĶ������б����
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_OpenConnect(int* iListNums);




/** 
 * @fn		CRT_CloseConnect 
 * @detail	�ر�CRT���ܶ�����
 * @see		...
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_CloseConnect();




/** 
 * @fn		CRT_GetReaderListName 
 * @detail	��ȡ���ܶ������б�����
 * @see		...
 * @param	iListNum:���ȡ�Ķ��������кţ�0Ϊ��ʼ��
 * @param	szListName:��� ��ȡ���Ķ���������
 * @return	0 �ɹ��� ��0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetReaderListName(int iListNum, char szListName[]);




/** 
 * @fn		CRT_SetReaderName 
 * @detail	���õ�ǰ�����Ķ��������кţ���ʱ��Ĭ������Ϊ��һ����������
 * @see		...
 * @param	iListNum:�����õĶ��������кţ�0Ϊ��ʼ��
 * @return	0 �ɹ��� ��0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SetReaderName(int iListNum);




/** 
 * @fn		CRT_GetCardStatus 
 * @detail	��ȡ�������Ͽ�״̬
 * @see		...
 * @return	1 �п��� 2 �޿��� 9 ״̬δ֪
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetCardStatus();




/** 
 * @fn		CRT_EjectCard 
 * @detail	�����µ磬�������뿨Ƭ�Ͽ�����
 * @see		...
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_EjectCard();




/** 
 * @fn		CRT_ReaderConnect 
 * @detail	��������Ƭ�����ϵ�
 * @see		...
 * @param	byAtrData:��� �ϵ�ɹ����ص�ATR����
 * @param	iAtrLen: ��� �ϵ�ɹ����ص�ATR���ݳ���
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_ReaderConnect(BYTE byAtrData[], int* iAtrLen);




/** 
 * @fn		CRT_ChipPower 
 * @detail	�������Կ�Ƭ�����µ����
 * @see		...
 * @param	wChipPower:0x0002 �临λ�� 0x0004 �ȸ�λ�� 0x0008 �µ�
 * @param	byAtrData:��� �ϵ�ɹ����ص�ATR����
 * @param	iAtrLen: ��� �ϵ�ɹ����ص�ATR���ݳ���
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_ChipPower(WORD wChipPower, BYTE byAtrData[], int* iAtrLen);




/** 
 * @fn		CRT_SendApdu 
 * @detail	����������APDUָ��
 * @see		...
 * @param	bySendData:�跢�͵�APDUָ�֧��acsii����BCD�룩
 * @param	iSendDataLen: �跢�͵�APDUָ���
 * @param	byRecvData:��� APDUͨѶ�󷵻ص�����
 * @param	iRecvDataLen: ��� APDUͨѶ�󷵻ص����ݳ���
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SendApdu(BYTE bySendData[], int iSendDataLen, BYTE byRecvData[], int* iRecvDataLen);




/** 
 * @fn		CRT_SendControlCMD 
 * @detail	������������չ��������
 * @see		...
 * @param	bySendData:�跢�͵���չ�����������ݣ�֧��acsii����BCD�룩
 * @param	iSendDataLen: �跢�͵���չ�����������ݳ���
 * @param	byRecvData:��� ��չ��������ͨѶ�󷵻ص�����
 * @param	iRecvDataLen: ��� ��չ��������ͨѶ�󷵻ص����ݳ���
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SendControlCMD(BYTE bySendData[], int iSendDataLen, BYTE byRecvData[], int* iRecvDataLen);




//***********************����CRT603���幦����չ����****************************
/** 
 * @fn		CRT_HotReset 
 * @detail	�������ȸ�λ
 * @see		...
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_HotReset();




/** 
 * @fn		CRT_SetReaderType 
 * @detail	���ö���������ģʽ
 * @see		...
 * @param	iType: 1 ����RF��ģʽ�� 2 Felicaģʽ�� 3 ��Ե�ģʽ�� 4 ����֤ģʽ�� 5 ��ģ��ģʽ
  * @param	iDelayTime(ms): ����ִ�к���ʱʱ�䣬ģʽ�趨���֮����Щ�̼���Ҫ��ʱ��Ĭ��300ms 
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SetReaderType(int iType, int iDelayTime=100);




/** 
 * @fn		CRT_GetReaderType 
 * @detail	��ȡ����������ģʽ
 * @see		...
 * @return	>0 ʧ�� 1 ����RF��ģʽ�� 2 Felicaģʽ�� 3 ��Ե�ģʽ�� 4 ����֤ģʽ�� 5 ��ģ��ģʽ
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetReaderType();




/** 
 * @fn		CRT_GetVersionInfo 
 * @detail	��ȡ�������汾��Ϣ
 * @see		...
 * @param	iVerType: ���ȡ�İ汾 0 P/N��Ϣ�� 1 SN��Ϣ�� 2, �̼��汾��Ϣ�� 3 ���ɰ汾��Ϣ�� 4 EMID��Ϣ�� 5 ��̬��汾��Ϣ, 6 �豸���ͺ�
 * @param	szVersionInfo: ��� ���صİ汾��Ϣ
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetVersionInfo(int iVerType, char szVersionInfo[]);




/** 
 * @fn		CRT_AutoBeel 
 * @detail	�������Զ�����
 * @see		...
 * @param	bAutoBeel: �Ƿ��Զ�������0 �رգ�1 ������2 �ƿ��Զ�����
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_AutoBeel(int bAutoBeel);




/** 
 * @fn		CRT_Beel 
 * @detail	��������������
 * @see		...
 * @param	MultipleTime: ����ʱ�䣺0.25��ı���������2������ʱ�䣺2*0.25 ��0.5�루0-20 Ĭ��Ϊ1)
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_Beel(int MultipleTime = 1);




/** 
 * @fn		CRT_SetLightMode 
 * @detail	���ö�����ָʾ��ģʽ
 * @see		...
 * @param	iType: ָʾ��ģʽ 0 �Զ�ģʽ�� 1 �ֶ�ģʽ
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SetLightMode(int iType);




/** 
 * @fn		CRT_GetLightMode 
 * @detail	��ȡ������ָʾ��ģʽ
 * @see		...
 * @return	0 �Զ�ģʽ��1 �ֶ�ģʽ�� 9ģʽδ֪
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetLightMode();




/** 
 * @fn		CRT_SetLightStatus 
 * @detail	���ö�����ָʾ��״̬
 * @see		...
 * @param	iYellowType: ��ɫָʾ��״̬ 0 �أ� 1 ���� 2 ��˸
 * @param	iBlueType:   ��ɫָʾ��״̬ 0 �أ� 1 ���� 2 ��˸
 * @param	iGreenType:  ��ɫָʾ��״̬ 0 �أ� 1 ���� 2 ��˸
 * @param	iRedType:    ��ɫָʾ��״̬ 0 �أ� 1 ���� 2 ��˸
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SetLightStatus(int iYellowType, int iBlueType, int iGreenType,  int iRedType);




/** 
 * @fn		CRT_SetLightStatus 
 * @detail	��ȡ������ָʾ��״̬
 * @see		...
 * @param	iYellowType: ��� ��ɫָʾ��״̬ 0 �أ� 1 ���� 2 ��˸
 * @param	iBlueType:   ��� ��ɫָʾ��״̬ 0 �أ� 1 ���� 2 ��˸
 * @param	iGreenType:  ��� ��ɫָʾ��״̬ 0 �أ� 1 ���� 2 ��˸
 * @param	iRedType:    ��� ��ɫָʾ��״̬ 0 �أ� 1 ���� 2 ��˸
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetLightStatus(int* iYellowType, int* iBlueType, int* iGreenType,  int* iRedType);




/** 
 * @fn		CRT_BanTypeBCap 
 * @detail	���ö�������ȡTYPE B������
 * @see		...
 * @param	iBanMode: ִ��ģʽ�� 0 ������ 1 ��ֹ����TYPEB�� 2 ��ֹ����֤
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_BanTypeBCap(int iBanMode);




//***********************����CRT603���幦��APDU����****************************
/** 
 * @fn		CRT_LoadMifareKey 
 * @detail	Mifare����������
 * @see		...
 * @param	ilocal:   ����洢λ�á� 0 ��ʱ�Դ洢���� 1 ����ʧ�洢��
 * @param	iKeyType: ��Կ���͡� 0 TYPE A���ͣ� 1 TYPE B���� 
 * @param	iKeyNum:  �����浽��Կ��ţ���0-15�飩 
 * @param	byInKeyData: ������Ϣ�� ����6λ����0xFF,0xFF,0xFF,0xFF,0xFF,0xFF��
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_LoadMifareKey(int ilocal, int iKeyType, int iKeyNum, BYTE byInKeyData[]);




/** 
 * @fn		CRT_CheckMifareKey 
 * @detail	Mifare��У������
 * @see		...
 * @param	iKeyType: ��Կ���͡� 0 TYPE A���ͣ� 1 TYPE B���� 
 * @param	iKeyNum:  �����غõ���Կ��ţ���0-15�飩 
 * @param	iBlockNum: ��У���Mifare�����(0Ϊ��ʼλ)
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_CheckMifareKey(int iKeyType, int iKeyNum, int iBlockNum);




/** 
 * @fn		CRT_Read 
 * @detail	��CPU�������ݲ���
 * @see		...
 * @param	bFilica: �Ƿ�Ϊfilica���������� true ��filica�� false ��filica 
 * @param	iBlockNum: ���ȡ�Ŀ����(0Ϊ��ʼλ)
 * @param	byReadData: ��� ��ȡ��������
 * @param	iReadDataLen: ��� ��ȡ�������ݳ���
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_Read(bool bFilica, int iBlockNum, BYTE byReadData[], int* iReadDataLen);




/** 
 * @fn		CRT_Write 
 * @detail	��CPU��д���ݲ���
 * @see		...
 * @param	bFilica: �Ƿ�Ϊfilica���������� true ��filica�� false ��filica 
 * @param	iBlockNum: ��д��Ŀ����(0Ϊ��ʼλ)
 * @param	byWriteData: д�뵽�������ϵ�����
 * @param	iWriteLen: д�뵽�������ϵ����ݳ���
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_Write(bool bFilica, int iBlockNum, BYTE byWriteData[], int iWriteLen);




/** 
 * @fn		CRT_GetCardUID 
 * @detail	��ȡ��ƬUID��Ϣ
 * @see		...
 * @param	szUID: ��� ��Ƭ��UID��Ϣ
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetCardUID(char szUID[]);




//***********************����CRT603���幦��SAM������****************************
/** 
 * @fn		CRT_SAMSlotActivation 
 * @detail	SAM���л��������
 * @see		...
 * @param	iSlotNum: ���л�����Ŀ����� ��1-4������ 1��ʾSAM1������ 
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SAMSlotActivation(int iSlotNum);




//***********************ע��CCID����****************************
/** 
 * @fn		CRT_RegisterCCID 
 * @detail	ע��CCID����
 * @see		...
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_RegisterCCID();




//***********************��ȡ���һ�δ�������****************************
/** 
 * @fn		CRT_GetLastError 
 * @detail	��ȡ���һ�δ�������
 * @see		...
 * @return	��ȡ��Ϣ
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
char* WINAPI CRT_GetLastError();




//***********************������֤��Ϣ����****************************
/** 
 * @fn		CRT_ReadIDCardInifo 
 * @detail	������֤��Ϣ
 * @see		...
 * @param	crtdef_IdInfo: �������ȡ���Ķ���֤��Ϣ
 * @param	szHeadFullPath: ����Сͷ���ȫ·����ֻ֧��.bmp��
 * @param	szFrontFullPath: ��������ͼ���ȫ·����֧��.bmp, .jpg��
 * @param	szBackFullPath: ���ɷ���ͼ���ȫ·����֧��.bmp, .jpg��
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_ReadIDCardInifo(CRTDef_IDInfo* crtdef_IdInfo, char szHeadFullPath[], char szFrontFullPath[], char szBackFullPath[]);




//***********************SAM���ȸ�λ****************************
/** 
 * @fn		CRT_SAMSlotReset 
 * @detail	��ǰ�����SAM���ȸ�λ
 * @see		...
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SAMSlotReset();



//***********************��ȡSAM���ۿ�״̬****************************
/** 
 * @fn		CRT_GetSAMSlotStatus
 * @detail	��ȡSAM���ۿ�״̬
 * @param	iSamNum: ���ѯ��SAM���ۺ�(1-4)
 * @see		...
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetSAMSlotStatus(int iSamNum);



//***********************�����������дŵ�****************************
/** 
 * @fn		CRT_ReadMagAllTracks 
 * @detail	�����������дŵ�
 * @see		...
 * @param	szTrack1: �������ȡ����һ�ŵ�
 * @param	szTrack2: �������ȡ���Ķ��ŵ�
 * @param	szTrack3: �������ȡ�������ŵ�
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_ReadMagAllTracks(char szTrack1[], char szTrack2[], char szTrack3[]);




//***********************��ȡRF������****************************
/** 
 * @fn		CRT_GetRFCardType 
 * @detail	��ȡRF������
 * @see		...
 * @return	0 �޿���1 TYPE A���Ϳ��� 2 TYPE B���Ϳ��� 3 ���֤�� ���� ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetRFCardType();


//***********************���ö���֤����****************************
/** 
 * @fn		CRT_SetIDFonts 
 * @detail	���ö���֤����
 * @see		...
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2016/6/28
 */
int WINAPI CRT_SetIDFonts();


/** 
 * @fn		CRT_Wait_RequestCard 
 * @detail	�ȴ���ѯ������
 * @see		...
 * @param	rRecvData: ��������ص����� ���������� MagTracks ����������ATR��Ϣ Icc_atr
 * @param	itimeout: ���룬��ʱʱ�� ����Ϊ��λ
 * @param	...: ���룬�ɱ������ ������Ӵ�ʽIC���� �ǽ�IC����������������֤��EMID 
 * @return	����Ѱ�ҵ��Ŀ�����Ӵ�ʽIC������02�� ���ϵ磬ֱ�ӿ��Բ�����< 0ʧ�� -4 ȡ���� -10 ��ʱ
 * @exception ...
 *
 * @author	luowei
 * @date	2016/6/28
 */
int WINAPI CRT_Wait_RequestCard(Request_recv_data* rRecvData, int itimeout, ...);


/** 
 * @fn		CRT_Cancel_RequestCard 
 * @detail	ȡ����ѯ������
 * @see		... 
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2016/6/28
 */
int WINAPI CRT_Cancel_RequestCard();



/** 
 * @fn		CRT_M1ValueProcess 
 * @detail	M1��ֵ����
 * @see		...
 * @param	iMode: ���룬����ģʽ�� 1 ��ʼ��Ǯ���� 2 ��ֵ�� 3 ��ֵ
 * @param	iBlock: ���룬���������� ����Ե�ַ
 * @param	iValue: ���룬�������
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2016/9/19
 */
int WINAPI CRT_M1ValueProcess(int iMode, int iBlock, int iValue);



/** 
 * @fn		CRT_M1InquireBalance 
 * @detail	M1����ѯ���
 * @see		...
 * @param	iBlock: ���룬���������� ����Ե�ַ
 * @param	iValue: �������ѯ���Ľ��ֵ
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2016/9/19
 */
int WINAPI CRT_M1InquireBalance(int iBlock, int* iValue);



/** 
 * @fn		CRT_M1BackBlock 
 * @detail	M1������Ǯ��
 * @see		...
 * @param	iBlock: ���룬�豸�ݿ����� ����Ե�ַ
 * @param	iBackBlock: ���룬���ݵ������� ����Ե�ַ
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2016/9/19
 */
int WINAPI CRT_M1BackBlock(int iBlock, int iBackBlock);



/** 
 * @fn		CRT_GetIDFinger 
 * @detail	��ȡ����ָ֤����Ϣ
 * @see		...
 * @param	byFinger: �������ȡ����ָ����Ϣ(һ��Ϊ1024���ֽ�)
 * @param	iFingerLen: �����ָ����Ϣ����
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2016/10/27
 */
int WINAPI CRT_GetIDFinger(BYTE byFinger[], int *iFingerLen);



/** 
 * @fn		CRT_GetIDFinger 
 * @detail	��ȡ����֤DN��
 * @see		...
 * @param	szDNNums: �������ȡ����DN��
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2016/10/30
 */
int WINAPI CRT_GetIDDNNums(char szDNNums[]);


/** 
 * @fn		CRT_GetSAMID 
 * @detail	��ȡ���֤����SAM ID
 * @see		...
 * @param	szSAMID: �������ȡ����SAM ID
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2016/10/30
 */
int WINAPI CRT_GetSAMID(char szSAMID[]);



/** 
 * @fn		CRT_SwitchRF 
 * @detail	������������Ƶ��
 * @see		...
 * @param	iMode: ���� ������Ƶ����ʽ��0 ������ 1 �ر�
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2016/10/30
 */
int WINAPI CRT_SwitchRF(int iMode);




/** 
 * @fn		CRT_P2PTransBmp 
 * @detail	��Ե㴫��ͼƬ
 * @see		...
 * @param	szBmpPath: ���� �����ͼƬ·��
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2017/1/18
 */
int WINAPI CRT_P2PTransBmp(char szBmpPath[]);



/** 
 * @fn		CRT_P2PTransTxt 
 * @detail	��Ե㴫��������Ϣ
 * @see		...
 * @param	szTxtInfo: ���� �����������Ϣ�����1000��
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2017/1/18
 */
int WINAPI CRT_P2PTransTxt(char szTxtInfo[]);




/** 
 * @fn		CRT_P2PTransURL 
 * @detail	��Ե㴫��URL
 * @see		...
 * @param	iType: ����  URL���� 0 www.��ͷ����ַ ����www.baidu.com�� 1 �绰���룬 2 �����ʼ���ַ��
 * @param	szURLInfo: ���� �����URL��Ϣ
 * @return	0 �ɹ�����0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2017/1/18
 */
int WINAPI CRT_P2PTransURL(int iType, char szURLInfo[]);




//***********************������֤����Ϣ����������֤������˾���֤������****************************
/** 
 * @fn		CRT_ReadAllIDCardInfo 
 * @detail	������֤����Ϣ����������֤������˾���֤��
 * @see		...
 * @param	crtdef_IdInfo: �������ȡ���Ķ���֤��Ϣ
 * @param	prtd_Info: �������ȡ������������þ���֤��Ϣ
 * @param	szHeadFullPath: ����Сͷ���ȫ·����ֻ֧��.bmp��
 * @param	szFrontFullPath: ��������ͼ���ȫ·������NULL�����ϳ�ͼƬ��֧��.bmp, .jpg��
 * @param	szBackFullPath: ���ɷ���ͼ���ȫ·������NULL�����ϳ�ͼƬ��֧��.bmp, .jpg��������˾���֤�޷��棩
 * @return	1 ����֤��Ϣ(crtdef_IdInfo ��Ч)�� 2 ����˾���֤��Ϣ(prtd_Info ��Ч)�� С��0ʧ��
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_ReadAllIDCardInfo(CRTDef_IDInfo* crtdef_IdInfo, CRTDef_PRTDInfo* prtd_Info, char szHeadPath[], char szFrontPath[], char szBackPath[]);





#ifdef __cplusplus
}
#endif