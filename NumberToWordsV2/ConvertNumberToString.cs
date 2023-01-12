using System;
using System.Collections.Generic;
using System.Speech.Recognition;

public class ConvertNumberToString
{
    private readonly string[] _ones = { "zero", "one", "two", "three", "four", "five", 
                                        "six", "seven", "eight", "nine" };

    private readonly string[] _teens = { "ten", "eleven", "twelve", "thirteen", "fourteen", 
                                        "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };

    private readonly string[] _tens = { "", "", "twenty", "thirty", "forty", "fifty", "sixty", 
                                        "seventy", "eighty", "ninety" };

    private readonly string[] _scale = { "", "thousand", "million", "billion", "trillion", "quadrillion", 
                                        "quintillion", "sextillion", "septillion", "octillion", 
                                        "nonillion","decillion","undecillion","duodecillion",
                                        "tredecillion","quattuordecillion","quindecillion"};


    public string Convert(Int128 number)
    {
        if (number == 0)
            return _ones[0];

        var elements = new List<string>();
        int i = 0;
        do
        {
            int n = (int)(number % 1000);

            if (n != 0)
            {
                elements.Insert(0, string.Format("{0} {1}", ConvertAScaleSection((int)n), _scale[i]));
            }
            number = (number / 1000);
            i++;

        } while (number > 0);

        return string.Join(", ", elements);
    }

    private string ConvertAScaleSection(int number)
    {
        var temp = "";

        int numMod100 = (int)(number % 100);

        if (numMod100 < 10)
            temp = _ones[numMod100 % 10];
        else if (number % 100 < 20)
            temp = _teens[numMod100 - 10];
        else
        {
            temp = _tens[numMod100 / 10];
            if ((numMod100) % 10 > 0)
                temp += "-" + _ones[numMod100 % 10];
        }

        int numDiv100 = (int)(number / 100);
        if (numMod100 == 0)
            temp = _ones[numDiv100] + " hundred ";

        else if (number > 99)
            temp = _ones[numDiv100] + " hundred " + temp;

        return temp;
    }


    public string FormatNumbersWithCommas(string num)
    {
        Int128 n = Int128.Parse(num);
        return string.Format("{0:n0}", n);
    }

    public string OnlyNumbers(string num)
    {
        if (num == null) return ("nothing to convert");
        if (num.Length == 0) return ("nothing to convert");
        if(char.IsDigit(num[0])==false) return ("must be a whole number");

        string newstr = "";
        int startLocation = 0;

        for (int i = startLocation; i < num.Length; i++)
        {
            if (char.IsAsciiDigit(num[i]))
            {
                newstr += num[i];
            }
        }

        return (newstr);
    }

    
}
