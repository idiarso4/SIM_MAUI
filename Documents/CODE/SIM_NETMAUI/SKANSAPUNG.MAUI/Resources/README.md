# SKANSAPUNG MAUI Resources

## 📁 Struktur Folder

```
Resources/
├── AppIcon/           # App icons (SVG)
├── Fonts/            # Custom fonts (.ttf)
├── Images/           # UI icons (SVG/PNG)
├── Splash/           # Splash screen (SVG)
└── Styles/           # XAML styles
```

## 🎨 Assets yang Diperlukan

### Icons (SVG/PNG)
- `person.svg` - Profile, user management
- `sports_soccer.svg` - Extracurricular activities  
- `school.svg` - Students, education
- `check_circle.svg` - Attendance, validation
- `arrow_right.svg` - Navigation, next

### App Icons
- `appicon.svg` - Main app icon
- `appiconfg.svg` - Foreground app icon

### Splash Screen
- `splash.svg` - App launch screen

### Fonts (.ttf)
- `OpenSans-Regular.ttf` - Regular text
- `OpenSans-Semibold.ttf` - Bold text

## 📥 Download Fonts

Download Open Sans fonts dari Google Fonts:
https://fonts.google.com/specimen/Open+Sans

## 🔧 Penggunaan

### Di XAML
```xml
<Image Source="person.png" />
<Label FontFamily="OpenSansRegular" />
```

### Di Code-behind
```csharp
image.Source = "person.png";
label.FontFamily = "OpenSansRegular";
```

## 📱 Platform Assets

Untuk platform-specific assets, tambahkan di folder:
- `Platforms/Android/Resources/`
- `Platforms/iOS/Resources/`
- `Platforms/Windows/`
- `Platforms/MacCatalyst/` 