## Python

1. Модули:
    1. Объявление сущностей в модулях
       ```csharp 
       Interpreter.Assign(ModuleDescriptor, <name>, <entity-descriptor>);
       ```
    2. Ссылка на сущности из разных модулей

2. Классы:
    1. Описание класса с именем и конструктором (+с учётом модуля)
       ```csharp
       var baseRequestClass = PythonTypes.CreateClass(<name>)
           .WithMethod("__init__", <init-handler>)
       Interpreter.Assign(ModuleDescriptor, <name>, baseRequestClass.ClassDescriptor);
       ```
    2. Указание обработчиков методов (при объявлении класса)
    3. Указание значений полей объекта (в конструкторе):
       ```csharp
       PythonTypes.SetAttribute(location, <instance>, <name>, <value>);
       ```
3. Методы/функции:
    1. В классе
    2. В модуле:
       ```csharp
       CreateModuleFunction(<name>, <hanlder>)
       ```
    3. Есть аргументы:
        1. Можно получить их по индексу:
           ```csharp
           functionCall.Arguments[<idx>]
           ```
           Или по имени (приоритетный вариант):
           ```csharp
           ProcessorApi.ProcessKeywordArguments(functionCall)
           ```
           По первому индексу self (если метод в классе)
        2. Перегрузки функций по аргументам (не актуально для python) 
    4. Есть стейтменты
    5. Есть return (стейтмент)
       Возвращается или `CallHandlerResult.Unprocessed`, или `CallHandlerResult.Processed(<res>)`,
       где `res` — возвращаемое выражение (возможно, `SemanticsApi.None`)
4. Стейтменты/выражения:
   1. Присваивание значения полю this/другого объекта
   2. Вызов external функции (возможно, интринсика) чтобы спрятать там вызов колбэков и прочую магию
   3. Чтение значения из поля объекта (возможно, интринсика)
   4. Ассоциативные массивы с чтением значений из полей 
5. Интринсики:
   1. `Detector.Detect(<location>, <value>, <vulnerability-type>, <argument-grammar>)`
   2. Создание инстанса объекта (`new`-expression)
      ```csharp
      <class>.CreateInstance(<location>)
      ```
   3. Вызов символьной функции
6. Типы
   1. `any` — любой тип значения
   2. `int`, `float`, `string`, `bool`
   3. `Callable` — функциональный символьный тип
