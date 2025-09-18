-- Brands published before the given date

declare @PublishedBefore datetime2

select *
from Brand
where PublishedFrom < @PublishedBefore
