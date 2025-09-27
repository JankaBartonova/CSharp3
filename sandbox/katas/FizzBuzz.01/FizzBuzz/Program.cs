Console.WriteLine("Welcome to FizzBuzz!");
Console.Write("Enter the upper bound: ");

FizzBuzz fizzBuzz = new();
int upperBound = int.Parse(Console.ReadLine());
Console.WriteLine("You entered number: " + upperBound);
Console.WriteLine("----------------------------------");

fizzBuzz.CountTo(upperBound);
