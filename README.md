# Todo App - .NET MAUI for Android

A simple Todo application built with .NET MAUI (Multi-platform App UI) targeting Android devices. This app demonstrates MVVM architecture, local data persistence, and modern UI design patterns.

## Features

### ✅ Core Functionality
- **Add new todos** with title and description
- **Edit existing todos** including marking as complete/incomplete
- **Delete todos** with confirmation dialog
- **Filter todos** by status (All, Pending, Completed)
- **Persistent storage** using local JSON file
- **Real-time UI updates** with data binding

### ✅ UI/UX Features
- **Modern Material Design** interface
- **Pull-to-refresh** functionality
- **Responsive layout** for various screen sizes
- **Visual feedback** for completed items (strikethrough, grayed out)
- **Loading indicators** for better user experience
- **Empty state** messaging

### ✅ Technical Features
- **MVVM Architecture** with proper separation of concerns
- **Dependency Injection** for services and ViewModels
- **Navigation** between pages using Shell routing
- **Data Binding** with value converters
- **Local Storage** with JSON serialization
- **Error Handling** with user-friendly messages

## Prerequisites

To build and run this application, you need:

### Required Software
- **.NET 8.0 SDK** or later
- **Visual Studio 2022** (17.8 or later) with MAUI workload, OR
- **Visual Studio Code** with C# Dev Kit extension
- **Android SDK** (API 21-34)
- **Java Development Kit** (JDK 11 or later)

### Android Development Setup
1. Install Android SDK through Visual Studio Installer or Android Studio
2. Ensure you have Android API levels 21 (minimum) and 34 (target) installed
3. Set up an Android emulator or connect a physical device

## Quick Start

### 1. Clone the Repository
```bash
git clone <repository-url>
cd dotnet-maui-test
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Build the Application
```bash
# For Debug build
dotnet build -c Debug -f net8.0-android

# For Release build
dotnet build -c Release -f net8.0-android
```

### 4. Run on Android Emulator
```bash
# Start Android emulator first, then run:
dotnet run -f net8.0-android
```

### 5. Deploy to Android Device
```bash
# With device connected via USB debugging:
dotnet publish -f net8.0-android -c Release
```

## Project Structure

```
TodoApp-Maui/
├── Models/
│   └── TodoItem.cs                 # Todo item data model
├── Services/
│   ├── ITodoService.cs             # Service interface
│   └── TodoService.cs              # Data service implementation
├── ViewModels/
│   ├── BaseViewModel.cs            # Base ViewModel with MVVM helpers
│   ├── MainPageViewModel.cs        # Main page logic
│   ├── AddTodoViewModel.cs         # Add todo logic
│   └── EditTodoViewModel.cs        # Edit todo logic
├── Views/
│   ├── AddTodoPage.xaml/.cs        # Add todo interface
│   └── EditTodoPage.xaml/.cs       # Edit todo interface
├── Converters/
│   └── ValueConverters.cs          # UI value converters
├── Platforms/
│   └── Android/
│       ├── AndroidManifest.xml     # Android app configuration
│       ├── MainActivity.cs         # Main Android activity
│       └── MainApplication.cs      # Android application class
├── Resources/                      # App resources (fonts, images, styles)
├── MainPage.xaml/.cs              # Main todo list interface
├── App.xaml/.cs                   # Application configuration
├── AppShell.xaml/.cs              # Navigation shell
├── MauiProgram.cs                 # Dependency injection setup
└── TodoApp-Maui.csproj           # Project configuration
```

## Architecture Overview

### MVVM Pattern
- **Models**: `TodoItem` represents the data structure
- **Views**: XAML pages for user interface
- **ViewModels**: Business logic and data binding

### Data Flow
1. **UI Events** → ViewModels (via Commands)
2. **ViewModels** → Services (data operations)
3. **Services** → Local Storage (JSON persistence)
4. **Data Updates** → UI (via INotifyPropertyChanged)

### Navigation
- Uses Shell-based navigation
- Route registration in `AppShell.xaml.cs`
- Parameter passing for edit operations

## Building for Release

### Create APK
```bash
# Build release APK
dotnet publish -f net8.0-android -c Release -p:AndroidPackageFormat=apk

# APK location: bin/Release/net8.0-android/publish/
```

### Create AAB (Android App Bundle)
```bash
# Build AAB for Google Play Store
dotnet publish -f net8.0-android -c Release -p:AndroidPackageFormat=aab
```

## Testing

### Recommended Testing Scenarios
1. **Add new todos** with various title/description combinations
2. **Edit todos** and verify persistence
3. **Delete todos** and confirm removal
4. **Toggle completion status** and verify visual changes
5. **Filter functionality** across different states
6. **App lifecycle** (close/reopen to verify persistence)
7. **Rotation testing** for responsive design

### Android Compatibility
- **Minimum API**: Android 5.0 (API 21)
- **Target API**: Android 14 (API 34)
- **Tested on**: Android emulators and various devices

## Troubleshooting

### Common Issues

#### Build Errors
- **NETSDK1147**: Install required workloads with `dotnet workload restore`
- **Missing Android SDK**: Ensure Android SDK is properly installed and configured
- **Java version**: Verify JDK 11+ is installed and in PATH

#### Runtime Issues
- **App crashes on startup**: Check Android logs with `adb logcat`
- **Data not persisting**: Verify app has storage permissions
- **Navigation errors**: Ensure routes are properly registered

#### Development Tips
- Use **Hot Reload** for faster UI development
- Enable **Debug logging** for troubleshooting
- Test on **multiple screen sizes** and orientations

## Future Enhancements

### Planned Features
- [ ] **Cloud synchronization** (Azure, Firebase)
- [ ] **Categories/Tags** for better organization
- [ ] **Due dates** and notifications
- [ ] **Search functionality**
- [ ] **Data export/import**
- [ ] **Dark theme** support
- [ ] **Accessibility** improvements

### Technical Improvements
- [ ] **Unit tests** for ViewModels and Services
- [ ] **UI tests** with Appium
- [ ] **SQLite** database for better performance
- [ ] **Offline-first** architecture
- [ ] **Performance optimizations**

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License. See LICENSE file for details.

## Support

For issues and questions:
- Create an issue in the repository
- Check the troubleshooting section above
- Review Microsoft MAUI documentation

---

**Built with ❤️ using .NET MAUI**