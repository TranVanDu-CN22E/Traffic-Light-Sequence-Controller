import { MaterialCommunityIcons } from '@expo/vector-icons';
import { useRouter } from 'expo-router';
import React, { useEffect, useState } from 'react';
import {
  FlatList,
  SafeAreaView,
  StatusBar,
  StyleSheet,
  Text,
  TouchableOpacity,
  View
} from 'react-native';

import LogService from '@/services/log.service';

type LogItem = {
  message: string;
  time: string;
};

export default function HistoryScreen() {
  const router = useRouter();

  const [logs, setLogs] = useState<LogItem[]>([]);

  useEffect(() => {
    const unsub = LogService.listenLogs((data) => {
      const formatted = data.map((item: any) => ({
        message: item.message,
        time: formatTime(item.timestamp),
      }));

      setLogs(formatted);
    });

    return unsub;
  }, []);

  const formatTime = (timestamp: any) => {
    if (!timestamp) return 'N/A';

    const date = timestamp.toDate();

    return new Intl.DateTimeFormat('vi-VN', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
    }).format(date);
  };

  const renderItem = ({ item }: { item: LogItem }) => (
    <View style={styles.logCard}>
      <Text style={styles.logText}>{item.message}</Text>
      <Text style={styles.timeText}>{item.time}</Text>
    </View>
  );

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar backgroundColor="#FDFBFF" barStyle="dark-content" />

      {/* HEADER */}
      <View style={styles.header}>
        <TouchableOpacity onPress={() => router.back()} style={styles.backButton}>
          <MaterialCommunityIcons name="arrow-left" size={24} color="#1D1B20" />
        </TouchableOpacity>

        <Text style={styles.headerTitle}>Lịch sử hoạt động</Text>

        <View style={{ width: 24 }} />
      </View>

      {/* LIST */}
      <FlatList
        data={logs}
        renderItem={renderItem}
        keyExtractor={(_, index) => index.toString()}
        contentContainerStyle={styles.listContent}
        showsVerticalScrollIndicator={false}
      />
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#FDFBFF',
  },

  header: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    backgroundColor: '#FDFBFF',
    paddingTop: 50,
    paddingBottom: 16,
    paddingHorizontal: 16,
    borderBottomWidth: 1,
    borderBottomColor: '#E6E0EC',
  },

  backButton: {
    padding: 4,
  },

  headerTitle: {
    fontSize: 22,
    fontWeight: '400',
    color: '#1D1B20',
  },

  listContent: {
    paddingHorizontal: 16,
    paddingTop: 16,
    paddingBottom: 30,
  },

  logCard: {
    backgroundColor: '#EBE3F0',
    padding: 16,
    borderRadius: 16,
    marginBottom: 14,
  },

  logText: {
    fontSize: 16,
    color: '#1D1B20',
    marginBottom: 4,
  },

  timeText: {
    fontSize: 12,
    color: '#49454F',
  },
});