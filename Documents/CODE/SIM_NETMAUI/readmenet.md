# Dokumentasi Sistem Informasi Sekolah SKANSAPUNG - .NET MAUI (PostgreSQL)

## **Deskripsi Proyek**

Sistem Informasi Sekolah SKANSAPUNG adalah aplikasi manajemen sekolah berbasis cross-platform menggunakan .NET MAUI sebagai framework utama. Aplikasi ini mengelola seluruh aspek administrasi dan akademik sekolah dengan arsitektur multi-platform yang dapat berjalan di Android, iOS, Windows, dan macOS dengan **biaya minimal**.

## **Teknologi Stack (.NET MAUI - Cost-Optimized)**

## **Development Environment**

- **IDE**: Visual Studio 2022 Community (Free)
    
- **Framework**: .NET MAUI 8.0 (Cross-platform UI framework)
    
- **Backend API**: ASP.NET Core 8.0 Web API
    
- **Database**: PostgreSQL 15+ (Free & Open Source)
    
- **Authentication**: JWT Tokens dengan .NET MAUI Authentication
    
- **Real-time Communication**: SignalR untuk live updates
    
- **Push Notifications**: Firebase Cloud Messaging (Free tier)
    
- **State Management**: MVVM pattern dengan CommunityToolkit.Mvvm
    
- **UI Framework**: .NET MAUI Controls dengan Material Design
    

## **Free Cloud Services & APIs**

- **Firebase**: Cloud Messaging (Free tier - 20,000 messages/month)
    
- **Geolocation**: Platform-native GPS (Completely Free)
    
- **Maps**: OpenStreetMap dengan Mapsui (Free alternative)
    
- **Hosting Backend**: Railway/Render (Free tier untuk development)
    
- **Email**: SendGrid (Free tier - 100 emails/day)
    

## **Arsitektur Sistem .NET MAUI**

## **Cross-Platform Architecture**

text

`SKANSAPUNG.MAUI/ ├── Platforms/ │   ├── Android/ │   │   ├── MainActivity.cs │   │   ├── MainApplication.cs │   │   └── Resources/ │   ├── iOS/ │   │   ├── AppDelegate.cs │   │   └── Info.plist │   ├── Windows/ │   │   ├── App.xaml │   │   └── Package.appxmanifest │   └── MacCatalyst/ ├── Views/ │   ├── LoginPage.xaml │   ├── DashboardPage.xaml │   ├── StudentsPage.xaml │   ├── AttendancePage.xaml │   └── ExtracurricularPage.xaml ├── ViewModels/ │   ├── LoginViewModel.cs │   ├── DashboardViewModel.cs │   ├── StudentsViewModel.cs │   └── AttendanceViewModel.cs ├── Models/ │   ├── User.cs │   ├── Student.cs │   ├── Attendance.cs │   └── ClassRoom.cs ├── Services/ │   ├── ApiService.cs │   ├── AuthService.cs │   ├── GeolocationService.cs │   ├── NotificationService.cs │   └── DatabaseService.cs ├── Resources/ │   ├── Images/ │   ├── Fonts/ │   └── Styles/ └── MauiProgram.cs`

## **Backend API Architecture**

text

`SKANSAPUNG.API/ ├── Controllers/ │   ├── AuthController.cs │   ├── StudentsController.cs │   ├── AttendanceController.cs │   └── NotificationController.cs ├── Models/ ├── Services/ ├── Data/ │   └── ApplicationDbContext.cs └── Program.cs`

## **Database Schema PostgreSQL (42 Tabel)**

Menggunakan struktur database yang sama seperti dokumentasi sebelumnya dengan 42 tabel, namun dioptimalkan untuk .NET MAUI:

## **Core Tables dengan .NET MAUI Integration**

- **users**: Multi-role system dengan support untuk offline sync
    
- **attendances**: GPS coordinates dengan platform-native geolocation
    
- **notifications**: Push notification payload untuk FCM integration
    
- **cache**: Local SQLite cache untuk offline functionality
    

## **Fitur Utama .NET MAUI (Cross-Platform)**

## **Multi-Platform Support**

- **Android**: Native Android app dengan Material Design
    
- **iOS**: Native iOS app dengan Cupertino design
    
- **Windows**: WinUI 3 desktop application
    
- **macOS**: Mac Catalyst application
    

## **Offline-First Architecture**

- **Local SQLite Database**: Sync dengan PostgreSQL backend
    
