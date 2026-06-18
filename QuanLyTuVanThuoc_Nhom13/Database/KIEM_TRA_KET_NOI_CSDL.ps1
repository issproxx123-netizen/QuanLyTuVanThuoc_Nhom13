$ErrorActionPreference = 'Stop'
$serverName = '(localdb)\MSSQLLocalDB'
$databaseName = 'QuanLyTuVanThuocDB'

Add-Type -AssemblyName System.Data
$connectionString = "Server=$serverName;Database=$databaseName;Integrated Security=True;TrustServerCertificate=True"
$connection = New-Object System.Data.SqlClient.SqlConnection $connectionString

try {
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = @"
SELECT
    DB_NAME() AS DatabaseName,
    (SELECT COUNT(*) FROM dbo.VaiTro) AS SoVaiTro,
    (SELECT COUNT(*) FROM dbo.NguoiDung) AS SoNguoiDung,
    (SELECT COUNT(*) FROM dbo.BenhNhan) AS SoBenhNhan,
    (SELECT COUNT(*) FROM dbo.Thuoc) AS SoThuoc;
"@
    $reader = $command.ExecuteReader()
    if ($reader.Read()) {
        Write-Host 'KET NOI CSDL THANH CONG' -ForegroundColor Green
        Write-Host "Database   : $($reader['DatabaseName'])"
        Write-Host "Vai tro    : $($reader['SoVaiTro'])"
        Write-Host "Nguoi dung : $($reader['SoNguoiDung'])"
        Write-Host "Benh nhan  : $($reader['SoBenhNhan'])"
        Write-Host "Thuoc      : $($reader['SoThuoc'])"
    }
    $reader.Close()
}
catch {
    Write-Host 'KHONG THE KET NOI CSDL' -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
}
finally {
    if ($connection.State -ne [System.Data.ConnectionState]::Closed) { $connection.Close() }
    $connection.Dispose()
}
