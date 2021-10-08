using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArduinoBluetoothAPI;
using System;
using EasyMobile;

public class BLEServicesManager : MonoBehaviour
{
    private BluetoothHelper bluetoothHelper;
    private float timer;

    void Start()
    {
        timer = 0;
        try
        {
            Debug.Log("HI");
            BluetoothHelper.BLE = true; //use Bluetooth Low Energy Technology
            bluetoothHelper = BluetoothHelper.GetInstance("TEST");

#if UNITY_EDITOR
            Debug.Log(bluetoothHelper.getDeviceName());
#endif
#if PLATFORM_ANDROID
            NativeUI.ShowToast(bluetoothHelper.getDeviceName());
#endif
            bluetoothHelper.OnConnected += () =>
            {
#if UNITY_EDITOR
                Debug.Log("Connected");
#endif
#if PLATFORM_ANDROID
                NativeUI.ShowToast("Connected");
#endif
                sendData();
            };
            bluetoothHelper.OnConnectionFailed += () =>
            {
#if UNITY_EDITOR
                Debug.Log("Connection failed");
#endif
#if PLATFORM_ANDROID
                NativeUI.ShowToast("Connection failed");
#endif
            };
            bluetoothHelper.OnScanEnded += OnScanEnded;
            bluetoothHelper.OnServiceNotFound += (serviceName) =>
            {
#if UNITY_EDITOR
                Debug.Log(serviceName);
#endif
#if PLATFORM_ANDROID
                NativeUI.ShowToast(serviceName);
#endif
            };
            bluetoothHelper.OnCharacteristicNotFound += (serviceName, characteristicName) =>
            {
#if UNITY_EDITOR
                Debug.Log(characteristicName);
#endif
#if PLATFORM_ANDROID
                NativeUI.ShowToast(characteristicName);
#endif
            };
            bluetoothHelper.OnCharacteristicChanged += (value, characteristic) =>
            {
                Debug.Log(characteristic.getName());
                Debug.Log(System.Text.Encoding.ASCII.GetString(value));
            };

            // BluetoothHelperService service = new BluetoothHelperService("FFE0");
            // service.addCharacteristic(new BluetoothHelperCharacteristic("FFE1"));
            // BluetoothHelperService service2 = new BluetoothHelperService("180A");
            // service.addCharacteristic(new BluetoothHelperCharacteristic("2A24"));
            // bluetoothHelper.Subscribe(service);
            // bluetoothHelper.Subscribe(service2);
            // bluetoothHelper.ScanNearbyDevices();

            BluetoothHelperService service = new BluetoothHelperService("19B10000-E8F2-537E-4F6C-D104768A1214");
            service.addCharacteristic(new BluetoothHelperCharacteristic("19B10001-E8F2-537E-4F6C-D104768A1214"));
            //BluetoothHelperService service2 = new BluetoothHelperService("180A");
            //service.addCharacteristic(new BluetoothHelperCharacteristic("2A24"));
            bluetoothHelper.Subscribe(service);
            //bluetoothHelper.Subscribe(service2);
            bluetoothHelper.ScanNearbyDevices();
        }
        catch (Exception ex)
        {
#if UNITY_EDITOR
            Debug.Log(ex.Message);
#endif
#if PLATFORM_ANDROID
            NativeUI.ShowToast(ex.Message);
#endif
        }
    }

    private void OnScanEnded(LinkedList<BluetoothDevice> devices)
    {
        Debug.Log("FOund " + devices.Count);
        if (devices.Count == 0)
        {
            bluetoothHelper.ScanNearbyDevices();
            return;
        }

        try
        {
            bluetoothHelper.setDeviceName("KURV_Master");
            // bluetoothHelper.setDeviceName("HC-08");
            bluetoothHelper.Connect();
#if UNITY_EDITOR
            Debug.Log("Connecting");
#endif
#if PLATFORM_ANDROID
            NativeUI.ShowToast("Connecting");
#endif
        }
        catch (Exception ex)
        {
#if UNITY_EDITOR
            Debug.Log(ex.Message);
#endif
#if PLATFORM_ANDROID
            NativeUI.ShowToast(ex.Message);
#endif
        }
    }

    void OnDestroy()
    {
        if (bluetoothHelper != null)
            bluetoothHelper.Disconnect();
    }

    void Update()
    {
        if (bluetoothHelper == null)
            return;
        if (!bluetoothHelper.isConnected())
            return;
        timer += Time.deltaTime;

        if (timer < 5)
            return;
        timer = 0;
        sendData();
    }

    void sendData()
    {
        // Debug.Log("Sending");
        // BluetoothHelperCharacteristic ch = new BluetoothHelperCharacteristic("FFE1");
        // ch.setService("FFE0"); //this line is mandatory!!!
        // bluetoothHelper.WriteCharacteristic(ch, new byte[]{0x44, 0x55, 0xff});

#if UNITY_EDITOR
        Debug.Log("Sending");
#endif
#if PLATFORM_ANDROID
        NativeUI.ShowToast("Sending");
#endif
        BluetoothHelperCharacteristic ch = new BluetoothHelperCharacteristic("19B10001-E8F2-537E-4F6C-D104768A1214");
        ch.setService("19B10000-E8F2-537E-4F6C-D104768A1214"); //this line is mandatory!!!
        bluetoothHelper.WriteCharacteristic(ch, "10001000"); //string: 10001000 is this binary? no, as string.
    }

    void read()
    {
        BluetoothHelperCharacteristic ch = new BluetoothHelperCharacteristic("2A24");
        ch.setService("180A"); //this line is mandatory!!!

        bluetoothHelper.ReadCharacteristic(ch);
        //Debug.Log(System.Text.Encoding.ASCII.GetString(x));
    }
}