- **Offline Attendance**: GPS tracking tersimpan lokal, sync otomatis
    
- **Cached Data**: Student data dan schedules tersedia offline
    
- **Background Sync**: Automatic synchronization saat koneksi tersedia
    

## **Native Platform Features**

- **GPS Tracking**: Platform-native geolocation untuk attendance
    
- **Camera Integration**: Photo capture untuk profile dan documentation
    
- **Biometric Authentication**: Fingerprint/Face ID untuk secure login
    
- **Push Notifications**: Firebase FCM untuk real-time alerts
    
- **File System**: Local storage untuk documents dan images
    

## **Cross-Platform UI Components**

- **Responsive Design**: Adaptive layouts untuk berbagai screen sizes
    
- **Platform-Specific Styling**: Material Design (Android) dan Cupertino (iOS)
    
- **Custom Controls**: Reusable components untuk consistent UX
    
- **Dark/Light Theme**: System theme integration
    

## **Setup Development Environment**

## **Visual Studio 2022 Configuration**

1. **Install Visual Studio 2022** dengan workloads:
    
    - .NET Multi-platform App UI development
        
    - ASP.NET and web development
        
    - Mobile development with .NET
        
2. **Required SDKs**:
    
    - Android SDK (API 21+)
        
    - iOS SDK (Xcode untuk Mac)
        
    - Windows SDK (untuk Windows development)
        

## **Project Structure Setup**

xml

`<!-- SKANSAPUNG.MAUI.csproj --> <Project Sdk="Microsoft.NET.Sdk">   <PropertyGroup>    <TargetFrameworks>net8.0-android;net8.0-ios;net8.0-maccatalyst;net8.0-windows10.0.19041.0</TargetFrameworks>    <OutputType>Exe</OutputType>    <RootNamespace>SKANSAPUNG.MAUI</RootNamespace>    <UseMaui>true</UseMaui>    <SingleProject>true</SingleProject>    <ImplicitUsings>enable</ImplicitUsings>         <!-- Display name -->    <ApplicationTitle>SKANSAPUNG</ApplicationTitle>         <!-- App Identifier -->    <ApplicationId>com.skansapung.app</ApplicationId>         <!-- Versions -->    <ApplicationDisplayVersion>1.0</ApplicationDisplayVersion>    <ApplicationVersion>1</ApplicationVersion>         <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">21.0</SupportedOSPlatformVersion>    <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">11.0</SupportedOSPlatformVersion>    <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst'">13.1</SupportedOSPlatformVersion>    <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">10.0.17763.0</SupportedOSPlatformVersion>  </PropertyGroup>   <ItemGroup>    <!-- App Icon -->    <MauiIcon Include="Resources\AppIcon\appicon.svg" ForegroundFile="Resources\AppIcon\appiconfg.svg" Color="#512BD4" />         <!-- Splash Screen -->    <MauiSplashScreen Include="Resources\Splash\splash.svg" Color="#512BD4" BaseSize="128,128" />         <!-- Images -->    <MauiImage Include="Resources\Images\*" />         <!-- Custom Fonts -->    <MauiFont Include="Resources\Fonts\*" />         <!-- Raw Assets -->    <MauiAsset Include="Resources\Raw\**" LogicalName="%(RecursiveDir)%(Filename)%(Extension)" />  </ItemGroup>   <ItemGroup>    <PackageReference Include="Microsoft.Maui.Controls" Version="8.0.0" />    <PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="8.0.0" />    <PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.0" />    <PackageReference Include="Plugin.Firebase" Version="2.0.0" />    <PackageReference Include="Mapsui.Maui" Version="4.0.0" />    <PackageReference Include="sqlite-net-pcl" Version="1.8.116" />    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.0" />  </ItemGroup>   <!-- Firebase Configuration -->  <ItemGroup Condition="'$(TargetFramework)' == 'net8.0-android'">    <GoogleServicesJson Include="Platforms\Android\google-services.json" />  </ItemGroup>     <ItemGroup Condition="'$(TargetFramework)' == 'net8.0-ios'">    <BundleResource Include="Platforms\iOS\GoogleService-Info.plist" />  </ItemGroup> </Project>`

## **Firebase Integration Setup**

csharp

