# F4 Shop (Shopee Clone) ASP.NET MVC Core  7.0 
- Luồng sử lý dữ liệu cơ bản: User->Routeing->Controller<->Model->View->User

## Thành phần chính
### Routing
- Dựa vào các request Để gọi các Controller

### Controller
- Thực hiện các logic code từ yêu cầu cảu request và trả về response

### View
- Giao diện HTML, CSS được trả về theo logic của controller

### Model
- Được sử dụng để tương tác với các trường dữ liệu của bảng (định nghĩa các field, primary keys, foreign key,...)
- Khi tương tác với các dữ liệu có thể viết vào 1 file Repository riêng

## Khác
### Repository 
- Nơi tương tác với các dữ liệu của thực thể
- Được sử dụng để lấy dữ liệu và tương tác với dữ liệu của table (create, read, update, delete)

### Viết thủ tục lưu trong CSDL
  - Thủ tục được viết trên Server và khi truy vấn ta chỉ cần gọi thủ tục đó
### Quy tắc đặt tên trong CSDL
 - Tên Database: db_ (Ví dụ: db_F4_Shop)
 - Tên bảng: tbl_ (Ví dụ tbl_Categories)
 - Tên thủ tục: sp_ (Ví dụL sp_GetCategories)
 - ...
## Công việc của từng Dev
Chi tiết tại: https://docs.google.com/document/d/1OA526wTnw-2Jn4faBllxIqHbD78u58EU/edit?usp=drive_link&ouid=102969611045986692309&rtpof=true&sd=true
## Kết quả thực hiện
### Trang chủ
![image](https://github.com/DangVanCong2301/F4-Shop/assets/111124018/0a5c5d59-3f51-43f7-bb47-2b222c0a2932)
### Trang sản phẩm
![image](https://github.com/DangVanCong2301/F4-Shop/assets/111124018/9126d19c-cc46-49c9-87cf-ac0b98ab04c6)
### Chi tiết sản phẩm
![image](https://github.com/DangVanCong2301/F4-Shop/assets/111124018/1dc1ab47-6c82-4cf9-96b6-4e30309d8e08)
### Bình luận, đánh giá sản phẩm
![image](https://github.com/DangVanCong2301/F4-Shop/assets/111124018/92aafd63-a34e-4092-a23b-f2e344673e9f)
### Giỏ hàng
![image](https://github.com/DangVanCong2301/F4-Shop/assets/111124018/77acbdb4-d53b-466f-8021-1203338ed907)
### Quản lý thông tin tài khoản
![image](https://github.com/DangVanCong2301/F4-Shop/assets/111124018/bef0bda4-ea4b-46eb-af38-e77e64aee75f)
### Đặt hàng
![image](https://github.com/DangVanCong2301/F4-Shop/assets/111124018/30261bcb-5b7f-48aa-aa36-315cda07caae)










