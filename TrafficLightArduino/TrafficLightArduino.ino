#include "TrafficMode.h"
#include "TrafficTiming.h"
#include "LightPins.h"
#include "SevenSegmentDisplay.cpp"
#include "TrafficLight.cpp"
#include "ButtonController.cpp"
/*
 * Lớp điều khiển trung tâm của hệ thống đèn giao thông.
 *
 * Nhiệm vụ:
 * - Quản lý 4 cụm đèn giao thông
 * - Quản lý LED 7 đoạn đếm ngược
 * - Xử lý nút nhấn MODE và POWER
 * - Nhận lệnh điều khiển từ Serial/Bluetooth
 * - Điều phối các chế độ hoạt động:
 *      + MODE_NORMAL
 *      + MODE_FLASH
 *      + MODE_CUSTOM
 *
 * Đây là lớp trung tâm (Controller) của toàn bộ hệ thống.
 */
class TrafficController
{
private:
    enum PinMap
    {
        ST_CP = 8,
        SH_CP = 9,
        DS_A = 11,
        BTN_MODE = 12,
        BTN_POWER = 13,
        HEARTBEAT_PIN = 10
    };

    TrafficLight lightA;
    TrafficLight lightB;
    TrafficLight lightC;
    TrafficLight lightD;
    SevenSegmentDisplay display;
    ButtonController buttons;

    TrafficTiming timing = {35, 5, 55};
    TrafficMode mode = MODE_NORMAL;
    bool powerOn = true;
    String customRoute = "";
    bool heartbeatState = LOW;
    unsigned long lastHeartbeatTime = 0;
/*
 * Constructor.
 *
 * Khởi tạo:
 * - 4 hướng đèn giao thông
 * - LED 7 đoạn
 * - Bộ điều khiển nút nhấn
 *
 * Đồng thời ánh xạ các chân Arduino
 * tới các thiết bị tương ứng.
 */
public:
    TrafficController()
        : lightA(2, 3, 4),
          lightB(5, 6, 7),
          lightC(16, 15, 14),
          lightD(19, 18, 17),
          display(ST_CP, SH_CP, DS_A),
          buttons(BTN_MODE, BTN_POWER)
    {
    }
/*
 * Khởi tạo toàn bộ hệ thống.
 *
 * Thực hiện:
 * - Khởi tạo các cụm đèn giao thông
 * - Khởi tạo LED 7 đoạn
 * - Khởi tạo nút nhấn
 * - Khởi tạo LED Heartbeat
 * - Khởi tạo cổng Serial
 *
 * Hàm chỉ được gọi một lần trong setup().
 */
    void begin()
    {
        lightA.begin();
        lightB.begin();
        lightC.begin();
        lightD.begin();
        display.begin();
        buttons.begin();

        pinMode(HEARTBEAT_PIN, OUTPUT);
        digitalWrite(HEARTBEAT_PIN, LOW);

        Serial.begin(9600);
        Serial.setTimeout(50);
    }
/*
 * Hàm điều khiển chính của hệ thống.
 *
 * Được gọi liên tục trong loop().
 *
 * Luồng xử lý:
 *
 * 1. Đọc nút nhấn và Serial
 * 2. Cập nhật LED Heartbeat
 * 3. Kiểm tra trạng thái nguồn
 * 4. Chạy chế độ tương ứng:
 *      NORMAL
 *      FLASH
 *      CUSTOM
 */
    void update()
    {
        pollInputs();
        updateHeartbeat();

        if (!powerOn)
        {
            turnAllOff();
            return;
        }

        if (mode == MODE_FLASH)
        {
            runFlashMode();
            return;
        }

        if (mode == MODE_CUSTOM)
        {
            runCustomMode();
            return;
        }

        runNormalMode();
    }

private:
    /*
    * Thu thập dữ liệu đầu vào.
    *
    * Bao gồm:
    * - Nút MODE
    * - Nút POWER
    * - Dữ liệu Serial/Bluetooth
    */
    void pollInputs()
    {
        buttons.update(mode, powerOn);
        readSerialCommand();
    }
/*
 * Điều khiển LED Heartbeat.
 *
 * LED sẽ đổi trạng thái mỗi 200ms.
 *
 * Mục đích:
 * - Báo hiệu chương trình vẫn đang hoạt động
 * - Giúp phát hiện hệ thống bị treo
 */
    void updateHeartbeat()
    {
        unsigned long now = millis();

        if (now - lastHeartbeatTime >= 200)
        {
            heartbeatState = !heartbeatState;
            digitalWrite(HEARTBEAT_PIN, heartbeatState);
            lastHeartbeatTime = now;
        }
    }
    // tắt tất cả
    void turnAllOff()
    {
        lightA.off();
        lightB.off();
        lightC.off();
        lightD.off();
        display.showCountdown(0, 0);
    }
    // bật chế độ flash
    void runFlashMode()
    {
        setAllYellow(true);
        display.showCountdown(0, 0);

        if (!waitWhileMode(MODE_FLASH, 500))
        {
            return;
        }

        setAllYellow(false);
        waitWhileMode(MODE_FLASH, 1000);
    }
    // chạy chế độ CUSTOM
    void runCustomMode()
    {
        display.showCountdown(0, 0);

        lightA.setAllowed(routeAllows('A'));
        lightB.setAllowed(routeAllows('B'));
        lightC.setAllowed(routeAllows('C'));
        lightD.setAllowed(routeAllows('D'));
    }
/*
 * Chế độ hoạt động bình thường.
 *
 * Chu trình:
 *
 * Pha 1:
 *      A,C xanh
 *      B,D đỏ
 *
 * Pha 2:
 *      A,C vàng
 *      B,D đỏ
 *
 * Pha 3:
 *      B,D xanh
 *      A,C đỏ
 *
 * Pha 4:
 *      B,D vàng
 *      A,C đỏ
 *
 * Sau đó lặp lại từ đầu.
 */
    void runNormalMode()
    {
        setPhaseGreenA();
        countDown(timing.aGreen, timing.aGreen + timing.yellow, MODE_NORMAL);

        if (!canContinueNormal())
        {
            return;
        }

        setPhaseYellowA();
        countDown(timing.yellow, timing.yellow, MODE_NORMAL);

        if (!canContinueNormal())
        {
            return;
        }

        setPhaseGreenB();
        countDown(timing.bGreen + timing.yellow, timing.bGreen, MODE_NORMAL);

        if (!canContinueNormal())
        {
            return;
        }

        setPhaseYellowB();
        countDown(timing.yellow, timing.yellow, MODE_NORMAL);
    }
    // các phase sáng ở chế độ bình thường
    void setPhaseGreenA()
    {
        lightA.green();
        lightB.red();
        lightC.green();
        lightD.red();
    }

