1. Database Migration
`dotnet ef migrations add namaofmigration --startup-project ..\..\Persentation\Trisatech.KampDigi.WebApp\Trisatech.KampDigi.WebApp.csproj`
- Bukan commandline/powershell/terminal dan masuk ke project yang ada DbContext-nya Trisatech.KampDigi.Domain
- Jalankan command ini `dotnet ef database update --startup-project ..\..\Persentation\Trisatech.KampDigi.WebApp\Trisatech.KampDigi.WebApp.csproj`
