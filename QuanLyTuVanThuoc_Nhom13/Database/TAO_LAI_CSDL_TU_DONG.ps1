$ErrorActionPreference = 'Stop'
$Host.UI.RawUI.WindowTitle = 'Tao CSDL QuanLyTuVanThuocDB - Nhom 13'

$serverName = '(localdb)\MSSQLLocalDB'
$databaseName = 'QuanLyTuVanThuocDB'
$sqlFile = Join-Path $PSScriptRoot 'Tao_CSDL_QuanLyTuVanThuocDB.sql'

Write-Host '============================================================' -ForegroundColor Cyan
Write-Host '  TAO LAI CSDL QUAN LY TU VAN THUOC - NHOM 13' -ForegroundColor Cyan
Write-Host '============================================================' -ForegroundColor Cyan
Write-Host "Server  : $serverName"
Write-Host "Database: $databaseName"
Write-Host "Script  : $sqlFile"
Write-Host ''
Write-Host 'CANH BAO: thao tac nay se xoa cac bang va du lieu cu trong database.' -ForegroundColor Yellow
$answer = Read-Host 'Nhap YES de tiep tuc'
if ($answer -ne 'YES') {
    Write-Host 'Da huy thao tac.' -ForegroundColor Yellow
    exit 0
}

if (-not (Test-Path $sqlFile)) {
    throw "Khong tim thay file SQL: $sqlFile"
}

# Khởi động LocalDB nếu công cụ có sẵn.
$localDb = Get-Command SqlLocalDB.exe -ErrorAction SilentlyContinue
if ($localDb) {
    & $localDb.Path start MSSQLLocalDB | Out-Null
}

Add-Type -AssemblyName System.Data
$connectionString = "Server=$serverName;Database=master;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
$connection = New-Object System.Data.SqlClient.SqlConnection $connectionString

try {
    $connection.Open()
    $script = Get-Content -LiteralPath $sqlFile -Raw -Encoding UTF8
    $batches = [System.Text.RegularExpressions.Regex]::Split(
        $script,
        '(?im)^\s*GO\s*(?:--.*)?$'
    )

    $batchNumber = 0
    foreach ($batch in $batches) {
        if ([string]::IsNullOrWhiteSpace($batch)) { continue }
        $batchNumber++
        $command = $connection.CreateCommand()
        $command.CommandText = $batch
        $command.CommandTimeout = 180
        [void]$command.ExecuteNonQuery()
        $command.Dispose()
        Write-Host "Da chay batch $batchNumber" -ForegroundColor DarkGray
    }

    Write-Host ''
    Write-Host 'TAO CSDL THANH CONG.' -ForegroundColor Green
    Write-Host 'Database: QuanLyTuVanThuocDB' -ForegroundColor Green
    Write-Host 'Tai khoan demo: admin / 123456' -ForegroundColor Green
}
catch {
    Write-Host ''
    Write-Host 'TAO CSDL THAT BAI:' -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host ''
    Write-Host 'Hay kiem tra SQL Server LocalDB da duoc cai va server name la:' -ForegroundColor Yellow
    Write-Host '(localdb)\MSSQLLocalDB' -ForegroundColor Yellow
    exit 1
}
finally {
    if ($connection.State -ne [System.Data.ConnectionState]::Closed) {
        $connection.Close()
    }
    $connection.Dispose()
}
