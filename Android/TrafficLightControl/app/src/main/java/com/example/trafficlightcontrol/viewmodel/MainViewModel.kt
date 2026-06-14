package com.example.trafficlightcontrol.viewmodel

import androidx.lifecycle.ViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow

class MainViewModel : ViewModel() {

    private val _bluetoothConnected = MutableStateFlow(false)
    val bluetoothConnected: StateFlow<Boolean> = _bluetoothConnected

    private val _internetConnected = MutableStateFlow(true)
    val internetConnected: StateFlow<Boolean> = _internetConnected

    fun updateBluetoothStatus(status: Boolean) {
        _bluetoothConnected.value = status
    }

    fun updateInternetStatus(status: Boolean) {
        _internetConnected.value = status
    }
}