# SKANSAPUNG - Sistem Informasi Sekolah

## Deskripsi Proyek

Sistem Informasi Sekolah SKANSAPUNG adalah aplikasi manajemen sekolah berbasis cross-platform menggunakan .NET MAUI sebagai framework utama. Aplikasi ini mengelola seluruh aspek administrasi dan akademik sekolah dengan arsitektur multi-platform yang dapat berjalan di Android, iOS, Windows, dan macOS dengan biaya minimal.

## Teknologi Stack

- **IDE**: Visual Studio 2022 Community (Free)
- **Framework**: .NET MAUI 8.0 (Cross-platform UI framework)
- **Backend API**: ASP.NET Core 8.0 Web API
- **Database**: PostgreSQL 15+ (Free & Open Source)
- **Authentication**: JWT Tokens dengan .NET MAUI Authentication
- **Real-time Communication**: SignalR untuk live updates
- **Push Notifications**: Firebase Cloud Messaging (Free tier)
- **State Management**: MVVM pattern dengan CommunityToolkit.Mvvm
- **UI Framework**: .NET MAUI Controls dengan Material Design

## **Struktur Proyek**

### **Frontend (.NET MAUI)**

Struktur saat ini mencerminkan arsitektur MVVM dengan dukungan offline.

```
SKANSAPUNG.MAUI/
├── Platforms/
│   ├── Android/
│   ├── iOS/
│   ├── Windows/
│   └── MacCatalyst/
├── Models/
│   ├── User.cs, Student.cs, ... (Model Domain)
│   └── LocalUser.cs, LocalStudent.cs, ... (Model SQLite)
├── ViewModels/
│   ├── BaseViewModel.cs
│   ├── LoginViewModel.cs
│   ├── DashboardViewModel.cs
│   ├── StudentsViewModel.cs
│   ├── AttendanceViewModel.cs
│   ├── ProfileViewModel.cs
│   ├── ExtracurricularViewModel.cs
│   └── NotificationsViewModel.cs
├── Views/
│   ├── LoginPage.xaml
│   ├── DashboardPage.xaml
│   ├── StudentsPage.xaml
│   ├── StudentDetailPage.xaml
│   ├── AttendancePage.xaml
│   ├── ProfilePage.xaml
│   ├── ExtracurricularPage.xaml
│   └── NotificationsPage.xaml
├── Services/
│   ├── IApiService.cs & ApiService.cs
│   ├── IAuthService.cs & AuthService.cs
│   ├── IGeolocationService.cs & GeolocationService.cs
│   ├── INotificationService.cs & NotificationService.cs
│   └── IDatabaseService.cs & DatabaseService.cs
├── Resources/
│   ├── Images/, Fonts/, Styles/
├── App.xaml
├── AppShell.xaml
└── MauiProgram.cs
```

### **Backend (ASP.NET Core Web API)**

Backend saat ini difokuskan pada fungsionalitas push notification.

```
SKANSAPUNG.API/
├── Controllers/
│   └── NotificationsController.cs
├── Models/
│   ├── NotificationRequest.cs
│   ├── RegisterTokenRequest.cs
│   └── FcmToken.cs
├── Services/
│   ├── INotificationService.cs & FirebaseNotificationService.cs
│   └── IFcmTokenService.cs & FcmTokenService.cs
├── Properties/
├── firebase-service-account.json
├── Program.cs
└── appsettings.json
```

## **Database Schema (PostgreSQL)**

### 42 Tabel Database

#### Tabel Utama

1. **users**
   - `id` (bigserial, primary key)
   - `name` (varchar)
   - `email` (varchar, unique)
   - `email_verified_at` (timestamp)
   - `password` (varchar)
   - `remember_token` (varchar)
   - `created_at` (timestamp)
   - `updated_at` (timestamp)
   - `image` (varchar)
   - `user_type` (enum: 'admin', 'guru', 'siswa')
   - `role` (enum: 'super_admin', 'admin', 'guru', 'siswa')
   - `status` (enum: 'aktif', 'tidak aktif', 'lulus', 'pindah')
   - `class_room_id` (bigint, foreign key)

2. **departments**
   - `id` (bigserial, primary key)
   - `name` (varchar)
   - `kode` (varchar, unique)
   - `status` (boolean)
   - `created_at` (timestamp)
   - `updated_at` (timestamp)

3. **school_years**
   - `id` (bigserial, primary key)
   - `tahun` (varchar)
   - `semester` (enum: 'ganjil', 'genap')
   - `status` (enum: 'aktif', 'tidak aktif')
   - `created_at` (timestamp)
   - `updated_at` (timestamp)

