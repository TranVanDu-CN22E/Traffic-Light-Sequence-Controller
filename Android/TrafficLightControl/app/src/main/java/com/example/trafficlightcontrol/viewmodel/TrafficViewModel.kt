package com.example.trafficlightcontrol.viewmodel

import android.Manifest
import android.annotation.SuppressLint
import android.bluetooth.BluetoothAdapter
import android.bluetooth.BluetoothDevice
import androidx.annotation.RequiresPermission
import androidx.lifecycle.ViewModel
import com.example.trafficlightcontrol.bluetooth.BluetoothManager
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow

class TrafficViewModel : ViewModel() {

    private val bluetoothManager = BluetoothManager()
    private val _devices =
        MutableStateFlow<List<BluetoothDevice>>(emptyList())

    val devices: StateFlow<List<BluetoothDevice>>
            = _devices

    private val _connected =
        MutableStateFlow(false)

    val connected: StateFlow<Boolean>
            = _connected

    private val _deviceName =
        MutableStateFlow("Chưa kết nối")

    val deviceName: StateFlow<String>
            = _deviceName
    private val _response =
        MutableStateFlow("Chưa có phản hồi")

    val response: StateFlow<String>
            = _response
    fun updateResponse(msg: String) {
        _response.value = msg
    }
    @RequiresPermission(Manifest.permission.BLUETOOTH_CONNECT)
    fun connect(device: BluetoothDevice) {

        val success =
            bluetoothManager.connect(device)

        _connected.value = success

        if(success)
        {
            _deviceName.value =
                device.name ?: "Unknown"
            bluetoothManager.startListening {

                _response.value = it
            }
        }
    }

    @SuppressLint("MissingPermission")
    fun loadDevices() {

        val adapter =
            BluetoothAdapter.getDefaultAdapter()

        _devices.value =
            adapter.bondedDevices.toList()
    }
    fun flash() {
        bluetoothManager.send("FLASH")
    }

    fun normal() {
        bluetoothManager.send("NORMAL")
    }

    fun powerOn() {
        bluetoothManager.send("POWER_ON")
    }

    fun powerOff() {
        bluetoothManager.send("POWER_OFF")
    }

    fun setTime(
        a: Int,
        yellow: Int,
        b: Int
    ) {

        bluetoothManager.send(
            "SET,$a,$yellow,$b"
        )
    }
}