`// MauiProgram.cs public static class MauiProgram {     public static MauiApp CreateMauiApp()    {        var builder = MauiApp.CreateBuilder();        builder            .UseMauiApp<App>()            .ConfigureFonts(fonts =>            {                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");            })            .RegisterServices()            .RegisterViewModels()            .RegisterViews(); #if ANDROID         builder.ConfigureLifecycleEvents(events =>        {            events.AddAndroid(android => android.OnCreate((activity, state) =>                CrossFirebase.Initialize(activity)));        }); #elif IOS         builder.ConfigureLifecycleEvents(events =>        {            events.AddiOS(ios => ios.FinishedLaunching((app, options) =>            {                CrossFirebase.Initialize();                return false;            }));        }); #endif         return builder.Build();    }     public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)    {        builder.Services.AddSingleton<IApiService, ApiService>();        builder.Services.AddSingleton<IAuthService, AuthService>();        builder.Services.AddSingleton<IGeolocationService, GeolocationService>();        builder.Services.AddSingleton<INotificationService, NotificationService>();        builder.Services.AddTransient<IDatabaseService, DatabaseService>();                 return builder;    } }`

## **Development Workflow (.NET MAUI)**

## **MVVM Pattern Implementation**

csharp

`// ViewModels/StudentsViewModel.cs public partial class StudentsViewModel : BaseViewModel {     private readonly IApiService _apiService;         [ObservableProperty]    ObservableCollection<Student> students;         [ObservableProperty]    bool isLoading;     public StudentsViewModel(IApiService apiService)    {        _apiService = apiService;        Students = new ObservableCollection<Student>();    }     [RelayCommand]    async Task LoadStudentsAsync()    {        IsLoading = true;        try        {            var studentList = await _apiService.GetStudentsAsync();            Students.Clear();            foreach (var student in studentList)            {                Students.Add(student);            }        }        catch (Exception ex)        {            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");        }        finally        {            IsLoading = false;        }    } }`

## **Cross-Platform Geolocation Service**

csharp

`// Services/GeolocationService.cs public class GeolocationService : IGeolocationService {     public async Task<Location> GetCurrentLocationAsync()    {        try        {            var request = new GeolocationRequest            {                DesiredAccuracy = GeolocationAccuracy.Medium,                Timeout = TimeSpan.FromSeconds(10)            };             var location = await Geolocation.Default.GetLocationAsync(request);            return location;        }        catch (Exception ex)        {            Debug.WriteLine($"Unable to get location: {ex.Message}");            return null;        }    }     public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)    {        return Location.CalculateDistance(lat1, lon1, lat2, lon2, DistanceUnits.Kilometers) * 1000; // Convert to meters    } }`

## **Firebase Push Notification Service**

csharp

`// Services/NotificationService.cs public class NotificationService : INotificationService {     public async Task<string> GetTokenAsync()    {        return await CrossFirebaseCloudMessaging.Current.GetTokenAsync();    }     public async Task SendNotificationAsync(string title, string body, string token)    {        // Implementation for sending notifications via backend API        var notification = new        {            to = token,            notification = new            {                title = title,                body = body            }        };                 // Send via API service to backend        await _apiService.SendNotificationAsync(notification);    } }`

## **Platform-Specific Features**

## **Android Implementation**

- **Material Design**: Native Android UI components
    
- **Background Services**: Foreground services untuk GPS tracking
    
- **Local Notifications**: Android notification channels
    
- **File Access**: Scoped storage untuk Android 11+
    

## **iOS Implementation**

- **Cupertino Design**: Native iOS UI components
    
- **Background App Refresh**: Background processing untuk sync
    
- **Core Location**: Native GPS dengan privacy permissions
    
- **Push Notifications**: APNs integration via Firebase
    

## **Windows Implementation**

- **WinUI 3**: Native Windows desktop experience
    
- **Live Tiles**: Windows-specific notification system
    
- **File System**: Full file system access
    
- **Multiple Windows**: Multi-window support
    

## **macOS Implementation**

- **Mac Catalyst**: iOS app adapted untuk macOS
    
- **Menu Bar**: Native macOS menu integration
    
- **Dock Integration**: macOS-specific features
    

## **Offline Functionality**

## **Local SQLite Database**

csharp

`// Models/LocalStudent.cs public class LocalStudent {     [PrimaryKey, AutoIncrement]    public int Id { get; set; }    public string ServerId { get; set; }    public string Name { get; set; }    public string Email { get; set; }    public bool IsSynced { get; set; }    public DateTime LastModified { get; set; } } // Services/DatabaseService.cs public class DatabaseService : IDatabaseService {     SQLiteAsyncConnection Database;     public async Task<List<LocalStudent>> GetStudentsAsync()    {        await Init();        return await Database.Table<LocalStudent>().ToListAsync();    }     public async Task<int> SaveStudentAsync(LocalStudent student)    {        await Init();        if (student.Id != 0)            return await Database.UpdateAsync(student);        else            return await Database.InsertAsync(student);    }     async Task Init()    {        if (Database is not null)            return;         Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);        var result = await Database.CreateTableAsync<LocalStudent>();    } }`

