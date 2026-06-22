import { ThemedView } from '@/components/themed-view';
import { MaterialCommunityIcons } from '@expo/vector-icons';
import { useRouter } from 'expo-router';
import React, { useEffect, useState } from 'react';
import { StatusBar, StyleSheet, Text, TouchableOpacity, View } from 'react-native';

import TrafficService from '@/services/traffic.service';

export default function HomeScreen() {
  const router = useRouter();
  const [arduinoStatus, setArduinoStatus] = useState('');

  useEffect(() => {
    const unsub = TrafficService.listenArduino(setArduinoStatus);
    return unsub;
  }, []);

  const handlePress = (item: string) => {
    if (item === 'Internet') router.push('/internet');
    if (item === 'History') router.push('/history');
  };

  return (
    <ThemedView style={styles.container}>
      <StatusBar backgroundColor="#F1EBF6" barStyle="dark-content" />

      <View style={styles.header}>
        <MaterialCommunityIcons name="traffic-light" size={28} color="#2A1B4E" />
        <Text style={styles.headerTitle}>Traffic Control Home</Text>
      </View>

      <View style={styles.content}>
        <Text style={styles.sectionTitle}>Trạng thái hệ thống</Text>

        {/* Bluetooth giữ nguyên */}
        <TouchableOpacity style={styles.card}>
          <View style={[styles.iconContainer, { backgroundColor: '#E8DEF8' }]}>
            <MaterialCommunityIcons name="bluetooth" size={24} color="#2A1B4E" />
          </View>
          <View style={styles.cardTextContainer}>
            <Text style={styles.cardTitle}>Bluetooth</Text>
            <Text style={{ color: '#B3261E' }}>Chưa kết nối</Text>
          </View>
        </TouchableOpacity>

        {/* INTERNET realtime */}
        <TouchableOpacity onPress={() => handlePress('Internet')} style={styles.card}>
          <View style={[styles.iconContainer, { backgroundColor: '#E8DEF8' }]}>
            <MaterialCommunityIcons name="cloud" size={24} color="#2A1B4E" />
          </View>

          <View style={styles.cardTextContainer}>
            <Text style={styles.cardTitle}>Internet</Text>
            <Text style={{ color: '#6750A4' }}>
              {arduinoStatus ? 'Đang online' : 'Mất kết nối'}
            </Text>
          </View>

          <MaterialCommunityIcons name="chevron-right" size={24} color="#49454F" />
        </TouchableOpacity>

        <Text style={styles.sectionTitle}>Tiện ích</Text>

        <TouchableOpacity onPress={() => handlePress('History')} style={styles.card}>
          <View style={[styles.iconContainer, { backgroundColor: '#E8DEF8' }]}>
            <MaterialCommunityIcons name="history" size={24} color="#2A1B4E" />
          </View>

          <View style={styles.cardTextContainer}>
            <Text style={styles.cardTitle}>Lịch sử thao tác</Text>
            <Text style={styles.cardSubtitle}>Xem nhật ký hệ thống</Text>
          </View>

          <MaterialCommunityIcons name="chevron-right" size={24} color="#49454F" />
        </TouchableOpacity>
      </View>
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#FAF7FD' },
  header: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#F1EBF6', 
    paddingTop: 50, 
    paddingBottom: 16,
    paddingHorizontal: 20,
    borderBottomWidth: 1,
    borderBottomColor: '#E6E0EC',
  },
  headerTitle: { fontSize: 20, fontWeight: 'bold', color: '#2A1B4E', marginLeft: 12 },
  content: { paddingHorizontal: 20, paddingTop: 20 },
  sectionTitle: { fontSize: 22, fontWeight: 'bold', color: '#6750A4', marginBottom: 16, marginTop: 10 },
  card: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#F7F2FA', 
    padding: 16,
    borderRadius: 16, 
    marginBottom: 16,
    // Đổ bóng nhẹ bằng thuộc tính Native iOS
    shadowColor: "#000",
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.1,
    shadowRadius: 1.41,
    elevation: 2,
  },
  iconContainer: { width: 48, height: 48, borderRadius: 24, justifyContent: 'center', alignItems: 'center' },
  cardTextContainer: { flex: 1, marginLeft: 16 },
  cardTitle: { fontSize: 16, fontWeight: 'bold', color: '#1D1B20' },
  cardSubtitle: { fontSize: 14, color: '#49454F', marginTop: 2 },
  divider: { height: 1, backgroundColor: '#E6E0EC', marginVertical: 15 },
});
