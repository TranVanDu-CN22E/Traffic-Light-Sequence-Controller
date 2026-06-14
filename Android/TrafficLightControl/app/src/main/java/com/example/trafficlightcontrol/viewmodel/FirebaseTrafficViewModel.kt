package com.example.trafficlightcontrol.viewmodel

import androidx.compose.runtime.mutableStateListOf
import androidx.lifecycle.ViewModel
import com.example.trafficlightcontrol.firebase.FirebaseManager
import com.google.firebase.database.*
import com.google.firebase.database.FirebaseDatabase
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow

class FirebaseTrafficViewModel : ViewModel() {


    private val database =
        FirebaseDatabase.getInstance()
    private val selectedRoutes = mutableStateListOf<String>()
    private val trafficRef =
        database.getReference("Arduino_Data")


    private val _trafficData =
        MutableStateFlow("Chưa có dữ liệu")

    val trafficData: StateFlow<String>
            = _trafficData

    private val _connected =
        MutableStateFlow(false)

    val connected: StateFlow<Boolean>
            = _connected

    init {
         listenFirebase()
    }

    private fun listenFirebase() {

        trafficRef.addValueEventListener(
            object : ValueEventListener {

                override fun onDataChange(
                    snapshot: DataSnapshot
                ) {

                    val value =
                        snapshot.getValue(String::class.java)

                    _trafficData.value =
                        value ?: "Không có dữ liệu"

                    _connected.value = true
                }

                override fun onCancelled(
                    error: DatabaseError
                ) {

                    _connected.value = false

                    _trafficData.value =
                        "Lỗi: ${error.message}"
                }
            }
        )

    }
    fun flash() {
        FirebaseManager.send("FLASH")
    }

    fun normal() {
        FirebaseManager.send("NORMAL")
    }

    fun powerOn() {
        FirebaseManager.send("POWER_ON")
    }

    fun powerOff() {
        FirebaseManager.send("POWER_OFF")
    }

    fun setTime(
        a: Int,
        yellow: Int,
        b: Int
    ) {

        FirebaseManager.send(
            "SET,$a,$yellow,$b"
        )
    }

    fun toggleRoute(route: String) {
        if (selectedRoutes.contains(route)) {
            selectedRoutes.remove(route)
        } else {
            selectedRoutes.add(route)
        }

        val command = selectedRoutes.sorted().joinToString("")
        FirebaseManager.send("ROUTE,$command")
    }

    fun isSelected(route: String): Boolean {
        return selectedRoutes.contains(route)
    }
}