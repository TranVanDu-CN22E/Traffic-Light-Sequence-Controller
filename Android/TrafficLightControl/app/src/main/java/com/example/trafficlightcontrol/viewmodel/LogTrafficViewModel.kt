package com.example.trafficlightcontrol.viewmodel

import android.util.Log
import androidx.lifecycle.ViewModel
import com.google.firebase.Timestamp
import com.google.firebase.firestore.FirebaseFirestore
import com.google.firebase.firestore.Query
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow



class LogTrafficViewModel : ViewModel() {

    private val firestore = FirebaseFirestore.getInstance()

    private val _logsState =
        MutableStateFlow<List<TrafficLog>>(emptyList())
    val logsState: StateFlow<List<TrafficLog>> = _logsState

    init {
        fetchTrafficLogs()
    }

    private fun fetchTrafficLogs() {
        firestore.collection("logs")
            .orderBy("timestamp", Query.Direction.DESCENDING)
            .addSnapshotListener { snapshot, error ->

                if (error != null) {
                    Log.e("Firestore", error.message ?: "Unknown error")
                    return@addSnapshotListener
                }

                val logs = snapshot?.documents
                    ?.mapNotNull { it.toObject(TrafficLog::class.java) }
                    ?: emptyList()

                _logsState.value = logs
            }
    }
}
data class TrafficLog(
    val message: String = "",
    val timestamp: Timestamp? = null
)
