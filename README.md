Це простий REST API, призначений для проведення A/B тестування для перевірки гіпотез. Він складається з двох ендпоінтів для керування експериментами та надання даних експерименту клієнтам.
Використані технології:C#,ASP.NET Core,Entity Framework Core,MS SQL Server,NUnit,Moq,AutoFixture.
Ендпоінти API:
1. Отримати експеримент
URL: /api/experiment/{experimentName}
Метод: GET
Опис: Отримує поточне значення експерименту для заданої назви експерименту.
Автор:Вадим Корнелюк
