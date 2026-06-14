package com.example.trafficlightcontrol.ui

import android.annotation.SuppressLint
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
import com.example.trafficlightcontrol.viewmodel.BluetoothTrafficViewModel

@SuppressLint("MissingPermission")
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun BluetoothTrafficScreen(
    vm: BluetoothTrafficViewModel
) {
    val devices by vm.devices.collectAsState()
    val connected by vm.connected.collectAsState()
    val deviceName by vm.deviceName.collectAsState()
    val response by vm.response.collectAsState()

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

            // --- 1. THẺ TRẠNG THÁI KẾT NỐI ---
            item {
                Card(
                    modifier = Modifier.fillMaxWidth(),
                    colors = CardDefaults.cardColors(
                        containerColor = if (connected) Color(0xFFE8F5E9) else Color(0xFFFFEBEE)
                    )
                ) {
                    Row(
                        modifier = Modifier
                            .fillMaxWidth()
                            .padding(16.dp),
                        verticalAlignment = Alignment.CenterVertically,
                        horizontalArrangement = Arrangement.SpaceBetween
                    ) {
                        Row(verticalAlignment = Alignment.CenterVertically) {
                            Icon(
                                imageVector = if (connected) Icons.Default.CheckCircle else Icons.Default.Warning,
                                contentDescription = null,
                                tint = if (connected) Color(0xFF2E7D32) else Color(0xFFC62828),
                                modifier = Modifier.size(24.dp)
                            )
                            Spacer(modifier = Modifier.width(8.dp))
                            Text(
                                text = if (connected) "Đã kết nối: $deviceName" else "Chưa kết nối thiết bị",
                                color = if (connected) Color(0xFF2E7D32) else Color(0xFFC62828),
                                fontWeight = FontWeight.Medium
                            )
                        }

                        IconButton(onClick = { vm.loadDevices() }) {
                            Icon(Icons.Default.Refresh, contentDescription = "Tải thiết bị")
                        }
                    }
                }
            }

            // --- 2. THẺ PHẢN HỒI TỪ ARDUINO ---
            item {
                Card(
                    modifier = Modifier.fillMaxWidth(),
                    colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.surfaceVariant)
                ) {
                    Column(modifier = Modifier.padding(16.dp)) {
                        Text("Phản hồi từ Arduino:", fontSize = 12.sp, color = MaterialTheme.colorScheme.onSurfaceVariant)
                        Text(
                            text = response.ifBlank { "Không có phản hồi" },
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

            // --- 5. DANH SÁCH THIẾT BỊ KHẢ DỤNG ---
            item {
                Text("Thiết bị khả dụng (${devices.size})", fontWeight = FontWeight.Bold, fontSize = 16.sp)
            }

            if (devices.isEmpty()) {
                item {
                    Text(
                        "Không tìm thấy thiết bị nào. Hãy ấn nút làm mới ở trên.",
                        style = MaterialTheme.typography.bodyMedium,
                        color = Color.Gray,
                        modifier = Modifier.padding(vertical = 8.dp)
                    )
                }
            } else {
                items(devices) { device ->
                    OutlinedButton(
                        onClick = {
                            vm.connect(device)
                        },
                        modifier = Modifier.fillMaxWidth(),
                        shape = RoundedCornerShape(8.dp)
                    ) {
                        Row(
                            modifier = Modifier.fillMaxWidth(),
                            horizontalArrangement = Arrangement.SpaceBetween,
                            verticalAlignment = Alignment.CenterVertically
                        ) {
                            Text(
                                text = device.name ?: "Thiết bị không tên",
                                fontWeight = FontWeight.SemiBold
                            )
                            Text(
                                text = device.address,
                                fontSize = 12.sp,
                                color = Color.Gray
                            )
                        }
                    }
                }
            }
        }
    }
}


