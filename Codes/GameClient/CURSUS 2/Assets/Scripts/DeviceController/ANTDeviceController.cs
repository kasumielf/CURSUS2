using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using ANT_Managed_Library;
using UnityEngine;

public struct ANTRawData
{
    public ushort eventTime;
    public ushort revCount;
    public override string ToString()
    {
        return eventTime + "\t" + revCount;
    }
}
public class ANTDeviceController : MonoBehaviour {
    public TextMesh speedMesh;
    public TextMesh rpmMesh;

    int circ = 200;
    uint timeout = 0;
    ANT_Device dev0;
    ANT_Channel channel_speed;
    ANT_Channel channel_cadence;

    ANTRawData[] speedData;
    ANTRawData[] rpmData;

    double current_speed;
    double current_rpm;
    bool stop = false;

    public BikeControl bikeController;

    void Awake()
    {
    }
    // Use this for initialization
    void Start ()
    {
        current_speed = 0;
        current_rpm = 0;
        speedData = new ANTRawData[2];
        rpmData = new ANTRawData[2];

        if(bikeController == null)
        {
            bikeController = GameObject.FindWithTag("Player").GetComponent<BikeControl>();
        }

        try
        {
            dev0 = new ANT_Device();

            dev0.deviceResponse += new ANT_Device.dDeviceResponseHandler(dev0_deviceResponse);

            dev0.getChannel(0).channelResponse += new dChannelResponseHandler(speedSensorResponse);
            dev0.getChannel(1).channelResponse += new dChannelResponseHandler(cadenceSensorResponse);

            dev0.setNetworkKey(0, new byte[] { 0xB9, 0xA5, 0x21, 0xFB, 0xBD, 0x72, 0xC3, 0x45 });

            channel_speed = dev0.getChannel(0);
            channel_cadence = dev0.getChannel(1);

            channel_speed.assignChannel(ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00, 0, timeout);
            channel_speed.setChannelID(0, false, 123, 0, timeout);
            channel_speed.setChannelPeriod(8118, timeout);
            channel_speed.setChannelFreq(57, timeout);
            channel_speed.openChannel(timeout);

            channel_cadence.assignChannel(ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00, 0, timeout);
            channel_cadence.setChannelID(0, false, 122, 0, timeout);
            channel_cadence.setChannelPeriod(8102, timeout);
            channel_cadence.setChannelFreq(57, timeout);
            channel_cadence.openChannel(timeout);

            StartCoroutine(UpdateCoroutine());
            StartCoroutine(WaitForDecreaseSpeed());
            StartCoroutine(DecreaseSpeed());

			bikeController.is_ant_used = true;
        }
        catch (System.Exception)
        {
			bikeController.is_ant_used = false;
        }
		finally
		{
		}

		Debug.Log ("ANT Reset : " + bikeController.is_ant_used);
    }

