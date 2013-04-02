﻿using System;


class ConvertNumberToWord
{
    public static string GetWord(int n)
    {
        switch (n)
        {
            case 1: return "one";
            case 2: return "two";
            case 3: return "three";
            case 4: return "four";
            case 5: return "five";
            case 6: return "six";
            case 7: return "seven";
            case 8: return "eight";
            case 9: return "nine";
            case 10: return "ten";
            case 11: return "eleven";
            case 12: return "twelve";
            case 13: return "thirteen";
            case 14: return "fourteen";
            case 15: return "fifteen";
            case 16: return "sixteen";
            case 17: return "seventeen";
            case 18: return "eighteen";
            case 19: return "nineteen";
            case 20: return "twenty";
            case 30: return "thirty";
            case 40: return "fourty";
            case 50: return "fifty";
            case 60: return "sixty";
            case 70: return "seventy";
            case 80: return "eighty";
            case 90: return "ninety";
            default: return "";
        }
    }

    static void Main()
    {
        string number = null;
        int n = 234;
        int firstDigit = n % 10;
        int secondDigit = (n % 100) / 10;
        int thirdDigit = n / 100;

        if (thirdDigit > 0 && thirdDigit <= 9)
        {
            number += GetWord(thirdDigit);
            number += " hundred ";
        }

        if (thirdDigit != 0)
        {
            if ((secondDigit == 0 || secondDigit == 1) && n % 100 != 0)
            {
                number += " and ";
            }
        }

        if (secondDigit == 0 || secondDigit == 1)
        {
            number += GetWord(n % 100);
        }
        else
        {
            if (firstDigit == 0)
            {
                number += GetWord(n % 100);
            }
            else
            {
                number += GetWord(((n % 100) / 10) * 10);
                number += " ";
                number += GetWord(n % 10);
            }
        }

        Console.WriteLine(number);
    }
}