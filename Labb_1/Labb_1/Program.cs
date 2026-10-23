using System.ComponentModel.DataAnnotations;

static string myLineread()
{
    string inputtedString = Console.ReadLine();
    return inputtedString;
       
        }

Console.WriteLine("Enter a string:");
string myString = myLineread();
int strlen = myString.Length;
long total_substring = 0;
for (int i = 0; i < strlen; i++) 
{
    bool isaNumberfirst = char.IsDigit(myString[i]);
    
        if (isaNumberfirst is true) 
    {  

        for (int j = i + 1; j < strlen; j++) 
        {
            if ((myString[i] != myString[j] && char.IsDigit(myString[j]) == true)) //om nästa element är en siffra men inte samma siffra
                continue;
            else if ((char.IsDigit(myString[j]) == false)) //om nästa element inte är en siffra -> skapar ingen substring
            {
                break;
            }
            else if (myString[i] == myString[j])  //om nästa element är samma som startelementet -> färga om substring och printa hela strängen + addera substrängen
            {
                Console.Write(myString[..i]);

                Console.ForegroundColor = ConsoleColor.Red;

                Console.Write(myString[i..(j+1)]);
                total_substring += long.Parse(myString[i..(j + 1)]);

                Console.ResetColor();

                Console.WriteLine(myString[(j + 1)..]);
                break;
            }

        }
            }
    }
Console.WriteLine($"\nThe substring total amounts to {total_substring}");