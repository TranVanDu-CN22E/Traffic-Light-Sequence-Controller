#include <SoftwareSerial.h>

int tgAXanh  = 35;
int tgVang  = 5;
int tgBXanh  = 55;
//================================================
// CHÂN LED GIAO THÔNG
//================================================

const int H_RED    = 2;
const int H_YELLOW = 3;
const int H_GREEN  = 4;

const int V_RED    = 5;
const int V_YELLOW = 6;
const int V_GREEN  = 7;

//================================================
// CHÂN 74HC595
//================================================

const int ST_CP = 8;
const int SH_CP = 9;

const int DS_A  = 11;

//================================================
// NÚT NHẤN
//================================================

const int BTN_MODE  = 12;
const int BTN_POWER = 13;

//================================================

bool powerOn = true;

enum TrafficMode
{
    MODE_NORMAL,
    MODE_FLASH
};

TrafficMode mode = MODE_NORMAL;

//================================================
// HIỂN THỊ 7 ĐOẠN
//================================================

void hienThiDemNguoc(int h, int v)
{
    byte dataA =
        ((h % 10) << 4) |
        (h / 10);

    byte dataB =
        ((v % 10) << 4) |
        (v / 10);

    digitalWrite(ST_CP, LOW);

    shiftOut(DS_A, SH_CP, MSBFIRST, dataB);
    shiftOut(DS_A, SH_CP, MSBFIRST, dataA);

    digitalWrite(ST_CP, HIGH);
}

//================================================
// ĐỌC NÚT NHẤN
//================================================

void docNutNhan()
{
    static bool lastMode = HIGH;
    static bool lastPower = HIGH;

    bool modeState  = digitalRead(BTN_MODE);
    bool powerState = digitalRead(BTN_POWER);

    if(lastMode == HIGH && modeState == LOW)
    {
        if(mode == MODE_NORMAL)
            mode = MODE_FLASH;
        else
            mode = MODE_NORMAL;

        delay(200);
    }

    if(lastPower == HIGH && powerState == LOW)
    {
        powerOn = !powerOn;
        delay(200);
    }

    lastMode = modeState;
    lastPower = powerState;
}

//================================================
// TẮT TOÀN BỘ
//================================================

void tatTatCa()
{
    digitalWrite(H_RED, LOW);
    digitalWrite(H_YELLOW, LOW);
    digitalWrite(H_GREEN, LOW);

    digitalWrite(V_RED, LOW);
    digitalWrite(V_YELLOW, LOW);
    digitalWrite(V_GREEN, LOW);

    hienThiDemNguoc(0, 0);
}

//================================================
// CHẾ ĐỘ NHÁY VÀNG
//================================================

void cheDoNhayVang()
{
    digitalWrite(H_RED, LOW);
    digitalWrite(H_GREEN, LOW);

    digitalWrite(V_RED, LOW);
    digitalWrite(V_GREEN, LOW);

    digitalWrite(H_YELLOW, HIGH);
    digitalWrite(V_YELLOW, HIGH);

    hienThiDemNguoc(0, 0);

    for(int i = 0; i < 50; i++)
    {
        docNutNhan();
        docBluetooth();

        if(!powerOn || mode != MODE_FLASH)
            return;

        delay(10);
    }

    digitalWrite(H_YELLOW, LOW);
    digitalWrite(V_YELLOW, LOW);

    for(int i = 0; i < 100; i++)
    {
        docNutNhan();
        docBluetooth();

        if(!powerOn || mode != MODE_FLASH)
            return;

        delay(10);
    }
}

//================================================
// PHA 1
// A XANH - B ĐỎ
//================================================

void pha1()
{
    digitalWrite(H_GREEN, HIGH);
    digitalWrite(H_YELLOW, LOW);
    digitalWrite(H_RED, LOW);

    digitalWrite(V_GREEN, LOW);
    digitalWrite(V_YELLOW, LOW);
    digitalWrite(V_RED, HIGH);

    for(int h = tgAXanh, v = tgAXanh + tgVang; h > 0; h--, v--)
    {
        hienThiDemNguoc(h, v);

        for(int i=0;i<100;i++)
        {
            docNutNhan();
            docBluetooth();

            if(!powerOn || mode != MODE_NORMAL)
                return;

            delay(10);
        }
    }
}

//================================================
// PHA 2
// A VÀNG - B ĐỎ
//================================================

