// CRT_603_CZ7.h : CRT_603_CZ7 DLL 的主头文件
/** *************************************************************************************
 *  @file     	CRT_603_CZ7 DLL
 *  @brief      CRT_603_CZ7动态库
 *  @details 	提供CRT_603_CZ7 PCSC动态库及CRT_603_CZ7 通讯协议下的命令发送
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

//二代证信息
typedef struct
{
	char szName[31];   //姓名
	char szSex[3];     //性别
	char szNation[20]; //民族
	char szBornDay[10]; //出生
	char szAddress[128]; //地址
	char szIDNum[20];   //身份证编号
	char szIssued[31];  //签发机关
	char szBeginValidity[10]; //开始有效日期
	char szEndValidity[10];   //截止有效日期
	char szImgPath[256];      //头文件路径
}CRTDef_IDInfo, *pCRTDef_IDInf;

//外国人永久居留证信息
typedef struct  
{
	char szEnName[128];  // 英文名
	char szSex[3];       // 性别
	char szIDNum[31];    // 永久居留证号码
	char szAddress[20];  // 国籍或所在地区代码
	char szCnName[31];   // 中文名
	char szBeginValidity[16];   // 证件签发日期
	char szEndValidity[16];     // 证件终止日期
	char szBornDay[16];         // 出生日期
	char szVersion[5];          // 证件版本号
	char szIssued[8];           // 申请受理机关代码
	char szType[2];             // 证件类型标识
	char szImgPath[256];        // 头像路径
}CRTDef_PRTDInfo, *pCRTDef_PRTDInfo;



// 轮询请求
#define REQUEST_CONTSCT_ICC       0x0002    // 请 求 接 触 式 IC 卡
#define REQUEST_RF_ICC            0x0004    // 请 求 非 接 触 式 RF 卡
#define REQUEST_MAG               0x0008    // 请 求 磁 条 卡
#define REQUEST_ID_ICC            0x0010    // 请 求 二代证
#define REQUEST_EMID              0x0020    // 请 求 EMID

#define REQUEST_ERR_CANCEL        -4     // 取消错误
#define REQUEST_ERR_TIMEOUT       -10    // 超时错误
#define REQUEST_ERR_OTHER         -99    // 其他错误

//返回寻卡数据信息
struct MagTracks //磁条卡信息
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
 * @detail	打开CRT智能读卡器
 * @see		...
 * @param	iListNums:输出 连接的读卡器列表个数
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_OpenConnect(int* iListNums);




/** 
 * @fn		CRT_CloseConnect 
 * @detail	关闭CRT智能读卡器
 * @see		...
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_CloseConnect();




/** 
 * @fn		CRT_GetReaderListName 
 * @detail	获取智能读卡器列表名字
 * @see		...
 * @param	iListNum:需获取的读卡器序列号（0为起始）
 * @param	szListName:输出 获取到的读卡器名字
 * @return	0 成功， 非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetReaderListName(int iListNum, char szListName[]);




/** 
 * @fn		CRT_SetReaderName 
 * @detail	设置当前工作的读卡器序列号（打开时，默认设置为第一个读卡器）
 * @see		...
 * @param	iListNum:需设置的读卡器序列号（0为起始）
 * @return	0 成功， 非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SetReaderName(int iListNum);




/** 
 * @fn		CRT_GetCardStatus 
 * @detail	获取读卡器上卡状态
 * @see		...
 * @return	1 有卡， 2 无卡， 9 状态未知
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetCardStatus();




/** 
 * @fn		CRT_EjectCard 
 * @detail	弹卡下电，读卡器与卡片断开连接
 * @see		...
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_EjectCard();




/** 
 * @fn		CRT_ReaderConnect 
 * @detail	读卡器卡片连接上电
 * @see		...
 * @param	byAtrData:输出 上电成功返回的ATR数据
 * @param	iAtrLen: 输出 上电成功返回的ATR数据长度
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_ReaderConnect(BYTE byAtrData[], int* iAtrLen);




/** 
 * @fn		CRT_ChipPower 
 * @detail	读卡器对卡片的上下电操作
 * @see		...
 * @param	wChipPower:0x0002 冷复位， 0x0004 热复位， 0x0008 下电
 * @param	byAtrData:输出 上电成功返回的ATR数据
 * @param	iAtrLen: 输出 上电成功返回的ATR数据长度
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_ChipPower(WORD wChipPower, BYTE byAtrData[], int* iAtrLen);




/** 
 * @fn		CRT_SendApdu 
 * @detail	读卡器发送APDU指令
 * @see		...
 * @param	bySendData:需发送的APDU指令（支持acsii码与BCD码）
 * @param	iSendDataLen: 需发送的APDU指令长度
 * @param	byRecvData:输出 APDU通讯后返回的数据
 * @param	iRecvDataLen: 输出 APDU通讯后返回的数据长度
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SendApdu(BYTE bySendData[], int iSendDataLen, BYTE byRecvData[], int* iRecvDataLen);




/** 
 * @fn		CRT_SendControlCMD 
 * @detail	读卡器发送扩展控制命令
 * @see		...
 * @param	bySendData:需发送的扩展控制命令数据（支持acsii码与BCD码）
 * @param	iSendDataLen: 需发送的扩展控制命令数据长度
 * @param	byRecvData:输出 扩展控制命令通讯后返回的数据
 * @param	iRecvDataLen: 输出 扩展控制命令通讯后返回的数据长度
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SendControlCMD(BYTE bySendData[], int iSendDataLen, BYTE byRecvData[], int* iRecvDataLen);




//***********************发送CRT603具体功能扩展命令****************************
/** 
 * @fn		CRT_HotReset 
 * @detail	读卡器热复位
 * @see		...
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_HotReset();




/** 
 * @fn		CRT_SetReaderType 
 * @detail	设置读卡器操作模式
 * @see		...
 * @param	iType: 1 正常RF卡模式， 2 Felica模式， 3 点对点模式， 4 二代证模式， 5 卡模拟模式
  * @param	iDelayTime(ms): 设置执行后延时时间，模式设定完成之后有些固件需要延时，默认300ms 
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SetReaderType(int iType, int iDelayTime=100);




/** 
 * @fn		CRT_GetReaderType 
 * @detail	获取读卡器操作模式
 * @see		...
 * @return	>0 失败 1 正常RF卡模式， 2 Felica模式， 3 点对点模式， 4 二代证模式， 5 卡模拟模式
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetReaderType();




/** 
 * @fn		CRT_GetVersionInfo 
 * @detail	获取读卡器版本信息
 * @see		...
 * @param	iVerType: 需获取的版本 0 P/N信息， 1 SN信息， 2, 固件版本信息， 3 生成版本信息， 4 EMID信息， 5 动态库版本信息, 6 设备子型号
 * @param	szVersionInfo: 输出 返回的版本信息
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetVersionInfo(int iVerType, char szVersionInfo[]);




/** 
 * @fn		CRT_AutoBeel 
 * @detail	读卡器自动蜂鸣
 * @see		...
 * @param	bAutoBeel: 是否自动蜂鸣，0 关闭，1 开启，2 移卡自动蜂鸣
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_AutoBeel(int bAutoBeel);




/** 
 * @fn		CRT_Beel 
 * @detail	读卡器蜂鸣设置
 * @see		...
 * @param	MultipleTime: 蜂鸣时间：0.25秒的倍数，比如2，蜂鸣时间：2*0.25 即0.5秒（0-20 默认为1)
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_Beel(int MultipleTime = 1);




/** 
 * @fn		CRT_SetLightMode 
 * @detail	设置读卡器指示灯模式
 * @see		...
 * @param	iType: 指示灯模式 0 自动模式， 1 手动模式
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SetLightMode(int iType);




/** 
 * @fn		CRT_GetLightMode 
 * @detail	获取读卡器指示灯模式
 * @see		...
 * @return	0 自动模式，1 手动模式， 9模式未知
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetLightMode();




/** 
 * @fn		CRT_SetLightStatus 
 * @detail	设置读卡器指示灯状态
 * @see		...
 * @param	iYellowType: 黄色指示灯状态 0 关， 1 开， 2 闪烁
 * @param	iBlueType:   蓝色指示灯状态 0 关， 1 开， 2 闪烁
 * @param	iGreenType:  绿色指示灯状态 0 关， 1 开， 2 闪烁
 * @param	iRedType:    红色指示灯状态 0 关， 1 开， 2 闪烁
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SetLightStatus(int iYellowType, int iBlueType, int iGreenType,  int iRedType);




/** 
 * @fn		CRT_SetLightStatus 
 * @detail	获取读卡器指示灯状态
 * @see		...
 * @param	iYellowType: 输出 黄色指示灯状态 0 关， 1 开， 2 闪烁
 * @param	iBlueType:   输出 蓝色指示灯状态 0 关， 1 开， 2 闪烁
 * @param	iGreenType:  输出 绿色指示灯状态 0 关， 1 开， 2 闪烁
 * @param	iRedType:    输出 红色指示灯状态 0 关， 1 开， 2 闪烁
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetLightStatus(int* iYellowType, int* iBlueType, int* iGreenType,  int* iRedType);




/** 
 * @fn		CRT_BanTypeBCap 
 * @detail	设置读卡器读取TYPE B卡能力
 * @see		...
 * @param	iBanMode: 执行模式。 0 开启， 1 禁止所有TYPEB， 2 禁止二代证
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_BanTypeBCap(int iBanMode);




//***********************发送CRT603具体功能APDU命令****************************
/** 
 * @fn		CRT_LoadMifareKey 
 * @detail	Mifare卡下载密码
 * @see		...
 * @param	ilocal:   密码存储位置。 0 临时性存储器， 1 非易失存储器
 * @param	iKeyType: 密钥类型。 0 TYPE A类型， 1 TYPE B类型 
 * @param	iKeyNum:  将保存到密钥组号（共0-15组） 
 * @param	byInKeyData: 密码信息。 （共6位，如0xFF,0xFF,0xFF,0xFF,0xFF,0xFF）
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_LoadMifareKey(int ilocal, int iKeyType, int iKeyNum, BYTE byInKeyData[]);




/** 
 * @fn		CRT_CheckMifareKey 
 * @detail	Mifare卡校验密码
 * @see		...
 * @param	iKeyType: 密钥类型。 0 TYPE A类型， 1 TYPE B类型 
 * @param	iKeyNum:  已下载好的密钥组号（共0-15组） 
 * @param	iBlockNum: 需校验的Mifare卡块号(0为起始位)
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_CheckMifareKey(int iKeyType, int iKeyNum, int iBlockNum);




/** 
 * @fn		CRT_Read 
 * @detail	非CPU卡读数据操作
 * @see		...
 * @param	bFilica: 是否为filica卡读操作。 true 是filica， false 非filica 
 * @param	iBlockNum: 需读取的卡块号(0为起始位)
 * @param	byReadData: 输出 读取到的数据
 * @param	iReadDataLen: 输出 读取到的数据长度
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_Read(bool bFilica, int iBlockNum, BYTE byReadData[], int* iReadDataLen);




/** 
 * @fn		CRT_Write 
 * @detail	非CPU卡写数据操作
 * @see		...
 * @param	bFilica: 是否为filica卡读操作。 true 是filica， false 非filica 
 * @param	iBlockNum: 需写入的卡块号(0为起始位)
 * @param	byWriteData: 写入到读卡器上的数据
 * @param	iWriteLen: 写入到读卡器上的数据长度
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_Write(bool bFilica, int iBlockNum, BYTE byWriteData[], int iWriteLen);




/** 
 * @fn		CRT_GetCardUID 
 * @detail	获取卡片UID信息
 * @see		...
 * @param	szUID: 输出 卡片的UID信息
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetCardUID(char szUID[]);




//***********************发送CRT603具体功能SAM卡命令****************************
/** 
 * @fn		CRT_SAMSlotActivation 
 * @detail	SAM卡切换并激活卡座
 * @see		...
 * @param	iSlotNum: 需切换激活的卡座号 （1-4个，如 1表示SAM1卡座） 
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SAMSlotActivation(int iSlotNum);




//***********************注册CCID命令****************************
/** 
 * @fn		CRT_RegisterCCID 
 * @detail	注册CCID命令
 * @see		...
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_RegisterCCID();




//***********************获取最后一次错误描叙****************************
/** 
 * @fn		CRT_GetLastError 
 * @detail	获取最后一次错误描叙
 * @see		...
 * @return	获取信息
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
char* WINAPI CRT_GetLastError();




//***********************读二代证信息命令****************************
/** 
 * @fn		CRT_ReadIDCardInifo 
 * @detail	读二代证信息
 * @see		...
 * @param	crtdef_IdInfo: 输出，读取到的二代证信息
 * @param	szHeadFullPath: 生成小头像的全路径（只支持.bmp）
 * @param	szFrontFullPath: 生成正面图像的全路径（支持.bmp, .jpg）
 * @param	szBackFullPath: 生成反面图像的全路径（支持.bmp, .jpg）
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_ReadIDCardInifo(CRTDef_IDInfo* crtdef_IdInfo, char szHeadFullPath[], char szFrontFullPath[], char szBackFullPath[]);




//***********************SAM卡热复位****************************
/** 
 * @fn		CRT_SAMSlotReset 
 * @detail	当前激活的SAM卡热复位
 * @see		...
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_SAMSlotReset();



//***********************获取SAM卡槽卡状态****************************
/** 
 * @fn		CRT_GetSAMSlotStatus
 * @detail	获取SAM卡槽卡状态
 * @param	iSamNum: 需查询的SAM卡槽号(1-4)
 * @see		...
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetSAMSlotStatus(int iSamNum);



//***********************读卡器读所有磁道****************************
/** 
 * @fn		CRT_ReadMagAllTracks 
 * @detail	读卡器读所有磁道
 * @see		...
 * @param	szTrack1: 输出，读取到的一磁道
 * @param	szTrack2: 输出，读取到的二磁道
 * @param	szTrack3: 输出，读取到的三磁道
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_ReadMagAllTracks(char szTrack1[], char szTrack2[], char szTrack3[]);




//***********************获取RF卡类型****************************
/** 
 * @fn		CRT_GetRFCardType 
 * @detail	获取RF卡类型
 * @see		...
 * @return	0 无卡，1 TYPE A类型卡， 2 TYPE B类型卡， 3 身份证， 其他 失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_GetRFCardType();


//***********************设置二代证字体****************************
/** 
 * @fn		CRT_SetIDFonts 
 * @detail	设置二代证字体
 * @see		...
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2016/6/28
 */
