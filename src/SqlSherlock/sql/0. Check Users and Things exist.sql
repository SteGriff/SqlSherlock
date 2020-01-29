--
-- Are users and things configured?
--
DECLARE @userId int
DECLARE @thingId int
select *
from MyTable
where UserId = @userId
and ThingId = @thingId
