@echo off
echo ========================================
echo  سیستم مدیریت فروش و حسابداری
echo  انتشار خودکار
echo ========================================
echo.

echo [1/3] در حال ساخت نسخه اجرایی...
dotnet publish "PosAccountingApp.csproj" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o "publish"

if %ERRORLEVEL% NEQ 0 (
    echo خطا در ساخت!
    pause
    exit /b 1
)

echo [2/3] نسخه اجرایی ساخته شد در پوشه publish
echo [3/3] برای ساخت نصب‌کننده، فایل installer.iss را با Inno Setup باز کنید.
echo.
echo فایل اجرایی: publish\PosAccountingApp.exe
echo.
pause
