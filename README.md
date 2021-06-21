<p align="center">
<img height="300" src="https://user-images.githubusercontent.com/43035417/122687424-17337700-d21f-11eb-9d6b-26000d720291.jpg">
</p>

# Başlarken

devnot-mentor-back-end projesini çalıştırabilmek için bilgisayarınıza [.NET Core](https://dotnet.microsoft.com/download) yüklemelisiniz.

Projeyi klonlamak için.

```sh
$ git clone https://github.com/devnotcom/devnot-mentor-back-end.git
```

Development, Test ya da Production ortamında çalıştırmak için.

```sh
$  dotnet run --launch-profile Development
$  dotnet run --launch-profile Test
$  dotnet run --launch-profile Production
```

Publish alma işlemleri için.

```sh
$ dotnet publish
```

publish alma işleminden sonra uygun profili seçmek için ENV. değerinin karşılığı `Developemen`, `Test` ya da `Production` olmalıdır.

ENV. Key: DEVNOT_MENTOR_ENVIRONMENT

Örnek bir ENV. ataması: ***DEVNOT_MENTOR_ENVIRONMENT = production***