4. **class_rooms**
   - `id` (bigserial, primary key)
   - `name` (varchar)
   - `level` (varchar)
   - `department_id` (bigint, foreign key)
   - `school_year_id` (bigint, foreign key)
   - `homeroom_teacher_id` (bigint, foreign key)
   - `is_active` (boolean)
   - `created_at` (timestamp)
   - `updated_at` (timestamp)

5. **students**
   - `id` (bigserial, primary key)
   - `nis` (varchar, unique)
   - `nama_lengkap` (varchar)
   - `email` (varchar, unique)
   - `telp` (varchar)
   - `jenis_kelamin` (enum: 'L', 'P')
   - `agama` (varchar)
   - `class_room_id` (bigint, foreign key)
   - `user_id` (bigint, foreign key)
   - `created_at` (timestamp)
   - `updated_at` (timestamp)

6. **student_details**
   - `id` (bigserial, primary key)
   - `user_id` (bigint, foreign key)
   - `nipd` (varchar)
   - `gender` (enum: 'L', 'P')
   - `nisn` (varchar)
   - `birth_place` (varchar)
   - `birth_date` (date)
   - `nik` (varchar)
   - `religion` (varchar)
   - `address` (text)
   - `rt` (varchar)
   - `rw` (varchar)
   - `dusun` (varchar)
   - `kelurahan` (varchar)
   - `kecamatan` (varchar)
   - `postal_code` (varchar)
   - `residence_type` (varchar)
   - `transportation` (varchar)
   - `phone` (varchar)
   - `mobile_phone` (varchar)
   - `email` (varchar)
   - `skhun` (varchar)
   - `kps_recipient` (boolean)
   - `kps_number` (varchar)
   - `class_group` (varchar)
   - `un_number` (varchar)
   - `ijazah_number` (varchar)
   - `kip_recipient` (boolean)
   - `kip_number` (varchar)
   - `kip_name` (varchar)
   - `kks_number` (varchar)
   - `birth_certificate_no` (varchar)
   - `bank_name` (varchar)
   - `bank_account_number` (varchar)
   - `bank_account_holder` (varchar)
   - `pip_eligible` (boolean)
   - `pip_eligible_reason` (text)
   - `special_needs` (varchar)
   - `previous_school` (varchar)
   - `child_order` (integer)
   - `latitude` (decimal)
   - `longitude` (decimal)
   - `kk_number` (varchar)
   - `weight` (decimal)
   - `height` (decimal)
   - `head_circumference` (decimal)
   - `siblings_count` (integer)
   - `distance_to_school` (decimal)
   - `father_name` (varchar)
   - `father_birth_year` (integer)
   - `father_education` (varchar)
   - `father_occupation` (varchar)
   - `father_income` (varchar)
   - `father_nik` (varchar)
   - `mother_name` (varchar)
   - `mother_birth_year` (integer)
   - `mother_education` (varchar)
   - `mother_occupation` (varchar)
   - `mother_income` (varchar)
   - `mother_nik` (varchar)
   - `guardian_name` (varchar)
   - `guardian_birth_year` (integer)
   - `guardian_education` (varchar)
   - `guardian_occupation` (varchar)
   - `guardian_income` (varchar)
   - `guardian_nik` (varchar)
   - `created_at` (timestamp)
   - `updated_at` (timestamp)

7. **subjects**
   - `id` (bigserial, primary key)
   - `name` (varchar)
   - `code` (varchar, unique)
   - `description` (text)
   - `is_active` (boolean)
   - `created_at` (timestamp)
   - `updated_at` (timestamp)

8. **attendances**
   - `id` (bigserial, primary key)
   - `user_id` (bigint, foreign key)
   - `schedule_latitude` (double precision)
   - `schedule_longitude` (double precision)
   - `schedule_start_time` (time)
   - `schedule_end_time` (time)
   - `start_latitude` (double precision)
   - `start_longitude` (double precision)
   - `start_time` (time)
   - `end_time` (time)
   - `is_leave` (boolean)
   - `created_at` (timestamp)
   - `updated_at` (timestamp)
   - `deleted_at` (timestamp)
   - `end_latitude` (double precision)
   - `end_longitude` (double precision)

9. **extracurriculars**
   - `id` (bigserial, primary key)
   - `nama` (varchar)
   - `deskripsi` (text)
   - `hari` (varchar)
   - `jam_mulai` (time)
   - `jam_selesai` (time)
   - `tempat` (varchar)
   - `status` (boolean)
   - `created_at` (timestamp)
   - `updated_at` (timestamp)

