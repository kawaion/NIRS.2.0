using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

public static class DeepCopyer
{
    private static readonly HashSet<Type> PrimitiveTypes = new HashSet<Type>
    {
        typeof(string), typeof(decimal), typeof(DateTime), typeof(DateTimeOffset),
        typeof(TimeSpan), typeof(Guid), typeof(Uri), typeof(Enum)
    };

    /// <summary>
    /// Универсальный метод глубокого копирования
    /// </summary>
    public static T DeepCopy<T>(T obj)
    {
        if (obj == null) return default(T);

        var type = obj.GetType();

        // Для примитивных типов и строк
        if (type.IsValueType || PrimitiveTypes.Contains(type))
            return obj;

        // Для массивов
        if (type.IsArray)
            return (T)CopyArray((Array)(object)obj);

        // Для списков и коллекций
        if (obj is IEnumerable enumerable && !(obj is string))
            return (T)CopyCollection(enumerable, type);

        // Для обычных объектов
        return (T)CopyObject(obj, type, new Dictionary<object, object>());
    }

    private static object CopyArray(Array array)
    {
        var elementType = array.GetType().GetElementType();
        var copiedArray = Array.CreateInstance(elementType, array.Length);

        for (int i = 0; i < array.Length; i++)
        {
            var value = array.GetValue(i);
            if (value != null)
            {
                var copiedValue = DeepCopy(value);
                copiedArray.SetValue(copiedValue, i);
            }
        }

        return copiedArray;
    }

    private static object CopyCollection(IEnumerable collection, Type collectionType)
    {
        // Создаем новую коллекцию того же типа
        object newCollection;

        try
        {
            newCollection = Activator.CreateInstance(collectionType);
        }
        catch
        {
            // Если не удается создать коллекцию, используем сериализацию
            return CopyViaSerialization(collection);
        }

        // Для List<T>
        if (collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(List<>))
        {
            var addMethod = collectionType.GetMethod("Add");
            foreach (var item in collection)
            {
                var copiedItem = DeepCopy(item);
                addMethod.Invoke(newCollection, new[] { copiedItem });
            }
        }
        // Для Dictionary<TKey, TValue>
        else if (collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            var addMethod = collectionType.GetMethod("Add");
            foreach (var entry in collection)
            {
                // Для Dictionary используем KeyValuePair
                var keyProperty = entry.GetType().GetProperty("Key");
                var valueProperty = entry.GetType().GetProperty("Value");

                if (keyProperty != null && valueProperty != null)
                {
                    var key = keyProperty.GetValue(entry);
                    var value = valueProperty.GetValue(entry);

                    var copiedKey = DeepCopy(key);
                    var copiedValue = DeepCopy(value);
                    addMethod.Invoke(newCollection, new[] { copiedKey, copiedValue });
                }
            }
        }
        // Для HashSet<T>
        else if (collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(HashSet<>))
        {
            var addMethod = collectionType.GetMethod("Add");
            foreach (var item in collection)
            {
                var copiedItem = DeepCopy(item);
                addMethod.Invoke(newCollection, new[] { copiedItem });
            }
        }
        // Для Queue<T>
        else if (collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(Queue<>))
        {
            var enqueueMethod = collectionType.GetMethod("Enqueue");
            foreach (var item in collection)
            {
                var copiedItem = DeepCopy(item);
                enqueueMethod.Invoke(newCollection, new[] { copiedItem });
            }
        }
        // Для Stack<T>
        else if (collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(Stack<>))
        {
            var pushMethod = collectionType.GetMethod("Push");
            foreach (var item in collection)
            {
                var copiedItem = DeepCopy(item);
                pushMethod.Invoke(newCollection, new[] { copiedItem });
            }
        }
        // Для не-generic коллекций
        else if (collection is IList nonGenericList)
        {
            var addMethod = collectionType.GetMethod("Add");
            foreach (var item in nonGenericList)
            {
                var copiedItem = DeepCopy(item);
                addMethod.Invoke(newCollection, new[] { copiedItem });
            }
        }
        else
        {
            // Используем сериализацию как запасной вариант
            return CopyViaSerialization(collection);
        }