int WINAPI CRT_SetIDFonts();


/** 
 * @fn		CRT_Wait_RequestCard 
 * @detail	等待轮询卡命令
 * @see		...
 * @param	rRecvData: 输出，返回的数据 磁条卡类型 MagTracks ，其他返回ATR信息 Icc_atr
 * @param	itimeout: 输入，超时时间 毫秒为单位
 * @param	...: 输入，可变参数， 可请求接触式IC卡， 非接IC卡，磁条卡，二代证，EMID 
 * @return	返回寻找到的卡，如接触式IC卡返回02， 已上电，直接可以操作。< 0失败 -4 取消， -10 超时
 * @exception ...
 *
 * @author	luowei
 * @date	2016/6/28
 */
int WINAPI CRT_Wait_RequestCard(Request_recv_data* rRecvData, int itimeout, ...);


/** 
 * @fn		CRT_Cancel_RequestCard 
 * @detail	取消轮询卡命令
 * @see		... 
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2016/6/28
 */
int WINAPI CRT_Cancel_RequestCard();



/** 
 * @fn		CRT_M1ValueProcess 
 * @detail	M1卡值操作
 * @see		...
 * @param	iMode: 输入，操作模式。 1 初始化钱包， 2 增值， 3 减值
 * @param	iBlock: 输入，操作块区。 需绝对地址
 * @param	iValue: 输入，操作金额
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2016/9/19
 */
