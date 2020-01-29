--
-- Do Things have Statuses?
--
DECLARE @thingId int
select *
from MyOtherTable
where ThingId = @thingId
and Status is not null