        return newCollection;
    }

    /// <summary>
    /// Копирование через сериализацию (универсальный метод для сложных коллекций)
    /// </summary>
    private static object CopyViaSerialization(object obj)
    {
        if (obj == null) return null;

        try
        {
            // Пробуем бинарную сериализацию (требует [Serializable])
            if (obj.GetType().IsSerializable)
            {
                return CopyViaBinarySerialization(obj);
            }
            else
            {
                // Пробуем XML сериализацию
                return CopyViaXmlSerialization(obj);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сериализации для типа {obj.GetType().Name}: {ex.Message}");
            throw new InvalidOperationException($"Не удалось скопировать объект типа {obj.GetType().Name} через сериализацию", ex);
        }
    }

    /// <summary>
    /// Копирование через бинарную сериализацию
    /// </summary>
    private static object CopyViaBinarySerialization(object obj)
    {
        if (!obj.GetType().IsSerializable)
        {
            throw new ArgumentException($"Тип {obj.GetType().Name} не поддерживает бинарную сериализацию. Добавьте атрибут [Serializable]");
        }

        using (var stream = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(stream);
        }
    }

    /// <summary>
    /// Копирование через XML сериализацию
    /// </summary>
    private static object CopyViaXmlSerialization(object obj)
    {
        var type = obj.GetType();

        // XML сериализация требует конструктор без параметров
        if (type.GetConstructor(Type.EmptyTypes) == null)
        {
            throw new InvalidOperationException($"Тип {type.Name} не имеет конструктора без параметров для XML сериализации");
        }

        using (var stream = new MemoryStream())
        {
            var serializer = new XmlSerializer(type);
            serializer.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            return serializer.Deserialize(stream);
        }
    }

    private static object CopyObject(object obj, Type type, Dictionary<object, object> visited)
    {
        // Проверяем циклические ссылки
        if (visited.ContainsKey(obj))
            return visited[obj];

        // Создаем новый экземпляр
        object copiedObject;
        try
        {
            copiedObject = FormatterServices.GetUninitializedObject(type);
        }
        catch
        {
            // Если не получается создать через FormatterServices, пробуем конструктор
            copiedObject = CreateInstanceWithConstructor(type);
        }

        visited[obj] = copiedObject;

        // Копируем поля
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (field.IsInitOnly) continue; // Пропускаем readonly поля

            try
            {
                var fieldValue = field.GetValue(obj);
                if (fieldValue != null)
                {
                    var copiedValue = DeepCopy(fieldValue);
                    field.SetValue(copiedObject, copiedValue);
                }
                else
                {
                    field.SetValue(copiedObject, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка копирования поля {field.Name}: {ex.Message}");
            }
        }

        // Копируем свойства
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var prop in properties)
        {
            if (!prop.CanWrite || !prop.CanRead) continue;
            if (prop.GetIndexParameters().Length > 0) continue; // Пропускаем индексаторы

            try
            {
                var propValue = prop.GetValue(obj);
                if (propValue != null)
                {
                    var copiedValue = DeepCopy(propValue);
                    prop.SetValue(copiedObject, copiedValue);
                }
                else
                {
                    prop.SetValue(copiedObject, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка копирования свойства {prop.Name}: {ex.Message}");
            }
        }

        return copiedObject;
    }

    private static object CreateInstanceWithConstructor(Type type)
    {
        // Ищем конструктор без параметров
        var parameterlessConstructor = type.GetConstructor(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
            null, Type.EmptyTypes, null);

        if (parameterlessConstructor != null)
        {
            return Activator.CreateInstance(type, true);
        }

        // Ищем любой конструктор и создаем с default значениями
        var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (constructors.Length > 0)
        {
            var firstConstructor = constructors[0];
            var parameters = firstConstructor.GetParameters();
            var paramValues = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                paramValues[i] = GetDefaultValue(parameters[i].ParameterType);
            }

            return firstConstructor.Invoke(paramValues);
        }

        throw new InvalidOperationException($"Не удалось создать экземпляр типа {type.Name}");
    }

    private static object GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}