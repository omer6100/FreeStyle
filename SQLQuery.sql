select * from Album
select * from Artist
select * from EndUser
select * from Review

delete Review
where Id=3006

update Artist
set avgScore = 10
where id =4

update Artist
set AvgScore = 9
where id = 3

update Album
set avgScore =0
where id = 3 or id=1039