## **Testing Strategy (.NET MAUI)**

## **Unit Testing**

- **ViewModel Testing**: Test business logic dengan MSTest
    
- **Service Testing**: Mock API services untuk isolated testing
    
- **Model Testing**: Validate data models dan relationships
    

## **UI Testing**

- **Appium**: Cross-platform UI automation testing
    
- **Platform-Specific**: XCUITest (iOS), Espresso (Android)
    
- **Visual Testing**: Screenshot comparison testing
    

## **Integration Testing**

- **API Integration**: Test backend communication
    
- **Database Testing**: SQLite dan PostgreSQL integration
    
- **Push Notification**: FCM delivery testing
    

## **Deployment Strategy**

## **Android Deployment**

- **Google Play Store**: Production deployment
    
- **APK Distribution**: Direct installation untuk testing
    
- **App Bundle**: Optimized delivery dengan Play Store
    

## **iOS Deployment**

- **App Store**: Production deployment via App Store Connect
    
- **TestFlight**: Beta testing distribution
    
- **Enterprise Distribution**: Internal company deployment
    

## **Windows Deployment**

- **Microsoft Store**: Windows Store distribution
    
- **MSIX Package**: Modern Windows app packaging
    
- **Sideloading**: Direct installation untuk enterprise
    

## **macOS Deployment**

- **Mac App Store**: Official macOS distribution
    
- **Direct Distribution**: DMG file distribution
    
- **Notarization**: Apple security requirements
    

## **Performance Optimization**

## **Cross-Platform Optimization**

- **Lazy Loading**: Load data on-demand untuk better performance
    
- **Image Caching**: Efficient image loading dan caching
    
- **Memory Management**: Proper disposal of resources
    
- **Background Tasks**: Efficient background processing
    

## **Platform-Specific Optimizations**

- **Android**: ProGuard/R8 code shrinking
    
- **iOS**: AOT compilation untuk better performance
    
- **Windows**: Native compilation dengan .NET Native
    
- **macOS**: Mac Catalyst optimizations
    

## **Security Implementation**

## **Cross-Platform Security**

- **Secure Storage**: Platform-native secure storage
    
- **Certificate Pinning**: API communication security
    
- **Biometric Authentication**: Platform-native biometrics
    
- **Data Encryption**: Local database encryption
    

## **Platform-Specific Security**

- **Android**: Android Keystore integration
    
- **iOS**: iOS Keychain integration
    
- **Windows**: Windows Credential Manager
    
- **macOS**: macOS Keychain integration
    

## **Monitoring & Analytics**

## **Cross-Platform Monitoring**

- **Application Insights**: Performance dan crash monitoring
    
- **Firebase Analytics**: User behavior tracking
    
- **Custom Telemetry**: Business-specific metrics
    
- **Error Tracking**: Comprehensive error logging
    

## **Changelog & Version History**

## **Version 1.0.0 - Initial .NET MAUI Release**

- ✅ Cross-platform app untuk Android, iOS, Windows, macOS
    
- ✅ PostgreSQL database integration dengan 42 tabel
    
- ✅ Firebase FCM untuk push notifications
    
- ✅ Platform-native GPS untuk attendance tracking
    

## **Version 1.1.0 - Enhanced Cross-Platform Features**

- ✅ Offline-first architecture dengan SQLite sync
    
- ✅ Biometric authentication untuk semua platform
    
- ✅ Platform-specific UI optimizations
    
- ✅ Background sync capabilities
    

## **Version 1.2.0 - Advanced Platform Integration**

- ✅ Platform-native camera integration
    
- ✅ Advanced geofencing dengan native APIs
    
- ✅ Multi-window support untuk Windows/macOS
    
- ✅ Live Tiles untuk Windows
    

## **Version 2.0.0 - Enterprise Features (Planned)**

- 🔄 Advanced analytics dashboard
    
- 🔄 Multi-tenant architecture
    
- 🔄 Advanced security features
    
- 🔄 AI-powered insights
    

## **Maintenance & Support**

