Техническое задание:

 Web-Сервис сотрудников, сделанный на платформе .Net Core.
 Сервис должен уметь:
 1. Добавлять сотрудников, в ответ должен приходить Id добавленного сотрудника.
 2. Удалять сотрудников по Id.
 3. Выводить список сотрудников для указанной компании. Все доступные поля.
 4. Выводить список сотрудников для указанного отдела компании. Все доступные
 поля.
 5. Изменять сотрудника по его Id. Изменения должно быть только тех полей,
 которые указаны в запросе.
 Модель сотрудника:

```csharp
{
   Id int
   Name string
   Surname string
   Phone string
   CompanyId int
   Passport 
   {
     Type string
     Number string
   }
   Department
   {
     Name string
     Phone string
   }
}
```

 Все методы должны быть реализованы в виде HTTP запросов в формате JSON.
 БД: любая.
 ORM: Dapper.

Для запуска приложения достаточно выполнить `docker-compose up -d` в файле docker-compose.yaml указана строка подключения к бд, в самом приложении её нет. 
После чего перейти на `http://localhost:8080/swagger`

По умолчанию база данных пустая, вот немного запросов для сидирования

Companies
```sql
insert into public.companies (name, phone)
values ('goydochka','+79997776655');

insert into public.companies (name, phone)
values ('somecompany','+77777777777');
```
Departments
```sql
insert into public.departments (name, phone, company_id)
values ('Hr','+79023456789',1);

insert into public.departments (name, phone, company_id)
values ('Developers','+79168723415',1);

insert into public.departments (name, phone, company_id)
values ('Sales','+79251095678',2);

insert into public.departments (name, phone, company_id)
values ('Support','+79034781290',2);

insert into public.departments (name, phone, company_id)
values ('It','+79776543210',2);
```
Employees
```sql
INSERT INTO public.employees (name, phone, company_id, department_id, passport) VALUES
('Лариса Жукова', '+79746063459', 2, 3, '{"type": "TEMP", "number": "4039884935"}'::jsonb),
('Наина Наумов', '+79426368850', 1, 1, '{"type": "RF", "number": "1918044544"}'::jsonb),
('Евдоким Григорьев', '+79846561131', 1, 2, '{"type": "INT", "number": "6457985906"}'::jsonb),
('Федор Степанов', '+79217187883', 2, 4, '{"type": "RF", "number": "3987822919"}'::jsonb),
('Вениамин Калашникова', '+79787860388', 1, 2, '{"type": "INT", "number": "9677003277"}'::jsonb),
('Симон Кудряшова', '+79812935955', 2, 5, '{"type": "INT", "number": "9071913626"}'::jsonb),
('Исай Ефимова', '+79075245087', 2, 4, '{"type": "INT", "number": "7732927741"}'::jsonb),
('Мир Чернова', '+79701984751', 1, 1, '{"type": "RF", "number": "1703672147"}'::jsonb),
('Аверкий Игнатов', '+79190877601', 1, 1, '{"type": "TEMP", "number": "3475937409"}'::jsonb),
('Аскольд Нестерова', '+79646082593', 2, 4, '{"type": "RF", "number": "9013361639"}'::jsonb)
```
