# SKANSAPUNG .NET MAUI Application

A comprehensive school management system built with .NET MAUI, featuring offline-first architecture and real-time synchronization capabilities.

## 🚀 Features

### 📱 Cross-Platform Support
- **Android**: Full native support with Firebase integration
- **iOS**: Native iOS app with push notifications
- **Windows**: Desktop application
- **macOS**: Native macOS support

### 🔄 Offline-First Architecture
- **SQLite Database**: Local data storage for offline functionality
- **Two-Way Sync**: Automatic synchronization when online
- **Connectivity Detection**: Smart handling of network status
- **Data Persistence**: All data cached locally for instant access

### 🎯 Core Modules
- **Authentication**: Secure login with role-based access
- **Dashboard**: Overview of student activities and statistics
- **Students Management**: Complete student information and profiles
- **Attendance Tracking**: GPS-based attendance with offline support
- **Grades Management**: Academic performance tracking
- **Schedule Management**: Class schedules and timetables
- **Notifications**: Real-time push notifications
- **Reports**: Comprehensive reporting system
- **Extracurricular Activities**: Activity management
- **Teachers Management**: Staff information and assignments

### 🔥 Firebase Integration
- **Push Notifications**: Real-time notifications for important events
- **Device Registration**: Automatic token management
- **Cross-Platform**: Unified notification system

## 🏗️ Project Structure

```
SKANSAPUNG.MAUI/
├── Models/                 # Data models (local and API)
├── Views/                  # XAML pages and UI components
├── ViewModels/            # MVVM pattern implementation
├── Services/              # Business logic and data services
├── Resources/             # Images, fonts, and styling
├── Platforms/             # Platform-specific implementations
└── Converters/            # XAML value converters
```

## 🗄️ Database Architecture

### Local Database (SQLite)
- **Offline Storage**: All data cached locally
- **Sync Management**: Automatic conflict resolution
- **Performance**: Fast local queries and operations

### API Integration
- **RESTful API**: Full CRUD operations
- **Real-time Updates**: Live data synchronization
- **Error Handling**: Robust error recovery mechanisms

## 🔧 Recent Updates

### Database Migration (Latest)
- ✅ **Complete Model Migration**: All API models successfully migrated to database
- ✅ **New Tables Added**: 
  - Announcements, Documents, Events, School Events
  - Notifications, Offices, Permissions, Roles, Shifts
- ✅ **Foreign Key Relationships**: Proper database constraints
- ✅ **Naming Conflicts Resolved**: Fixed Role enum/class conflicts
- ✅ **Service Integration**: Updated Firebase notification service

### Technical Improvements
- **Offline-First Design**: All operations work without internet
- **Smart Sync**: Intelligent data synchronization
- **Performance Optimization**: Efficient data loading and caching
- **Error Recovery**: Graceful handling of network issues

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 or VS Code
- Android SDK (for Android development)
- Xcode (for iOS development, macOS only)

### Installation
1. Clone the repository
2. Navigate to the MAUI project directory
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
4. Build the project:
   ```bash
   dotnet build
   ```

### Running the Application
- **Android**: `dotnet build -t:Run -f net9.0-android`
- **iOS**: `dotnet build -t:Run -f net9.0-ios`
- **Windows**: `dotnet build -t:Run -f net9.0-windows10.0.19041.0`
- **macOS**: `dotnet build -t:Run -f net9.0-maccatalyst`

## 🔐 Authentication

The application supports multiple user roles:
- **Super Admin**: Full system access
- **Admin**: Administrative functions
- **Teacher**: Teaching staff access
- **Student**: Student-specific features

## 📊 Data Synchronization

### Offline Mode
- All data operations work offline
- Changes are queued for synchronization
- Automatic retry on network restoration

### Online Mode
- Real-time data synchronization
- Conflict resolution for concurrent changes
- Background sync for optimal performance

## 🧪 Testing

### Connectivity Testing
- Manual offline/online simulation
- Network status indicators
- Sync status monitoring

### Data Integrity
- Local data validation
- API response validation
- Conflict resolution testing

## 🔧 Configuration

### API Endpoints
Configure API endpoints in `Services/ApiService.cs`

### Database Settings
SQLite database configuration in `Services/DatabaseService.cs`

### Firebase Configuration
- Android: `Platforms/Android/google-services.json`
- iOS: `Platforms/iOS/GoogleService-Info.plist`

## 📱 Platform-Specific Features

### Android
- Firebase Cloud Messaging
- GPS location services
- Background sync

### iOS
- Push notifications
- Location permissions
- Background app refresh

### Windows
- Desktop notifications
- File system access
- Windows integration

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 🆘 Support

For support and questions:
- Create an issue in the repository
- Contact the development team
- Check the documentation

---

**Last Updated**: December 2024
**Version**: 1.0.0
**Status**: Active Development 