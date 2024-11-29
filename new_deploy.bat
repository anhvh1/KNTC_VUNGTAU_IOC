@echo off

set "serverIP=192.168.100.42"
set "username=administrator"
set "password=Admin@123"
set "siteName=kntcv2dev.gosol.com.vn"

:: Tạo đối tượng Credential từ thông tin đăng nhập
echo. | set /p dummyName="password=%password%" > "%temp%\%~n0.txt"
powershell -Command "$password = Get-Content '%temp%\%~n0.txt' | ConvertTo-SecureString -AsPlainText -Force; $credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList '%username%', $password"

:: Dừng dịch vụ IIS trên máy chủ từ xa
powershell -Command "Invoke-Command -ComputerName %serverIP% -Credential $credential -ScriptBlock { Import-Module WebAdministration; Stop-WebSite -Name %siteName% }"

:: Xóa tệp chứa mật khẩu
del "%temp%\%~n0.txt"