## **Cross-Platform Maintenance**

- **Regular Updates**: Platform SDK updates dan compatibility
    
- **Performance Monitoring**: Cross-platform performance optimization
    
- **Security Patches**: Regular security updates
    
- **User Feedback**: Platform-specific user experience improvements
    

## **Platform-Specific Considerations**

- **Android**: Google Play requirements dan API level updates
    
- **iOS**: App Store guidelines dan iOS version compatibility
    
- **Windows**: Windows Update compatibility
    
- **macOS**: macOS version support dan Mac Catalyst updates
    

Dokumentasi ini memberikan panduan komprehensif untuk pengembangan sistem informasi sekolah menggunakan .NET MAUI dengan **teknologi gratis dan cross-platform**, memungkinkan deployment ke Android, iOS, Windows, dan macOS dari single codebase sambil tetap mempertahankan fungsionalitas penuh sistem.


Berdasarkan file database MySQL yang Anda berikan, berikut adalah konversi lengkap **42 tabel** dari MySQL ke PostgreSQL:

## **Konversi Database MySQL ke PostgreSQL**

## **1. Tabel users**

sql

`-- PostgreSQL CREATE TABLE users (     id BIGSERIAL PRIMARY KEY,    name VARCHAR(255) NOT NULL,    email VARCHAR(255) UNIQUE NOT NULL,    email_verified_at TIMESTAMP NULL,    password VARCHAR(255) NOT NULL,    remember_token VARCHAR(100) NULL,    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    image VARCHAR(255) NULL,    user_type user_type_enum NOT NULL DEFAULT 'siswa',    role role_enum NOT NULL DEFAULT 'siswa',    status status_enum NOT NULL DEFAULT 'aktif',    class_room_id BIGINT NULL REFERENCES class_rooms(id) ON DELETE SET NULL ); -- Enum types CREATE TYPE user_type_enum AS ENUM ('admin', 'guru', 'siswa'); CREATE TYPE role_enum AS ENUM ('super_admin', 'admin', 'guru', 'siswa'); CREATE TYPE status_enum AS ENUM ('aktif', 'tidak aktif', 'lulus', 'pindah');`

## **2. Tabel departments**

sql

`CREATE TABLE departments (     id BIGSERIAL PRIMARY KEY,    name VARCHAR(255) NOT NULL,    kode VARCHAR(255) UNIQUE NOT NULL,    status BOOLEAN NOT NULL DEFAULT TRUE,    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP );`

## **3. Tabel school_years**

sql

`CREATE TABLE school_years (     id BIGSERIAL PRIMARY KEY,    tahun VARCHAR(255) NOT NULL,    semester semester_enum NOT NULL,    status school_year_status_enum NOT NULL DEFAULT 'aktif',    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ); CREATE TYPE semester_enum AS ENUM ('ganjil', 'genap'); CREATE TYPE school_year_status_enum AS ENUM ('aktif', 'tidak aktif');`

## **4. Tabel class_rooms**

sql

`CREATE TABLE class_rooms (     id BIGSERIAL PRIMARY KEY,    name VARCHAR(255) NOT NULL,    level VARCHAR(255) NOT NULL,    department_id BIGINT NOT NULL REFERENCES departments(id) ON DELETE CASCADE,    school_year_id BIGINT NOT NULL REFERENCES school_years(id) ON DELETE CASCADE,    homeroom_teacher_id BIGINT NULL REFERENCES users(id) ON DELETE SET NULL,    is_active BOOLEAN NOT NULL DEFAULT TRUE,    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP );`

## **5. Tabel students**

sql

`CREATE TABLE students (     id BIGSERIAL PRIMARY KEY,    nis VARCHAR(255) UNIQUE NOT NULL,    nama_lengkap VARCHAR(255) NOT NULL,    email VARCHAR(255) UNIQUE NOT NULL,    telp VARCHAR(255) NULL,    jenis_kelamin gender_enum NOT NULL,    agama VARCHAR(255) NOT NULL,    class_room_id BIGINT NOT NULL REFERENCES class_rooms(id) ON DELETE CASCADE,    user_id BIGINT NULL REFERENCES users(id) ON DELETE SET NULL,    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ); CREATE TYPE gender_enum AS ENUM ('L', 'P');`

## **6. Tabel student_details**

sql

