package com.example.trafficlightcontrol.firebase

import android.util.Log
import com.google.firebase.database.FirebaseDatabase

object FirebaseManager {
    private val database = FirebaseDatabase.getInstance()

    private val commandRef1 =
        database.getReference("Nut_Bam")

    fun send(command: String) {
        commandRef1.setValue(command)
            .addOnSuccessListener {
                Log.d(
                    "FirebaseManager",
                    "Đã gửi: $command"
                )
            }
            .addOnFailureListener { e ->
                Log.e(
                    "FirebaseManager",
                    "Lỗi gửi dữ liệu",
                    e
                )
            }
    }
}