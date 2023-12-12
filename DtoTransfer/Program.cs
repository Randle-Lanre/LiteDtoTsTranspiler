// See https://aka.ms/new-console-template for more information

using System.Reflection;

namespace DtoTransfer;

internal class Program
{

    
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var assembly = Assembly.GetExecutingAssembly();
        var dtoClasses = assembly.GetTypes().Where(type => type.Name.EndsWith("Dto")).ToList();


        foreach (var dto in dtoClasses)
        {
            Console.WriteLine("class Name ---> {0}",dto.Name);

            var propties = dto.GetProperties().ToList();

            foreach (PropertyInfo item in propties)
            {
                Console.WriteLine("PropertyName --> {0}, PropertyType --> {1}",item.Name, item.PropertyType);
                
            }

        }

    }
    
}