void pha2()
{
    digitalWrite(H_GREEN, LOW);
    digitalWrite(H_YELLOW, HIGH);
    digitalWrite(H_RED, LOW);

    digitalWrite(V_GREEN, LOW);
    digitalWrite(V_YELLOW, LOW);
    digitalWrite(V_RED, HIGH);

    for(int h = tgVang, v = tgVang; h > 0; h--, v--)
    {
        hienThiDemNguoc(h, v);

        for(int i=0;i<100;i++)
        {
            docNutNhan();
            docBluetooth();

            if(!powerOn || mode != MODE_NORMAL)
                return;

            delay(10);
        }
    }
}

//================================================
// PHA 3
// A ĐỎ - B XANH
//================================================

void pha3()
{
    digitalWrite(H_GREEN, LOW);
    digitalWrite(H_YELLOW, LOW);
    digitalWrite(H_RED, HIGH);

    digitalWrite(V_RED, LOW);
    digitalWrite(V_YELLOW, LOW);
    digitalWrite(V_GREEN, HIGH);

    for(int h = tgBXanh + tgVang, v = tgBXanh; v > 0; h--, v--)
    {
        hienThiDemNguoc(h, v);

        for(int i=0;i<100;i++)
        {
            docNutNhan();
            docBluetooth();

            if(!powerOn || mode != MODE_NORMAL)
                return;

            delay(10);
        }
    }
}

//================================================
// PHA 4
// A ĐỎ - B VÀNG
//================================================

void pha4()
{
    digitalWrite(H_GREEN, LOW);
    digitalWrite(H_YELLOW, LOW);
    digitalWrite(H_RED, HIGH);

    digitalWrite(V_GREEN, LOW);
    digitalWrite(V_YELLOW, HIGH);
    digitalWrite(V_RED, LOW);

    for(int h = tgVang, v = tgVang; h > 0; h--, v--)
    {
        hienThiDemNguoc(h, v);

        for(int i=0;i<100;i++)
        {
            docNutNhan();
            docBluetooth();

            if(!powerOn || mode != MODE_NORMAL)
                return;

            delay(10);
        }
    }

    digitalWrite(V_YELLOW, LOW);
}

void docBluetooth()
{
    if(Serial.available())
    {
        String cmd = Serial.readString();

        cmd.trim();

        if(cmd == "FLASH")
        {
            mode = MODE_FLASH;
            Serial.println("Đã bạt chế độ FLASH.");
        }
        else if(cmd == "NORMAL")
        {
            mode = MODE_NORMAL;
            Serial.println("Đẫ bật chế độ bình thường.");
        }
        else if(cmd == "POWER_ON")
        {
            powerOn = true;
            Serial.println("Đã nhận tín hiệu khởi động.");
        }
        else if(cmd == "POWER_OFF")
        {
            powerOn = false;
            Serial.println("Đa tắt hệ thống.");
        }
        else if(cmd.startsWith("SET"))
        {
            int a,b,c;

            sscanf(
                cmd.c_str(),
                "SET,%d,%d,%d",
                &a,&b,&c
            );

            tgAXanh = a;
            tgVang = b;
            tgBXanh = c;

            Serial.println("Cập nhập thời gian, vui lòng khởi động lại.");
        }
    }
}
//================================================
// SETUP
//================================================

void setup()
{
    pinMode(H_RED, OUTPUT);
    pinMode(H_YELLOW, OUTPUT);
    pinMode(H_GREEN, OUTPUT);

    pinMode(V_RED, OUTPUT);
    pinMode(V_YELLOW, OUTPUT);
    pinMode(V_GREEN, OUTPUT);

    pinMode(ST_CP, OUTPUT);
    pinMode(SH_CP, OUTPUT);
    pinMode(DS_A, OUTPUT);

    pinMode(BTN_MODE, INPUT_PULLUP);
    pinMode(BTN_POWER, INPUT_PULLUP);

    Serial.begin(9600);
}

//================================================
// LOOP
//================================================

void loop()
{
    docNutNhan();
    docBluetooth();

    if(!powerOn)
    {
        tatTatCa();
        return;
    }

    if(mode == MODE_FLASH)
    {
        cheDoNhayVang();
        return;
    }

    pha1();

    if(!powerOn || mode != MODE_NORMAL) return;

    pha2();

    if(!powerOn || mode != MODE_NORMAL) return;

    pha3();

    if(!powerOn || mode != MODE_NORMAL) return;

    pha4();
}