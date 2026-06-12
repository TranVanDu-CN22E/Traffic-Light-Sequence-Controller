#ifndef ButtonController_H
#define ButtonController_H

#include "TrafficMode.h"
#include <Arduino.h>

/*
 * Lớp quản lý các nút nhấn vật lý của hệ thống.
 *
 * Chức năng:
 * - Đọc nút chuyển chế độ (MODE)
 * - Đọc nút bật/tắt hệ thống (POWER)
 * - Chống dội phím (Debounce)
 * - Cập nhật trạng thái mode và nguồn
 */
class ButtonController
{
private:
    // Chân nút chuyển chế độ
    byte modePin;

    // Chân nút bật/tắt nguồn
    byte powerPin;

    // Trạng thái lần đọc trước của nút MODE
    bool lastModeState = HIGH;

    // Trạng thái lần đọc trước của nút POWER
    bool lastPowerState = HIGH;

    // Thời điểm debounce gần nhất
    unsigned long lastDebounceTime = 0;

    // Thời gian chống dội phím (ms)
    const unsigned long debounceMs = 180;

public:

    /*
     * Constructor
     *
     * Khởi tạo đối tượng ButtonController
     * và lưu lại các chân nút nhấn.
     *
     * modeButton  : chân nút MODE
     * powerButton : chân nút POWER
     */
    ButtonController(byte modeButton, byte powerButton)
        : modePin(modeButton), powerPin(powerButton)
    {
    }

    /*
     * Khởi tạo phần cứng cho nút nhấn.
     *
     * INPUT_PULLUP:
     * - Không nhấn = HIGH
     * - Nhấn       = LOW
     *
     * Không cần điện trở kéo lên bên ngoài.
     */
    void begin()
    {
        pinMode(modePin, INPUT_PULLUP);
        pinMode(powerPin, INPUT_PULLUP);
    }

    /*
     * Cập nhật trạng thái nút nhấn.
     *
     * Chức năng:
     * - Đọc trạng thái hai nút
     * - Chống dội phím
     * - Chuyển mode khi nhấn MODE
     * - Bật/tắt hệ thống khi nhấn POWER
     *
     * mode    : chế độ hiện tại của hệ thống
     * powerOn : trạng thái nguồn hiện tại
     */
    void update(TrafficMode &mode, bool &powerOn)
    {
        // Đọc trạng thái nút MODE
        bool modeState = digitalRead(modePin);

        // Đọc trạng thái nút POWER
        bool powerState = digitalRead(powerPin);

        // Lấy thời gian hiện tại
        unsigned long now = millis();

        // Kiểm tra khoảng thời gian debounce
        if (now - lastDebounceTime >= debounceMs)
        {
            /*
             * Phát hiện cạnh xuống:
             * HIGH -> LOW
             *
             * Nghĩa là người dùng vừa nhấn nút MODE.
             */
            if (lastModeState == HIGH && modeState == LOW)
            {
                mode = nextMode(mode);
                lastDebounceTime = now;
            }

            /*
             * Phát hiện cạnh xuống:
             * HIGH -> LOW
             *
             * Nghĩa là người dùng vừa nhấn nút POWER.
             */
            if (lastPowerState == HIGH && powerState == LOW)
            {
                // Đảo trạng thái nguồn
                powerOn = !powerOn;

                lastDebounceTime = now;
            }
        }

        // Lưu trạng thái hiện tại cho lần kiểm tra tiếp theo
        lastModeState = modeState;
        lastPowerState = powerState;
    }

private:

    /*
     * Chuyển sang chế độ kế tiếp.
     *
     * Chu trình:
     * NORMAL -> FLASH -> CUSTOM -> NORMAL
     *
     * currentMode : chế độ hiện tại
     *
     * return : chế độ kế tiếp
     */
    TrafficMode nextMode(TrafficMode currentMode)
    {
        // Chế độ bình thường -> Nhấp nháy
        if (currentMode == MODE_NORMAL)
        {
            return MODE_FLASH;
        }

        // Chế độ nhấp nháy -> Tùy chỉnh
        if (currentMode == MODE_FLASH)
        {
            return MODE_CUSTOM;
        }

        // CUSTOM -> quay lại NORMAL
        return MODE_NORMAL;
    }
};

#endif