    void setPhaseYellowA()
    {
        lightA.yellow();
        lightB.red();
        lightC.yellow();
        lightD.red();
    }

    void setPhaseGreenB()
    {
        lightA.red();
        lightB.green();
        lightC.red();
        lightD.green();
    }

    void setPhaseYellowB()
    {
        lightA.red();
        lightB.yellow();
        lightC.red();
        lightD.yellow();
    }
    // function bật đèn vàng ở chế độ FLASH
    void setAllYellow(bool enabled)
    {
        lightA.setYellow(enabled);
        lightB.setYellow(enabled);
        lightC.setYellow(enabled);
        lightD.setYellow(enabled);
    }
/*
 * Hiển thị và thực hiện đếm ngược.
 *
 * horizontalStart:
 *      thời gian hướng ngang.
 *
 * verticalStart:
 *      thời gian hướng dọc.
 *
 * requiredMode:
 *      chế độ yêu cầu phải duy trì.
 *
 * Trong mỗi giây:
 * - cập nhật LED 7 đoạn
 * - giảm bộ đếm
 * - kiểm tra thay đổi chế độ
 */
    void countDown(int horizontalStart, int verticalStart, TrafficMode requiredMode)
    {
        int horizontal = horizontalStart;
        int vertical = verticalStart;

        while (horizontal > 0 || vertical > 0)
        {
            if (!powerOn || mode != requiredMode)
            {
                return;
            }

            display.showCountdown(horizontal, vertical);

            if (!waitWhileMode(requiredMode, 1000))
            {
                return;
            }

            if (horizontal > 0)
            {
                horizontal--;
            }

            if (vertical > 0)
            {
                vertical--;
            }
        }
    }
/*
 * Delay có kiểm soát.
 *
 * Khác với delay() thông thường,
 * hàm vẫn:
 * - đọc nút nhấn
 * - đọc Serial
 * - cập nhật Heartbeat
 *
 * Nếu:
 *      mode thay đổi
 * hoặc:
 *      powerOff
 *
 * hàm sẽ thoát ngay lập tức.
 *
 * Trả về:
 *      true  : chờ thành công
 *      false : bị ngắt giữa chừng
 */
    bool waitWhileMode(TrafficMode requiredMode, unsigned long durationMs)
    {
        unsigned long start = millis();

        while (millis() - start < durationMs)
        {
            pollInputs();
            updateHeartbeat();

            if (!powerOn || mode != requiredMode)
            {
                return false;
            }

            delay(10);
        }

        return true;
    }
/*
 * Kiểm tra có được phép tiếp tục
 * chế độ NORMAL hay không.
 *
 * Điều kiện:
 * - Hệ thống đang bật
 * - Chế độ hiện tại là NORMAL
 */
    bool canContinueNormal()
    {
        return powerOn && mode == MODE_NORMAL;
    }
/*
 * Kiểm tra một hướng giao thông
 * có nằm trong tuyến được phép đi hay không.
 *
 * Ví dụ:
 *      customRoute = "A,C"
 *
 * routeAllows('A')
 *      -> true
 *
 * routeAllows('B')
 *      -> false
 */
    bool routeAllows(char direction)
    {
        return customRoute.indexOf(direction) >= 0;
    }
/*
 * Đọc và xử lý lệnh từ Serial/Bluetooth.
 *
 * Các lệnh hỗ trợ:
 *
 * FLASH
 * NORMAL
 * POWER_ON
 * POWER_OFF
 * SET,aGreen,yellow,bGreen
 * ROUTE,A,C
 *
 * Sau khi nhận lệnh sẽ cập nhật
 * trạng thái hệ thống tương ứng.
 */
    void readSerialCommand()
    {
        if (!Serial.available())
        {
            return;
        }

        String command = Serial.readString();
        command.trim();

        if (command == "FLASH")
        {
            mode = MODE_FLASH;
            Serial.println("Da bat che do FLASH.");
        }
        else if (command == "NORMAL")
        {
            mode = MODE_NORMAL;
            Serial.println("Da bat che do binh thuong.");
        }
        else if (command == "POWER_ON")
        {
            powerOn = true;
            Serial.println("Da nhan tin hieu khoi dong.");
        }
        else if (command == "POWER_OFF")
        {
            powerOn = false;
            Serial.println("Da tat he thong.");
        }
        else if (command.startsWith("SET"))
        {
            updateTiming(command);
        }
        else if (command.startsWith("ROUTE"))
        {
            updateRoute(command);
        }
    }
/*
 * Cập nhật thời gian đèn giao thông.
 *
 * Cú pháp:
 *      SET,aGreen,yellow,bGreen
 *
 * Ví dụ:
 *      SET,35,5,55
 *
 * Kết quả:
 *      timing.aGreen = 35
 *      timing.yellow = 5
 *      timing.bGreen = 55
 */
    void updateTiming(const String &command)
    {
        int aGreen = 0;
        int yellow = 0;
        int bGreen = 0;

        if (sscanf(command.c_str(), "SET,%d,%d,%d", &aGreen, &yellow, &bGreen) == 3 &&
            aGreen > 0 && yellow > 0 && bGreen > 0)
        {
            timing.aGreen = aGreen;
            timing.yellow = yellow;
            timing.bGreen = bGreen;
            Serial.println("Da cap nhat thoi gian.");
            return;
        }

        Serial.println("Lenh SET khong hop le. Dung: SET,aGreen,yellow,bGreen");
    }
/*
 * Cập nhật tuyến đường được phép lưu thông.
 *
 * Ví dụ:
 *      ROUTE,A,D
 *
 * Kết quả:
 *      customRoute = "A,D"
 *
 * Đồng thời chuyển hệ thống
 * sang MODE_CUSTOM.
 */
    void updateRoute(const String &command)
    {
        int separatorIndex = command.indexOf(',');

        if (separatorIndex < 0)
        {
            separatorIndex = command.indexOf(' ');
        }

        customRoute = separatorIndex >= 0 ? command.substring(separatorIndex + 1) : "";
        customRoute.toUpperCase();
        customRoute.trim();
        mode = MODE_CUSTOM;

        Serial.print("Route = ");
        Serial.println(customRoute);
    }
};

TrafficController trafficController;

void setup()
{
    trafficController.begin();
}

void loop()
{
    trafficController.update();
}
