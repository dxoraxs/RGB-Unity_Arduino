using ArduinoBluetoothAPI;
using EasyMobile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BluetoothController : MonoBehaviour
{
    [SerializeField] private Color connected;
    [SerializeField] private Color notConnected;
    [SerializeField] private Image bluetoothImage;
    [SerializeField] private Text nameDevice;
    private BluetoothHelper bluetoothHelper;
    private string deviceName;
    private string received_message;

    public void StartConnect()
    {
        if (!bluetoothHelper.isConnected())
        {
            if (bluetoothHelper.isDevicePaired())
            {
                isConnected = true;
                bluetoothHelper.Connect();
            }
        }
        else
        {
            bluetoothImage.color = notConnected;
            bluetoothHelper.Disconnect();
        }
    }
    [SerializeField] private GUIStyle guiStyle = new GUIStyle(); 
    private void Start()
    {
        deviceName = "JDY-31-SPP";

        try
        {
            bluetoothHelper = BluetoothHelper.GetInstance(deviceName);
            bluetoothHelper.OnConnected += OnConnected;
            bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
            bluetoothHelper.OnDataReceived += OnMessageReceived;
            bluetoothHelper.setTerminatorBasedStream("\n");

            LinkedList<BluetoothDevice> ds = bluetoothHelper.getPairedDevicesList();
        }
        catch (Exception ex)
        {
            debugText+= "\nERROR: "+ex.Message;
        }
    }

    private void OnMessageReceived()
    {
        received_message = bluetoothHelper.Read();
        debugText += "\nENTER MESSAGE: "+ received_message;
    }

    private bool isAnimation;
    private float time = 0;
    private bool isConnected;

    private void Update()
    {
        if (!isConnected) return;

        bluetoothImage.color = Color.Lerp(Color.white, connected, time);
        time += (isAnimation ? -2 : 2) * Time.deltaTime;
        if (time > 1 || time < 0 )
        {
            isAnimation = !isAnimation;
        }
    }
    
    public void SendData(string data) => StartCoroutine(SendDataToArduino(data));
    
    private IEnumerator SendDataToArduino(string data)
    { 
        debugText += $"\nSend message: {data}";
        for (int i = 0; i < data.Length;)
        {
            yield return null;
                var dateTime = DateTime.Now;
                var dateString = $"[{dateTime.Minute}:{dateTime.Second}:{dateTime.Millisecond}]: ";
            if (bluetoothHelper.isConnected())
            {
                bluetoothHelper.SendData(data[i].ToString());
                i++;
            }
            else
            {
                debugText += $"\n{dateString}skip frame, bluetooth not ready";
            }
        }
    }

    private string debugText;
    
    /*
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, Screen.width - 20, Screen.height - 20), debugText, guiStyle);

        if (GUI.Button(new Rect(Screen.width - 250, Screen.height - 250, 250, 250), ""))
            debugText = "";
    }
    */

    private void OnConnected()
    {
        isConnected = false;
        try
        {
            bluetoothHelper.StartListening();

            bluetoothImage.color = connected;
            nameDevice.text = deviceName;
            //NativeUI.ShowToast("Connected");
        }
        catch (Exception ex)
        {
            bluetoothImage.color = notConnected;
            debugText+= "\n"+ex.Message;
        }
    }

    private void OnConnectionFailed()
    {
        isConnected = false;
        bluetoothImage.color = notConnected;
        debugText+= "\nConnection Failed";
    }
}
