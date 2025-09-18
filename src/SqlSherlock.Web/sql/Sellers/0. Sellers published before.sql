-- Sellers published before the given date

declare @PublishedBefore datetime2

select *
from Seller
where PublishedFrom < @PublishedBefore
