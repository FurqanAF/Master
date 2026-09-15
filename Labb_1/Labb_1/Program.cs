using System.ComponentModel.DataAnnotations;

static string myLineread()
{
    string inputtedString = Console.ReadLine();
    return inputtedString;
       
        }

Console.WriteLine("Enter a string:");
string myString = myLineread();
int strlen = myString.Length;

for (int i = 0; i < strlen; i++) 
{
    bool isaNumberfirst = char.IsDigit(myString[i]);
    
        if (isaNumberfirst is true) 
    {  // int matchnumber = myString[i] - '0';
        char checkNumberfirst = myString[i];

        for (int j = i + 1; j < strlen - 1; j++) 
        {
            if ((myString[i] != myString[j] && char.IsDigit(myString[j]) == true))
                continue;
            else if ((char.IsDigit(myString[j]) == false))
            {
                break;
            }
            else if (myString[i] == myString[j]) 
            {
                Console.Write(myString[..i]);

                Console.ForegroundColor = ConsoleColor.Red;

                Console.Write(myString[i..(j+1)]);

                Console.ResetColor();

                Console.WriteLine(myString[(j + 1)..]);
                break;
            }

        }
            }
    }