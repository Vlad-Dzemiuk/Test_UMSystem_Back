# run_tests_cleanup.ps1

# Запуск PowerShell від імені адміністратора
if (-not ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Start-Process powershell.exe -Verb RunAs -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$($myInvocation.MyCommand.Path)`""
    exit
}

Write-Host "--- Розпочато очищення та запуск тестів ---" -ForegroundColor Yellow

# Шлях до кореневої папки вашого рішення
$SolutionPath = "C:\Users\Admin\3_course\Coursework\Backend"

# Переходимо до кореневої папки рішення
Set-Location $SolutionPath

Write-Host "`n1. Очищення папок 'bin' та 'obj'..." -ForegroundColor Cyan
try {
    Get-ChildItem -Path $SolutionPath -Include bin,obj -Recurse -ErrorAction SilentlyContinue | ForEach-Object {
        Remove-Item $_.FullName -Recurse -Force -ErrorAction SilentlyContinue
    }
    Remove-Item -Path "$SolutionPath\.vs" -Recurse -Force -ErrorAction SilentlyContinue
    Remove-Item -Path "$SolutionPath\.idea" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "   Папки 'bin', 'obj', '.vs' та '.idea' очищено." -ForegroundColor Green
} catch {
    Write-Host "   Помилка при очищенні папок: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n2. Очищення глобального кешу NuGet..." -ForegroundColor Cyan
try {
    dotnet nuget locals all --clear
    if ($LASTEXITCODE -ne 0) {
        Write-Host "   Помилка при очищенні кешу NuGet. Можливо, деякі файли заблоковані. Спробуйте перезавантажити ПК та запустити знову." -ForegroundColor Red
        exit 1 # Виходимо, бо це критична помилка
    }
    Write-Host "   Кеш NuGet успішно очищено." -ForegroundColor Green
} catch {
    Write-Host "   Критична помилка при очищенні кешу NuGet: $($_.Exception.Message)" -ForegroundColor Red
    exit 1 # Виходимо
}

Write-Host "`n3. Відновлення NuGet пакетів..." -ForegroundColor Cyan
try {
    dotnet restore
    if ($LASTEXITCODE -ne 0) {
        Write-Host "   Помилка при відновленні NuGet пакетів. Перевірте вивід вище." -ForegroundColor Red
        exit 1 # Виходимо
    }
    Write-Host "   NuGet пакети успішно відновлено." -ForegroundColor Green
} catch {
    Write-Host "   Критична помилка при відновленні NuGet пакетів: $($_.Exception.Message)" -ForegroundColor Red
    exit 1 # Виходимо
}

Write-Host "`n4. Збірка проекту..." -ForegroundColor Cyan
try {
    dotnet build
    if ($LASTEXITCODE -ne 0) {
        Write-Host "   Помилка при збірці проекту. Перевірте вивід вище." -ForegroundColor Red
        exit 1 # Виходимо
    }
    Write-Host "   Проект успішно зібрано." -ForegroundColor Green
} catch {
    Write-Host "   Критична помилка при збірці проекту: $($_.Exception.Message)" -ForegroundColor Red
    exit 1 # Виходимо
}

Write-Host "`n5. Перевірка наявності критичних файлів в папці тестів..." -ForegroundColor Cyan
$TestBinPath = Join-Path $SolutionPath "API.Tests\bin\Debug\net8.0"
$ApiDepsJson = Join-Path $TestBinPath "API.deps.json"
$ApiRuntimeConfig = Join-Path $TestBinPath "API.runtimeconfig.json"
$TestHostDepsJson = Join-Path $TestBinPath "testhost.deps.json"

$filesFound = $true
if (-not (Test-Path $ApiDepsJson)) {
    Write-Host "   Помилка: $ApiDepsJson не знайдено." -ForegroundColor Red
    $filesFound = $false
} else {
    Write-Host "   $ApiDepsJson знайдено." -ForegroundColor Green
}

if (-not (Test-Path $ApiRuntimeConfig)) {
    Write-Host "   Помилка: $ApiRuntimeConfig не знайдено." -ForegroundColor Red
    $filesFound = $false
} else {
    Write-Host "   $ApiRuntimeConfig знайдено." -ForegroundColor Green
}

if (-not (Test-Path $TestHostDepsJson)) {
    Write-Host "   Попередження: $TestHostDepsJson не знайдено. Це може бути нормально, оскільки він генерується пізніше, але зверніть увагу, якщо тести все ще не працюють." -ForegroundColor Yellow
} else {
    Write-Host "   $TestHostDepsJson знайдено." -ForegroundColor Green
}

if (-not $filesFound) {
    Write-Host "   Деякі критичні файли відсутні після збірки. Перевірте ваші .csproj файли, особливо налаштування CopyBuildOutputToOutputDirectory та PreserveCompilationContext." -ForegroundColor Red
    exit 1 # Виходимо
}

Write-Host "`n6. Перевірка xunit.runner.json для вимкнення shadowCopy..." -ForegroundColor Cyan
$XunitRunnerPath = Join-Path $SolutionPath "API.Tests\xunit.runner.json"
if (-not (Test-Path $XunitRunnerPath)) {
    Write-Host "   Попередження: Файл '$XunitRunnerPath' не знайдено. Будь ласка, створіть його з вмістом: `{ ""shadowCopy"": false }`" -ForegroundColor Yellow
    try {
        Set-Content -Path $XunitRunnerPath -Value '{ "shadowCopy": false }'
        Write-Host "   Файл '$XunitRunnerPath' створено з вимкненим shadowCopy." -ForegroundColor Green
    } catch {
        Write-Host "   Помилка при створенні '$XunitRunnerPath': $($_.Exception.Message)" -ForegroundColor Red
    }
} else {
    try {
        $xunitContent = Get-Content $XunitRunnerPath | Out-String | ConvertFrom-Json -ErrorAction Stop
        if ($xunitContent.shadowCopy -eq $true) {
            Write-Host "   Попередження: 'shadowCopy' у '$XunitRunnerPath' встановлено на 'true'. Це може викликати проблеми. Встановіть на 'false'." -ForegroundColor Yellow
        } elseif ($xunitContent.shadowCopy -eq $false) {
            Write-Host "   'shadowCopy' у '$XunitRunnerPath' встановлено на 'false'. Це добре." -ForegroundColor Green
        } else {
            Write-Host "   Не вдалося визначити стан 'shadowCopy' у '$XunitRunnerPath'. Переконайтеся, що це валідний JSON з `""shadowCopy"": false`." -ForegroundColor Yellow
        }
    catch {
        Write-Host "   Помилка при читанні/парсингу '$XunitRunnerPath'. Переконайтеся, що це валідний JSON: $($_.Exception.Message)" -ForegroundColor Red
    }

Write-Host "`n7. Запуск тестів..." -ForegroundColor Cyan
try {
    dotnet test
    if ($LASTEXITCODE -ne 0) {
        Write-Host "   Тести завершилися з помилками. Перевірте вивід вище." -ForegroundColor Red
    } else {
        Write-Host "   Тести успішно пройшли!" -ForegroundColor Green
    }
} catch {
    Write-Host "   Критична помилка при запуску тестів: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n--- Завершено ---" -ForegroundColor Yellow