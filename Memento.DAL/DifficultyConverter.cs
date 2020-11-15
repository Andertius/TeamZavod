using System;
using System.Collections.Generic;
using System.Text;

namespace Memento.DAL
{
    static internal class DifficultyConverter
    {
        public static Difficulty ToDifficultyConverter(string stringrepr)
        {
            Difficulty diff;
            diff = stringrepr switch
            {
                "none" => Difficulty.None,
                "beginner" => Difficulty.Beginner,
                "advanced" => Difficulty.Advanced,
                "intermediate" => Difficulty.Intermediate,
                _ => throw new NotSupportedException("Not valid difficulty level")
            };

            return diff;
        }

        public static string ToStringConverter(Difficulty diff)
        {
            string stringrepr;

            stringrepr = diff switch
            {
                Difficulty.None => "none",
                Difficulty.Beginner => "beginner",
                Difficulty.Intermediate => "intermediate",
                Difficulty.Advanced => "advanced",
                _ => throw new NotSupportedException("Not valid difficulty level"),
            };

            return stringrepr;
        }

    }
}
