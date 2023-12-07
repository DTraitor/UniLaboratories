// Отримання зі значень головної діагоналі двовимірного рядка одновимірного рядка

using Delegates;

Main();

void Main()
{
    string[] array =
    {
        "Change",
        "da",
        "world.",
        "My",
        "final",
        "message.",
        "Goodbye."
    };

    GetDiagonal getDiagonal = delegate(string[] arr)
    {
        string diagonal = "";
        for (int i = 0; i < arr.GetLength(0); i++)
            diagonal += arr[i][arr[i].Length / 2];
        return diagonal;
    };

    // Result: nalynab
    string diagonalArray = getDiagonal(array);
    Console.WriteLine(diagonalArray);

    // Використовуючи створений у п.3 компонент, створити додаток, у якому визначити метод- обробник події для цього компонента, що реалізує реакцію додатка на подію (наприклад, повідомлення користувачеві про виникнення події). Метод-обробник події повинний отримувати інформацію про об’єкт-ініціатор події та аргумент події.**
    Account account = new Account(100, 50);
    account.Notify += () => Console.WriteLine("Account is empty!");
    account.PayTariff();
    account.PayTariff();
    account.PayTariff();
    // Result: Account is empty!
}

delegate string GetDiagonal(string[] array);