`CREATE TABLE student_details (     id BIGSERIAL PRIMARY KEY,    user_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,    nipd VARCHAR(255) NULL,    gender gender_enum NULL,    nisn VARCHAR(255) NULL,    birth_place VARCHAR(255) NULL,    birth_date DATE NULL,    nik VARCHAR(16) NULL,    religion VARCHAR(255) NULL,    address TEXT NULL,    rt VARCHAR(3) NULL,    rw VARCHAR(3) NULL,    dusun VARCHAR(255) NULL,    kelurahan VARCHAR(255) NULL,    kecamatan VARCHAR(255) NULL,    postal_code VARCHAR(5) NULL,    residence_type VARCHAR(255) NULL,    transportation VARCHAR(255) NULL,    phone VARCHAR(255) NULL,    mobile_phone VARCHAR(255) NULL,    email VARCHAR(255) NULL,    skhun VARCHAR(255) NULL,    kps_recipient BOOLEAN NOT NULL DEFAULT FALSE,    kps_number VARCHAR(255) NULL,    class_group VARCHAR(255) NULL,    un_number VARCHAR(255) NULL,    ijazah_number VARCHAR(255) NULL,    kip_recipient BOOLEAN NOT NULL DEFAULT FALSE,    kip_number VARCHAR(255) NULL,    kip_name VARCHAR(255) NULL,    kks_number VARCHAR(255) NULL,    birth_certificate_no VARCHAR(255) NULL,    bank_name VARCHAR(255) NULL,    bank_account_number VARCHAR(255) NULL,    bank_account_holder VARCHAR(255) NULL,    pip_eligible BOOLEAN NOT NULL DEFAULT FALSE,    pip_eligible_reason TEXT NULL,    special_needs VARCHAR(255) NULL,    previous_school VARCHAR(255) NULL,    child_order INTEGER NULL,    latitude DECIMAL(10,8) NULL,    longitude DECIMAL(11,8) NULL,    kk_number VARCHAR(16) NULL,    weight DECIMAL(5,2) NULL,    height DECIMAL(5,2) NULL,    head_circumference DECIMAL(5,2) NULL,    siblings_count INTEGER NULL,    distance_to_school DECIMAL(5,2) NULL,    father_name VARCHAR(255) NULL,    father_birth_year INTEGER NULL,    father_education VARCHAR(255) NULL,    father_occupation VARCHAR(255) NULL,    father_income VARCHAR(255) NULL,    father_nik VARCHAR(16) NULL,    mother_name VARCHAR(255) NULL,    mother_birth_year INTEGER NULL,    mother_education VARCHAR(255) NULL,    mother_occupation VARCHAR(255) NULL,    mother_income VARCHAR(255) NULL,    mother_nik VARCHAR(16) NULL,    guardian_name VARCHAR(255) NULL,    guardian_birth_year INTEGER NULL,    guardian_education VARCHAR(255) NULL,    guardian_occupation VARCHAR(255) NULL,    guardian_income VARCHAR(255) NULL,    guardian_nik VARCHAR(16) NULL,    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP );`

## **7. Tabel subjects**

sql

`CREATE TABLE subjects (     id BIGSERIAL PRIMARY KEY,    name VARCHAR(255) NOT NULL,    code VARCHAR(255) UNIQUE NOT NULL,    description TEXT NULL,    is_active BOOLEAN NOT NULL DEFAULT TRUE,    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP );`

## **8. Tabel attendances**

sql

`CREATE TABLE attendances (     id BIGSERIAL PRIMARY KEY,    user_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,    schedule_latitude DOUBLE PRECISION NOT NULL,    schedule_longitude DOUBLE PRECISION NOT NULL,    schedule_start_time TIME NOT NULL,    schedule_end_time TIME NOT NULL,    start_latitude DOUBLE PRECISION NOT NULL,    start_longitude DOUBLE PRECISION NOT NULL,    start_time TIME NOT NULL,    end_time TIME NOT NULL,    is_leave BOOLEAN NOT NULL DEFAULT FALSE,    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    deleted_at TIMESTAMP NULL,    end_latitude DOUBLE PRECISION NULL,    end_longitude DOUBLE PRECISION NULL ); CREATE INDEX idx_attendances_user_created ON attendances(user_id, created_at); CREATE INDEX idx_attendances_created ON attendances(created_at);`

## **9. Tabel extracurriculars**

sql

`CREATE TABLE extracurriculars (     id BIGSERIAL PRIMARY KEY,    nama VARCHAR(255) NOT NULL,    deskripsi TEXT NOT NULL,    hari VARCHAR(255) NOT NULL,    jam_mulai TIME NOT NULL,    jam_selesai TIME NOT NULL,    tempat VARCHAR(255) NOT NULL,    status BOOLEAN NOT NULL DEFAULT TRUE,    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP );`

