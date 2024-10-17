Нужны ноды:

1. statement `using System.Collections.Immutable;`
2. statement объявление переменной с init значением и без
3. statement объявление переменной с указанием типа и без
4. statement для if-else
5. statement для return
6. foreach
7. for
4. Вызов метода с expression-ресивером:
   ```
   var appStackClass = PythonTypes.CreateClass("<bottle.AppStack>", SemanticsApi.ListType.ToExpression())
    .WithMethod("__call__", ProcessAppStackCall) // `v = AppStack()` is for `__init__`, `v2 = v() = AppStack()()` is for `__call__`
    .WithMethod("peek", ProcessAppStackPeek) // for convenience
    .WithMethod("push", ProcessAppStackPush);
   ```
5. expression для литералов (инты, строки)
6. expression для переменных
8. expression для индексации массивов
9. expression для new
10. expression для бинарных операций (+, -, /, * и тд; ==, !=, <, >, <=, >=; ||, &&) и унарных?