    IEnumerator WaitForDecreaseSpeed()
    {
        while(true)
        {
            // 1초에 한 번 씩 stop 플래그를 true로 바꿈.
            yield return new WaitForSeconds(0.5f);
            stop = true;
        }
    }
    IEnumerator DecreaseSpeed()
    {
        while(true)
        {
            if(stop)
            {
                current_speed -= 5.0f;

                if (current_speed <= 0)
                {
                    current_speed = 0.0f;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    // Update is called once per frame
    void Update ()
    {
    }

    IEnumerator UpdateCoroutine()
    {
        while(true)
        {
            if(speedMesh != null)
                speedMesh.text = current_speed.ToString("N2");

            if (rpmMesh != null)
                rpmMesh.text = current_rpm.ToString();

            bikeController.MoveSpeed = (float)current_speed;

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnDestroy()
    {
        ANT_Device.shutdownDeviceInstance(ref dev0);
    }

    void dev0_deviceResponse(ANT_Response response)
    {
        Debug.Log("ANT Response\t" + decodeDeviceFeedback(response));
    }

    //Print the channel response to the textbox
    void speedSensorResponse(ANT_Response response)
    {
        //if (response.responseID == 0x4E && response.messageContents[1] == 0x05)
        if (response.responseID == 0x4E)
        {
            stop = false;

            ushort bs = BitConverter.ToUInt16(new byte[2] { response.messageContents[5], response.messageContents[6] }, 0);
            ushort crc = BitConverter.ToUInt16(new byte[2] { response.messageContents[7], response.messageContents[8] }, 0);

            speedData[0].eventTime = (ushort)(bs);
            speedData[0].revCount = crc;

            speedData[0] = speedData[1];
          
            speedData[1].eventTime = (ushort)(bs);
            speedData[1].revCount = crc;

            if(speedData[0].Equals(speedData[1]) == false)
            {
                current_speed = (1.8 * ((speedData[1].revCount - speedData[0].revCount))) / (speedData[1].eventTime - speedData[0].eventTime) * 3600;

                if (current_speed > 100.0 || current_speed < 0)
                    current_speed = 0;

                Debug.Log("Speed : " + current_speed.ToString() + " km/h");
            }
        }
    }

    void cadenceSensorResponse(ANT_Response response)
    {
        if (response.responseID == 0x4E && response.messageContents[1] == 0x00)
        {
            ushort bs = BitConverter.ToUInt16(new byte[2] { response.messageContents[5], response.messageContents[6] }, 0);
            ushort crc = BitConverter.ToUInt16(new byte[2] { response.messageContents[7], response.messageContents[8] }, 0);

            rpmData[0].eventTime = (ushort)(bs);
            rpmData[0].revCount = crc;

            rpmData[0] = rpmData[1];

            rpmData[1].eventTime = (ushort)(bs);
            rpmData[1].revCount = crc;

            if (rpmData[0].Equals(rpmData[1]) == false)
            {
                current_rpm = (60 * ((rpmData[1].revCount - rpmData[0].revCount)) * 1024) / (rpmData[1].eventTime - rpmData[0].eventTime);

                Debug.Log("RPM : " + current_rpm.ToString() + " rpm");
            }
        }

    }

    String decodeDeviceFeedback(ANT_Response response)
    {
        string toDisplay = "Device: ";

        if (response.responseID == (byte)ANT_ReferenceLibrary.ANTMessageID.RESPONSE_EVENT_0x40)
        {
            toDisplay += (ANT_ReferenceLibrary.ANTMessageID)response.messageContents[1] + ", Ch:" + response.messageContents[0];
            if ((ANT_ReferenceLibrary.ANTEventID)response.messageContents[2] != ANT_ReferenceLibrary.ANTEventID.RESPONSE_NO_ERROR_0x00)
                toDisplay += Environment.NewLine + ((ANT_ReferenceLibrary.ANTEventID)response.messageContents[2]).ToString();
        }
        else   //If the message is not an event, we just show the messageID
            toDisplay += ((ANT_ReferenceLibrary.ANTMessageID)response.responseID).ToString();

        toDisplay += Environment.NewLine + "::" + Convert.ToString(response.responseID, 16) + ", " + BitConverter.ToString(response.messageContents) + Environment.NewLine;
        return toDisplay;
    }

    String decodeChannelFeedback(ANT_Response response)
    {
        StringBuilder stringToPrint;    //We use a stringbuilder for speed and better memory usage, but, it doesn't really matter for the demo.
        stringToPrint = new StringBuilder("Channel: ", 100); //Begin the string and allocate some more space

        if (response.responseID == (byte)ANT_ReferenceLibrary.ANTMessageID.RESPONSE_EVENT_0x40)
            stringToPrint.AppendLine(((ANT_ReferenceLibrary.ANTEventID)response.messageContents[2]).ToString());
        else   //This is a receive event, so display the ID
            stringToPrint.AppendLine("Received " + ((ANT_ReferenceLibrary.ANTMessageID)response.responseID).ToString());

        stringToPrint.Append("  :: ");
        stringToPrint.Append(Convert.ToString(response.responseID, 16));
        stringToPrint.Append(", ");
        stringToPrint.Append(BitConverter.ToString(response.messageContents) + Environment.NewLine);
        return stringToPrint.ToString();
    }

}