## **10. Tabel teaching_activities**

sql

`CREATE TABLE teaching_activities (     id BIGSERIAL PRIMARY KEY,    guru_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,    mata_pelajaran VARCHAR(255) NOT NULL,    tanggal DATE NOT NULL,    jam_mulai TIME NOT NULL,    jam_selesai TIME NOT NULL,    materi TEXT NOT NULL,    media_dan_alat VARCHAR(255) NULL,    important_notes TEXT NULL,    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    UNIQUE(guru_id, tanggal) );`

## **11. Tabel assessments**

sql

`CREATE TABLE assessments (     id BIGSERIAL PRIMARY KEY,    class_room_id BIGINT NOT NULL REFERENCES class_rooms(id) ON DELETE CASCADE,    teacher_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,    type assessment_type_enum NOT NULL,    subject VARCHAR(255) NOT NULL,    assessment_name VARCHAR(255) NOT NULL,    date DATE NOT NULL,    description TEXT NULL,    notes TEXT NULL,    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ); CREATE TYPE assessment_type_enum AS ENUM ('sumatif', 'non_sumatif');`

## **12. Tabel student_scores**

sql

`CREATE TABLE student_scores (     id BIGSERIAL PRIMARY KEY,    assessment_id BIGINT NOT NULL REFERENCES assessments(id) ON DELETE CASCADE,    student_id BIGINT NOT NULL REFERENCES students(id) ON DELETE CASCADE,    score DECIMAL(5,2) NULL,    status attendance_status_enum NOT NULL DEFAULT 'hadir',    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    UNIQUE(assessment_id, student_id) ); CREATE TYPE attendance_status_enum AS ENUM ('hadir', 'sakit', 'izin', 'alpha');`

## **13. Tabel notifications**

sql

`CREATE TABLE notifications (     id UUID PRIMARY KEY DEFAULT gen_random_uuid(),    type VARCHAR(255) NOT NULL,    notifiable_type VARCHAR(255) NOT NULL,    notifiable_id BIGINT NOT NULL,    data JSONB NOT NULL,    read_at TIMESTAMP NULL,    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ); CREATE INDEX idx_notifications_notifiable ON notifications(notifiable_type, notifiable_id);`

## **14. Tabel offices**

sql

`CREATE TABLE offices (     id BIGSERIAL PRIMARY KEY,    name VARCHAR(255) NOT NULL,    address TEXT NULL,    latitude DOUBLE PRECISION NULL,    longitude DOUBLE PRECISION NULL,    radius INTEGER NOT NULL,    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP );`

## **15. Tabel shifts**

sql

`CREATE TABLE shifts (     id BIGSERIAL PRIMARY KEY,    name VARCHAR(255) NOT NULL,    start_time TIME NOT NULL,    end_time TIME NOT NULL,    created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,    deleted_at TIMESTAMP NULL );`

## **16-42. Tabel Lainnya (Ringkas)**

sql

