public class FizzBuzz
{
    public void CountTo(int lastNumber)
    {
        for (int current = 1; current <= lastNumber; current++)
        {
            if (current % 3 == 0 && current % 5 == 0)
            {
                Console.WriteLine("FizzBuzz");
                continue;
            }

            if (current % 3 == 0)
            {
                Console.WriteLine("Fizz");
                continue;
            }

            if (current % 5 == 0)
            {
                Console.WriteLine("Buzz");
                continue;
            }

            Console.WriteLine(current);
        }
    }
}
