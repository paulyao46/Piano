using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct KEY
{
    public int isPress;
    public float proximity;
}
public enum TRACK
{
    TRACK3,
    TRACK4,
    TRACK5,
    TRACK10
}

public class GyroObj : MonoBehaviour
{
    public DeviceNumber deviceNumber;
    public BaudRate baudRate;
    public parity_mode parityMode;
    public data_bits dataBits;
    public stop_bits stopBits;
    public TRACK tracks;
    public KEY[] keyCode;
    public int nimble;
    public Rs232 rs232;
    const byte checkSum = 0;
    byte[] getKeyCmd = { 0xA0, 0x00, 0x00, checkSum };
    int Light1 = 0x00;
    int Light2 = 0x00;
    public byte[] getBytes;

    void Start()
    {

        rs232.OpenComport(deviceNumber, baudRate, parityMode, dataBits, stopBits);
        rs232.ReceiveEvent += OnReceive;
        keyCode = new KEY[10];

    }

    void Update()
    {
        if (rs232.isRunning)
        {
            rs232.Send(getKeyCmd, "GetKey");
        }
    }

    public void OnReceive(byte[] p_buffer)
    {
        switch (p_buffer[0])
        {
            case 0xA1:
                OnGetKey(p_buffer);

                break;
        }
    }

    void OnGetKey(byte[] p_buffer)
    {
        switch (tracks)
        {
            case TRACK.TRACK3:
                break;
            case TRACK.TRACK4:
                Light1 = 0;
                Light2 = 0;
                getBytes = p_buffer;
                for (int i = 0; i < keyCode.Length; i++)
                {
                    int bytePos = i / 8;
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 2:
                            keyCode[i].proximity = getBytes[4 + i * 2] | getBytes[5 + i * 2] << 8;
                            if (bytePos == 0)
                            {
                                if (keyCode[i].proximity >= nimble)
                                {
                                    Light1 = Light1 | 1 << 0;
                                    Light1 = Light1 | 1 << 1;
                                    Light1 = Light1 | 1 << 2;
                                    keyCode[0].isPress = 1;
                                    keyCode[1].isPress = 1;
                                    keyCode[2].isPress = 1;
                                    i = 2;
                                }
                                else
                                {
                                    Light1 = Light1 | 0 << i;
                                    keyCode[i].isPress = 0;
                                }
                            }
                            break;
                        case 3:
                        case 4:
                        case 5:
                            keyCode[i].proximity = getBytes[4 + i * 2] | getBytes[5 + i * 2] << 8;
                            if (keyCode[i].proximity >= nimble)
                            {
                                Light1 = Light1 | 1 << 3;
                                Light1 = Light1 | 1 << 4;
                                Light1 = Light1 | 1 << 5;
                                keyCode[3].isPress = 1;
                                keyCode[4].isPress = 1;
                                keyCode[5].isPress = 1;
                                i = 5;
                            }
                            else
                            {
                                Light1 = Light1 | 0 << i;
                                keyCode[i].isPress = 0;
                            }
                            break;
                        case 6:
                        case 7:
                        case 8:
                            keyCode[i].proximity = getBytes[4 + i * 2] | getBytes[5 + i * 2] << 8;
                            if (bytePos == 0)
                            {
                                if (keyCode[i].proximity >= nimble)
                                {
                                    Light1 = Light1 | 1 << 6;
                                    Light1 = Light1 | 1 << 7;
                                    Light2 = Light2 | 1 << 0;
                                    keyCode[6].isPress = 1;
                                    keyCode[7].isPress = 1;
                                    keyCode[8].isPress = 1;
                                    i = 8;
                                }
                                else
                                {
                                    Light1 = Light1 | 0 << i;
                                    keyCode[i].isPress = 0;
                                }
                            }else
                            {
                                if (keyCode[i].proximity >= nimble)
                                {
                                    Light1 = Light1 | 1 << 6;
                                    Light1 = Light1 | 1 << 7;
                                    Light2 = Light2 | 1 << 0;
                                    keyCode[6].isPress = 1;
                                    keyCode[7].isPress = 1;
                                    keyCode[8].isPress = 1;
                                    i = 8;
                                }
                                else
                                {
                                    Light2 = Light2 | 0 << i - 8 * bytePos;
                                    keyCode[i].isPress = 0;
                                }
                            }
                            break;
                        case 9:
                            keyCode[i].proximity = getBytes[4 + i * 2] | getBytes[5 + i * 2] << 8;
                            if (keyCode[i].proximity >= nimble)
                            {
                                Light2 = Light2 | 1 << i - 8 * bytePos;
                                keyCode[i].isPress = 1;
                            }
                            else
                            {
                                Light2 = Light2 | 0 << i - 8 * bytePos;
                                keyCode[i].isPress = 0;
                            }

                            break;
                    }

                }
                break;
            case TRACK.TRACK5:
                break;
            case TRACK.TRACK10:
                Light1 = 0;
                Light2 = 0;
                getBytes = p_buffer;
                for (int i = 0; i < keyCode.Length; i++)
                {
                    int bytePos = i / 8;
                    //keyCode[i].isPress = getBytes[2+bytePos] >> i%8 & 0x01;
                    keyCode[i].proximity = getBytes[4 + i * 2] | getBytes[5 + i * 2] << 8;
                    if (bytePos == 0)
                    {
                        if (keyCode[i].proximity >= nimble)
                        {
                            Light1 = Light1 | 1 << i;
                            keyCode[i].isPress = 1;
                        }
                        else
                        {
                            Light1 = Light1 | 0 << i;
                            keyCode[i].isPress = 0;
                        }
                    }
                    else
                    {
                        if (keyCode[i].proximity >= nimble)
                        {
                            Light2 = Light2 | 1 << i - 8 * bytePos;
                            keyCode[i].isPress = 1;
                        }
                        else
                        {
                            Light2 = Light2 | 0 << i - 8 * bytePos;
                            keyCode[i].isPress = 0;
                        }
                    }
                }
                break;

        }
        getKeyCmd[1] = (byte)Light1;
        getKeyCmd[2] = (byte)Light2;
        //byte _checkSum = 0x00;
        //for (int f = 0; f < (p_buffer.Length - 1); f++)
        //{
        //    _checkSum += getBytes[f];
        //}
        //if (_checkSum == getBytes[24])
        //{
        //    Debug.Log("good");
        //}
    }
    public KEY[] GetKeysState()
    {
        return keyCode;
    }
}