int WINAPI CRT_M1ValueProcess(int iMode, int iBlock, int iValue);



/** 
 * @fn		CRT_M1InquireBalance 
 * @detail	M1卡查询余额
 * @see		...
 * @param	iBlock: 输入，操作块区。 需绝对地址
 * @param	iValue: 输出，查询到的金额值
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2016/9/19
 */
int WINAPI CRT_M1InquireBalance(int iBlock, int* iValue);



/** 
 * @fn		CRT_M1BackBlock 
 * @detail	M1卡备份钱包
 * @see		...
 * @param	iBlock: 输入，需备份块区。 需绝对地址
 * @param	iBackBlock: 输入，备份到块区。 需绝对地址
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2016/9/19
 */
int WINAPI CRT_M1BackBlock(int iBlock, int iBackBlock);



/** 
 * @fn		CRT_GetIDFinger 
 * @detail	获取二代证指纹信息
 * @see		...
 * @param	byFinger: 输出，获取到的指纹信息(一般为1024个字节)
 * @param	iFingerLen: 输出，指纹信息长度
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2016/10/27
 */
int WINAPI CRT_GetIDFinger(BYTE byFinger[], int *iFingerLen);



/** 
 * @fn		CRT_GetIDFinger 
 * @detail	获取二代证DN号
 * @see		...
 * @param	szDNNums: 输出，获取到的DN号
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2016/10/30
 */
