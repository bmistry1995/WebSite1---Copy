using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FixtureObject
/// </summary>
public class FixtureObject
{
    public string fixtureDate { get; set; }
    public string homeTeam { get; set; }
    public int homeGoalsFT { get; set; }
    public int awayGoalsFT { get; set; }
    public string awayTeam { get; set; }

    public FixtureObject(string _date, string _homeTeam, int _homeGoalsFT, int _awayGoalsFT, string _awayTeam)
    {
        fixtureDate = _date;
        homeTeam = _homeTeam;
        homeGoalsFT = _homeGoalsFT;
        awayGoalsFT = _awayGoalsFT;
        awayTeam = _awayTeam;
    }
}