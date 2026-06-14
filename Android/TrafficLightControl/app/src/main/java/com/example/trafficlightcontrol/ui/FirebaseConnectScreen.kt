package com.example.trafficlightcontrol.ui

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.CheckCircle
import androidx.compose.material.icons.filled.Refresh
import androidx.compose.material.icons.filled.Warning
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import com.example.trafficlightcontrol.viewmodel.FirebaseTrafficViewModel

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun FirebaseConnectScreen(
    vm: FirebaseTrafficViewModel
) {

    val data by vm.trafficData
        .collectAsStateWithLifecycle()

    val connected by vm.connected
        .collectAsStateWithLifecycle()

    var aGreen by remember { mutableStateOf("35") }
    var yellow by remember { mutableStateOf("5") }
    var bGreen by remember { mutableStateOf("55") }

    // Dùng Scaffold để có một thanh TopAppBar chuẩn chỉnh, hiện đại
    Scaffold(
        topBar = {
            TopAppBar(
                title = {
                    Text("Bộ Điều Khiển Đèn Giao Thông", fontWeight = FontWeight.Bold)
                },
                colors = TopAppBarDefaults.topAppBarColors(
                    containerColor = MaterialTheme.colorScheme.primaryContainer,
                    titleContentColor = MaterialTheme.colorScheme.onPrimaryContainer
                )
            )
        }
    ) { innerPadding ->

        // Dùng LazyColumn làm bố cục chính thay cho Column để tránh bị tràn màn hình khi cuộn
        LazyColumn(
            modifier = Modifier
                .fillMaxSize()
                .padding(innerPadding)
                .padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(16.dp)
        ) {

            // --- 2. THẺ PHẢN HỒI TỪ ARDUINO ---
            item {
                Card(
                    modifier = Modifier.fillMaxWidth(),
                    colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.surfaceVariant)
                ) {
                    Column(modifier = Modifier.padding(16.dp)) {
                        Text("Phản hồi từ Arduino:", fontSize = 12.sp, color = MaterialTheme.colorScheme.onSurfaceVariant)
                        Text(
                            text = data.ifBlank { "Không có phản hồi" },
                            fontSize = 16.sp,
                            fontWeight = FontWeight.Bold,
                            color = MaterialTheme.colorScheme.primary
                        )
                    }
                }
            }

            // --- 3. KHU VỰC ĐIỀU KHIỂN NHANH ---
            item {
                Text("Điều khiển hệ thống", fontWeight = FontWeight.Bold, fontSize = 16.sp)
                Spacer(modifier = Modifier.height(8.dp))

                // Hàng 1: Bật / Tắt hệ thống
                Row(
                    modifier = Modifier.fillMaxWidth(),
                    horizontalArrangement = Arrangement.spacedBy(8.dp)
                ) {
                    Button(
                        onClick = { vm.powerOn() },
                        modifier = Modifier.weight(1f),
                        colors = ButtonDefaults.buttonColors(containerColor = Color(0xFF4CAF50)) // Màu xanh lá hiện đại
                    ) {
                        Text("Bật Hệ Thống")
                    }
                    Button(
                        onClick = { vm.powerOff() },
                        modifier = Modifier.weight(1f),
                        colors = ButtonDefaults.buttonColors(containerColor = Color(0xFFF44336)) // Màu đỏ hiện đại
                    ) {
                        Text("Tắt Hệ Thống")
                    }
                }

                Spacer(modifier = Modifier.height(8.dp))

                // Hàng 2: Chế độ Bình thường / Nháy vàng
                Row(
                    modifier = Modifier.fillMaxWidth(),
                    horizontalArrangement = Arrangement.spacedBy(8.dp)
                ) {
                    ElevatedButton(
                        onClick = { vm.normal() },
                        modifier = Modifier.weight(1f)
                    ) {
                        Text("Bình Thường")
                    }
                    ElevatedButton(
                        onClick = { vm.flash() },
                        modifier = Modifier.weight(1f),
                        colors = ButtonDefaults.elevatedButtonColors(contentColor = Color(0xFFFF9800)) // Chữ màu cam nháy vàng
                    ) {
                        Text("Nháy Vàng")
                    }
                }
            }


            // --- 4. CẤU HÌNH THỜI GIAN ĐÈN ---
            item {
                Card(
                    modifier = Modifier.fillMaxWidth(),
                    elevation = CardDefaults.cardElevation(defaultElevation = 2.dp)
                ) {
                    Column(
                        modifier = Modifier.padding(16.dp),
                        verticalArrangement = Arrangement.spacedBy(12.dp)
                    ) {
                        Text("Cấu hình thời gian (giây)", fontWeight = FontWeight.Bold)

                        // Chia 3 ô nhập liệu nằm ngang nhau cho gọn và đẹp mắt
                        Row(
                            modifier = Modifier.fillMaxWidth(),
                            horizontalArrangement = Arrangement.spacedBy(8.dp)
                        ) {
                            OutlinedTextField(
                                value = aGreen,
                                onValueChange = { aGreen = it },
                                label = { Text("A Xanh") },
                                keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Number),
                                modifier = Modifier.weight(1f)
                            )
                            OutlinedTextField(
                                value = yellow,
                                onValueChange = { yellow = it },
                                label = { Text("Vàng") },
                                keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Number),
                                modifier = Modifier.weight(1f)
                            )
                            OutlinedTextField(
                                value = bGreen,
                                onValueChange = { bGreen = it },
                                label = { Text("B Xanh") },
                                keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Number),
                                modifier = Modifier.weight(1f)
                            )
                        }

                        Button(
                            onClick = {
                                val aGreenInt = aGreen.toIntOrNull() ?: 0
                                val yellowInt = yellow.toIntOrNull() ?: 0
                                val bGreenInt = bGreen.toIntOrNull() ?: 0
                                vm.setTime(aGreenInt, yellowInt, bGreenInt)
                            },
                            modifier = Modifier.fillMaxWidth()
                        ) {
                            Text("Cập Nhật Thời Gian")
                        }
                    }
                }
            }

            item {
                Text(
                    "Chuyển hướng",
                    fontWeight = FontWeight.Bold,
                    fontSize = 16.sp
                )

                Spacer(modifier = Modifier.height(8.dp))

                Row(
                    modifier = Modifier.fillMaxWidth(),
                    horizontalArrangement = Arrangement.spacedBy(8.dp)
                ) {
                    listOf("A", "B", "C", "D").forEach { route ->
                        Button(
                            onClick = {
                                vm.toggleRoute(route)
                            },
                            modifier = Modifier.weight(1f),
                            colors = ButtonDefaults.buttonColors(
                                containerColor = if (vm.isSelected(route))
                                    Color(0xFF4CAF50)
                                else
                                    Color.Gray
                            )
                        ) {
                            Text(route)
                        }
                    }
                }
            }
        }
    }
}