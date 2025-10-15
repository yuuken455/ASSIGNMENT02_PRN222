CREATE DATABASE EVDManagement;
GO

USE EVDManagement;
GO

CREATE TABLE Staffs (
    StaffID INT IDENTITY(1,1) PRIMARY KEY,
    Email VARCHAR(50) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    FullName NVARCHAR(150),
    Phone VARCHAR(12),
    DealerID INT NULL
);

-- Mẫu xe
CREATE TABLE Models (
    ModelID INT IDENTITY(1,1) PRIMARY KEY,
    ModelName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500),
    Segment NVARCHAR(100)
);

-- Phiên bản xe
CREATE TABLE Versions (
    VersionID INT IDENTITY(1,1) PRIMARY KEY,
    ModelID INT NOT NULL,
    VersionName NVARCHAR(150) NOT NULL,
    BatteryCapacity DECIMAL(10,2),
    RangeKm INT,
    Seat INT,
    BasePrice DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (ModelID) REFERENCES Models(ModelID)
);

-- Màu xe
CREATE TABLE Colors (
    ColorID INT IDENTITY(1,1) PRIMARY KEY,
    VersionID INT NOT NULL,
    ColorName NVARCHAR(100),
    HexCode VARCHAR(7),
    ExtraCost DECIMAL(18,2) DEFAULT 0,
    FOREIGN KEY (VersionID) REFERENCES Versions(VersionID)
);

-- Xe tồn kho ở mỗi đại lý
CREATE TABLE Inventory (
    InventoryID INT IDENTITY(1,1) PRIMARY KEY,
    VersionID INT NOT NULL,
    ColorID INT NOT NULL,
    Quantity INT DEFAULT 0,
    FOREIGN KEY (VersionID) REFERENCES Versions(VersionID),
    FOREIGN KEY (ColorID) REFERENCES Colors(ColorID)
);

CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Phone VARCHAR(20),
    Email VARCHAR(100),
    Address NVARCHAR(300),
    IDNumber VARCHAR(50), -- CCCD
    DOB DATE, -- Ngày sinh
    Note NVARCHAR(500)
);

CREATE TABLE TestDriveAppointments (
    AppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL,
    CarVersionID INT NOT NULL,
    ColorID INT NOT NULL,
    DateTime DATETIME NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    Feedback NVARCHAR(500),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (CarVersionID) REFERENCES Versions(VersionID),
    FOREIGN KEY (ColorID) REFERENCES Colors(ColorID)
);

-- Đơn hàng
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL,
    StaffID INT NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (StaffID) REFERENCES Staffs(StaffID)
);

-- Chi tiết đơn hàng
CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    VersionID INT NOT NULL,
    ColorID INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    Discount DECIMAL(18,2) DEFAULT 0,
    FinalPrice DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (VersionID) REFERENCES Versions(VersionID),
    FOREIGN KEY (ColorID) REFERENCES Colors(ColorID)
);

--Dữ liệu 
--Models 
INSERT INTO Models (ModelName, Description, Segment)
VALUES 
(N'VinFast VF e34', N'Mẫu SUV điện cỡ C, phù hợp đô thị', N'SUV C'),
(N'VinFast VF 8', N'SUV điện cỡ D, công nghệ hiện đại', N'SUV D'),
(N'VinFast VF 9', N'SUV điện cỡ E, 7 chỗ rộng rãi', N'SUV E');

--Versions
INSERT INTO Versions (ModelID, VersionName, BatteryCapacity, RangeKm, Seat, BasePrice)
VALUES
(1, N'Bản tiêu chuẩn', 42.00, 318, 5, 690000000),
(2, N'Eco', 82.00, 420, 5, 1090000000),
(2, N'Plus', 87.70, 460, 5, 1250000000),
(3, N'Eco', 92.00, 485, 7, 1490000000),
(3, N'Plus', 123.00, 520, 7, 1680000000);

