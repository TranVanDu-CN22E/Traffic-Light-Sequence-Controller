package com.example.trafficlightcontrol

import android.Manifest
import android.os.Build
import android.os.Bundle
import androidx.activity.compose.setContent
import androidx.appcompat.app.AppCompatActivity
import androidx.activity.enableEdgeToEdge
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.example.trafficlightcontrol.ui.BluetoothTrafficScreen
import com.example.trafficlightcontrol.ui.FirebaseConnectScreen
import com.example.trafficlightcontrol.ui.LogTrafficScreen
import com.example.trafficlightcontrol.ui.MainScreen
import com.example.trafficlightcontrol.viewmodel.BluetoothTrafficViewModel
import com.example.trafficlightcontrol.viewmodel.FirebaseTrafficViewModel
import com.example.trafficlightcontrol.viewmodel.LogTrafficViewModel
import com.example.trafficlightcontrol.viewmodel.MainViewModel

class MainActivity : AppCompatActivity() {
    private val requestPermissionLauncher =
        registerForActivityResult(
            ActivityResultContracts.RequestMultiplePermissions()
        ) { }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.S) {
            requestPermissionLauncher.launch(
                arrayOf(
                    Manifest.permission.BLUETOOTH_CONNECT,
                    Manifest.permission.BLUETOOTH_SCAN
                )
            )
        }

        setContent {
            val navController = rememberNavController()
            val bluetoothTrafficViewModel: BluetoothTrafficViewModel = viewModel()
            val firebaseTrafficViewModel: FirebaseTrafficViewModel = viewModel()
            val logTrafficViewModel: LogTrafficViewModel = viewModel()
            val mainViewModel: MainViewModel = viewModel()

            // Đồng bộ trạng thái từ các ViewModel khác sang MainViewModel
            val bluetoothConnected by bluetoothTrafficViewModel.connected.collectAsState()
            val firebaseConnected by firebaseTrafficViewModel.connected.collectAsState()

            LaunchedEffect(bluetoothConnected) {
                mainViewModel.updateBluetoothStatus(bluetoothConnected)
            }
            LaunchedEffect(firebaseConnected) {
                mainViewModel.updateInternetStatus(firebaseConnected)
            }

            MaterialTheme {
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    NavHost(navController = navController, startDestination = "main") {
                        composable("main") {
                            MainScreen(
                                vm = mainViewModel,
                                onBluetoothClick = { navController.navigate("bluetooth") },
                                onFirebaseClick = { navController.navigate("firebase") },
                                onHistoryClick = { navController.navigate("log")}
                            )
                        }
                        composable("bluetooth") {
                            BluetoothTrafficScreen(vm = bluetoothTrafficViewModel)
                        }

                        composable("firebase") {
                            FirebaseConnectScreen(vm = firebaseTrafficViewModel)
                        }
                        composable("log") {
                            LogTrafficScreen(viewModel = logTrafficViewModel)
                        }
                    }
                }
            }
        }
    }
}
