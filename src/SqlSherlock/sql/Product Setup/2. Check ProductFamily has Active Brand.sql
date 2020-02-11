-- Is the ProductFamily connected to an active brand?
declare @ProductFamilyCode nvarchar(10)
select 
pf.ID as ProductFamilyID,
pf.Name,
pf.Code, 
b.Name as BrandName,
b.Code as BrandCode
from ProductFamily pf
join Brand b
on pf.BrandId = b.Id
and pf.Active = 1
and b.Active = 1
and GETDATE() > pf.PublishedFrom
and GETDATE() < pf.PublishedTo
and pf.Code = @ProductFamilyCode
