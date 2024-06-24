using System.Text;

namespace Epic_2._1___Hash_function_for_passwords;
public class Program
{
    static void Main(string[] args)
    {
        string input = "SuperPassword";
        byte[] inputData = Encoding.UTF8.GetBytes(input);
        byte[] hash = SHA256.ComputeHash(inputData);

        Console.WriteLine(BitConverter.ToString(hash).Replace("-", "").ToLower());
    }
}