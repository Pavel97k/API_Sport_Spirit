using System.Text;
using System.Security.Cryptography;

public class Password_Security
{
    public static string ComputeHash(string plainText)
    {
         // Определите минимальный и максимальный размеры соли.
         int maxSaltSize = 8;

        // Выделите массив байтов, который будет содержать соль.
        byte[] saltBytes = new byte[maxSaltSize];
        for (int i = 0; i < maxSaltSize; i++)
        {
            saltBytes[i] = 1;
        }        

        // Преобразование обычного текста в массив байтов.
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        // Выделите массив, который будет содержать обычный текст и соль.
        byte[] plainTextWithSaltBytes =
                new byte[plainTextBytes.Length + saltBytes.Length];

        // Копирование байтов обычного текста в результирующий массив.
        for (int i = 0; i < plainTextBytes.Length; i++)
            plainTextWithSaltBytes[i] = plainTextBytes[i];

        // Добавление байтов соли к полученному массиву.
        for (int i = 0; i < saltBytes.Length; i++)
            plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

        // Инициализируйте соответствующий класс алгоритма хэширования.
        SHA256 sha256Hash = SHA256.Create();

        // Вычислите хэш-значение нашего обычного текста с добавлением соли.
        byte[] hashBytes = sha256Hash.ComputeHash(plainTextWithSaltBytes);

        // Создайте массив, который будет содержать хэш и исходные байты соли.
        byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

        // Копирование хэш-байтов в результирующий массив.
        for (int i = 0; i < hashBytes.Length; i++)
            hashWithSaltBytes[i] = hashBytes[i];

        // Добавление байтов соли к результату.
        for (int i = 0; i < saltBytes.Length; i++)
            hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

        // Преобразование результата в строку в кодировке base64.
        string hashValue = Convert.ToBase64String(hashWithSaltBytes);

        // Возвратите результат.
        return hashValue;
    }
}