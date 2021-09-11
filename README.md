# Devnot Mentor  

## Devnot Nedir?

Güncel yazılım konularıyla ilgili yazılar yayınlayan, yazılım konferansları, buluşmalar ve kamplar düzenleyen yazılım geliştirici topluluğudur.  

## Devnot Mentor Projesi Nedir?

Yazılım alanında kendini geliştirmek isteyen öğrencilerin(mentilerin) kendilerinden daha tecrübeli yazılımcılar(mentorlar) bulmalarını, onlarla tanışmalarını ve düzenli görüşmeler yapmalarını sağlayacak açık kaynak kod bir topluluk projesidir.

Yazılımcı olmayı veya mesleğinde daha iyi noktalara gelmeyi hedefleyen kişileri(öğrenci) bu program kapsamında bir mentorle eşleştirerek düzenli görüşmeler yapmalarını amaçlamaktayız. Bu görüşmelerde öğrenciler mentorlerine sorular sorabilecek, çözmekte zorlandıkları konularda yardım isteyebilecek, gelişimleri için uygun kaynak önerilerini dinleyebilecekler.

## Başlarken

devnot-mentor-back-end projesini çalıştırabilmek için bilgisayarınızda [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0) yüklü olmalıdır.

Projeyi klonlamak için.

```sh
git clone https://github.com/devnotcom/devnot-mentor-back-end.git
```

Development, Test ya da Production ortamında çalıştırmak için.

```sh
dotnet run --launch-profile Development
dotnet run --launch-profile Test
dotnet run --launch-profile Production
```

## Database

* Migration veya Update için environment ataması yapılmalıdır.  
Environment Key: DEVNOT_MENTOR_ENVIRONMENT  
Powershell:  
`env:DEVNOT_MENTOR_ENVIRONMENT='Development'`  
Linux Terminal:  
`export DEVNOT_MENTOR_ENVIRONMENT='Development'`

* Update için [DevnotMentor.Configurations](./src/DevnotMentor.Configurations/) altında olan [appsettings.development.json](./src/DevnotMentor.Configurations/appsettings.development.json) dosyasında veya diğer environment dosyalarında bulunan SQL Server bağlantı dizisi düzeltilmelidir.

### Database Migration

[DbContext](./src/DevnotMentor.Data/MentorDBContext.cs)'ın OnModelCreating metotunda bulunan ModelBuilder üzerinde bir değişiklik yapmadıysanız veya [Entities](./src/DevnotMentor.Data/Entities) klasöründeki tablo varlıklarını değiştirmediyseniz yeni bir migration oluşturmanıza gerek yok. Var olan migrationlar ile database'ı oluşturabilir veya güncelleyebilirsiniz.

Package Manager Console

```sh
add-migration 'MentorDB_Initialization'
```

veya Terminal

```sh
cd src/DevnotMentor.Data 
dotnet ef --startup-project ../DevnotMentor.WebAPI/ migrations add 'MentorDB_Initialization'
```

### Database Update

Package Manager Console

```sh
Update-Database
```

veya Terminal

```sh
cd src/DevnotMentor.Data 
dotnet ef --startup-project ../DevnotMentor.WebAPI/ database update
```
