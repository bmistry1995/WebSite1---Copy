using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UpcomingFixtureObject
/// </summary>
public class UpcomingFixtureObject
{
    public string fixtureDate { get; set; }
    public string homeTeam { get; set; }
    public string awayTeam { get; set; }
    public UpcomingFixtureObject(string _date, string _homeTeam, string _awayTeam)
    {
        fixtureDate = _date;
        homeTeam = _homeTeam;
        awayTeam = _awayTeam;
    }
}