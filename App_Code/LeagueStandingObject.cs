using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LeagueStandingObject
/// </summary>
public class LeagueStandingObject
{
    public int position { get; set; }
    public string teamName { get; set; }
    public int gamesPlayed { get; set; }
    public int wins { get; set; }
    public int draws { get; set; }
    public int losses { get; set; }
    public int GF { get; set; }
    public int GA { get; set; }
    public int GD { get; set; }
    public int points { get; set; }
    public LeagueStandingObject(int _position, string _team, int _played, int _wins, int _draws, int _losses, int _GF, int _GA, int _GD, int _points)
    {
        position = _position;
        teamName = _team;
        gamesPlayed = _played;
        wins = _wins;
        draws = _draws;
        losses = _losses;
        GF = _GF;
        GA = _GA;
        GD = _GD;
        points = _points;
    }
}