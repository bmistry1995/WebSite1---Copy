create table tblTeams(
	teamName nvarchar(1024) not null,
	leagueID int not null,
	shortName nvarchar(1024),
	crestURL nvarchar(max),
	playersLink nvarchar(max)
);