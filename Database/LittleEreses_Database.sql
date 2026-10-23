/* =====================================================================
   Appointment and Billing System for Little Erese's Aesthetics and Spa
   Database: LittleEresesDB   |   Target: Microsoft SQL Server (SSMS)
   18 tables
   ===================================================================== */

IF DB_ID('LittleEresesDB') IS NULL
    CREATE DATABASE LittleEresesDB;
GO
USE LittleEresesDB;
GO

/* ---------------------------------------------------------------------
   1. USER MANAGEMENT
   --------------------------------------------------------------------- */
CREATE TABLE Users (
    user_id         INT IDENTITY(1,1) PRIMARY KEY,
    full_name       VARCHAR(100) NOT NULL,
    username        VARCHAR(50)  NOT NULL UNIQUE,
    password_hash   VARCHAR(255) NOT NULL,
    role            VARCHAR(20)  NOT NULL CHECK (role IN ('Admin','Staff')),
    contact_number  VARCHAR(20)  NULL,
    status          VARCHAR(20)  NOT NULL DEFAULT 'Active'
                    CHECK (status IN ('Active','Inactive')),
    created_at      DATETIME     NOT NULL DEFAULT GETDATE()
);

/* ---------------------------------------------------------------------
   2. SERVICE AND PROMO MAINTENANCE
   --------------------------------------------------------------------- */
