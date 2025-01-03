﻿using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCA.CodingTracker;
internal static class Validation
{
    internal static string ParseDate(DateTime date)
    {
        return "";
    }

    internal static int ValidateUserIntInput(string maybeNumber, out string errorMessage, string typeOfDateUnit ="")
    {
        //validateUserIntInput = 
        // they input in GetUserInput(voiceMode) method in UserInterface
        // send the anything-input to ValidateIntInput()
        //  if errorMessage != "", then input was valid for the type of input entered
        //     and string maybeNumber should be now assigned to the int realNumberValue, which is returned to the user.
        //        if return value == -1, then either input string wasn't a number, or some other problem occured in the method.

        int realNumberValue = -1;
        errorMessage = "";
        typeOfDateUnit = typeOfDateUnit.Trim().ToLower();

        Dictionary<string, Func<int,string>> validationForDateTime = new Dictionary<string, Func<int, string>>
        {
            {"year", num => num>1500 && num<=2500 ? "" : "Wow! You must live in another age. We can only do years between 1000 and 3000. Please try again." },
            {"month", num=> num >= 0 && num <= 12 ? "" : "Woah! I don't know what cool calendar you're on, but ours only has months between 1 and 12." },
            {"day", num => num >=1 && num <=31 ? "" : "Woah! I don't know which cool time system you're on, but our months only have days between 1 and 31. Please try again."},
            {"hour", num => num >=0 && num <=23 ? "" : "Wow! I would love to live in a world with that many hours in the day. Our clock only goes from 0 to 23 hours." },
            {"minute", num => num >=0 && num <= 59 ? "" : "Wow! That looks like a cool time system, but our clock only includes minutes from 0 to 59." },
            {"second", num => num >= 0 && num <= 59 ? "" : "Wow! That looks like a cool time system, but our clock only includes seconds from 0 to 59."}
        };

        if (int.TryParse(maybeNumber, out realNumberValue))
        {
            errorMessage = validationForDateTime[typeOfDateUnit](realNumberValue);   
        }
        else
        {
            errorMessage = "I'm sorry, but I didn't understand that number. Please try again.";
        }
        return realNumberValue;
    }
}

    
