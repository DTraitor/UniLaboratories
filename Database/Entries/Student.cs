﻿using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Database.Entries.Abilities;

namespace Database.Entries;

public class Student : Entry
{
    [DatabaseAbilityFunction("Study")]
    public string StudyAbility() => Study.Study();
    [DatabaseAbilityVariable("Study")]
    public StudyAbility Study { get; set; }
    [DatabaseVariable("Sex")]
    public string Sex { get; set; }
    [DatabaseVariable("Place of residence")]
    public string Residence { get; set; }
    [DatabaseVariable("Student Card ID")]
    public string StudentCardId 
    {
        get => studentCardId;
        set
        {
            if(!Regex.IsMatch(value, "^КВ-\\d{8}$"))
                throw new ArgumentException("Student ID Card value should be between КВ-00000000 and КВ-99999999.");
            studentCardId = value;
        }  
    }
    [DatabaseVariable("Grade Book")]
    public string GradeBook 
    {
        get => gradeBook;
        set
        {
            if(!Regex.IsMatch(value, "^\\d{8}$"))
                throw new ArgumentException("Grade Book value should be between 00000000 and 99999999.");
            gradeBook = value;
        }  
    }

    [DatabaseVariable("Year of study")]
    public string YearOfStudy
    {
        get => yearOfStudy;
        set
        {
            if(!Regex.IsMatch(value, "^(1|2|3|4|5|6)$"))
                throw new ArgumentException("Year of study should be between 1 and 6.");
            yearOfStudy = value;
        }
    }

    [JsonIgnore]
    private string studentCardId;
    [JsonIgnore]
    private string gradeBook;
    [JsonIgnore]
    private string yearOfStudy;
}