--Colors
INSERT INTO Colors (VersionID, ColorName, HexCode, ExtraCost)
VALUES
(1, N'Trắng', '#FFFFFF', 0),
(1, N'Đen', '#000000', 5000000),
(1, N'Xanh dương', '#0000FF', 3000000),
(2, N'Xám', '#808080', 0),
(2, N'Đỏ', '#FF0000', 7000000),
(3, N'Trắng ngọc trai', '#F8F8FF', 10000000),
(3, N'Xanh rêu', '#2E8B57', 5000000),
(4, N'Đen', '#000000', 0),
(4, N'Xanh navy', '#000080', 7000000),
(5, N'Trắng ngọc trai', '#F8F8FF', 12000000),
(5, N'Xám bạc', '#C0C0C0', 5000000);

--Inventory
INSERT INTO Inventory (VersionID, ColorID, Quantity)
VALUES
(1, 1, 10),
(1, 2, 5),
(1, 3, 7),
(2, 4, 8),
(2, 5, 6),
(3, 6, 4),
(3, 7, 3),
(4, 8, 5),
(4, 9, 2),
(5, 10, 3),
(5, 11, 4);

--Staff 
INSERT INTO Staffs (Email, Password, FullName, Phone, DealerID)
VALUES
('admin@vinfast.com', '123456', N'Nguyễn Văn Tùng', '0912345678', 1),
('sales1@vinfast.com', '123456', N'Trần Thị Hoa', '0987654321', 1),
('sales2@vinfast.com', '123456', N'Lê Văn Cường', '0923423456', 2),
('sales3@vinfast.com', '123456', N'Phạm Nhật Minh', '0943223456', 2),
('sales4@vinfast.com', '123456', N'Lê Toàn', '0977678456', 2),
('sales5@vinfast.com', '123456', N'Trần Nhật Nam', '0678123456', 2);

--Customer 
INSERT INTO Customers (FullName, Phone, Email, Address, IDNumber, DOB)
VALUES
(N'Phạm Minh Đức', '0911111111', 'ducpham@gmail.com', N'123 Lê Lợi, Hà Nội', '012345678901', '1990-05-12'),
(N'Ngô Thị Hoa', '0922222222', 'hoa.ngo@yahoo.com', N'45 Trần Hưng Đạo, Đà Nẵng', '123456789012', '1985-08-25'),
(N'Hoàng Văn Nam', '0933333333', 'namhv@outlook.com', N'78 Nguyễn Huệ, TP.HCM', '234567890123', '1992-02-10'),
(N'Lê Thị Mai', '0944444444', 'lemai@gmail.com', N'12 Pasteur, Hải Phòng', '345678901234', '1995-11-20');

--Orders
INSERT INTO Orders (CustomerID, StaffID, OrderDate, Status)
VALUES
(1, 2, '2025-09-01', 'Confirmed'),
(2, 2, '2025-09-05', 'Confirmed'),
(3, 3, '2025-09-10', 'Cancelled'),
(4, 1, '2025-09-15', 'Confirmed');

--OrderDetails
--Order 1: Khách 1 mua VF e34 (trắng) số lượng 1
INSERT INTO OrderDetails (OrderID, VersionID, ColorID, Quantity, UnitPrice, Discount, FinalPrice)
VALUES
(1, 1, 1, 1, 690000000, 10000000, 680000000),

-- Order 2: Khách 2 mua VF 8 Eco (đỏ) số lượng 2
(2, 2, 5, 2, 1090000000, 50000000, (1090000000+7000000-25000000)*2),

-- Order 3: Khách 3 mua VF 9 Eco (đen) số lượng 1
(3, 4, 8, 1, 1490000000, 0, 1490000000),

-- Order 4: Khách 4 mua VF 8 Plus (trắng ngọc trai) số lượng 1
(4, 3, 6, 1, 1250000000, 20000000, 1240000000);