CREATE TABLE ServiceCategories (
    category_id     INT IDENTITY(1,1) PRIMARY KEY,
    category_name   VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Services (
    service_id              INT IDENTITY(1,1) PRIMARY KEY,
    category_id             INT NOT NULL,
    service_name            VARCHAR(100) NOT NULL,
    description             VARCHAR(MAX) NULL,
    price                   DECIMAL(10,2) NOT NULL CHECK (price >= 0),
    duration_minutes        INT NOT NULL DEFAULT 60 CHECK (duration_minutes > 0),
    default_total_sessions  INT NOT NULL DEFAULT 1 CHECK (default_total_sessions >= 1),
    status                  VARCHAR(20) NOT NULL DEFAULT 'Active'
                            CHECK (status IN ('Active','Archived')),
    CONSTRAINT FK_Services_Category FOREIGN KEY (category_id)
        REFERENCES ServiceCategories(category_id)
);

CREATE TABLE Promotions (
    promo_id        INT IDENTITY(1,1) PRIMARY KEY,
    promo_name      VARCHAR(100) NOT NULL,
    description     VARCHAR(MAX) NULL,           -- promo conditions (Scope 3.2)
    discount_type   VARCHAR(20)  NOT NULL CHECK (discount_type IN ('Percentage','Fixed')),
    discount_value  DECIMAL(10,2) NOT NULL CHECK (discount_value >= 0),
    start_date      DATE NOT NULL,
    end_date        DATE NOT NULL,
    status          VARCHAR(20) NOT NULL DEFAULT 'Active'
                    CHECK (status IN ('Active','Inactive','Expired')),
    CONSTRAINT CK_Promotions_Dates CHECK (end_date >= start_date)
);

/* A promotion may be linked to specific services. No link means "applies to the whole bill". */
CREATE TABLE PromotionServices (
    promo_service_id INT IDENTITY(1,1) PRIMARY KEY,
    promo_id         INT NOT NULL,
    service_id       INT NOT NULL,
    CONSTRAINT FK_PS_Promotion FOREIGN KEY (promo_id)   REFERENCES Promotions(promo_id),
    CONSTRAINT FK_PS_Service   FOREIGN KEY (service_id) REFERENCES Services(service_id),
    CONSTRAINT UQ_PS UNIQUE (promo_id, service_id)
);

/* ---------------------------------------------------------------------
   3. ROOMS (optional - not in the proposal's scope; see notes)
   --------------------------------------------------------------------- */
CREATE TABLE Rooms (
    room_id     INT IDENTITY(1,1) PRIMARY KEY,
    room_name   VARCHAR(100) NOT NULL UNIQUE,
    capacity    INT NOT NULL DEFAULT 1 CHECK (capacity > 0),
    status      VARCHAR(20) NOT NULL DEFAULT 'Available'
                CHECK (status IN ('Available','Unavailable'))
);

CREATE TABLE RoomServiceCategories (
    room_category_id INT IDENTITY(1,1) PRIMARY KEY,
    room_id          INT NOT NULL,
    category_id      INT NOT NULL,
    CONSTRAINT FK_RSC_Room     FOREIGN KEY (room_id)     REFERENCES Rooms(room_id),
    CONSTRAINT FK_RSC_Category FOREIGN KEY (category_id) REFERENCES ServiceCategories(category_id),
    CONSTRAINT UQ_RSC UNIQUE (room_id, category_id)
);

/* ---------------------------------------------------------------------
   4. CLIENT RECORDS
   --------------------------------------------------------------------- */
CREATE TABLE Clients (
    client_id           INT IDENTITY(1,1) PRIMARY KEY,
    first_name          VARCHAR(50)  NOT NULL,
    last_name           VARCHAR(50)  NOT NULL,
    birth_date          DATE NULL,
    gender              VARCHAR(10) NULL,
    address             VARCHAR(255) NULL,
    contact_number      VARCHAR(20)  NOT NULL,
    allergies           VARCHAR(MAX) NULL,
    medical_history     VARCHAR(MAX) NULL,       -- medical conditions
    treatment_history   VARCHAR(MAX) NULL,       -- treatments in past 6 months
    consent_status      BIT NOT NULL DEFAULT 0,  -- 1 = physical form signed
    created_at          DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at          DATETIME NOT NULL DEFAULT GETDATE()
);
CREATE INDEX IX_Clients_Name ON Clients(last_name, first_name);

CREATE TABLE ClientPackages (
    package_id          INT IDENTITY(1,1) PRIMARY KEY,
    client_id           INT NOT NULL,
    service_id          INT NOT NULL,
    total_sessions      INT NOT NULL CHECK (total_sessions >= 1),
    sessions_completed  INT NOT NULL DEFAULT 0,
    purchase_date       DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    expiry_date         DATE NULL,
    status              VARCHAR(20) NOT NULL DEFAULT 'Active'
                        CHECK (status IN ('Active','Completed','Expired')),
    CONSTRAINT FK_CP_Client  FOREIGN KEY (client_id)  REFERENCES Clients(client_id),
    CONSTRAINT FK_CP_Service FOREIGN KEY (service_id) REFERENCES Services(service_id),
    CONSTRAINT CK_CP_Sessions CHECK (sessions_completed BETWEEN 0 AND total_sessions)
);

/* ---------------------------------------------------------------------
   5. APPOINTMENTS
   --------------------------------------------------------------------- */
CREATE TABLE Appointments (
    appointment_id   INT IDENTITY(1,1) PRIMARY KEY,
    client_id        INT NOT NULL,
    user_id          INT NOT NULL,               -- staff who encoded the booking
    appointment_date DATE NOT NULL,
    start_time       TIME NOT NULL,
    end_time         TIME NOT NULL,
    booking_source   VARCHAR(20) NOT NULL DEFAULT 'Walk-in'
                     CHECK (booking_source IN ('Walk-in','Messenger','Text','Phone')),
    status           VARCHAR(20) NOT NULL DEFAULT 'Upcoming'
                     CHECK (status IN ('Upcoming','Ongoing','Completed','Cancelled','Rescheduled')),
    notes            VARCHAR(MAX) NULL,
    cancelled_at     DATETIME NULL,
    created_at       DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at       DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Appt_Client FOREIGN KEY (client_id) REFERENCES Clients(client_id),
    CONSTRAINT FK_Appt_User   FOREIGN KEY (user_id)   REFERENCES Users(user_id),
    CONSTRAINT CK_Appt_Time   CHECK (end_time > start_time)
);
CREATE INDEX IX_Appt_DateTime ON Appointments(appointment_date, start_time);

CREATE TABLE AppointmentServices (
    appointment_service_id INT IDENTITY(1,1) PRIMARY KEY,
    appointment_id         INT NOT NULL,
    service_id             INT NOT NULL,
    package_id             INT NULL,             -- set when the visit uses a package
    quantity               INT NOT NULL DEFAULT 1 CHECK (quantity >= 1),
    price                  DECIMAL(10,2) NOT NULL CHECK (price >= 0),  -- price at booking time
    session_number         INT NULL,             -- e.g. 6 of 8
    total_sessions         INT NULL,
    status                 VARCHAR(20) NOT NULL DEFAULT 'Pending'
                           CHECK (status IN ('Pending','Completed','Cancelled')),
    CONSTRAINT FK_AS_Appointment FOREIGN KEY (appointment_id) REFERENCES Appointments(appointment_id),
    CONSTRAINT FK_AS_Service     FOREIGN KEY (service_id)     REFERENCES Services(service_id),
    CONSTRAINT FK_AS_Package     FOREIGN KEY (package_id)     REFERENCES ClientPackages(package_id)
);

CREATE TABLE AppointmentStaff (
    appointment_staff_id   INT IDENTITY(1,1) PRIMARY KEY,
    appointment_id         INT NOT NULL,
    appointment_service_id INT NOT NULL,
    user_id                INT NOT NULL,         -- technician who performs the service
    assigned_date          DATETIME NOT NULL DEFAULT GETDATE(),
    status                 VARCHAR(20) NOT NULL DEFAULT 'Assigned'
                           CHECK (status IN ('Assigned','Completed','Reassigned')),
    CONSTRAINT FK_AST_Appointment FOREIGN KEY (appointment_id)         REFERENCES Appointments(appointment_id),
    CONSTRAINT FK_AST_ApptService FOREIGN KEY (appointment_service_id) REFERENCES AppointmentServices(appointment_service_id),
    CONSTRAINT FK_AST_User        FOREIGN KEY (user_id)                REFERENCES Users(user_id)
);

CREATE TABLE AppointmentRooms (
    appointment_room_id    INT IDENTITY(1,1) PRIMARY KEY,
    appointment_id         INT NOT NULL,
    appointment_service_id INT NOT NULL,
    room_id                INT NOT NULL,
    status                 VARCHAR(20) NOT NULL DEFAULT 'Reserved'
                           CHECK (status IN ('Reserved','In Use','Released')),
    CONSTRAINT FK_AR_Appointment FOREIGN KEY (appointment_id)         REFERENCES Appointments(appointment_id),
    CONSTRAINT FK_AR_ApptService FOREIGN KEY (appointment_service_id) REFERENCES AppointmentServices(appointment_service_id),
    CONSTRAINT FK_AR_Room        FOREIGN KEY (room_id)                REFERENCES Rooms(room_id)
);

CREATE TABLE AppointmentHistory (
    history_id           INT IDENTITY(1,1) PRIMARY KEY,
    appointment_id       INT NOT NULL,
    changed_by           INT NOT NULL,           -- references Users.user_id
    change_type          VARCHAR(20) NOT NULL
                         CHECK (change_type IN ('Created','Rescheduled','Cancelled','Status Change')),
    old_appointment_date DATE NULL,
    old_start_time       TIME NULL,
    old_status           VARCHAR(20) NULL,
    new_status           VARCHAR(20) NULL,
    reason               VARCHAR(MAX) NULL,
    changed_at           DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_AH_Appointment FOREIGN KEY (appointment_id) REFERENCES Appointments(appointment_id),
    CONSTRAINT FK_AH_User        FOREIGN KEY (changed_by)     REFERENCES Users(user_id)
);

/* ---------------------------------------------------------------------
   6. BILLING AND PAYMENTS
   --------------------------------------------------------------------- */
CREATE TABLE Billing (
    billing_id         INT IDENTITY(1,1) PRIMARY KEY,
    appointment_id     INT NOT NULL UNIQUE,      -- one bill per appointment
    promo_id           INT NULL,
    subtotal           DECIMAL(10,2) NOT NULL DEFAULT 0,
    discount_amount    DECIMAL(10,2) NOT NULL DEFAULT 0,
    downpayment        DECIMAL(10,2) NOT NULL DEFAULT 0,   -- 50% deposit
    downpayment_status VARCHAR(20) NOT NULL DEFAULT 'None'
                       CHECK (downpayment_status IN ('None','Held','Applied','Forfeited')),
    total_amount       DECIMAL(10,2) NOT NULL DEFAULT 0,   -- subtotal - discount
    balance_due        DECIMAL(10,2) NOT NULL DEFAULT 0,   -- total - downpayment - payments
    payment_status     VARCHAR(20) NOT NULL DEFAULT 'Unpaid'
                       CHECK (payment_status IN ('Unpaid','Partial','Paid','Forfeited')),
    created_at         DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at         DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Billing_Appointment FOREIGN KEY (appointment_id) REFERENCES Appointments(appointment_id),
    CONSTRAINT FK_Billing_Promo       FOREIGN KEY (promo_id)       REFERENCES Promotions(promo_id)
);

CREATE TABLE Payments (
    payment_id          INT IDENTITY(1,1) PRIMARY KEY,
    billing_id          INT NOT NULL,
    received_by         INT NOT NULL,            -- staff who recorded the payment
    payment_type        VARCHAR(20) NOT NULL CHECK (payment_type IN ('Downpayment','Final')),
    payment_method      VARCHAR(20) NOT NULL CHECK (payment_method IN ('Cash','GCash','Bank Transfer')),
    reference_number    VARCHAR(100) NULL,       -- GCash / bank reference
    amount_paid         DECIMAL(10,2) NOT NULL CHECK (amount_paid > 0),
    payment_date        DATETIME NOT NULL DEFAULT GETDATE(),
    verification_status VARCHAR(20) NOT NULL DEFAULT 'Pending'
                        CHECK (verification_status IN ('Pending','Verified','Rejected')),
    CONSTRAINT FK_Pay_Billing FOREIGN KEY (billing_id)  REFERENCES Billing(billing_id),
    CONSTRAINT FK_Pay_User    FOREIGN KEY (received_by) REFERENCES Users(user_id)
);

/* ---------------------------------------------------------------------
   7. STAFF COMMISSION
   --------------------------------------------------------------------- */
CREATE TABLE Commissions (
    commission_id          INT IDENTITY(1,1) PRIMARY KEY,
    user_id                INT NOT NULL,
    appointment_service_id INT NOT NULL,
    payment_id             INT NULL,             -- payment the commission is based on
    commission_rate        DECIMAL(5,2) NOT NULL DEFAULT 10.00,
    commission_amount      DECIMAL(10,2) NOT NULL DEFAULT 0,
    payout_status          VARCHAR(20) NOT NULL DEFAULT 'Unpaid'
                           CHECK (payout_status IN ('Unpaid','Paid')),
    payout_date            DATETIME NULL,
    created_at             DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Comm_User       FOREIGN KEY (user_id)                REFERENCES Users(user_id),
    CONSTRAINT FK_Comm_ApptService FOREIGN KEY (appointment_service_id) REFERENCES AppointmentServices(appointment_service_id),
    CONSTRAINT FK_Comm_Payment    FOREIGN KEY (payment_id)             REFERENCES Payments(payment_id)
);

/* ---------------------------------------------------------------------
   8. AUDIT TRAIL
   --------------------------------------------------------------------- */
CREATE TABLE ActivityLog (
    log_id      INT IDENTITY(1,1) PRIMARY KEY,
    user_id     INT NOT NULL,
    action      VARCHAR(50)  NOT NULL,           -- e.g. Login, Add Client, Backup
    module      VARCHAR(50)  NOT NULL,
    description VARCHAR(MAX) NULL,
    logged_at   DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Log_User FOREIGN KEY (user_id) REFERENCES Users(user_id)
);
GO

/* ---------------------------------------------------------------------
   SEED DATA
   --------------------------------------------------------------------- */
INSERT INTO ServiceCategories (category_name) VALUES
 ('Botox'), ('Glutathione'), ('Facial'), ('Slimming'),
 ('Hair Removal and Whitening'), ('Face Beautification'),
 ('Nails'), ('Massage - Feet'), ('Massage - Hand'), ('Massage - Full Body');

INSERT INTO Rooms (room_name, capacity) VALUES
 ('Aesthetic Lobby', 6), ('Diode Machine Lobby', 3), ('Diode Machine Room', 1),
 ('Facial Room', 1), ('Massage Room', 2);

-- Placeholder admin. Replace the hash with one generated by your C# app (e.g., BCrypt/PBKDF2).
INSERT INTO Users (full_name, username, password_hash, role)
VALUES ('System Administrator', 'admin', 'REPLACE_WITH_HASH', 'Admin');
GO