10. **teaching_activities**
    - `id` (bigserial, primary key)
    - `guru_id` (bigint, foreign key)
    - `mata_pelajaran` (varchar)
    - `tanggal` (date)
    - `jam_mulai` (time)
    - `jam_selesai` (time)
    - `materi` (text)
    - `media_dan_alat` (varchar)
    - `important_notes` (text)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

11. **assessments**
    - `id` (bigserial, primary key)
    - `class_room_id` (bigint, foreign key)
    - `teacher_id` (bigint, foreign key)
    - `type` (enum: 'sumatif', 'non_sumatif')
    - `subject` (varchar)
    - `assessment_name` (varchar)
    - `date` (date)
    - `description` (text)
    - `notes` (text)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

12. **student_scores**
    - `id` (bigserial, primary key)
    - `assessment_id` (bigint, foreign key)
    - `student_id` (bigint, foreign key)
    - `score` (decimal)
    - `status` (enum: 'hadir', 'sakit', 'izin', 'alpha')
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

#### Tabel Tambahan

13. **notifications**
    - `id` (uuid, primary key)
    - `type` (varchar)
    - `notifiable_type` (varchar)
    - `notifiable_id` (bigint)
    - `data` (jsonb)
    - `read_at` (timestamp)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

14. **offices**
    - `id` (bigserial, primary key)
    - `name` (varchar)
    - `address` (text)
    - `latitude` (double precision)
    - `longitude` (double precision)
    - `radius` (integer)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

15. **shifts**
    - `id` (bigserial, primary key)
    - `name` (varchar)
    - `start_time` (time)
    - `end_time` (time)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)
    - `deleted_at` (timestamp)

16. **roles**
    - `id` (bigserial, primary key)
    - `name` (varchar)
    - `guard_name` (varchar)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

17. **permissions**
    - `id` (bigserial, primary key)
    - `name` (varchar)
    - `guard_name` (varchar)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

18. **model_has_roles**
    - `role_id` (bigint)
    - `model_type` (varchar)
    - `model_id` (bigint)

19. **model_has_permissions**
    - `permission_id` (bigint)
    - `model_type` (varchar)
    - `model_id` (bigint)

20. **role_has_permissions**
    - `permission_id` (bigint)
    - `role_id` (bigint)

21. **cache**
    - `key` (varchar, primary key)
    - `value` (text)
    - `expiration` (integer)

22. **sessions**
    - `id` (varchar, primary key)
    - `user_id` (bigint)
    - `ip_address` (inet)
    - `user_agent` (text)
    - `payload` (text)
    - `last_activity` (integer)

23. **jobs**
    - `id` (bigserial, primary key)
    - `queue` (varchar)
    - `payload` (text)
    - `attempts` (smallint)
    - `reserved_at` (integer)
    - `available_at` (integer)
    - `created_at` (integer)

24. **failed_jobs**
    - `id` (bigserial, primary key)
    - `uuid` (varchar, unique)
    - `connection` (text)
    - `queue` (text)
    - `payload` (text)
    - `exception` (text)
    - `failed_at` (timestamp)

25. **extracurricular_members**
    - `id` (bigserial, primary key)
    - `extracurricular_id` (bigint, foreign key)
    - `student_id` (bigint, foreign key)
    - `status` (enum: 'active', 'inactive')
    - `notes` (varchar)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

26. **pkl_internships**
    - `id` (bigserial, primary key)
    - `student_id` (bigint, foreign key)
    - `guru_pembimbing_id` (bigint, foreign key)
    - `office_id` (bigint, foreign key)
    - `company_leader` (varchar)
    - `company_type` (varchar)
    - `company_phone` (varchar)
    - `company_description` (text)
    - `start_date` (date)
    - `end_date` (date)
    - `position` (varchar)
    - `phone` (varchar)
    - `description` (text)
    - `status` (enum: 'pending', 'active', 'inactive')
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

27. **schedules**
    - `id` (bigserial, primary key)
    - `class_room_id` (bigint, foreign key)
    - `subject_id` (bigint, foreign key)
    - `teacher_id` (bigint, foreign key)
    - `day` (varchar)
    - `start_time` (time)
    - `end_time` (time)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

28. **announcements**
    - `id` (bigserial, primary key)
    - `title` (varchar)
    - `content` (text)
    - `author_id` (bigint, foreign key)
    - `published_at` (timestamp)
    - `expires_at` (timestamp)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

29. **events**
    - `id` (bigserial, primary key)
    - `title` (varchar)
    - `description` (text)
    - `location` (varchar)
    - `start_date` (timestamp)
    - `end_date` (timestamp)
    - `organizer_id` (bigint, foreign key)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

