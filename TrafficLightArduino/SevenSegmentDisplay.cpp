#ifndef SevenSegmentDisplay_H
#define SevenSegmentDisplay_H

#include <Arduino.h>

/*
 * Lớp quản lý LED 7 đoạn thông qua IC dịch dữ liệu (74HC595).
 *
 * Chức năng:
 * - Khởi tạo các chân giao tiếp với IC 74HC595
 * - Hiển thị giá trị đếm ngược lên LED 7 đoạn
 * - Hỗ trợ hiển thị đồng thời 2 giá trị:
 *      + horizontal (ngang)
 *      + vertical (dọc)
 *
 * Dữ liệu được gửi theo dạng BCD:
 *      Hàng chục | Hàng đơn vị
 *
 * Ví dụ:
 *      35 -> 0011 0101
 */
class SevenSegmentDisplay
{
private:

    /*
     * Chân chốt dữ liệu (ST_CP)
     *
     * Khi chuyển từ LOW -> HIGH
     * dữ liệu trong thanh ghi dịch sẽ được xuất ra LED.
     */
    byte latchPin;

    /*
     * Chân xung Clock (SH_CP)
     *
     * Mỗi xung clock sẽ dịch dữ liệu vào IC 74HC595.
     */
    byte clockPin;

    /*
     * Chân dữ liệu nối với DS của 74HC595.
     */
    byte dataPin;

public:

    /*
     * Constructor
     *
     * Khởi tạo đối tượng LED 7 đoạn
     * và lưu các chân điều khiển.
     *
     * latch : chân ST_CP
     * clock : chân SH_CP
     * data  : chân DS
     */
    SevenSegmentDisplay(byte latch, byte clock, byte data)
        : latchPin(latch),
          clockPin(clock),
          dataPin(data)
    {
    }

    /*
     * Khởi tạo phần cứng.
     *
     * Chức năng:
     * - Cấu hình các chân điều khiển là OUTPUT
     * - Hiển thị giá trị mặc định 00 00
     */
    void begin()
    {
        pinMode(latchPin, OUTPUT);
        pinMode(clockPin, OUTPUT);
        pinMode(dataPin, OUTPUT);

        showCountdown(0, 0);
    }

    /*
     * Hiển thị thời gian đếm ngược lên LED 7 đoạn.
     *
     * horizontal:
     *      Giá trị hiển thị cho hướng ngang.
     *
     * vertical:
     *      Giá trị hiển thị cho hướng dọc.
     *
     * Phạm vi hỗ trợ:
     *      0 -> 99
     */
    void showCountdown(int horizontal, int vertical)
    {
        /*
         * Giới hạn giá trị trong khoảng 0-99.
         *
         * Nếu nhỏ hơn 0:
         *      -> 0
         *
         * Nếu lớn hơn 99:
         *      -> 99
         */
        horizontal = constrain(horizontal, 0, 99);
        vertical = constrain(vertical, 0, 99);

        /*
         * Chuyển số thành dữ liệu BCD.
         *
         * Ví dụ:
         *      horizontal = 35
         *
         * Hàng chục:
         *      3
         *
         * Hàng đơn vị:
         *      5
         *
         * Kết quả:
         *      0011 0101
         */
        byte dataA =
            ((horizontal % 10) << 4) |
            (horizontal / 10);

        /*
         * Chuyển giá trị vertical
         * sang dạng BCD tương tự.
         */
        byte dataB =
            ((vertical % 10) << 4) |
            (vertical / 10);

        /*
         * Bắt đầu truyền dữ liệu.
         */
        digitalWrite(latchPin, LOW);

        /*
         * Gửi dữ liệu của hướng dọc.
         */
        shiftOut(
            dataPin,
            clockPin,
            MSBFIRST,
            dataB
        );

        /*
         * Gửi dữ liệu của hướng ngang.
         */
        shiftOut(
            dataPin,
            clockPin,
            MSBFIRST,
            dataA
        );

        /*
         * Chốt dữ liệu để hiển thị ra LED.
         */
        digitalWrite(latchPin, HIGH);
    }
};

#endif