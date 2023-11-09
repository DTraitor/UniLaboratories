using System.Collections;
using System.ComponentModel;
using Console;

CustomStringClass temp;

List<CustomStringClass> stringsList = new List<CustomStringClass>();
stringsList.Add(new CustomStringClass("wow", 3));
stringsList.Add(new CustomStringClass("this", 2));
stringsList.Add(new CustomStringClass("is", -2));
stringsList.Add(new CustomStringClass("funny!", -5));
stringsList.FirstOrDefault(s => s.Value == "wow")?.Encrypt();
stringsList.Remove(stringsList.FirstOrDefault(s => s.Value == "is"));
System.Console.WriteLine("\nList<T>:");
foreach (var st in stringsList)
{
    System.Console.WriteLine(st.Value);
}


ArrayList stringsArrayList = new ArrayList();
stringsArrayList.Add(new CustomStringClass("wow", 3));
stringsArrayList.Add(new CustomStringClass("this", 2));
stringsArrayList.Add(new CustomStringClass("is", -2));
stringsArrayList.Add(new CustomStringClass("funny!", -5));
stringsArrayList.OfType<CustomStringClass>().FirstOrDefault(s => s.Value == "wow")?.Encrypt();
stringsArrayList.Remove(stringsArrayList.OfType<CustomStringClass>().FirstOrDefault(s => s.Value == "is"));
System.Console.WriteLine("\nArrayList<T>:");
foreach (var st in stringsArrayList.OfType<CustomStringClass>())
{
    System.Console.WriteLine(st.Value);
}


CustomStringClass[] array = new CustomStringClass[4];
array[0] = new CustomStringClass("wow", 3);
array[1] = new CustomStringClass("this", 2);
array[2] = new CustomStringClass("is", -2);
array[3] = new CustomStringClass("funny!", -5);
array[0].Encrypt();
array[2] = null;
System.Console.WriteLine("\nSimple array:");
foreach (CustomStringClass? st in array)
{
    if(st == null)
        continue;
    System.Console.WriteLine(st.Value);
}

BinaryTree<CustomStringClass> tree = new BinaryTree<CustomStringClass>();
tree.Insert(new CustomStringClass("wow", -3));
tree.Insert(new CustomStringClass("this", 2));
tree.Insert(new CustomStringClass("is", 4));
tree.Insert(new CustomStringClass("funny", -5));
System.Console.WriteLine("\nBinary tree iteration:");
foreach (CustomStringClass obj in tree)
{
    System.Console.WriteLine(obj.Value);
}
