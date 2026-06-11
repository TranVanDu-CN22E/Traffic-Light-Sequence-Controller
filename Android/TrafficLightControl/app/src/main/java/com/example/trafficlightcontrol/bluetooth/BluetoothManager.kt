package com.example.trafficlightcontrol.bluetooth

import android.annotation.SuppressLint
import android.bluetooth.BluetoothAdapter
import android.bluetooth.BluetoothDevice
import android.bluetooth.BluetoothSocket
import android.util.Log
import java.io.BufferedReader
import java.io.InputStream
import java.io.InputStreamReader
import java.io.OutputStream
import java.util.UUID

class BluetoothManager {

    private var socket: BluetoothSocket? = null
    private var output: OutputStream? = null
    private var input: InputStream? = null

    companion object {
        private val UUID_SPP =
            UUID.fromString("00001101-0000-1000-8000-00805F9B34FB")
    }

    @SuppressLint("MissingPermission")
    fun connect(device: BluetoothDevice): Boolean {
        return try {

            socket =
                device.createRfcommSocketToServiceRecord(UUID_SPP)

            socket?.connect()

            output = socket?.outputStream
            input = socket?.inputStream

            true

        } catch (e: Exception) {

            e.printStackTrace()
            false
        }
    }

    fun send(message: String) {

        output?.write("$message\n".toByteArray())
        output?.flush()
    }
    fun startListening(
        onMessage: (String) -> Unit
    ) {

        Thread {

            try {
                val reader =
                    BufferedReader(
                        InputStreamReader(input)
                    )

                while (true) {

                    val line =
                        reader.readLine() ?: break

                    Log.d("BT", "Nhan day du: $line")

                    onMessage(line)
                }

            } catch (e: Exception) {
                e.printStackTrace()
            }

        }.start()
    }
    fun disconnect() {
        socket?.close()
    }
}