int WINAPI CRT_GetIDDNNums(char szDNNums[]);


/** 
 * @fn		CRT_GetSAMID 
 * @detail	获取身份证盒子SAM ID
 * @see		...
 * @param	szSAMID: 输出，获取到的SAM ID
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2016/10/30
 */
int WINAPI CRT_GetSAMID(char szSAMID[]);



/** 
 * @fn		CRT_SwitchRF 
 * @detail	读卡器开关射频场
 * @see		...
 * @param	iMode: 输入 开关射频场方式，0 开启， 1 关闭
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2016/10/30
 */
int WINAPI CRT_SwitchRF(int iMode);




/** 
 * @fn		CRT_P2PTransBmp 
 * @detail	点对点传输图片
 * @see		...
 * @param	szBmpPath: 输入 传输的图片路径
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2017/1/18
 */
int WINAPI CRT_P2PTransBmp(char szBmpPath[]);



/** 
 * @fn		CRT_P2PTransTxt 
 * @detail	点对点传输文字信息
 * @see		...
 * @param	szTxtInfo: 输入 传输的文字信息（最大1000）
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2017/1/18
 */
int WINAPI CRT_P2PTransTxt(char szTxtInfo[]);




/** 
 * @fn		CRT_P2PTransURL 
 * @detail	点对点传输URL
 * @see		...
 * @param	iType: 输入  URL类型 0 www.开头的网址 比如www.baidu.com， 1 电话号码， 2 发送邮件地址。
 * @param	szURLInfo: 输入 传输的URL信息
 * @return	0 成功，非0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2017/1/18
 */