30. **documents**
    - `id` (bigserial, primary key)
    - `title` (varchar)
    - `file_path` (varchar)
    - `file_type` (varchar)
    - `file_size` (integer)
    - `uploaded_by` (bigint, foreign key)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

31. **payments**
    - `id` (bigserial, primary key)
    - `student_id` (bigint, foreign key)
    - `amount` (decimal)
    - `description` (varchar)
    - `payment_date` (date)
    - `payment_method` (varchar)
    - `transaction_id` (varchar)
    - `status` (varchar)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

32. **fees**
    - `id` (bigserial, primary key)
    - `name` (varchar)
    - `amount` (decimal)
    - `description` (text)
    - `due_date` (date)
    - `class_room_id` (bigint, foreign key)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

33. **student_fees**
    - `id` (bigserial, primary key)
    - `student_id` (bigint, foreign key)
    - `fee_id` (bigint, foreign key)
    - `amount_paid` (decimal)
    - `payment_date` (date)
    - `payment_status` (varchar)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

34. **inventories**
    - `id` (bigserial, primary key)
    - `name` (varchar)
    - `category` (varchar)
    - `quantity` (integer)
    - `condition` (varchar)
    - `location` (varchar)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

35. **books**
    - `id` (bigserial, primary key)
    - `title` (varchar)
    - `author` (varchar)
    - `publisher` (varchar)
    - `isbn` (varchar)
    - `category` (varchar)
    - `quantity` (integer)
    - `available` (integer)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

36. **book_loans**
    - `id` (bigserial, primary key)
    - `book_id` (bigint, foreign key)
    - `user_id` (bigint, foreign key)
    - `borrow_date` (date)
    - `due_date` (date)
    - `return_date` (date)
    - `status` (varchar)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

37. **rooms**
    - `id` (bigserial, primary key)
    - `name` (varchar)
    - `capacity` (integer)
    - `type` (varchar)
    - `location` (varchar)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

38. **room_schedules**
    - `id` (bigserial, primary key)
    - `room_id` (bigint, foreign key)
    - `user_id` (bigint, foreign key)
    - `purpose` (varchar)
    - `start_time` (timestamp)
    - `end_time` (timestamp)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

39. **device_tokens**
    - `id` (bigserial, primary key)
    - `user_id` (bigint, foreign key)
    - `device_token` (varchar)
    - `device_type` (varchar)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

40. **settings**
    - `id` (bigserial, primary key)
    - `key` (varchar)
    - `value` (text)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

41. **logs**
    - `id` (bigserial, primary key)
    - `user_id` (bigint)
    - `action` (varchar)
    - `description` (text)
    - `ip_address` (varchar)
    - `user_agent` (varchar)
    - `created_at` (timestamp)

42. **media**
    - `id` (bigserial, primary key)
    - `model_type` (varchar)
    - `model_id` (bigint)
    - `collection_name` (varchar)
    - `name` (varchar)
    - `file_name` (varchar)
    - `mime_type` (varchar)
    - `disk` (varchar)
    - `size` (integer)
    - `manipulations` (jsonb)
    - `custom_properties` (jsonb)
    - `responsive_images` (jsonb)
    - `order_column` (integer)
    - `created_at` (timestamp)
    - `updated_at` (timestamp)

## Fitur Utama

- **Multi-Platform Support**: Android, iOS, Windows, dan macOS
- **Offline-First Architecture**: Data tersimpan lokal dan sinkronisasi otomatis
- **GPS Tracking**: Absensi dengan geolokasi
- **Push Notifications**: Notifikasi real-time
- **Biometric Authentication**: Login dengan sidik jari/Face ID
- **Responsive Design**: Adaptive layouts untuk berbagai ukuran layar

## Setup Development Environment

1. **Install Visual Studio 2022** dengan workloads:
   - .NET Multi-platform App UI development
   - ASP.NET and web development
   - Mobile development with .NET

2. **Required SDKs**:
   - Android SDK (API 21+)
   - iOS SDK (Xcode untuk Mac)
   - Windows SDK (untuk Windows development)

3. **Clone Repository**:
   ```
   git clone https://github.com/yourusername/SIM_NETMAUI.git
   cd SIM_NETMAUI
   ```

4. **Restore Packages**:
   ```
   dotnet restore
   ```

5. **Run Application**:
   ```
   dotnet build
   dotnet run
   ```

## Lisensi

Hak Cipta © 2025 SKANSAPUNG. Hak Cipta Dilindungi. 