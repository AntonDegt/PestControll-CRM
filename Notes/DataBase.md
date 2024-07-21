# База данних


## **Теоретичне проектування бази даних**
<details>
<summary>Необхідна функціональність</summary>


1. Список контактів.

2. Список клієнтів:
	- Приватні особи
	- Підприємства

3. Список клієнтів на постійному відвідуванні:
	- Об'єкт
	- Кількість відвідувань на місяць
	- Дати минулих відвідувань
	- Нотатки

4. Список дзвінків:
	- Контакт
	- Результат розмови

5. Список Запланованих дзвінків

6. Список бригад
	- бригади
 	- працівники бригад
  	- куратор бригади

8. План на місяць (місяці)
	- Планових відвідуваннь
	- Обробок
</details>

## **Схема**
<details>
<summary>Скріншоти</summary>

![image](https://github.com/user-attachments/assets/62d95ef9-8c94-43cf-9493-229a36c8cb91)
![image](https://github.com/user-attachments/assets/9642b1b2-b088-47e8-ae20-0868a559a7c0)
![image](https://github.com/user-attachments/assets/32736896-2749-4944-8ca0-da1cafabe8b3)

</details>

## **Опис**
<details>
<summary>Застосовані таблиці</summary>

### **Contacts**
_Контаки - всі люди з якими буде працювати программа
	- Клієнти
 	- Працівники юридичних осіб
 	- Працівники
  	- Куратори_

### **PhoneNumbers**
_Номера телефонів клієнтів
(окрема таблиця на випадок декількох номерів у однієї особи)_

### **Calls**
_Дзвінки які здійснюються та повинні бути записані до программи_

### **CallResultTypes**
_Типи результатів дзвінків_

### **CallTypes**
_Типи дзвінків (вхідні та вихідні)_

### **PlannedCalls**
_Заплановані або перенесені дзвінки_

### **NaturalPerson**
_Фізична особо - замовник_

### **LegalPerson**
_Юридична особа замовник_

### **TaxSystem**
_Тип опадткування
	- Загальна система
	- Єдиний податок ІІ група
	- Єдиний податок ІІІ група_

### **Position**
_Посада особи (контакту) у Юридичної особи_

### **PlanedWork**
_Планові роботи та кількість відвідувань на місяць_

### **PlanetVisit**
_Плановий візит_

### **Brigade**
_Бригада_

### **Workes**
_Співробітники (куратори та оператори)_

</details>


## **Створення Бази Данних**
<details>
<summary>Запити</summary>

### **Створення БД**
```
CREATE DATABASE PCCRM;
```

### **Контакти**
```
CREATE TABLE Contacts (
    id int NOT NULL AUTO_INCREMENT,
    PIB varchar(100) NOT NULL,
    email varchar(255) NULL,
    notes text NULL,
    PRIMARY KEY (id)
);
```

### **Номера телефону**
```
CREATE TABLE PhoneNumbers (
    phone_number varchar(15) NOT NULL,
    contact_id INT NOT NULL,
    PRIMARY KEY (phone_number)
);
```

### **Фізична особа**
```
CREATE TABLE NaturalPerson (
    contact_id int NOT NULL AUTO_INCREMENT,
    IPN varchar(30) NOT NULL,
    address varchar(100),
    PRIMARY KEY (contact_id)
);
```

### **Юридична особа**
```
CREATE TABLE LegalPerson (
    id int NOT NULL AUTO_INCREMENT,
    name varchar(50) NOT NULL,
    EDRPOU varchar(20) NOT NULL,
    address varchar(100) NOT NULL,
    current_account varchar(20),
    email varchar(20),
    tax_system_id INT,
    PRIMARY KEY (id)
);
```

### **Співробітники юридичних осіб**
```
CREATE TABLE Position (
    contact_id int NOT NULL,
    legal_person_id INT NOT NULL,
    priority_contact BOOLEAN NOT NULL,
    position varchar(30) NOT NULL,
    PRIMARY KEY (contact_id)
);
```

### **Виклики**
```
CREATE TABLE Calls (
    id int NOT NULL AUTO_INCREMENT,
    call_type int NOT NULL,
    contact_id int NOT NULL,
    date_time DATETIME NOT NULL,
    result_type_id INT NOT NULL,
    comment TEXT NOT NULL,
    PRIMARY KEY (id)
);
```

### **Типи викликів**
```
CREATE TABLE CallTypes (
    id int NOT NULL AUTO_INCREMENT,
    name varchar(15) NOT NULL,
    PRIMARY KEY (id)
);
```

### **Типи результатів викликів**
```
CREATE TABLE CallResultTypes (
    id int NOT NULL AUTO_INCREMENT,
    resunt varchar(15) NOT NULL,
    PRIMARY KEY (id)
);
```

### **Планові дзвінки**
```
CREATE TABLE PlannedCalls (
    id int NOT NULL AUTO_INCREMENT,
    contact_id INT NOT NULL,
    date DATE NOT NULL,
    time TIME NULL,
    goal text NULL,
    PRIMARY KEY (id)
);
```

### **Типи опадаткування**
```
CREATE TABLE TaxSystem (
    id int NOT NULL AUTO_INCREMENT,
    type varchar(20),
    PRIMARY KEY (id)
);
```

### **Планові роботи**
```
CREATE TABLE PlannedWork (
    id int NOT NULL AUTO_INCREMENT,
    legal_person BOOLEAN NOT NULL,
    person_id int NOT NULL,
    quantity_per_month INT NOT NULL,
    brigade_id INT NULL,
    notes text NULL,
    PRIMARY KEY (id)
);
```

### **Планові візити**
```
CREATE TABLE PlannedVisit (
    id int NOT NULL AUTO_INCREMENT,
    planned_work_id INT NOT NULL,
    date DATE NULL,
    notes text NULL,
    PRIMARY KEY (id)
);
```

### **Бригада**
```
CREATE TABLE Brigade (
    id int NOT NULL AUTO_INCREMENT,
    name varchar(20) NOT NULL,
    curator_contact_id INT NOT NULL,
    PRIMARY KEY (id)
);
```

### **Співробітники**
```
CREATE TABLE Workers (
    contact_id int NOT NULL,
    brigade_id INT NOT NULL,
    PRIMARY KEY (contact_id)
);
```

</details>


## **Тестове заповнення БД**
<details>
<summary>Запити</summary>

### **Контакти**
```
INSERT INTO Contacts (PIB, email)
VALUES 
('Іван Іваненко', 'ivan.ivanenko@example.com'),
('Петро Петренко', 'petro.petrenko@example.com'),
('Олена Шевченко', 'olena.shevchenko@example.com'),
('Марія Горобець', 'mariya.horobets@example.com'),
('Оксана Бойко', 'oksana.boyko@example.com'),
('Андрій Лисенко', 'andriy.lysenko@example.com'),
('Сергій Коваленко', 'serhiy.kovalenko@example.com'),
('Надія Бондар', 'nadia.bondar@example.com'),
('Юлія Кравець', 'yulia.kravets@example.com'),
('Володимир Мороз', 'volodymyr.moroz@example.com'),
('Наталія Гриценко', 'natalia.hrytsenko@example.com'),
('Олександр Тимошенко', 'oleksandr.tymoshenko@example.com'),
('Віктор Мельник', 'viktor.melnyk@example.com'),
('Тетяна Ковальчук', 'tetiana.kovalchuk@example.com'),
('Ірина Федорук', 'iryna.fedoruk@example.com'),
('Микола Сидоренко', 'mykola.sydorenko@example.com'),
('Галина Савченко', 'halyna.savchenko@example.com'),
('Олег Павленко', 'oleh.pavlenko@example.com'),
('Дмитро Заєць', 'dmytro.zayats@example.com'),
('Катерина Довженко', 'kateryna.dovzhenko@example.com');
```

### **Номера телефонів**
```
INSERT INTO phonenumbers (phone_number, contact_id) 
VALUES 
('+380501234567', 1), 
('+380671234567', 2), 
('+380931234567', 2), 
('+380631234567', 3), 
('+380991234567', 4), 
('+380681234567', 5), 
('+380501234568', 6), 
('+380671234568', 7), 
('+380931234568', 8), 
('+380631234568', 9), 
('+380991234568', 10), 
('+380681234568', 11), 
('+380501234569', 12), 
('+380671234569', 12), 
('+380931234569', 13), 
('+380631234569', 14), 
('+380991234569', 15), 
('+380681234569', 16), 
('+380501234570', 17), 
('+380671234570', 18), 
('+380931234570', 18), 
('+380631234570', 18), 
('+380991234570', 19), 
('+380681234570', 20), 
('+380501234571', 20);
```


</details>
