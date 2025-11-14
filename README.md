🚗 Electric Vehicle Dealer Management System
Flow thực hiện: Đặt lịch lái thử xe (Test Drive Booking)
Nhóm thực hiện:Nhóm 6
📌 1. Giới thiệu

Dự án mô phỏng quy trình đặt lịch lái thử xe điện thông qua đại lý, giúp khách hàng dễ dàng đặt lịch, và nhân viên đại lý quản lý – xử lý lịch hẹn hiệu quả hơn.

Flow này chỉ tập trung vào nghiệp vụ đặt và xử lý lịch lái thử xe.

 2. Các chức năng nhóm đã thực hiện
1. Khách hàng (Customer)

Tra cứu danh sách xe có thể lái thử

Xem thông tin xe (model, phiên bản, màu sắc, tính năng)

Đặt lịch lái thử

Chọn xe

Chọn ngày giờ

Ghi chú yêu cầu

Nhận thông báo xác nhận hoặc thay đổi từ đại lý

Xem lịch đã đặt

2. Nhân viên đại lý (Dealer Staff)

Nhận danh sách lịch hẹn lái thử

Xem chi tiết từng lịch: khách hàng, xe, thời gian, ghi chú

Xác nhận lịch lái thử

Điều chỉnh lịch hẹn nếu trùng giờ hoặc xe không sẵn có

Hủy lịch lái thử nếu cần

Gửi thông báo cập nhật trạng thái cho khách hàng


 3. Công nghệ sử dụng

ASP.NET Core / Razor Pages

Entity Framework Core

SQL Server

Bootstrap 5

SignalR (nếu nhóm có thông báo realtime)

 5. Cách chạy dự án

Restore database từ file script SQL trong thư mục /Database.

Chỉnh lại chuỗi kết nối trong appsettings.json.

Chạy bằng Visual Studio hoặc dotnet run.
