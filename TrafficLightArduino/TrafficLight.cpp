#ifndef TrafficLight_H
#define TrafficLight_H

#include "LightPins.h"
#include <Arduino.h>

/*
 * Lớp quản lý một cụm đèn giao thông.
 *
 * Mỗi đối tượng TrafficLight đại diện cho một hướng giao thông
 * gồm 3 đèn:
 * - Đỏ
 * - Vàng
 * - Xanh
 *
 * Chức năng:
 * - Khởi tạo chân điều khiển đèn
 * - Điều khiển từng trạng thái đèn
 * - Hỗ trợ chế độ nhấp nháy vàng
 * - Hỗ trợ chế độ ưu tiên tuyến đường
 */
class TrafficLight
{
private:

    /*
     * Lưu thông tin các chân điều khiển:
     * pins.red
     * pins.yellow
     * pins.green
     */
    LightPins pins;

public:

    /*
     * Constructor
     *
     * Khởi tạo một cụm đèn giao thông
     * và gán các chân điều khiển tương ứng.
     *
     * red    : chân đèn đỏ
     * yellow : chân đèn vàng
     * green  : chân đèn xanh
     */
    TrafficLight(byte red, byte yellow, byte green)
        : pins{red, yellow, green}
    {
    }

    /*
     * Khởi tạo phần cứng.
     *
     * Chức năng:
     * - Cấu hình các chân là OUTPUT
     * - Tắt toàn bộ đèn khi hệ thống khởi động
     */
    void begin()
    {
        pinMode(pins.red, OUTPUT);
        pinMode(pins.yellow, OUTPUT);
        pinMode(pins.green, OUTPUT);

        off();
    }

    /*
     * Bật đèn đỏ.
     *
     * Trạng thái:
     * RED    = ON
     * YELLOW = OFF
     * GREEN  = OFF
     *
     * Sử dụng khi hướng giao thông bị cấm đi.
     */
    void red()
    {
        digitalWrite(pins.red, HIGH);
        digitalWrite(pins.yellow, LOW);
        digitalWrite(pins.green, LOW);
    }

    /*
     * Bật đèn vàng.
     *
     * Trạng thái:
     * RED    = OFF
     * YELLOW = ON
     * GREEN  = OFF
     *
     * Sử dụng khi chuẩn bị chuyển pha đèn.
     */
    void yellow()
    {
        digitalWrite(pins.red, LOW);
        digitalWrite(pins.yellow, HIGH);
        digitalWrite(pins.green, LOW);
    }

    /*
     * Bật đèn xanh.
     *
     * Trạng thái:
     * RED    = OFF
     * YELLOW = OFF
     * GREEN  = ON
     *
     * Cho phép phương tiện lưu thông.
     */
    void green()
    {
        digitalWrite(pins.red, LOW);
        digitalWrite(pins.yellow, LOW);
        digitalWrite(pins.green, HIGH);
    }

    /*
     * Tắt toàn bộ đèn.
     *
     * Trạng thái:
     * RED    = OFF
     * YELLOW = OFF
     * GREEN  = OFF
     *
     * Dùng khi:
     * - Tắt hệ thống
     * - Khởi tạo ban đầu
     */
    void off()
    {
        digitalWrite(pins.red, LOW);
        digitalWrite(pins.yellow, LOW);
        digitalWrite(pins.green, LOW);
    }

    /*
     * Điều khiển riêng đèn vàng.
     *
     * enabled = true
     *      -> bật đèn vàng
     *
     * enabled = false
     *      -> tắt đèn vàng
     *
     * Hai đèn đỏ và xanh luôn tắt.
     *
     * Hàm này được sử dụng trong chế độ:
     * MODE_FLASH (đèn vàng nhấp nháy).
     */
    void setYellow(bool enabled)
    {
        digitalWrite(pins.red, LOW);
        digitalWrite(pins.green, LOW);

        digitalWrite(
            pins.yellow,
            enabled ? HIGH : LOW
        );
    }

    /*
     * Thiết lập trạng thái cho phép lưu thông.
     *
     * allowed = true
     *      -> đèn xanh
     *
     * allowed = false
     *      -> đèn đỏ
     *
     * Được sử dụng trong MODE_CUSTOM
     * để bật/tắt từng hướng giao thông.
     */
    void setAllowed(bool allowed)
    {
        if (allowed)
        {
            green();
        }
        else
        {
            red();
        }
    }
};

#endif