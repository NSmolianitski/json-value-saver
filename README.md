# Задача 1

Для простоты запуска используется SQLite.

## Запуск
Скачать репозиторий и из корня выполнить в терминале команду:
```sh
dotnet run
```

Адрес клиента
```
http://localhost:5268
```

# Задача 2

## Запрос 1
```sql
   SELECT c.ClientName,
          COUNT(cc.Id) AS ContactCount
     FROM Clients c
LEFT JOIN ClientContacts cc 
       ON c.Id = cc.ClientId
 GROUP BY c.Id
 ORDER BY c.ClientName
```

## Запрос 2

```sql
   SELECT c.*
    FROM Clients c
    JOIN ClientContacts cc ON c.Id = cc.ClientId
GROUP BY c.Id
  HAVING COUNT(cc.Id) > 2
```

# Задача 3
```sql
WITH DateIntervals AS (SELECT Id,
                              Dt,
                              LAG(Dt) OVER (PARTITION BY Id 
                                                ORDER BY Dt) AS PreviousDate
                       FROM Dates)

SELECT Id,
       PreviousDate AS Sd,
       Dt AS Ed
FROM DateIntervals
WHERE PreviousDate IS NOT NULL
GROUP BY Id, Dt, PreviousDate
ORDER BY Id, Sd;
```