int WINAPI CRT_P2PTransURL(int iType, char szURLInfo[]);




//***********************读所有证件信息（包括二代证，外国人居留证）命令****************************
/** 
 * @fn		CRT_ReadAllIDCardInfo 
 * @detail	读所有证件信息（包括二代证，外国人居留证）
 * @see		...
 * @param	crtdef_IdInfo: 输出，读取到的二代证信息
 * @param	prtd_Info: 输出，读取到的外国人永久居留证信息
 * @param	szHeadFullPath: 生成小头像的全路径（只支持.bmp）
 * @param	szFrontFullPath: 生成正面图像的全路径（传NULL，不合成图片。支持.bmp, .jpg）
 * @param	szBackFullPath: 生成反面图像的全路径（传NULL，不合成图片。支持.bmp, .jpg）（外国人居留证无反面）
 * @return	1 二代证信息(crtdef_IdInfo 有效)， 2 外国人居留证信息(prtd_Info 有效)， 小于0失败
 * @exception ...
 *
 * @author	luowei
 * @date	2015/12/1
 */
int WINAPI CRT_ReadAllIDCardInfo(CRTDef_IDInfo* crtdef_IdInfo, CRTDef_PRTDInfo* prtd_Info, char szHeadPath[], char szFrontPath[], char szBackPath[]);





#ifdef __cplusplus
}
#endif