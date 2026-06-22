import { useRouter } from 'expo-router';
import React, { useEffect, useState } from 'react';
import {
  SafeAreaView,
  ScrollView,
  StatusBar,
  StyleSheet,
  Text,
  TextInput,
  TouchableOpacity,
  View
} from 'react-native';

import TrafficService from '@/services/traffic.service';

export default function InternetControlScreen() {
  const router = useRouter();

  // ===== Arduino feedback =====
  const [feedback, setFeedback] = useState('');

  // ===== time config =====
  const [aXanh, setAXanh] = useState('35');
  const [vang, setVang] = useState('5');
  const [bXanh, setBXanh] = useState('55');

  // ===== route state =====
  const [routes, setRoutes] = useState<string[]>([]);

  // ===== realtime Arduino listener =====
  useEffect(() => {
    const unsub = TrafficService.listenArduino(setFeedback);
    return unsub;
  }, []);

  // ===== ROUTE TOGGLE LOGIC =====
  const toggleRoute = (r: string) => {
    setRoutes(prev =>
      prev.includes(r)
        ? prev.filter(x => x !== r)   // bỏ chọn
        : [...prev, r]                // thêm chọn
    );

    // gửi ngay xuống Firebase (realtime)
    TrafficService.toggleRoute(r);
  };

  // ===== UI =====
  return (
    <SafeAreaView style={styles.container}>
      <StatusBar backgroundColor="#F5ECFC" barStyle="dark-content" />

      {/* HEADER */}
      <View style={styles.header}>
        <Text style={styles.headerTitle}>Bộ Điều Khiển Đèn Giao Thông</Text>
      </View>

      <ScrollView contentContainerStyle={styles.content}>

        {/* FEEDBACK */}
        <View style={styles.feedbackCard}>
          <Text style={styles.label}>Phản hồi từ Arduino:</Text>
          <Text style={styles.feedbackText}>
            {feedback || 'Da nhan tin hieu khoi dong.'}
          </Text>
        </View>

        {/* CONTROL SYSTEM */}
        <Text style={styles.sectionTitle}>Điều khiển hệ thống</Text>

        <View style={styles.row}>
          <TouchableOpacity
            style={styles.btnGreen}
            onPress={() => TrafficService.powerOn()}
          >
            <Text style={styles.btnTextLight}>Bật Hệ Thống</Text>
          </TouchableOpacity>

          <TouchableOpacity
            style={styles.btnRed}
            onPress={() => TrafficService.powerOff()}
          >
            <Text style={styles.btnTextLight}>Tắt Hệ Thống</Text>
          </TouchableOpacity>
        </View>

        <View style={styles.row}>
          <TouchableOpacity
            style={styles.btnPurpleLight}
            onPress={() => TrafficService.normal()}
          >
            <Text style={styles.btnTextPurple}>Bình Thường</Text>
          </TouchableOpacity>

          <TouchableOpacity
            style={styles.btnYellowLight}
            onPress={() => TrafficService.flash()}
          >
            <Text style={styles.btnTextYellow}>Nháy Vàng</Text>
          </TouchableOpacity>
        </View>

        {/* TIME CONFIG CARD */}
        <View style={styles.timeConfigCard}>
          <Text style={styles.cardTitle}>Cấu hình thời gian (giây)</Text>
          
          <View style={styles.inputGroupRow}>
            {/* Input A Xanh */}
            <View style={styles.inputContainer}>
              <View style={styles.inputLabelBg}>
                <Text style={styles.inputLabelText}>A Xanh</Text>
              </View>
              <TextInput
                style={styles.input}
                value={aXanh}
                onChangeText={setAXanh}
                keyboardType="numeric"
              />
            </View>

            {/* Input Vàng */}
            <View style={styles.inputContainer}>
              <View style={styles.inputLabelBg}>
                <Text style={styles.inputLabelText}>Vàng</Text>
              </View>
              <TextInput
                style={styles.input}
                value={vang}
                onChangeText={setVang}
                keyboardType="numeric"
              />
            </View>

            {/* Input B Xanh */}
            <View style={styles.inputContainer}>
              <View style={styles.inputLabelBg}>
                <Text style={styles.inputLabelText}>B Xanh</Text>
              </View>
              <TextInput
                style={styles.input}
                value={bXanh}
                onChangeText={setBXanh}
                keyboardType="numeric"
              />
            </View>
          </View>

          <TouchableOpacity
            style={styles.submitBtn}
            onPress={() =>
              TrafficService.setTime(
                Number(aXanh),
                Number(vang),
                Number(bXanh)
              )
            }
          >
            <Text style={styles.submitBtnText}>Cập nhật Thời Gian</Text>
          </TouchableOpacity>
        </View>

        {/* ROUTE CONTROL */}
        <Text style={styles.sectionTitle}>Chuyển hướng</Text>

        <View style={styles.routeRow}>
          {['A', 'B', 'C', 'D'].map(r => (
            <TouchableOpacity
              key={r}
              onPress={() => toggleRoute(r)}
              style={[
                styles.routeBtn,
                routes.includes(r) && styles.routeActive
              ]}
            >
              <Text style={styles.routeText}>{r}</Text>
            </TouchableOpacity>
          ))}
        </View>

      </ScrollView>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#FAF7FD' // Màu nền tổng thể tím nhạt
  },

  header: {
    backgroundColor: '#F5ECFC',
    paddingTop: 60,
    paddingBottom: 20,
    paddingHorizontal: 20,
    alignItems: 'flex-start',
  },

  headerTitle: {
    fontSize: 22,
    fontWeight: 'bold',
    color: '#31106A',
  },

  content: {
    padding: 20,
    paddingBottom: 40
  },

  feedbackCard: {
    backgroundColor: '#F2E9F4',
    padding: 18,
    borderRadius: 20,
    marginBottom: 25
  },

  label: {
    fontSize: 14,
    color: '#8A8094',
    marginBottom: 6
  },

  feedbackText: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#654B9E'
  },

  sectionTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#1C1B1F',
    marginBottom: 15,
    marginTop: 5
  },

  row: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 15
  },

  btnGreen: {
    flex: 1,
    backgroundColor: '#4CB050',
    paddingVertical: 14,
    borderRadius: 25,
    alignItems: 'center',
    marginRight: 8
  },

  btnRed: {
    flex: 1,
    backgroundColor: '#FF4D3D',
    paddingVertical: 14,
    borderRadius: 25,
    alignItems: 'center',
    marginLeft: 8
  },

  btnPurpleLight: {
    flex: 1,
    backgroundColor: '#F5ECFC',
    borderWidth: 1,
    borderColor: '#E6D3F7',
    paddingVertical: 14,
    borderRadius: 25,
    alignItems: 'center',
    marginRight: 8
  },

  btnYellowLight: {
    flex: 1,
    backgroundColor: '#FFF9EE',
    borderWidth: 1,
    borderColor: '#FFECC7',
    paddingVertical: 14,
    borderRadius: 25,
    alignItems: 'center',
    marginLeft: 8
  },

  btnTextLight: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: 'bold'
  },

  btnTextPurple: {
    color: '#654B9E',
    fontSize: 16,
    fontWeight: 'bold'
  },

  btnTextYellow: {
    color: '#FFA114',
    fontSize: 16,
    fontWeight: 'bold'
  },

  // Khối cấu hình thời gian
  timeConfigCard: {
    backgroundColor: '#ECE6F0',
    borderRadius: 20,
    padding: 16,
    marginBottom: 25,
    marginTop: 10
  },

  cardTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#1C1B1F',
    marginBottom: 20
  },

  inputGroupRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 20
  },

  inputContainer: {
    width: '31%',
    position: 'relative',
  },

  inputLabelBg: {
    position: 'absolute',
    top: -10,
    left: 12,
    backgroundColor: '#ECE6F0', // Trùng màu nền card để đè lên viền
    paddingHorizontal: 6,
    zIndex: 1,
  },

  inputLabelText: {
    fontSize: 12,
    color: '#49454F'
  },

  input: {
    borderWidth: 1,
    borderColor: '#79747E',
    borderRadius: 8,
    paddingVertical: 10,
    paddingHorizontal: 12,
    fontSize: 18,
    color: '#1C1B1F',
    backgroundColor: 'transparent',
    textAlign: 'left'
  },

  submitBtn: {
    backgroundColor: '#654B9E',
    paddingVertical: 14,
    borderRadius: 25,
    alignItems: 'center'
  },

  submitBtnText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: 'bold'
  },

  routeRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 5
  },

  routeBtn: {
    width: '22%',
    height: 48,
    borderRadius: 24,
    backgroundColor: '#8E8E93',
    justifyContent: 'center',
    alignItems: 'center'
  },

  routeActive: {
    backgroundColor: '#654B9E' // Bạn có thể đổi màu khi kích hoạt nếu muốn
  },

  routeText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: 'bold'
  }
});