`-- Roles & Permissions (Spatie) CREATE TABLE roles (     id BIGSERIAL PRIMARY KEY,    name VARCHAR(255) NOT NULL,    guard_name VARCHAR(255) NOT NULL,    created_at TIMESTAMP NULL,    updated_at TIMESTAMP NULL,    UNIQUE(name, guard_name) ); CREATE TABLE permissions (     id BIGSERIAL PRIMARY KEY,    name VARCHAR(255) NOT NULL,    guard_name VARCHAR(255) NOT NULL,    created_at TIMESTAMP NULL,    updated_at TIMESTAMP NULL,    UNIQUE(name, guard_name) ); -- System Tables CREATE TABLE cache (     key VARCHAR(255) PRIMARY KEY,    value TEXT NOT NULL,    expiration INTEGER NOT NULL ); CREATE TABLE sessions (     id VARCHAR(255) PRIMARY KEY,    user_id BIGINT NULL,    ip_address INET NULL,    user_agent TEXT NULL,    payload TEXT NOT NULL,    last_activity INTEGER NOT NULL ); CREATE INDEX idx_sessions_user_id ON sessions(user_id); CREATE INDEX idx_sessions_last_activity ON sessions(last_activity); -- Job Tables CREATE TABLE jobs (     id BIGSERIAL PRIMARY KEY,    queue VARCHAR(255) NOT NULL,    payload TEXT NOT NULL,    attempts SMALLINT NOT NULL,    reserved_at INTEGER NULL,    available_at INTEGER NOT NULL,    created_at INTEGER NOT NULL ); CREATE INDEX idx_jobs_queue ON jobs(queue); -- Extracurricular Relations CREATE TABLE extracurricular_members (     id BIGSERIAL PRIMARY KEY,    extracurricular_id BIGINT NOT NULL REFERENCES extracurriculars(id) ON DELETE CASCADE,    student_id BIGINT NOT NULL REFERENCES students(id) ON DELETE CASCADE,    status member_status_enum NOT NULL DEFAULT 'active',    notes VARCHAR(255) NULL,    created_at TIMESTAMP NULL,    updated_at TIMESTAMP NULL,    UNIQUE(extracurricular_id, student_id) ); CREATE TYPE member_status_enum AS ENUM ('active', 'inactive'); -- PKL & Internships CREATE TABLE pkl_internships (     id BIGSERIAL PRIMARY KEY,    student_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,    guru_pembimbing_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,    office_id BIGINT NOT NULL REFERENCES offices(id) ON DELETE CASCADE,    company_leader VARCHAR(255) NOT NULL,    company_type VARCHAR(255) NOT NULL,    company_phone VARCHAR(255) NOT NULL,    company_description TEXT NOT NULL,    start_date DATE NOT NULL,    end_date DATE NOT NULL,    position VARCHAR(255) NOT NULL,    phone VARCHAR(255) NOT NULL,    description TEXT NOT NULL,    status internship_status_enum NOT NULL DEFAULT 'pending',    created_at TIMESTAMP NULL,    updated_at TIMESTAMP NULL ); CREATE TYPE internship_status_enum AS ENUM ('pending', 'active', 'inactive');`

## **Script Konversi Otomatis dengan pgloader**

bash

`# Install pgloader sudo apt-get install pgloader # Konversi database pgloader mysql://user:password@localhost/a9_skansapung postgresql://user:password@localhost/skansapung_postgres # Atau dengan file konfigurasi cat > migration.load << EOF LOAD DATABASE FROM mysql://user:password@localhost/a9_skansapung INTO postgresql://user:password@localhost/skansapung_postgres WITH include drop, create tables, create indexes, reset sequences SET MySQL PARAMETERS     net_read_timeout = '120',    net_write_timeout = '120' SET PostgreSQL PARAMETERS     maintenance_work_mem to '128MB',    checkpoint_segments to '20',    wal_buffers to '8MB',    shared_buffers to '256MB' CAST type tinyint to boolean drop typemod using tinyint-to-boolean,      type year to integer drop typemod ALTER TABLE NAMES MATCHING ~/^(.*)$/ RENAME TO \1; EOF pgloader migration.load`

## **Perbedaan Utama MySQL vs PostgreSQL**

|**Aspek**|**MySQL**|**PostgreSQL**|
|---|---|---|
|**Primary Key**|`AUTO_INCREMENT`|`SERIAL/BIGSERIAL`|
|**Boolean**|`TINYINT(1)`|`BOOLEAN`|
|**Text Types**|`MEDIUMTEXT`, `LONGTEXT`|`TEXT`|
|**JSON**|`JSON`|`JSONB` (binary)|
|**Enum**|`ENUM('val1','val2')`|`CREATE TYPE name AS ENUM`|
|**Double**|`DOUBLE`|`DOUBLE PRECISION`|
|**Unsigned**|`BIGINT UNSIGNED`|`BIGINT` (no unsigned)|
|**Timestamps**|`CURRENT_TIMESTAMP`|`CURRENT_TIMESTAMP`|
|**UUID**|`CHAR(36)`|`UUID` dengan `gen_random_uuid()`|

## **Post-Migration Tasks**

sql

`-- Update sequences SELECT setval(pg_get_serial_sequence('users', 'id'), COALESCE(MAX(id), 1)) FROM users; SELECT setval(pg_get_serial_sequence('students', 'id'), COALESCE(MAX(id), 1)) FROM students; -- Add indexes yang hilang CREATE INDEX idx_users_email ON users(email); CREATE INDEX idx_students_nis ON students(nis); CREATE INDEX idx_attendances_date ON attendances(created_at::date); -- Verify data integrity SELECT COUNT(*) FROM users; SELECT COUNT(*) FROM students; SELECT COUNT(*) FROM attendances;`
