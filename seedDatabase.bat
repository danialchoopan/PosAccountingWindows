@echo off
echo ================================================
echo   دانلود و اجرای دیتابیس نمونه
echo ================================================
echo.

echo [1/3] در حال ساخت نسخه اجرایی...
dotnet publish "PosAccountingApp.csproj" -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o "publish" -f net9.0-windows

if %ERRORLEVEL% NEQ 0 (
    echo خطا در ساخت!
    pause
    exit /b 1
)

echo [2/3] نسخه اجرایی ساخته شد
echo [3/3] در حال اجرای برنامه باید خودکار نمونه داده تولید شود...
echo.
echo برنامه اجرا شد. دیتابیس نمونه ایجاد می‌شود.
echo.
pause
